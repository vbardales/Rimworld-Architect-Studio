# TESTING.md 8, 10, 12 and 15.3, the half a single process cannot run: launch 1 of 2, WRITE.
#
# Everything the mod persists is configured in one scenario: a group created and filled, a category created
# (which builds a key binding category the game only generates at startup), a category moved, a colour set,
# and a group that belongs to another mod deleted. The last step keeps it all on disk on purpose and records a
# snapshot of the whole Architect menu. Launch 2 (21-restart-read.feature) starts from that file in a new
# process and compares.
#
# Played only where the switch mod is staged, which is what @requires expresses: see wsl-deps.redemarrage.map.
@requires:nelim.architectstudio.restartpass
Feature: what Architect Studio configured survives a real restart (1 of 2, write)

  Scenario: everything is configured and kept for the next launch
    Given the save "test-colony" is loaded
    And Architect Studio starts this restart test from a clean profile
    And four buildings "A", "B", "C" and "D" from one category, in no group
    And the group "Floor_Carpet" has at least 2 members
    When I create the group "Pickle seats"
    And I add "A" to the group "Pickle seats"
    And I add "B" to the group "Pickle seats"
    And I create the category "Pickle tab"
    And I move the category "Furniture" up
    And I colour the category "Structure" with palette colour 5
    And I delete the group "Floor_Carpet"
    Then the Architect menu shows the group "Pickle seats" as 1 button
    And no building belongs to the group "Floor_Carpet"
    And the category "Pickle tab" has a key binding category
    And the game log holds nothing from Architect Studio since startup
    When Architect Studio keeps this configuration for the next launch
