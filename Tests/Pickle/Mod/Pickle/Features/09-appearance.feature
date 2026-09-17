# TESTING.md scenario 9. Three mechanisms sharing one window: the label is rewritten on the def,
# the colour is laid down as the button draws, the icon is answered in place of Architect Icons.
Feature: label, colour and icon of a category

  Scenario: a new label reaches the def
    When I relabel the category "Furniture" as "Pickle furniture"
    Then the category "Furniture" is labelled "Pickle furniture"

  Scenario: going back to the original label removes the override
    When I relabel the category "Furniture" as "Pickle furniture"
    And I relabel the category "Furniture" as ""
    Then the mod settings offer nothing to reset

  Scenario: a colour is set and cleared
    When I colour the category "Structure" with palette colour 3
    Then the category "Structure" is drawn in palette colour 3
    When I clear the colour of the category "Structure"
    Then the category "Structure" has no colour

  @requires:com.bymarcin.architecticons
  Scenario: a chosen icon shows without a restart
    When I give the category "Structure" the first icon the picker offers
    Then Architect Icons returns that icon for "Structure" straight away
