# TESTING.md 15, step 2: in RIMMSQOL, reveal the shortcut, open the same settings, hide it again. Feature 15
# tests THIS mod's side of that contract by opening the page through the shortcut's own worker. This one drives
# RIMMSQOL itself, through the shared steps of PickleTools/RimmsqolSteps:
#
#   - RIMMSQOL's own list of main buttons offers ArchitectStudio_Settings, and the entry a player would click
#     reads hidden, which is what the mod ships;
#   - RIMMSQOL reveals it (its own settings instance and its own write; the def's buttonVisible moves as a
#     result), the main bar then draws it and the file RIMMSQOL wrote says so;
#   - the revealed button opens Architect Studio's OWN settings page and not another mod's;
#   - hiding it again empties the bar, and forgetting the choice leaves nothing in RIMMSQOL's file.
#
# WHAT THIS DOES NOT DO: it does not click RIMMSQOL's checkbox. The steps make the calls the checkbox makes; that
# the checkbox is wired to them is read from RIMMSQOL's source, not shown in a game. "The bar draws it" is worked
# out from the bar's own list and rule, not photographed: the captures are what shows pixels, and a green
# scenario says only that the path ran. RIMMSQOL's own persistence is outside this mod's contract.
#
# Played only by the pass "avec-rimmsqol": without RIMMSQOL staged the first step stops with a sentence.
# Every scenario is followed by a teardown that puts RIMMSQOL back, pass or fail.
@wip @review @rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: RIMMSQOL reveals and hides the Architect Studio shortcut

  Background:
    Given the save "test-colony" is loaded
    And I close all dialogs
    Then mod "MalteSchulze.RIMMSqol" is loaded
    And RIMMSQOL is ready to be driven

  Scenario: RIMMSQOL's own list offers the shortcut, hidden, and the bar does not draw it
    Then RIMMSQOL's own list of main buttons offers "ArchitectStudio_Settings"
    And RIMMSQOL shows the main button "ArchitectStudio_Settings" as hidden
    And RIMMSQOL holds no choice for the main button "ArchitectStudio_Settings"
    And the main bar does not draw the button "ArchitectStudio_Settings"
    When RIMMSQOL's own window is opened on its list of main buttons
    Then RIMMSQOL's own window is open
    When I take a screenshot "rimmsqol, its list of main buttons, with the Architect Studio shortcut"
    And I close all dialogs

  Scenario: revealed in RIMMSQOL the shortcut is drawn, and it opens Architect Studio's own page
    When RIMMSQOL reveals the main button "ArchitectStudio_Settings"
    Then RIMMSQOL shows the main button "ArchitectStudio_Settings" as visible
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as visible
    And the main bar draws the button "ArchitectStudio_Settings"
    When RIMMSQOL's own window is opened on the main button "ArchitectStudio_Settings"
    Then RIMMSQOL's own window is open
    When I take a screenshot "rimmsqol, edit page of the Architect Studio shortcut, revealed"
    And I close all dialogs
    And the main bar's button "ArchitectStudio_Settings" is activated
    Then window "Dialog_ModSettings" is open
    And the settings window open is Architect Studio's own
    When I take a screenshot "architect studio settings, opened by the shortcut RIMMSQOL revealed"
    And I close all dialogs

  # The last steps put RIMMSQOL back: this scenario's teardown would do it anyway, but a scenario that says it
  # and checks it cannot be mistaken for one that merely ended.
  Scenario: hidden again in RIMMSQOL the shortcut leaves the bar, and forgetting the choice leaves nothing behind
    Given RIMMSQOL reveals the main button "ArchitectStudio_Settings"
    And the main bar draws the button "ArchitectStudio_Settings"
    When RIMMSQOL hides the main button "ArchitectStudio_Settings"
    Then RIMMSQOL shows the main button "ArchitectStudio_Settings" as hidden
    And the main bar does not draw the button "ArchitectStudio_Settings"
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as hidden
    When RIMMSQOL forgets its choice for the main button "ArchitectStudio_Settings"
    Then RIMMSQOL holds no choice for the main button "ArchitectStudio_Settings"
    And RIMMSQOL's settings file records no choice for the main button "ArchitectStudio_Settings"
    And the main bar does not draw the button "ArchitectStudio_Settings"
    And no errors were logged
