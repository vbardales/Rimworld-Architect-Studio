# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The shots use Nelim's reusable zen-meadow studio, not the generic functional fixture. The game's
# screenshot mode hides the tab bar, alerts, colonist bar, dev toolbar and Pickle panel, leaving
# each real window over a deliberate colony view rather than over an incidental test map.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
@review @requires:nelim.pickletools.screenshotstudio
Feature: the windows as a player would show them

  Background:
    Given the save "nelim-zen-meadow-studio" is loaded
    And game speed is paused
    And god mode is disabled

  Scenario: the group editor with a group of its own, filled
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Monolith machines"
    And I add "A" to the group "Monolith machines"
    And I add "B" to the group "Monolith machines"
    And I add "C" to the group "Monolith machines"
    And I select the group "Monolith machines" in the editor
    And Nelim's Pickle Tools: I frame the studio "emblem"
    And I hide the interface around the windows on screen
    And I take a screenshot "group editor, a filled group"
    And I bring the interface back

  Scenario: the category editor over the map
    When I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When Nelim's Pickle Tools: I frame the studio "emblem"
    And I hide the interface around the windows on screen
    And I take a screenshot "category editor"
    And I bring the interface back

  Scenario: the settings page over the map
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When Nelim's Pickle Tools: I frame the studio "emblem"
    And I hide the interface around the windows on screen
    And I take a screenshot "settings page"
    And I bring the interface back
