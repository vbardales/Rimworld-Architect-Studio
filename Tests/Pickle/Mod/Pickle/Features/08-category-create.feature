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

  # TESTING.md scenario 8: "give it a name, then a parent if Better Architect Menu is present".
  # BAM owns the only nesting mechanism there is; no scenario had ever created a category under a
  # parent before.
  @requires:ferny.betterarchitect
  Scenario: a category created with a parent nests under it
    When I create the category "Pickle parent"
    And I create the category "Pickle child" under the parent "Pickle parent"
    Then the category "Pickle child" sits under the parent "Pickle parent"

  # Request of Ali50 (Workshop, 2026-10): take a tab another mod or the game added and put it under
  # another tab. The def is not ours; Better Architect Menu's extension is grafted on it.
  @requires:ferny.betterarchitect
  Scenario: a tab that is not ours moves under another, then back, then is put back by the reset
    When I move the category "Temperature" under the parent "Structure"
    Then the category "Temperature" is listed under the parent "Structure"
    And the Architect menu has no tab for the category "Temperature"
    When I move the category "Temperature" back to the top level
    Then the category "Temperature" has no parent
    And the Architect menu has a tab for the category "Temperature"
    When I move the category "Temperature" under the parent "Structure"
    And I reset everything from the mod settings
    Then the category "Temperature" has no parent
    And the Architect menu has a tab for the category "Temperature"
