# Publication

What the Steam Workshop page asks for and the repository holds nowhere else. It serves twice: for an update, and
for whoever takes the mod over. Workshop item **3792784018**. `Mod/About/PublishedFileId.txt` holds the id and
must never be lost: without it the next upload creates a second item.

## Steam description

The release sends the Workshop description with **every** publication, and it **overwrites what is on the
page**. Its source is the Markdown block below, the only one for the whole mod: the release converts it to Steam
BBCode for the Workshop page and generates the plain-text `<description>` of `Mod/About/About.xml` from it, and
every run checks that `About.xml` says the same. It refuses to run if the block is missing or above the 8000 bytes
Steam accepts once converted. Edit the block, never the Steam page (a hand edit there is lost at the next release)
and never the `<description>` of `About.xml`: run `node .github/scripts/sync-about-description.mjs --write` to
regenerate it. The dry-run prints the whole converted text with its size and SHA-256. There is no
`Mod/README.template.md`; `Mod/.steamignore` still keeps a `README.template.md` or `README.md` out of what ships
to players.

```
Organise the Architect menu from inside the game, without restarting.

# Dropdown groups

- Create a group, put buildings into it, take them out.
- Order the members of a group, by dragging or with up/down arrows.
- Force a category on a whole group: its members are moved there, and any you add later follow. Without this, a group has no category of its own and splits into one button per category its members happen to sit in.
- Grid or list menu, and choice of icon source.
- Delete a group. Groups provided by another mod are dissolved and hidden instead, since their def is recreated on every startup; a button restores them.
- Warns when a group is spread across several categories, where the game silently produces several separate buttons.

# Categories and subcategories

- Create a category, or a subcategory when [Better Architect Menu](https://steamcommunity.com/sharedfiles/filedetails/?id=3563882422) is present.
- Reorder with up/down buttons, among siblings.
- Change the label, colour and icon of any category, vanilla ones included.
- Icon picker browsing every icon already loaded by your active mods.
- Empty categories greyed out, with a building count that includes their subcategories.

Nothing is written to the game's def files, nor to another mod's: everything is stored in the mod settings and reapplied on startup. The mod can be added to or removed from a game in progress.

Embedded English and French translations. Designed to stay usable without a keyboard, with the Steam Deck in mind.

# Works with

Detected automatically, none required.

- [Better Architect Menu](https://steamcommunity.com/sharedfiles/filedetails/?id=3563882422): subcategories, and invalidation of its display caches.
- [Architect Icons](https://steamcommunity.com/sharedfiles/filedetails/?id=1195427067): category icon picking.
- [Float Sub-Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2864015430): nested subcategories in the pick menus.
- [Searchable Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2928608119): adds a search field to those menus by itself.
- [Basic Dropdowns](https://steamcommunity.com/sharedfiles/filedetails/?id=3455529827) (formerly Categories Dropdowns): the groups it adds can be edited, extended or taken apart like any other.

# Also recommended

- [Architect Icons: Improved](https://steamcommunity.com/sharedfiles/filedetails/?id=2879451234), and [Optional Icons for Architect Icons](https://steamcommunity.com/sharedfiles/filedetails/?id=1966995052) — more icons for the picker to offer, since it browses whatever your active mods have loaded.
- [Bradson's Main Button Icons (Forked + Expanded)](https://steamcommunity.com/sharedfiles/filedetails/?id=3532359201) — the same treatment for the bottom bar.
- [Basic Dropdowns - Extended](https://steamcommunity.com/sharedfiles/filedetails/?id=3562304092) — an add-on to Basic Dropdowns with more ready-made dropdown groups, which this mod then lets you edit, extend or take apart.
- [Even More Linkables Dropdown Patch](https://steamcommunity.com/sharedfiles/filedetails/?id=3150535403) — dropdowns for linkable buildings.

# If I go quiet

If I do not answer within a reasonable time after being contacted, anyone may freely update this or any other of my mods, including publishing a continuation of it. All credit must be preserved.

# AI-generated

This mod's code was written with Claude Code (Anthropic) and Codex (OpenAI), and its images generated with DALL-E (OpenAI), under human direction, review and testing. Stated openly: designing with these tools is my job.

# Thanks

- ferny (fernyrepos) for [Better Architect Menu](https://steamcommunity.com/sharedfiles/filedetails/?id=3563882422) and [Colored Categories](https://steamcommunity.com/sharedfiles/filedetails/?id=3323569935), MIT licensed, whose study showed where the right hooks were.
- ferny also for [Basic Dropdowns](https://steamcommunity.com/sharedfiles/filedetails/?id=3455529827), whose ready-made groups Architect Studio lets players edit.
- bymarcin for [Architect Icons](https://steamcommunity.com/sharedfiles/filedetails/?id=1195427067), kathanon for [Float Sub-Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2864015430) and [Searchable Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2928608119).
- Andreas Pardeike for [Harmony](https://steamcommunity.com/sharedfiles/filedetails/?id=2009463077).
- [Pickle](https://steamcommunity.com/sharedfiles/filedetails/?id=3791648678), [RimLogging](https://steamcommunity.com/sharedfiles/filedetails/?id=3733484696), and [PickleTools](https://steamcommunity.com/sharedfiles/filedetails/?id=3806142401) for development-only testing. They are not dependencies of Architect Studio.
- [RIMMSQOL](https://steamcommunity.com/sharedfiles/filedetails/?id=1084452457) for exercising the optional MainButtons customization path during testing.

See ATTRIBUTION.md. This mod is MIT licensed.

[Source code on GitHub](https://github.com/vbardales/Rimworld-Architect-Studio)
```

