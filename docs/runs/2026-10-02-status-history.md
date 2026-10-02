# Status history, trimmed on 2026-10-02

Dated sections of `STATUS.md` and closed `remaining` entries, one line each. The full text is in git (`git log -p STATUS.md`; the last full version is the commit before the trim). Kept in `STATUS.md`: the open `remaining` entries, the 2026-10-01 audit and the 2026-10-02 Preview source migration.

## Sections

- Translation audit addendum — 2026-09-28: Checked revision: `3f0d844` plus the docs commits after it (no Mod/ change after `899820d`).
- Pass matrix after 1.0.4, and French wording — 2026-09-25: **1.0.5 published, 2026-09-25, fail fast.** Documented release, exact commit `ccad16850a8c3f6213ba785947edd9c4abcfb197` (`ccad168`): dry-run run 36167737673 (green, version 1.0.5 computed, description 5296 bytes sha256 `418d2463…de58`, change note read from PUBLICATION.md, nothing sent to Steam), pu
- French review corrections — 2026-09-22: During owner review of the French `16-language-review.feature` media, two wording changes were made: the no-selection hint no longer inserts a comma before “ou”, and the category editor now explains that arrows change display order (its second sentence was reworded again on 2026-09-25, see above).
- Audit — 2026-09-22: Audited revision: `46124c334c778382eb19d0bd1e7b1d4bd304a01b`.
- Pickle coverage and later-gate review — 2026-09-22: The Pickle suite was reviewed against every behavior in `TESTING.md`, the shared PickleTools catalogue and the actual pass maps.
- Targeted InterfaceScale pass — 2026-09-22: `Tests/Pickle/wsl-deps.avec-pickletools.map` is a valid narrow alternative to the broader review pass: it stages only `nelim.pickletools.interfacescale`, allowing `16-language-review.feature` to exercise its 150% click against the stock Workshop Pickle without a local Pickle fork.
- Publication preparation — 2026-09-22: The distributed `About.xml` description and the hand-edit source in `PUBLICATION.md` now agree on direct Workshop links for every named mod that has an item, including Colored Categories (`3323569935`).
- Publication copy refinement — 2026-09-22: The local Steam-format source and distributed description were refined without changing the Workshop item: Steam must still be edited by hand from `PUBLICATION.md`.
- Workshop page actions — 2026-09-22: The mod owner reports that the existing Workshop description was manually updated from `PUBLICATION.md`, the current ordered screenshots were uploaded, the release notes were entered, and the prepared thank-you comments were posted on their recipients' pages.
- RIMMSQOL integration and retired restart chain — 2026-09-22: Feature `19-rimmsqol-shortcut.feature` passed **3/3** on the staged RIMMSQOL pass.
- Studio Workshop captures — 2026-09-22: The dedicated Pickle presentation pass ran after the Workshop shots were moved onto `nelim-zen-meadow-studio`: **3/3 passed**, `0` failed, `exitReason: passed`, set `studio` (archive `0922-1655`).
- 1.0.3 — checked and prepared, 2026-09-21: 1.0.3 carries four fixes a player can see: a deleted group belonging to another mod no longer comes back at the next start; *Restore deleted groups* now restores; the introduction line of both editors is no longer clipped when it wraps; and *Reset everything* restores the preferences as well and is 
- The last red scenario, and it was never ours — 2026-09-20: *screenshots of both editors at 150 percent* had been failing since the suite first ran, and this file said above that the cause lay in our own interface-scale step.
- Six scenarios moved down to unit tests — 2026-09-20: Six Pickle scenarios asserted stored state and nothing else — no click, no drawing, no reload — so they confiscated a game session at every run to read fields a headless process reads in milliseconds.
- First full in-game run — 2026-09-20: The Pickle suite ran inside a real game for the first time: **40 Architect Studio scenarios, 39 passed, 1 failed**, the failure being a click at 150% interface scale.
- Settings completion — 2026-09-13: Current result: **preOptions -> done**, ready for final in-game validation, not `tested`.
- ModIcon user override — 2026-09-13: The user explicitly overrode the ModIcon style finding ("j'override pour ModIcon").
- Ordered workflow audit — 2026-09-13: This section supersedes historical status conclusions below, without deleting their evidence.
- Translation audit — 2026-09-13: - Scope: working tree based on `3441b9d8ef0c3fbfea7c828f930f8508d1fafe86`, with the translation changes described here.
- Preview recomposition — 2026-09-12: - Final delivered image: `Mod/About/Preview.png`, 896 x 504, 612,584 bytes (under 900 KB).
- Verification — 2026-09-12: - **Title:** keep `Architect Studio`, with no `Continued` or `Fork` suffix.

