# Testing Architect Studio in game

Three versions have been published, v1.0.0 to v1.0.2, and parts of the mod have been played. Other
parts have only ever been compiled. This file separates the two, because the point of testing is to
spend the time on what has never run.

Already seen working, by hand, in a real save: category colours, subcategory colours, deleting a
category that was created here, and editing the parent of a created category.

Never run once: the drag of a group member, the up/down arrows since v1.0.2 rewrote them, a category
forced on a whole group, creating a category, and the research-locked option. Two bugs have already
been fixed on the drag path alone without either fix ever being replayed, which is why it comes
first below.

Each scenario says what it proves. A test whose failure you cannot interpret is not worth running.

## Before anything

1. **Harmony must be active, and this mod after it.** It is the only hard dependency, declared in
   `About.xml`; the mod list says so if it is missing.
2. **The packageId is `nelim.architectstudio`.** Unchanged since the first release, so a saved mod
   list keeps it.
3. **Keep `Player.log`.** Everything this mod complains about is prefixed `[Architect Studio]`. The
   file is overwritten at the next launch and moved to `Player-prev.log`, so copy it out before
   relaunching.
4. **Note which optional mods are active**, because half the scenarios below change
   shape without them: Better Architect Menu, Architect Icons, Float Sub-Menus, Searchable Menus.
   The mod settings list the first three under *Detected integrations*, and that list is the fastest way to
   find out that a feature is missing because a mod is, not because the code is wrong.
5. **There is no default keyboard shortcut.** F1 to F11 are taken by the main tabs, and F9 in
   particular collided with the history tab. Assign one in Options if you want it; the normal way in
   is the buttons in the Architect window.

Both editors open from the bottom row of the Architect window, **Groups…** and **Categories…**, or
from the mod settings. The settings route always works; the buttons are themselves under test.

## 1 — The mod loads and its patches apply

**Proves** the Harmony patches, the deferred ones included, and the startup pass that rebuilds the
created defs. Every other scenario depends on this one.

Start the game, reach the main menu, quit. Search `Player.log` for `[Architect Studio]`.
**Silence is the pass.**

What a failure looks like:

- Any unhandled exception naming `ArchitectStudio`. The stack trace names the step: `StartupInit`,
  `CustomCategoryRuntime.EnsureDefs`, `CategoryAppearance.ApplyLabels`, `CategoryRuntime.Apply` or
  `DropdownRuntime.Apply`.
- *"Tried to get a resource … from a different thread"* — the v1.0.0 crash. Two patches are applied
  late, from `StartupInit`, precisely to avoid it: `ArchitectIconsCompat` and `CategoryColorPainter`.
  If this comes back, one of them has drifted back into the mod constructor.
- *"The runtime refused access to a non-public member of the game …"* — the build lost the
  publicizer waiver. The mod reads four non-public members of the game, and the attribute that
  makes that legal is applied through a generated file this project switches off, so a rebuild
  without `Source/AccessChecks.cs` produces a clean, silent, broken assembly. The probe that
  prints this line is the only thing that says so; without it the failure shows up later, one
  feature at a time, as an exception at first use. **If you ever see this line, copy it out** —
  no one has yet seen the game's own runtime refuse such an access, and that log would be the
  missing evidence for every mod of this collection built the same way.
- *"Dropdown group not found: '…'"* — a saved assignment names a group no longer present. Not a
  failure in itself: the buildings fall back to their default and the assignment is kept in case the
  mod returns. Only worrying if you did not change your mod list.
- *"Better Architect Menu is loaded, but none of its cache …"* — BAM renamed or removed the methods
  this mod calls by reflection. Everything still applies to the defs, but nothing will refresh until
  a restart. This one is a real break, and it is the most likely to arrive with a BAM update.

## 2 — The two buttons appear in the Architect window

**Proves** the window-height patch and the drawing patch agree. If the height patch works and the
drawing one does not, you get an empty band; the reverse overlaps the buttons with the tabs.

Open the Architect menu. **A row of two buttons, `Groups…` and `Categories…`, sits at the bottom of
the window**, and nothing else is clipped or overlapping.

Turn *Button in the Architect menu* off in the settings, **reopen the Architect menu** — the tip
says so, the window height is only recomputed when it opens — and the row is gone with no gap left
behind.

## 3 — Dragging a group member, downwards

