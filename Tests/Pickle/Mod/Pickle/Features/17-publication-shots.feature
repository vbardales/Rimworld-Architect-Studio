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
# Rhythm: 6 h, 10 h, 18 h, 21 h, each set with `I set the hour`. Variety (2026-11-19, from the zooms 7, 7, 9, 3.7): medium (1, 2), close (3), wide (4). The windows
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
#   own listing ticket before scenes 2 to 4. The DepMap of this suite holds the hair mod (Roo's HD Hairstyles), Hat Hair Control and VEFA since 2026-11-05; it once lacked the
#   (the three were added to `wsl-deps.sanctuary.map` before the first scene ticket).
#
#   1. "workshop-1-dawn" (OPENING, MEDIUM): place `house` door, framed by `I frame the cell (188, 105) at zoom 7` (108 px per cell; r13 recomposition 2026-11-07 put Nelim low-left beside the menu, its screen estimates are superseded by the CURRENT line below), 6 h, Clear, 30 ticks of setup, a walk of 25 ticks, then paused.
#         the smile is set (MouthSmile, LidCheerful) but the face is a few pixels wide at zoom 7: the plan does not claim it readable, the line says only that she is content.
#         Directed 2026-11-06 after the owner refused r10 ("without a subtitle it does not tell me at all what to look at": whole house, tiny pawns, tiny grid). The subject is
#         now an OPEN menu: `I show the Architect menu on the tab "Structure"` puts a row of building icons about 900 px wide along the bottom (seen in
#         `Evidence/building-tabs-r1`, Structure: Cancel, Deconstruct, Smooth walls, Wall, Door, Fence...) above the tab grid, and the greyed Furniture row is read next to it.
#         Subject: her build bar, tidied: the building row is the thing to read, Furniture is greyed beside it (`I hide the Architect tab "Furniture"`), Unsorted is listed
#         (`I turn on the Unsorted tab`); says: "the tab bar she tidied waits for the day". Not lit: no hover step exists (NPT, 2026-11-06); the subtitle names what was hidden
#         ("Furniture hidden, Unsorted added"). Scene 3 opens the Unsorted tab, so this shot keeps Structure.
#         Nelim: Apparel_CollarShirt dyed ochre (200, 140, 50), Apparel_Headwrap dyed teal (40, 120, 130), hairstyle Roo_Royal_LongHD (canon
#         under closed headwear, `NELIM-PROFIL.md`) shown under the headwrap by Hat Hair Control (3760821540, `wsl-deps.sanctuary.map` line 54), no bun, no ponytail; own face, MouthSmile, LidCheerful.
#         Instant: she has just stepped out of the house and goes to check the daylily; obtained: `stands at (186, 106)`, then `is forced to walk to (184, 103)` and `I let 25 ticks pass`,
#         then the pause; the door reads open while she crosses its cell (N to adjust on the first run).
#         Shogun stands awake at (186, 103) facing South (front view; r11 had him lying in the sleep pose, rated adequacy 3.5, redone as r12), wearing only Apparel_leatherdogcollar (dark brown `#3B2F2F`, validated by the owner 2026-11-06, ticket 47a9).
#         Decor (removed after): two TorchLamp lit by the door (6 h light), PlantPot + Plant_Daylily fully grown at (184, 104). Both pawns asserted in the camera view before the capture.
#         Risk 1: the open panel may cover Nelim or shift the camera; plan: the in-view asserts, one first-run look. Risk 2: the smile is barely readable at 77 px; accepted (written, not a malus).
#         Risk 3: the walk may drop her out of frame or leave the door shut; plan: the in-view asserts. Risk 4: `Furniture` may stay clickable-looking, only greyed; that is the mod's real look.
#         1 CURRENT (aligned with the scenario, 2026-11-17; supersedes the crop and instant lines above where they differ): frame (188, 105) at zoom 7, sun glow held at 0.55 (dawn), she is forced to walk to (184, 103) for 10 ticks then the game is paused;
#         crop 800 x 450 around the building row of the Structure tab and Nelim (series rule, criterion j), menu in the series corner; no blueprint by the house (no step needed). Scenario stable since r21 (TM: do not refile scene 1 unless the plan changes).

