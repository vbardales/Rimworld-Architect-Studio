# TESTING.md scenario 3. The drop is replayed through the callback the editor registered on its
# last repaint, so the insertion-index conversion under test is the mod's own. The review scenario
# records the rendered member rows before and after that callback; no separate manual procedure is
# left. Play this feature with wsl-deps.avec-revues.map.
@review @requires:nelim.pickletools.filmticks
Feature: dragging a group member

  Background:
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Seats"
    And I add "A" to the group "Seats"
    And I add "B" to the group "Seats"
    And I add "C" to the group "Seats"
    And I add "D" to the group "Seats"
    Then the group "Seats" has at least 4 members

  Scenario: the first member dragged below the last lands last
    Given the group "Seats" is ordered "A, B, C, D"
    # Below the fourth row is insertion index 4. Landing third is the old off-by-one.
    When I select the group "Seats" in the editor
    And Nelim's Pickle Tools: I film every 1 ticks as "group-member-first-to-last"
    And I drag member 1 of the group "Seats" to insertion index 4
    And I wait 30 ticks
    And Nelim's Pickle Tools: I stop filming
    Then the group "Seats" holds "B, C, D, A" in that order
    And the Architect menu lists the group "Seats" in the same order as the editor

  Scenario: the last member dragged above the first lands first
    Given the group "Seats" is ordered "A, B, C, D"
    When I drag member 4 of the group "Seats" to insertion index 0
    Then the group "Seats" holds "D, A, B, C" in that order
    And the Architect menu lists the group "Seats" in the same order as the editor

  Scenario: a one-row drag downwards moves exactly one row
    Given the group "Seats" is ordered "A, B, C, D"
    When I drag member 2 of the group "Seats" to insertion index 3
    Then the group "Seats" holds "A, C, B, D" in that order
