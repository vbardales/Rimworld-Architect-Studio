---
settings_audit: passed
localization: complete
translation_en: complete
translation_fr: complete
mod:          Architect Studio
packageId:    nelim.architectstudio
repo:         Rimworld-Architect-Studio
visibility:   public
detached:     yes
workflow_stage: code[1.2.0]
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
upstream_mod_remotes:
  - https://github.com/fernyrepos/Better-Architect-Menu.git
  - https://github.com/fernyrepos/Colored-Categories.git
dependencies: declared
showcase:     complete
publication_changelog_review_sha: 7c34a444f1baeab83498663311513ce1e7b187aa  # content reviewed and validated by the owner in chat on 2e0a58b, 2026-10-14 (1.1.x line); 7c34a44 only adds the date to the changelog heading and the Steam change note (12.e)
code_review_sha: 1a645f359b08c4472ec234d1cd9667a4be452a79
echo_review_sha: 41c67f2fbdc7cee30f4ba3af68afd7f379bdb8cc  # echo-source.png kept at mountPreview 2026-10-14 against gallery 0 to 6; ModIcon badge cropped at f506f2c
social_preview_sha256: bcb3f205674595d74059a911a7b6fabe006bfb9cb9ca02fc020e17a13e2616f5  # uploaded 2026-10-12 (new echo-source.png); the image served by GitHub is byte-identical to Mod/About/Preview.png
tested_on:    2026-10-14
workshop:     3792784018
remaining:
  - feature 1.2.0 (owner choices 2026-10-14: 1A all tabs incl. base game, 2B hidden tab buildings fall into Unsorted when on, 3A checkbox in the category editor + Reset everything): coded on branch `feature/hide-tab` (e895bbe, worktree `../../rimworld-hidetab/ArchitectStudio`), NOT on main, stage stays followUp[1.1.0] until dormant then code[1.2.0] (AUDIT 14.e). Open before leaving each stage: 3.c CHANGELOG `[Unreleased]` entry; 4 dependencies recheck; 5 settings audit (hiddenCategories persisted, reset); 6 French review by the owner of the 3 new keys (`EditCategory.Hide`, `HideTip`, `Categories.HiddenTag`), `translation_fr` back to partial, FRENCH_REVIEW.md regenerated; 7 declare local steps (`HideTabSteps`, `ShowArchitectOnUnsorted`) to NPT, feature 24 and TESTING.md section; 8 Pickle run of 24 and 22 (tickets 5480 and 8f4d ended with launcher exit 143 and no report; refiled 3957 and 92c5 on 2026-10-15), in-game check unverified: a hidden parent tab with Better Architect Menu children, Visible patch order vs research-locked; 8.m code review; Harmony pinned to 2.4.2 in the csproj (floating 2.* pulled 2.4.3), possible cause of the CI DLL mismatch
  - followUp 1.1.0: non-regression done and green on 81a8439 (2026-10-15, docs/runs/2026-10-14-publication-1.1.0.md, `full-110/`); still open: Workshop replies to Bone (not posted: hiding a whole tab is on branch feature/hide-tab for 1.2.0) and Ali50 (short thanks), cleanup 14.c, branches 14.d, dormant
  - closed on 2026-10-14: 1.1.0 published, the Workshop description (two English edits, Unsorted line), Preview, tags and change notes sent by CI and read on the public page; gallery 0 to 6 uploaded by the owner; SarahSH answered by the owner on the Better Architect Menu page
  - closed on 2026-10-11: settings replay in French and English on `d52167b`, four tickets green (`20261011-012324-702-33a2` RIMMSQOL shortcut fr, `-012325-790-8463` restart chain fr, `-012326-678-5550` and `-012327-608-eec6` Architect button tooltip en, fr; `Tests/Pickle/Evidence/settings-replay/`); `settings_audit: passed`
  - closed on 2026-10-11: the review line of `FRENCH_REVIEW.md` is filled (owner, 2026-10-11, `6059cb3`)
  - closed on 2026-10-12: feature 1.1.0, the opt-in "Unsorted" Architect tab, coded in `f88c331` (UnsortedRuntime, setting `showUnsorted`, count in the category editor, EN/FR keys, Pickle feature 22, TESTING.md 22); to play at playTests, French to review before leaving writeTests; steps declared to NPT (PickleTools `Elsewhere/ArchitectStudio.md`, 2026-10-12); tested on the commit of this state, `Run-Behavior.ps1` 37 and `Validate-Mod.ps1` 1033 green
  - closed on 2026-10-14: gallery image 6 for 1.1.0 accepted by the owner, `Art/Gallery/6-unsorted-tab.jpg` (ticket `17a8`, `gallery-candidate-6/`); to upload to the Workshop with the next publication
  - closed on 2026-10-09: Basic Dropdowns row added to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, `30d160b`); gallery image `5-parent-row.jpg` accepted and uploaded by the owner; delete confirmation of a group the player made captured in French (`full-1007/delete-confirm-fr`), wording as reviewed. Moves to `docs/runs/` at the next cleanup
