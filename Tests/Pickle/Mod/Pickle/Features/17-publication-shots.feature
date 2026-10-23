# Captures meant for the Workshop page and for nothing else - they assert nothing.
#
# The shots use Nelim's tribe (fixture Nelims-tribe.rws of Nelim's Sanctuary Backlot, the exhibition zone), not the generic functional fixture. The game's
# screenshot mode hides the tab bar, alerts, colonist bar, dev toolbar and Pickle panel, leaving
# each real window over a deliberate colony view rather than over an incidental test map.
#
# The group here is created and filled by the scenario rather than borrowed from the mod list: a
# group belonging to another mod shows its raw defName, which reads as debug output on a store page.
#
# SHOTLIST 1.2.0 (2026-10-18, extended 2026-10-23 with an opening and a closing, director pass by an Opus sub-agent; written
# before anything is played; a scene not listed here is not written). Upload indexes are assigned at reindex (names below
# are story order).
# Story: one working day of Nelim in her Sanctuary, the Architect menu being what keeps her colony's buildings in order.
# Who: Nelim (protagonist) with her dogs Shogun (in all four scenes) and Yuki (scene 3). Where: house door, statue garden,
# tea hut, hearth hall. Which day: one working day; she works off screen between 10 h and 18 h (the gap is part of the story).
# Rhythm: 6 h, 10 h, 18 h, 21 h, each set with `I set the hour`. Variety: wide (1), medium (2, 3), close (4). The windows
# (group editor, category editor, settings, appearance window) are interface shots on `window-backdrop-for-height`,
# cropped, no subtitle, not staged, not scored. The menu stays what it is, the map behind it lives. Echo: the opening and
# the closing share the same door and the same two lit torches (1 outside, 4 from inside).
# Every gesture is caught mid-act by pausing at a set tick through a Pickle step, never by hand; selection brackets and
# labels hidden, nothing selected. No face expression is written for a scene wider than zoom 9 (invisible).
#
# 1 "workshop-1-dawn" (OPENING, WIDE): place `house` (190, 115, zoom 15), 6 h, Clear, game time: none beyond setup (60 ticks).
#   Subject: the colony wakes and its menu is already tidy; says: "the tab bar she tidied waits for the day". Menu: tab
#   bar only, no tab open, Furniture hidden (set by `I hide the Architect tab "Furniture"`), Unsorted on, the Unsorted
#   button hovered so it reads lit; the subtitle names what was hidden.
#   Nelim: Apparel_CollarShirt dyed ochre (200, 140, 50), Apparel_Headwrap dyed teal (40, 120, 130), hairstyle Long loose under
#   the headwrap (hat-hair mod of the Sanctuary map, no bun, no ponytail). Instant: the door has just opened, she stands on
#   the threshold tile facing South; obtained: a move order to the door cell, paused when the door reads open. Shogun beside
#   her facing South (front view). Decor (removed after): two TorchLamp lit by the door (6 h light, glow checked before), PlantPot
#   + Plant_Daylily fully grown. Composition: house centred, door at lower right, river left, menu bar bottom-left.
#   Risk: pawns a few pixels tall; plan: they are never the subject of the thumbnail, the tab grid and the torch glow are.
# 2 "workshop-2-morning-list" (MEDIUM): place `statue-garden` (155, 97, zoom 13), 10 h, Clear, weather steady.
#   Subject: the menu on a group of buildings; says: "she builds from a list she made herself". Menu open on her group, dropdown
#   on the side the build site is not.
#   Nelim: same outfit as scene 1 (same morning). Instant: she has just placed one building of the group from the dropdown and
#   starts work on its frame; action: the Construct job on that frame, tool-less mid-motion, paused two seconds after start.
#   Shogun lies facing South to her left (no sitting pose exists for animals). Decor: PlantPot + Plant_Daylily fully grown, the
#   blueprint frame of one building from the group. Two buildings of two Nelim mods in the group (defNames only from the
#   `d056` listing marked N, buildable without research). Composition: Nelim off-centre right, menu bottom bar full width, statues
#   behind, light from the south. Risk: a defName does not resolve or needs research; plan: take defNames only from the listing,
#   the step fails with the list.
# 3 "workshop-3-evening-shelf" (MEDIUM): place `hut` (141, 72, zoom 9), 18 h, Clear (dusk), the lit lamps warm the porch.
#   Subject: the Unsorted tab with the buildings no other tab offers; says: "nothing she owns gets lost".
#   Nelim: Apparel_Robe dyed plum (110, 50, 120), Apparel_Corset kept, hair let down (Long), default own face. She changed after
#   the day's work (the scene sets the outfit with the wears step; the story says so). Instant: she has just come in from
#   work; action: a haul job carrying a bolt of Cloth to the Dresser of the hut, paused mid-walk. Yuki the husky lies on the
#   porch facing the camera. Decor (removed after): two TorchLamp lit, PlantPot + Plant_Rose fully grown. Orphans: the twelve
#   vanilla buildings switched off in the scenario below plus one building of a Nelim mod, its row first and recoloured so it
#   reads in the thumbnail. Composition: hut at left, Nelim at the door, river behind on the left, menu full width.
#   Risk: the husky leaves the frame or faces away; plan: spawn at its cell, `facing South`, frame then capture with no tick between.
# 4 "workshop-4-night-fire" (CLOSING, CLOSE): place `sleeping-nook`/`fire-pit` side of `hearth-hall` (177, 121, zoom 6.5), 21 h,
#   braziers lit, subjects kept inside their light. Seen from inside the same door as scene 1, the same two TorchLamp lit.
#   Subject: the last feature, her own categories; says: "the day ends, the list stays hers". Menu open on a category she
#   relabelled and recoloured in teal (`I relabel the category` and `I colour the category` steps).
#   Nelim: still the plum robe (continuity from scene 3). Instant: she has just taken her meal; action: the eat job, MealSimple
#   in hand on the way to a chair, paused mid-walk. Shogun asleep on an animal sleeping spot inside the room, facing South,
#   paused once the sleep marks show. Decor (removed after): the sleeping spot, two TorchLamp lit at the door.
#   Risk: no step pauses a pawn mid-eat-job; plan: ask NPT for it (Ticket Manager), fall back to a placed meal and a move order.
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
