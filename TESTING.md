# Testing Architect Studio in game

Three versions published, v1.0.0 to v1.0.2. Parts of mod played. Other parts only compiled. File separates the two: spend test time on what never ran.

Already seen working, by hand, in real save: category colours, subcategory colours, deleting category created here, editing parent of created category.

Never run once: drag of group member, up/down arrows since v1.0.2 rewrote them, category forced on whole group, creating category, research-locked option. Two bugs already fixed on drag path alone, neither fix ever replayed, so drag comes first below.

Each scenario says what it proves. Test whose failure you cannot interpret not worth running.

## Before anything

1. **Harmony must be active, this mod after it.** Only hard dependency, declared in `About.xml`; mod list says so if missing.
2. **The packageId is `nelim.architectstudio`.** Unchanged since first release, so saved mod list keeps it.
3. **Keep `Player.log`.** Everything this mod complains about is prefixed `[Architect Studio]`. File overwritten at next launch, moved to `Player-prev.log`, so copy out before relaunching.
4. **Note which optional mods active**, because half the scenarios change
   shape without them: Better Architect Menu, Architect Icons, Float Sub-Menus, Searchable Menus.
   Mod settings list first three under *Detected integrations*; fastest way to
   learn a feature is missing because a mod is, not because code is wrong.
5. **No default keyboard shortcut.** F1 to F11 taken by main tabs, F9 in
   particular collided with history tab. Assign one in Options if wanted; normal way in
   is buttons in Architect window.

Both editors open from bottom row of Architect window, **Groups…** and **Categories…**, or
from mod settings. Settings route always works; buttons themselves under test.

## 1: The mod loads and its patches apply

**Proves** Harmony patches, deferred ones included, and startup pass rebuilding created defs. Every other scenario depends on this.

Start game, reach main menu, quit. Search `Player.log` for `[Architect Studio]`.
**Silence is the pass.**

Failure looks like:

- Any unhandled exception naming `ArchitectStudio`. Stack trace names step: `StartupInit`,
  `CustomCategoryRuntime.EnsureDefs`, `CategoryAppearance.ApplyLabels`, `CategoryRuntime.Apply` or
  `DropdownRuntime.Apply`.
- *"Tried to get a resource … from a different thread"*: the v1.0.0 crash. Two patches applied
  late, from `StartupInit`, precisely to avoid it: `ArchitectIconsCompat` and `CategoryColorPainter`.
  If it returns, one drifted back into mod constructor.
- *"The runtime refused access to a non-public member of the game …"*: build lost publicizer waiver. Mod reads four non-public game members; attribute making that legal applied through generated file this project switches off, so rebuild
  without `Source/AccessChecks.cs` produces clean, silent, broken assembly. Probe printing this line only thing that says so; without it failure shows up later, one
  feature at a time, as exception at first use. **If you ever see this line, copy it out**:
  no one yet seen game's own runtime refuse such access, and that log would be missing evidence for every mod of this collection built same way.
- *"Dropdown group not found: '…'"*: saved assignment names group no longer present. Not failure itself: buildings fall back to default, assignment kept in case mod returns. Worrying only if mod list unchanged.
- *"Better Architect Menu is loaded, but none of its cache …"*: BAM renamed or removed methods
  this mod calls by reflection. Everything still applies to defs, but nothing refreshes until
  restart. Real break, most likely to arrive with BAM update.

## 2: The two buttons appear in the Architect window

**Proves** window-height patch and drawing patch agree. Height patch works but drawing not: empty band; reverse overlaps buttons with tabs.

Open Architect menu. **Row of two buttons, `Groups…` and `Categories…`, sits at bottom of
window**, nothing else clipped or overlapping.

Turn *Button in the Architect menu* off in settings, **reopen Architect menu** (tip
says so, window height only recomputed on open) and row gone, no gap left.

## 3: Dragging a group member, downwards

**Proves** reordering path, carried two separate bugs, never played since
either fixed. Single most likely thing still wrong.

Open **Groups…**, pick group with at least four members, drag **first** member down to
**last**.

- **Must land last.** One slot short of end is signature of older bug: callback
  hands back insertion index computed *before* removal; vanilla inserts then removes while
  this code removes then inserts. Only downward drags affected, hence hard to
  see.
- **Drag must start at all.** If nothing moves and row never lifts, reordering group id
  went back into local variable: `NewGroup` only returns real value on Repaint, so local
  records -1, drag never begins.
- Drag upwards too, and drag last member to first. Both directions must be right, not just
  one tried.

Then **open Architect menu, click group's button**: menu order must match editor,
top to bottom.

