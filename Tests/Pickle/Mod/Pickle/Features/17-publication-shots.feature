# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The game's own screenshot mode is turned on around each capture: it hides the tab bar, the
# alerts, the colonist bar, the dev toolbar and Pickle's own runner panel, so what lands in the
# report is the window over the map and nothing else. That is the difference between a capture a
# person reviews and one that can be published as it is.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
@review
Feature: the windows as a player would show them

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled

  Scenario: the group editor with a group of its own, filled
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Monolith machines"
    And I add "A" to the group "Monolith machines"
    And I add "B" to the group "Monolith machines"
    And I add "C" to the group "Monolith machines"
    And I select the group "Monolith machines" in the editor
    And I hide the interface around the windows on screen
    And I take a screenshot "group editor, a filled group"
    And I bring the interface back

  Scenario: the category editor over the map
    When I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When I hide the interface around the windows on screen
    And I take a screenshot "category editor"
    And I bring the interface back

  Scenario: the settings page over the map
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When I hide the interface around the windows on screen
    And I take a screenshot "settings page"
    And I bring the interface back
