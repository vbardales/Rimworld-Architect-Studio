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
stage:        prepublished
workflow_stage: prepublished
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
upstream_mod_remotes:
  - https://github.com/fernyrepos/Better-Architect-Menu.git
  - https://github.com/fernyrepos/Colored-Categories.git
dependencies: declared
showcase:     complete
tested_on:    2026-09-21
workshop:     3792784018
remaining:
  - unverified: the whole suite on the published revision (fail fast, owner decision 2026-10-05: it runs after the deploy; tickets db18 to f46b on `8e86471` are in flight, `minimal-en` green 35/0/30 skipped); a red afterwards means rollback to `v1.0.5` (`ccad168`)
  - unverified: the French reviews pass of `68fe26f` (42 passed of 51) has no report left on disk, only its line in `docs/runs/2026-09-25-pass-matrix.md`
  - defect: `Mod/desktop.ini` was tracked and shipped to subscribers; untracked and ignored on 2026-10-01, gone from the next publication
  - unverified: Basic Dropdowns (ex Categories Dropdowns, `3455529827`): the description now names it once and thanks ferny for it (About.xml and PUBLICATION.md, 2026-10-02, validator 989); the Workshop page still shows the old text until a description update, and the comment register (`WORKSHOP_COMMENTS.md`, protocols repository) has no row for it
  - feature: pull requests to the origin repositories `fernyrepos/Better-Architect-Menu` and `fernyrepos/Colored-Categories` (`BACKLOG.md`, owner rule of 2026-09-28; nothing is sent without her agreement)
  - external: the Workshop description standard changed on 2026-09-25 (CI/CD, Rimworld-Release-Admin f196148): one Markdown block under `## Steam description` of PUBLICATION.md with `aboutDescription: true`, replacing this repository's BBCode block; adopt at the next publication, ask CI/CD for the plugin pull request; a new dry-run follows because the SHA changes
  - unverified: owner reading of the review media: the member drag film validated; group editor at 150% no defect, gallery use needs a crop; French category editor wording validated; the long-name capture (media 5) validated by the owner on 2026-09-28, cut kept; if it goes to the Workshop gallery it must be cropped first. The other media are read by me, not by the owner
  - external: the Pickle defect behind that scenario is fixed on `fix/tag-rect-interface-scale`, 122f21f in the fork, sent to RimWorks as PR 23 (github.com/RimWorks/Rimworld-Pickle/pull/23) and not merged yet; a Workshop Pickle still sends the pointer off screen at 150%. Played on 2026-09-21 against a Pickle built from that PR alone, the feature holding the scenario passes 3 of 3, the 150% one included
  - unverified: read against each mod's own source (`Search-Workshop.sh`, per SEARCHING.md) to find what to test. Float Sub-Menus was already named: scenario 13 (`ModSteps.IntegrationsMatchLoadedMods`) asserts `FloatSubMenuCompat.Available` matches whether `kathanon.floatsubmenu` is loaded, in every pass. Two new scenarios (`-DepMap wsl-deps.avec-facultatifs.map`): `22e9` staged all 16 mods then Pickle exited 2 (0 scenarios played); `0169` retried the same and failed the same way, for a real reason this time: my `-Filter "08-category-create|10-delete-foreign-group"` is not a regex alternation, Pickle read it as one literal filter and matched nothing (`Player.log`: "filter '...' matched no scenarios"). Split into two tickets, one filter each: `8742` (§8) and `f3ae` (§10, not yet returned). `08-category-create.feature` (`@requires:ferny.betterarchitect`) creates a category under a parent and asserts `BetterArchitectCompat.ParentCategoryOf` reports it, the real nesting TESTING.md §5 and §8 call for, never driven before — **passed on `8742`, 4/4 of the feature, English**. `10-delete-foreign-group.feature` (`@requires:ferny.categoriesdropdowns`) dissolves one of that mod's own groups (`Ferny_Walls`) without crashing — its own `patch.xml` ships every building assignment commented out, so the group exists with no member on this list, which is enough to prove the dissolve path survives a rival mod's def — **passed on `f3ae`, 4/4 of the feature, English**. Searchable Menus stays untested on our side: by its own author's design (`FloatSubMenuCompat.cs`) it grafts a search field onto any menu directly, without our code calling into it, so there is nothing of ours to assert against; the with-optionals pass (it breaks nothing) is what coverage looks like for it