## 4: The up/down arrows, including at 150% interface scale

**Proves** v1.0.2 rewrite. In v1.0.0 arrows drawn rotated around pivot not
multiplied by `Prefs.UIScale`, so flew out of button at any scale other than 100%. Now
two ready-made textures, rotate nothing, but never seen on screen.

In members column, use arrows on middle row: **arrow stays inside its button**, member
moves one place.

Then set interface scale to **150%** in Options, look again, in both editors. Arrows must
sit in buttons at every scale.

First and last rows carry disabled arrow: drawn faded, **must not be clickable**: no
click sound, no highlight on hover.

## 5: Reordering categories

**Proves** writing `DesignationCategoryDef.order` moves both top-level category and subcategory, and whole sibling set renumbered, not just two rows swapped.

Open **Categories…**, move category up, close editor, open Architect menu: tabs in
new order.

Same with **subcategory**: needs Better Architect Menu, owns only nesting
mechanism. Without it tree flat, nothing to test.

Move category several times in a row without closing window. Each click redraws from fresh
tree, so nothing should jump two places or bounce back.

*Reset order* puts everything back.

## 6: Creating a group and filling it

**Proves** whole dropdown chain: def created in memory, buildings reassigned onto it, category rebuilt so menu shows it without restart.

*New group…*, name it, add three or four buildings **from same category** with
right-hand column. **Architect menu collapses them into one button immediately**, without
closing and reopening game.

Remove one member: becomes own button again at once.

Search field and category filter both ways to find building. On Steam Deck filter is
the one that matters, since searching means opening virtual keyboard.

## 7: A category forced on a whole group

**Proves** the one feature with no equivalent anywhere: natively group has no category of its own,
follows its members, splits into as many buttons as they occupy categories.

Build group from buildings **taken from two different categories**. Editor warns group
is split, Architect menu indeed shows two separate buttons.

Now set group's *Category*. **Every member moves there at once**, two buttons become one.

Then add further building from third category to group: **must follow on its own**,
without touching setting again.

Set category back to *— none*: members go back where they came from.

## 8: Creating a category, and its keyboard shortcut

**Proves** hand-built `KeyBindingCategoryDef`. Game only generates one when defs created, at startup, so category made mid-game would otherwise have none, and code reading
it does not expect null.

*New category…* in Categories editor. Give name, then parent if Better Architect Menu present. Without it editor says outright category will be created at top level, rather
than ask question whose answer it would ignore.

- New tab appears in Architect menu.
- **Open Options → Keyboard configuration.** Dialog must draw with new category defined, log must stay silent. Category **not listed** there: dialog draws category only if
  it has bindings, this one has none, so no screenshot can show entry. Scenario asserts generated binding category on def instead, and its translated label and description.
- Move building into it with Better Architect Menu's own Edit Mode, pencil left of
  sort buttons. Architect Studio deliberately does not duplicate that.

Deleting category afterwards is already-confirmed half: buildings return to original category before def removed.

## 8b: Moving a category that is not ours under another

**Proves** request of Ali50 (Workshop, 2026-10): tab added by game or another mod can be put
under another tab as subcategory. Needs Better Architect Menu, owns only nesting mechanism: its
`NestedCategoryExtension` grafted on foreign def, nothing written to def's file.

Open appearance window of category (click its name in Categories editor).

- *Parent category* row there for every category. Category already having subcategories shows note
  instead of button: cannot become subcategory itself (two levels only).
- Choose top-level tab as parent: tab leaves bar, appears under parent. Choose *Top level*:
  comes back to bar. Category cannot be own parent.
- Restart game: choice still applied. *Reset everything* puts tab back where game had it, at once.
- Delete category of ours that adopted tab: adopted tab goes back where it was.
- Tab research still locks follows *Show what research still locks* wherever it sits (scenario 11).
- Without Better Architect Menu row says so, nothing can be moved.

Not played by hand: greyed sub-tab drawn in parent's dropdown for locked category.

## 9: Appearance: label, colour, icon

**Proves** three different mechanisms sharing one window. Colours confirmed at both
levels; labels and icons not.

Click category's name in Categories editor.

- **Label**: rewritten on def at startup. Change it, Architect tab follows.
- **Colour**: laid down as button drawn. Confirmed working on top-level categories and on
  Better Architect Menu's subcategory rows.
- **Icon**: needs Architect Icons. Choice must show **immediately**, not at next startup:
  Architect Icons caches category's icon permanently, this mod evicts that entry by hand. Change that only appears after restart means eviction missed.

