---
settings_audit: complete
localization: complete
translation_en: complete
translation_fr: complete
mod:          Architect Studio
packageId:    nelim.architectstudio
repo:         Rimworld-Architect-Studio
visibility:   public
detached:     yes
stage:        done
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
dependencies: declared
showcase:     complete
tested_on:    2026-09-20
workshop:     3792784018
remaining:
  - unverified: the English and French walkthrough side by side, which needs RimWorld restarted between the two languages; the 2026-09-20 run covered English only (TESTING.md translation gate)
  - unverified: the RIMMSQOL half of scenario 15, revealing and hiding the MainButtons shortcut, which needs RIMMSQOL installed and a real restart
  - verified: the 150% interface-scale screenshots of both editors, read by the mod's owner on 2026-09-20 and found clean — no clipping, no raw keys. English only, so the French half stays under the translation-gate line above
  - external: the Pickle defect behind that scenario is fixed on `fix/tag-rect-interface-scale`, e8aeae1 in the fork at github.com/vbardales/Rimworld-Pickle, not merged to its main nor sent to RimWorks; a Workshop Pickle still sends the pointer off screen at 150%
session:      local_bc1e5351-947b-42cf-a3a1-46da9c81cff9
updated:      2026-09-20, the 150% click defect measured and fixed in Pickle; six state-only scenarios moved down to the harness
---

# Architect Studio — status

## The last red scenario, and it was never ours — 2026-09-20

*screenshots of both editors at 150 percent* had been failing since the suite first ran, and this
file said above that the cause lay in our own interface-scale step. **It did not.** A probe
scenario written for the purpose, `18-tag-geometry.feature` with `Tests/Pickle/Source/TagProbe.cs`,
measured what Pickle is handed for a tagged button and what it turns that into, at both scales.

`GUIUtility.GUIToScreenRect` composes two spaces: it adds the clip origin **unscaled** and the
local offset **scaled**. For the same button on a 1920x1080 window:

| | clip origin | local rect | stored by `TagStore.Record` | true GUI rect |
| --- | --- | --- | --- | --- |
| 100% | `(0, 735)` | `y 285` | `y 1020` | `y 1020` |
| 150% | `(0, 375)` | `y 285` | `y 802.5` | `y 660` |

`375 + 285 x 1.5 = 802.5` is neither GUI space nor screen space. `InputBackends.ToScreen` then
multiplies by `Prefs.UIScale` again and the pointer goes to `1203` on a screen 1080 tall. At scale
1 the two spaces coincide, which is why exactly one scenario was red.

Ruled out with numbers, so nobody spends another afternoon on them: our scale step (the GUI space
does follow, `1920x1080` to `1280x720`), the Architect window's layout at that scale, a stale rect,
and re-indexing the tag store's guard on `UI.screenWidth`/`screenHeight`.

The fix is one statement in `TagStore.Record`, keeping the rect in GUI space, which is what every
consumer of it wants. Built, deployed into `Mods\Pickle-local`, and replayed twice in the WSL game
changing only which Pickle was staged: **with the fix 150% passes, with the Workshop copy it
fails**, and 100% passes either way, which is what rules out the environment. The failing run names
the defect exactly — *the pointer never reached (50.25, 814.00): the OS reports x:75 y:1079* — a y
of 814 in a GUI space 720 tall, clamped to the bottom edge. The scenario clicks the button for real
and then asserts the dialog opened, so this is a click landing, not a capture succeeding.
`Tests/Pickle/README.md` carries the full account and the code.

`16` is `@review`, so its green said only that the route ran; the two 150% captures it finally
produced were read by the mod's owner the same evening and found clean, no clipping and no raw
keys. That is the verification, not the passing scenario. They were taken on an English game, so
the French side of the translation gate is untouched by it. One limit is left in the `remaining`
list: the fix has not reached RimWorks — it is `fix/tag-rect-interface-scale`, e8aeae1 in the fork
— so anyone else's Pickle still has the defect.

