using FlightStreamDeck.Logics;
using Microsoft.Extensions.Logging;
using Microsoft.FlightSimulator.SimConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace FlightStreamDeck.SimConnectFSX;

public class SimConnectFlightConnector : IFlightConnector
{
    public event EventHandler<AircraftStatusUpdatedEventArgs>? AircraftStatusUpdated;
    public event EventHandler<ToggleValueUpdatedEventArgs>? GenericValuesUpdated;
    public event EventHandler<InvalidEventRegisteredEventArgs>? InvalidEventRegistered;
    public event EventHandler? Closed;

    // Extra SimConnect functions via native pointer
    IntPtr hSimConnect;
    [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern int /* HRESULT */ SimConnect_GetLastSentPacketID(IntPtr hSimConnect, out uint /* DWORD */ dwSendID);
    
    [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern int /* HRESULT */ SimConnect_RequestResponseTimes(IntPtr hSimConnect, uint /* DWORD */ nCount, ref float /* FLOAT32 */ fElapsedSeconds);
    
    [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern int /* HRESULT */ SimConnect_InsertString(IntPtr pDest, uint /* DWORD */ cbDest, IntPtr pSrc, uint /* DWORD */ pcbStringV, uint /* DWORD */ pcbString);
    
    [DllImport("SimConnect.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
    private static extern int /* HRESULT */ SimConnect_RetrieveString(IntPtr pDest, uint /* DWORD */ cbDest, IntPtr pSrc, ref uint /* DWORD */ pcbStringV, ref uint /* DWORD */ pcbString);

    private const int StatusDelayMilliseconds = 100;

    /// <summary>
    /// This is a reference counter to make sure we do not deregister variables that are still in use.
    /// </summary>
    private readonly Dictionary<SimVarRegistration, int> genericValues = new();

    private readonly object lockLists = new object();

    // User-defined win32 event
    const int WM_USER_SIMCONNECT = 0x0402;
    private readonly ILogger<SimConnectFlightConnector> logger;

    public IntPtr Handle { get; private set; }

    private SimConnect? simconnect = null;
    private CancellationTokenSource? cts = null;

    public SimConnectFlightConnector(ILogger<SimConnectFlightConnector> logger)
    {
        this.logger = logger;
    }

    // Simconnect client will send a win32 message when there is
    // a packet to process. ReceiveMessage must be called to
    // trigger the events. This model keeps simconnect processing on the main thread.
    public IntPtr HandleSimConnectEvents(int message, ref bool isHandled)
    {
        isHandled = false;

        switch (message)
        {
            case WM_USER_SIMCONNECT:
                {
                    if (simconnect != null)
                    {
                        try
                        {
                            this.simconnect.ReceiveMessage();
                        }
                        catch (Exception ex)
                        {
                            RecoverFromError(ex);
                        }

                        isHandled = true;
                    }
                }
                break;

            default:
                break;
        }

        return IntPtr.Zero;
    }

    // Set up the SimConnect event handlers
    public void Initialize(IntPtr Handle)
    {
        if (simconnect != null)
        {
            logger.LogWarning("Initialization is already done. Cancelled this request.");
            return;
        }
        simconnect = new SimConnect("Flight Tracker Stream Deck", Handle, WM_USER_SIMCONNECT, null, 0);

        // Get direct access to the SimConnect handle, to use functions otherwise not supported.
        var fiSimConnectValue = typeof(SimConnect).GetField("hSimConnect", BindingFlags.NonPublic | BindingFlags.Instance)?.GetValue(simconnect);
        if (fiSimConnectValue == null)
        {
            throw new InvalidOperationException("hSimConnect is not available!");
        }
        hSimConnect = (IntPtr)fiSimConnectValue;

        // listen to connect and quit msgs
        simconnect.OnRecvOpen += new SimConnect.RecvOpenEventHandler(Simconnect_OnRecvOpen);
        simconnect.OnRecvQuit += new SimConnect.RecvQuitEventHandler(Simconnect_OnRecvQuit);

        // listen to exceptions
        simconnect.OnRecvException += Simconnect_OnRecvException;

        simconnect.OnRecvSimobjectDataBytype += Simconnect_OnRecvSimobjectDataBytypeAsync;
        simconnect.OnRecvSystemState += Simconnect_OnRecvSystemState;

        RegisterFlightStatusDefinition(simconnect);

        simconnect.MapClientEventToSimEvent(EVENTS.AUTOPILOT_ON, "AUTOPILOT_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.AUTOPILOT_OFF, "AUTOPILOT_OFF");
        simconnect.MapClientEventToSimEvent(EVENTS.AUTOPILOT_TOGGLE, "AP_MASTER");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_HDG_TOGGLE, "AP_PANEL_HEADING_HOLD");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_NAV_TOGGLE, "AP_NAV1_HOLD");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_APR_TOGGLE, "AP_APR_HOLD");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_ALT_TOGGLE, "AP_PANEL_ALTITUDE_HOLD");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_VS_TOGGLE, "AP_VS_HOLD");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_FLC_ON, "FLIGHT_LEVEL_CHANGE_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_FLC_OFF, "FLIGHT_LEVEL_CHANGE_OFF");

        simconnect.MapClientEventToSimEvent(EVENTS.AP_HDG_SET, "HEADING_BUG_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_HDG_INC, "HEADING_BUG_INC");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_HDG_DEC, "HEADING_BUG_DEC");

        simconnect.MapClientEventToSimEvent(EVENTS.AP_ALT_SET, "AP_ALT_VAR_SET_ENGLISH");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_ALT_INC, "AP_ALT_VAR_INC");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_ALT_DEC, "AP_ALT_VAR_DEC");

        simconnect.MapClientEventToSimEvent(EVENTS.AP_VS_SET, "AP_VS_VAR_SET_ENGLISH");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_VS_INC, "AP_VS_VAR_INC");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_VS_DEC, "AP_VS_VAR_DEC");

        simconnect.MapClientEventToSimEvent(EVENTS.AP_AIRSPEED_SET, "AP_SPD_VAR_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_AIRSPEED_INC, "AP_SPD_VAR_INC");
        simconnect.MapClientEventToSimEvent(EVENTS.AP_AIRSPEED_DEC, "AP_SPD_VAR_DEC");

        simconnect.MapClientEventToSimEvent(EVENTS.QNH_SET, "KOHLSMAN_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.QNH_INC, "KOHLSMAN_INC");
        simconnect.MapClientEventToSimEvent(EVENTS.QNH_DEC, "KOHLSMAN_DEC");

        simconnect.MapClientEventToSimEvent(EVENTS.AVIONICS_TOGGLE, "AVIONICS_MASTER_SET");

        // Add all the new event mappings for comprehensive SimConnect support
        simconnect.MapClientEventToSimEvent(EVENTS.AVIONICS_MASTER_1_ON, "AVIONICS_MASTER_1_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.AVIONICS_MASTER_1_OFF, "AVIONICS_MASTER_1_OFF");
        simconnect.MapClientEventToSimEvent(EVENTS.AVIONICS_MASTER_2_ON, "AVIONICS_MASTER_2_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.AVIONICS_MASTER_2_OFF, "AVIONICS_MASTER_2_OFF");
        
        // Engine Controls
        simconnect.MapClientEventToSimEvent(EVENTS.ENGINE_AUTO_START, "ENGINE_AUTO_START");
        simconnect.MapClientEventToSimEvent(EVENTS.ENGINE_AUTO_SHUTDOWN, "ENGINE_AUTO_SHUTDOWN");
        
        // Throttle Controls
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_FULL, "THROTTLE_FULL");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_INCR, "THROTTLE_INCR");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_INCR_SMALL, "THROTTLE_INCR_SMALL");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_DECR, "THROTTLE_DECR");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_DECR_SMALL, "THROTTLE_DECR_SMALL");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_CUT, "THROTTLE_CUT");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE_SET, "THROTTLE_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE1_SET, "THROTTLE1_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE2_SET, "THROTTLE2_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE3_SET, "THROTTLE3_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.THROTTLE4_SET, "THROTTLE4_SET");
        
        // Propeller Controls
        simconnect.MapClientEventToSimEvent(EVENTS.PROP_PITCH_INCR, "PROP_PITCH_INCR");
        simconnect.MapClientEventToSimEvent(EVENTS.PROP_PITCH_DECR, "PROP_PITCH_DECR");
        simconnect.MapClientEventToSimEvent(EVENTS.PROP_PITCH_HI, "PROP_PITCH_HI");
        simconnect.MapClientEventToSimEvent(EVENTS.PROP_PITCH_LO, "PROP_PITCH_LO");
        simconnect.MapClientEventToSimEvent(EVENTS.PROP_PITCH_SET, "PROP_PITCH_SET");
        
        // Mixture Controls
        simconnect.MapClientEventToSimEvent(EVENTS.MIXTURE_RICH, "MIXTURE_RICH");
        simconnect.MapClientEventToSimEvent(EVENTS.MIXTURE_LEAN, "MIXTURE_LEAN");
        simconnect.MapClientEventToSimEvent(EVENTS.MIXTURE_INCR, "MIXTURE_INCR");
        simconnect.MapClientEventToSimEvent(EVENTS.MIXTURE_DECR, "MIXTURE_DECR");
        simconnect.MapClientEventToSimEvent(EVENTS.MIXTURE_SET, "MIXTURE_SET");
        
        // Magneto Controls
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_OFF, "MAGNETO_OFF");
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_RIGHT, "MAGNETO_RIGHT");
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_LEFT, "MAGNETO_LEFT");
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_BOTH, "MAGNETO_BOTH");
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_START, "MAGNETO_START");
        simconnect.MapClientEventToSimEvent(EVENTS.MAGNETO_SET, "MAGNETO_SET");
        
        // Trim Controls
        simconnect.MapClientEventToSimEvent(EVENTS.ELEV_TRIM_UP, "ELEV_TRIM_UP");
        simconnect.MapClientEventToSimEvent(EVENTS.ELEV_TRIM_DN, "ELEV_TRIM_DN");
        simconnect.MapClientEventToSimEvent(EVENTS.ELEV_TRIM_SET, "ELEV_TRIM_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.AILERON_TRIM_LEFT, "AILERON_TRIM_LEFT");
        simconnect.MapClientEventToSimEvent(EVENTS.AILERON_TRIM_RIGHT, "AILERON_TRIM_RIGHT");
        simconnect.MapClientEventToSimEvent(EVENTS.RUDDER_TRIM_LEFT, "RUDDER_TRIM_LEFT");
        simconnect.MapClientEventToSimEvent(EVENTS.RUDDER_TRIM_RIGHT, "RUDDER_TRIM_RIGHT");
        
        // Flight Controls
        simconnect.MapClientEventToSimEvent(EVENTS.FLAPS_UP, "FLAPS_UP");
        simconnect.MapClientEventToSimEvent(EVENTS.FLAPS_DOWN, "FLAPS_DOWN");
        simconnect.MapClientEventToSimEvent(EVENTS.FLAPS_SET, "FLAPS_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.GEAR_TOGGLE, "GEAR_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.GEAR_UP, "GEAR_UP");
        simconnect.MapClientEventToSimEvent(EVENTS.GEAR_DOWN, "GEAR_DOWN");
        simconnect.MapClientEventToSimEvent(EVENTS.SPOILERS_TOGGLE, "SPOILERS_TOGGLE");
        
        // Brakes
        simconnect.MapClientEventToSimEvent(EVENTS.BRAKES, "BRAKES");
        simconnect.MapClientEventToSimEvent(EVENTS.BRAKES_LEFT, "BRAKES_LEFT");
        simconnect.MapClientEventToSimEvent(EVENTS.BRAKES_RIGHT, "BRAKES_RIGHT");
        simconnect.MapClientEventToSimEvent(EVENTS.PARKING_BRAKES, "PARKING_BRAKES");
        
        // Lights
        simconnect.MapClientEventToSimEvent(EVENTS.LANDING_LIGHTS_TOGGLE, "LANDING_LIGHTS_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.LANDING_LIGHTS_ON, "LANDING_LIGHTS_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.LANDING_LIGHTS_OFF, "LANDING_LIGHTS_OFF");
        simconnect.MapClientEventToSimEvent(EVENTS.TAXI_LIGHTS_TOGGLE, "TAXI_LIGHTS_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV_LIGHTS_TOGGLE, "NAV_LIGHTS_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.BEACON_LIGHTS_TOGGLE, "BEACON_LIGHTS_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.STROBE_LIGHTS_TOGGLE, "STROBES_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.PANEL_LIGHTS_TOGGLE, "PANEL_LIGHTS_TOGGLE");
        
        // Radio Controls
        simconnect.MapClientEventToSimEvent(EVENTS.COM1_RADIO_SET, "COM_RADIO_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.COM_STBY_RADIO_SWAP, "COM_STBY_RADIO_SWAP");
        simconnect.MapClientEventToSimEvent(EVENTS.COM2_RADIO_SET, "COM2_RADIO_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.COM2_STBY_RADIO_SWAP, "COM2_STBY_RADIO_SWAP");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV1_RADIO_SET, "NAV1_RADIO_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV1_STBY_RADIO_SWAP, "NAV1_STBY_RADIO_SWAP");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV1_OBS_SET, "VOR1_OBI_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV2_RADIO_SET, "NAV2_RADIO_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV2_STBY_RADIO_SWAP, "NAV2_STBY_RADIO_SWAP");
        simconnect.MapClientEventToSimEvent(EVENTS.NAV2_OBS_SET, "VOR2_OBI_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.XPNDR_SET, "XPNDR_SET");
        simconnect.MapClientEventToSimEvent(EVENTS.ADF_COMPLETE_SET, "ADF_COMPLETE_SET");
        
        // Fuel System
        simconnect.MapClientEventToSimEvent(EVENTS.FUEL_PUMP, "FUEL_PUMP");
        simconnect.MapClientEventToSimEvent(EVENTS.TOGGLE_FUEL_VALVE_ENG1, "TOGGLE_FUEL_VALVE_ENG1");
        simconnect.MapClientEventToSimEvent(EVENTS.TOGGLE_FUEL_VALVE_ENG2, "TOGGLE_FUEL_VALVE_ENG2");
        simconnect.MapClientEventToSimEvent(EVENTS.FUEL_SELECTOR_SET, "FUEL_SELECTOR_SET");
        
        // Electrical System
        simconnect.MapClientEventToSimEvent(EVENTS.TOGGLE_MASTER_BATTERY, "TOGGLE_MASTER_BATTERY");
        simconnect.MapClientEventToSimEvent(EVENTS.TOGGLE_MASTER_ALTERNATOR, "TOGGLE_MASTER_ALTERNATOR");
        simconnect.MapClientEventToSimEvent(EVENTS.MASTER_BATTERY_ON, "MASTER_BATTERY_ON");
        simconnect.MapClientEventToSimEvent(EVENTS.MASTER_BATTERY_OFF, "MASTER_BATTERY_OFF");
        
        // Anti-Ice
        simconnect.MapClientEventToSimEvent(EVENTS.PITOT_HEAT_TOGGLE, "PITOT_HEAT_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.TOGGLE_STRUCTURAL_DEICE, "TOGGLE_STRUCTURAL_DEICE");
        
        // Simulation
        simconnect.MapClientEventToSimEvent(EVENTS.PAUSE_TOGGLE, "PAUSE_TOGGLE");
        simconnect.MapClientEventToSimEvent(EVENTS.SIM_RATE_SET, "SIM_RATE_SET");

        isGenericValueRegistered = false;
        RegisterGenericValues();
    }

    public void Send(string message)
    {
        simconnect?.Text(SIMCONNECT_TEXT_TYPE.PRINT_BLACK, 3, EVENTS.MESSAGE_RECEIVED, message);
    }

    public void ApOn()
    {
        SendCommand(EVENTS.AUTOPILOT_ON);
    }

    public void ApOff()
    {
        SendCommand(EVENTS.AUTOPILOT_OFF);
    }

    public void ApToggle()
    {
        SendCommand(EVENTS.AUTOPILOT_TOGGLE);
    }

    public void ApHdgToggle()
    {
        SendCommand(EVENTS.AP_HDG_TOGGLE);
    }

    public void ApNavToggle()
    {
        SendCommand(EVENTS.AP_NAV_TOGGLE);
    }

    public void ApAprToggle()
    {
        SendCommand(EVENTS.AP_APR_TOGGLE);
    }

    public void ApAltToggle()
    {
        SendCommand(EVENTS.AP_ALT_TOGGLE);
    }

    public void ApVsToggle()
    {
        SendCommand(EVENTS.AP_VS_TOGGLE);
    }

    public void ApFlcOn()
    {
        SendCommand(EVENTS.AP_FLC_ON);
    }

    public void ApFlcOff()
    {
        SendCommand(EVENTS.AP_FLC_OFF);
    }

    public void ApHdgSet(uint heading)
    {
        SendCommand(EVENTS.AP_HDG_SET, heading);
    }

    public void ApHdgInc()
    {
        SendCommand(EVENTS.AP_HDG_INC);
    }

    public void ApHdgDec()
    {
        SendCommand(EVENTS.AP_HDG_DEC);
    }

    public void ApAltSet(uint altitude)
    {
        SendCommand(EVENTS.AP_ALT_SET, altitude);
    }

    public void ApAltInc()
    {
        SendCommand(EVENTS.AP_ALT_INC);
    }

    public void ApAltDec()
    {
        SendCommand(EVENTS.AP_ALT_DEC);
    }

    public void ApVsSet(int speed)
    {
        SendCommand(EVENTS.AP_VS_SET, unchecked((uint)speed));
    }

    public void ApAirSpeedSet(uint speed)
    {
        SendCommand(EVENTS.AP_AIRSPEED_SET, speed);
    }

    public void ApAirSpeedInc()
    {
        SendCommand(EVENTS.AP_AIRSPEED_INC);
    }

    public void ApAirSpeedDec()
    {
        SendCommand(EVENTS.AP_AIRSPEED_DEC);
    }

    public void QNHSet(uint qnh)
    {
        SendCommand(EVENTS.QNH_SET, qnh);
    }

    public void QNHInc()
    {
        SendCommand(EVENTS.QNH_INC);
    }

    public void QNHDec()
    {
        SendCommand(EVENTS.QNH_DEC);
    }


    public void AvMasterToggle(uint state)
    {
        SendCommand(EVENTS.AVIONICS_TOGGLE, state);
    }

    private void SendCommand(EVENTS sendingEvent, uint data = 0)
    {
        try
        {
            simconnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, sendingEvent, data, GROUPID.MAX, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }
        catch (COMException ex) when (ex.Message == "0xC00000B0")
        {
            RecoverFromError(ex);
        }
    }

    private void SendGenericCommand(Enum sendingEvent, uint dwData = 0)
    {
        try
        {
            simconnect?.TransmitClientEvent(SimConnect.SIMCONNECT_OBJECT_ID_USER, sendingEvent, dwData, GROUPID.MAX, SIMCONNECT_EVENT_FLAG.GROUPID_IS_PRIORITY);
        }
        catch (COMException ex) when (ex.Message == "0xC00000B0")
        {
            RecoverFromError(ex);
        }
    }

    public void CloseConnection()
    {
        try
        {
            logger.LogDebug("Trying to cancel request loop");
            cts?.Cancel();
            cts = null;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Cannot cancel request loop! Error: {ex.Message}");
        }
        try
        {
            // Dispose serves the same purpose as SimConnect_Close()
            simconnect?.Dispose();
            simconnect = null;
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, $"Cannot unsubscribe events! Error: {ex.Message}");
        }
    }

    private void RegisterFlightStatusDefinition(SimConnect simconnect)
    {
        void AddToFlightStatusDefinition(string simvar, string unit, SIMCONNECT_DATATYPE type)
        {
            simconnect.AddToDataDefinition(DEFINITIONS.FlightStatus, simvar, unit, type, 0.0f, SimConnect.SIMCONNECT_UNUSED);
        }

        AddToFlightStatusDefinition("SIMULATION RATE", "number", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("PLANE LATITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE LONGITUDE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE ALTITUDE", "Feet", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE ALT ABOVE GROUND", "Feet", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE PITCH DEGREES", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE BANK DEGREES", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE HEADING DEGREES TRUE", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("PLANE HEADING DEGREES MAGNETIC", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("GROUND ALTITUDE", "Meters", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("GROUND VELOCITY", "Knots", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("AIRSPEED INDICATED", "Knots", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("VERTICAL SPEED", "Feet per minute", SIMCONNECT_DATATYPE.FLOAT64);

        AddToFlightStatusDefinition("FUEL TOTAL QUANTITY", "Gallons", SIMCONNECT_DATATYPE.FLOAT64);

        AddToFlightStatusDefinition("AMBIENT WIND VELOCITY", "Feet per second", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("AMBIENT WIND DIRECTION", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);

        AddToFlightStatusDefinition("SIM ON GROUND", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("STALL WARNING", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("OVERSPEED WARNING", "number", SIMCONNECT_DATATYPE.INT32);

        #region Autopilot

        AddToFlightStatusDefinition("AUTOPILOT MASTER", "number", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT HEADING LOCK", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("AUTOPILOT HEADING LOCK DIR", "Degrees", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT NAV1 LOCK", "number", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT APPROACH HOLD", "number", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT ALTITUDE LOCK", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("AUTOPILOT ALTITUDE LOCK VAR", "Feet", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT VERTICAL HOLD", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("AUTOPILOT VERTICAL HOLD VAR", "Feet per minute", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("AUTOPILOT FLIGHT LEVEL CHANGE", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("AUTOPILOT AIRSPEED HOLD VAR", "Knots", SIMCONNECT_DATATYPE.INT32);

        #endregion

        AddToFlightStatusDefinition("KOHLSMAN SETTING MB", "number", SIMCONNECT_DATATYPE.INT32);

        AddToFlightStatusDefinition("TRANSPONDER CODE:1", "Hz", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("COM ACTIVE FREQUENCY:1", "kHz", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("COM ACTIVE FREQUENCY:2", "kHz", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("AVIONICS MASTER SWITCH", "number", SIMCONNECT_DATATYPE.INT32);
        AddToFlightStatusDefinition("NAV OBS:1", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("NAV OBS:2", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);
        AddToFlightStatusDefinition("ADF CARD", "Degrees", SIMCONNECT_DATATYPE.FLOAT64);

        // IMPORTANT: register it with the simconnect managed wrapper marshaller
        // if you skip this step, you will only receive a uint in the .dwData field.
        simconnect.RegisterDataDefineStruct<FlightStatusStruct>(DEFINITIONS.FlightStatus);
    }

    private void Simconnect_OnRecvSimobjectDataBytypeAsync(SimConnect sender, SIMCONNECT_RECV_SIMOBJECT_DATA_BYTYPE data)
    {
        // Must be general SimObject information
        switch (data.dwRequestID)
        {
            case (uint)DATA_REQUESTS.FLIGHT_STATUS:
                {
                    var flightStatus = data.dwData[0] as FlightStatusStruct?;

                    if (flightStatus.HasValue)
                    {
                        logger.LogTrace("Get Aircraft status");
                        AircraftStatusUpdated?.Invoke(this, new AircraftStatusUpdatedEventArgs(
                            new AircraftStatus
                            {
                                //SimTime = flightStatus.Value.SimTime,
                                //SimRate = flightStatus.Value.SimRate,
                                Latitude = flightStatus.Value.Latitude,
                                Longitude = flightStatus.Value.Longitude,
                                Altitude = flightStatus.Value.Altitude,
                                AltitudeAboveGround = flightStatus.Value.AltitudeAboveGround,
                                Pitch = flightStatus.Value.Pitch,
                                Bank = flightStatus.Value.Bank,
                                Heading = flightStatus.Value.MagneticHeading,
                                TrueHeading = flightStatus.Value.TrueHeading,
                                GroundSpeed = flightStatus.Value.GroundSpeed,
                                IndicatedAirSpeed = flightStatus.Value.IndicatedAirSpeed,
                                VerticalSpeed = flightStatus.Value.VerticalSpeed,
                                FuelTotalQuantity = flightStatus.Value.FuelTotalQuantity,
                                WindDirection = flightStatus.Value.WindDirection,
                                WindVelocity = flightStatus.Value.WindVelocity,
                                IsOnGround = flightStatus.Value.IsOnGround == 1,
                                StallWarning = flightStatus.Value.StallWarning == 1,
                                OverspeedWarning = flightStatus.Value.OverspeedWarning == 1,
                                IsAutopilotOn = flightStatus.Value.IsAutopilotOn == 1,
                                IsApHdgOn = flightStatus.Value.IsApHdgOn == 1,
                                ApHeading = flightStatus.Value.ApHdg,
                                IsApNavOn = flightStatus.Value.IsApNavOn == 1,
                                IsApAprOn = flightStatus.Value.IsApAprOn == 1,
                                IsApAltOn = flightStatus.Value.IsApAltOn == 1,
                                ApAltitude = flightStatus.Value.ApAlt,
                                IsApVsOn = flightStatus.Value.IsApVsOn == 1,
                                IsApFlcOn = flightStatus.Value.IsApFlcOn == 1,
                                ApAirspeed = flightStatus.Value.ApAirspeed,
                                ApVs = flightStatus.Value.ApVs,
                                QNHMbar = flightStatus.Value.QNHmbar,
                                Transponder = flightStatus.Value.Transponder.ToString().PadLeft(4, '0'),
                                FreqencyCom1 = flightStatus.Value.Com1,
                                FreqencyCom2 = flightStatus.Value.Com2,
                                IsAvMasterOn = flightStatus.Value.AvMasterOn == 1,
                                Nav1OBS = flightStatus.Value.Nav1OBS,
                                Nav2OBS = flightStatus.Value.Nav2OBS,
                                ADFCard = flightStatus.Value.ADFCard,
                            }));
                    }
                    else
                    {
                        // Cast failed
                        logger.LogError("Cannot cast to FlightStatusStruct!");
                    }
                }
                break;

            case (uint)DATA_REQUESTS.TOGGLE_VALUE_DATA:
                {
                    var result = new Dictionary<SimVarRegistration, double>();
                    lock (lockLists)
                    {
                        if (data.dwDefineCount != genericValues.Count)
                        {
                            logger.LogError("Incompatible array count {actual}, expected {expected}. Skipping received data", data.dwDefineCount, genericValues.Count);
                            return;
                        }

                        var dataArray = data.dwData[0] as GenericValuesStruct?;

                        if (!dataArray.HasValue)
                        {
                            logger.LogError("Invalid data received");
                            return;
                        }

                        for (int i = 0; i < data.dwDefineCount; i++)
                        {
                            var genericValue = genericValues.Keys.ElementAt(i);
                            result.Add(genericValue, dataArray.Value.Get(i));
                        }
                    }

                    GenericValuesUpdated?.Invoke(this, new ToggleValueUpdatedEventArgs(result));
                }
                break;
        }
    }

    private void Simconnect_OnRecvSystemState(SimConnect sender, SIMCONNECT_RECV_SYSTEM_STATE data)
    {
        switch (data.dwRequestID)
        {
            case (int)DATA_REQUESTS.FLIGHT_PLAN:
                if (!string.IsNullOrEmpty(data.szString))
                {
                    logger.LogInformation("Receive flight plan {flightPlan}", data.szString);
                }
                break;
        }
    }

    void Simconnect_OnRecvOpen(SimConnect sender, SIMCONNECT_RECV_OPEN data)
    {
        logger.LogInformation("Connected to Flight Simulator");

        cts?.Cancel();
        cts = new CancellationTokenSource();
        Task.Run(async () =>
        {
            try
            {
                while (true)
                {
                    await Task.Delay(StatusDelayMilliseconds);
                    await smGeneric.WaitAsync();
                    try
                    {
                        cts?.Token.ThrowIfCancellationRequested();
                        simconnect?.RequestDataOnSimObjectType(DATA_REQUESTS.FLIGHT_STATUS, DEFINITIONS.FlightStatus, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);

                        if (genericValues.Count > 0 && isGenericValueRegistered)
                        {
                            simconnect?.RequestDataOnSimObjectType(DATA_REQUESTS.TOGGLE_VALUE_DATA, DEFINITIONS.GenericData, 0, SIMCONNECT_SIMOBJECT_TYPE.USER);
                        }
                    }
                    catch (COMException ex) when (ex.Message == "0xC00000B0")
                    {
                        // Ignore
                    }
                    finally
                    {
                        smGeneric.Release();
                    }
                }
            }
            catch (TaskCanceledException) { }
        });
    }

    // The case where the user closes Flight Simulator
    void Simconnect_OnRecvQuit(SimConnect sender, SIMCONNECT_RECV data)
    {
        logger.LogInformation("Flight Simulator has exited");
        CloseConnection();
        Closed?.Invoke(this, new EventArgs());
    }

    void Simconnect_OnRecvException(SimConnect sender, SIMCONNECT_RECV_EXCEPTION data)
    {
        logger.LogError("Exception received: {error} from {sendID}", (SIMCONNECT_EXCEPTION)data.dwException, data.dwSendID);
        switch ((SIMCONNECT_EXCEPTION)data.dwException)
        {
            case SIMCONNECT_EXCEPTION.ERROR:
                // Try to reconnect on unknown error
                CloseConnection();
                Closed?.Invoke(this, new EventArgs());
                break;

            case SIMCONNECT_EXCEPTION.NAME_UNRECOGNIZED:
                InvalidEventRegistered?.Invoke(this, new InvalidEventRegisteredEventArgs(data.dwSendID));
                break;

            case SIMCONNECT_EXCEPTION.VERSION_MISMATCH:
                // HACK: when sending an event repeatedly,
                // SimConnect might sendd thihs error and stop reacting and responding.
                // The workaround would be to force a reconnection.
                CloseConnection();
                Closed?.Invoke(this, new EventArgs());
                break;
        }
    }

    private void RecoverFromError(Exception exception)
    {
        // 0xC000014B: CTD
        // 0xC00000B0: Sim has exited or any generic SimConnect error
        // 0xC000014B: STATUS_PIPE_BROKEN
        logger.LogError(exception, "Exception received");
        CloseConnection();
        Closed?.Invoke(this, new EventArgs());
    }

    private uint GetLastSendID()
    {
        SimConnect_GetLastSentPacketID(hSimConnect, out uint dwSendID);
        return dwSendID;
    }

    #region Generic Buttons

    public uint? RegisterToggleEvent(Enum eventEnum, string eventName)
    {
        if (simconnect == null) return null;

        logger.LogInformation("RegisterEvent {action} {simConnectAction}", eventEnum, eventName);
        simconnect.MapClientEventToSimEvent(eventEnum, eventName);

        return GetLastSendID();
    }

    public void RegisterSimValues(IEnumerable<SimVarRegistration> simValues)
    {
        var changed = false;
        lock (lockLists)
        {
            logger.LogInformation("Registering {values}", string.Join(", ", simValues.Select(o => o.variableName + " " + o.variableUnit)));
            foreach (var simValue in simValues)
            {
                if (genericValues.ContainsKey(simValue))
                {
                    genericValues[simValue]++;
                }
                else
                {
                    genericValues.Add(simValue, 1);
                    changed = true;
                }
            }
        }
        if (changed)
        {
            RegisterGenericValues();
        }
    }

    public void DeRegisterSimValues(IEnumerable<SimVarRegistration> simValues)
    {
        var changed = false;
        lock (lockLists)
        {
            logger.LogInformation("De-Registering {values}", string.Join(", ", simValues.Select(o => o.variableName + " " + o.variableUnit)));
            foreach (var simValue in simValues)
            {
                if (genericValues.ContainsKey(simValue))
                {
                    var currentCount = genericValues[simValue];
                    if (currentCount > 1)
                    {
                        genericValues[simValue]--;
                    }
                    else
                    {
                        genericValues.Remove(simValue);
                        changed = true;
                    }
                }
            }
        }
        if (changed)
        {
            RegisterGenericValues();
        }
    }

    private CancellationTokenSource? ctsGeneric = null;
    private readonly object lockGeneric = new object();
    private readonly SemaphoreSlim smGeneric = new SemaphoreSlim(1);
    private bool isGenericValueRegistered = false;

    private void RegisterGenericValues()
    {
        if (simconnect == null) return;

        CancellationTokenSource cts;
        lock (lockGeneric)
        {
            ctsGeneric?.Cancel();
            cts = ctsGeneric = new CancellationTokenSource();
        }

        Task.Run(async () =>
        {
            await smGeneric.WaitAsync();
            try
            {

                await Task.Delay(500, cts.Token);
                cts.Token.ThrowIfCancellationRequested();

                if (simconnect == null) return;

                if (isGenericValueRegistered)
                {
                    logger.LogInformation("Clearing Data definition");
                    simconnect.ClearDataDefinition(DEFINITIONS.GenericData);
                    isGenericValueRegistered = false;
                }

                if (genericValues.Count == 0)
                {
                    logger.LogInformation("Registration is not needed.");
                }
                else
                {
                    var log = "Registering generic data structure:";

                    foreach (var registration in genericValues.Keys)
                    {
                        log += string.Format("\n- {0} {1}", registration.variableName, registration.variableUnit);

                        simconnect.AddToDataDefinition(
                            DEFINITIONS.GenericData,
                            registration.variableName,
                            registration.variableUnit,
                            SIMCONNECT_DATATYPE.FLOAT64,
                            0.0f,
                            SimConnect.SIMCONNECT_UNUSED
                        );
                    }

                    logger.LogInformation(log);

                    simconnect.RegisterDataDefineStruct<GenericValuesStruct>(DEFINITIONS.GenericData);

                    isGenericValueRegistered = true;
                }
            }
            catch (TaskCanceledException)
            {
                logger.LogDebug("Registration is cancelled.");
            }
            finally
            {
                smGeneric.Release();
            }
        });
    }

    public void Trigger(Enum eventEnum, uint data)
    {
        logger.LogInformation("Toggle {event} {data}", eventEnum, data);
        SendGenericCommand(eventEnum, data);
    }

    #region New Methods Implementation

    // Avionics Master Controls
    public void AvionicsMaster1On()
    {
        SendCommand(EVENTS.AVIONICS_MASTER_1_ON);
    }

    public void AvionicsMaster1Off()
    {
        SendCommand(EVENTS.AVIONICS_MASTER_1_OFF);
    }

    public void AvionicsMaster2On()
    {
        SendCommand(EVENTS.AVIONICS_MASTER_2_ON);
    }

    public void AvionicsMaster2Off()
    {
        SendCommand(EVENTS.AVIONICS_MASTER_2_OFF);
    }

    // Engine Controls
    public void EngineAutoStart()
    {
        SendCommand(EVENTS.ENGINE_AUTO_START);
    }

    public void EngineAutoShutdown()
    {
        SendCommand(EVENTS.ENGINE_AUTO_SHUTDOWN);
    }

    // Throttle Controls
    public void ThrottleFull()
    {
        SendCommand(EVENTS.THROTTLE_FULL);
    }

    public void ThrottleIncr()
    {
        SendCommand(EVENTS.THROTTLE_INCR);
    }

    public void ThrottleIncrSmall()
    {
        SendCommand(EVENTS.THROTTLE_INCR_SMALL);
    }

    public void ThrottleDecr()
    {
        SendCommand(EVENTS.THROTTLE_DECR);
    }

    public void ThrottleDecrSmall()
    {
        SendCommand(EVENTS.THROTTLE_DECR_SMALL);
    }

    public void ThrottleCut()
    {
        SendCommand(EVENTS.THROTTLE_CUT);
    }

    public void ThrottleSet(uint value)
    {
        SendCommand(EVENTS.THROTTLE_SET, value);
    }

    public void Throttle1Set(uint value)
    {
        SendCommand(EVENTS.THROTTLE1_SET, value);
    }

    public void Throttle2Set(uint value)
    {
        SendCommand(EVENTS.THROTTLE2_SET, value);
    }

    public void Throttle3Set(uint value)
    {
        SendCommand(EVENTS.THROTTLE3_SET, value);
    }

    public void Throttle4Set(uint value)
    {
        SendCommand(EVENTS.THROTTLE4_SET, value);
    }

    // Propeller Controls
    public void PropPitchIncr()
    {
        SendCommand(EVENTS.PROP_PITCH_INCR);
    }

    public void PropPitchDecr()
    {
        SendCommand(EVENTS.PROP_PITCH_DECR);
    }

    public void PropPitchHi()
    {
        SendCommand(EVENTS.PROP_PITCH_HI);
    }

    public void PropPitchLo()
    {
        SendCommand(EVENTS.PROP_PITCH_LO);
    }

    public void PropPitchSet(uint value)
    {
        SendCommand(EVENTS.PROP_PITCH_SET, value);
    }

    // Mixture Controls
    public void MixtureRich()
    {
        SendCommand(EVENTS.MIXTURE_RICH);
    }

    public void MixtureLean()
    {
        SendCommand(EVENTS.MIXTURE_LEAN);
    }

    public void MixtureIncr()
    {
        SendCommand(EVENTS.MIXTURE_INCR);
    }

    public void MixtureDecr()
    {
        SendCommand(EVENTS.MIXTURE_DECR);
    }

    public void MixtureSet(uint value)
    {
        SendCommand(EVENTS.MIXTURE_SET, value);
    }

    // Magneto Controls
    public void MagnetoOff()
    {
        SendCommand(EVENTS.MAGNETO_OFF);
    }

    public void MagnetoRight()
    {
        SendCommand(EVENTS.MAGNETO_RIGHT);
    }

    public void MagnetoLeft()
    {
        SendCommand(EVENTS.MAGNETO_LEFT);
    }

    public void MagnetoBoth()
    {
        SendCommand(EVENTS.MAGNETO_BOTH);
    }

    public void MagnetoStart()
    {
        SendCommand(EVENTS.MAGNETO_START);
    }

    public void MagnetoSet(uint value)
    {
        SendCommand(EVENTS.MAGNETO_SET, value);
    }

    // Trim Controls
    public void ElevTrimUp()
    {
        SendCommand(EVENTS.ELEV_TRIM_UP);
    }

    public void ElevTrimDown()
    {
        SendCommand(EVENTS.ELEV_TRIM_DN);
    }

    public void ElevTrimSet(uint value)
    {
        SendCommand(EVENTS.ELEV_TRIM_SET, value);
    }

    public void AileronTrimLeft()
    {
        SendCommand(EVENTS.AILERON_TRIM_LEFT);
    }

    public void AileronTrimRight()
    {
        SendCommand(EVENTS.AILERON_TRIM_RIGHT);
    }

    public void RudderTrimLeft()
    {
        SendCommand(EVENTS.RUDDER_TRIM_LEFT);
    }

    public void RudderTrimRight()
    {
        SendCommand(EVENTS.RUDDER_TRIM_RIGHT);
    }

    // Flight Controls
    public void FlapsUp()
    {
        SendCommand(EVENTS.FLAPS_UP);
    }

    public void FlapsDown()
    {
        SendCommand(EVENTS.FLAPS_DOWN);
    }

    public void FlapsSet(uint value)
    {
        SendCommand(EVENTS.FLAPS_SET, value);
    }

    public void GearToggle()
    {
        SendCommand(EVENTS.GEAR_TOGGLE);
    }

    public void GearUp()
    {
        SendCommand(EVENTS.GEAR_UP);
    }

    public void GearDown()
    {
        SendCommand(EVENTS.GEAR_DOWN);
    }

    public void SpoilersToggle()
    {
        SendCommand(EVENTS.SPOILERS_TOGGLE);
    }

    // Brakes
    public void Brakes()
    {
        SendCommand(EVENTS.BRAKES);
    }

    public void BrakesLeft()
    {
        SendCommand(EVENTS.BRAKES_LEFT);
    }

    public void BrakesRight()
    {
        SendCommand(EVENTS.BRAKES_RIGHT);
    }

    public void ParkingBrakes()
    {
        SendCommand(EVENTS.PARKING_BRAKES);
    }

    // Lights
    public void LandingLightsToggle()
    {
        SendCommand(EVENTS.LANDING_LIGHTS_TOGGLE);
    }

    public void LandingLightsOn()
    {
        SendCommand(EVENTS.LANDING_LIGHTS_ON);
    }

    public void LandingLightsOff()
    {
        SendCommand(EVENTS.LANDING_LIGHTS_OFF);
    }

    public void TaxiLightsToggle()
    {
        SendCommand(EVENTS.TAXI_LIGHTS_TOGGLE);
    }

    public void NavLightsToggle()
    {
        SendCommand(EVENTS.NAV_LIGHTS_TOGGLE);
    }

    public void BeaconLightsToggle()
    {
        SendCommand(EVENTS.BEACON_LIGHTS_TOGGLE);
    }

    public void StrobeLightsToggle()
    {
        SendCommand(EVENTS.STROBE_LIGHTS_TOGGLE);
    }

    public void PanelLightsToggle()
    {
        SendCommand(EVENTS.PANEL_LIGHTS_TOGGLE);
    }

    // Radio Controls
    public void Com1RadioSet(uint frequency)
    {
        SendCommand(EVENTS.COM1_RADIO_SET, frequency);
    }

    public void Com1StbyRadioSwap()
    {
        SendCommand(EVENTS.COM_STBY_RADIO_SWAP);
    }

    public void Com2RadioSet(uint frequency)
    {
        SendCommand(EVENTS.COM2_RADIO_SET, frequency);
    }

    public void Com2StbyRadioSwap()
    {
        SendCommand(EVENTS.COM2_STBY_RADIO_SWAP);
    }

    public void Nav1RadioSet(uint frequency)
    {
        SendCommand(EVENTS.NAV1_RADIO_SET, frequency);
    }

    public void Nav1StbyRadioSwap()
    {
        SendCommand(EVENTS.NAV1_STBY_RADIO_SWAP);
    }

    public void Nav1OBSSet(uint value)
    {
        SendCommand(EVENTS.NAV1_OBS_SET, value);
    }

    public void Nav2RadioSet(uint frequency)
    {
        SendCommand(EVENTS.NAV2_RADIO_SET, frequency);
    }

    public void Nav2StbyRadioSwap()
    {
        SendCommand(EVENTS.NAV2_STBY_RADIO_SWAP);
    }

    public void Nav2OBSSet(uint value)
    {
        SendCommand(EVENTS.NAV2_OBS_SET, value);
    }

    public void TransponderSet(uint code)
    {
        SendCommand(EVENTS.XPNDR_SET, code);
    }

    public void ADF1CompleteSet(uint frequency)
    {
        SendCommand(EVENTS.ADF_COMPLETE_SET, frequency);
    }

    // Fuel System
    public void FuelPump()
    {
        SendCommand(EVENTS.FUEL_PUMP);
    }

    public void ToggleFuelValveEng1()
    {
        SendCommand(EVENTS.TOGGLE_FUEL_VALVE_ENG1);
    }

    public void ToggleFuelValveEng2()
    {
        SendCommand(EVENTS.TOGGLE_FUEL_VALVE_ENG2);
    }

    public void FuelSelectorSet(uint value)
    {
        SendCommand(EVENTS.FUEL_SELECTOR_SET, value);
    }

    // Electrical System
    public void ToggleMasterBattery()
    {
        SendCommand(EVENTS.TOGGLE_MASTER_BATTERY);
    }

    public void ToggleMasterAlternator()
    {
        SendCommand(EVENTS.TOGGLE_MASTER_ALTERNATOR);
    }

    public void MasterBatteryOn()
    {
        SendCommand(EVENTS.MASTER_BATTERY_ON);
    }

    public void MasterBatteryOff()
    {
        SendCommand(EVENTS.MASTER_BATTERY_OFF);
    }

    // Anti-Ice
    public void PitotHeatToggle()
    {
        SendCommand(EVENTS.PITOT_HEAT_TOGGLE);
    }

    public void ToggleStructuralDeice()
    {
        SendCommand(EVENTS.TOGGLE_STRUCTURAL_DEICE);
    }

    // Simulation
    public void PauseToggle()
    {
        SendCommand(EVENTS.PAUSE_TOGGLE);
    }

    public void SimRateSet(uint rate)
    {
        SendCommand(EVENTS.SIM_RATE_SET, rate);
    }

    #endregion

    #endregion
}
