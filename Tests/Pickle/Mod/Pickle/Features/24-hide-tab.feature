# TESTING.md scenario 24. A whole Architect tab can be hidden from the category editor. Nothing is
# deleted: the tab stops being shown, its buildings stay in the game and, with the Unsorted tab on,
# are offered there. "Reset everything" shows every hidden tab again.
Feature: hiding a whole Architect tab

  Background:
    Given the save "test-colony" is loaded

  Scenario: a hidden tab is no longer shown, and showing it again brings it back
    Then the Architect menu shows the tab "Furniture"
    When I hide the Architect tab "Furniture"
    Then the Architect menu does not show the tab "Furniture"
    And the editor lists the tab "Furniture" as hidden
    And the building "Bed" still exists and is buildable by the player
    When I show the Architect tab "Furniture" again
    Then the Architect menu shows the tab "Furniture"

  Scenario: with the Unsorted tab on, the buildings of a hidden tab are offered there
    When I turn on the Unsorted tab
    Then the Unsorted tab does not offer "Bed"
    When I hide the Architect tab "Furniture"
    Then the Unsorted tab offers "Bed"
    And the category editor counts as many buildings in the Unsorted tab as it offers
    When I show the Architect tab "Furniture" again
    Then the Unsorted tab does not offer "Bed"

  Scenario: the Unsorted tab itself has its own switch and cannot be hidden here
    When I turn on the Unsorted tab
    And I hide the Architect tab "AS_Unsorted"
    Then the Architect menu shows the tab "AS_Unsorted"

  Scenario: the reset shows every hidden tab again
    When I hide the Architect tab "Furniture"
    And I reset everything from the mod settings
    Then the Architect menu shows the tab "Furniture"
