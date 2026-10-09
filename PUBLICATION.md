# Publication

What the Steam Workshop page asks for and the repository holds nowhere else. It serves twice: for an update, and
for whoever takes the mod over. Workshop item **3792784018**. `Mod/About/PublishedFileId.txt` holds the id and
must never be lost: without it the next upload creates a second item.

## Steam description

The release sends the Workshop description with **every** publication, and it **overwrites what is on the
page**. Its source is [`docs/STEAM_DESCRIPTION.md`](docs/STEAM_DESCRIPTION.md), the only one for the whole mod (the
whole file is the Markdown; it lives outside `Mod/` so it never ships to players): the release converts it to Steam
BBCode for the Workshop page and generates the plain-text `<description>` of `Mod/About/About.xml` from it, and
every run checks that `About.xml` says the same. It refuses to run if the file is missing or above the 8000 bytes
Steam accepts once converted. Edit the file, never the Steam page (a hand edit there is lost at the next release)
and never the `<description>` of `About.xml`: run `node .github/scripts/sync-about-description.mjs --write` to
regenerate it. The dry-run prints the whole converted text with its size and SHA-256. A description-only run does
not exist: the text goes out with the next publication, dry-run with `update_description=true` first, its text read
against the live page. There is no `Mod/README.template.md`; `Mod/.steamignore` still keeps a `README.template.md`
or `README.md` out of what ships to players. (Owner choice, 2026-10-09: a dedicated file rather than the
`## Steam description` heading form; both are supported by the template.)

One known gap: the settings page lists the integrations it **detects** (Better Architect Menu, Architect Icons,
Float Sub-Menus). Searchable Menus and Basic Dropdowns have no detection, so they are named on the page but
not on that screen, and the page does not claim the screen shows them.

## Screenshots, in the order to upload

Steam shows the first one large. The folder `Art/Gallery/` holds only the images to upload, numbered in upload order,
and `0-preview.png` is a byte-for-byte copy of `Mod/About/Preview.png` (the Preview was regenerated on 2026-10-05 with the new ModIcon at the bottom left: the version on the page is the earlier one until the owner uploads this copy). `Mod/About/ModIcon.png` is never resized by hand: `scripts/Render-Preview.cjs bottom-left` generates it, and the two ICOs, from `Art/ModIcon-source.png` (the owner's own file, which only the owner changes) through the key `modIconSource` of `Art/Preview.config.json`. The owner validated keeping the file on 2026-10-06. The others are
English, taken by the Pickle presentation scenario (`Tests/Pickle/Mod/Pickle/Features/17-publication-shots.feature`)
with `wsl-deps.sanctuary.map`: no optional Architect Studio integration is staged, while Nelim's tribe supplies the backdrop. The lists therefore show the game's own groups, not another mod's raw defNames.
Menus and interface windows are shown as what they are, with no staged pawn.

**Upload budget** (owner, 2026-10-06): as many images as wanted, each under **2 MB**, the folder under **8 MB**. Today 0 to 4 weigh about 1.3 MB in all. JPEGs, framed around the window; the three windows are kept at full screen height so that the backdrop shows.

0. `Art/Gallery/0-preview.png` - the Preview.
1. `Art/Gallery/1-groups.jpg` - **the group editor**: three columns, a group of three buildings selected, its members with their order arrows, and the buildings available to add. The *Category* button reads "— none (members stay where they …", truncated at that width; it is the real interface.
2. `Art/Gallery/2-categories.jpg` - **the category editor**: every category with its icon and building count, the empty ones greyed out, the up/down arrows among siblings.
3. `Art/Gallery/3-settings.jpg` - **the settings page**: the two editors, the two toggles, the keyboard-shortcut hint and the detected integrations.

4. `Art/Gallery/4-architect-menu.jpg` - **the Architect menu**: a group as one button, its dropdown open, the placement tool disarmed.

1 to 4 are crops (window plus 40 px; the menu is the bottom-left of the screen) of the captures in `Tests/Pickle/Evidence/full-1001/gallery-backdrop-3-en/` (ticket `5b7d`, 2026-10-06), played on Nelim's tribe (`Nelims-tribe`): the three windows over `window-backdrop-for-height` of the sanctuary, kept at full height so that the two smileys stay visible, and the menu over the exhibition zone. Owner validated them on 2026-10-06; upload to Steam is the owner's.

## Dependencies and DLC

- **Hard dependency: Harmony only** (`modDependencies`). The code uses `HarmonyLib` and no other third-party
  assembly; every integration is resolved by reflection and the mod works without any of them.
- **Optional, in `loadAfter`** so that they load first when present: Better Architect Menu, Architect Icons,
  Categories Dropdowns (now Basic Dropdowns), Float Sub-Menus, Searchable Menus, plus the base game and the five expansions for order.
- **No expansion is required.** The one branch on an expansion is `ModsConfig.AnomalyActive` in
  `Source/Runtime/Patches/ResearchLockedVisibility.cs`, guarded, for the research-locked option. There is no
  `LoadFolders.xml`. Supported version: 1.6.

## Content boxes

No adult content. The Preview, the ModIcon and the screenshots were opened: a workbench with blueprints and storage
crates, a cartoon mascot in a hard hat, and four interface views (three windows and the Architect menu) over
bamboo fields, the exhibition zone and the river of Nelim's tribe, with orange smileys and no pawn (re-read on 2026-10-06 for
the 1.0.6 set).

