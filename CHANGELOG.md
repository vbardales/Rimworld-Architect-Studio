# Changelog

Format inspired by [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This file serves the repository and the writing of Steam patch notes; RimWorld does not display it in game.

## [Unreleased]

### Tests

- TESTING.md 15.4 and 15.5 are no longer manual checks: each preference alone is put back by the real reset
  (two scenarios in `14-reset.feature`, plus 33 out-of-game checks), and one review scenario captures both editors
  with a very long accented name for the owner to read. The dependency maps are pinned to LF.

### Fixed

- French interface text no longer gives orders: the category editor and group editor intros, the empty-selection
  hint and two tooltips explain what an action allows or use the infinitive.

## [1.0.4] — 2026-09-23

First release published by the automated pipeline (semantic-release, then Steam), not from the game.

### Fixed

- French hints read correctly in the two editors: a stray comma is gone from the group editor's
  empty-selection line, and the category editor's introduction is rewritten.

### Changed

- The mod's About page credits Codex alongside Claude Code, names Pickle, RimLogging, PickleTools and
  RIMMSQOL as development-only testing tools (not dependencies), and says "Embedded English and French
  translations".
- The distributed Workshop description and its publication source now link every cited Workshop mod
  directly to its own Workshop page. The description now comes from `Mod/README.template.md`.

### Tests

- The Pickle review pass now stages shared FilmTicks and InterfaceScale companions. It records the
  rendered group-member reorder as video and makes 150% review scenarios declare their required
  coordinate repair explicitly.
- The RIMMSQOL shortcut scenario declares both required companion mods and records reveal/hide
  integration with Architect Studio. The redundant tests of RIMMSQOL's own restart persistence were retired.
- No scenario is tagged `@wip` any more; the optional-mods scenario runs in every pass.
- A real restart is played: two game launches under one lock hold, the second a new process that finds the
  configuration of the first.

## [1.0.3] — 2026-09-21

### Fixed

- Deleting a group that belongs to another mod did not survive a restart: the group came back the
  next time the game started. The dissolution was recorded as one empty entry per member in
  `dropdownAssignments` (64 of them for the carpet group), and those entries did not come back
  through `Mod.GetSettings<T>()`, while `hiddenGroupIds`, written by the same call, did. It is now
  carried by `hiddenGroupIds` alone.
- Restore deleted groups brought nothing back. It cleared the hidden list and never re-applied the
  groups, so the buildings kept the arrangement the deletion had left them with.
- The introduction line at the top of the group editor and the category editor was clipped once it
  wrapped, as it does in a narrower window or a longer language: it was drawn centred in a fixed
  24 px box. Its height is now measured.
- Reset everything now restores both preferences and is available when only a preference differs
  from its default.

- The assembly now declares `IgnoresAccessChecksTo("Assembly-CSharp")`. Krafs.Publicizer applies
  it through the SDK's generated AssemblyInfo, which this project switches off, so the attribute
  type was embedded and the waiver never applied. The mod does rely on publicization - removing
  the directive stops the build on eight uses of four non-public members - and a runtime that
  enforces the check would have taken those paths down with no message.

### Added

- An optional MainButtons shortcut to the existing mod settings, hidden by default and available
  to RIMMSQOL and compatible customization tools, with English/French text.
- Local automated tests of the shipped assembly's settings serialization, defaults, legacy
  loading, order changes, name validation and reset behavior against installed RimWorld assemblies.

- A startup probe now performs one deliberate non-public access and logs a named error if the
  runtime refuses it. The failure it guards against has no other symptom: the build stays clean,
  startup is silent, and only the features that touch such a member die, each at first use.

### Tests

- The Pickle step that clicks an Architect Studio button now waits for any modal window to close
  first. A window that absorbs input around itself eats the click wherever it lands, so the
  covering-window check could not see it — that one asks what is drawn over the button, and such
  a window blocks from a distance.
- Scenario 13, the only one asserting that the mod works with none of its optional integrations
  present, now loads the shared test colony and therefore runs. A second mod list,
  `Tests/Pickle/wsl-deps.avec-facultatifs.map`, stages the five optional mods for the pass that
  proves the mod still behaves beside them.

## [1.0.2] — 2026-09-03

### Fixed

- Dragging a group member downwards dropped it one slot too high. `ReorderableWidget` hands the
  callback an insertion index computed before removal, not a final position — vanilla inserts
  first, then removes.

### Changed

- The category and member arrows now use RimWorld's own `ReorderUp` / `ReorderDown` textures
  instead of a rotated horizontal arrow, removing all matrix maths from the mod. An inactive
  arrow no longer places a clickable area at all, so it cannot swallow a click at the end of a list.

## [1.0.1] — 2026-08-30

### Fixed

- The category up/down arrows drifted out of their button as soon as the UI scale went above 100%.

## [1.0.0] — 2026-08-30

First version. RimWorld 1.6.
Steam Workshop item: [3792784018](https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018).

### Dropdown groups

- Create a group, put buildings in it, take them out.
- Order the members of a group, by drag and drop or with up/down arrows.
- Force a category on a whole group: its members are moved there, and ones added later follow.
- Grid or list menu, and a choice of icon source.
- Delete a group. Ones supplied by another mod are dissolved and removed from the list, since their def comes back on every start; a button restores them.
- A warning when a group spreads across several categories, a case where the game silently makes several buttons out of it.

### Categories and subcategories

- Create a category, or a subcategory when Better Architect Menu is present.
- Reorder with up/down arrows, within the sibling set.
- Change the label, colour and icon of any category, including the base game's.
- An icon picker that walks the icons already loaded by the active mods.
- Empty categories greyed out, with a building count.

### Miscellaneous

- An option to show buildings and categories that research still locks, greyed out and unbuildable, so you can organise them before unlocking them.

- An access button in the Architect window, plus an entry in the mod settings.
- A key binding, unassigned by default, for you to set.
- A summary of detected integrations in the settings.
- A global reset, plus targeted per-screen resets.
- French and English.

### Notes

- Nothing is written to defs on disk: everything is saved in the mod settings and reapplied on start.
- Optional integrations, resolved by reflection: Better Architect Menu, Architect Icons, Float Sub-Menus, Searchable Menus. None is required.
