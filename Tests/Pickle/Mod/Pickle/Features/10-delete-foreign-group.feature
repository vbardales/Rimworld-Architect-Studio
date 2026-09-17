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
    # TESTING.md promises the members come back. Read in the source, the button only clears the
    # hidden list and leaves the "no group" assignments in place, so this is expected to fail
    # until the code or the promise changes.
    When I delete the group "Floor_Carpet"
    And I restore the deleted groups
    Then the group "Floor_Carpet" has members again