**Proves** the reordering path, which has carried two separate bugs and has never been played since
either was fixed. This is the single most likely thing to still be wrong.

Open **Groups…**, pick a group with at least four members, and drag the **first** member down to
**last**.

- **It must land last.** One slot short of the end is the signature of the older bug: the callback
  hands back an insertion index computed *before* removal, and vanilla inserts then removes while
  this code removes then inserts. Only downward drags were affected, which is what made it hard to
  see.
- **The drag must start at all.** If nothing moves and the row never lifts, the reordering group id
  has gone back into a local variable: `NewGroup` only returns a real value on Repaint, so a local
  records -1 and the drag never begins.
- Drag upwards too, and drag the last member to first. Both directions have to be right, not just
  the one you tried.

Then **open the Architect menu and click the group's button**: the menu order must match the editor,
top to bottom.

## 4 — The up/down arrows, including at 150% interface scale

**Proves** the v1.0.2 rewrite. In v1.0.0 these arrows were drawn rotated around a pivot that was not
multiplied by `Prefs.UIScale`, so they flew out of their button at any scale other than 100%. They
are now two ready-made textures and rotate nothing, but that has never been seen on screen.

In the members column, use the arrows on a middle row: **the arrow stays inside its button** and the
member moves one place.

Then set the interface scale to **150%** in Options and look again, in both editors. The arrows must
sit in their buttons at every scale.

The first and last rows carry a disabled arrow: it is drawn faded and **must not be clickable** — no
click sound, no highlight on hover.

## 5 — Reordering categories

**Proves** that writing `DesignationCategoryDef.order` moves both a top-level category and a
subcategory, and that the whole sibling set is renumbered rather than just the two rows swapped.

Open **Categories…**, move a category up, close the editor, open the Architect menu: the tabs are in
the new order.

Do the same with a **subcategory** — this needs Better Architect Menu, which owns the only nesting
mechanism there is. Without it the tree is flat and there is nothing to test here.

Move a category several times in a row without closing the window. Each click redraws from a fresh
tree, so nothing should jump two places or bounce back.

*Reset order* puts everything back.

## 6 — Creating a group and filling it

**Proves** the whole dropdown chain: a def created in memory, buildings reassigned onto it, and the
category rebuilt so the menu shows it without a restart.

*New group…*, name it, then add three or four buildings **from the same category** with the
right-hand column. **The Architect menu collapses them into one button immediately**, without
closing and reopening the game.

Remove one member: it becomes its own button again at once.

The search field and the category filter are both ways to find a building. On a Steam Deck the
filter is the one that matters, since searching means opening the virtual keyboard.

## 7 — A category forced on a whole group

**Proves** the one feature with no equivalent anywhere: natively a group has no category of its own,
it follows its members and splits into as many buttons as they occupy categories.

Build a group from buildings **taken from two different categories**. The editor warns that the
group is split, and the Architect menu indeed shows two separate buttons.

Now set the group's *Category*. **Every member moves there at once** and the two buttons become one.

Then add a further building from a third category to the group: **it must follow on its own**,
without touching the setting again.

Set the category back to *— none*: the members go back where they came from.

## 8 — Creating a category, and its keyboard shortcut

**Proves** the hand-built `KeyBindingCategoryDef`. The game only generates one when defs are
created, at startup, so a category made mid-game would otherwise have none — and the code that reads
it does not expect null.

*New category…* in the Categories editor. Give it a name, then a parent if Better Architect Menu is
present. Without it the editor says outright that the category will be created at top level, rather
than ask a question whose answer it would ignore.

- The new tab appears in the Architect menu.
- **Open Options → Keyboard configuration and scroll to the Architect section.** The new category
  must be listed there with the others. A missing entry, or an error in the log at that moment, is
  this scenario failing.
- Move a building into it with Better Architect Menu's own Edit Mode, the pencil left of the sort
  buttons. Architect Studio deliberately does not duplicate that.

Deleting the category afterwards is the already-confirmed half: its buildings return to their
original category before the def is removed.

## 9 — Appearance: label, colour, icon

**Proves** three different mechanisms that happen to share one window. Colours are confirmed at both
levels; labels and icons are not.

Click a category's name in the Categories editor.

- **Label** — rewritten on the def at startup. Change it, and the Architect tab follows.
- **Colour** — laid down as the button is drawn. Confirmed working on top-level categories and on
  Better Architect Menu's subcategory rows.