One trap this session fell into, kept because the next person can fall into it too. The build first
deployed here was made from a Pickle checkout on `feat/clear-the-screen`, which branches *before*
`3f514b2`, "ignore a tagged rect measured at another interface scale" — so it carried the fix
while silently dropping that guard, which the binary it replaced had. The binary in that checkout's
`Assemblies/` had been built before the branch switch, and its md5 matching was taken as proof that
the working tree matched too. It was not. A second session caught it and replaced the build at
22:24 with one made on top of `3f514b2`; the deployed assembly now has `get_UiScale`,
`HeldAtAnotherScale` and `GUIToScreenPoint`, and no `GUIToScreenRect`. **Check the binary, and
check case-sensitively** — a case-insensitive search for `UiScale` matches `Prefs.UIScale` and
answers yes on every build, including the ones without the guard.

## Six scenarios moved down to unit tests — 2026-09-20

Six Pickle scenarios asserted stored state and nothing else — no click, no drawing, no reload —
so they confiscated a game session at every run to read fields a headless process reads in
milliseconds. They are now six assertions in `Tests/BehaviorTests.cs`, which goes from **27 to 33
passing assertions**, and they are gone from `04-arrows`, `05-category-order` and `09-appearance`
along with the seven steps that became orphaned in `Tests/Pickle/Source/CategorySteps.cs`.

What was kept in those same files is what only a running game shows: `05` compares the order
against the Architect window's own `desPanelsCached`, `04` against the menu's button order, `09`'s
icon asks Architect Icons itself. No coverage was lost — none of the six had an equivalent in the
harness before, each one is now asserted through the same mod code the scenario called, the group
arrows included, reached through the dialog's own private `ReorderMember`.

One boundary is worth naming: nesting a category needs Better Architect Menu's
`NestedCategoryExtension`, which the harness stands in for with a stub of the same type and field
name. The test therefore cannot tell that BAM still spells it that way — but neither could the
scenario, which skipped rather than failed when the type was missing.

## First full in-game run — 2026-09-20

The Pickle suite ran inside a real game for the first time: **40 Architect Studio scenarios, 39
passed, 1 failed**, the failure being a click at 150% interface scale. Measured later the same day:
the cause is in Pickle's tag store, which stores a rect converted half into screen space, and not
in this mod nor in the suite's own scale step as first thought. `Tests/Pickle/README.md` carries
the numbers. This supersedes the "not run yet" of the sections below for
every behaviour it covers; it does not by itself carry the stage to `tested`, because the English
and French walkthrough side by side and the RIMMSQOL half of scenario 15 remain unexecuted.

Five of the seven lines the `remaining` list carried since 2026-09-13 are settled by it, each by
named scenarios rather than by inference: dragging a group member, the up/down arrows, a category
forced on a whole group with members added later inheriting it, creating a category and its entry in
the keyboard configuration, and the option showing what research still locks.

**Two defects were found and fixed, both reaching the player, neither caught by any static check.**

- *Deleting a group belonging to another mod did not survive a settings reload.* `DissolveGroup`
  recorded the dissolution as one empty-string entry per member in `dropdownAssignments` - 64 of them
  for `Floor_Carpet` - and those entries did not come back through `Mod.GetSettings<T>()`, while
  `hiddenGroupIds`, written by the same call, did. A diagnostic log inside the reload showed the
  dictionary at zero entries with the hidden list intact. The dissolution is now carried by
  `hiddenGroupIds` alone, read through `DropdownRuntime.SkipHidden`, which no longer depends on that
  dictionary. A headless round-trip of 64 empty-value entries through the real Scribe passes, so the
  serializer was never the culprit; the reload path was.
- *`Restore deleted groups` brought nothing back.* It cleared `hiddenGroupIds` and never called
  `DropdownRuntime.Apply()`, so the buildings kept the group the dissolution had left them with.
  TESTING.md promised the members come back; now they do. The scenario that documented this as an
  expected failure is green.

A third defect was found by eye on a screenshot, which is what the `@review` scenarios exist for:
both editors drew their intro paragraph in a fixed 24px box with middle anchoring, so the line was
centred on a box too short for it and clipped at both ends as soon as it wrapped - which it does in
a narrower window or a longer language. Measured with `Text.CalcHeight` now.

