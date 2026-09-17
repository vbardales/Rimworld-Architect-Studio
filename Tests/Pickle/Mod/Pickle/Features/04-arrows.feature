# TESTING.md scenario 4, the logic half. Whether the arrows sit inside their buttons at 150% is
# not something a number can say: 04b attaches screenshots for a person to look at.
Feature: the up and down arrows

  Background:
    Given "Stool", "DiningChair", "Armchair" and "EndTable" start in the same category
    When I create the group "Seats"
    And I add "Stool" to the group "Seats"
    And I add "DiningChair" to the group "Seats"
    And I add "Armchair" to the group "Seats"
    And I add "EndTable" to the group "Seats"
    And the group "Seats" is ordered "Stool, DiningChair, Armchair, EndTable"

  Scenario: an arrow on a middle row moves it one place
    When I press the down arrow on member 2 of the group "Seats"
    Then the group "Seats" holds "Stool, Armchair, DiningChair, EndTable" in that order
    When I press the up arrow on member 3 of the group "Seats"
    Then the group "Seats" holds "Stool, DiningChair, Armchair, EndTable" in that order
    And the Architect menu lists the group "Seats" in the same order as the editor

  Scenario: the arrows at the ends do nothing
    When I press the up arrow on member 1 of the group "Seats"
    And I press the down arrow on member 4 of the group "Seats"
    Then the group "Seats" holds "Stool, DiningChair, Armchair, EndTable" in that order