## After an upload

- `Mod/About/PublishedFileId.txt` is unchanged for an update. `git status` must stay clean.
- Steam creates a new item privately only on an initial upload, and RimWorld never calls `SetItemVisibility`; this existing item is already public.
- The release is **manual** (`.github/workflows/publish-tag.yml`, configured by `.github/publish.config.json`): the version is the `version` input of the workflow and must be above every existing tag, so no `feat:` or `fix:` commit is needed. The GitHub release notes are the `## [<version>]` section of `CHANGELOG.md` (dated, written by hand); the Steam change note is the fenced block under `### <version>` of this file, BBCode, sent as written. Both are checked and printed before any tag exists, and the dry-run stops on purpose when either is missing.
- The workflow uploads to Steam first, then creates the tag and the GitHub release: a failed upload leaves no tag. If the upload timed out or its result is unknown, check the item on Steam before any new attempt.
- `publish` takes the full 40-character SHA of a commit whose dry-run passed (`Rimworld-Release-Admin/scripts/dispatch-publish.sh vbardales/Rimworld-Architect-Studio publish-tag.yml <SHA> <version>`); only the owner approves `steam-production`. Rollback target: `v1.0.6` (`0880367`), the last version published and tested in full (non-regression green on 2026-10-05); each good version gets its tag, which is the next target.

## Steam change notes

One fenced block per version, under `### <version>`, BBCode, sent to Steam as written by the release. The
block of the version being published must exist before its dry-run. Start each block with the version heading, as the 1.0.4 note did (`[h2][url=…/compare/vA...vB]B[/url] (date)[/h2]`): without it the Workshop change notes list the entry with no version number.

### 1.0.7

```
[h2][url=https://github.com/vbardales/Rimworld-Architect-Studio/compare/v1.0.6...v1.0.7]1.0.7[/url] (2026-10-07)[/h2]

[h3]Added[/h3]
[list]
[*]A category added by another mod or by the game can be moved under another one as a subcategory, from its appearance window (needs Better Architect Menu). A category that already has subcategories stays where it is. The choice is saved, reapplied at startup, and "Reset everything" puts the tab back.
[/list]

[h3]Fixed[/h3]
[list]
[*]French interface text reviewed: the categories window intro and the group deletion confirmation.
[/list]
```

### 1.0.6

```
[h2][url=https://github.com/vbardales/Rimworld-Architect-Studio/compare/v1.0.5...v1.0.6]1.0.6[/url] (2026-10-05)[/h2]

[h3]Fixed[/h3]
[list]
[*]Counted phrases read correctly in English and French: "1 building moved" / "2 buildings moved", "1 group deleted", the delete confirmation for one or several buildings, and "0 bâtiment déplacé" in French.
[*]French interface text reviewed: no more orders or "toi", "Catégorie parente" for the parent row, "détecté" / "non détecté" for each integration.
[*]Category editor: the parent label no longer clips in French, and the note shown without Better Architect Menu wraps instead of being cut off.
[/list]

[h3]Changed[/h3]
[list]
[*]Basic Dropdowns (formerly Categories Dropdowns) is named in the description, with thanks to its author.
[*]New icon and Preview, new screenshots.
[/list]
```

## Thanks to post on the mods' pages

The register is `WORKSHOP_COMMENTS.md` of the protocols repository (rows Better Architect Menu, Colored Categories, Architect Icons, Float Sub-Menus, Searchable Menus, Harmony, Pickle, RimLogging, RIMMSQOL: `posted`; PickleTools: `not_applicable`). The drafts that were sent are in `docs/runs/2026-10-02-publication-sent.md`.

**Basic Dropdowns** (ferny, `3455529827`): `posted` by the owner on 2026-10-08; no row in the register yet (protocols repository). Page: https://steamcommunity.com/sharedfiles/filedetails/?id=3455529827

```
Your ready-made groups are what I tested the group editor on: you can open one in [url=https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018]Architect Studio[/url], reorder it, add a building, or take it apart. Thanks for them :)
```

The reply to Ali50 (not a thank-you; to post with the version that carries the feature) is in `docs/runs/2026-10-05-communication.md`.
