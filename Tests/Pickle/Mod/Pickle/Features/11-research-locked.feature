# TESTING.md scenario 11. Designator_Build.Visible is the only research lock: a visible designator
# that is not also disabled lets a blueprint be placed. The pointer click itself is not replayed;
# the disabled flag is what the gizmo grid refuses a click on.
Feature: showing what research still locks

  Background:
    Given the save "test-colony" is loaded
    And god mode is disabled
    And research "ComplexFurniture" is not finished

  Scenario: off by default, the locked building is hidden
    Then the Architect menu hides "EndTable"

  Scenario: on, it shows greyed out and refuses
    When I turn on showing what research still locks
    Then the Architect menu shows "EndTable" greyed out with the reason keyed "ArchitectStudio.ResearchLocked.Reason"

  Scenario: finishing the research makes it buildable, without a restart
    When I turn on showing what research still locks
    And research "ComplexFurniture" is finished
    Then the Architect menu shows "EndTable" as buildable

  Scenario: turning the option off hides it again
    When I turn on showing what research still locks
    And I turn off showing what research still locks
    Then the Architect menu hides "EndTable"

  # Request of Ali50, case left open on 2026-10-06: a tab moved under another one keeps the research
  # lock it had. The lock is the category's own Visible, so it must not depend on where the tab sits.
  # A leaf category is given a prerequisite for the scenario (the game's locked categories are
  # parents of Better Architect Menu, or need a DLC).
  @requires:ferny.betterarchitect
  Scenario: a moved tab that research still locks follows the option and the research
    Given research "Electricity" is not finished
    And the category "Misc" needs the research "Electricity"
    When I move the category "Misc" under the parent "Structure"
    Then the category "Misc" is listed under the parent "Structure"
    And the category "Misc" is hidden from the game
    When I turn on showing what research still locks
    Then the category "Misc" is visible to the game
    And the Architect menu has no tab for the category "Misc"
    When I turn off showing what research still locks
    Then the category "Misc" is hidden from the game
    When I turn on showing what research still locks
    And research "Electricity" is finished
    Then the category "Misc" is visible to the game

