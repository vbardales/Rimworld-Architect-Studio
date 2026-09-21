# TESTING.md scenario 13. Needs its own mod list - Core, Harmony, Pickle, RimLogging, Architect
# Studio and this test mod, nothing else - so it is tagged @wip and an ordinary run skips it.
# On that list: -pickle-include-wip -pickle-run=13-without-optional-mods.feature
@wip
Feature: without the optional mods

  # The same fixture the other map-bound features load, and for the same reason: the scenario
  # names buildings and reads the Architect menu, so it needs a map, and it must be the SAME map
  # every run rather than whichever colony happens to be open. The fixture ships with Pickle, so
  # nothing has to be staged for it.
  Background:
    Given the save "test-colony" is loaded

  Scenario: the editors work and nothing is logged
    Given mod "ferny.betterarchitect" is not loaded
    And mod "com.bymarcin.architecticons" is not loaded
    And mod "kathanon.floatsubmenu" is not loaded
    And four buildings "A", "B", "C" and "D" from one category, in no group
    Then the game log holds nothing from Architect Studio since startup
    When I create the group "Pickle seats"
    And I add "A" to the group "Pickle seats"
    And I add "B" to the group "Pickle seats"
    Then the Architect menu shows the group "Pickle seats" as 1 button
    When I open the category editor
    Then window "Dialog_Categories" is open
