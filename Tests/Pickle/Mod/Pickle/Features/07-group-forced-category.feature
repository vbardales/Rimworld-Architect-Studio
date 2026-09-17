# TESTING.md scenario 7: the one feature with no equivalent anywhere. The buildings come from
# three vanilla categories; the Given steps stop with an explanation if the mod list regrouped them.
Feature: a category forced on a whole group

  Background:
    Given "Stool" and "Heater" start in different categories
    When I create the group "Pickle mixed"
    And I add "Stool" to the group "Pickle mixed"
    And I add "Heater" to the group "Pickle mixed"

  Scenario: without a category of its own, the group splits
    Then the Architect menu shows the group "Pickle mixed" as 2 buttons

  Scenario: forcing a category moves every member there
    When I force the category "Furniture" on the group "Pickle mixed"
    Then "Stool" sits in the category "Furniture"
    And "Heater" sits in the category "Furniture"
    And the Architect menu shows the group "Pickle mixed" as 1 button

  Scenario: a member added later follows on its own
    When I force the category "Furniture" on the group "Pickle mixed"
    And I add "PowerConduit" to the group "Pickle mixed"
    Then "PowerConduit" sits in the category "Furniture"
    And the Architect menu shows the group "Pickle mixed" as 1 button

  Scenario: clearing the category sends members home
    When I force the category "Furniture" on the group "Pickle mixed"
    And I clear the forced category of the group "Pickle mixed"
    Then "Heater" is back in its original category
    And the Architect menu shows the group "Pickle mixed" as 2 buttons
