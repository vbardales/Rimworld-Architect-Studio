---
settings_audit: partial
localization: complete
translation_en: complete
translation_fr: complete
mod:          Architect Studio
packageId:    nelim.architectstudio
repo:         Rimworld-Architect-Studio
visibility:   public
detached:     yes
workflow_stage: writeTests[1.1.0]
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
upstream_mod_remotes:
  - https://github.com/fernyrepos/Better-Architect-Menu.git
  - https://github.com/fernyrepos/Colored-Categories.git
dependencies: declared
showcase:     complete
publication_changelog_review_sha: 6059cb345fe7b473acbbf94eaa864de26fc65f17  # reviewed (confirmed 2026-10-11) on this commit, PUBLICATION.md and CHANGELOG.md
code_review_sha: 6059cb345fe7b473acbbf94eaa864de26fc65f17
echo_review_sha: f9b264500faf675d635429ff65330f425f8b7244  # echo validated by the owner on 2026-10-10 (keep): it still fits the accepted gallery, the subject of the mod has not moved
social_preview_sha256: 6d36e22e6a97b34d75f501c334b5b5ef2b8a0eb35c4e5696454e603f299f1805  # uploaded by the owner on 2026-10-10; the image served by GitHub is byte-identical to Mod/About/Preview.png
tested_on:    2026-10-09
workshop:     3792784018
remaining:
  - external: the Workshop page still shows the pre-1.0.6 description (1.0.7 did not send it: `update_description` is off by default). It goes out with the next publication: dry-run with `update_description=true` on the new SHA, its text read against the live page, `dispatch-publish.sh ... --description`, the owner approves `steam-production`, the page checked afterwards (`update_description` is not yet proven against real steamcmd). The two English description edits (next entry) wait for it
  - feature: two English edits to the public description PLUS a sentence for the Unsorted tab (suggested by the review of 2026-10-12, not yet in PUBLICATION.md, owner to confirm);, to go out with the next publication run with `update_description=true` (they change the Workshop page and `About.xml`; owner agreement implied by the next-publication rule, wording from the review of 2026-10-09): `...and any you add later follow.` becomes `...and any buildings added later follow.`; `...where the game silently produces several separate buttons.` becomes `...: the game silently produces several separate buttons.` (the full stop of the preceding clause replaced by the colon); detail in BACKLOG.md
  - closed on 2026-10-11: settings replay in French and English on `d52167b`, four tickets green (`20261011-012324-702-33a2` RIMMSQOL shortcut fr, `-012325-790-8463` restart chain fr, `-012326-678-5550` and `-012327-608-eec6` Architect button tooltip en, fr; `Tests/Pickle/Evidence/settings-replay/`); `settings_audit: passed`
  - closed on 2026-10-11: the review line of `FRENCH_REVIEW.md` is filled (owner, 2026-10-11, `6059cb3`)
  - closed on 2026-10-12: feature 1.1.0, the opt-in "Unsorted" Architect tab, coded in `f88c331` (UnsortedRuntime, setting `showUnsorted`, count in the category editor, EN/FR keys, Pickle feature 22, TESTING.md 22); to play at playTests, French to review before leaving writeTests
  - closed on 2026-10-09: Basic Dropdowns row added to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, `30d160b`); gallery image `5-parent-row.jpg` accepted and uploaded by the owner; delete confirmation of a group the player made captured in French (`full-1007/delete-confirm-fr`), wording as reviewed. Moves to `docs/runs/` at the next cleanup
session:      local_4a8e2ff6-9358-4fba-9a31-f91aae222ed1
updated:      2026-10-10, workflow_stage `dormant` (1.0.7 published 2026-10-09; non-regression, thanks comments, cleanup and branches checked against AUDIT 14.a to 14.d); closing pass and cleanup at `published` done (STATUS.md, `Art/`, mod root, evidence); gallery image 5 accepted and uploaded by the owner, images 1 to 4 replayed on SanctuaryBacklot staging (identical); delete-confirmation capture read (French)
protocols_read_sha: 3bb4b9701f253249217d7e830afcd62aa2eb1da9
---

# Architect Studio: status

