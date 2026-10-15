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




## Gallery

- [ ] **Image 4 (the Architect menu) with a staged scene** (owner, 2026-10-06). The published `Art/Gallery/4-architect-menu.jpg` is validated as it is: the group as one button with its dropdown open, over the exhibition zone, a menu being exempt from the staged-photo rule. For a later publication, a scene where the group makes sense: a group of beds or tables (`Sleeping`, `Tables`) whose members are placed around a named place (house, hut), a colonist on it, the dropdown open in the same frame. Needs the story agreed with Pickle Tools (named place, StageDecor to furnish the subject), then a scenario in `17-publication-shots.feature`.

## Release

- [ ] `Mod/desktop.ini` was shipped by earlier releases (it was tracked until 2026-10-01). The next publication stops sending it.



## 1.2.0: hide a whole Architect tab

- [ ] **Hide an entire category** (request by Bone on the Workshop page, 2026-10-14). Owner choices 2026-10-14: any tab, base game included; the buildings of a hidden tab fall into the Unsorted tab when it is on; a checkbox in the category editor, undone by it or by "Reset everything". Coded on branch `feature/hide-tab`; open points in `STATUS.md` (`remaining`). Reply to Bone only once 1.2.0 is published, saying the feature is in.
