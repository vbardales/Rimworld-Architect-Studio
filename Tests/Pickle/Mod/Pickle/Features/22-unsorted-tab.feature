# TESTING.md scenario 22. The opt-in Unsorted tab gathers the buildings the game generated a
# blueprint for but no tab provides (default designator off, nothing else offers it). A vanilla
# building is switched off for the length of one scenario to be that orphan.
Feature: the Unsorted tab

  Background:
    Given the save "test-colony" is loaded

  Scenario: off by default, there is no Unsorted tab
    Then the Architect menu has no Unsorted tab

  Scenario: on, an orphan building is offered there and nowhere else
    Given the building "EndTable" has no default designator
    Then no other tab offers "EndTable"
    When I turn on the Unsorted tab
    Then the Architect menu has an Unsorted tab
    And the Unsorted tab offers "EndTable"
    And the category editor counts as many buildings in the Unsorted tab as it offers

  Scenario: a building another tab shows is not an orphan
    Given the building "EndTable" has no default designator
    When I turn on the Unsorted tab
    Then the Unsorted tab does not offer "Bed"

  Scenario: turning it off removes the tab
    When I turn on the Unsorted tab
    And I turn off the Unsorted tab
    Then the Architect menu has no Unsorted tab

  Scenario: the reset puts the preference back
    When I turn on the Unsorted tab
    And I reset everything from the mod settings
    Then the Architect menu has no Unsorted tab
