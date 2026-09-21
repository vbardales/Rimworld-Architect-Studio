# TESTING.md 15, step 3, chain launch 2 of 3: the reveal survived the restart, then hide. Launch 1 revealed the
# shortcut and kept the choice; this process starts from RIMMSQOL's file alone. The first step refuses to pass
# when the writer ran in this same process, so a memory that merely still holds the value cannot pass for a file.
# It also proves the revealed shortcut is what the player sees at startup: drawn, and opening our own page.
@wip @rimmsqol
Feature: a choice made in RIMMSQOL is read at the next launch (2 of 3, hide)

  Scenario: the reveal survived the restart, and the shortcut is hidden again
    Given the save "test-colony" is loaded
    And I close all dialogs
    And RIMMSQOL is ready to be driven
    And the choices RIMMSQOL kept in the previous launch are in place
    Then RIMMSQOL shows the main button "ArchitectStudio_Settings" as visible
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as visible
    And the main bar draws the button "ArchitectStudio_Settings"
    When the main bar's button "ArchitectStudio_Settings" is activated
    Then the settings window open is Architect Studio's own
    When I take a screenshot "architect studio settings, opened by the shortcut revealed before this restart"
    And I close all dialogs
    When RIMMSQOL hides the main button "ArchitectStudio_Settings"
    Then the main bar does not draw the button "ArchitectStudio_Settings"
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as hidden
    And no errors were logged
    And RIMMSQOL's choices are kept for the next launch
