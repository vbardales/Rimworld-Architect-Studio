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

## Preview recomposition — 2026-09-12

- Final delivered image: `Mod/About/Preview.png`, 896 x 504, 612,584 bytes (under 900 KB).
  Name and summary preserved verbatim from the previous Preview. Version badge is 1.6,
  selected from the highest stable version actually declared in `Mod/About/About.xml`.
  No status tag applies to this public original mod. Nothing published.
- Illustration replaced: the previous source already contained a large title and interface
  labels, so it could not serve as a text-free background. It remains intact under its distinct
  archival name `Art/Preview-source.png`. New text-free source: `Art/Preview.png`, generated
  with the built-in imagegen tool; exact prompt: `Art/PROMPT_Preview.md`. Visually checked:
  overhead colony workshop, grouped miniature buildings on the right, calm tiled floor on
  the left, no text or faces. Original full-resolution generated source retained here.
- Composition and parameters: `Art/preview.html`; reproducible renderer: `Art/render-preview.cjs`
  (Node.js with `playwright` and `sharp` available through NODE_PATH, installed Chrome or
  CHROME_PATH override). Run `node Art/render-preview.cjs` from the repository root.
  Colors load exclusively from `Art/preview-palette.json`; no duplicate palette in the HTML.
- Palette rationale: the large slate-blue tiled floor supplies the veil and the dominant
  blue family, lightened for the secondary ink. The characteristic amber lamp pool on the
  sorting table supplies the vivid accent, with saturation increased for the rule and badge.
  Primary ink is shared exactly by title and summary; the dark veil uses the guide's radial
  gradient and text shadow. Secondary ink is saved but unused because there is no tag.
  Rechecked against the revised guide: the warm orange-amber accent is a distinct hue family
  from the cool blue secondary ink and dominant slate floor. Its stronger saturation makes
  the rule and badge stand out at both sizes; no palette adjustment is needed.
  Title hierarchy: both `Architect` and `Studio` are identity-bearing words, retained at 100%
  (46 px, weight 600, primary ink). There is no prefix, suffix or linking word requiring 65%.
- Font verified through Chromium's actual platform-font report after `document.fonts.ready`:
  Segoe UI Semibold for the 46 px/600 title, Segoe UI regular for the 21 px/400 summary,
  Segoe UI Bold for the 26 px/700 version. No fallback. Text origin is (50,54); rule is 58 x 3;
  badge uses the guide's 80 x 80 triangle and center (869,27), rotated 45 degrees.
- QA: `Art/preview-qa/report.json`, background without text in
  `Art/preview-qa/background.png`, and `Art/preview-qa/thumbnail.png` at 268 px wide.
  Contrast was checked against the rendered background at every pixel of the title and
  summary bounding rectangles, including their corners: minima 10.19:1 and 7.44:1;
  badge digits against the rendered opaque badge: 9.58:1. All exceed 4.5:1; tag not applicable.
  Visual checks at both sizes: no overlap or clipped text, identifiable title and version,
  visible rule. The summary is intended to be read at full size, as specified by the guide.

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
