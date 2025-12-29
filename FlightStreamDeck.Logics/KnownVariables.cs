using System.Collections.Generic;

namespace FlightStreamDeck.Core;

public static class KnownVariables
{
    record ValueEntry(string Unit, short Decimals);

    const string DEFAULT_UNIT = "number";
    const int DEFAULT_DECIMALS = 0;

    static Dictionary<string, ValueEntry?> availableValues;

    static KnownVariables()
    {
        availableValues = new Dictionary<string, ValueEntry?>()
        {
            // Basic SimConnect variables - core functionality only to prevent initialization issues
            { "ACCELERATION BODY X", new ValueEntry("meter per second squared", 2) },
            { "ACCELERATION BODY Y", new ValueEntry("meter per second squared", 2) },
            { "ACCELERATION BODY Z", new ValueEntry("meter per second squared", 2) },
            { "ACCELERATION WORLD X", new ValueEntry("meter per second squared", 2) },
            { "ACCELERATION WORLD Y", new ValueEntry("meter per second squared", 2) },
            { "ACCELERATION WORLD Z", new ValueEntry("meter per second squared", 2) },
            
            // Engine variables
            { "GENERAL ENG RPM:1", new ValueEntry("Rpm", 1) },
            { "GENERAL ENG RPM:2", new ValueEntry("Rpm", 1) },
            { "GENERAL ENG THROTTLE LEVER POSITION:1", new ValueEntry("Percent", 2) },
            { "GENERAL ENG THROTTLE LEVER POSITION:2", new ValueEntry("Percent", 2) },
            { "ENG FUEL FLOW GPH:1", new ValueEntry("Gallons per hour", 0) },
            { "ENG FUEL FLOW GPH:2", new ValueEntry("Gallons per hour", 0) },
            
            // Flight controls
            { "ELEVATOR POSITION", new ValueEntry("Position", 2) },
            { "AILERON POSITION", new ValueEntry("Position", 2) },
            { "RUDDER POSITION", new ValueEntry("Position", 2) },
            
            // Navigation
            { "PLANE LATITUDE", new ValueEntry("Degrees", 6) },
            { "PLANE LONGITUDE", new ValueEntry("Degrees", 6) },
            { "PLANE ALTITUDE", new ValueEntry("Feet", 2) },
            { "PLANE HEADING DEGREES MAGNETIC", new ValueEntry("Degrees", 2) },
            { "PLANE HEADING DEGREES TRUE", new ValueEntry("Degrees", 2) },
            { "AIRSPEED INDICATED", new ValueEntry("Knots", 2) },
            { "AIRSPEED TRUE", new ValueEntry("Knots", 2) },
            { "GROUND VELOCITY", new ValueEntry("Knots", 2) },
            { "VERTICAL SPEED", new ValueEntry("Feet per minute", 0) },
            
            // Autopilot
            { "AUTOPILOT MASTER", new ValueEntry("Bool", 0) },
            { "AUTOPILOT ALTITUDE LOCK", new ValueEntry("Bool", 0) },
            { "AUTOPILOT ALTITUDE LOCK VAR", new ValueEntry("Feet", 0) },
            { "AUTOPILOT HEADING LOCK", new ValueEntry("Bool", 0) },
            { "AUTOPILOT HEADING LOCK DIR", new ValueEntry("Degrees", 0) },
            { "AUTOPILOT AIRSPEED HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT AIRSPEED HOLD VAR", new ValueEntry("Knots", 0) },
            
            // Radio navigation
            { "NAV ACTIVE FREQUENCY:1", new ValueEntry("MHz", 2) },
            { "NAV ACTIVE FREQUENCY:2", new ValueEntry("MHz", 2) },
            { "COM ACTIVE FREQUENCY:1", new ValueEntry("MHz", 3) },
            { "COM ACTIVE FREQUENCY:2", new ValueEntry("MHz", 3) },
            
            // Weather
            { "AMBIENT TEMPERATURE", new ValueEntry("Celsius", 2) },
            { "AMBIENT PRESSURE", new ValueEntry("inHg", 2) },
            { "AMBIENT WIND VELOCITY", new ValueEntry("Knots", 2) },
            { "AMBIENT WIND DIRECTION", new ValueEntry("Degrees", 0) },
            
            // Fuel
            { "FUEL TOTAL QUANTITY", new ValueEntry("Gallons", 2) },
            { "FUEL LEFT QUANTITY", new ValueEntry("Gallons", 2) },
            { "FUEL RIGHT QUANTITY", new ValueEntry("Gallons", 2) },
            
            // Landing gear
            { "GEAR POSITION", new ValueEntry("Enum", 0) },
            { "GEAR TOTAL PCT EXTENDED", new ValueEntry("Percent", 1) },
            { "GEAR HANDLE POSITION", new ValueEntry("Position", 0) },
            { "BRAKE LEFT POSITION", new ValueEntry("Position", 2) },
            { "BRAKE RIGHT POSITION", new ValueEntry("Position", 2) },
            { "BRAKE PARKING POSITION", new ValueEntry("Position", 2) },
            
            // Flaps and trim
            { "FLAPS HANDLE INDEX", new ValueEntry("Number", 0) },
            { "FLAPS HANDLE PERCENT", new ValueEntry("Percent", 2) },
            { "ELEVATOR TRIM POSITION", new ValueEntry("Radians", 4) },
            { "AILERON TRIM PCT", new ValueEntry("Percent", 2) },
            { "RUDDER TRIM PCT", new ValueEntry("Percent", 2) },
            
            // Time
            { "ZULU TIME", new ValueEntry("Seconds", 0) },
            { "LOCAL TIME", new ValueEntry("Seconds", 0) },
            { "ABSOLUTE TIME", new ValueEntry("Seconds", 0) },
            { "TIME OF DAY", new ValueEntry("Enum", 0) },
            { "SEASON", new ValueEntry("Enum", 0) },
            
            // Engine extended
            { "GENERAL ENG OIL PRESSURE:1", new ValueEntry("psi", 1) },
            { "GENERAL ENG OIL PRESSURE:2", new ValueEntry("psi", 1) },
            { "GENERAL ENG OIL TEMPERATURE:1", new ValueEntry("celsius", 1) },
            { "GENERAL ENG OIL TEMPERATURE:2", new ValueEntry("celsius", 1) },
            { "ENG EXHAUST GAS TEMPERATURE:1", new ValueEntry("celsius", 0) },
            { "ENG EXHAUST GAS TEMPERATURE:2", new ValueEntry("celsius", 0) },
            
            // Navigation extended
            { "GPS GROUND SPEED", new ValueEntry("knots", 1) },
            { "GPS GROUND TRUE TRACK", new ValueEntry("degrees", 1) },
            
            // Autopilot extended
            { "AUTOPILOT AVAILABLE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT FLIGHT DIRECTOR ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT ALTITUDE ARM", new ValueEntry("Bool", 0) },
            { "AUTOPILOT AIRSPEED ACQUISITION", new ValueEntry("Bool", 0) },
            { "AUTOPILOT AIRSPEED MAX CALCULATED", new ValueEntry("knots", 0) },
            { "AUTOPILOT AIRSPEED MIN CALCULATED", new ValueEntry("knots", 0) },
            { "AUTOPILOT VERTICAL HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT VERTICAL HOLD VAR", new ValueEntry("feet per minute", 0) },
            { "AUTOPILOT MACH HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT MACH HOLD VAR", new ValueEntry("mach", 3) },
            { "AUTOPILOT YAW DAMPER", new ValueEntry("Bool", 0) },
            { "AUTOPILOT NAV1 LOCK", new ValueEntry("Bool", 0) },
            { "AUTOPILOT NAV SELECTED", new ValueEntry("Number", 0) },
            { "AUTOPILOT APPROACH HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT APPROACH ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT APPROACH ARM", new ValueEntry("Bool", 0) },
            { "AUTOPILOT APPROACH CAPTURED", new ValueEntry("Bool", 0) },
            { "AUTOPILOT APPROACH IS LOCALIZER", new ValueEntry("Bool", 0) },
            { "AUTOPILOT GLIDESLOPE ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT GLIDESLOPE ARM", new ValueEntry("Bool", 0) },
            { "AUTOPILOT GLIDESLOPE HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT BACKCOURSE HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT WING LEVELER", new ValueEntry("Bool", 0) },
            { "AUTOPILOT ATTITUDE HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT BANK HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT BANK HOLD REF", new ValueEntry("degrees", 1) },
            { "AUTOPILOT PITCH HOLD", new ValueEntry("Bool", 0) },
            { "AUTOPILOT PITCH HOLD REF", new ValueEntry("degrees", 1) },
            { "AUTOPILOT FLIGHT LEVEL CHANGE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT TAKEOFF POWER ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT THROTTLE ARM", new ValueEntry("Bool", 0) },
            { "AUTOPILOT THROTTLE MAX THRUST", new ValueEntry("Bool", 0) },
            { "AUTOPILOT MANAGED THROTTLE ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT MANAGED SPEED IN MACH", new ValueEntry("Bool", 0) },
            { "AUTOPILOT MAX BANK", new ValueEntry("degrees", 0) },
            { "AUTOPILOT MAX BANK ID", new ValueEntry("Number", 0) },
            { "AUTOPILOT FLIGHT DIRECTOR BANK", new ValueEntry("degrees", 1) },
            { "AUTOPILOT FLIGHT DIRECTOR PITCH", new ValueEntry("degrees", 1) },
            { "AUTOPILOT ALT RADIO MODE", new ValueEntry("Bool", 0) },
            { "AUTOPILOT DISENGAGED", new ValueEntry("Bool", 0) },
            { "AUTOPILOT AVIONICS MANAGED", new ValueEntry("Bool", 0) },
            
            // Radio extended
            { "NAV STANDBY FREQUENCY:1", new ValueEntry("Hz", 2) },
            { "NAV STANDBY FREQUENCY:2", new ValueEntry("Hz", 2) },
            { "COM STANDBY FREQUENCY:1", new ValueEntry("Hz", 2) },
            { "COM STANDBY FREQUENCY:2", new ValueEntry("Hz", 2) },
            { "TRANSPONDER CODE:1", new ValueEntry("Number", 0) },
            
            // Weather extended
            { "BAROMETER PRESSURE", new ValueEntry("inHg", 2) },
            { "SEA LEVEL PRESSURE", new ValueEntry("millibars", 2) },
            
            // Fuel extended
            { "FUEL TOTAL CAPACITY", new ValueEntry("gallons", 1) },
            { "FUEL LEFT CAPACITY", new ValueEntry("gallons", 1) },
            { "FUEL RIGHT CAPACITY", new ValueEntry("gallons", 1) },
            
            // Aircraft info
            { "AIRCRAFT TITLE", new ValueEntry("String", 0) },
            { "ATC ID", new ValueEntry("String", 0) },
            { "ATC FLIGHT NUMBER", new ValueEntry("String", 0) },
            { "SIM ON GROUND", new ValueEntry("Bool", 0) },
            
            // Brakes extended
            { "ANTISKID BRAKES ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTOBRAKES ACTIVE", new ValueEntry("Bool", 0) },
            { "AUTO BRAKE SWITCH CB", new ValueEntry("Number", 0) },
            { "BRAKE DEPENDENT HYDRAULIC PRESSURE", new ValueEntry("psi", 0) },
            { "BRAKE INDICATOR", new ValueEntry("Bool", 0) },
            { "BRAKE PARKING INDICATOR", new ValueEntry("Bool", 0) },
            { "REJECTED TAKEOFF BRAKES ACTIVE", new ValueEntry("Bool", 0) },
            { "TOE BRAKES AVAILABLE", new ValueEntry("Bool", 0) },
            
            // Landing gear extended
            { "GEAR LEFT POSITION", new ValueEntry("Percent", 0) },
            { "GEAR RIGHT POSITION", new ValueEntry("Percent", 0) },
            { "GEAR CENTER POSITION", new ValueEntry("Percent", 0) },
            { "GEAR AUX POSITION", new ValueEntry("Percent", 0) },
            { "GEAR POSITION:0", new ValueEntry("Percent", 0) },
            { "GEAR POSITION:1", new ValueEntry("Percent", 0) },
            { "GEAR POSITION:2", new ValueEntry("Percent", 0) },
            { "GEAR IS ON GROUND:0", new ValueEntry("Bool", 0) },
            { "GEAR IS ON GROUND:1", new ValueEntry("Bool", 0) },
            { "GEAR IS ON GROUND:2", new ValueEntry("Bool", 0) },
            { "GEAR DAMAGE BY SPEED", new ValueEntry("Bool", 0) },
            { "GEAR SPEED EXCEEDED", new ValueEntry("Bool", 0) },
            { "IS GEAR RETRACTABLE", new ValueEntry("Bool", 0) },
            { "IS GEAR WHEELS", new ValueEntry("Bool", 0) },
            { "NOSEWHEEL LOCK ON", new ValueEntry("Bool", 0) },
            { "TAILWHEEL LOCK ON", new ValueEntry("Bool", 0) },
            { "LEFT WHEEL RPM", new ValueEntry("rpm", 0) },
            { "RIGHT WHEEL RPM", new ValueEntry("rpm", 0) },
            { "CENTER WHEEL RPM", new ValueEntry("rpm", 0) },
            
            // AI and assistance
            { "AI CONTROLS", new ValueEntry("Bool", 0) },
            { "AI AUTOTRIM ACTIVE", new ValueEntry("Bool", 0) },
            { "DELEGATE CONTROLS TO AI", new ValueEntry("Bool", 0) },
            { "ATTITUDE INDICATOR BANK DEGREES", new ValueEntry("degrees", 1) },
            { "ATTITUDE INDICATOR PITCH DEGREES", new ValueEntry("degrees", 1) },
            { "ATTITUDE BARS POSITION", new ValueEntry("Position", 2) },
            { "ATTITUDE CAGE", new ValueEntry("Bool", 0) },
            { "FLY ASSISTANT STALL SPEED", new ValueEntry("knots", 0) },
            { "FLY ASSISTANT TAKEOFF SPEED", new ValueEntry("knots", 0) },
            { "FLY ASSISTANT LANDING SPEED", new ValueEntry("knots", 0) },
            { "ASSISTANCE LANDING ENABLED", new ValueEntry("Bool", 0) },
            { "ASSISTANCE TAKEOFF ENABLED", new ValueEntry("Bool", 0) },
            
            // Electrical extended
            { "ELECTRICAL MASTER BATTERY", new ValueEntry("Bool", 0) },
            { "ELECTRICAL MAIN BUS VOLTAGE", new ValueEntry("volts", 1) },
            { "ELECTRICAL MAIN BUS AMPS", new ValueEntry("amps", 1) },
            { "ELECTRICAL BATTERY BUS VOLTAGE", new ValueEntry("volts", 1) },
            { "ELECTRICAL AVIONICS MASTER", new ValueEntry("Bool", 0) },
            { "ELECTRICAL AVIONICS BUS VOLTAGE", new ValueEntry("volts", 1) },
            { "GENERAL ENG MASTER ALTERNATOR:1", new ValueEntry("Bool", 0) },
            { "GENERAL ENG MASTER ALTERNATOR:2", new ValueEntry("Bool", 0) },
            { "BATTERY SWITCH:1", new ValueEntry("Bool", 0) },
            { "EXTERNAL POWER AVAILABLE:1", new ValueEntry("Bool", 0) },
            { "EXTERNAL POWER ON:1", new ValueEntry("Bool", 0) },
            
            // Warnings
            { "MASTER CAUTION ACTIVE", new ValueEntry("Bool", 0) },
            { "MASTER WARNING ACTIVE", new ValueEntry("Bool", 0) },
            
            // Simulation
            { "SIMULATION RATE", new ValueEntry("Number", 1) }
        };
    }

    public static bool IsKnown(this string value)
        => availableValues.ContainsKey(value);

    public static string GetUnit(this string value, string? unit)
    {
        if (!string.IsNullOrEmpty(unit))
        {
            // TODO: add some validation
            return unit;
        }
        else if (availableValues.TryGetValue(value, out var availableEntry) && availableEntry != null)
        {
            return availableEntry.Unit;
        }
        return DEFAULT_UNIT;
    }

    public static int GetDecimals(this string value, int? decimals = null)
    {
        if (decimals.HasValue)
        {
            return decimals.Value;
        }
        else if (availableValues.TryGetValue(value, out var availableEntry) && availableEntry != null)
        {
            return availableEntry.Decimals;
        }

        return DEFAULT_DECIMALS;
    }
}