# TESTING.md scenario 9. Of the three mechanisms sharing that window, only the icon needs a game:
# it is answered in place of Architect Icons, a real third-party mod, without a restart. The label
# and the colour are stored values, read back in Tests/BehaviorTests.cs.
Feature: label, colour and icon of a category

  @requires:com.bymarcin.architecticons
  Scenario: a chosen icon shows without a restart
    When I give the category "Structure" the first icon the picker offers
    Then Architect Icons returns that icon for "Structure" straight away