Without Architect Icons icon section says unavailable, correct behaviour, not bug to chase.

## 10: Deleting a group that belongs to another mod

**Proves** only available approach: def cannot be erased, its mod recreates it at every
startup, so emptied of members instead and remembered as hidden.

Delete vanilla or modded group from list. Disappears from Architect menu and from
editor, footer counts it under deleted groups.

**Restart game.** Must still be gone: whole point, and the half single
session cannot check. Restart pass automates it (`20-restart-write`, then `21-restart-read`: two real
game launches under one lock hold); do by hand only to see with own eyes.

*Restore deleted groups* brings it back with members.

## 11: Showing what research still locks

**Proves** the one option where mistake is serious, not cosmetic. `Designator_Build.Visible`
is only lock: nothing revalidates research when blueprint placed, so making designator
visible without also disabling it would let you build unresearched buildings.

Turn *Show what research still locks* on, in save with unfinished research.

- Locked buildings appear, **greyed out**.
- **Click one.** Must refuse, with *Research not completed*. If blueprint can be placed, stop
  and say so: failure this scenario exists for.
- Finish research: same building becomes buildable, no restart, no greyed
  state sticking.
- Turn option off: they disappear again.

Building hidden by faction's tech level stays hidden either way. Different lock, this option does not touch it.

## 12: Everything survives a restart

**Proves** settings round-trip and startup replay, where created defs rebuilt
from scratch.

With group created, category created, order changed, colour set: quit game entirely,
relaunch, look again. Everything back, `Player.log` silent. Restart pass plays this in
two real launches (group, category, order, colour, deleted group); manual run optional.

## 13: Without the optional mods

**Proves** reflection bridges soft, as Workshop page claims.

Played by `13-optional-mods.feature` in **every** pass. Asserts what holds in all: each
bridge (Better Architect Menu, Architect Icons, Float Sub-Menus) reports itself exactly when its mod
loaded, editors open, group can be created and filled, log stays silent. Which world
it ran in attached to report.

In **minimal pass** (Core, Harmony and Architect Studio, nothing else) that is scenario as
first written: mod loads with none of optional mods, both editors open, groups work, log
silent, subcategories, icon selection and nested pick menus simply absent. In pass with optional
mods same scenario proves every bridge resolved. Replaces version tagged `@wip` that
asserted three absences: Pickle can skip scenario on presence of mod but not on absence, so
that version could only be hidden, not played.
## 14: Reset everything

**Proves** reset unwinds in right order: created categories go through normal
deletion, so buildings handed back before def disappears.

With several things configured, use *Reset everything* in mod settings. Architect menu
returns to how it looked with mod absent, nothing logged.

## 22: The Unsorted tab (1.1.0)

**Proves** opt-in catch-all gathers buildings no other tab provides, nothing else.

Orphan is building with blueprint (a category) whose default designator off and which no other
tab offers. With preference off (default) no tab. Turned on, `22-unsorted-tab.feature`
switches one vanilla building's default designator off, checks no other tab offers it, then checks Unsorted tab does and category editor's count equals what tab offers. Building another tab
shows not offered there. Turning option off, or *Reset everything*, removes tab.

`23-unsorted-language.feature` (`@review`, needs `hoversteps`) adds two screenshots person reads in English and
French: tooltip of setting on settings page, and Architect menu with tab on.

## 24: Hiding a whole Architect tab (1.2.0)

**Proves** tab can be hidden from category editor without deleting anything.

`24-hide-tab.feature` hides tab `Furniture`, checks game no longer shows it (its `Visible` answer),
editor lists it as hidden and `Bed` still building player can build; showing again
brings it back. With Unsorted tab on, buildings of hidden tab offered there and editor's count
matches. Unsorted tab itself cannot be hidden this way (own setting). *Reset everything* shows every
hidden tab again. Setting persists by defName (`hiddenCategories`, checked in `Tests/BehaviorTests.cs`).

Not covered in game yet: hidden parent tab with Better Architect Menu children, and order of `Visible`
patches against research-locked option.

## Translation gate and in-game language checks

Repeat source/text inventory and resource validation after UI, Def or translation changes,
before entering `writeTests`. Record evidence in `STATUS.md` under `Translation audit`, reset
affected localization/language fields to `unchecked` until revalidated.

Run following through Pickle passes in English and French (`-Language English` and
`-Language French`), preserving attached media for review:

- Open mod settings, both Architect editors, naming dialogs, category appearance and parent
  menus. Exercise confirmations, empty lists, search filters, counters, split-group warnings,
  integration availability, research-locked tooltips. Check for raw keys, English fallback
  in French, wrong parameters, clipping at 100% and 150% interface scale.