The Workshop description now closes with `[url=...]Source code on GitHub[/url]` instead of the raw
URL it carried since 2026-09-12. PUBLISHING.md makes that closing link a blocking criterion of
`Preview générée -> preOptions`, so the stage was resting on an unmet condition; `Tests/Validate-Mod.ps1`
passes its 739 checks with the new form, and the target matches both the `<url>` field and the remote.

Three scenarios failed for days for a reason outside the mod entirely: RimIris kept a window over
the bottom-left corner of the screen and the OS click landed there instead of on the button. The
click step now names the covering window and its assembly, so the same situation reports itself
instead of reading as a dead button. Removing RimIris turned them green. `Tests/Pickle/README.md`
keeps the full account.

## Settings completion — 2026-09-13

Current result: **preOptions -> done**, ready for final in-game validation, not `tested`.
This section supersedes the earlier audit and override conclusions below while preserving their
history. The user's `go` authorized the shortcut and applicable settings tests. The ModIcon
override remains in force; neither image was changed.

Revision base: `d1bf712f0016993aff87ee18801ae296b2a55663`, with local changes to settings/reset,
the naming helper, the new MainButtons worker/Def, FR/EN resources, shipped DLL, tests and
documentation. The previous uncommitted STATUS edits were preserved. Nothing committed or
published. Complete test scope and limitations: **Tests/RESULTS.md**.

- Added `ArchitectStudio_Settings` MainButtonDef, hidden with native `buttonVisible=false`.
  Its worker opens the game's `Dialog_ModSettings` with `ArchitectStudioMod.Instance`, sharing
  the primary settings page and save path. It does not override visibility, remove the Def,
  require RIMMSQOL, or reset a customization tool's visibility choice.
- Confirmed the native 1.6 access mechanism by decompilation. Inspected local RIMMSQOL's
  Main Buttons enumeration and `buttonVisible` read/write implementation. No integration was
  interactively tested; reveal/hide/restart testing is explicitly in new scenario 15.
- Fixed a settings defect found during this work: full reset omitted both general preferences
  and was unavailable when only those differed from defaults. It now restores the Architect
  editor button to on and research visibility to off; both single-toggle cases are tested.
  Name trimming is extracted unchanged into a testable helper; no arbitrary length limit added.
- `dotnet build Source/ArchitectStudio.csproj -c Release --no-restore`: passed, zero
  warnings/errors. Shipped DLL SHA-256:
  `F674C2251982ECF06C6132A426535CA54D5EC4681DEF2D78B6BA3FC4ED70CB7C`.
- `pwsh -NoProfile -File Tests/Run-Behavior.ps1`: **27 passing assertions**, executing the
  shipped DLL and real Scribe serializer with installed game assemblies. Covers all stored
  settings collections and toggles, defaults/legacy empty files, accents/XML escaping,
  post-load invalid-record cleanup, effective stable ordering/reset, name boundaries, preference
  reset availability and the research runtime flag. This is a headless .NET 8 test process,
  not Unity Mono or a rendered game session. Harness fixtures/engine limitations are documented.
- `pwsh -NoProfile -File Tests/Validate-Mod.ps1`: **739 passing assertions** including new
  shortcut metadata and language coverage. `Check-DefInjected.ps1 -TransMod Mod`: **11,588 Defs
  indexed, 3 paths checked, zero errors**. Both are executed checks, not planned tests.
- Localization revalidated for the changed interface: shortcut label/description use native
  English Def fields with French injections; reset confirmation updated in both Keyed files.
  No new literal UI prose in C#. Existing 78-key inventory and parameter checks remain valid.
  No third-party text targets or conditional LoadFolders/patches were added.
- Dependency review remains valid: Harmony is mandatory, customization/integration mods remain
  optional. TESTING.md has 15 functional scenarios with setup/actions/expected outcomes and
  explicit new/existing-save, FR/EN, settings and shortcut checks. Relevant automated and XML
  tests now pass, so the cumulative gates through `done` are established under the user's
  clarification that in-game settings interaction belongs to `done -> tested`.