## State

**Published: 1.0.7, 2026-10-08** (tag and release `v1.0.7`, SHA `5869dc26632c43336915c469602c4c355253688a`, dry-run 37799121470, publish 37799288436 approved by the owner). Workshop item `3792784018`. Rollback target for the next publication: `v1.0.7`. Previous: `v1.0.6` (2026-10-05), `v1.0.5` (2026-09-25).

**What 1.0.7 carries:** a category that is not ours moves under another from its appearance window (parent row, needs Better Architect Menu; request by Ali50); two French corrections kept by the owner (`ArchitectStudio.Categories.Intro`, `ArchitectStudio.Dropdowns.ConfirmDelete`, `9e7c8e2`; the other three suggestions of the reviewer were not kept; `.Zero` forms accepted); the gallery folder of the publication config points to `Art/Gallery`.

**Policy:** fail fast (owner, 2026-09-25): the deploy does not wait for the non-regression, which runs right after it.

**Gates, on the published revision:** `Tests/Validate-Mod.ps1` 1000 checks; `Tests/Run-Behavior.ps1` 35 assertions; Pickle non-regression after the deploy complete on 2026-10-09 in `Tests/Pickle/Evidence/full-1007/` (`minimal-en/fr`, `optionals-en`, `reviews-en`, `rimmsqol-en`, `restart-en` green; `reviews-fr` 42 passed, its one failure a missing fixture save, replayed alone and passed). Scenario 08 (a tab moved under another), 09 and 11 (moved tab locked by research, `full-1007`) played green. `translation_fr` and `translation_en` complete (French reviewed by the owner, `FRENCH_REVIEW.md` regenerated from the committed XML).

**Reviews:** `code_review_sha` `6059cb3` (code review of `Source/`, no finding; since the last review only About.xml punctuation, the build/ path in the Directory.Build.props files and the French MoreResults text, owner-confirmed 2026-10-11). `publication_changelog_review_sha` `6059cb3` (confirmed by the owner, 2026-10-11). `echo_review_sha` `f9b2645`: echo kept (owner, 2026-10-10), it still fits the accepted gallery and the subject of the mod has not moved. `social_preview_sha256`: the GitHub social preview is `Mod/About/Preview.png` of 2026-10-05, uploaded by the owner on 2026-10-10 and checked byte-identical.

