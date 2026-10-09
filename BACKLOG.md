# Backlog

Open work for Architect Studio that is not a defect of the published version. Defects and unverified checks
live in `STATUS.md` (`remaining`).

## Pull requests to the origin repositories

Rule (owner, 2026-09-28, `PUBLISHING.md`, "Départ depuis le projet d'origine"): when an origin repository exists,
a pull request to it is systematic and stays here until it is done. It is independent of the Workshop
publication. A fork and a pull request are public: **nothing goes out without the owner's agreement.**

Origin repositories (also in `STATUS.md`, `upstream_mod_remotes`), checked on 2026-10-01 with `gh`:

| Repository | Licence on GitHub | Last push | Our permission | Pull requests of ours |
| --- | --- | --- | --- | --- |
| [fernyrepos/Better-Architect-Menu](https://github.com/fernyrepos/Better-Architect-Menu) | none declared by GitHub (MIT per the Workshop page and `LICENSE-fernyrepos.txt`) | 2026-09-05 | read | none |
| [fernyrepos/Colored-Categories](https://github.com/fernyrepos/Colored-Categories) | none declared by GitHub | 2026-07-07 | read | none |

Architect Studio studied these two mods and shares no code with them (`ATTRIBUTION.md`: what each one showed). The
history was not forked, and rebuilding on their code is not an option now: 1.0.5 is public. What a pull request can
carry is what we learned from them:

- [x] Owner decision, 2026-10-09: no pull request to Better Architect Menu for now. No defect of theirs is recorded
      (sources, STATUS.md, test reports), so there is nothing to carry. Reopen if testing against it finds one.
- [x] Colored Categories: no pull request, the project is abandoned (owner, 2026-10-09).

## Feature requests

- [x] **Move an existing category under another, as a subcategory** (request by Ali50 on the Better Architect Menu page, 2026-10-02/03): delivered in 1.0.7 (published 2026-10-08). A category that is not ours moves under another from its appearance window, through the parent row; it needs Better Architect Menu, which accepts a foreign category as a child. Proof: scenario 08 and the behavior check green, parent row read in both languages, the research-locked case played on a moved tab (`locked-tab-1008`, scenario 11 passed), the French string `ArchitectStudio.EditCategory.ParentBlocked` confirmed by the owner (2026-10-06), code review of `2b16bdf..2a21877` without finding.
- [ ] Two English improvements to the public description, to go out with the next publication run with `update_description=true` (they change the Workshop page and `About.xml`): `...and any you add later follow.` becomes `...and any buildings added later follow.`; `...where the game silently produces several separate buttons.` becomes `...: the game silently produces several separate buttons.` (after "several categories").

## Gallery

- [ ] **Image 4 (the Architect menu) with a staged scene** (owner, 2026-10-06). The published `Art/Gallery/4-architect-menu.jpg` is validated as it is: the group as one button with its dropdown open, over the exhibition zone, a menu being exempt from the staged-photo rule. For a later publication, a scene where the group makes sense: a group of beds or tables (`Sleeping`, `Tables`) whose members are placed around a named place (house, hut), a colonist on it, the dropdown open in the same frame. Needs the story agreed with Pickle Tools (named place, StageDecor to furnish the subject), then a scenario in `17-publication-shots.feature`.

## Release

- [ ] Send the description to the Workshop page at the next publication: the Markdown source and the workflow are
      already in place (no plugin pull request owed, CI/CD 2026-10-09); the run needs `update_description=true`,
      its text compared with the live page, and the page checked afterwards.
- [x] Basic Dropdowns (ex Categories Dropdowns, `3455529827`) named once and thanked in the description (2026-10-02). The Workshop page changes only with `update_description` or by hand.
- [ ] Add Basic Dropdowns to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, not this repository's to edit): feature 10 exercises it.
- [ ] `Mod/desktop.ini` was shipped by earlier releases (it was tracked until 2026-10-01). The next publication stops sending it.

## Verification

- [ ] Complete the post-publication regression suite of 1.0.7 (played: `minimal-en/fr`, `optionals-en`, `reviews-en`, `rimmsqol-en`): `reviews-fr` (queued replay after the missing fixture save) and `restart-en`.
- [x] French review by the owner of `FRENCH_REVIEW.md`: validated 2026-10-02, `translation_fr: complete`.
