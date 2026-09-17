# TESTING.md scenario 5. The arrows move a category among its siblings, so that is what is
# compared, whole list at once; then the Architect window's own tab cache must agree.
Feature: reordering categories

  Scenario: moving a category up
    When I remember where "Furniture" sits among its siblings
    And I move the category "Furniture" up
    Then it comes 1 place earlier among its siblings
    And the Architect window draws the siblings in that order

  Scenario: several moves in a row, without closing anything
    When I remember where "Temperature" sits among its siblings
    And I move the category "Temperature" up
    And I move the category "Temperature" up
    And I move the category "Temperature" down
    Then it comes 1 place earlier among its siblings
    And the Architect window draws the siblings in that order

  Scenario: Reset order puts every category back
    When I remember where "Furniture" sits among its siblings
    And I move the category "Furniture" down
    And I move the category "Furniture" down
    And I reset the category order
    Then the siblings of "Furniture" are back in their remembered order

  @requires:ferny.betterarchitect
  Scenario: a subcategory moves among its own siblings
    Given I create the category "Pickle child A" under "Structure"
    And I create the category "Pickle child B" under "Structure"
    When I remember where "Pickle child B" sits among its siblings
    And I move the category "Pickle child B" up
    Then it comes 1 place earlier among its siblings