Next transition: execute the remaining functional scenarios in game, inspect Player.log and
FR/EN layouts, validate actual research/build prevention and group/category effects, persistence,
both settings routes and RIMMSQOL reveal/hide behavior in new/existing games. No in-game success
is inferred from the automated tests. The historical `tested_on` is retained as history only.

## ModIcon user override — 2026-09-13

The user explicitly overrode the ModIcon style finding ("j'override pour ModIcon").
The current delivered icon is accepted as-is: its composition exception no longer blocks
the workflow and no image correction is required. The original visual observation below is
preserved as audit history, not withdrawn or presented as newly conforming to the style guide.
Combined with the already validated build, Preview and naming checks, this advances the stage
from `horsMonoRepo` to **preOptions** and restores `showcase: complete`.
The override applies only to ModIcon; `settings_audit: partial` and the remaining settings
shortcut/technical-test requirements are unchanged. Only STATUS.md is edited; no tests rerun
or images changed for this documentary update.

## Ordered workflow audit — 2026-09-13

This section supersedes historical status conclusions below, without deleting their evidence.
Audited revision: `d1bf712f0016993aff87ee18801ae296b2a55663` (also GitHub HEAD).
The working tree was clean at entry. Only this status document is changed by the audit;
temporary build and 32 px inspection outputs are under ignored `.build/audit-20260913/`.
No source, shipped assembly, image or historical result was replaced; nothing was published.

Repository: `C:/Users/nelim/Documents/rimworld/ArchitectStudio`, with its own `.git`.
Distributed root: `Mod/`. The independent Git repository is nested geographically under the
collection directory, but has its own Git root and remote; it is not merely a monorepo subfolder.
The collection's own remote is irrelevant to this audit.

The user's ordered workflow takes precedence over the linked protocols, especially the
clarification that interactive settings tests belong to `done -> tested`, not `preOptions -> options`.
Stage values here use the workflow names literally: `horsMonoRepo` means independent repository
established; `ModIcon générée`, `Preview générée`, `preOptions`, `options`, `l10n`, `preTest`,
`done` and `tested` are its successive gates. Initial audit result: **horsMonoRepo** (previously
`done`); after the explicit ModIcon override above, the retained global stage is **preOptions**.
Later independent validations below do not imply passage through the remaining settings gate.

| Transition | Result | Evidence or blocker |
| --- | --- | --- |
| dansMonoRepo -> horsMonoRepo | Validated | Own Git root, origin, GitHub PUBLIC repository and pushed HEAD checked live; coherent identifiers, English documentation and distributed MIT/attribution copies. |
| horsMonoRepo -> ModIcon générée | Accepted by explicit user override | Release build and shipped binary freshness pass. Icon format is valid. The observed drawing-board backdrop and excess accessories are accepted as-is by the user on 2026-09-13. |
| ModIcon générée -> Preview générée | Independently validated | Delivered PNG directly inspected at 896 x 504 and existing 268 px thumbnail; 612,584 bytes. Overhead workshop, tiled floor, warm lamp/cool surroundings, no faces, readable title/version and no clipping. No concrete camera defect or unresolved camera doubt. |
| Preview générée -> preOptions | Independently validated | Amber accent clearly separates from slate-blue scene and blue secondary ink; palette and composition files present. English description and title; no prefix, suffix or linking word applies to Architect Studio. |
| preOptions -> options | Defect found; tests partly unverified | Useful settings and primary Mod options route exist, but no dedicated MainButtons shortcut exists. Applicable behavioral automation is absent; static assertions do not execute settings logic. |
| options -> l10n | Existing independent static validation retained and resource checks rerun | EN/FR resources pass, including generated category keys and the native Def label/injection. No relevant source/text change since the documented translation audit. Cannot advance cumulatively through the settings blocker. |
| l10n -> preTest | Independent declaration review passes | Harmony is used and mandatory; optional integrations use reflection and guarded fallbacks. About declares Harmony and appropriate loadAfter entries. No LoadFolders, version folders or conditional XML patches to reconcile. Runtime integration compatibility remains unverified. |
| preTest -> done | Partly validated; not established | Fourteen meaningful functional scenarios exist with setup/actions/expected results. Static automated/XML tests pass, but there is no automated behavioral suite or justified exemption for testable settings/order/persistence logic. |
| done -> tested | Unverified | No game session executed in this audit. Historical observations cover only part of the behavior; current complete FR/EN walkthrough, logs, persistence, new/existing games and integrations have no full passing record. |

