# TESTING.md 15, step 3, chain launch 1 of 3: reveal. A choice made in RIMMSQOL is written to its settings file
# when its window closes; only a second process shows that the file is the one the next launch reads at startup,
# applies to MainButtonDef.buttonVisible, and draws. Reading the file back in the same process shows nothing:
# the values are still in memory.
#
# The chain is three launches under ONE hold of the lock (-Then stages once and keeps the profile):
#   -DepMap wsl-deps.avec-rimmsqol.map -IncludeWip -Filter '20-rimmsqol-restart-reveal.feature'
#       -Then '21-rimmsqol-restart-hide.feature','22-rimmsqol-restart-forget.feature'
#
# THIS LAUNCH LEAVES RIMMSQOL'S CHOICE BEHIND ON PURPOSE. The last step says so, and it comes last so that a
# scenario that fails before it leaves nothing: the teardown puts the choice back and the launcher stops the
# chain. If the game dies after that step, or the chain is cut, the choice stays in the WSL profile's
# Config/Mod_1084452457_QOLMod.xml; PickleTools/RimmsqolSteps/README.md, "Leftovers", gives the files to delete.
@wip @rimmsqol
Feature: a choice made in RIMMSQOL is written for the next launch (1 of 3, reveal)

  Scenario: RIMMSQOL reveals the shortcut and the choice is kept
    Given the save "test-colony" is loaded
    And I close all dialogs
    And RIMMSQOL is ready to be driven
    And the main bar does not draw the button "ArchitectStudio_Settings"
    When RIMMSQOL reveals the main button "ArchitectStudio_Settings"
    Then the main bar draws the button "ArchitectStudio_Settings"
    And RIMMSQOL's settings file records the main button "ArchitectStudio_Settings" as visible
    And no errors were logged
    And RIMMSQOL's choices are kept for the next launch