session:      local_fe5827e4-3c40-40ba-860f-ab81f7108d72
updated:      2026-10-05, 1.0.6 prepublished: dry-run green (see Fail-fast publication below); non-regression runs after the deploy, by the owner's decision
---

# Architect Studio — status

## Audit — 2026-10-01

**French review, 2026-10-02 (reported by the owner in chat, entered by me as her words, not my own review):** Virginie validated the French of `FRENCH_REVIEW.md` after two corrections (`Catégorie parente`; `détecté` / `non détecté`). `.Zero` singular forms accepted, no pawn agreement owed. Revision reviewed: `b6e1f9f` (XML committed, tree clean for `Mod/Languages`); the report was regenerated from it. `translation_fr: complete`, `workflow_stage: done`. Still open: the category editor look in game (clipping of the widened label) and the full suite, both in the pending tickets.

Audited revision `5b5fe20` (HEAD of `main`, level with `origin/main`) with local changes that are not this audit's: the badge work on `Art/` and `Mod/About/{Preview,ModIcon}.png`, kept untouched. No game was launched and no Pickle request was deposited (the tree is not frozen).

**Old state → retained state:** `stage: done` → `stage: showcase`, `workflow_stage: options`. Replaced on 2026-10-01: the `done` of 2026-09-25 ("stage stays done" after the 1.0.5 publication). The first transition that fails is `options → l10n`: TRANSLATIONS.md of 2026-09-30 forbids `translation_fr: complete` before the owner has read the French, and her review has not happened. It is a verification that is missing, not a defect of the French text; it is the only cause of the step back. Every transition up to `options` holds, and the `preTest → done` checks below were replayed anyway so the climb back is one step.

Codes: `showcase` covers `Preview générée` to `l10n`; `workflow_stage` carries the exact state.

| Transition | Result |
| --- | --- |
| `dansMonoRepo → horsMonoRepo` | Holds. Standalone repository, remote `vbardales/Rimworld-Architect-Studio`, in sync. `upstream_mod_remotes` filled; origin repositories checked with `gh` (`BACKLOG.md`). Documentation in English. |
| `→ ModIcon`, `→ Preview` | Holds, by direct inspection: `Mod/About/ModIcon.png` 128×128 RGBA 29 KB; `Mod/About/Preview.png` 896×504, 525 KB (< 1 MB), identical byte for byte to `Art/Gallery/0-preview.png`, ModIcon badge bottom left. Neither was generated or modified by this audit. |
| `→ preOptions` | Holds. English description, ends with `Source code on GitHub (…)`. Reservation: the description names Basic Dropdowns twice (see `remaining`). |
| `→ options` | Holds. `settings_audit: complete`, features 15 and 19 (hidden MainButtons shortcut, RIMMSQOL) played green earlier; nothing in the options changed since. |
| `→ l10n` | **Not established.** Keyed English/French: 86 keys each, same keys and placeholders, validator 989 checks passed today; counted phrases are `.Zero/.One/.Many` families. No French text agrees with a pawn, so no `{PAWN_gender ? … }` switch is owed (the mod names no pawn). Missing: the owner's reading of `FRENCH_REVIEW.md`. `translation_fr: partial`. |
| `→ preTest` | Holds (not re-run today beyond the files): `modDependencies` is Harmony alone, the `loadAfter` entries are the optional integrations and the DLCs; no `LoadFolders.xml` (single 1.6 folder). Audit date 2026-10-01, source read in `About.xml`. |
| `→ done` (replayed) | Validator `Tests/Validate-Mod.ps1`: **989 checks passed**. `Tests/Run-Behavior.ps1`: **34 runtime behavior checks passed**. The behavior harness loads `Mod/Assemblies/ArchitectStudio.dll`; its last commit (`4e86e04`) is also the last of `Source/`, so the DLL matches the sources. Pickle suites: 22 feature files written and justified in `Tests/Pickle/README.md`. |

**Checks added for `done → tested` (AUDIT.md, step 9), as they stand today:**

