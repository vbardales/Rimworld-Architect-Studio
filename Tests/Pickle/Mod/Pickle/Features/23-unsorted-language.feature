# TESTING.md scenario 22, the part a person reads. Nothing is asserted about the text; the report
# carries two screenshots, read in the language the game runs in (play once with `-Language English`,
# once with `-Language French`): the tooltip of the Unsorted setting (raw key? truncated?) and the
# Architect menu with the Unsorted tab turned on (tab label).
@review @requires:nelim.pickletools.hoversteps
Feature: the Unsorted setting and tab in the language the game runs in

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled

  Scenario: the tooltip of the Unsorted setting
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    And Nelim's Pickle Tools: I hover over the tooltip keyed "ArchitectStudio.Settings.ShowUnsortedTip"
    And I take a screenshot "tooltip of the Unsorted setting"

  Scenario: the Architect menu with the Unsorted tab turned on
    Given the building "EndTable" has its default Architect designator switched off
    When I turn on the Unsorted tab
    And I close all dialogs
    And I open the "Architect" tab
    Then the Architect menu has an Unsorted tab
    When I take a screenshot "Architect menu with the Unsorted tab"
