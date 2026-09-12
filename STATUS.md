---
mod:          Architect Studio
packageId:    nelim.architectstudio
repo:         Rimworld-Architect-Studio
visibility:   public
detached:     yes
stage:        done
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
dependencies: declared
showcase:     complete
tested_on:    2026-09-03
workshop:     3792784018
remaining:
  - unverified: dragging a group member, never replayed since either of its two fixes
  - unverified: the up/down arrows since the 1.0.2 rewrite, and at 150% interface scale
  - unverified: a category forced on a whole group, and members added later inheriting it
  - unverified: creating a category, and its entry in the keyboard configuration
  - unverified: the option showing what research still locks
session:      local_ea269783-fdb3-4329-83ed-5e4ad5f22536
updated:      2026-09-12, mod session
---

# Architect Studio — status

## Verification — 2026-09-12

- **Title:** keep `Architect Studio`, with no `Continued` or `Fork` suffix. According to
  `ATTRIBUTION.md`, this is a separate implementation, not a continuation or copied codebase.
  Compatibility with 1.6 is already declared in About.xml.
- **Manual functional tests:** 14 scenarios in `TESTING.md`, with steps, expected outcomes
  and failure signatures. They exist; they have not all passed. The five cases above are
  explicit priorities, not an exhaustive inventory of everything unverified (labels, icons
  and other scenarios also lack recorded results). No game session was run for this audit.
- **Automated checks:** added `Tests/Validate-Mod.ps1`; 710 assertions passed. Covers all five
  shipped XML files, metadata, EN/FR key parity, duplicate/empty translations, placeholder
  parity, literal C# translation references, keybinding definition and French injection,
  matching distribution notices, and unwanted development files in the published directory.
  These are static checks, not automated tests of runtime C# behavior. Drag ordering,
  settings persistence, research locking and reflection integrations still need in-game
  validation; no automated behavioral test suite exists yet.
- **Build:** `dotnet build Source/ArchitectStudio.csproj -c Release --no-restore` passed,
  zero warnings and errors. This does not establish functional correctness in RimWorld.
- **Description:** GitHub URL was already in `<url>`; now also included in `<description>`:
  https://github.com/vbardales/Rimworld-Architect-Studio. Local files updated; Workshop not republished.
- **Licence:** MIT, copyright (c) 2026 nelim, in `LICENSE` and `Mod/LICENSE`.
  The separate MIT notice for studied fernyrepos mods (copyright 2025 fernyrepos) is preserved
  in both `LICENSE-fernyrepos.txt` copies. Classification is `original`: an original mod idea,
  not an update or continuation of another mod. Its software licence remains MIT.

## Previous status and context

Status sheet, read by a pass over every mod rather than by asking each session in turn.
It lives at the root, never in `Mod/`, so Steam never receives it.

The twelve fields derived from disk on 2026-09-12 were checked one by one and hold. The four
the sweep cannot fill are settled here.

- **`stage`** — `done`, confirmed. Three versions published, 1.0.0 to 1.0.2, showcase finished,
  Workshop item 3792784018 online.
- **`dependencies`** — `declared`, but only after checking, because the About lists five
  non-vanilla `loadAfter` entries that are not in `modDependencies`: Architect Icons, Better
  Architect Menu, its dropdowns, Float Sub-Menus and Searchable Menus. None of them is a
  dependency. Every one is resolved by reflection, no third-party assembly is referenced at
  build time, and the mod is meant to run with all five absent. `loadAfter` is there because we
  hook onto them when they happen to be present. An undeclared dependency is not cosmetic - on
  2026-09-11 Reequilibrage animaux took 47 vanilla animals down with it, Muffalo included,
  because the class it injects belongs to a mod that was not declared and not loaded - which is
  why this one is written out rather than waved through.
- **`tested_on`** — 2026-09-03, the date of the last fix that required seeing the mod run: the
  up/down arrows left their button as soon as the interface scale went above 100%, which shows
  on screen and nowhere else. The line the sweep puts there by default, "never seen running in
  game", was wrong for this mod: `TESTING.md` names four things already seen working in a real
  save, category and subcategory colours, deleting a category created here, and changing its
  parent.
- **`remaining`** — the five scenarios of `TESTING.md` that have never run. No known defect left
  unfixed, no feature missing from the first pass: what remains is unverified, not broken.
  Dragging comes first because it has carried two separate bugs and neither fix has been
  replayed since.

`TESTING.md` stays the source: it says for each scenario what it proves and what its failure
looks like. This sheet keeps only the balance.

`licence` vocabulary: `original` an original mod idea, not an update or continuation of another
mod; studying other mods or integrating with them does not exclude this classification.
For updates or continuations: `open` an explicit licence, `silent` no licence and a dead source,
`alive` no licence but a living source, `forbidden` a written refusal.
