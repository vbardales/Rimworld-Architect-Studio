# TESTING.md "Translation gate and in-game language checks", the visual half. Nothing is asserted
# about the text: the report carries screenshots taken in whatever language the game is running in,
# and a person looks for raw keys, English left standing in a French game, wrong parameters and
# clipping. The static half - key inventory, EN/FR parity, parameter parity, literal strings in C# -
# is covered out of game by Tests/Validate-Mod.ps1 and is not repeated here.
#
# Language selection is a launch parameter: play this feature once with `-Language English` and once
# with `-Language French`, never by switching a running game. The process-restart half of generated
# binding coverage remains explicitly unverified until a dedicated Pickle restart pass is played.
@review @requires:nelim.pickletools.interfacescale
Feature: the interface in the language the game runs in

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled

  Scenario: screenshots of both editors at 100 percent
    When I close all dialogs
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I wait 30 ticks
    And I take a screenshot "group editor at 100 percent"
    And I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When I take a screenshot "category editor at 100 percent"

  # The gate asks for both scales: 150% is where a translated label has the least room left.
  Scenario: screenshots of both editors at 150 percent
    When I close all dialogs
    And I set the interface scale to 150 percent
    And I wait 60 ticks
    And I open the "Architect" tab
    And I click the Architect Studio button keyed "ArchitectStudio.ArchitectButton"
    Then window "Dialog_DropdownGroups" is open
    When I wait 30 ticks
    And I take a screenshot "group editor at 150 percent"
    And I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When I take a screenshot "category editor at 150 percent"

  # Scenario 8 already asserts that a created category reaches the keyboard configuration. What a
  # person is asked to read here is the generated tab label and description: the entered name must
  # come back with its accents, inside a sentence written in the language the game runs in.
  Scenario: an accented category name in the generated keyboard category
    When I close all dialogs
    And I create the category "Entrepôt élevé"
    And I open the keyboard configuration and let it draw
    And I scroll the keyboard configuration to the bottom
    And I take a screenshot "keyboard configuration with an accented category"
