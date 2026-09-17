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
    Then the Architect menu shows "EndTable" greyed out with the reason "Research not completed"

  Scenario: finishing the research makes it buildable, without a restart
    When I turn on showing what research still locks
    And research "ComplexFurniture" is finished
    Then the Architect menu shows "EndTable" as buildable

  Scenario: turning the option off hides it again
    When I turn on showing what research still locks
    And I turn off showing what research still locks
    Then the Architect menu hides "EndTable"
