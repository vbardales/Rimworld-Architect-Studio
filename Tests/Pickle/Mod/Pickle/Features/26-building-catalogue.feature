# One photograph per Architect tab, for the building sheet of Nelim's Pickle Tools (NPT): the whole bottom bar of the tab in
# frame, no crop, no per-building cut (owner, 2026-10-18). These are tool captures, not Workshop gallery images: nothing is
# staged, the game's screenshot mode only removes the interface around the menu. Tab defNames come from NPT's reading of the
# Defs on disk (Core, Royalty, Ideology, Biotech, Anomaly, Odyssey): 13 tabs plus the opt-in Unsorted tab. Floors holds terrain,
# Orders and Zone hold no building: skipped. Mod tabs are unknown until NPT's listing ran.
# A tab of a DLC that is not loaded fails with "no Architect category named X": play with the DLCs staged.
# Play with `-Filter '::the Architect tab '` after the maps of NPT's catalogue are staged.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.screenshotmode
Feature: the Architect menu, one tab per photograph

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused
    And god mode is disabled

  Scenario Outline: the Architect tab <tab>
    When I close all dialogs
    And I show the Architect menu on the tab "<tab>"
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect tab <tab>"
    And Nelim's Pickle Tools: screenshot mode is disabled

    Examples:
      | tab         |
      | Structure   |
      | Production  |
      | Furniture   |
      | Power       |
      | Security    |
      | Misc        |
      | Joy         |
      | Ship        |
      | Temperature |
      | Biotech     |
      | Ideology    |
      | Anomaly     |
      | Odyssey     |

  Scenario: the Architect tab Unsorted
    When I turn on the Unsorted tab
    And I close all dialogs
    And I show the Architect menu on the Unsorted tab
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect tab Unsorted"
    And Nelim's Pickle Tools: screenshot mode is disabled