One known gap: the settings page lists the integrations it **detects** (Better Architect Menu, Architect Icons,
Float Sub-Menus). Searchable Menus and Basic Dropdowns have no detection, so they are named on the page but
not on that screen, and the page does not claim the screen shows them.

## Screenshots, in the order to upload

Steam shows the first one large. The folder `Art/Gallery/` holds only the images to upload, numbered in upload order,
and `0-preview.png` is a byte-for-byte copy of `Mod/About/Preview.png` (uploaded by the owner on 2026-10-02). The others are
English, taken by the Pickle presentation scenario (`Tests/Pickle/Mod/Pickle/Features/17-publication-shots.feature`)
with `wsl-deps.sanctuary.map`: no optional Architect Studio integration is staged, while Nelim's tribe supplies the backdrop. The lists therefore show the game's own groups, not another mod's raw defNames.
Menus and interface windows are shown as what they are, with no staged pawn.

**Upload budget:** JPEGs at 1280×800 framed around the window, each at most **2 MB**, the batch at most **8 MB**.

0. `Art/Gallery/0-preview.png` - the Preview.
1. `Art/Gallery/1-groups.jpg` - **the group editor**: three columns, a group of three buildings selected, its members with their order arrows, and the buildings available to add. The *Category* button reads "— none (members stay where they …", truncated at that width; it is the real interface.
2. `Art/Gallery/2-categories.jpg` - **the category editor**: every category with its icon and building count, the empty ones greyed out, the up/down arrows among siblings.
3. `Art/Gallery/3-settings.jpg` - **the settings page**: the two editors, the two toggles, the keyboard-shortcut hint and the detected integrations.

4. `Art/Gallery/4-architect-menu.jpg` - **the Architect menu**: a group as one button, its dropdown open, the placement tool disarmed.

1 to 4 are crops (window plus 40 px; the menu is the bottom-left of the screen) of the captures in `Tests/Pickle/Evidence/full-1001/gallery-exhibition-5-en/` (ticket `d845`, 2026-10-06), played on Nelim's tribe (`Nelims-tribe`, the exhibition zone, filth cleaned, its animals removed). Owner validated them on 2026-10-06; upload to Steam is the owner's.

## Dependencies and DLC

- **Hard dependency: Harmony only** (`modDependencies`). The code uses `HarmonyLib` and no other third-party
  assembly; every integration is resolved by reflection and the mod works without any of them.
- **Optional, in `loadAfter`** so that they load first when present: Better Architect Menu, Architect Icons,
  Categories Dropdowns (now Basic Dropdowns), Float Sub-Menus, Searchable Menus, plus the base game and the five expansions for order.
- **No expansion is required.** The one branch on an expansion is `ModsConfig.AnomalyActive` in
  `Source/Runtime/Patches/ResearchLockedVisibility.cs`, guarded, for the research-locked option. There is no
  `LoadFolders.xml`. Supported version: 1.6.

## Content boxes

No adult content. The Preview, the ModIcon and the three screenshots above were opened on 2026-09-21: a workbench
with blueprints and storage crates, a cartoon mascot in a hard hat, and three interface windows over a map.

## After an upload

- `Mod/About/PublishedFileId.txt` is unchanged for an update. `git status` must stay clean.
- Steam creates a **new** item private and RimWorld never calls `SetItemVisibility`; this item is already public.
- The release is **manual** (`.github/workflows/publish-tag.yml`, configured by `.github/publish.config.json`): the version is the `version` input of the workflow and must be above every existing tag, so no `feat:` or `fix:` commit is needed. The GitHub release notes are the `## [<version>]` section of `CHANGELOG.md` (dated, written by hand); the Steam change note is the fenced block under `### <version>` of this file, BBCode, sent as written. Both are checked and printed before any tag exists, and the dry-run stops on purpose when either is missing.
- The workflow uploads to Steam first, then creates the tag and the GitHub release: a failed upload leaves no tag. If the upload timed out or its result is unknown, check the item on Steam before any new attempt.
- `publish` takes the full 40-character SHA of a commit whose dry-run passed (`Rimworld-Release-Admin/scripts/dispatch-publish.sh vbardales/Rimworld-Architect-Studio publish-tag.yml <SHA> <version>`); only the owner approves `steam-production`. Rollback target: `v1.0.5` (`ccad168`), the last published version; each good version gets its tag, which is the next target.

## Steam change notes

One fenced block per version, under `### <version>`, BBCode, sent to Steam as written by the release. The
block of the version being published must exist before its dry-run. Start each block with the version heading, as the 1.0.4 note did (`[h2][url=…/compare/vA...vB]B[/url] (date)[/h2]`): without it the Workshop change notes list the entry with no version number.

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

All posted; the register is `WORKSHOP_COMMENTS.md` of the protocols repository (rows Better Architect Menu, Colored Categories, Architect Icons, Float Sub-Menus, Searchable Menus, Harmony, Pickle, RimLogging, RIMMSQOL: `posted`). Missing: Basic Dropdowns (`3455529827`), see `BACKLOG.md`. The drafts that were sent are in `docs/runs/2026-10-02-publication-sent.md`.
