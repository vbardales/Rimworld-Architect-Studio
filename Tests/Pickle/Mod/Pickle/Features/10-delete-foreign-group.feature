# TESTING.md scenario 10. The half after a real restart cannot run in one process; the second
# scenario replays the settings file, which is the part the mod owns.
Feature: deleting a group that belongs to another mod

  Background:
    Given the group "Floor_Carpet" has at least 2 members

  Scenario: the group is emptied and remembered as deleted
    When I delete the group "Floor_Carpet"
    Then no building belongs to the group "Floor_Carpet"
    And the group "Floor_Carpet" is remembered as deleted

  Scenario: it stays deleted when the settings are replayed
    When I delete the group "Floor_Carpet"
    And Architect Studio starts again from its settings file
    Then no building belongs to the group "Floor_Carpet"

  Scenario: Restore deleted groups brings its members back
    # TESTING.md promises the members come back, and until 2026-09-18 they did not: the button
    # cleared the hidden list but nothing reapplied the groups, so the members stayed where the
    # dissolution had left them. Clearing that list is now enough on its own.
    When I delete the group "Floor_Carpet"
    And I restore the deleted groups
    Then the group "Floor_Carpet" has members again

  # Categories Dropdowns ships its own DesignatorDropdownGroupDef list (Ferny_Walls, Ferny_Doors,
  # Ferny_Bridges...) with every assignment to a vanilla or third-party building commented out in
  # its own patch.xml: on this mod list the group exists but holds no member. That is still the
  # closest thing to a rival group system this mod has to stay compatible with, and no scenario had
  # ever dissolved one of its groups before.
  @requires:ferny.categoriesdropdowns
  Scenario: a Categories Dropdowns group can be dissolved without breaking
    When I delete the group "Ferny_Walls"
    Then the group "Ferny_Walls" is remembered as deleted