session:      local_4a8e2ff6-9358-4fba-9a31-f91aae222ed1
updated:      2026-10-15, workflow_stage `code[1.2.0]` (reopened from dormant for Bone's request; feature/hide-tab merged into main, CHANGELOG [Unreleased] written; Pickle 24 and 22 refiled as 3957 and 92c5)
protocols_read_sha: 228c98ab061efe4883c879bd8e5be6d97c22c128
---

# Architect Studio: status

## State
- **Published: 1.1.0, 2026-10-14** (tag and release `v1.1.0`, SHA `81a8439c62a29c8e90797232f6078dc83c83968e`, dry-run 38587371650, publish 38587531042 approved by the owner; options preview, description and tags). Workshop item `3792784018`, page read after the upload. Rollback target for the next publication: `v1.1.0` (then `v1.0.7`). Non-regression of 1.1.0 green on that revision (`full-110/`, 2026-10-15). History of 1.0.x and of the 1.1.0 audits: `docs/runs/2026-10-09-status-history.md`, `docs/runs/2026-10-15-status-history.md`, `docs/runs/2026-10-14-publication-1.1.0.md`.
- Next: 1.2.0, hiding a whole Architect tab (Bone), coded on branch `feature/hide-tab`; see `remaining`. Policy: fail fast (owner, 2026-09-25).

**Reviews:** `code_review_sha` `1a645f3` (code review of `Source/` and the 1.1.0 keys, `6059cb3..1a645f3`, no finding, 2026-10-14; the 1.1.0 code is `f88c331`). `publication_changelog_review_sha` `7c34a44` (content confirmed by the owner in chat on `2e0a58b`, 2026-10-14, for the 1.1.x line; `7c34a44` only dates the heading; the previous one, `6059cb3`, covered 1.0.x). `echo_review_sha` `41c67f2`: echo kept at `mountPreview` (2026-10-14), a menu window with an empty search bar, a row of slots and five category icons: it still shows what the gallery shows (the Architect menu, groups and tabs, now also the Unsorted tab), the subject of the mod has not moved; the Preview is not reopened. `social_preview_sha256`: the GitHub social preview is the `Mod/About/Preview.png` of 2026-10-12, byte-identical to it today (sha256 `bcb3f205`); no new upload owed.

**Evidence kept:** `full-110/` (non-regression of 1.1.0 on `81a8439`), `settings-replay/`, `unsorted-en/fr/` and `unsorted-lang-en/fr/` (features 22 and 23, 1.1.0), `gallery-sb/` and `gallery-candidate-5/`, `gallery-candidate-6/` (sources of gallery images 1 to 6, cited in `PUBLICATION.md`); `full-1007/` is superseded by `full-110/`.

**Gallery (`Art/Gallery/`, 0 to 6 contiguous, 1.9 MB in all; all online since 2026-10-14):** 0 `0-preview.png` the Preview (byte-identical to `Mod/About/Preview.png`); 1 `1-groups.jpg` the group editor; 2 `2-categories.jpg` the category editor; 3 `3-settings.jpg` the settings page; 4 `4-architect-menu.jpg` the Architect menu with a group open; 5 `5-parent-row.jpg` the appearance window of a tab moved under another (1.0.7, accepted by the owner 2026-10-09).; 6 `6-unsorted-tab.jpg` the Architect menu on the Unsorted tab (1.1.0, accepted by the owner 2026-10-14, to upload at publication). The order is the one of `PUBLICATION.md`.

**ModIcon:** `Mod/About/ModIcon.png` is 128 x 128 and 31 360 bytes, a little above the 20-30 KB guide; the owner validated it as it stands on 2026-10-11.

**Override, illustration source (owner, 2026-10-11):** `Art/Preview-source.png` measures a mean value of 0.260 (`Measure-Palette.sh`, 2 hue families), under the 0.30 of AUDIT 2.e. The owner keeps it as it stands: the Preview is accepted and was uploaded as the GitHub social preview. No regression of `workflow_stage`.

**Still true of the product:** a Pickle at `v6` takes clicks at the widget, so the 150% scenario holds without the fork (PR 23 closed, step and scenario in RimWorks PR 42); `Mod/desktop.ini` is no longer shipped (untracked 2026-10-01); the settings page lists the three detected integrations, Searchable Menus and Basic Dropdowns have no detection and are named on the Workshop page only.
