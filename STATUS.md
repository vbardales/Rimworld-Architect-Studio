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
workflow_stage: dormant
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
upstream_mod_remotes:
  - https://github.com/fernyrepos/Better-Architect-Menu.git
  - https://github.com/fernyrepos/Colored-Categories.git
dependencies: declared
showcase:     complete
publication_changelog_review_sha: 4729cc811625e283ce1187b08319946daa415701  # reviewed (confirmed 2026-10-10) on this commit, PUBLICATION.md and CHANGELOG.md
code_review_sha: d01891d7743a7d59ce6808474c2a47adaea9b89f
echo_review_sha: f9b264500faf675d635429ff65330f425f8b7244  # echo validated by the owner on 2026-10-10 (keep): it still fits the accepted gallery, the subject of the mod has not moved
social_preview_sha256: 6d36e22e6a97b34d75f501c334b5b5ef2b8a0eb35c4e5696454e603f299f1805  # uploaded by the owner on 2026-10-10; the image served by GitHub is byte-identical to Mod/About/Preview.png
tested_on:    2026-10-09
workshop:     3792784018
remaining:
  - external: the Workshop page still shows the pre-1.0.6 description (1.0.7 did not send it: `update_description` is off by default). It goes out with the next publication: dry-run with `update_description=true` on the new SHA, its text read against the live page, `dispatch-publish.sh ... --description`, the owner approves `steam-production`, the page checked afterwards (`update_description` is not yet proven against real steamcmd). The two English description edits (next entry) wait for it
  - feature: two English edits to the public description, to go out with the next publication run with `update_description=true` (they change the Workshop page and `About.xml`; owner agreement implied by the next-publication rule, wording from the review of 2026-10-09): `...and any you add later follow.` becomes `...and any buildings added later follow.`; `...where the game silently produces several separate buttons.` becomes `...: the game silently produces several separate buttons.` (the full stop of the preceding clause replaced by the colon); detail in BACKLOG.md
  - closed on 2026-10-09: Basic Dropdowns row added to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, `30d160b`); gallery image `5-parent-row.jpg` accepted and uploaded by the owner; delete confirmation of a group the player made captured in French (`full-1007/delete-confirm-fr`), wording as reviewed. Moves to `docs/runs/` at the next cleanup
session:      local_4a8e2ff6-9358-4fba-9a31-f91aae222ed1
updated:      2026-10-10, workflow_stage `dormant` (1.0.7 published 2026-10-09; non-regression, thanks comments, cleanup and branches checked against AUDIT 14.a to 14.d); closing pass and cleanup at `published` done (STATUS.md, `Art/`, mod root, evidence); gallery image 5 accepted and uploaded by the owner, images 1 to 4 replayed on SanctuaryBacklot staging (identical); delete-confirmation capture read (French)
protocols_read_sha: 6591dbc87e6e41defa0dcf1fc510aac5ca522048
---

# Architect Studio — status

## State

**Published: 1.0.7, 2026-10-08** (tag and release `v1.0.7`, SHA `5869dc26632c43336915c469602c4c355253688a`, dry-run 37799121470, publish 37799288436 approved by the owner). Workshop item `3792784018`. Rollback target for the next publication: `v1.0.7`. Previous: `v1.0.6` (2026-10-05), `v1.0.5` (2026-09-25).

**What 1.0.7 carries:** a category that is not ours moves under another from its appearance window (parent row, needs Better Architect Menu; request by Ali50); two French corrections kept by the owner (`ArchitectStudio.Categories.Intro`, `ArchitectStudio.Dropdowns.ConfirmDelete`, `9e7c8e2`; the other three suggestions of the reviewer were not kept; `.Zero` forms accepted); the gallery folder of the publication config points to `Art/Gallery`.

**Policy:** fail fast (owner, 2026-09-25): the deploy does not wait for the non-regression, which runs right after it.

**Gates, on the published revision:** `Tests/Validate-Mod.ps1` 1000 checks; `Tests/Run-Behavior.ps1` 35 assertions; Pickle non-regression after the deploy complete on 2026-10-09 in `Tests/Pickle/Evidence/full-1007/` (`minimal-en/fr`, `optionals-en`, `reviews-en`, `rimmsqol-en`, `restart-en` green; `reviews-fr` 42 passed, its one failure a missing fixture save, replayed alone and passed). Scenario 08 (a tab moved under another), 09 and 11 (moved tab locked by research, `full-1007`) played green. `translation_fr` and `translation_en` complete (French reviewed by the owner, `FRENCH_REVIEW.md` regenerated from the committed XML).

**Reviews:** `code_review_sha` `d01891d` (code review of `Source/`, no finding; `Source/` unchanged since, `Mod/` only About.xml punctuation, owner-confirmed 2026-10-10). `publication_changelog_review_sha` `4729cc8` (confirmed by the owner, 2026-10-10, after the em dash fix). `echo_review_sha` `f9b2645`: echo kept (owner, 2026-10-10), it still fits the accepted gallery and the subject of the mod has not moved. `social_preview_sha256`: the GitHub social preview is `Mod/About/Preview.png` of 2026-10-05, uploaded by the owner on 2026-10-10 and checked byte-identical.

**Evidence kept:** `full-1007/` (latest report of each scenario, scenarios 08, 09 and 11 included), `gallery-sb/` (feature 17 on SanctuaryBacklot staging, 2026-10-09: the sources of gallery images 1 to 4, identical to the images online) and `gallery-candidate-5/` (image 5), both cited in `PUBLICATION.md`. `full-1006/`, `locked-tab-1008/`, `feature-move-category/`, `full-1001/` and `sb-staging-1007/` deleted on 2026-10-09 (every scenario they proved passes in `full-1007`). History in `docs/runs/`.

**Closing pass at `published`, 2026-10-09:** mod root clean (`.build/` deleted, regenerated by the builds; `.editorconfig` now at `rimworld/.editorconfig`), `Art/` clean (`Preview-original.png` removed), stale lines corrected, old branches deleted, the dated sections of this file moved to `docs/runs/2026-10-09-status-history.md`.

**Still true of the product:** a Pickle at `v6` takes clicks at the widget, so the 150% scenario holds without the fork (PR 23 closed, step and scenario in RimWorks PR 42); `Mod/desktop.ini` is no longer shipped (untracked 2026-10-01); the settings page lists the three detected integrations, Searchable Menus and Basic Dropdowns have no detection and are named on the Workshop page only.