- **Icon** — needs Architect Icons. The choice must show **immediately**, not at the next startup:
  Architect Icons caches a category's icon permanently, and this mod evicts that entry by hand. A
  change that only appears after a restart means the eviction missed.

Without Architect Icons the icon section says it is unavailable, which is the correct behaviour and
not a bug to chase.

## 10 — Deleting a group that belongs to another mod

**Proves** the only available approach: the def cannot be erased, its mod recreates it at every
startup, so it is emptied of its members instead and remembered as hidden.

Delete a vanilla or modded group from the list. It disappears from the Architect menu and from the
editor, and the footer counts it under deleted groups.

**Restart the game.** It must still be gone — that is the whole point, and it is the half a single
session cannot check.

*Restore deleted groups* brings it back with its members.

## 11 — Showing what research still locks

**Proves** the one option where a mistake is serious rather than cosmetic. `Designator_Build.Visible`
is the only lock: nothing revalidates research when a blueprint is placed, so making a designator
visible without also disabling it would let you build unresearched buildings.

Turn *Show what research still locks* on, in a save with unfinished research.

- Locked buildings appear, **greyed out**.
- **Click one.** It must refuse, with *Research not completed*. If a blueprint can be placed, stop
  and say so: that is the failure this scenario exists for.
- Finish the research: the same building becomes buildable, without a restart and without the greyed
  state sticking.
- Turn the option off: they disappear again.

A building hidden by your faction's tech level stays hidden either way. That is a different lock and
this option does not touch it.

## 12 — Everything survives a restart

**Proves** the settings round-trip and the startup replay, which is where created defs are rebuilt
from scratch.

With a group created, a category created, an order changed and a colour set: quit the game entirely,
relaunch, and look again. Everything is back, and `Player.log` is silent.

## 13 — Without the optional mods

**Proves** that the reflection bridges are soft, as the Workshop page claims.

Turn off all optional mods, including Better Architect Menu, Architect Icons, Float Sub-Menus,
Searchable Menus and dropdown packs, keeping only Core, Harmony and Architect Studio.
The mod must load, both editors must open, groups must still work, and the log must stay
silent. Subcategories, icon selection and nested pick menus are simply absent, and the interface says
so where it matters.

## 14 — Reset everything

**Proves** that the reset unwinds in the right order: created categories go through their normal
deletion, so their buildings are handed back before the def disappears.

With several things configured, use *Reset everything* in the mod settings. The Architect menu
returns to how it looked with the mod absent, and nothing is logged.

## Translation gate and in-game language checks

Repeat the source/text inventory and resource validation after UI, Def or translation changes,
before entering `preTest`. Record evidence in `STATUS.md` under `Translation audit` and reset
affected localization/language fields to `unchecked` until revalidated.

Run the following in English and French, restarting RimWorld after changing language:

- Open mod settings, both Architect editors, naming dialogs, category appearance and parent
  menus. Exercise confirmations, empty lists, search filters, counters, split-group warnings,
  integration availability and research-locked tooltips. Check for raw keys, English fallback
  in French, wrong parameters and clipping at 100% and 150% interface scale.
- Create a category with an accented name, then open keyboard configuration. Its generated
  tab label and description must use the selected language and preserve the entered name.
  Restart and check the same category again, covering bindings generated by the game itself.
- Check the mod's dropdown shortcut label, OK and Close buttons. User-entered names and
  integration product names must remain unchanged; existing building/category labels come
  from their owning mods' translations.

These checks have not yet been run for the 2026-09-13 translation changes.

## Automated validation commands

From the repository root, run:

```powershell
pwsh -NoProfile -File Tests/Validate-Mod.ps1
dotnet build Source/ArchitectStudio.csproj -c Release --no-restore
```

On a fresh checkout, omit `--no-restore` for the first build to restore NuGet dependencies.
On 2026-09-12: 710 static assertions passed; Release build passed with zero warnings/errors.
The script checks every shipped XML, metadata, translation keys and placeholders, C# literal
translation references, the keybinding and its injected label, and distribution notices/content.
It throws on the first failure and exits unsuccessfully. It does not load RimWorld, execute the
C# runtime or prove the manual scenarios above. Automated behavioral coverage remains absent.
