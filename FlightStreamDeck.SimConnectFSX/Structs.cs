using System.Runtime.InteropServices;

namespace FlightStreamDeck.SimConnectFSX;

enum GROUPID
{
    FLAG = 2000000000,
    MAX = 1,
};

enum DEFINITIONS
{
    AircraftData,
    FlightStatus,
    GenericData,
    EngineData,
    SystemsData,
    EnvironmentalData,
    NavigationData,
    FuelData,
    ElectricalData
}

internal enum DATA_REQUESTS
{
    NONE,
    SUBSCRIBE_GENERIC,
    AIRCRAFT_DATA,
    FLIGHT_STATUS,
    ENVIRONMENT_DATA,
    FLIGHT_PLAN,
    TOGGLE_VALUE_DATA,
    ENGINE_DATA,
    SYSTEMS_DATA,
    NAVIGATION_DATA,
    FUEL_DATA,
    ELECTRICAL_DATA,
    AI_OBJECTS,
    FACILITIES_DATA,
    INPUT_EVENTS,
    SYSTEM_STATE,
    CLIENT_DATA,
    WEATHER_DATA
}

public enum EVENTS
{
    MESSAGE_RECEIVED,
    
    // Autopilot Events
    AUTOPILOT_ON,
    AUTOPILOT_OFF,
    AUTOPILOT_TOGGLE,
    AP_MASTER,
    AP_HDG_TOGGLE,
    AP_NAV_TOGGLE,
    AP_APR_TOGGLE,
    AP_ALT_TOGGLE,
    AP_VS_TOGGLE,
    AP_FLC_ON,
    AP_FLC_OFF,
    AP_HDG_SET,
    AP_HDG_INC,
    AP_HDG_DEC,
    AP_ALT_SET,
    AP_ALT_INC,
    AP_ALT_DEC,
    AP_VS_SET,
    AP_VS_INC,
    AP_VS_DEC,
    AP_AIRSPEED_SET,
    AP_AIRSPEED_INC,
    AP_AIRSPEED_DEC,
    FLIGHT_LEVEL_CHANGE,
    FLIGHT_LEVEL_CHANGE_ON,
    FLIGHT_LEVEL_CHANGE_OFF,
    YAW_DAMPER_TOGGLE,
    YAW_DAMPER_ON,
    YAW_DAMPER_OFF,
    
    // Instrumentation
    QNH_SET,
    QNH_INC,
    QNH_DEC,
    KOHLSMAN_SET,
    KOHLSMAN_INC,
    KOHLSMAN_DEC,
    BAROMETRIC,
    BAROMETRIC_STD_PRESSURE,
    
    // Avionics
    AVIONICS_TOGGLE,
    AVIONICS_MASTER_SET,
    AVIONICS_MASTER_1_ON,
    AVIONICS_MASTER_1_OFF,
    AVIONICS_MASTER_1_SET,
    AVIONICS_MASTER_2_ON,
    AVIONICS_MASTER_2_OFF,
    AVIONICS_MASTER_2_SET,
    
    // Engine Events
    ENGINE_AUTO_START,
    ENGINE_AUTO_SHUTDOWN,
    THROTTLE_FULL,
    THROTTLE_INCR,
    THROTTLE_INCR_SMALL,
    THROTTLE_DECR,
    THROTTLE_DECR_SMALL,
    THROTTLE_CUT,
    THROTTLE_SET,
    THROTTLE1_SET,
    THROTTLE2_SET,
    THROTTLE3_SET,
    THROTTLE4_SET,
    INCREASE_THROTTLE,
    DECREASE_THROTTLE,
    THROTTLE1_FULL,
    THROTTLE1_INCR,
    THROTTLE1_INCR_SMALL,
    THROTTLE1_DECR,
    THROTTLE1_DECR_SMALL,
    THROTTLE1_CUT,
    THROTTLE2_FULL,
    THROTTLE2_INCR,
    THROTTLE2_INCR_SMALL,
    THROTTLE2_DECR,
    THROTTLE2_DECR_SMALL,
    THROTTLE2_CUT,
    THROTTLE3_FULL,
    THROTTLE3_INCR,
    THROTTLE3_INCR_SMALL,
    THROTTLE3_DECR,
    THROTTLE3_DECR_SMALL,
    THROTTLE3_CUT,
    THROTTLE4_FULL,
    THROTTLE4_INCR,
    THROTTLE4_INCR_SMALL,
    THROTTLE4_DECR,
    THROTTLE4_DECR_SMALL,
    THROTTLE4_CUT,
    
