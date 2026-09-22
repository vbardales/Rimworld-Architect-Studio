# TESTING.md scenario 15, the part that needs no third-party mod. Nothing is asserted about the
# drawn page: the report carries a screenshot of the settings window opened through the shortcut's
# own worker, and a person checks that both toggles, the two editor buttons and the reset control
# read correctly in the language the game runs in.
#
# RIMMSQOL reveal/hide and its real-restart persistence are covered by features 19 through 22 with
# PickleTools/RimmsqolSteps. This feature owns the Architect Studio side: the native shortcut worker
# reaches this same settings page.
#
# The opening step waits frames rather than the scenario waiting ticks: Dialog_ModSettings is a
# full-screen modal that pauses the simulation, so "I wait N ticks" under it never advances and
# times out after 5s - which is exactly how this scenario failed on its first run.
@review
Feature: the settings page behind the hidden shortcut

  Scenario: screenshot of the settings page opened through the shortcut
    Given the save "test-colony" is loaded
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When I take a screenshot "settings page through the shortcut"