### Checks executed and artifact evidence

- `git rev-parse --show-toplevel`, `git status --short`, `git remote -v`, `git log -1`;
  `gh repo view vbardales/Rimworld-Architect-Studio --json name,visibility,url,defaultBranchRef`
  returned PUBLIC/main; `git ls-remote origin HEAD` returned the audited revision.
  Initial sandbox network/config restrictions were resolved by a read-only elevated retry.
- `pwsh -NoProfile -File Tests/Validate-Mod.ps1`: **728 assertions passed**. Reviewed the
  validator itself: shipped XML parsing, metadata, nonempty/duplicate Keyed entries, EN/FR
  parity, indexed parameter parity, literal code keys, keybinding injection, notices and
  distribution cleanliness. These are static tests, not execution of runtime C#.
- `powershell -NoProfile -ExecutionPolicy Bypass -File ../scripts/Check-DefInjected.ps1 -TransMod Mod`:
  **11,587 Defs indexed, one key checked, zero errors**; 29 patch operations applied by the
  checker. The one owned target is `ArchitectStudio_OpenDropdowns.label`.
- `dotnet build Source/ArchitectStudio.csproj -c Release --no-restore -p:OutputPath=../.build/audit-20260913/bin/`:
  **passed, zero warnings/errors**, after retrying with SDK-cache access outside the sandbox.
  The initial MSB4184 access denial was environmental, not a source/build defect.
  References resolved to RimWorld **1.6.4871**, Harmony **2.4.2**, Publicizer **2.3.2**.
  Rebuilt and shipped DLL SHA-256 both equal
  `17360D9A443BC3B6408D853804E94B64B6BBE6E8A311E1E9145D7803F58AA376`.
- PNG metadata read directly: icon **128 x 128, 32,548 bytes**; preview **896 x 504,
  612,584 bytes**. Icon inspected at 128 and a diagnostic 32 px reduction: the head remains
  visible, but surrounding tools merge into clutter. This composition finding initially blocked
  the gate and was subsequently waived by the explicit user override above.
- Preview directly inspected at full and thumbnail sizes; the saved palette, HTML and QA report
  were reviewed. Historical measured contrast/font results remain preserved; the renderer was
  not rerun and those numerical measurements are not claimed as newly executed.
- Rights: README, ATTRIBUTION and both MIT notices are coherent with an original implementation
  that studied other mods without copying their code. Matching root/distribution copies passed.
  Public/original naming is appropriate; no licence was invented for Category Manager.

### Settings audit

`settings_audit: partial` reflects the actual missing shortcut and incomplete technical checks,
not a demand for in-game verification at this gate. Inspected the mod class, settings serializer,
editors, reset helpers, research visibility patches and reflection bridges.

- Useful configuration is global ModSettings: group membership/order/category/grid/icon source;
  category creation, parent, label, colour, icon and order; deleted-group restoration; two toggles.
  Controls exist through Mod options -> Architect Studio and its editor buttons; manual XML
  editing is not the primary configuration path.
- Defaults in fields and Scribe agree: `showArchitectButton=true`, `showResearchLocked=false`,
  schema version 1 and empty collections. Loading initializes missing collections and removes
  invalid null/id-less records. Writes call `WriteSettings`; startup reapplies stored changes.
  These facts were inspected, not demonstrated by a serialization round-trip test.
- The Architect button toggle applies when reopening that menu. Research visibility reads the
  setting at runtime and adds a disabled reason; proving build prevention still requires the
  corresponding scenario. Group/category edits update runtime definitions and relevant caches.
