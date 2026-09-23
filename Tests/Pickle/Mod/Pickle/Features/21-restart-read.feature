# TESTING.md 8, 10, 12 and 15.3: launch 2 of 2, READ. This is a NEW game process. It started with the settings
# file launch 1 left, so what it finds was rebuilt at startup, on a def database generated from XML, by the
# mod's own StartupInit: exactly what a player's next session does.
#
# The first step refuses to pass if the marker was written by this same process. The comparison is the whole
# Architect menu (every building's category and group, every category's order, label and colour), not a
# selection of facts. The settings are put back when the scenario ends, pass or fail.
@requires:nelim.architectstudio.restartpass
Feature: what Architect Studio configured survives a real restart (2 of 2, read)

  Scenario: the new process finds everything back
    Given the save "test-colony" is loaded
    And the configuration kept by the previous launch is what this launch started from
    Then the Architect menu is as it was when the previous launch kept it
    And the Architect menu shows the group "Pickle seats" as 1 button
    And the group "Pickle seats" holds "A, B" in that order
    And the Architect menu has a tab labelled "Pickle tab"
    And the category "Pickle tab" has a key binding category
    And no building belongs to the group "Floor_Carpet"
    And the group "Floor_Carpet" is remembered as deleted
    And the game log holds nothing from Architect Studio since startup