    // Propeller/Mixture
    PROP_PITCH_INCR,
    PROP_PITCH_DECR,
    PROP_PITCH_HI,
    PROP_PITCH_LO,
    PROP_PITCH_SET,
    PROP_PITCH1_SET,
    PROP_PITCH2_SET,
    PROP_PITCH3_SET,
    PROP_PITCH4_SET,
    MIXTURE_RICH,
    MIXTURE_LEAN,
    MIXTURE_INCR,
    MIXTURE_DECR,
    MIXTURE_SET,
    MIXTURE1_SET,
    MIXTURE2_SET,
    MIXTURE3_SET,
    MIXTURE4_SET,
    
    // Magneto/Ignition
    MAGNETO,
    MAGNETO_DECR,
    MAGNETO_INCR,
    MAGNETO_OFF,
    MAGNETO_RIGHT,
    MAGNETO_LEFT,
    MAGNETO_BOTH,
    MAGNETO_START,
    MAGNETO_SET,
    MAGNETO1_OFF,
    MAGNETO1_RIGHT,
    MAGNETO1_LEFT,
    MAGNETO1_BOTH,
    MAGNETO1_START,
    MAGNETO1_SET,
    MAGNETO2_OFF,
    MAGNETO2_RIGHT,
    MAGNETO2_LEFT,
    MAGNETO2_BOTH,
    MAGNETO2_START,
    MAGNETO2_SET,
    MAGNETO3_OFF,
    MAGNETO3_RIGHT,
    MAGNETO3_LEFT,
    MAGNETO3_BOTH,
    MAGNETO3_START,
    MAGNETO3_SET,
    MAGNETO4_OFF,
    MAGNETO4_RIGHT,
    MAGNETO4_LEFT,
    MAGNETO4_BOTH,
    MAGNETO4_START,
    MAGNETO4_SET,
    
    // Starter
    STARTER1_SET,
    STARTER2_SET,
    STARTER3_SET,
    STARTER4_SET,
    TOGGLE_STARTER1,
    TOGGLE_STARTER2,
    TOGGLE_STARTER3,
    TOGGLE_STARTER4,
    
    // Flight Controls
    ELEV_TRIM_UP,
    ELEV_TRIM_DN,
    ELEV_TRIM_SET,
    AILERON_TRIM_LEFT,
    AILERON_TRIM_RIGHT,
    AILERON_TRIM_SET,
    RUDDER_TRIM_LEFT,
    RUDDER_TRIM_RIGHT,
    RUDDER_TRIM_SET,
    FLAPS_UP,
    FLAPS_DOWN,
    FLAPS_1,
    FLAPS_2,
    FLAPS_3,
    FLAPS_SET,
    FLAPS_INCR,
    FLAPS_DECR,
    SPOILERS_ON,
    SPOILERS_OFF,
    SPOILERS_TOGGLE,
    SPOILERS_SET,
    GEAR_TOGGLE,
    GEAR_UP,
    GEAR_DOWN,
    GEAR_SET,
    
    // Brakes
    BRAKES,
    BRAKES_LEFT,
    BRAKES_RIGHT,
    PARKING_BRAKES,
    TOGGLE_TAIL_WHEEL_LOCK,
    
