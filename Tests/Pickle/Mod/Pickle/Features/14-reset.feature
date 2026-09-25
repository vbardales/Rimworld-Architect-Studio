# TESTING.md scenario 14. Every scenario already starts from a reset configuration, so what is
# remembered first is the Architect menu with the mod doing nothing.
Feature: reset everything

  Scenario: the menu returns to how it looked with nothing configured
    Given "Stool" and "Heater" start in different categories
    When I remember the Architect menu
    And I create the group "Pickle mixed"
    And I add "Stool" to the group "Pickle mixed"
    And I add "Heater" to the group "Pickle mixed"
    And I force the category "Furniture" on the group "Pickle mixed"
    And I create the category "Pickle tab"
    And I move the category "Furniture" up
    And I relabel the category "Structure" as "Pickle walls"
    And I colour the category "Structure" with palette colour 2
    And I turn on showing what research still locks
    And I reset everything from the mod settings
    Then the Architect menu is as remembered
    And the mod settings offer nothing to reset

  # TESTING.md 15.4. Each preference alone, with no editor customization: the reset must become
  # available and put it back. The default is on for the Architect button and off for research.
  Scenario: the Architect button preference alone makes the reset available and restores it to on
    When I switch the Architect button preference off in the mod settings
    Then the mod settings offer a reset
    When I reset everything from the mod settings
    Then the Architect button preference is on
    And the mod settings offer nothing to reset

  Scenario: showing what research still locks alone makes the reset available and restores it to off
    When I turn on showing what research still locks
    Then the mod settings offer a reset
    When I reset everything from the mod settings
    Then showing what research still locks is off
    And the mod settings offer nothing to reset