#   2. "workshop-2-morning-list" (MEDIUM): place `statue-garden`, frame (156, 98) at zoom 5 (the dropdown covers what sits at screen centre, so the wall and Nelim stand 3 cells west of it), 10 h, Clear, weather steady, 30 ticks of work then paused (a long advance finishes the wall).
#         Subject: the menu on a group she made herself, "Her favourites" (TorchLamp, PlantPot, TorchWallLamp, all buildable at once), its dropdown open on the side away from the site; the group name is not drawn by the game
#         (r11: the button reads "Plant pot..."), so the image says the three buildings of her own list, in the same bar where the wall she is building comes from. Says: "she builds from a short list of her own".
#         Nelim: same outfit as scene 1 (same morning). Instant: she has just placed a Wall blueprint at (153, 99) and builds it (NPT step), caught 30 ticks in, beside the wall at (154, 99), seen in profile (r19 and r20 logs: the builds step puts her east of the wall, Shogun blocks the south cell), smile written (MouthSmile, LidCheerful).
#         Shogun lies at (153, 98) facing West, in profile (series rule: front in 1, profile in 2; r20 turned him from East to West so the collar reads and the body clears the wall), Apparel_leatherdogcollar. Decor (removed after): PlantPot + Plant_Daylily fully grown at (151, 97) (moved west in r19 so Shogun no longer lies on it), a lit TorchLamp at (152, 100) beside the wall, a FermentingBarrel near (150, 100), a Stool near (150, 98) and a ShelfSmall near (152, 96) so the ground round the site is not bare (r21, vanilla, Cat) so the dropdown icons match objects on the ground, statues behind.
#         Composition: Nelim and the wall left of centre, dropdown at the right, bottom bar full width, light from the south; crop 800 x 450 (series). Risk: the free cell for the lamp differs from the guess; plan: `near` picks the nearest free cell, the in-view asserts stay.

#   3. "workshop-3-evening-shelf" (MEDIUM): place `hut` (141, 72, zoom 9), 18 h, Clear (dusk), the lit lamps warm the porch.
#         Subject: the Unsorted tab with the buildings no other tab offers; says: "nothing she owns gets lost".
#         Nelim: Apparel_Robe dyed plum (110, 50, 120), Apparel_Corset kept, hair Roo_HD_Hair_Highpony (canon, no hat so a ponytail is allowed), own face then the smile (MouthSmile + LidCheerful). She changed after
#         the day's work (the scene sets the outfit with the wears step; the story says so). Instant: she has just come in from
#         work; action (Opus pass 2026-11-16, replaces Cloth to the Dresser): a Dresser BLUEPRINT stands inside the hut and she hauls a WoodLog to it, paused mid-walk (the reason is readable, the gesture differs from scene 2). Yuki the husky lies on the
#         porch facing the camera, Apparel_leatherdogcollar dyed fuchsia (NSB YUKI-PROFIL, her visual marker, 2026-11-05). Decor (removed after): two TorchLamp lit, PlantPot + Plant_Rose fully grown. Orphans: the twelve
#         vanilla buildings switched off in the scenario below plus one building of a Nelim mod, its row first and recoloured so it
#         reads in the thumbnail. Composition: hut at left, Nelim at the door, river behind on the left, menu full width.
#         Risk: the husky leaves the frame or faces away; plan: spawn at its cell, `facing South`, frame then capture with no tick between.
#         3 CURRENT (aligned with the scenario, 2026-11-17): orphans = the twelve vanilla buildings switched off in the scenario (the building listing d87f found NO Nelim-mod building in the map, so no recoloured Nelim-mod row is claimed);
#         the Unsorted tab lists them, the Dresser blueprint at (141, 73) and the WoodLog haul carry the story; Yuki faces South at (139, 75); crop 800 x 450, menu in the series corner.

