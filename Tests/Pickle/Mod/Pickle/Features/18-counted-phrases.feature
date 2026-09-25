# TESTING.md "Translation gate": the counted phrases and the tooltips no earlier capture shows.
# Nothing is asserted about the text; a person reads the screenshots in the language the game runs
# in (play once with `-Language English`, once with `-Language French`).
# Not reachable on screen: the ".Zero" form of the delete confirmation (an empty group has its own
# text), the ".One" form of "…and N more" (needs exactly 201 buildings) and the ".Many" form of
# "groups deleted" (needs a second foreign group).
@review @requires:nelim.pickletools.screenshotmode @requires:nelim.pickletools.hoversteps
Feature: counted phrases and tooltips in the language the game runs in

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled

  Scenario: the footer counts one building, then several
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I create the group "Pickle seats"
    And I add "A" to the group "Pickle seats"
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "footer with one building moved"
    And Nelim's Pickle Tools: screenshot mode is disabled
    When I add "B" to the group "Pickle seats"
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "footer with two buildings moved"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the delete confirmation for several buildings, then for one
    Given the group "Floor_Carpet" has at least 2 members
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I ask to delete the group "Floor_Carpet" and leave the question open
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "delete confirmation with several buildings"
    And Nelim's Pickle Tools: screenshot mode is disabled
    When I close all dialogs
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I remove every member but one from the group "Floor_Carpet"
    And I ask to delete the group "Floor_Carpet" and leave the question open
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "delete confirmation with one building"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the footer after one deleted group
    Given the group "Floor_Carpet" has at least 2 members
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I delete the group "Floor_Carpet"
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "footer with one group deleted"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the add column cut short by the result limit
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I create the group "Pickle seats"
    And I select the group "Pickle seats" in the editor
    And I list the buildings of every category in the add column
    And I wait 30 ticks
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "add column with more results than shown"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the tooltip of the Groups button in the Architect window
    When I close all dialogs
    And I open the "Architect" tab
    And Nelim's Pickle Tools: I hover over the tooltip keyed "ArchitectStudio.ArchitectButtonTip"
    And I take a screenshot "tooltip of the groups button"

  Scenario: the tooltip of the forced category in the group editor
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I create the group "Pickle seats"
    And I select the group "Pickle seats" in the editor
    And Nelim's Pickle Tools: I hover over the tooltip keyed "ArchitectStudio.Dropdowns.GroupCategoryTip"
    And I take a screenshot "tooltip of the forced category"

  Scenario: the tooltip of the Architect button setting
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    And Nelim's Pickle Tools: I hover over the tooltip keyed "ArchitectStudio.Settings.ShowArchitectButtonTip"
    And I take a screenshot "tooltip of the Architect button setting"
