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
stage:        published[1.0.7]
workflow_stage: published[1.0.7]
licence:      original
licence_at:   LICENSE (MIT, copyright 2026 Nelim); LICENSE-fernyrepos.txt (MIT, copyright 2025 fernyrepos)
upstream_mod_remotes:
  - https://github.com/fernyrepos/Better-Architect-Menu.git
  - https://github.com/fernyrepos/Colored-Categories.git
dependencies: declared
showcase:     complete
publication_changelog_review_sha: d872414864755165c000a1cc83aa038e80619c0c  # was `publication_changelog_review`: reviewed (confirmed 2026-10-09) on <sha>, PUBLICATION.md and CHANGELOG.md; the BACKLOG.md reliquat noted in the same review is fixed in bd7c48b
code_review_sha: 2a21877af903eb97978d23f40664cb3f00c1b37a
tested_on:    2026-09-21
workshop:     3792784018
remaining:
  - external: the Workshop page still shows the pre-1.0.6 description (1.0.7 did not send it: `update_description` is off by default). It goes out with the next publication: dry-run with `update_description=true` on the new SHA, its text read against the live page, `dispatch-publish.sh ... --description`, the owner approves `steam-production`, the page checked afterwards (`update_description` is not yet proven against real steamcmd). The two English description edits (next entry) wait for it
  - unverified: Basic Dropdowns (ex Categories Dropdowns, `3455529827`) has no row in the comment register (`WORKSHOP_COMMENTS.md`, protocols repository); the owner posted the thanks on 2026-10-08
  - feature: two English edits to the public description, to go out with the next publication run with `update_description=true` (they change the Workshop page and `About.xml`; owner agreement implied by the next-publication rule, wording from the review of 2026-10-09): `...and any you add later follow.` becomes `...and any buildings added later follow.`; `...where the game silently produces several separate buttons.` becomes `...: the game silently produces several separate buttons.` (the full stop of the preceding clause replaced by the colon); detail in BACKLOG.md
  - closed on 2026-10-09: gallery image `5-parent-row.jpg` accepted and uploaded by the owner; delete confirmation of a group the player made captured in French (`full-1007/delete-confirm-fr`), wording as reviewed. Moves to `docs/runs/` at the next cleanup
session:      local_4a8e2ff6-9358-4fba-9a31-f91aae222ed1
updated:      2026-10-09, 1.0.7 published (stage `published[1.0.7]`); closing pass and cleanup at `published` done (STATUS.md, `Art/`, mod root, evidence); gallery image 5 accepted and uploaded by the owner, images 1 to 4 replayed on SanctuaryBacklot staging (identical); delete-confirmation capture read (French)
---

# Architect Studio — status

## State

**Published: 1.0.7, 2026-10-08** (tag and release `v1.0.7`, SHA `5869dc26632c43336915c469602c4c355253688a`, dry-run 37799121470, publish 37799288436 approved by the owner). Workshop item `3792784018`. Rollback target for the next publication: `v1.0.7`. Previous: `v1.0.6` (2026-10-05), `v1.0.5` (2026-09-25).

**What 1.0.7 carries:** a category that is not ours moves under another from its appearance window (parent row, needs Better Architect Menu; request by Ali50); two French corrections kept by the owner (`ArchitectStudio.Categories.Intro`, `ArchitectStudio.Dropdowns.ConfirmDelete`, `9e7c8e2`; the other three suggestions of the reviewer were not kept; `.Zero` forms accepted); the gallery folder of the publication config points to `Art/Gallery`.

**Policy:** fail fast (owner, 2026-09-25): the deploy does not wait for the non-regression, which runs right after it.

**Gates, on the published revision:** `Tests/Validate-Mod.ps1` 1000 checks; `Tests/Run-Behavior.ps1` 35 assertions; Pickle non-regression after the deploy complete on 2026-10-09 in `Tests/Pickle/Evidence/full-1007/` (`minimal-en/fr`, `optionals-en`, `reviews-en`, `rimmsqol-en`, `restart-en` green; `reviews-fr` 42 passed, its one failure a missing fixture save, replayed alone and passed). Scenario 08 (a tab moved under another), 09 and 11 (moved tab locked by research, `full-1007`) played green. `translation_fr` and `translation_en` complete (French reviewed by the owner, `FRENCH_REVIEW.md` regenerated from the committed XML).

**Reviews:** `code_review_sha` `2a21877` (code review of `Source/`, no finding; `Source/` and `Mod/` unchanged since). `publication_changelog_review` on `d872414`.

**Evidence kept:** `full-1007/` (latest report of each scenario, scenarios 08, 09 and 11 included), `gallery-sb/` (feature 17 on SanctuaryBacklot staging, 2026-10-09: the sources of gallery images 1 to 4, identical to the images online) and `gallery-candidate-5/` (image 5), both cited in `PUBLICATION.md`. `full-1006/`, `locked-tab-1008/`, `feature-move-category/`, `full-1001/` and `sb-staging-1007/` deleted on 2026-10-09 (every scenario they proved passes in `full-1007`). History in `docs/runs/`.

**Closing pass at `published`, 2026-10-09:** mod root clean (`.build/` deleted, regenerated by the builds; `.editorconfig` now at `rimworld/.editorconfig`), `Art/` clean (`Preview-original.png` removed), stale lines corrected, old branches deleted, the dated sections of this file moved to `docs/runs/2026-10-09-status-history.md`.

**Still true of the product:** a Pickle at `v6` takes clicks at the widget, so the 150% scenario holds without the fork (PR 23 closed, step and scenario in RimWorks PR 42); `Mod/desktop.ini` is no longer shipped (untracked 2026-10-01); the settings page lists the three detected integrations, Searchable Menus and Basic Dropdowns have no detection and are named on the Workshop page only.