#   4. "workshop-4-night-fire" (CLOSING, CLOSE): place `dining-nook` (176, 108, zoom 3.7), 21 h, Clear (roof on, night inside), game
#         time: 60 ticks of setup, then paused one second after she leaves the table. The same two TorchLamp as scene 1, lit, one each
#         side of the table, are the light of the frame; the room braziers add warmth from the east.
#         Subject: the last feature, her own categories; says: "the day ends, the list stays hers". Menu open on the category
#         "Structure" relabelled and recoloured in teal (`I relabel the category "Structure" as "Her walls"` and
#         `I colour the category "Structure" with palette colour N`, N the teal one of the palette).
#         Nelim: Apparel_Robe dyed plum (110, 50, 120) + Apparel_Corset as in scene 3 (continuity, same evening), hair Roo_HD_Hair_Highpony,
#         own face (`Nelim has her own face`) then the mouth overridden to MouthSmile (tired, content: the day is done; the canon
#         MouthLipsSmirk and the step's MouthSmirkSmile read as a smirk, which the scene does not want). Instant: she has just taken her meal from the table and walks to the chair;
#         action: she carries a MealSimple (carries-the-item step) on a move order to the chair cell, paused mid-walk, no eat job
#         needed. Shogun (Apparel_leatherdogcollar) asleep on an animal sleeping spot (defName to confirm in the building listing) at lower right, facing North (Opus pass 2026-11-16),
#         sleep marks shown. Composition: close, table in the foreground with a bowl, Nelim off-centre left, Shogun lower right,
#         wardrobes behind, light from the two torches. Decor (removed after): the two TorchLamp, the sleeping spot, MealSimple.
#         Risk: the sleeping spot defName, and Shogun waking; plan: read the defName in the listing, spawn him asleep and frame
#         with no tick between. An eat job would need a step from NPT (Ticket Manager); not required.
#         Opus pass 2026-11-16 (director, applied): tight crop on the teal tab and Nelim; Shogun asleep with his back to the camera (`facing North`); the profile smile
#         `the Beauty2 mouth vertical offset is {float}` (NPT b6dec3e) stays OPTIONAL, not played.
#
# SERIES RULES (Opus pass 2026-11-16, applied to the plan, scenarios follow): all four crops are 800 x 450; the Architect menu sits in the same screen corner in every scene;
# animal orientation varies (Shogun front in 1, profile in 2, Yuki front in 3, Shogun asleep back to the camera in 4); each ground shows the result of the previous scene
# (the wall of 2 stands near the hut of 3 as a trace of the day).
# RECOMPOSITION 2026-11-19 (score-gallery on the four clean originals: all REDO, criterion j weak at 268 px on every scene, the menu read as a dark block; replaces the crop and zoom lines above where they differ):
#   - interface scale: every scenario starts with `Nelim's Pickle Tools: the interface scale is 200 percent` (NPT step, sets Prefs.UIScale for the length of the scenario and puts it back), so the building icons are 128 px and the category labels about 28 px
#     instead of 64 and 14 px. The game world is not scaled, only the menu and the bar.
#   - crop: 1280 x 720 from x 0, y 360 (the bottom-left two thirds of the 1920 x 1080 capture), the same in all four scenes, so the menu stays in the same corner. At 268 px wide that crop scales by 0.21: a menu icon reads about 27 px, a dropdown icon about 20 px,
#     the category column about 84 px wide (labels are shapes, not text, at that size; the subtitle names them). The subject of j is the menu icon row plus the greyed or relabelled category beside it.
#   - placement: the building row of the bar occupies y about 830 to 1010 at 200 percent (estimate, measured at the first run); each pawn and animal stands above it, at y about 400 to 780 of the capture, and left of x 1280.
#   - after the first run, `score-gallery` shrinks the CROP to 268 px and reads j on it; a weak j is redone, not offered.
# 1 CURRENT 2026-11-19: frame (188, 105) at zoom 7 (108 px per cell), Nelim walks to (184, 103) (screen about x 528, y 756), Shogun at (186, 103) facing South (about x 744, y 756), torches (185, 104) and (187, 104) in frame; Structure row open, Furniture greyed in the category column. Subject sentence (g): "dawn at her door, her Architect bar open on walls with Furniture greyed"; the subtitle stays "The tab bar she tidied waits for the day."
# 2 CURRENT 2026-11-19: frame (156, 100) at zoom 7, Nelim (153, 100) at screen about x 636, y 540 facing South, wall blueprint (153, 99), the dropdown of "Her favourites" opens at the pointer from x 960 and stays right of her. Subject sentence (g): "she builds a wall while her favourites button lists three buildings (Plant pot, Torch lamp, Wall torch lamp), a torch and a pot already on the ground".
# 3 CURRENT 2026-11-19: frame (141, 74) at zoom 9 (139 px per cell), Nelim (142, 74) at about y 540, Yuki (139, 75) facing South at about x 682, y 401, Dresser blueprint at (141, 73), WoodLog haul to it; the Unsorted rows (13 icons) wrap in three rows at 200 percent and fill y about 640 to 1010, so no pawn stands below y 640. Subject sentence (g): "her Unsorted tab lists thirteen buildings no other tab offers, and she hauls a log to the Dresser blueprint from it"; Yuki's fuchsia collar reads at full size only (not claimed at 268 px, written, no malus).
# 4 CURRENT 2026-11-19: frame (176, 105) at zoom 3.7, Nelim (180, 107) walks to (178, 108), Shogun asleep (179, 108) back to the camera (r8: (179, 110) was outside the view); the category column shows the teal "Her walls" label at the left of the Structure row. Subject sentence (g): "night in the hearth hall, her own teal category Her walls on the bar, she carries her meal to the chair".
# Scene 1: crop about 800 x 450 around the building row and Nelim (criterion j, SUPERSEDED by the crop above); a blueprint line by the house
# only if a step exists, else the crop alone. Scene 2: frame (156, 98) zoom 5 kept, crop 800 x 450, a lit TorchLamp beside the wall so the dropdown icons match objects on the ground.
@review @requires:nelim.sanctuarybacklot @requires:nelim.pickletools.screenshotstudio @requires:nelim.pickletools.screenshotmode @requires:nelim.pickletools.camerazoom @requires:nelim.pickletools.colonistrace @requires:nelim.pickletools.hoversteps @requires:nelim.pickletools.stagedecor
Feature: the windows as a player would show them

  Background:
    Given the save "Nelims-tribe" is loaded
    And game speed is paused
    And god mode is disabled
    And Nelim's Pickle Tools: all filth is cleaned
    And Nelim's Sanctuary: the animals are removed from the sanctuary "exhibition-zone"

  Scenario: interface: the group editor with a group of its own, filled
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

  Scenario: interface: the category editor over the map
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
  Scenario: interface: the Architect menu with the group as one button, its dropdown open
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
  Scenario: interface: the Architect menu on the Unsorted tab, buildings no other tab offers
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
  Scenario: interface: the appearance window of a tab moved under another
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
  Scenario: interface: the category editor with a hidden tab
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
    Given Nelim's Pickle Tools: the interface scale is 200 percent
    When I close all dialogs
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
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Daylily" fully grown at (184, 104)
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Rose" fully grown at (189, 104)
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Rose" fully grown at (190, 104)
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Daylily" fully grown at (188, 103)
    And Nelim's Pickle Tools: I place the decor "Brazier" at (190, 103)
    And Nelim's Pickle Tools: the decor "Brazier" at (190, 103) is lit
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_CollarShirt" dyed rgb (200, 140, 50)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_Headwrap" dyed rgb (40, 120, 130)
    And Nelim's Pickle Tools: "Nelim" hairstyle is "Roo_Royal_LongHD"
    And Nelim's Pickle Tools: the animal "Shogun" is dressed in "Apparel_leatherdogcollar"
    And Nelim's Pickle Tools: the animal "Shogun" stands at (186, 103) facing South
    And Nelim's Pickle Tools: "Nelim" stands at (186, 106) facing South
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
    And I hide the Architect tab "Furniture"
    And I turn on the Unsorted tab
    And I show the Architect menu on the tab "Structure"
    And Nelim's Pickle Tools: the light of the map is logged
    And Nelim's Pickle Tools: the sun glow of the map is held at 0.55
    And Nelim's Pickle Tools: the light of the map is logged
    And Nelim's Pickle Tools: I frame the cell (188, 105) at zoom 7
    And Nelim's Pickle Tools: "Nelim" is forced to walk to (184, 103)
    And Nelim's Pickle Tools: I let 10 ticks pass
    And Nelim's Pickle Tools: the light of the map is refreshed
    And Nelim's Pickle Tools: the light of the map is logged
    And Nelim's Pickle Tools: "Nelim" is in the camera view
    And Nelim's Pickle Tools: "Shogun" is in the camera view
    And Nelim's Pickle Tools: time is paused for the capture
    And I take a screenshot "workshop 1, the dawn"
    And Nelim's Pickle Tools: the decor is removed

  # Gallery scene 2 of 1.2.0, "workshop-2-morning-list": the group "Her favourites" holds three real vanilla defNames (TorchLamp, PlantPot, TorchWallLamp).
  # r13: Shogun lies at (153, 98) facing East, which blocks the cell south of the wall: the NPT step then puts Nelim north of it (first free cell, south first), facing the camera. Nelim builds a Wall blueprint at (153, 99) (NPT c6deb77 step), caught at work (30 ticks, then paused: a long advance finishes the wall).
  # The group button is a vanilla Designator_Dropdown (reads "Plant pot...", first member): its tooltip never carries the group name (r11: no tooltip drawn), so the image
  # says the three buildings of her list, no name. The dropdown opens at the
  # pointer; r9 showed it covers whatever sits at screen centre, so the frame (156, 98) at zoom 5 puts the wall and Nelim 3 cells west of it.
  # Play it with `-Filter '::workshop-2-morning-list'`.
  Scenario: workshop-2-morning-list, she builds from a list she made herself
    Given Nelim's Pickle Tools: the interface scale is 200 percent
    And I close all dialogs
    And I create the group "Her favourites"
    And I add "TorchLamp" to the group "Her favourites"
    And I add "PlantPot" to the group "Her favourites"
    And I add "TorchWallLamp" to the group "Her favourites"
    And I close all dialogs
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 10
    And I set the weather to "Clear"
    And Nelim's Sanctuary: I am at the sanctuary "statue-garden"
    # r15: the r14 capture showed a squirrel, a flamingo, a heron, a donkey and a crow beside Shogun; the frame (156, 98) at zoom 5 spans about x 143-169, z 91-105, cleared before Shogun is placed.
    And Nelim's Pickle Tools: the animals are removed from the area from (142, 90) to (170, 106)
    # r16: r15 still showed grey statues and a hen statue (the place `statue-garden` is a garden of statues) crowding the frame. PiT: hide them for the run with the NPT things steps (SaB owns the place list, no new place). The defNames are unknown, so the things within 12 cells are listed once (log `[things-near]`) and the artificial buildings within 10 cells of (153, 98) are hidden before the decor is placed; read the capture for what else went (bed, torches, walls).
    And Nelim's Pickle Tools: the things within 12 cells of (153, 98) are listed
    And Nelim's Pickle Tools: the artificial buildings within 10 cells of (153, 98) are hidden
    # r19: r18 had Shogun clear of the blueprint but lying on the flower pot (alpaca-like); only change: the pot moves from (153, 97) to (151, 97), west of Shogun.
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Daylily" fully grown near (151, 97)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_CollarShirt" dyed rgb (200, 140, 50)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_Headwrap" dyed rgb (40, 120, 130)
    # r23: r22 rated image 3.5 / adequacy 3.5 (apron reads as a plain cream torso from behind, gloves not visible, Shogun head over the blueprint); only change: VAE_Apparel_BuildersJacket (Shell layer: Neck, Torso, Shoulders, Arms, read in XML by Cat) replaces the apron so the builder look reads on the shoulders and arms. Gloves kept. Vanilla Apparel Expanded 1814987817 is in the map (e8421b5). Hair stays visible (no Overhead piece).
    And Nelim's Pickle Tools: "Nelim" wears "VAE_Apparel_BuildersJacket"
    And Nelim's Pickle Tools: "Nelim" wears "VAE_Handwear_Gloves"
    And Nelim's Pickle Tools: "Nelim" hairstyle is "Roo_Royal_LongHD"
    And Nelim's Pickle Tools: the animal "Shogun" is dressed in "Apparel_leatherdogcollar"
    # r18: r17 showed Shogun lying head down with the body over the Wall blueprint (alpaca-like); only change: facing East (the director plan), same cell (153, 98) so the south cell stays blocked.
    # r20: r19 still read alpaca-like (long neck, head down, facing East); only change: facing West (head toward the pot, profile mirrored), same cell (153, 98) so the south cell stays blocked.
    And Nelim's Pickle Tools: the animal "Shogun" lies at (153, 98) facing West
    And Nelim's Pickle Tools: I place the decor "TorchLamp" near (152, 100)
    # r21: r20 rated image 4 / adequacy 3.5, the ground round the site is bare; only change: three vanilla props (Cat, 2026-11-18, defNames read in XML) west of the wall, clear of Shogun (153, 98), the south cell and Nelim (154, 99): a materials barrel, a stool, a small shelf.
    And Nelim's Pickle Tools: I place the decor "FermentingBarrel" near (150, 100)
    And Nelim's Pickle Tools: I place the decor "Stool" near (150, 98)
    And Nelim's Pickle Tools: I place the decor "ShelfSmall" near (152, 96)
    And Nelim's Pickle Tools: I place a blueprint of "Wall" at (153, 99)
    # r24: r23 rated image 3.5 / adequacy 3.5, Nelim always east of the wall seen from behind in profile; only change: PiT step (StageDecor 0cd003f3) builds from the chosen cell (153, 100), north of the wall, so she faces South, toward the camera, face visible. (153, 100) must stay free: TorchLamp is at (152, 100), no other pawn there.
    And Nelim's Pickle Tools: "Nelim" builds the blueprint at (153, 99) standing at (153, 100)
    And Nelim's Pickle Tools: the colonist bar is hidden
    And Nelim's Pickle Tools: the learning helper is hidden
    And Nelim's Pickle Tools: the resource readout is hidden
    And Nelim's Pickle Tools: the alerts are hidden
    And Nelim's Pickle Tools: the selection brackets are hidden
    And Nelim's Pickle Tools: the item and name labels are hidden
    And Nelim's Sanctuary: Nelim has her own face
    And Nelim's Pickle Tools: "Nelim" mouth is "MouthSmile"
    And Nelim's Pickle Tools: "Nelim" lids are "LidCheerful"
    And Nelim's Pickle Tools: the game is advanced by 30 ticks
    And I show the Architect menu on the category of the group "Her favourites"
    And I open the dropdown of the group "Her favourites" in the Architect menu
    And the Architect designator is disarmed
    # r17: r16 was clean of statues but j stayed weak at 268 px (Nelim and the dropdown text small); only change: zoom 5 to 7 (wall and Nelim about 4 cells west of the frame centre stay clear of the dropdown).
    # r25: at 200 percent the menu hid her face (rows from y 478); frame two cells SOUTH of her (north is up, so the centre going south lifts her on screen). r24 of this line went north by mistake.
    # r25 (scene 2): ticket fa9b passed but Shogun (153, 98) fell behind the menu; only change: centre (156, 98) to (156, 97) lifts the site one cell on screen.
    # r26 (scene 2): ticket 321e passed but Shogun (153, 98) still sat behind the dropdown (tail only); only change: centre (156, 97) to (156, 95). Scene 4 in 321e showed Shogun asleep with Z marks (good) but the label read Structure, not Her walls: scenes 2 and 4 run as two tickets from now on (r9 5eb0 alone showed Her walls).
    And Nelim's Pickle Tools: I frame the cell (156, 95) at zoom 7
    And Nelim's Pickle Tools: "Nelim" is in the camera view
    And Nelim's Pickle Tools: "Shogun" is in the camera view
    And Nelim's Pickle Tools: time is paused for the capture
    And I take a screenshot "workshop 2, the morning list"
    And Nelim's Pickle Tools: the decor is removed

  # Gallery scene 3 of 1.2.0, "workshop-3-evening-shelf" (written 2026-11-13). Orphans: the twelve vanilla buildings of image 6 switched off
  # (no Nelim-mod building in the listing). Cells are proposals, NOT verified; the hut Dresser cell is a guess, the first run lists the free cells.
  # Play it with `-Filter '::workshop-3-evening-shelf'`.
  Scenario: workshop-3-evening-shelf, nothing she owns gets lost
    Given Nelim's Pickle Tools: the interface scale is 200 percent
    And the building "PlantPot" has its default Architect designator switched off
    And the building "SleepingSpot" has its default Architect designator switched off
    And the building "Bedroll" has its default Architect designator switched off
    And the building "Table1x2c" has its default Architect designator switched off
    # r8 (score-gallery REDO of r7: the log said "5 designator(s) left" but only the Plant pot drew; Dresser and Bookcase never showed in r6 or r7, cause not proven, probably research-locked): the four buildings r6 DREW (Plant pot, Sleeping spot, Bedroll, Table (1x2)) so the bar is Cancel, Deconstruct and three icons in the 1280 crop.
    # r5: those 12 switch-offs make the buildings ORPHANS of Unsorted (they are what the tab shows); the tab also listed about 30 Ideology and Odyssey items that filled 5 rows at 200 percent and hid every pawn. NPT step 1204594e limits the tab to the 12 (nothing else is shown).
    # r6: the tab does not exist before it is turned on (r5 failed: no Architect tab 'AS_Unsorted'), so the limit comes after.
    When I turn on the Unsorted tab
    And Nelim's Pickle Tools: the architect tab "AS_Unsorted" shows only the defs "PlantPot, SleepingSpot, Bedroll, Table1x2c"
    And I close all dialogs
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 18
    And I set the weather to "Clear"
    And Nelim's Sanctuary: I am at the sanctuary "hut"
    And Nelim's Pickle Tools: the free standable cells of the area from (137, 68) to (146, 76) are listed
    And Nelim's Pickle Tools: I place the decor "TorchLamp" near (140, 74)
    And Nelim's Pickle Tools: I place the decor "TorchLamp" near (143, 74)
    # r4: r3 rated image 3.5 / adequacy 3.5, Yuki collar hidden behind the flower pot; only change: the PlantPot moves from (139, 74) to near (137, 74), west of Yuki (139, 75), so the fuchsia collar reads.
    And Nelim's Pickle Tools: I place the decor "PlantPot" holding "Plant_Rose" fully grown near (137, 74)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_Robe" dyed rgb (110, 50, 120)
    And Nelim's Pickle Tools: "Nelim" hairstyle is "Roo_HD_Hair_Highpony"
    # r13: NPT animal steps for the collar (dye and proofs), NSB fuchsia #D1008F.
    And Nelim's Pickle Tools: the animal "Yuki" is dressed in "Apparel_leatherdogcollar" dyed "#D1008F"
    And Nelim's Pickle Tools: the render tree of the animal "Yuki" draws "Apparel_leatherdogcollar"
    And Nelim's Pickle Tools: the animal "Yuki" is wearing "Apparel_leatherdogcollar"
    And Nelim's Pickle Tools: the animal "Yuki" stands at (141, 75) facing South
    And Nelim's Pickle Tools: "Nelim" stands at (142, 74) facing South
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
    And I show the Architect menu on the Unsorted tab
    And the Architect designator is disarmed
    # r8: zoom 11 to 14 centred on (140, 74) so Yuki and her collar read larger (r7 at 11: collar unreadable); the Dresser blueprint never drew at (141, 73) in r6 or r7 (Dresser is probably research-locked), so the blueprint is a Bedroll at (144, 72) (r8 at (143, 73) failed: "Space already occupied", the 1x2 bedroll reaches the TorchLamp at (143, 74); (144, 72) and (144, 73) are in the free-cells list).
    # r10 (score-gallery REDO of r9 `f56f`: green, but at zoom 14 the whole hut shows and the pawns are tiny, collar unreadable, no haul readable; zoom 14 was the wrong direction, a LOWER number is closer (scene 4 uses 3.7); only change: centre (140, 74) to (142, 74) and zoom 14 to 8 so Nelim, her hauled log and Yuki fill the frame; the two blue blobs on the floor are not explained, check them on the new capture).
    # r11 (score-gallery REDO of r10 `9350`: green, notes image 3.5, understanding 3.5, decoration 3.5, adequacy 3; Dresser only a drawer at the right edge, Yuki about 40 px, two blue blobs on the floor at about (136, 73) and (144, 78) by the 67 px per cell of zoom 8, cause unknown; only change: centre (142, 74) to (141, 74) and zoom 8 to 6, 89 px per cell, so Yuki and her collar grow, the hut Dresser at the right edge comes in, and both blobs fall outside (west of the panel edge, south behind the designator row).
    # r12 (score-gallery REDO of r11 `a1e6`: green, image 3, understanding 3, decoration 3.5, adequacy 2.5; the capture is the LEFT 1280 px of a 1920 px screen, so the view centre is at x 960 of the crop, not 640: at zoom 6 (90 px per cell) Nelim sat on the right edge, the Dresser (cell 146) fell outside and a blue-flame brazier of the hut stayed at (136, 73). Only change: centre (141, 74) to (144, 74), so cell x = 960 + (cell - 144) * 90: Yuki (139) 510, Nelim (142) 780, Bedroll blueprint (144) 960, Dresser (146) 1140, brazier (136) 240 hidden under the panel. Yuki collar still unread in r10 and r11: asked of NPT.)
    # r13 (score-gallery REDO of r12 `3fe4`: green, no blue blobs, Unsorted tab and icons readable, notes image 3.5, understanding 3.5, decoration 3.5, adequacy 3; Yuki about 50 px with the collar unreadable, haul of the bolt ambiguous, Dresser only partly at the right edge. Changes: zoom 6 to 5 (108 px per cell: Yuki 141 at 636, Nelim 142 at 744, Dresser 146 at 1176), Yuki moved from (139, 75) to (141, 75) beside Nelim, collar by the NPT animal steps, things on the area logged to name any prop left.)
    # r14 (score-gallery REDO of r13 `16c7`: green, image 3, understanding 3, decoration 3.5, adequacy 2.5; the crop is screen y 360 to 1080 and the icon row fills screen y 830 to 1010, so only cells with z >= 73 are visible above it at zoom 5 (108 px per cell, centre z at screen y 540): at centre z 74 Yuki (z 75) had her ears and tail cut at the top edge, the haul ended on the row's edge (blueprint z 72-73 at y 756-864) and the Dresser showed as two drawers. Only change: centre (144, 74) to (144, 75) and the Bedroll blueprint (1x2, covers z and z+1) from (144, 72) to (144, 74): Yuki (141, 75) at screen x 636, y 540; Nelim (142, 74) at x 744, y 648; blueprint (144, 74-75) at x 960, y 540 to 648; Dresser (146, 73-74) at x 1176, y 648 to 756. Risk: (144, 74) or (144, 75) occupied: fail fast "Space already occupied", then use (144, 73).)
    And Nelim's Pickle Tools: I frame the cell (144, 75) at zoom 5
    And Nelim's Pickle Tools: I place a blueprint of "Bedroll" at (144, 74)
    And Nelim's Pickle Tools: "Nelim" is forced to haul 1 of the item "WoodLog" to (144, 74)
    And Nelim's Pickle Tools: I let 10 ticks pass
    And Nelim's Pickle Tools: the animal "Yuki" stands at (141, 75) facing South
    And Nelim's Pickle Tools: the things on the area from (136, 72) to (146, 79) are logged
    And Nelim's Pickle Tools: "Nelim" is in the camera view
    And Nelim's Pickle Tools: "Yuki" is in the camera view
    And Nelim's Pickle Tools: time is paused for the capture
    And I take a screenshot "workshop 3, the evening shelf"
    And Nelim's Pickle Tools: the decor is removed

  # Gallery scene 4 of 1.2.0, "workshop-4-night-fire" (written 2026-11-13). Sleeping spot: AnimalSleepingSpot (1x1, N, Core Furniture in the listing).
  # Cells are proposals, NOT verified. Play it with `-Filter '::workshop-4-night-fire'`.
  Scenario: workshop-4-night-fire, the day ends, the list stays hers
    Given Nelim's Pickle Tools: the interface scale is 200 percent
    When I close all dialogs
    And I relabel the category "Structure" as "Her walls"
    And I colour the category "Structure" with palette colour 4
    And Nelim's Pickle Tools: the eclipse of the map is ended
    And I set the hour to 21
    And I set the weather to "Clear"
    And Nelim's Sanctuary: I am at the sanctuary "dining-nook"
    And Nelim's Pickle Tools: the free standable cells of the area from (172, 104) to (181, 112) are listed
    And Nelim's Pickle Tools: I place the decor "TorchLamp" near (174, 106)
    And Nelim's Pickle Tools: I place the decor "TorchLamp" near (178, 106)
    And Nelim's Pickle Tools: I place the decor "AnimalSleepingSpot" near (179, 108)
    And Nelim's Pickle Tools: "Nelim" wears "Apparel_Robe" dyed rgb (110, 50, 120)
    And Nelim's Pickle Tools: "Nelim" hairstyle is "Roo_HD_Hair_Highpony"
    And Nelim's Pickle Tools: the animal "Shogun" is dressed in "Apparel_leatherdogcollar"
    And Nelim's Pickle Tools: the animal "Shogun" lies at (179, 108) facing North
    And Nelim's Pickle Tools: "Nelim" stands at (180, 107) facing South
    And Nelim's Pickle Tools: I let 60 ticks pass
    And Nelim's Pickle Tools: the colonist bar is hidden
    And Nelim's Pickle Tools: the learning helper is hidden
    And Nelim's Pickle Tools: the resource readout is hidden
    And Nelim's Pickle Tools: the alerts are hidden
    And Nelim's Pickle Tools: the selection brackets are hidden
    And Nelim's Pickle Tools: the item and name labels are hidden
    And Nelim's Sanctuary: Nelim has her own face
    And I show the Architect menu on the tab "Structure"
    And the Architect designator is disarmed
    # r7: at 200 percent the two menu rows hid her face (y 590 up); the centre goes south to (176, 105) so she (z 107) stands two cells above it, y about 426. The first try went north (176, 112) and dropped her out of the view.
    # r8: ticket fa9b put Shogun at (179, 110), the top edge of the view (cells 168-184 x 100-110): drawn above the picture. Shogun and the sleeping spot move to (179, 108), Nelim starts (180, 107) and walks to (178, 108), all in view above the menu. r9: ticket 5209 said (177, 106) not standable (the torch near (178, 106) took it); the free list shows 178-181 free on rows 107 to 109.
    # r10 (scene 4): ticket 5eb0 passed, capture rated image 3.5 / adequacy 3.5: Shogun cut by the top edge, meal not visible; only change: centre (176, 105) to (176, 106) puts everything one cell lower on screen (Shogun fully in, Nelim face still above the menu).
    And Nelim's Pickle Tools: I frame the cell (176, 106) at zoom 3.7
    And Nelim's Pickle Tools: "Nelim" carries the item "MealSimple"
    And Nelim's Pickle Tools: "Nelim" is forced to walk to (178, 108)
    And Nelim's Pickle Tools: "Nelim" face kit is "smile"
    And Nelim's Pickle Tools: "Nelim" face is held neutral
    And Nelim's Pickle Tools: "Nelim" face state is logged
    And Nelim's Pickle Tools: I let 10 ticks pass
    And Nelim's Pickle Tools: "Nelim" face state is logged
    And Nelim's Pickle Tools: "Nelim" is in the camera view
    And Nelim's Pickle Tools: "Shogun" is in the camera view
    And Nelim's Pickle Tools: time is paused for the capture
    And I take a screenshot "workshop 4, the night fire"
    And Nelim's Pickle Tools: the decor is removed