- Name entry trims whitespace and rejects an empty result; ordering controls disable boundary
  arrows; colour uses preset swatches. Automated invalid/long input, ordering, restore/reset and
  persistence checks have not been executed. No failure is inferred merely from that absence.
- `Mod/Defs/KeyBindings.xml` contains a keyboard binding, not a MainButtonDef. Source searches
  found only uses of vanilla Architect's MainButton and no custom registration. The visible
  buttons inside the Architect window do not satisfy the optional hidden MainButtons contract.
- No integration was interactively tested in this audit: neither RIMMSQOL nor Better Architect
  Menu, Architect Icons, Float Sub-Menus or Searchable Menus. Reflection review validates the
  optional dependency design, not compatibility with every installed version.

### Remaining work and non-blocking observations

To cross the next transition (`preOptions -> options`), provide the optional hidden MainButtons
shortcut opening the same settings and complete the applicable automated settings behavior
checks. Interactive in-game validation remains at `done -> tested`. No icon change is required
after the user override; unrelated build, Preview, translation and dependency checks are retained.

Publication follow-up, outside the next transition: About.xml contains a raw GitHub URL rather
than PUBLISHING.md's discrete Steam `[url=...]Source code on GitHub[/url]` closing link. Normalize
it before a future publication/description update; no Workshop edit was attempted here.
No additional camera-comparison document, image-generation history or redundant English
DefInjected file is requested. Existing language validation fields remain `complete` for the
unchanged implementation only; any future shortcut UI will require its own localization audit.

## Translation audit — 2026-09-13

- Scope: working tree based on `3441b9d8ef0c3fbfea7c828f930f8508d1fafe86`, with
  the translation changes described here. Inspected `Source/`, `Mod/Defs/` and
  `Mod/Languages/`. The mod has one shared content folder, no LoadFolders, XML patches,
  grammar resources or version-specific text. Runtime Harmony patches and optional
  integration branches are included.
- Inventory: settings and integration status; Architect buttons; dropdown lists, filters,
  counters, menus, confirmations and tooltips; category lists, appearance and naming dialogs;
  research-lock reason; the shortcut Def label and dynamically generated keyboard categories.
  Existing building/group/category labels use Def labels; custom names are user input.
- Fixed two hardcoded English strings in `CustomCategoryRuntime.EnsureKeyBindingCategory`:
  generated keyboard tab labels and descriptions now use complete parameterized Keyed text.
  Existing bindings are refreshed too, including those created by the game at startup.
- English and French each contain 78 owned Keyed entries. Reviewed meaning and parameters;
  XML, nonempty values, duplicate keys, EN/FR parity and literal source references pass.
  The shortcut uses its English Def label and the French injection
  `ArchitectStudio_OpenDropdowns.label`. No English DefInjected override is needed.
- Reused Core keys: `OK` and `CloseButton`, verified in installed English Keyed resources
  and `French (Français).tar` (OK / Fermer). No third-party translation keys are reused.
  Proper mod names, internal identifiers, icon paths, diagnostic logs and user-entered names
  are excluded. Punctuation and rich-text decoration around existing labels add no prose.
- Validation: `pwsh -NoProfile -File Tests/Validate-Mod.ps1` passed 728 assertions.
  `powershell -NoProfile -ExecutionPolicy Bypass -File ../scripts/Check-DefInjected.ps1
  -TransMod Mod` checked one injection with zero errors (11,587 indexed Defs).
  The repository build workflow now runs the static validator on every CI build.
  `dotnet build Source/ArchitectStudio.csproj -c Release --no-restore` passed with
  zero warnings/errors; the shipped DLL was rebuilt. Nothing was published.
- These fields certify the static translation gate only. English/French in-game checks
  have not run; their procedure is in `TESTING.md` and remains tracked above. Historical
  `stage` and `tested_on` are preserved. Reset affected fields after relevant changes.

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
- **Licence:** MIT, copyright (c) 2026 Nelim, in `LICENSE` and `Mod/LICENSE`.
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
