# Changelog

Format inspired by [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
This file serves the repository and the writing of Steam patch notes; RimWorld does not display it in game.

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
