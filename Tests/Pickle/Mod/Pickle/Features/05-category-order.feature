# TESTING.md scenario 5. The arrows move a category among its siblings, so that is what is
# compared, whole list at once; then the Architect window's own tab cache must agree - which is
# what keeps these two here. Resetting the order, and moving a subcategory, assert on the stored
# order alone and are in Tests/BehaviorTests.cs.
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