    // Lights
    LANDING_LIGHTS_TOGGLE,
    LANDING_LIGHTS_ON,
    LANDING_LIGHTS_OFF,
    LANDING_LIGHTS_SET,
    TAXI_LIGHTS_TOGGLE,
    TAXI_LIGHTS_ON,
    TAXI_LIGHTS_OFF,
    TAXI_LIGHTS_SET,
    NAV_LIGHTS_TOGGLE,
    NAV_LIGHTS_ON,
    NAV_LIGHTS_OFF,
    NAV_LIGHTS_SET,
    BEACON_LIGHTS_TOGGLE,
    BEACON_LIGHTS_ON,
    BEACON_LIGHTS_OFF,
    BEACON_LIGHTS_SET,
    STROBE_LIGHTS_TOGGLE,
    STROBE_LIGHTS_ON,
    STROBE_LIGHTS_OFF,
    STROBE_LIGHTS_SET,
    PANEL_LIGHTS_TOGGLE,
    PANEL_LIGHTS_ON,
    PANEL_LIGHTS_OFF,
    PANEL_LIGHTS_SET,
    
    // Radio/Navigation
    COM_RADIO,
    COM_RADIO_WHOLE_DEC,
    COM_RADIO_WHOLE_INC,
    COM_RADIO_FRACT_DEC,
    COM_RADIO_FRACT_INC,
    COM_STBY_RADIO_SET,
    COM_STBY_RADIO_SWAP,
    COM1_RADIO_WHOLE_DEC,
    COM1_RADIO_WHOLE_INC,
    COM1_RADIO_FRACT_DEC,
    COM1_RADIO_FRACT_INC,
    COM1_STBY_RADIO_SET,
    COM1_RADIO_SET,
    COM1_STBY_RADIO_SWAP,
    COM2_RADIO_WHOLE_DEC,
    COM2_RADIO_WHOLE_INC,
    COM2_RADIO_FRACT_DEC,
    COM2_RADIO_FRACT_INC,
    COM2_STBY_RADIO_SET,
    COM2_RADIO_SET,
    COM2_STBY_RADIO_SWAP,
    
    NAV1_RADIO_WHOLE_DEC,
    NAV1_RADIO_WHOLE_INC,
    NAV1_RADIO_FRACT_DEC,
    NAV1_RADIO_FRACT_INC,
    NAV1_STBY_SET,
    NAV1_RADIO_SET,
    NAV1_STBY_RADIO_SWAP,
    NAV1_OBS_SET,
    NAV1_OBS_DEC,
    NAV1_OBS_INC,
    
    NAV2_RADIO_WHOLE_DEC,
    NAV2_RADIO_WHOLE_INC,
    NAV2_RADIO_FRACT_DEC,
    NAV2_RADIO_FRACT_INC,
    NAV2_STBY_SET,
    NAV2_RADIO_SET,
    NAV2_STBY_RADIO_SWAP,
    NAV2_OBS_SET,
    NAV2_OBS_DEC,
    NAV2_OBS_INC,
    
    // ADF
    ADF_1_DEC,
    ADF_1_INC,
    ADF_10_DEC,
    ADF_10_INC,
    ADF_100_DEC,
    ADF_100_INC,
    ADF_1000_DEC,
    ADF_1000_INC,
    ADF_COMPLETE_SET,
    ADF_STBY_SET,
    ADF_ACTIVE_SET,
    ADF1_COMPLETE_SET,
    ADF2_COMPLETE_SET,
    
    // Transponder
    XPNDR,
    XPNDR_SET,
    XPNDR_1_INC,
    XPNDR_1_DEC,
    XPNDR_10_INC,
    XPNDR_10_DEC,
    XPNDR_100_INC,
    XPNDR_100_DEC,
    XPNDR_1000_INC,
    XPNDR_1000_DEC,
    
    // Fuel
    FUEL_PUMP,
    TOGGLE_FUEL_VALVE_ENG1,
    TOGGLE_FUEL_VALVE_ENG2,
    TOGGLE_FUEL_VALVE_ENG3,
    TOGGLE_FUEL_VALVE_ENG4,
    TOGGLE_FUEL_VALVE_ALL,
    FUEL_SELECTOR_OFF,
    FUEL_SELECTOR_ALL,
    FUEL_SELECTOR_LEFT,
    FUEL_SELECTOR_RIGHT,
    FUEL_SELECTOR_LEFT_AUX,
    FUEL_SELECTOR_RIGHT_AUX,
    FUEL_SELECTOR_CENTER,
    FUEL_SELECTOR_SET,
    
