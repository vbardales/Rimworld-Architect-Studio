---
mod:          Architect Studio
packageId:    nelim.architectstudio
repo:         Rimworld-Architect-Studio
visibility:   public
detached:     yes
stage:        done
licence:      open
licence_at:   LICENSE-fernyrepos.txt, MIT
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

Status sheet, read by a pass over every mod rather than by asking each session in turn.
It lives at the root, never in `Mod/`, so Steam never receives it.

The field names above are deliberately left in French: they are read by that cross-mod pass, not
by a reader, so renaming them here would break it for one mod out of many. Everything written
for a reader is in English, as in the rest of this repository.

The eleven fields derived from disk on 2026-09-12 were checked one by one and hold. The three
the sweep could not fill are settled here.

- **`etape`** — `done`, confirmed. Three versions published, 1.0.0 to 1.0.2, showcase finished,
- **`dependencies`** — `declared` when every mod this one needs is named in the About's
  `modDependencies`, `to check` when a non-vanilla `loadAfter` suggests a dependency that is not
  declared, `none` when the mod needs nothing. An undeclared dependency is not cosmetic: on
  2026-09-11 Reequilibrage animaux took 47 vanilla animals down with it, Muffalo included, because
  the class it injects belongs to a mod that was not declared and not loaded.

  Workshop item 3792784018 online.
- **`teste_le`** — 2026-09-03, the date of the last fix that required seeing the mod run: the
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

`licence` vocabulary: `open` an explicit licence, `silent` no licence and a dead source,
`alive` no licence but a living source, `forbidden` a written refusal, `original` owing nothing
to anyone — not a name, not an idea traceable to one mod, not a value derived from its assets.
