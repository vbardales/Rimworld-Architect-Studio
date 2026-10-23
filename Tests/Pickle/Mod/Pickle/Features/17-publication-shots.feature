# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The shots use Nelim's tribe (fixture Nelims-tribe.rws of Nelim's Sanctuary Backlot, the exhibition zone), not the generic functional fixture. The game's
# screenshot mode hides the tab bar, alerts, colonist bar, dev toolbar and Pickle panel, leaving
# each real window over a deliberate colony view rather than over an incidental test map.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
#
# SHOTLIST 1.2.0 (2026-10-18, written before anything is played; a scene not listed here is not written).
# Story: one working day of Nelim in her tribe, the Architect menu being what keeps her colony's buildings in order.
# Morning: she picks what to build from a tab she arranged. Dusk: she gathers the leftovers in the Unsorted tab. The
# windows (images 1, 2, 3, 5) are interface shots on `window-backdrop-for-height`, cropped, no subtitle, not staged.
# Only the two menu shots below are staged scenes; the menu stays what it is, the map behind it lives. Who: Nelim, alone
# with her dogs. Where: the Sanctuary, statue garden then tea hut. Which day: one working day. Rhythm: episode 1 at 10 h,
# episode 2 at 18 h, each set with `I set the hour` (8 hours between, the light turns from white to amber).
#
# Image 4 "the morning list" (episode 1): place `statue-garden` (155, 97, zoom 13), 10 h, Clear, weather steady.
#   Subject: the menu on a group of buildings; says: "she builds from a list she made herself".
#   Nelim: Apparel_CollarShirt dyed ochre (200, 140, 50), Apparel_Headwrap dyed teal (40, 120, 130), hairstyle Long (loose, shown under the
#   headwrap by Show Hair Under Stuff, RaccoonCuddler.ShowHair, already in the Sanctuary map: hat rule, no bun, no ponytail),
#   smile (MouthSmile + LidCheerful), carries a WoodLog towards the build site (action). Shogun the labrador sits left
#   of her facing South (front view). Decor (placed, removed after the take): PlantPot with Plant_Daylily fully grown,
#   an Apparel-free corner, two buildings from the group (one from each of two Nelim mods, picked in the d056 listing,
#   buildable without research). Composition: Nelim off-centre right, menu bottom bar full width, statues behind, light
#   from the south. Mods: Sanctuary map + Better Architect Menu + nelim.* building mods. Risk: a building defName does not
#   resolve or needs research; plan: take defNames only from the d056 listing marked N, the step fails with the list.
#
# Image 6 "the evening shelf" (episode 2): place `hut` (141, 72, zoom 9), 18 h, Clear, the lit lamps warm the porch.
#   Subject: the Unsorted tab with the buildings no other tab offers; says: "nothing she owns gets lost".
#   Nelim: Apparel_Robe dyed plum (110, 50, 120), Apparel_Corset kept, hair let down (Long), tattoo Face_Stars, calm
#   (default own face), carries a bolt of Cloth (action, different from image 4). Yuki the husky lies on the porch facing
#   the camera. Decor (removed after): two TorchLamp lit, PlantPot with Plant_Rose fully grown. Orphans: the twelve vanilla
#   buildings already switched off in the scenario below plus one building of a Nelim mod switched off the same way.
#   Composition: hut at left, Nelim at the door, river behind on the left, menu full width. Mods: as image 4.
#   Risk: the husky leaves the frame or faces away; plan: spawn at its cell, `facing South`, frame then capture with no tick between.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.screenshotmode
Feature: the windows as a player would show them

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused
    And god mode is disabled
    And Nelim's Pickle Tools: all filth is cleaned
    And Nelim's Sanctuary: the animals are removed from the sanctuary "exhibition-zone"

  Scenario: the group editor with a group of its own, filled
    Given four buildings "A", "B", "C" and "D" from one category, in no group
    When I create the group "Monolith machines"
    And I add "A" to the group "Monolith machines"
    And I add "B" to the group "Monolith machines"
    And I add "C" to the group "Monolith machines"
    And I select the group "Monolith machines" in the editor
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "group editor, a filled group"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the category editor over the map
    When I close all dialogs
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "category editor"
    And Nelim's Pickle Tools: screenshot mode is disabled

  Scenario: the settings page over the map
    When I close all dialogs
    And I open the Architect Studio settings through the shortcut and let it draw
    Then window "Dialog_ModSettings" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "settings page"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # The result, not the tool: the Architect menu with the group as one button, its dropdown open.
  # A menu is shown for what it is, so nothing is staged beyond the group the scenario builds itself.
  Scenario: the Architect menu with the group as one button, its dropdown open
    Given four buildings "A", "B", "C" and "D" from one category, in no group, that the player can already build
    When I create the group "Workshop favourites"
    And I add "A" to the group "Workshop favourites"
    And I add "B" to the group "Workshop favourites"
    And I add "C" to the group "Workshop favourites"
    And I close all dialogs
    And I show the Architect menu on the category of the group "Workshop favourites"
    And I open the dropdown of the group "Workshop favourites" in the Architect menu
    And the Architect designator is disarmed
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect menu, a group opened"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Gallery image 6 (1.1.0, the opt-in Unsorted tab; session decision, 2026-10-14). Plan: Nelim's tribe, same frame and
  # light as image 4 (cell 213,168, zoom 21, noon, clear), so the two menu shots read as a pair. Subject: the Architect
  # menu on the Unsorted tab, its bottom bar full of furniture no other tab offers (twelve vanilla buildings are switched
  # off for the length of the scenario to be the orphans a modded colony collects). Says: nothing stays out of reach.
  # A menu is shown for what it is: no colonists or decor staged. Needs `wsl-deps.sanctuary.map` and the setting turned on.
  Scenario: the Architect menu on the Unsorted tab, buildings no other tab offers
    Given the building "Stool" has its default Architect designator switched off
    And the building "DiningChair" has its default Architect designator switched off
    And the building "EndTable" has its default Architect designator switched off
    And the building "Dresser" has its default Architect designator switched off
    And the building "Couch" has its default Architect designator switched off
    And the building "Armchair" has its default Architect designator switched off
    And the building "TorchLamp" has its default Architect designator switched off
    And the building "PlantPot" has its default Architect designator switched off
    And the building "Bedroll" has its default Architect designator switched off
    And the building "SleepingSpot" has its default Architect designator switched off
    And the building "Table2x2c" has its default Architect designator switched off
    And the building "Table1x2c" has its default Architect designator switched off
    When I turn on the Unsorted tab
    And I close all dialogs
    And I show the Architect menu on the Unsorted tab
    And the Architect designator is disarmed
    And Nelim's Pickle Tools: I frame the cell (213, 168) at zoom 21
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "architect menu, the unsorted tab"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Candidate for the next gallery (1.0.7 feature, Ali50's request): a tab the game ships, moved under another one
  # from its own appearance window. Needs both integrations, hence `wsl-deps.sanctuary-bam.map`; play it with
  # `-Filter '::the appearance window of a tab moved under another'`.
  @requires:ferny.betterarchitect @requires:com.bymarcin.architecticons
  Scenario: the appearance window of a tab moved under another
    When I close all dialogs
    And I move the category "Joy" under the parent "Structure"
    And I open the appearance window of the category "Joy"
    And I wait 30 ticks
    Then window "Dialog_EditCategory" is open
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "appearance window, a tab moved under another"
    And Nelim's Pickle Tools: screenshot mode is disabled

  # Candidate for gallery image 2 (1.2.0, hide a whole tab; session decision, 2026-10-18). Same frame and light as the
  # category editor shot, but one vanilla tab is hidden, so the row reads "Furniture (Hidden)" and greyed: the new feature
  # shows in the list a player opens first. Play it with `-Filter '::the category editor with a hidden tab'`.
  # Orders and Zone hold no building, so the editor greys them too (count 0): they are sent to the end of the list and
  # the window is shortened so they scroll out of frame, leaving "Furniture (Hidden)" the only greyed row in view.
  Scenario: the category editor with a hidden tab
    When I close all dialogs
    And I hide the Architect tab "Furniture"
    And I move the category "Orders" to the end of its siblings
    And I move the category "Zone" to the end of its siblings
    And I open the category editor
    And I wait 30 ticks
    Then window "Dialog_Categories" is open
    When the category editor shows 14 rows
    When Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 12
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: all animals are removed
    And Nelim's Sanctuary: I am at the sanctuary "window-backdrop-for-height"
    And Nelim's Pickle Tools: screenshot mode is enabled around the open windows
    And I take a screenshot "category editor, a hidden tab"
    And Nelim's Pickle Tools: screenshot mode is disabled
