# TESTING.md scenario 12, in one process: the runtime is unwound to the unmodded defs, then the
# settings file is read back and replayed exactly as StartupInit does. A real restart also rebuilds
# the def database from XML, which stays a manual check.
Feature: everything survives a restart

  Scenario: group, category, order and colour come back from the file
    Given "Stool", "DiningChair", "Armchair" and "EndTable" start in the same category
    When I create the group "Pickle seats"
    And I add "Stool" to the group "Pickle seats"
    And I add "DiningChair" to the group "Pickle seats"
    And I create the category "Pickle tab"
    And I move the category "Furniture" up
    And I colour the category "Structure" with palette colour 5
    And I remember the Architect menu
    And Architect Studio starts again from its settings file
    Then the Architect menu is as remembered
    And the Architect menu shows the group "Pickle seats" as 1 button
