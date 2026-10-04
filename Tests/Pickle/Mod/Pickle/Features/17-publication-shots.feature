# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The shots use Nelim's tribe (fixture Nelims-tribe.rws, the house at (190, 115)), not the generic functional fixture. The game's
# screenshot mode hides the tab bar, alerts, colonist bar, dev toolbar and Pickle panel, leaving
# each real window over a deliberate colony view rather than over an incidental test map.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
@review @requires:nelim.pickletools.camerazoom @requires:nelim.pickletools.screenshotmode
Feature: the windows as a player would show them

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused
    And god mode is disabled

  Scenario: the group editor with a group of its own, filled
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Monolith machines"
    And I add "A" to the group "Monolith machines"
    And I add "B" to the group "Monolith machines"
    And I add "C" to the group "Monolith machines"
    And I select the group "Monolith machines" in the editor
    And Nelim's Pickle Tools: I frame the cell (190, 115) at zoom 15
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "group editor, a filled group"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the category editor over the map
    When I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When Nelim's Pickle Tools: I frame the cell (190, 115) at zoom 15
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "category editor"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the settings page over the map
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When Nelim's Pickle Tools: I frame the cell (190, 115) at zoom 15
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "settings page"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # The result, not the tool: the Architect menu with the group as one button, its dropdown open.
  # A menu is shown for what it is, so nothing is staged beyond the group the scenario builds itself.
  Scenario: the Architect menu with the group as one button, its dropdown open
    Given four buildings "A", "B", "C" and "D" from one category, in no group, that the player can already build
    When I create the group "Workshop favourites"
    And I add "A" to the group "Workshop favourites"
    And I add "B" to the group "Workshop favourites"
    And I add "C" to the group "Workshop favourites"
    And I close all dialogs
    And I show the Architect menu on the category of the group "Workshop favourites"
    And I open the dropdown of the group "Workshop favourites" in the Architect menu
    And Nelim's Pickle Tools: I frame the cell (190, 115) at zoom 15
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect menu, a group opened"
    And Nelim's Pickle Tools: screenshot mode is disabled
