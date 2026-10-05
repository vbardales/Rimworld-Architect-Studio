# TESTING.md scenario 9. Of the three mechanisms sharing that window, only the icon needs a game:
# it is answered in place of Architect Icons, a real third-party mod, without a restart. The label
# and the colour are stored values, read back in Tests/BehaviorTests.cs.
Feature: label, colour and icon of a category

  @requires:com.bymarcin.architecticons
  Scenario: a chosen icon shows without a restart
    When I give the category "Structure" the first icon the picker offers
    Then Architect Icons returns that icon for "Structure" straight away

  # Seen once in each language, for the parent row's label ("Catégorie parente" is wider than the
  # old "Parente" and must not be clipped). It asserts only that the window draws.
  @review
  Scenario: the appearance window of a created category, with its parent row
    Given the save "test-colony" is loaded
    When I create the category "Pickle tab"
    And I open the appearance window of the category "Pickle tab"
    And I wait 30 ticks
    Then window "Dialog_EditCategory" is open
    And no errors were logged
    When I take a screenshot "appearance window, parent row"