    // Electrical
    TOGGLE_MASTER_BATTERY,
    TOGGLE_MASTER_ALTERNATOR,
    TOGGLE_ELECTRIC_VACUUM_PUMP,
    MASTER_BATTERY_ON,
    MASTER_BATTERY_OFF,
    MASTER_ALTERNATOR_ON,
    MASTER_ALTERNATOR_OFF,
    BATTERY_ON,
    BATTERY_OFF,
    ALTERNATOR_ON,
    ALTERNATOR_OFF,
    
    // Environmental
    PITOT_HEAT_ON,
    PITOT_HEAT_OFF,
    PITOT_HEAT_TOGGLE,
    TOGGLE_STRUCTURAL_DEICE,
    TOGGLE_PROPELLER_DEICE,
    
    // Miscellaneous
    PAUSE_TOGGLE,
    PAUSE_ON,
    PAUSE_OFF,
    PAUSE_SET,
    SIM_RATE_INCR,
    SIM_RATE_DECR,
    SIM_RATE_SET,
    
    // View Events
    VIEW_COCKPIT_FORWARD,
    VIEW_VIRTUAL_COCKPIT_FORWARD,
    VIEW_CHASE,
    VIEW_TOWER,
    VIEW_RESET,
    
    // Slew Events
    SLEW_TOGGLE,
    SLEW_ON,
    SLEW_OFF,
    SLEW_SET,
    SLEW_RESET,
    SLEW_ALTIT_UP_FAST,
    SLEW_ALTIT_UP_SLOW,
    SLEW_ALTIT_FREEZE,
    SLEW_ALTIT_DN_SLOW,
    SLEW_ALTIT_DN_FAST,
    SLEW_PITCH_DN_FAST,
    SLEW_PITCH_DN_SLOW,
    SLEW_PITCH_FREEZE,
    SLEW_PITCH_UP_SLOW,
    SLEW_PITCH_UP_FAST,
    SLEW_BANK_LEFT_FAST,
    SLEW_BANK_LEFT_SLOW,
    SLEW_BANK_FREEZE,
    SLEW_BANK_RIGHT_SLOW,
    SLEW_BANK_RIGHT_FAST,
    SLEW_HEADING_LEFT_FAST,
    SLEW_HEADING_LEFT_SLOW,
    SLEW_HEADING_FREEZE,
    SLEW_HEADING_RIGHT_SLOW,
    SLEW_HEADING_RIGHT_FAST
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct AircraftDataStruct
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Type;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Model;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string Title;
    public double EstimatedCruiseSpeed;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct FlightStatusStruct
{
    public int SimRate;

    // Position & Orientation
    public double Latitude;
    public double Longitude;
    public double Altitude;
    public double AltitudeAboveGround;
    public double Pitch;
    public double Bank;
    public double TrueHeading;
    public double MagneticHeading;
    public double GroundAltitude;
    public double GroundSpeed;
    public double IndicatedAirSpeed;
    public double TrueAirSpeed;
    public double VerticalSpeed;
    public double MachSpeed;
    
    // Engine Data
    public double Engine1RPM;
    public double Engine2RPM;
    public double Engine3RPM;
    public double Engine4RPM;
    public double Engine1N1;
    public double Engine2N1;
    public double Engine3N1;
    public double Engine4N1;
    public double Engine1N2;
    public double Engine2N2;
    public double Engine3N2;
    public double Engine4N2;
    public double Engine1EGT;
    public double Engine2EGT;
    public double Engine3EGT;
    public double Engine4EGT;
    public double Engine1CHT;
    public double Engine2CHT;
    public double Engine3CHT;
    public double Engine4CHT;
    public double Engine1OilTemp;
    public double Engine2OilTemp;
    public double Engine3OilTemp;
    public double Engine4OilTemp;
    public double Engine1OilPressure;
    public double Engine2OilPressure;
    public double Engine3OilPressure;
    public double Engine4OilPressure;
    public double Engine1FuelFlow;
    public double Engine2FuelFlow;
    public double Engine3FuelFlow;
    public double Engine4FuelFlow;
    public double Engine1FuelPressure;
    public double Engine2FuelPressure;
    public double Engine3FuelPressure;
    public double Engine4FuelPressure;
    public double Engine1ThrottlePosition;
    public double Engine2ThrottlePosition;
    public double Engine3ThrottlePosition;
    public double Engine4ThrottlePosition;
    public double Engine1MixturePosition;
    public double Engine2MixturePosition;
    public double Engine3MixturePosition;
    public double Engine4MixturePosition;
    public double Engine1PropellerPosition;
    public double Engine2PropellerPosition;
    public double Engine3PropellerPosition;
    public double Engine4PropellerPosition;
    
    // Fuel
    public double FuelTotalQuantity;
    public double FuelTotalCapacity;
    public double FuelLeftQuantity;
    public double FuelRightQuantity;
    public double FuelLeftCapacity;
    public double FuelRightCapacity;
    public double FuelCenterQuantity;
    public double FuelCenterCapacity;
    public double FuelCenter2Quantity;
    public double FuelCenter2Capacity;
    public double FuelCenter3Quantity;
    public double FuelCenter3Capacity;
    public double FuelTotalWeight;
    public double FuelFlowGPH;
    public double FuelFlowPPH;
    
    // Environmental
    public double WindVelocity;
    public double WindDirection;
    public double TotalAirTemperature;
    public double StaticAirTemperature;
    public double BarometricPressure;
    public double SeaLevelPressure;
    public double DensityAltitude;
    public double PressureAltitude;
    
    // Flight Model
    public double AngleOfAttack;
    public double AngleOfAttackIndicator;
    public double SideSlipAngle;
    public double LoadFactor;
    public double GForce;
    public double TotalWeight;
    public double MaxGrossWeight;
    public double EmptyWeight;
    public double CenterOfGravityPCT;
    
    // Control Surfaces
    public double ElevatorPosition;
    public double AileronPosition;
    public double RudderPosition;
    public double ElevatorTrimPosition;
    public double AileronTrimPosition;
    public double RudderTrimPosition;
    public double FlapsPosition;
    public double SpoilerPosition;
    public double GearPosition;
    
    // Status Flags
    public int IsOnGround;
    public int StallWarning;
    public int OverspeedWarning;
    public int GearWarning;
    public int OilPressureWarning;
    public int FuelPumpWarning;
    public int VacuumWarning;
    public int LowVoltage;
    
    // Autopilot
    public int IsAutopilotOn;
    public int IsApHdgOn;
    public int ApHdg;
    public int IsApNavOn;
    public int IsApAprOn;
    public int IsApAltOn;
    public int ApAlt;
    public int IsApVsOn;
    public int ApVs;
    public int IsApFlcOn;
    public int ApAirspeed;
    public int IsYawDamperOn;
    
    // Instruments
    public int QNHmbar;
    public double QNHinHg;
    public int Transponder;
    public double GyroHeading;
    public double MagneticCompass;
    public double TurnIndicator;
    public double SlipIndicator;
    public double VerticalSpeedIndicator;
    
    // Radios
    public int Com1;
    public int Com1Standby;
    public int Com2;
    public int Com2Standby;
    public int Nav1;
    public int Nav1Standby;
    public int Nav2;
    public int Nav2Standby;
    public int ADF1;
    public int ADF1Standby;
    public int ADF2;
    public int ADF2Standby;
    
    // Navigation
    public double Nav1OBS;
    public double Nav2OBS;
    public double Nav1CDI;
    public double Nav2CDI;
    public double Nav1GSI;
    public double Nav2GSI;
    public double Nav1DME;
    public double Nav2DME;
    public double ADFCard;
    public int ADFActive1;
    public int ADFStandby1;
    public int ADFActive2;
    public int ADFStandby2;
    
    // Systems
    public int AvMasterOn;
    public int BatteryMasterOn;
    public int AlternatorMasterOn;
    public int FuelPump;
    public int DeiceSwitch;
    public int PitotHeat;
    public int StructuralDeice;
    public int PropellerDeice;
    
    // Lights
    public int LandingLights;
    public int TaxiLights;
    public int NavLights;
    public int BeaconLights;
    public int StrobeLights;
    public int PanelLights;
    public int CabinLights;
    
    // GPS (if available)
    public double GPSGroundSpeed;
    public double GPSGroundTrack;
    public double GPSMagneticTrack;
    public int GPSFlightPlanActive;
    public int GPSApproachActive;
    public double GPSCourseToSteer;
    public double GPSCrosstrackError;
    public double GPSWaypointDistance;
    public double GPSWaypointBearing;
    public double GPSDestinationDistance;
    public double GPSDestinationBearing;
    
    // Weather Radar (if available)
    public int WeatherRadarOn;
    public double WeatherRadarRange;
    public double WeatherRadarTilt;
    
    // TCAS (if available)
    public int TCASOn;
    public int TCASMode;
    public int TCASAdvisory;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct GenericValuesStruct
{
    unsafe public fixed double Data[64];

    unsafe public double Get(int index)
    {
        return Data[index];
    }
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct EngineDataStruct
{
    public double Engine1RPM;
    public double Engine2RPM;
    public double Engine3RPM;
    public double Engine4RPM;
    public double Engine1N1;
    public double Engine2N1;
    public double Engine1N2;
    public double Engine2N2;
    public double Engine1EGT;
    public double Engine2EGT;
    public double Engine1CHT;
    public double Engine2CHT;
    public double Engine1FuelFlow;
    public double Engine2FuelFlow;
    public double Engine1OilTemp;
    public double Engine2OilTemp;
    public double Engine1OilPressure;
    public double Engine2OilPressure;
    public double ManifoldPressure;
    public int Engine1Combustion;
    public int Engine2Combustion;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct SystemsDataStruct
{
    public int BatteryMaster;
    public int AlternatorMaster;
    public int AvionicsMaster;
    public int FuelPump;
    public int DeiceSwitch;
    public int PitotHeat;
    public int LandingLights;
    public int TaxiLights;
    public int NavLights;
    public int BeaconLights;
    public int StrobeLights;
    public int PanelLights;
    public double FlapsPosition;
    public double GearPosition;
    public double SpoilerPosition;
    public int ParkingBrake;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct NavigationDataStruct
{
    public int Com1Active;
    public int Com1Standby;
    public int Com2Active;
    public int Com2Standby;
    public int Nav1Active;
    public int Nav1Standby;
    public int Nav2Active;
    public int Nav2Standby;
    public double Nav1OBS;
    public double Nav2OBS;
    public double Nav1CDI;
    public double Nav2CDI;
    public double Nav1GSI;
    public double Nav2GSI;
    public double Nav1DME;
    public double Nav2DME;
    public int ADF1Active;
    public int ADF1Standby;
    public double ADFCard;
    public int Transponder;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct FuelDataStruct
{
    public double FuelTotalQuantity;
    public double FuelTotalCapacity;
    public double FuelLeftQuantity;
    public double FuelRightQuantity;
    public double FuelLeftCapacity;
    public double FuelRightCapacity;
    public double FuelCenterQuantity;
    public double FuelCenterCapacity;
    public double FuelTotalWeight;
    public double FuelFlowGPH;
    public double FuelFlowPPH;
    public int FuelPumpLeft;
    public int FuelPumpRight;
    public int FuelValveEngine1;
    public int FuelValveEngine2;
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct ElectricalDataStruct
{
    public double BatteryVoltage;
    public double BatteryLoad;
    public double AlternatorVoltage;
    public double AlternatorLoad;
    public double AvionicsBusVoltage;
    public double MainBusVoltage;
    public double ElectricalBusVoltage;
    public int BatterySwitch;
    public int AlternatorSwitch;
    public int AvionicsSwitch;
    public int ExternalPowerAvailable;
    public int ExternalPowerOn;
}
