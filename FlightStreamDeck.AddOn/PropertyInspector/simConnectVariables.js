// SimConnect Variables Helper for Flight Stream Deck
// This file contains common SimConnect variables organized by category for easy selection

const SimConnectVariablesData = {
    // Common display variables organized by functional groups
    groups: [
        {
            name: "Engine",
            variables: [
                "GENERAL ENG RPM:1",
                "GENERAL ENG RPM:2",
                "GENERAL ENG THROTTLE LEVER POSITION:1",
                "GENERAL ENG THROTTLE LEVER POSITION:2",
                "ENG FUEL FLOW GPH:1",
                "ENG FUEL FLOW GPH:2",
                "GENERAL ENG OIL PRESSURE:1",
                "GENERAL ENG OIL PRESSURE:2",
                "GENERAL ENG OIL TEMPERATURE:1",
                "GENERAL ENG OIL TEMPERATURE:2",
                "ENG EXHAUST GAS TEMPERATURE:1",
                "ENG EXHAUST GAS TEMPERATURE:2"
            ]
        },
        {
            name: "Navigation & Position", 
            variables: [
                "PLANE LATITUDE",
                "PLANE LONGITUDE", 
                "PLANE ALTITUDE",
                "PLANE HEADING DEGREES MAGNETIC",
                "PLANE HEADING DEGREES TRUE",
                "AIRSPEED INDICATED",
                "AIRSPEED TRUE",
                "GROUND VELOCITY",
                "VERTICAL SPEED",
                "GPS GROUND SPEED",
                "GPS GROUND TRUE TRACK"
            ]
        },
        {
            name: "Autopilot",
            variables: [
                "AUTOPILOT MASTER",
                "AUTOPILOT AVAILABLE",
                "AUTOPILOT FLIGHT DIRECTOR ACTIVE",
                "AUTOPILOT ALTITUDE LOCK",
                "AUTOPILOT ALTITUDE LOCK VAR",
                "AUTOPILOT ALTITUDE ARM",
                "AUTOPILOT HEADING LOCK",
                "AUTOPILOT HEADING LOCK DIR",
                "AUTOPILOT AIRSPEED HOLD",
                "AUTOPILOT AIRSPEED HOLD VAR",
                "AUTOPILOT AIRSPEED ACQUISITION",
                "AUTOPILOT AIRSPEED MAX CALCULATED",
                "AUTOPILOT AIRSPEED MIN CALCULATED",
                "AUTOPILOT VERTICAL HOLD",
                "AUTOPILOT VERTICAL HOLD VAR",
                "AUTOPILOT MACH HOLD",
                "AUTOPILOT MACH HOLD VAR",
                "AUTOPILOT YAW DAMPER",
                "AUTOPILOT NAV1 LOCK",
                "AUTOPILOT NAV SELECTED",
                "AUTOPILOT APPROACH HOLD",
                "AUTOPILOT APPROACH ACTIVE",
                "AUTOPILOT APPROACH ARM",
                "AUTOPILOT APPROACH CAPTURED",
                "AUTOPILOT APPROACH IS LOCALIZER",
                "AUTOPILOT GLIDESLOPE ACTIVE",
                "AUTOPILOT GLIDESLOPE ARM",
                "AUTOPILOT GLIDESLOPE HOLD",
                "AUTOPILOT BACKCOURSE HOLD",
                "AUTOPILOT WING LEVELER",
                "AUTOPILOT ATTITUDE HOLD",
                "AUTOPILOT BANK HOLD",
                "AUTOPILOT BANK HOLD REF",
                "AUTOPILOT PITCH HOLD",
                "AUTOPILOT PITCH HOLD REF",
                "AUTOPILOT FLIGHT LEVEL CHANGE",
                "AUTOPILOT TAKEOFF POWER ACTIVE",
                "AUTOPILOT THROTTLE ARM",
                "AUTOPILOT THROTTLE MAX THRUST",
                "AUTOPILOT MANAGED THROTTLE ACTIVE",
                "AUTOPILOT MANAGED SPEED IN MACH",
                "AUTOPILOT MAX BANK",
                "AUTOPILOT MAX BANK ID",
                "AUTOPILOT FLIGHT DIRECTOR BANK",
                "AUTOPILOT FLIGHT DIRECTOR PITCH",
                "AUTOPILOT ALT RADIO MODE",
                "AUTOPILOT DISENGAGED",
                "AUTOPILOT AVIONICS MANAGED"
            ]
        },
        {
            name: "Radio & Navigation",
            variables: [
                "NAV ACTIVE FREQUENCY:1",
                "NAV ACTIVE FREQUENCY:2",
                "COM ACTIVE FREQUENCY:1",
                "COM ACTIVE FREQUENCY:2",
                "NAV STANDBY FREQUENCY:1",
                "NAV STANDBY FREQUENCY:2",
                "COM STANDBY FREQUENCY:1",
                "COM STANDBY FREQUENCY:2",
                "TRANSPONDER CODE:1"
            ]
        },
        {
            name: "Weather",
            variables: [
                "AMBIENT TEMPERATURE",
                "AMBIENT PRESSURE",
                "AMBIENT WIND VELOCITY",
                "AMBIENT WIND DIRECTION",
                "BAROMETER PRESSURE",
                "SEA LEVEL PRESSURE"
            ]
        },
        {
            name: "Fuel",
            variables: [
                "FUEL TOTAL QUANTITY",
                "FUEL LEFT QUANTITY", 
                "FUEL RIGHT QUANTITY",
                "FUEL TOTAL CAPACITY",
                "FUEL LEFT CAPACITY",
                "FUEL RIGHT CAPACITY"
            ]
        },
        {
            name: "Flight Controls",
            variables: [
                "ELEVATOR POSITION",
                "AILERON POSITION", 
                "RUDDER POSITION",
                "FLAPS HANDLE INDEX",
                "FLAPS HANDLE PERCENT",
                "ELEVATOR TRIM POSITION",
                "AILERON TRIM PCT",
                "RUDDER TRIM PCT",
                "GEAR POSITION"
            ]
        },
        {
            name: "Time & Simulation",
            variables: [
                "ZULU TIME",
                "LOCAL TIME",
                "SIMULATION_RATE",
                "ABSOLUTE TIME",
                "TIME OF DAY",
                "SEASON",
                "AIRCRAFT TITLE",
                "ATC ID",
                "ATC FLIGHT NUMBER",
                "SIM ON GROUND"
            ]
        },
        {
            name: "Brakes & Landing Gear",
            variables: [
                "ANTISKID BRAKES ACTIVE",
                "AUTOBRAKES ACTIVE",
                "AUTO BRAKE SWITCH CB",
                "BRAKE DEPENDENT HYDRAULIC PRESSURE",
                "BRAKE INDICATOR",
                "BRAKE LEFT POSITION",
                "BRAKE RIGHT POSITION",
                "BRAKE PARKING INDICATOR",
                "BRAKE PARKING POSITION",
                "REJECTED TAKEOFF BRAKES ACTIVE",
                "TOE BRAKES AVAILABLE",
                "GEAR HANDLE POSITION",
                "GEAR TOTAL PCT EXTENDED",
                "GEAR LEFT POSITION",
                "GEAR RIGHT POSITION",
                "GEAR CENTER POSITION",
                "GEAR AUX POSITION",
                "GEAR POSITION:0",
                "GEAR POSITION:1",
                "GEAR POSITION:2",
                "GEAR IS ON GROUND:0",
                "GEAR IS ON GROUND:1",
                "GEAR IS ON GROUND:2",
                "GEAR DAMAGE BY SPEED",
                "GEAR SPEED EXCEEDED",
                "IS GEAR RETRACTABLE",
                "IS GEAR WHEELS",
                "NOSEWHEEL LOCK ON",
                "TAILWHEEL LOCK ON",
                "LEFT WHEEL RPM",
                "RIGHT WHEEL RPM",
                "CENTER WHEEL RPM"
            ]
        },
        {
            name: "AI & Flight Assistant",
            variables: [
                "AI CONTROLS",
                "AI AUTOTRIM ACTIVE", 
                "DELEGATE CONTROLS TO AI",
                "ATTITUDE INDICATOR BANK DEGREES",
                "ATTITUDE INDICATOR PITCH DEGREES",
                "ATTITUDE BARS POSITION",
                "ATTITUDE CAGE",
                "FLY ASSISTANT STALL SPEED",
                "FLY ASSISTANT TAKEOFF SPEED",
                "FLY ASSISTANT LANDING SPEED",
                "ASSISTANCE LANDING ENABLED",
                "ASSISTANCE TAKEOFF ENABLED"
            ]
        },
        {
            name: "Electrical & Systems",
            variables: [
                "ELECTRICAL MASTER BATTERY",
                "ELECTRICAL MAIN BUS VOLTAGE",
                "ELECTRICAL MAIN BUS AMPS",
                "ELECTRICAL BATTERY BUS VOLTAGE", 
                "ELECTRICAL AVIONICS MASTER",
                "ELECTRICAL AVIONICS BUS VOLTAGE",
                "GENERAL ENG MASTER ALTERNATOR:1",
                "GENERAL ENG MASTER ALTERNATOR:2",
                "BATTERY SWITCH:1",
                "EXTERNAL POWER AVAILABLE:1",
                "EXTERNAL POWER ON:1"
            ]
        },
        {
            name: "Warnings & Cautions",
            variables: [
                "MASTER CAUTION ACTIVE",
                "MASTER WARNING ACTIVE"
            ]
        }
    ],

    // Search functionality
    search: function(query) {
        const results = [];
        const searchTerms = query.toLowerCase().split(' ');
        
        this.groups.forEach(group => {
            group.variables.forEach(variable => {
                const variableLower = variable.toLowerCase();
                const matches = searchTerms.every(term => 
                    variableLower.includes(term) || 
                    group.name.toLowerCase().includes(term)
                );
                
                if (matches) {
                    results.push({
                        variable: variable,
                        group: group.name
                    });
                }
            });
        });
        
        return results;
    },

    // Helper function to populate a select element with grouped options
    populateSelect: function(selectElement, includeSearch = false) {
        // Clear existing options
        selectElement.innerHTML = '<option value="">Choose Variable...</option>';
        
        if (includeSearch) {
            const searchOption = document.createElement('option');
            searchOption.value = 'SEARCH';
            searchOption.textContent = '🔍 Type to search...';
            selectElement.appendChild(searchOption);
        }
        
        this.groups.forEach(group => {
            const optgroup = document.createElement('optgroup');
            optgroup.label = group.name;
            
            group.variables.forEach(variable => {
                const option = document.createElement('option');
                option.value = variable;
                option.textContent = variable;
                optgroup.appendChild(option);
            });
            
            selectElement.appendChild(optgroup);
        });
    },

    // Helper function to create input with dropdown helper
    createVariableInput: function(inputElement, onChangeCallback) {
        const wrapper = inputElement.parentElement;
        
        // Create helper button
        const helperBtn = document.createElement('button');
        helperBtn.type = 'button';
        helperBtn.className = 'sdpi-item-value sdpi-helper-btn';
        helperBtn.innerHTML = '▼';
        helperBtn.style.cssText = 'width: 30px; margin-left: 4px; padding: 0; font-size: 12px;';
        
        // Create dropdown
        const dropdown = document.createElement('select');
        dropdown.className = 'sdpi-item-value';
        dropdown.style.cssText = 'display: none; position: absolute; z-index: 1000; width: calc(100% - 34px);';
        
        this.populateSelect(dropdown, true);
        
        // Add elements to DOM
        wrapper.appendChild(helperBtn);
        wrapper.appendChild(dropdown);
        
        // Helper button click handler
        helperBtn.onclick = (e) => {
            e.preventDefault();
            const isVisible = dropdown.style.display !== 'none';
            dropdown.style.display = isVisible ? 'none' : 'block';
            if (!isVisible) {
                dropdown.focus();
            }
        };
        
        // Dropdown change handler
        dropdown.onchange = () => {
            if (dropdown.value && dropdown.value !== 'SEARCH') {
                inputElement.value = dropdown.value;
                dropdown.style.display = 'none';
                if (onChangeCallback) onChangeCallback();
            }
        };
        
        // Hide dropdown when clicking outside
        document.addEventListener('click', (e) => {
            if (!wrapper.contains(e.target)) {
                dropdown.style.display = 'none';
            }
        });
        
        return { helperBtn, dropdown };
    }
};

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = SimConnectVariablesData;
}