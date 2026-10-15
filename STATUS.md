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
workflow_stage: followUp[1.1.0]
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
  - feature 1.2.0 (owner choices 2026-10-14: 1A all tabs incl. base game, 2B hidden tab buildings fall into Unsorted when on, 3A checkbox in the category editor + Reset everything): coded on branch `feature/hide-tab` (e895bbe, worktree `../../rimworld-hidetab/ArchitectStudio`), NOT on main, stage stays followUp[1.1.0] until dormant then code[1.2.0] (AUDIT 14.e). Open before leaving each stage: 3.c CHANGELOG `[Unreleased]` entry; 4 dependencies recheck; 5 settings audit (hiddenCategories persisted, reset); 6 French review by the owner of the 3 new keys (`EditCategory.Hide`, `HideTip`, `Categories.HiddenTag`), `translation_fr` back to partial, FRENCH_REVIEW.md regenerated; 7 declare local steps (`HideTabSteps`, `ShowArchitectOnUnsorted`) to NPT, feature 24 and TESTING.md section; 8 Pickle run of 24 and 22 (tickets 5480, 8f4d filed with -Repo, not seen in the queue: asked the Ticket Manager), in-game check unverified: a hidden parent tab with Better Architect Menu children, Visible patch order vs research-locked; 8.m code review; Harmony pinned to 2.4.2 in the csproj (floating 2.* pulled 2.4.3), possible cause of the CI DLL mismatch
  - followUp 1.1.0: non-regression Pickle pass on the published build, filed 2026-10-14 at 17:32 (14.a): minimal en/fr `a1ab` `f185`, optionals en `f3a3`, reviews en/fr `ebcd` `aa34`, RIMMSQOL en `7de2`, restart en `7785`, evidence in `Tests/Pickle/Evidence/full-110/`; verdict here and in docs/runs/; Workshop replies to Bone (hiding a whole tab: not possible yet, feature request, answer drafted 2026-10-14, not posted) and Ali50 (short thanks), then cleanup (14.c) and dormant
  - closed on 2026-10-14: 1.1.0 published, the Workshop description (two English edits, Unsorted line), Preview, tags and change notes sent by CI and read on the public page; gallery 0 to 6 uploaded by the owner; SarahSH answered by the owner on the Better Architect Menu page
  - closed on 2026-10-11: settings replay in French and English on `d52167b`, four tickets green (`20261011-012324-702-33a2` RIMMSQOL shortcut fr, `-012325-790-8463` restart chain fr, `-012326-678-5550` and `-012327-608-eec6` Architect button tooltip en, fr; `Tests/Pickle/Evidence/settings-replay/`); `settings_audit: passed`
  - closed on 2026-10-11: the review line of `FRENCH_REVIEW.md` is filled (owner, 2026-10-11, `6059cb3`)
  - closed on 2026-10-12: feature 1.1.0, the opt-in "Unsorted" Architect tab, coded in `f88c331` (UnsortedRuntime, setting `showUnsorted`, count in the category editor, EN/FR keys, Pickle feature 22, TESTING.md 22); to play at playTests, French to review before leaving writeTests; steps declared to NPT (PickleTools `Elsewhere/ArchitectStudio.md`, 2026-10-12); tested on the commit of this state, `Run-Behavior.ps1` 37 and `Validate-Mod.ps1` 1033 green
  - closed on 2026-10-14: gallery image 6 for 1.1.0 accepted by the owner, `Art/Gallery/6-unsorted-tab.jpg` (ticket `17a8`, `gallery-candidate-6/`); to upload to the Workshop with the next publication
  - closed on 2026-10-09: Basic Dropdowns row added to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, `30d160b`); gallery image `5-parent-row.jpg` accepted and uploaded by the owner; delete confirmation of a group the player made captured in French (`full-1007/delete-confirm-fr`), wording as reviewed. Moves to `docs/runs/` at the next cleanup
session:      local_4a8e2ff6-9358-4fba-9a31-f91aae222ed1
updated:      2026-10-14, workflow_stage `followUp[1.1.0]` (1.1.0 published with preview, description and tags; tag and release v1.1.0 by CI; gallery 0 to 6 uploaded by the owner; regression pass and Workshop replies still open)
protocols_read_sha: 583ce1259235b54ebacca29e1116689d1d24e589
---

# Architect Studio: status

## State
- Publication of 1.1.0 (2026-10-14): publish run 38587531042 on `81a8439c62a29c8e90797232f6078dc83c83968e` (options preview, description, tags; dry-run 38587371650 green with the same options), approved by the owner on `steam-production`; jobs `publish` and `tag-and-release` success; tag and release `v1.1.0` on that SHA. Public page read: description with the Unsorted line, change notes 1.1.0 (9 entries), Preview, tags `Mod, 1.6`, updated 14 Oct 17:15. Gallery uploaded by the owner. `STEAM_REFRESH_TOKEN` was added to `steam-production` for the tags.