## Closed remaining entries (verified or found)

- verified: counted phrases (`.Zero`, `.One`, `.Many` families of `OverrideCount`, `HiddenCount`, `ConfirmDissolve`, `MoreResults`, rewritten on 2026-09-25; `CountedText.cs`, four call sites, validator requires every form and refuses a `(s)` suffix) read correctly in both languages, feature 18 on `11e
- found: `ConfirmDissolve.Zero` cannot be shown by the game as written, not merely hard to test: `Dialog_DropdownGroups.DeleteConfirmationText` routes an empty group to the separate key `ArchitectStudio.Dropdowns.ConfirmDissolveEmpty` instead, so the `.Zero` form of `ConfirmDissolve` is dead code that
- verified: added an eighth scenario to feature 18 ("the footer after two groups deleted", a new step `I dissolve a second foreign group besides` that picks any other foreign group with members instead of naming a vanilla one) to reach `HiddenCount.Many`; passed on `0a9f` (`fddea0d`, French, 8/9 of th
- found: my own mistake, not the suite's — the three tickets after this one (`9d4d`, `9350`, `7e0c`, run 2026-09-27) all came back green with every scenario skipped: I resubmitted feature 18 without `-DepMap wsl-deps.avec-infobulles.map`, so Nelim's Pickle Tools was never staged and the `@requires` on
- verified: the two tooltips (the "Button in the Architect menu" setting and the Groups… button), the forced-category tooltip and "… et 175 autres. La recherche permet d'affiner." are now read on the French capture of `11e0874` (`matrix-current/counted-French`, which supersedes the deleted `matrix-11e
- found: the ninth scenario (the "Groupe personnalisé" / "Custom" tooltip) failed on `0a9f`: `Nelim's Pickle Tools: I hover over the tooltip keyed "..."` matches a tag exactly, and the real tag is `tip:AS_Group_1\nGroupe personnalisé` — the defName and a newline first, never the bare translated line a
- found: that fix made the scenario pass on `2cc6` (no exception), but the screenshot it took (`manual--tooltip-of-a-custom-group--step0.png`) does not show the Custom tooltip: it shows "Ouvrir l'éditeur de groupes de menus déroulants." (`ArchitectButtonTip`, from clicking the Architect Studio button 
- verified: the owner asked to favour the infinitive form for a mod description or tooltip read as a bare verb, ambiguous between an elliptic indicative and a tutoyed imperative. Fixed on this rule: `ArchitectStudio.Dropdowns.GroupCategoryTip` ("Impose une catégorie…" → "Imposer une catégorie…") and `
- verified: TESTING.md 15.4 and 15.5 are automated on `68fe26f` (2026-09-25): the two reset scenarios of `14-reset.feature` passed (3/3 with the existing one), and the very long accented name scenario passed in English and French with its captures read. The owner validated the long-name capture on 202
- verified: `migrate-manual` merged (PR 6, 5c9f83b, main now runs `publish-tag.yml`, `release.yml` and the semantic-release files gone). The 1.0.6-beta.1 texts merged next (PR 7, `1b4f1a0`: `## [1.0.6-beta.1]` in `CHANGELOG.md`, `### 1.0.6-beta.1` in `PUBLICATION.md`). CI/CD ran the dry-run of `1b4f1a
- verified: the RIMMSQOL integration pass (`19`) passed 3/3 on 2026-09-22; the owner reviewed its list, revealed-button edit page and Architect Studio settings capture as correct. The former RIMMSQOL persistence chain was retired from this suite as redundant dependency coverage
- verified: the 150% interface-scale screenshots of both editors, read by the mod's owner on 2026-09-20 and found clean — no clipping, no raw keys. English only, so the French half stays under the translation-gate line above
- verified: two passes on the shipped build, 2026-09-21, English, headless — without the optional mods (11 mods loaded, 43 scenarios of 18 features, 40 passed, 1 failed, 2 skipped) and with the five optional mods (16 mods loaded, 43 scenarios, 41 passed, 1 failed, 1 skipped). The one failure is the 15
- verified: scenario 13, run alone on the minimal mod list on 2026-09-21, passes — the editors open, a group is created and the menu shows it, and nothing from the mod is logged with Better Architect Menu, Architect Icons and Float Sub-Menus absent
- verified: the Architect Icons scenario passes against the real Architect Icons (Workshop 1195427067) on 2026-09-21
