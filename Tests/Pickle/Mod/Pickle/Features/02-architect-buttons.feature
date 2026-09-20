# TESTING.md scenario 2. Clicks are real OS input: the pointer moves on screen while this runs.
# The buttons are named by translation key, not by their English text: the label drawn is the one
# of the language the game runs in.
Feature: the two buttons in the Architect window

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled
    When I open the "Architect" tab

  Scenario: Groups… opens the group editor
    When I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open

  Scenario: Categories… opens the category editor
    When I click the Architect Studio button keyed "ArchitectStudio.ArchitectButtonCategories"
    Then window "Dialog_Categories" is open

  Scenario: turning the button off removes the row and its height
    When I turn off the button in the Architect menu
    Then the Architect window got 26 pixels shorter
    And no Architect Studio button keyed "ArchitectStudio.ArchitectButton" is drawn
    And no Architect Studio button keyed "ArchitectStudio.ArchitectButtonCategories" is drawn

  Scenario: turning it back on restores both
    When I turn off the button in the Architect menu
    And I turn on the button in the Architect menu
    Then the Architect window got 26 pixels taller
    When I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
