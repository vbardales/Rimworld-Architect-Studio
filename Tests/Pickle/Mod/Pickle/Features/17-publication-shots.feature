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
# labels hidden, nothing selected. Nelim smiles in every scene (owner, 2026-10-24): `Nelim has her own face`, then `mouth is "MouthSmile"` and `lids are "LidCheerful"` as the last steps before the
# screenshot, even where the face is too small to read.
#
# STEPS (answer of NPT, 2026-10-25; compiled by NPT, NOT played yet, fidelity unverified unless said):
#   pause mid-job = no dedicated step; recipe: `<pawn> is forced into the job "Construct" on ...` or `is forced to walk to`, then
#   `I let N ticks pass`, then `time is paused for the capture` (N in game ticks). A door stays open only while a pawn stands
#   on it and the game runs (not verified paused): scene 1 uses `stands at (x, z)` on the door cell; NPT can build "advance N
#   ticks then pause" if the recipe is not exact enough.
#   held item: `Nelim's Pickle Tools: {string} carries the item {string}`; a move order may make the pawn drop it (not verified),
#   so the item is given after the walk order has run for a few ticks, just before the pause.
#   animal collar: `the animal "Shogun" is dressed in "Apparel_leatherdogcollar"`; asleep: `the animal "Shogun" lies at (x, z)
#   facing South` (new, on a cell: for scene 4 the cell of the sleeping spot, the spot itself is decor); hover: package HoverSteps
#   (not verified against the Architect bar or screenshot mode: needs a ticket); smile: `"Nelim" mouth is "MouthSmile"` and
#   `"Nelim" lids are "LidCheerful"` exist, `LidCheerful` in the Sanctuary map (VEFA) not verified: run `face state is logged`.
#   Building defNames: ticket d056 (PickleTools, 2026-10-18) ended exit 3 with no evidence, so no listing exists; ArS files its
#   own listing ticket before scenes 2 to 4. The DepMap of this suite still lacks the hair mod (Roo's HD Hairstyles), the
#   hat-hair mod and VEFA: added to `wsl-deps.sanctuary.map` (or a copy) before the first scene ticket.
#
# 1 "workshop-1-dawn" (OPENING, WIDE): place `house` (190, 115, zoom 15), 6 h, Clear, game time: none beyond setup (60 ticks).
#   Subject: the colony wakes and its menu is already tidy; says: "the tab bar she tidied waits for the day". Menu: tab
#   bar only, no tab open, Furniture hidden (set by `I hide the Architect tab "Furniture"`), Unsorted on, the Unsorted
#   button hovered so it reads lit; the subtitle names what was hidden.
#   Nelim: Apparel_CollarShirt dyed ochre (200, 140, 50), Apparel_Headwrap dyed teal (40, 120, 130), hairstyle Roo_Royal_LongHD (canon
#   under closed headwear, `NELIM-PROFIL.md`) shown under the headwrap by the hat-hair mod of the Sanctuary map, no bun, no ponytail. Instant: the door has just opened, she stands on
#   the threshold tile facing South; obtained: a move order to the door cell, paused when the door reads open. Shogun beside
#   her facing South (front view), wearing only Apparel_leatherdogcollar (NSB rule, `CATALOGUE-ANIMAUX.md`). Decor (removed after): two TorchLamp lit by the door (6 h light, glow checked before), PlantPot
#   + Plant_Daylily fully grown. Composition: house centred, door at lower right, river left, menu bar bottom-left.
#   Risk: pawns a few pixels tall; plan: they are never the subject of the thumbnail, the tab grid and the torch glow are.
# 2 "workshop-2-morning-list" (MEDIUM): place `statue-garden` (155, 97, zoom 13), 10 h, Clear, weather steady.
#   Subject: the menu on a group of buildings; says: "she builds from a list she made herself". Menu open on her group, dropdown
#   on the side the build site is not.
#   Nelim: same outfit as scene 1 (same morning). Instant: she has just placed one building of the group from the dropdown and
#   starts work on its frame; action: the Construct job on that frame, tool-less mid-motion, paused two seconds after start.
#   Shogun lies facing South to her left (no sitting pose exists for animals), Apparel_leatherdogcollar. Decor: PlantPot + Plant_Daylily fully grown, the
#   blueprint frame of one building from the group. Two buildings of two Nelim mods in the group (defNames only from the
#   `d056` listing marked N, buildable without research). Composition: Nelim off-centre right, menu bottom bar full width, statues
#   behind, light from the south. Risk: a defName does not resolve or needs research; plan: take defNames only from the listing,
#   the step fails with the list.
# 3 "workshop-3-evening-shelf" (MEDIUM): place `hut` (141, 72, zoom 9), 18 h, Clear (dusk), the lit lamps warm the porch.
#   Subject: the Unsorted tab with the buildings no other tab offers; says: "nothing she owns gets lost".
#   Nelim: Apparel_Robe dyed plum (110, 50, 120), Apparel_Corset kept, hair Roo_HD_Hair_Highpony (canon, no hat so a ponytail is allowed), own face then the smile (MouthSmile + LidCheerful). She changed after
#   the day's work (the scene sets the outfit with the wears step; the story says so). Instant: she has just come in from
#   work; action: a haul job carrying a bolt of Cloth to the Dresser of the hut, paused mid-walk. Yuki the husky lies on the
#   porch facing the camera, Apparel_leatherdogcollar. Decor (removed after): two TorchLamp lit, PlantPot + Plant_Rose fully grown. Orphans: the twelve
#   vanilla buildings switched off in the scenario below plus one building of a Nelim mod, its row first and recoloured so it
#   reads in the thumbnail. Composition: hut at left, Nelim at the door, river behind on the left, menu full width.
#   Risk: the husky leaves the frame or faces away; plan: spawn at its cell, `facing South`, frame then capture with no tick between.
# 4 "workshop-4-night-fire" (CLOSING, CLOSE): place `dining-nook` (176, 108, zoom 3.7), 21 h, Clear (roof on, night inside), game
#   time: 60 ticks of setup, then paused one second after she leaves the table. The same two TorchLamp as scene 1, lit, one each
#   side of the table, are the light of the frame; the room braziers add warmth from the east.
#   Subject: the last feature, her own categories; says: "the day ends, the list stays hers". Menu open on the category
#   "Structure" relabelled and recoloured in teal (`I relabel the category "Structure" as "Her walls"` and
#   `I colour the category "Structure" with palette colour N`, N the teal one of the palette).
#   Nelim: Apparel_Robe dyed plum (110, 50, 120) + Apparel_Corset as in scene 3 (continuity, same evening), hair Roo_HD_Hair_Highpony,
#   own face (`Nelim has her own face`) then the mouth overridden to MouthSmile (tired, content: the day is done; the canon
#   MouthLipsSmirk and the step's MouthSmirkSmile read as a smirk, which the scene does not want). Instant: she has just taken her meal from the table and walks to the chair;
#   action: she carries a MealSimple (carries-the-item step) on a move order to the chair cell, paused mid-walk, no eat job
#   needed. Shogun (Apparel_leatherdogcollar) asleep on an animal sleeping spot (defName to confirm in the building listing) at lower right, facing South,
#   sleep marks shown. Composition: close, table in the foreground with a bowl, Nelim off-centre left, Shogun lower right,
#   wardrobes behind, light from the two torches. Decor (removed after): the two TorchLamp, the sleeping spot, MealSimple.
#   Risk: the sleeping spot defName, and Shogun waking; plan: read the defName in the listing, spawn him asleep and frame
#   with no tick between. An eat job would need a step from NPT (Ticket Manager); not required.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.screenshotmode @requires:nelim.pickletools.camerazoom @requires:nelim.pickletools.colonistrace @requires:nelim.pickletools.hoversteps @requires:nelim.pickletools.stagedecor
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

  # Gallery scene 1 of 1.2.0, "workshop-1-dawn" (opening, wide; plan in the header). Cells of the door and the decor are the proposal of NSB
  # (2026-10-29, from Nelims-tribe.rws, NOT verified in game): door Door59718 at (186, 105), torches (185, 104) and (187, 104),
  # pot (184, 104); the first run lists the free cells. The tab bar is kept, the other interface is hidden by its own steps (screenshot
  # mode would hide the tab bar and leave the hover nothing to touch). Play it with `-Filter '::workshop-1-dawn'`.
  Scenario: workshop-1-dawn, the tab bar she tidied waits for the day
    When I close all dialogs
    And I hide the Architect tab "Furniture"
    And I turn on the Unsorted tab
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 6
    And I set the weather to "Clear"
    And Nelim's Pickle Tools: the temperature of the map is 20 degrees
    And Nelim's Sanctuary: I am at the sanctuary "house"
    And Nelim's Pickle Tools: the free standable cells of the area from (182, 100) to (192, 106) are listed
    And Nelim's Pickle Tools: I place the decor "TorchLamp" at (185, 104)
    And Nelim's Pickle Tools: the decor "TorchLamp" at (185, 104) is lit
    And Nelim's Pickle Tools: I place the decor "TorchLamp" at (187, 104)
    And Nelim's Pickle Tools: the decor "TorchLamp" at (187, 104) is lit
    And Nelim's Pickle Tools: I place the decor "PlantPot" at (184, 104)
    And Nelim's Pickle Tools: I place the decor "Plant_Daylily" at (184, 104) fully grown
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_CollarShirt" dyed rgb (200, 140, 50)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_Headwrap" dyed rgb (40, 120, 130)
    And Nelim's Pickle Tools: "Nelim" hairstyle is "Roo_Royal_LongHD"
    And Nelim's Pickle Tools: the animal "Shogun" is dressed in "Apparel_leatherdogcollar"
    And Nelim's Pickle Tools: the animal "Shogun" lies at (186, 103) facing South
    And Nelim's Pickle Tools: "Nelim" stands at (186, 105) facing South
    And Nelim's Pickle Tools: I let 30 ticks pass
    And Nelim's Pickle Tools: the colonist bar is hidden
    And Nelim's Pickle Tools: the learning helper is hidden
    And Nelim's Pickle Tools: the resource readout is hidden
    And Nelim's Pickle Tools: the alerts are hidden
    And Nelim's Pickle Tools: the selection brackets are hidden
    And Nelim's Pickle Tools: the item and name labels are hidden
    And Nelim's Sanctuary: Nelim has her own face
    And Nelim's Pickle Tools: "Nelim" mouth is "MouthSmile"
    And Nelim's Pickle Tools: "Nelim" lids are "LidCheerful"
    And Nelim's Pickle Tools: I hover over the tooltip containing "Unsorted"
    And Nelim's Pickle Tools: time is paused for the capture
    And I take a screenshot "workshop 1, the dawn"
    And Nelim's Pickle Tools: the decor is removed
