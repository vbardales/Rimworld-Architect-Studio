# TESTING.md scenario 6. "The menu" is the category's resolved designator list, the one the
# Architect tab iterates every frame, so no restart is involved anywhere below.
Feature: creating a group and filling it

  Background:
    Given "Stool", "DiningChair", "Armchair" and "EndTable" start in the same category

  Scenario: members collapse into one button at once
    When I create the group "Pickle seats"
    And I add "Stool" to the group "Pickle seats"
    And I add "DiningChair" to the group "Pickle seats"
    And I add "Armchair" to the group "Pickle seats"
    Then the Architect menu shows the group "Pickle seats" as 1 button

  Scenario: a removed member becomes its own button again
    When I create the group "Pickle seats"
    And I add "Stool" to the group "Pickle seats"
    And I add "DiningChair" to the group "Pickle seats"
    And I add "Armchair" to the group "Pickle seats"
    And I remove "Armchair" from its group
    Then the Architect menu shows the group "Pickle seats" as 1 button
    And "Armchair" is a button of its own in its category