**Published: 1.0.7, 2026-10-08** (tag and release `v1.0.7`, SHA `5869dc26632c43336915c469602c4c355253688a`, dry-run 37799121470, publish 37799288436 approved by the owner). Workshop item `3792784018`. Rollback target for the next publication: `v1.0.7`. Previous: `v1.0.6` (2026-10-05), `v1.0.5` (2026-09-25).

**What 1.0.7 carries:** a category that is not ours moves under another from its appearance window (parent row, needs Better Architect Menu; request by Ali50); two French corrections kept by the owner (`ArchitectStudio.Categories.Intro`, `ArchitectStudio.Dropdowns.ConfirmDelete`, `9e7c8e2`; the other three suggestions of the reviewer were not kept; `.Zero` forms accepted); the gallery folder of the publication config points to `Art/Gallery`.

**Policy:** fail fast (owner, 2026-09-25): the deploy does not wait for the non-regression, which runs right after it.

**Gates, on the published revision:** `Tests/Validate-Mod.ps1` 1000 checks; `Tests/Run-Behavior.ps1` 35 assertions; Pickle non-regression after the deploy complete on 2026-10-09 in `Tests/Pickle/Evidence/full-1007/` (`minimal-en/fr`, `optionals-en`, `reviews-en`, `rimmsqol-en`, `restart-en` green; `reviews-fr` 42 passed, its one failure a missing fixture save, replayed alone and passed). Scenario 08 (a tab moved under another), 09 and 11 (moved tab locked by research, `full-1007`) played green. `translation_fr` and `translation_en` complete (French reviewed by the owner, `FRENCH_REVIEW.md` regenerated from the committed XML).

**Reviews:** `code_review_sha` `1a645f3` (code review of `Source/` and the 1.1.0 keys, `6059cb3..1a645f3`, no finding, 2026-10-14; the 1.1.0 code is `f88c331`). `publication_changelog_review_sha` `7c34a44` (content confirmed by the owner in chat on `2e0a58b`, 2026-10-14, for the 1.1.x line; `7c34a44` only dates the heading; the previous one, `6059cb3`, covered 1.0.x). `echo_review_sha` `41c67f2`: echo kept at `mountPreview` (2026-10-14), a menu window with an empty search bar, a row of slots and five category icons: it still shows what the gallery shows (the Architect menu, groups and tabs, now also the Unsorted tab), the subject of the mod has not moved; the Preview is not reopened. `social_preview_sha256`: the GitHub social preview is the `Mod/About/Preview.png` of 2026-10-12, byte-identical to it today (sha256 `bcb3f205`); no new upload owed.

