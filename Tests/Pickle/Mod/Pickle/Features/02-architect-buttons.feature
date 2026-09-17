# TESTING.md scenario 2. Clicks are real OS input: the pointer moves on screen while this runs.
Feature: the two buttons in the Architect window

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled
    When I open the "Architect" tab

  Scenario: Groups… opens the group editor
    When I click button "Groups…"
    Then window "Dialog_DropdownGroups" is open

  Scenario: Categories… opens the category editor
    When I click button "Categories…"
    Then window "Dialog_Categories" is open

  Scenario: turning the button off removes the row and its height
    When I turn off the button in the Architect menu
    Then the Architect window got 26 pixels shorter
    And no button "Groups…" is drawn
    And no button "Categories…" is drawn

  Scenario: turning it back on restores both
    When I turn off the button in the Architect menu
    And I turn on the button in the Architect menu
    Then the Architect window got 26 pixels taller
    When I click button "Groups…"
    Then window "Dialog_DropdownGroups" is open
