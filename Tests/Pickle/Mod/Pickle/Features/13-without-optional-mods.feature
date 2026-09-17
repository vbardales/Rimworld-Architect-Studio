# TESTING.md scenario 13. Needs its own mod list - Core, Harmony, Pickle, RimLogging, Architect
# Studio and this test mod, nothing else - so it is tagged @wip and an ordinary run skips it.
# On that list: -pickle-include-wip -pickle-run=13-without-optional-mods.feature
@wip
Feature: without the optional mods

  Scenario: the editors work and nothing is logged
    Given mod "ferny.betterarchitect" is not loaded
    And mod "com.bymarcin.architecticons" is not loaded
    And mod "kathanon.floatsubmenu" is not loaded
    And "Stool", "DiningChair", "Armchair" and "EndTable" start in the same category
    Then the game log holds nothing from Architect Studio since startup
    When I create the group "Pickle seats"
    And I add "Stool" to the group "Pickle seats"
    And I add "DiningChair" to the group "Pickle seats"
    Then the Architect menu shows the group "Pickle seats" as 1 button
    When I open the category editor
    Then window "Dialog_Categories" is open
