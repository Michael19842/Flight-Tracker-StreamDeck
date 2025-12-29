// Toggle Events Helper for Flight Stream Deck
// This file contains common toggle events organized by category for easy selection

const ToggleEventsData = {
    // Common toggle events organized by functional groups
    groups: [
        {
            name: "Autopilot",
            events: [
                "TOGGLE_FLIGHT_DIRECTOR",
                "YAW_DAMPER_TOGGLE",
                "AP_PANEL_SPEED_HOLD_TOGGLE",
                "AP_PANEL_MACH_HOLD_TOGGLE",
                "FLY_BY_WIRE_ELAC_TOGGLE",
                "FLY_BY_WIRE_FAC_TOGGLE",
                "FLY_BY_WIRE_SEC_TOGGLE"
            ]
        },
        {
            name: "Engine & Fuel",
            events: [
                "TOGGLE_STARTER1",
                "TOGGLE_STARTER2",
                "TOGGLE_STARTER3",
                "TOGGLE_STARTER4",
                "TOGGLE_ALL_STARTERS",
                "TOGGLE_FUEL_VALVE_ALL",
                "TOGGLE_FUEL_VALVE_ENG1",
                "TOGGLE_FUEL_VALVE_ENG2",
                "TOGGLE_FUEL_VALVE_ENG3",
                "TOGGLE_FUEL_VALVE_ENG4",
                "TOGGLE_ELECT_FUEL_PUMP",
                "TOGGLE_ELECT_FUEL_PUMP1",
                "TOGGLE_ELECT_FUEL_PUMP2",
                "TOGGLE_ELECT_FUEL_PUMP3",
                "TOGGLE_ELECT_FUEL_PUMP4",
                "CROSS_FEED_TOGGLE",
                "FUEL_DUMP_TOGGLE",
                "TOGGLE_PRIMER",
                "TOGGLE_PRIMER1",
                "TOGGLE_PRIMER2",
                "TOGGLE_PRIMER3",
                "TOGGLE_PRIMER4"
            ]
        },
        {
            name: "Flight Controls",
            events: [
                "SPOILERS_TOGGLE",
                "SPOILERS_ARM_TOGGLE",
                "TOGGLE_FEATHER_SWITCHES",
                "TOGGLE_FEATHER_SWITCH_1",
                "TOGGLE_FEATHER_SWITCH_2",
                "TOGGLE_FEATHER_SWITCH_3",
                "TOGGLE_FEATHER_SWITCH_4",
                "TOGGLE_PROPELLER_SYNC",
                "TOGGLE_AUTOFEATHER_ARM",
                "GEAR_TOGGLE",
                "WATER_RUDDER_TOGGLE",
                "TOGGLE_TAIL_HOOK_HANDLE",
                "TOGGLE_WING_FOLD",
                "CANOPY_TOGGLE"
            ]
        },
        {
            name: "Lights",
            events: [
                "LANDING_LIGHTS_TOGGLE",
                "TAXI_LIGHTS_TOGGLE",
                "NAV_LIGHTS_TOGGLE",
                "BEACON_LIGHTS_TOGGLE",
                "STROBE_LIGHTS_TOGGLE",
                "PANEL_LIGHTS_TOGGLE",
                "LOGO_LIGHTS_TOGGLE",
                "WING_LIGHTS_TOGGLE",
                "RECOGNITION_LIGHTS_TOGGLE",
                "CABIN_LIGHTS_TOGGLE"
            ]
        },
        {
            name: "Anti-Ice & Environmental",
            events: [
                "ANTI_ICE_TOGGLE",
                "ANTI_ICE_TOGGLE_ENG1",
                "ANTI_ICE_TOGGLE_ENG2",
                "ANTI_ICE_TOGGLE_ENG3",
                "ANTI_ICE_TOGGLE_ENG4",
                "PITOT_HEAT_TOGGLE",
                "TOGGLE_STRUCTURAL_DEICE",
                "WINDSHIELD_DEICE_TOGGLE",
                "PROP_DEICE_TOGGLE"
            ]
        },
        {
            name: "Electrical & Avionics",
            events: [
                "TOGGLE_MASTER_BATTERY",
                "TOGGLE_MASTER_ALTERNATOR",
                "TOGGLE_AVIONICS_MASTER",
                "TOGGLE_ELECTRIC_VACUUM_PUMP",
                "TOGGLE_ALTERNATOR1",
                "TOGGLE_ALTERNATOR2",
                "TOGGLE_ALTERNATOR3",
                "TOGGLE_ALTERNATOR4",
                "APU_STARTER",
                "APU_OFF_SWITCH"
            ]
        },
        {
            name: "Navigation & Radio",
            events: [
                "DME1_TOGGLE",
                "DME2_TOGGLE",
                "COM_STBY_RADIO_SWAP",
                "COM1_RADIO_SWAP",
                "COM2_RADIO_SWAP",
                "NAV1_RADIO_SWAP",
                "NAV2_RADIO_SWAP",
                "ADF_CARD_SET",
                "GPS_POWER_BUTTON",
                "TRANSPONDER_IDENT_TOGGLE"
            ]
        },
        {
            name: "Pressurization & Air Systems",
            events: [
                "CABIN_PRESSURE_DUMP_SWITCH",
                "BLEED_AIR_SOURCE_CONTROL_TOGGLE",
                "APU_BLEED_AIR_SOURCE_TOGGLE"
            ]
        },
        {
            name: "Afterburner & Special",
            events: [
                "TOGGLE_AFTERBURNER",
                "TOGGLE_AFTERBURNER1",
                "TOGGLE_AFTERBURNER2",
                "TOGGLE_AFTERBURNER3",
                "TOGGLE_AFTERBURNER4",
                "SMOKE_TOGGLE"
            ]
        },
        {
            name: "Brakes & Ground",
            events: [
                "PARKING_BRAKES",
                "GEAR_PUMP"
            ]
        },
        {
            name: "Autopilot Flight Assist",
            events: [
                "AP_MASTER",
                "AUTOPILOT_ON",
                "AUTOPILOT_OFF",
                "AUTOPILOT_TOGGLE",
                "AP_PANEL_HEADING_HOLD",
                "AP_PANEL_ALTITUDE_HOLD",
                "AP_PANEL_SPEED_HOLD",
                "AP_PANEL_VERTICAL_SPEED_HOLD",
                "AP_NAV1_HOLD",
                "AP_LOC_HOLD",
                "AP_APR_HOLD",
                "AP_HDG_HOLD",
                "AP_ALT_HOLD",
                "AP_WING_LEVELER",
                "AP_BC_HOLD",
                "AP_ATT_HOLD",
                "HEADING_BUG_INC",
                "HEADING_BUG_DEC",
                "HEADING_BUG_SET",
                "AP_ALT_VAR_INC",
                "AP_ALT_VAR_DEC",
                "AP_ALT_VAR_SET_ENGLISH",
                "AP_VS_VAR_INC",
                "AP_VS_VAR_DEC",
                "AP_VS_VAR_SET_ENGLISH",
                "AP_SPD_VAR_INC",
                "AP_SPD_VAR_DEC",
                "AP_SPD_VAR_SET",
                "YAW_DAMPER_ON",
                "YAW_DAMPER_OFF",
                "YAW_DAMPER_SET",
                "AP_FLIGHT_LEVEL_CHANGE",
                "FLIGHT_LEVEL_CHANGE",
                "FLIGHT_LEVEL_CHANGE_ON",
                "FLIGHT_LEVEL_CHANGE_OFF"
            ]
        },
        {
            name: "Engine Advanced",
            events: [
                "MAGNETO",
                "MAGNETO_BOTH",
                "MAGNETO_LEFT",
                "MAGNETO_RIGHT",
                "MAGNETO_OFF",
                "MAGNETO_START",
                "MAGNETO1_BOTH",
                "MAGNETO1_LEFT",
                "MAGNETO1_RIGHT",
                "MAGNETO1_OFF",
                "MAGNETO1_START",
                "MAGNETO2_BOTH",
                "MAGNETO2_LEFT",
                "MAGNETO2_RIGHT",
                "MAGNETO2_OFF",
                "MAGNETO2_START",
                "MAGNETO3_BOTH",
                "MAGNETO3_LEFT",
                "MAGNETO3_RIGHT",
                "MAGNETO3_OFF",
                "MAGNETO3_START",
                "MAGNETO4_BOTH",
                "MAGNETO4_LEFT",
                "MAGNETO4_RIGHT",
                "MAGNETO4_OFF",
                "MAGNETO4_START",
                "STARTER1_SET",
                "STARTER2_SET",
                "STARTER3_SET",
                "STARTER4_SET",
                "ENGINE",
                "ENGINE_PRIME",
                "ENGINE_AUTO_START",
                "ENGINE_AUTO_SHUTDOWN",
                "JET_STARTER",
                "TURBINE_IGNITION_SWITCH_TOGGLE",
                "TURBINE_IGNITION_SWITCH_SET1",
                "TURBINE_IGNITION_SWITCH_SET2",
                "FUEL_PUMP",
                "THROTTLE_SET",
                "THROTTLE1_SET",
                "THROTTLE2_SET",
                "THROTTLE3_SET",
                "THROTTLE4_SET",
                "THROTTLE_INCR",
                "THROTTLE_DECR",
                "THROTTLE_INCR_SMALL",
                "THROTTLE_DECR_SMALL",
                "THROTTLE_FULL",
                "THROTTLE_CUT",
                "THROTTLE1_FULL",
                "THROTTLE1_CUT",
                "THROTTLE2_FULL",
                "THROTTLE2_CUT",
                "THROTTLE3_FULL",
                "THROTTLE3_CUT",
                "THROTTLE4_FULL",
                "THROTTLE4_CUT",
                "MIXTURE_SET",
                "MIXTURE1_SET",
                "MIXTURE2_SET",
                "MIXTURE3_SET",
                "MIXTURE4_SET",
                "MIXTURE_RICH",
                "MIXTURE_LEAN",
                "MIXTURE1_RICH",
                "MIXTURE1_LEAN",
                "MIXTURE2_RICH",
                "MIXTURE2_LEAN",
                "MIXTURE3_RICH",
                "MIXTURE3_LEAN",
                "MIXTURE4_RICH",
                "MIXTURE4_LEAN",
                "PROP_PITCH_SET",
                "PROP_PITCH1_SET",
                "PROP_PITCH2_SET",
                "PROP_PITCH3_SET",
                "PROP_PITCH4_SET",
                "PROP_PITCH_INCR",
                "PROP_PITCH_DECR",
                "PROP_PITCH1_INCR",
                "PROP_PITCH1_DECR",
                "PROP_PITCH2_INCR",
                "PROP_PITCH2_DECR",
                "PROP_PITCH3_INCR",
                "PROP_PITCH3_DECR",
                "PROP_PITCH4_INCR",
                "PROP_PITCH4_DECR"
            ]
        },
        {
            name: "Radio & Navigation Advanced",
            events: [
                "NAV1_RADIO_FRACT_DEC",
                "NAV1_RADIO_FRACT_INC",
                "NAV1_RADIO_WHOLE_DEC",
                "NAV1_RADIO_WHOLE_INC",
                "NAV1_RADIO_SET",
                "NAV2_RADIO_FRACT_DEC",
                "NAV2_RADIO_FRACT_INC",
                "NAV2_RADIO_WHOLE_DEC",
                "NAV2_RADIO_WHOLE_INC",
                "NAV2_RADIO_SET",
                "COM_RADIO_FRACT_DEC",
                "COM_RADIO_FRACT_INC",
                "COM_RADIO_WHOLE_DEC",
                "COM_RADIO_WHOLE_INC",
                "COM_RADIO_SET",
                "COM2_RADIO_FRACT_DEC",
                "COM2_RADIO_FRACT_INC",
                "COM2_RADIO_WHOLE_DEC",
                "COM2_RADIO_WHOLE_INC",
                "COM2_RADIO_SET",
                "ADF_FRACT_DEC",
                "ADF_FRACT_INC",
                "ADF_WHOLE_DEC",
                "ADF_WHOLE_INC",
                "ADF_SET",
                "ADF_100_INC",
                "ADF_100_DEC",
                "ADF_10_INC",
                "ADF_10_DEC",
                "ADF_1_INC",
                "ADF_1_DEC",
                "XPNDR_SET",
                "XPNDR_INC",
                "XPNDR_DEC",
                "XPNDR_1_INC",
                "XPNDR_1_DEC",
                "XPNDR_10_INC",
                "XPNDR_10_DEC",
                "XPNDR_100_INC",
                "XPNDR_100_DEC",
                "XPNDR_1000_INC",
                "XPNDR_1000_DEC",
                "VOR1_OBI_DEC",
                "VOR1_OBI_INC",
                "VOR2_OBI_DEC",
                "VOR2_OBI_INC",
                "ADF_CARD_DEC",
                "ADF_CARD_INC",
                "RADIO_VOR1_IDENT_DISABLE",
                "RADIO_VOR1_IDENT_ENABLE",
                "RADIO_VOR1_IDENT_SET",
                "RADIO_VOR1_IDENT_TOGGLE",
                "RADIO_VOR2_IDENT_DISABLE",
                "RADIO_VOR2_IDENT_ENABLE",
                "RADIO_VOR2_IDENT_SET",
                "RADIO_VOR2_IDENT_TOGGLE",
                "RADIO_DME1_IDENT_DISABLE",
                "RADIO_DME1_IDENT_ENABLE",
                "RADIO_DME1_IDENT_SET",
                "RADIO_DME1_IDENT_TOGGLE",
                "RADIO_DME2_IDENT_DISABLE",
                "RADIO_DME2_IDENT_ENABLE",
                "RADIO_DME2_IDENT_SET",
                "RADIO_DME2_IDENT_TOGGLE",
                "RADIO_ADF_IDENT_DISABLE",
                "RADIO_ADF_IDENT_ENABLE",
                "RADIO_ADF_IDENT_SET",
                "RADIO_ADF_IDENT_TOGGLE"
            ]
        },
        {
            name: "Flight Controls Advanced",
            events: [
                "FLAPS_UP",
                "FLAPS_DOWN",
                "FLAPS_1",
                "FLAPS_2",
                "FLAPS_3",
                "FLAPS_SET",
                "FLAPS_INCR",
                "FLAPS_DECR",
                "GEAR_UP",
                "GEAR_DOWN",
                "BRAKES",
                "BRAKES_LEFT",
                "BRAKES_RIGHT",
                "SPOILERS_ON",
                "SPOILERS_OFF",
                "SPOILERS_ARM_ON",
                "SPOILERS_ARM_OFF",
                "SPOILERS_SET",
                "TRIM_ELEVATOR_UP",
                "TRIM_ELEVATOR_DOWN",
                "TRIM_RUDDER_LEFT",
                "TRIM_RUDDER_RIGHT",
                "TRIM_AILERON_LEFT",
                "TRIM_AILERON_RIGHT",
                "RUDDER_CENTER",
                "AILERON_CENTER",
                "ELEVATOR_CENTER",
                "RUDDER_TRIM_RESET",
                "AILERON_TRIM_RESET",
                "ELEVATOR_TRIM_RESET",
                "ELEVATOR_TRIM_SET",
                "AILERON_TRIM_SET",
                "RUDDER_TRIM_SET",
                "CENTER_AILER_RUDDER",
                "ELEV_DOWN",
                "ELEV_UP",
                "AILERONS_LEFT",
                "AILERONS_RIGHT",
                "RUDDER_LEFT",
                "RUDDER_RIGHT"
            ]
        },
        {
            name: "Instrumentation",
            events: [
                "BAROMETRIC",
                "KOHLSMAN_INC",
                "KOHLSMAN_DEC",
                "KOHLSMAN_SET",
                "GYRO_DRIFT_INC",
                "GYRO_DRIFT_DEC",
                "ATTITUDE_BARS_POSITION_UP",
                "ATTITUDE_BARS_POSITION_DOWN",
                "ATTITUDE_CAGE_BUTTON",
                "HEADING_GYRO_SET",
                "TOGGLE_GPS_DRIVES_NAV1",
                "DME_TOGGLE",
                "EGT",
                "EGT_INC",
                "EGT_DEC",
                "EGT_SET",
                "EGT1_INC",
                "EGT1_DEC",
                "EGT1_SET",
                "EGT2_INC",
                "EGT2_DEC",
                "EGT2_SET",
                "EGT3_INC",
                "EGT3_DEC",
                "EGT3_SET",
                "EGT4_INC",
                "EGT4_DEC",
                "EGT4_SET",
                "VARIOMETER_SOUND_TOGGLE"
            ]
        },
        {
            name: "Fuel System Advanced",
            events: [
                "FUEL_SELECTOR_OFF",
                "FUEL_SELECTOR_ALL",
                "FUEL_SELECTOR_LEFT",
                "FUEL_SELECTOR_RIGHT",
                "FUEL_SELECTOR_LEFT_AUX",
                "FUEL_SELECTOR_RIGHT_AUX",
                "FUEL_SELECTOR_CENTER",
                "FUEL_SELECTOR_SET",
                "FUEL_SELECTOR_2_OFF",
                "FUEL_SELECTOR_2_ALL",
                "FUEL_SELECTOR_2_LEFT",
                "FUEL_SELECTOR_2_RIGHT",
                "FUEL_SELECTOR_2_LEFT_AUX",
                "FUEL_SELECTOR_2_RIGHT_AUX",
                "FUEL_SELECTOR_2_CENTER",
                "FUEL_SELECTOR_2_SET",
                "FUEL_SELECTOR_3_OFF",
                "FUEL_SELECTOR_3_ALL",
                "FUEL_SELECTOR_3_LEFT",
                "FUEL_SELECTOR_3_RIGHT",
                "FUEL_SELECTOR_3_LEFT_AUX",
                "FUEL_SELECTOR_3_RIGHT_AUX",
                "FUEL_SELECTOR_3_CENTER",
                "FUEL_SELECTOR_3_SET",
                "FUEL_SELECTOR_4_OFF",
                "FUEL_SELECTOR_4_ALL",
                "FUEL_SELECTOR_4_LEFT",
                "FUEL_SELECTOR_4_RIGHT",
                "FUEL_SELECTOR_4_LEFT_AUX",
                "FUEL_SELECTOR_4_RIGHT_AUX",
                "FUEL_SELECTOR_4_CENTER",
                "FUEL_SELECTOR_4_SET",
                "CROSS_FEED_OPEN",
                "CROSS_FEED_OFF",
                "FUEL_TRANSFER_AFT",
                "FUEL_TRANSFER_FORWARD",
                "FUEL_TRANSFER_AUTO",
                "FUEL_TRANSFER_OFF",
                "ADD_FUEL_QUANTITY",
                "FUEL_DUMP_SWITCH_SET"
            ]
        },
        {
            name: "Lights Advanced",
            events: [
                "LANDING_LIGHTS_ON",
                "LANDING_LIGHTS_OFF",
                "LANDING_LIGHTS_SET",
                "TAXI_LIGHTS_ON",
                "TAXI_LIGHTS_OFF",
                "TAXI_LIGHTS_SET",
                "NAV_LIGHTS_ON",
                "NAV_LIGHTS_OFF",
                "NAV_LIGHTS_SET",
                "BEACON_LIGHTS_ON",
                "BEACON_LIGHTS_OFF",
                "BEACON_LIGHTS_SET",
                "STROBES_ON",
                "STROBES_OFF",
                "STROBES_SET",
                "PANEL_LIGHTS_ON",
                "PANEL_LIGHTS_OFF",
                "PANEL_LIGHTS_SET",
                "CABIN_LIGHTS_ON",
                "CABIN_LIGHTS_OFF",
                "CABIN_LIGHTS_SET",
                "LOGO_LIGHTS_ON",
                "LOGO_LIGHTS_OFF",
                "LOGO_LIGHTS_SET",
                "WING_LIGHTS_ON",
                "WING_LIGHTS_OFF",
                "WING_LIGHTS_SET",
                "RECOGNITION_LIGHTS_ON",
                "RECOGNITION_LIGHTS_OFF",
                "RECOGNITION_LIGHTS_SET",
                "LANDING_LIGHT_UP",
                "LANDING_LIGHT_DOWN",
                "LANDING_LIGHT_LEFT",
                "LANDING_LIGHT_RIGHT",
                "LANDING_LIGHT_HOME"
            ]
        },
        {
            name: "Electrical Advanced",
            events: [
                "MASTER_BATTERY_ON",
                "MASTER_BATTERY_OFF",
                "MASTER_ALTERNATOR_ON",
                "MASTER_ALTERNATOR_OFF",
                "AVIONICS_MASTER_ON",
                "AVIONICS_MASTER_OFF",
                "BATTERY1_SET",
                "BATTERY2_SET",
                "BATTERY3_SET",
                "BATTERY4_SET",
                "ALTERNATOR1_SET",
                "ALTERNATOR2_SET",
                "ALTERNATOR3_SET",
                "ALTERNATOR4_SET",
                "EXTERNAL_POWER_ON",
                "EXTERNAL_POWER_OFF",
                "EXTERNAL_POWER_TOGGLE",
                "APU_GENERATOR_SWITCH_TOGGLE",
                "APU_GENERATOR_SWITCH_ON",
                "APU_GENERATOR_SWITCH_OFF"
            ]
        },
        {
            name: "View & Camera",
            events: [
                "VIEW_MODE",
                "COCKPIT_VIEW",
                "VIRTUAL_COPILOT_VIEW",
                "TOWER_VIEW",
                "CHASE_VIEW",
                "NEXT_SUB_VIEW",
                "PREV_SUB_VIEW",
                "VIEW_FORWARD",
                "VIEW_FORWARD_RIGHT",
                "VIEW_RIGHT",
                "VIEW_REAR_RIGHT",
                "VIEW_REAR",
                "VIEW_REAR_LEFT",
                "VIEW_LEFT",
                "VIEW_FORWARD_LEFT",
                "VIEW_UP",
                "VIEW_DOWN",
                "ZOOM_IN",
                "ZOOM_OUT",
                "ZOOM_1X",
                "ZOOM_IN_FINE",
                "ZOOM_OUT_FINE",
                "PAN_LEFT",
                "PAN_RIGHT",
                "PAN_UP",
                "PAN_DOWN",
                "PAN_LEFT_UP",
                "PAN_LEFT_DOWN",
                "PAN_RIGHT_UP",
                "PAN_RIGHT_DOWN",
                "PAN_RESET",
                "PAN_RESET_COCKPIT",
                "VIEW_AXIS_INDICATOR_CYCLE"
            ]
        },
        {
            name: "Simulation Control",
            events: [
                "SIM_RATE_INCR",
                "SIM_RATE_DECR",
                "SIM_RATE_SET",
                "PAUSE_ON",
                "PAUSE_OFF",
                "PAUSE_TOGGLE",
                "SLEW_ON",
                "SLEW_OFF",
                "SLEW_TOGGLE",
                "SLEW_SET",
                "SLEW_ALTIT_PLUS",
                "SLEW_ALTIT_MINUS",
                "SLEW_PITCH_DN_FAST",
                "SLEW_PITCH_DN_SLOW",
                "SLEW_PITCH_UP_FAST",
                "SLEW_PITCH_UP_SLOW",
                "SLEW_BANK_MINUS",
                "SLEW_BANK_PLUS",
                "SLEW_HEADING_MINUS",
                "SLEW_HEADING_PLUS",
                "SLEW_AHEAD_PLUS",
                "SLEW_AHEAD_MINUS",
                "SLEW_LEFT",
                "SLEW_RIGHT",
                "SOUND_TOGGLE",
                "SOUND_SET",
                "MAP_ORIENTATION_CYCLE"
            ]
        }
    ],

    // Helper function to get all events as a flat array
    getAllEvents: function() {
        return this.groups.reduce((all, group) => {
            return all.concat(group.events);
        }, []);
    },

    // Helper function to search for events containing a term
    searchEvents: function(searchTerm) {
        const term = searchTerm.toUpperCase();
        const results = [];
        
        this.groups.forEach(group => {
            const matchingEvents = group.events.filter(event => 
                event.includes(term)
            );
            if (matchingEvents.length > 0) {
                results.push({
                    name: group.name,
                    events: matchingEvents
                });
            }
        });
        
        return results;
    },

    // Helper function to populate a select element with grouped options
    populateSelect: function(selectElement, includeSearch = false) {
        // Clear existing options
        selectElement.innerHTML = '<option value="">Choose Common Event...</option>';
        
        if (includeSearch) {
            const searchOption = document.createElement('option');
            searchOption.value = 'SEARCH';
            searchOption.textContent = '🔍 Type to search...';
            selectElement.appendChild(searchOption);
        }
        
        this.groups.forEach(group => {
            const optgroup = document.createElement('optgroup');
            optgroup.label = group.name;
            
            group.events.forEach(event => {
                const option = document.createElement('option');
                option.value = event;
                option.textContent = event;
                optgroup.appendChild(option);
            });
            
            selectElement.appendChild(optgroup);
        });
    }
};

// Export for use in other files
if (typeof module !== 'undefined' && module.exports) {
    module.exports = ToggleEventsData;
}