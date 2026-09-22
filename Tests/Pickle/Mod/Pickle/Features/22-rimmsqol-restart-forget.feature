# TESTING.md 15, step 3, chain launch 3 of 3: the hide survived the restart, then forget. After this launch
# RIMMSQOL holds nothing about the shortcut and the profile is back to what a clean run starts from.
@wip @rimmsqol @requires:MalteSchulze.RIMMSqol @requires:nelim.pickletools.rimmsqol
Feature: a choice made in RIMMSQOL is read at the next launch (3 of 3, forget)

  Scenario: the hide survived the restart, and forgetting leaves nothing behind
    Given the save "test-colony" is loaded
    And I close all dialogs
    And RIMMSQOL is ready to be driven
    And the choices RIMMSQOL kept in the previous launch are in place
    Then RIMMSQOL shows the main button "ArchitectStudio_Settings" as hidden
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as hidden
    And the main bar does not draw the button "ArchitectStudio_Settings"
    When RIMMSQOL forgets its choice for the main button "ArchitectStudio_Settings"
    Then RIMMSQOL holds no choice for the main button "ArchitectStudio_Settings"
    And RIMMSQOL's settings file records no choice for the main button "ArchitectStudio_Settings"
    And the main bar does not draw the button "ArchitectStudio_Settings"
    And no errors were logged
