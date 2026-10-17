# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The shots use Nelim's tribe (fixture Nelims-tribe.rws of Nelim's Sanctuary Backlot, the exhibition zone), not the generic functional fixture. The game's
# screenshot mode hides the tab bar, alerts, colonist bar, dev toolbar and Pickle panel, leaving
# each real window over a deliberate colony view rather than over an incidental test map.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.screenshotmode
Feature: the windows as a player would show them

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused
    And god mode is disabled
    And Nelim's Pickle Tools: all filth is cleaned
    And Nelim's Sanctuary: the animals are removed from the sanctuary "exhibition-zone"

  Scenario: the group editor with a group of its own, filled
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Monolith machines"
    And I add "A" to the group "Monolith machines"
    And I add "B" to the group "Monolith machines"
    And I add "C" to the group "Monolith machines"
    And I select the group "Monolith machines" in the editor
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "group editor, a filled group"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the category editor over the map
    When I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "category editor"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the settings page over the map
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
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
    And the Architect designator is disarmed
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect menu, a group opened"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Gallery image 6 (1.1.0, the opt-in Unsorted tab; session decision, 2026-10-14). Plan: Nelim's tribe, same frame and
  # light as image 4 (cell 213,168, zoom 21, noon, clear), so the two menu shots read as a pair. Subject: the Architect
  # menu on the Unsorted tab, its bottom bar full of furniture no other tab offers (twelve vanilla buildings are switched
  # off for the length of the scenario to be the orphans a modded colony collects). Says: nothing stays out of reach.
  # A menu is shown for what it is: no colonists or decor staged. Needs `wsl-deps.sanctuary.map` and the setting turned on.
  Scenario: the Architect menu on the Unsorted tab, buildings no other tab offers
    Given the building "Stool" has its default Architect designator switched off
    And the building "DiningChair" has its default Architect designator switched off
    And the building "EndTable" has its default Architect designator switched off
    And the building "Dresser" has its default Architect designator switched off
    And the building "Couch" has its default Architect designator switched off
    And the building "Armchair" has its default Architect designator switched off
    And the building "TorchLamp" has its default Architect designator switched off
    And the building "PlantPot" has its default Architect designator switched off
    And the building "Bedroll" has its default Architect designator switched off
    And the building "SleepingSpot" has its default Architect designator switched off
    And the building "Table2x2c" has its default Architect designator switched off
    And the building "Table1x2c" has its default Architect designator switched off
    When I turn on the Unsorted tab
    And I close all dialogs
    And I show the Architect menu on the Unsorted tab
    And the Architect designator is disarmed
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect menu, the unsorted tab"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Candidate for the next gallery (1.0.7 feature, Ali50's request): a tab the game ships, moved under another one
  # from its own appearance window. Needs both integrations, hence `wsl-deps.sanctuary-bam.map`; play it with
  # `-Filter '::the appearance window of a tab moved under another'`.
  @requires:ferny.betterarchitect @requires:com.bymarcin.architecticons
  Scenario: the appearance window of a tab moved under another
    When I close all dialogs
    And I move the category "Joy" under the parent "Structure"
    And I open the appearance window of the category "Joy"
    And I wait 30 ticks
    Then window "Dialog_EditCategory" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "appearance window, a tab moved under another"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Candidate for gallery image 2 (1.2.0, hide a whole tab; session decision, 2026-10-18). Same frame and light as the
  # category editor shot, but one vanilla tab is hidden, so the row reads "Furniture (Hidden)" and greyed: the new feature
  # shows in the list a player opens first. Play it with `-Filter '::the category editor with a hidden tab'`.
  Scenario: the category editor with a hidden tab
    When I close all dialogs
    And I hide the Architect tab "Furniture"
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "category editor, a hidden tab"
    And Nelim's Pickle Tools: screenshot mode is disabled
