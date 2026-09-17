# TESTING.md scenario 4, the visual half. Nothing is asserted: the report carries two screenshots
# of the group editor, and a person decides whether the arrows sit inside their buttons.
@review
Feature: the arrows at 150% interface scale

  Scenario: screenshots of the group editor at 100% and 150%
    Given the save "test-colony" is loaded
    And "Stool", "DiningChair", "Armchair" and "EndTable" start in the same category
    When I create the group "Seats"
    And I add "Stool" to the group "Seats"
    And I add "DiningChair" to the group "Seats"
    And I add "Armchair" to the group "Seats"
    And I take a screenshot "group editor at 100 percent"
    And I set the interface scale to 150 percent
    And I wait 60 ticks
    And I take a screenshot "group editor at 150 percent"