- Create category with accented name, assert generated binding category's label
  and description in selected language. Category with no key bindings not drawn in
  keyboard dialog, so screenshot of dialog cannot verify this text. Restart coverage
  for game-generated bindings remains separate.
- Check mod's dropdown shortcut label, OK and Close buttons. User-entered names and
  integration product names must remain unchanged; existing building/category labels come
  from owning mods' translations.

Scenarios set up each route, attach screenshots/video; remaining human action is reading
that media for layout and language correctness. Never switch language inside running game.

## Automated in game: Pickle

All scenarios written in Gherkin under `Tests/Pickle/`, played inside RimWorld by Pickle test
mod. `Tests/Pickle/README.md` defines named minimal, optional-integration, review and RIMMSQOL passes.
No separate manual action checklist: `@review` scenarios reduce human validation to attached
screenshots or video. Execution and media review remain pending where `STATUS.md` says so.

## Local behavioral checks

Current local behavioral suite (added 2026-09-13): after building shipped DLL, run
`pwsh -NoProfile -File Tests/Run-Behavior.ps1`. Requires .NET 8 and installed RimWorld 1.6
runtime assemblies; `-Managed PATH` overrides location. Tests real Scribe XML round-trips
and compiled mod logic. See `Tests/RESULTS.md` for setup boundaries and actual results.
CI static checks remain independent; CI lacks game's runtime assemblies.

## 15: Optional MainButtons shortcut and preferences reset

Preconditions: Core, Harmony and Architect Studio; disposable new game and copy of
existing save. Keep both user saves and current global settings backed up before changing.
Repeat in English and French at 100% and 150% UI scale.

1. With no customization override, verify no visible or greyed-out Architect Studio
   Main Button. Open Options -> Mod settings -> Architect Studio, change each toggle, reopen.
   Same values remain, effects match scenarios 2 and 11.
2. Enable RIMMSQOL, find Architect Studio in its Main Buttons editor, set Visible to true.
   Button must appear and open same native settings page, including both editor links.
   Change toggle through it; reopen through Mod settings, verify value shared.
3. Restart, reload existing save, verify Architect Studio's own settings persist. Disable
   RIMMSQOL, verify primary settings access still works. No error in Player.log.
4. Starting with no editor customizations, change only Architect button preference; reset
   must become available and restore it to on. Repeat with only research visibility changed;
   reset restores it to off. Repeat scenario 14 with customizations and both toggles changed.
   Reopen Architect menu to refresh height after reset, then restart to check persistence.
5. Try blank, whitespace-only, accented and long group names. Empty names not accepted;
   accents survive saving. Names have no imposed length cap: verify long labels do not make
   editors unusable. Check reset confirmation and shortcut tooltip in both languages.

15.4 and 15.5 not manual checks:
- **15.4** covered out of game by `Tests/BehaviorTests.cs` ("changing only the Architect toggle enables reset",
  "changing only research visibility enables reset", "reset restores both toggles") and in game by two
  scenarios of `14-reset.feature`.
- **15.5**: rules (blank and whitespace-only refused, boundaries trimmed, accents kept, nothing truncated)
  covered by `BehaviorTests.cs`. Only eyes can judge whether very long accented name breaks
  editors: scenario "a very long accented name in both editors" in `16-language-review.feature` attaches one
  capture of each editor, in language of pass.
- New game versus existing save: not applicable. Mod writes nothing into save (groups, categories and
  preferences live in mod settings, global to all saves, mod can be added to or removed from
  game in progress); every scenario loads same fixture save, `test-colony`, restart chain proves
  settings survive new process.

Feature 19 uses shared `PickleTools/RimmsqolSteps` companion to drive RIMMSQOL's settings instance and
capture integration. Results per revision one line each in `docs/runs/`.

## Automated validation commands

From repository root, run:

```powershell
pwsh -NoProfile -File Tests/Validate-Mod.ps1
dotnet build Source/ArchitectStudio.csproj -c Release --no-restore
```

On fresh checkout, omit `--no-restore` for first build to restore NuGet dependencies.
Expected: 989 static checks pass, Release build zero warnings and errors (counts change with
validator; last run in `docs/runs/`). Script checks every shipped XML, metadata, translation keys and placeholders, C# literal
translation references, keybinding and its injected label, distribution notices/content.
Throws on first failure, exits unsuccessfully. Static validator does not execute
C# runtime or prove in-game scenarios above. Separate local behavioral suite (37 checks)
executes compiled mod logic and Scribe; neither suite proves in-game scenarios.