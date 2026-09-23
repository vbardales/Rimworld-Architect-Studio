# TESTING.md scenario 8. Moving a building into the category is Better Architect Menu's own Edit
# Mode and is not driven here.
Feature: creating a category, and its keyboard shortcut

  Background:
    Given the save "test-colony" is loaded

  Scenario: a created category gets a tab and a key binding category
    When I create the category "Pickle tab"
    Then the Architect menu has a tab labelled "Pickle tab"
    And the category "Pickle tab" has a key binding category

  Scenario: the keyboard configuration opens and draws without errors once a category exists
    When I create the category "Pickle tab"
    And I open the keyboard configuration and let it draw
    Then window "Dialog_KeyBindings" is open
    And no errors were logged

  Scenario: deleting it removes the tab
    When I create the category "Pickle tab"
    And I delete the category "Pickle tab"
    Then the Architect menu has no tab labelled "Pickle tab"
