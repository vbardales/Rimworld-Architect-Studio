# TESTING.md scenario 13: the reflection bridges are soft, as the Workshop page claims.
#
# This used to be tagged @wip and played only on a minimal mod list, asserting that three mods were
# ABSENT. Pickle can skip a scenario on the presence of a mod (@requires) but not on its absence, so the
# scenario could only be hidden behind @wip - and a scenario nobody plays proves nothing. It now runs in
# EVERY pass and asserts what is true in all of them: each bridge reports itself exactly when its mod is
# loaded, and the editors work and nothing is logged in whichever world the pass built.
#
# Which world that was is attached to the report ("optional integrations in this pass"):
#   - the minimal pass (no optional mod)   -> all three absent, and the mod still loads and works: §13 itself;
#   - the pass with the optional mods      -> all three present, and every bridge resolved.
# The composition of each pass is the launcher's business: it stages a named mod list. The launcher
# only PRINTS a staged mod that the game dropped, it does not fail the run on it, so this scenario cannot
# tell a pass that lost a mod from a pass that never had it: read the attachment against the pass meant.
Feature: the optional mods, present or absent

  # The same fixture the other map-bound features load, and for the same reason: the scenario names
  # buildings and reads the Architect menu, so it needs a map, and it must be the SAME map every run.
  Background:
    Given the save "test-colony" is loaded

  Scenario: the bridges match what is loaded, the editors work and nothing is logged
    Then each optional integration is reported exactly when its mod is loaded
    And the game log holds nothing from Architect Studio since startup
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Pickle seats"
    And I add "A" to the group "Pickle seats"
    And I add "B" to the group "Pickle seats"
    Then the Architect menu shows the group "Pickle seats" as 1 button
    When I open the category editor
    Then window "Dialog_Categories" is open
