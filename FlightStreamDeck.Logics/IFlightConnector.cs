using System;
using System.Collections.Generic;

namespace FlightStreamDeck.Logics;

public interface IFlightConnector
{
    event EventHandler<AircraftStatusUpdatedEventArgs> AircraftStatusUpdated;
    event EventHandler<ToggleValueUpdatedEventArgs> GenericValuesUpdated;
    event EventHandler<InvalidEventRegisteredEventArgs> InvalidEventRegistered;

    // Autopilot Controls
    void ApOff();
    void ApOn();
    void ApToggle();
    void ApHdgToggle();
    void ApNavToggle();
    void ApAprToggle();
    void ApAltToggle();
    void ApVsToggle();
    void ApFlcOn();
    void ApFlcOff();

    /// <param name="heading">In Degree</param>
    void ApHdgSet(uint heading);
    void ApHdgInc();
    void ApHdgDec();

    /// <param name="altitude">In Feet</param>
    void ApAltSet(uint altitude);
    void ApAltInc();
    void ApAltDec();

    /// <param name="speed">In Feet per min</param>
    void ApVsSet(int speed);

    void ApAirSpeedSet(uint speed);
    void ApAirSpeedInc();
    void ApAirSpeedDec();

    // Barometric/QNH Controls
    void QNHSet(uint qnh);
    void QNHInc();
    void QNHDec();

    // Avionics
    void AvMasterToggle(uint state);
    void AvionicsMaster1On();
    void AvionicsMaster1Off();
    void AvionicsMaster2On();
    void AvionicsMaster2Off();
    
    // Engine Controls
    void EngineAutoStart();
    void EngineAutoShutdown();
    void ThrottleFull();
    void ThrottleIncr();
    void ThrottleIncrSmall();
    void ThrottleDecr();
    void ThrottleDecrSmall();
    void ThrottleCut();
    void ThrottleSet(uint percent);
    void Throttle1Set(uint percent);
    void Throttle2Set(uint percent);
    void Throttle3Set(uint percent);
    void Throttle4Set(uint percent);
    
    // Propeller Controls
    void PropPitchIncr();
    void PropPitchDecr();
    void PropPitchHi();
    void PropPitchLo();
    void PropPitchSet(uint percent);
    
    // Mixture Controls
    void MixtureRich();
    void MixtureLean();
    void MixtureIncr();
    void MixtureDecr();
    void MixtureSet(uint percent);
    
    // Magneto/Ignition
    void MagnetoOff();
    void MagnetoRight();
    void MagnetoLeft();
    void MagnetoBoth();
    void MagnetoStart();
    void MagnetoSet(uint position);
    
    // Flight Controls
    void ElevTrimUp();
    void ElevTrimDown();
    void ElevTrimSet(uint position);
    void AileronTrimLeft();
    void AileronTrimRight();
    void RudderTrimLeft();
    void RudderTrimRight();
    
    // Flaps & Gear
    void FlapsUp();
    void FlapsDown();
    void FlapsSet(uint position);
    void GearToggle();
    void GearUp();
    void GearDown();
    void SpoilersToggle();
    
    // Brakes
    void Brakes();
    void BrakesLeft();
    void BrakesRight();
    void ParkingBrakes();
    
    // Lights
    void LandingLightsToggle();
    void LandingLightsOn();
    void LandingLightsOff();
    void TaxiLightsToggle();
    void NavLightsToggle();
    void BeaconLightsToggle();
    void StrobeLightsToggle();
    void PanelLightsToggle();
    
    // Radio/Navigation
    void Com1RadioSet(uint frequency);
    void Com1StbyRadioSwap();
    void Com2RadioSet(uint frequency);
    void Com2StbyRadioSwap();
    void Nav1RadioSet(uint frequency);
    void Nav1StbyRadioSwap();
    void Nav1OBSSet(uint degrees);
    void Nav2RadioSet(uint frequency);
    void Nav2StbyRadioSwap();
    void Nav2OBSSet(uint degrees);
    
    // Transponder & ADF
    void TransponderSet(uint code);
    void ADF1CompleteSet(uint frequency);
    
    // Fuel Systems
    void FuelPump();
    void ToggleFuelValveEng1();
    void ToggleFuelValveEng2();
    void FuelSelectorSet(uint position);
    
    // Electrical
    void ToggleMasterBattery();
    void ToggleMasterAlternator();
    void MasterBatteryOn();
    void MasterBatteryOff();
    
    // Environmental
    void PitotHeatToggle();
    void ToggleStructuralDeice();
    
    // Simulation Control
    void PauseToggle();
    void SimRateSet(uint rate);
    
    void Trigger(Enum eventEnum, uint data);

    uint? RegisterToggleEvent(Enum eventEnum, string eventName);

    void RegisterSimValues(IEnumerable<SimVarRegistration> simValues);
    void DeRegisterSimValues(IEnumerable<SimVarRegistration> simValues);
}

public class AircraftStatusUpdatedEventArgs : EventArgs
{
    public AircraftStatusUpdatedEventArgs(AircraftStatus aircraftStatus)
    {
        AircraftStatus = aircraftStatus;
    }

    public AircraftStatus AircraftStatus { get; }
}

public class ToggleValueUpdatedEventArgs : EventArgs
{
    public ToggleValueUpdatedEventArgs(Dictionary<SimVarRegistration, double> genericValueStatus)
    {
        GenericValueStatus = genericValueStatus;
    }

    public Dictionary<SimVarRegistration, double> GenericValueStatus { get; }
}

public class InvalidEventRegisteredEventArgs : EventArgs
{
    public InvalidEventRegisteredEventArgs(uint sendID)
    {
        SendID = sendID;
    }

    public uint SendID { get; }
}

public class AircraftStatus
{
    public string Callsign { get; set; } = "";

    public double SimTime { get; set; }
    public int? LocalTime { get; set; }
    public int? ZuluTime { get; set; }
    public long? AbsoluteTime { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }
    public double AltitudeAboveGround { get; set; }

    public double Heading { get; set; }
    public double TrueHeading { get; set; }

    public double WindDirection { get; set; }
    public double WindVelocity { get; set; }

    public double GroundSpeed { get; set; }
    public double IndicatedAirSpeed { get; set; }
    public double VerticalSpeed { get; set; }

    public double FuelTotalQuantity { get; set; }

    public double Pitch { get; set; }
    public double Bank { get; set; }

    public bool IsOnGround { get; set; }
    public bool StallWarning { get; set; }
    public bool OverspeedWarning { get; set; }

    public bool IsAutopilotOn { get; set; }

    public bool IsApHdgOn { get; set; }
    public int ApHeading { get; set; }

    public bool IsApNavOn { get; set; }

    public bool IsApAprOn { get; set; }

    public bool IsApAltOn { get; set; }
    public int ApAltitude { get; set; }

    public bool IsApVsOn { get; set; }
    public int ApVs { get; set; }

    public bool IsApFlcOn { get; set; }
    public int ApAirspeed { get; set; }

    public int QNHMbar { get; set; }

    public string Transponder { get; set; } = "";
    public int FreqencyCom1 { get; set; }
    public int FreqencyCom2 { get; set; }
    public bool IsAvMasterOn { get; set; }
    public double Nav1OBS { get; set; }
    public double Nav2OBS { get; set; }
    public double ADFCard { get; set; }
    public int ADFActiveFrequency1 { get; set; }
    public int ADFStandbyFrequency1 { get; set; }
    public int ADFActiveFrequency2 { get; set; }
    public int ADFStandbyFrequency2 { get; set; }
}

public record SimVarRegistration(
    string variableName,
    string? variableUnit
);