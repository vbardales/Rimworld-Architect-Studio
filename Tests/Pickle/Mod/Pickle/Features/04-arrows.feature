# TESTING.md scenario 4, the half a running menu can show: what the arrows do is compared against
# the Architect menu's own button order. Their bounds - an arrow at either end moving nothing - is
# state only, and lives in Tests/BehaviorTests.cs. Whether the arrows sit inside their buttons at
# 150% is not something a number can say either: 04b attaches screenshots for a person to look at.
Feature: the up and down arrows

  Background:
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Seats"
    And I add "A" to the group "Seats"
    And I add "B" to the group "Seats"
    And I add "C" to the group "Seats"
    And I add "D" to the group "Seats"
    And the group "Seats" is ordered "A, B, C, D"

  Scenario: an arrow on a middle row moves it one place
    When I press the down arrow on member 2 of the group "Seats"
    Then the group "Seats" holds "A, C, B, D" in that order
    When I press the up arrow on member 3 of the group "Seats"
    Then the group "Seats" holds "A, B, C, D" in that order
    And the Architect menu lists the group "Seats" in the same order as the editor