**Evidence kept:** `settings-replay/` (2026-10-11, four tickets), `full-1007/ (latest report of each scenario, scenarios 08, 09 and 11 included), `gallery-sb/` (feature 17 on SanctuaryBacklot staging, 2026-10-09: the sources of gallery images 1 to 4, identical to the images online) and `gallery-candidate-5/` (image 5), both cited in `PUBLICATION.md`. `full-1006/`, `locked-tab-1008/`, `feature-move-category/`, `full-1001/` and `sb-staging-1007/` deleted on 2026-10-09 (every scenario they proved passes in `full-1007`). History in `docs/runs/`.

**Gallery (`Art/Gallery/`, 0 to 6 contiguous, 1.9 MB in all; 6 not online yet):** 0 `0-preview.png` the Preview (byte-identical to `Mod/About/Preview.png`); 1 `1-groups.jpg` the group editor; 2 `2-categories.jpg` the category editor; 3 `3-settings.jpg` the settings page; 4 `4-architect-menu.jpg` the Architect menu with a group open; 5 `5-parent-row.jpg` the appearance window of a tab moved under another (1.0.7, accepted by the owner 2026-10-09).; 6 `6-unsorted-tab.jpg` the Architect menu on the Unsorted tab (1.1.0, accepted by the owner 2026-10-14, to upload at publication). The order is the one of `PUBLICATION.md`.

**ModIcon:** `Mod/About/ModIcon.png` is 128 x 128 and 31 360 bytes, a little above the 20-30 KB guide; the owner validated it as it stands on 2026-10-11.

**Override, illustration source (owner, 2026-10-11):** `Art/Preview-source.png` measures a mean value of 0.260 (`Measure-Palette.sh`, 2 hue families), under the 0.30 of AUDIT 2.e. The owner keeps it as it stands: the Preview is accepted and was uploaded as the GitHub social preview. No regression of `workflow_stage`.

**Dependencies and DLC (2026-10-12, 1.1.0 on `f88c331`):** unchanged. The Unsorted tab references only Core types (`DesignationCategoryDef`, `Designator_Build`, `BuildableDef`): no new mod, no DLC; `About.xml` and `LoadFolders` stand as declared (Harmony required; Better Architect Menu, Architect Icons, Float Sub-Menus optional by reflection).

**Settings audit 1.1.0 (2026-10-12, `settings_audit: partial`):** a third preference, `showUnsorted` (off by default, global to all saves, applied at once: the tab is created or removed without a restart; a restart rebuilds it from the file). Real use: SarahSH asked that no building stay out of reach; an opt-in tab adds a button, hence off by default. Tip text says what it does and that it changes no building. Reached by Options > Mod settings, same page; part of `HasNonDefaultPreferences`, so "Reset everything" is offered and puts it back off (Pickle scenario "the reset puts the preference back"). Persisted as `showUnsorted` in `ExposeData`. Automated: `Validate-Mod.ps1` 1033 checks green, `Run-Behavior.ps1` 37 green, Pickle feature 22 written (five scenarios). In game, English and French: `unverified` until `playTests`.

**Playtests 1.1.0 (2026-10-14, `settings_audit: passed`):** feature 22 (five scenarios) green in English and French on `e640e2e` (`Tests/Pickle/Evidence/unsorted-en/`, `unsorted-fr/`, code unchanged since `f88c331`); feature 23 (two `@review` captures, `unsorted-lang-en/` ticket `422f`, `unsorted-lang-fr/` ticket `031f`, `avec-infobulles` map, on `3ed48ee`) green and read: the tooltip of "Add an Unsorted tab" / « Ajouter un onglet « Non classé » » is complete and not truncated, no raw key, the Unsorted / Non classé tab shows in the Architect menu. `Player.log`: nothing from Architect Studio (only WSL audio and the empty companion tools). The first French ticket (`40c4`) was lost by the dispatcher and refiled. `TESTING.md` stands for `TEST_SCENARIOS.md` (owner, 2026-10-14).

**Settings audit (2026-10-11, `settings_audit: passed`):** two preferences, `showArchitectButton` (on by default) and `showResearchLocked` (off by default), plus the stored edits of the two editors (groups, categories, labels, icons, colours, parents); the page is reached by Options > Mod settings, the MainButtons shortcut is hidden by default and offered by RIMMSQOL. In `Tests/Pickle/Evidence/full-1007/` the reset of each preference (en and fr), the settings page opened through the shortcut (en and fr), the deleted-group replay (en and fr), the restart chain (en) and the RIMMSQOL shortcut, revealed and hidden again (en), are green. Replay filed on 2026-10-11 and green: the RIMMSQOL shortcut in French (3 scenarios, the settings page read in French on the screenshot), the restart chain in French (2 launches, both passed; no screenshot, the language is the one of the ticket), the tooltip of the Architect button setting in English and in French (the two screenshots read: tooltip text and page in the right language). `passed` is claimed on that evidence.

**Translation audit 1.1.0 (2026-10-12):** three Keyed keys added in both languages (`ArchitectStudio.Settings.ShowUnsorted`, `ArchitectStudio.Settings.ShowUnsortedTip`, `ArchitectStudio.Unsorted.Label`); no hard-coded text (the tab label is read through `.Translate()` when the tab is created). `Validate-Mod.ps1` 1033 checks green, `Check-Translations.ps1` green, `FRENCH_REVIEW.md` regenerated at `f88c331`. The three French strings (« Ajouter un onglet « Non classé » », its tip, « Non classé ») are mine and not reviewed: read and validated by Virginie on 2026-10-12 on `f88c331` (`FRENCH_REVIEW.md` review line), `translation_fr: complete`. In game (raw keys, truncation of the longer tip): `unverified` until `playTests`.

**Translation audit (2026-10-11):** English and French are embedded (`Mod/Languages/English`, `Mod/Languages/French`); `Tests/Validate-Mod.ps1` 1000 checks green, `Check-DefInjected.ps1` 3 keys, 0 errors, `FRENCH_REVIEW.md` regenerated and unchanged. The French was reviewed by the owner (two corrections kept, `9e7c8e2`, 2026-10-08, see above); the review line of `FRENCH_REVIEW.md` is still blank, to be filled by her.

**Closing pass at `published`, 2026-10-09:** mod root clean (`.build/` deleted, regenerated by the builds; `.editorconfig` now at `rimworld/.editorconfig`), `Art/` clean (`Preview-original.png` removed), stale lines corrected, old branches deleted, the dated sections of this file moved to `docs/runs/2026-10-09-status-history.md`.

**Still true of the product:** a Pickle at `v6` takes clicks at the widget, so the 150% scenario holds without the fork (PR 23 closed, step and scenario in RimWorks PR 42); `Mod/desktop.ini` is no longer shipped (untracked 2026-10-01); the settings page lists the three detected integrations, Searchable Menus and Basic Dropdowns have no detection and are named on the Workshop page only.
