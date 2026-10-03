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

- [ ] Decide with the owner what is worth proposing to ferny: for example the hooks Better Architect Menu exposes
      that Architect Studio invalidates (display caches), or a defect found while testing against it
      (`08-category-create.feature`, `bam-nesting`). Nothing is drafted yet, because no defect of theirs is
      recorded in `STATUS.md`.
- [ ] If something is worth proposing: fork under the owner's account, one branch per proposal, a pull request in
      English, then record its number here and in `STATUS.md`.

## Feature requests

- [ ] **Move an existing category under another, as a subcategory** (request by Ali50 on the Better Architect Menu page, 2026-10-02/03; a reply there pointed to Architect Studio). Wanted: take a top-level tab that mods add for one item or one designator (Natural Paths, Prioritize) and consolidate it into Management, Floors or any other, and move a subcategory between main categories. Today the parent row of the category editor is offered for the categories Architect Studio created only (`Dialog_EditCategory.cs`, `CustomCategoryRuntime.IsCustom`), and it needs Better Architect Menu for subcategories at all. Not a defect: not designed yet. Ali50 asked again on 2026-10-03, narrower: only a main tab with no subcategories, moved under another tab (including another mod's). That is the first step, since only its own parent changes. Open questions: does Better Architect Menu's nesting accept a foreign category as a child, and what happens to its key binding and research-locked state.

## Release

- [ ] Adopt the one-Markdown-source description standard (`## Steam description` of `PUBLICATION.md`,
      `aboutDescription: true`) at the next publication; a new dry-run follows because the SHA changes.
- [x] Basic Dropdowns (ex Categories Dropdowns, `3455529827`) named once and thanked in the description (2026-10-02). The Workshop page changes only with `update_description` or by hand.
- [ ] Add Basic Dropdowns to the comment register (`WORKSHOP_COMMENTS.md`, protocols repository, not this repository's to edit): feature 10 exercises it.
- [ ] `Mod/desktop.ini` was shipped by earlier releases (it was tracked until 2026-10-01). The next publication stops sending it.

## Verification

- [ ] Full suite on the revision that will be published, once the tree is frozen (small tickets, one per pass,
      `-Label` carries the SHA): minimal, optionals, reviews in both languages, restart chain.
- [x] French review by the owner of `FRENCH_REVIEW.md`: validated 2026-10-02, `translation_fr: complete`.