- *No scenario in `@wip`:* holds. `grep` finds the tag only in comments of `13-optional-mods.feature` that say it used to carry it.
- *Every `@requires` scenario has played, with the map that stages its mod, and its report read:* `ferny.betterarchitect` (feature 08) and `ferny.categoriesdropdowns` (feature 10) green in `matrix-fddea0d` (`wsl-deps.avec-facultatifs.map`); `com.bymarcin.architecticons` (the Architect Icons scenario) green in `full-1001/optionals-en` (run of 2026-10-01, see `docs/runs/`); `nelim.architectstudio.restartpass` (20, 21) green in `matrix-b7f8833`; `MalteSchulze.RIMMSqol` with `nelim.pickletools.rimmsqol` (19) green in `rimmsqol-en`; `screenshotstudio`, `screenshotmode`, `hoversteps`, `interfacescale`, `filmticks` (03, 04b, 16, 17, 18) green in `full-1001/reviews-en-retry`, `studio-en`, `counted-en-3` and `gallery-sanctuary-5-en`. `setName` and the scenario names of each report were read before citing it. **Gap:** these runs are on earlier revisions (`fddea0d`, the `full-1001` runs of 2026-10-01 to 05), not on the revision to publish. The French reviews pass is `full-1001/reviews-fr-3`. Trimmed on 2026-10-05: `matrix-current`, `matrix-84b7476`, `matrix-3fb0b93`, `matrix-4e86e04` and six superseded `full-1001` runs were deleted (their scenarios replayed in `counted-*-3`, `reviews-*`, `minimal-*`, `optionals-en`, `studio-en`, `rimmsqol-en`).
- *No manual test left to validate:* holds. TESTING.md 15.4 and 15.5 were automated on `68fe26f`; the disposable new game of section 15 is not applicable (reason in TESTING.md); the owner validated the long-name capture on 2026-09-28. The `@review` captures were opened by me (not a manual test); the owner read the member-drag film, the 150% editors and the French category editor.

Not established, so `tested` stays out of reach: the full suite on the revision that will be published (`remaining`, `unverified`).

**Housekeeping done in this audit** (no mod behavior changed):

- `.dds` files: none tracked, none on disk, `*.dds` already in `.gitignore`.
- `Mod/desktop.ini` untracked and ignored (with `Mod/**/desktop.ini` and `Mod/*.ico`): Steam sends `Mod/` as it stands and the file was in earlier releases.
- Evidence: nothing tracked in git; `Tests/Pickle/Evidence/` trimmed from 482 MB to 101 MB (list in `Tests/Pickle/README.md`, "What is on disk"); the `stalled-ArchitectStudio-0928-0759` archive of `pickle-reports-archive/` deleted (the only archive of this mod). Other mods' archives untouched. `messages.ndjson` of the restart chain is gone; the process ids are still in the two `junit.xml`.
- `CHANGELOG.md`: no `0.1.0` entry added. `Mod/About/PublishedFileId.txt` exists (`3792784018`) but the item was created before `1.0.0` and the file is append-only; an entry now would put a `0.1.0` below the `1.0.0` it should precede. The prepublication act is not recorded in the changelog and is not invented.
- `docs/PROTOCOLS-READ.md` rewritten with the versions read today; `BACKLOG.md` created (pull requests to ferny's repositories); `FRENCH_REVIEW.md` and `Tests/New-FrenchReview.ps1` created.

## Preview source migration — 2026-10-02

Copy, typography, layout and palette are consolidated in `Art/Preview.config.json`. The canonical inputs are `Art/Preview-source.png`, `Art/echo.png` and `Art/ModIcon-source.png`; the shared renderer writes temporary diagnostics under ignored `Art/.render/`. Existing distributed Preview, gallery and ICO outputs were preserved because they were present and coherent; no render was run for this migration. Superseded JSON files and generated QA intermediates were removed. Nothing published.

`licence` vocabulary: `original` an original mod idea, not an update or continuation of another mod; studying other mods or integrating with them does not exclude this classification. For updates or continuations: `open` an explicit licence, `silent` no licence and a dead source, `alive` no licence but a living source, `forbidden` a written refusal.

## Code review — 2026-10-05

`/code-review` (low effort) of `Source/` from `v1.0.0` (`b919046`) to `2b16bdf252c8221a3d171c2ad9fd27162b7aa27d`: no finding. `Source/` last changed in `f553e8c`; `2b16bdf` only trims evidence. Tests and fixtures were not reviewed at this level.

## Fail-fast publication — 2026-10-05

Owner decision: 1.0.6 is deployed before the non-regression passes, the gallery captures (`Art/Gallery/0` to `4`) being ready. Rollback target `v1.0.5` (`ccad168`). Dry-run of `8e8647131a2cb0dbf9bb4a6587e7ae10d00ad317` (run 37342232075, preview and description): green. The SHA to publish is the one of the dry-run recorded in `docs/runs/`.
