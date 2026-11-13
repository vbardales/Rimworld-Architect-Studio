# One listing of the buildings of the Architect menu with the gallery mod set (NPT step, owner 2026-11-13): it tells which
# buildings and tabs exist for gallery scenes 2 to 4 (menu over a staged corner). Reads only: nothing is staged or changed.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.colonistrace
Feature: the buildings the Architect menu offers with the gallery mods

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused

  Scenario: the building options are listed
    Given Nelim's Pickle Tools: the building options are listed