**Evidence kept:** `settings-replay/` (2026-10-11, four tickets), `full-1007/ (latest report of each scenario, scenarios 08, 09 and 11 included), `gallery-sb/` (feature 17 on SanctuaryBacklot staging, 2026-10-09: the sources of gallery images 1 to 4, identical to the images online) and `gallery-candidate-5/` (image 5), both cited in `PUBLICATION.md`. `full-1006/`, `locked-tab-1008/`, `feature-move-category/`, `full-1001/` and `sb-staging-1007/` deleted on 2026-10-09 (every scenario they proved passes in `full-1007`). History in `docs/runs/`.

**Gallery (`Art/Gallery/`, order as online, 0 to 5 contiguous, 1.7 MB in all):** 0 `0-preview.png` the Preview (byte-identical to `Mod/About/Preview.png`); 1 `1-groups.jpg` the group editor; 2 `2-categories.jpg` the category editor; 3 `3-settings.jpg` the settings page; 4 `4-architect-menu.jpg` the Architect menu with a group open; 5 `5-parent-row.jpg` the appearance window of a tab moved under another (1.0.7, accepted by the owner 2026-10-09). The order is the one of `PUBLICATION.md`.

**ModIcon:** `Mod/About/ModIcon.png` is 128 x 128 and 31 360 bytes, a little above the 20-30 KB guide; the owner validated it as it stands on 2026-10-11.

**Override, illustration source (owner, 2026-10-11):** `Art/Preview-source.png` measures a mean value of 0.260 (`Measure-Palette.sh`, 2 hue families), under the 0.30 of AUDIT 2.e. The owner keeps it as it stands: the Preview is accepted and was uploaded as the GitHub social preview. No regression of `workflow_stage`.

**Dependencies and DLC (2026-10-12, 1.1.0 on `f88c331`):** unchanged. The Unsorted tab references only Core types (`DesignationCategoryDef`, `Designator_Build`, `BuildableDef`): no new mod, no DLC; `About.xml` and `LoadFolders` stand as declared (Harmony required; Better Architect Menu, Architect Icons, Float Sub-Menus optional by reflection).

**Settings audit 1.1.0 (2026-10-12, `settings_audit: partial`):** a third preference, `showUnsorted` (off by default, global to all saves, applied at once: the tab is created or removed without a restart; a restart rebuilds it from the file). Real use: SarahSH asked that no building stay out of reach; an opt-in tab adds a button, hence off by default. Tip text says what it does and that it changes no building. Reached by Options > Mod settings, same page; part of `HasNonDefaultPreferences`, so "Reset everything" is offered and puts it back off (Pickle scenario "the reset puts the preference back"). Persisted as `showUnsorted` in `ExposeData`. Automated: `Validate-Mod.ps1` 1033 checks green, `Run-Behavior.ps1` 37 green, Pickle feature 22 written (five scenarios). In game, English and French: `unverified` until `playTests`.

**Settings audit (2026-10-11, `settings_audit: passed`):** two preferences, `showArchitectButton` (on by default) and `showResearchLocked` (off by default), plus the stored edits of the two editors (groups, categories, labels, icons, colours, parents); the page is reached by Options > Mod settings, the MainButtons shortcut is hidden by default and offered by RIMMSQOL. In `Tests/Pickle/Evidence/full-1007/` the reset of each preference (en and fr), the settings page opened through the shortcut (en and fr), the deleted-group replay (en and fr), the restart chain (en) and the RIMMSQOL shortcut, revealed and hidden again (en), are green. Replay filed on 2026-10-11 and green: the RIMMSQOL shortcut in French (3 scenarios, the settings page read in French on the screenshot), the restart chain in French (2 launches, both passed; no screenshot, the language is the one of the ticket), the tooltip of the Architect button setting in English and in French (the two screenshots read: tooltip text and page in the right language). `passed` is claimed on that evidence.

**Translation audit 1.1.0 (2026-10-12):** three Keyed keys added in both languages (`ArchitectStudio.Settings.ShowUnsorted`, `ArchitectStudio.Settings.ShowUnsortedTip`, `ArchitectStudio.Unsorted.Label`); no hard-coded text (the tab label is read through `.Translate()` when the tab is created). `Validate-Mod.ps1` 1033 checks green, `Check-Translations.ps1` green, `FRENCH_REVIEW.md` regenerated at `f88c331`. The three French strings (« Ajouter un onglet « Non classé » », its tip, « Non classé ») are mine and not reviewed: read and validated by Virginie on 2026-10-12 on `f88c331` (`FRENCH_REVIEW.md` review line), `translation_fr: complete`. In game (raw keys, truncation of the longer tip): `unverified` until `playTests`.

**Translation audit (2026-10-11):** English and French are embedded (`Mod/Languages/English`, `Mod/Languages/French`); `Tests/Validate-Mod.ps1` 1000 checks green, `Check-DefInjected.ps1` 3 keys, 0 errors, `FRENCH_REVIEW.md` regenerated and unchanged. The French was reviewed by the owner (two corrections kept, `9e7c8e2`, 2026-10-08, see above); the review line of `FRENCH_REVIEW.md` is still blank, to be filled by her.

**Closing pass at `published`, 2026-10-09:** mod root clean (`.build/` deleted, regenerated by the builds; `.editorconfig` now at `rimworld/.editorconfig`), `Art/` clean (`Preview-original.png` removed), stale lines corrected, old branches deleted, the dated sections of this file moved to `docs/runs/2026-10-09-status-history.md`.

**Still true of the product:** a Pickle at `v6` takes clicks at the widget, so the 150% scenario holds without the fork (PR 23 closed, step and scenario in RimWorks PR 42); `Mod/desktop.ini` is no longer shipped (untracked 2026-10-01); the settings page lists the three detected integrations, Searchable Menus and Basic Dropdowns have no detection and are named on the Workshop page only.
