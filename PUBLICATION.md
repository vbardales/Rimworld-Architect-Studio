# Publication

What the Steam Workshop page asks for and the repository holds nowhere else. It serves twice: for an update, and
for whoever takes the mod over. Workshop item **3792784018**. `Mod/About/PublishedFileId.txt` holds the id and
must never be lost: without it the next upload creates a second item.

## The description is sent by the release

The release sends the Workshop description with **every** publication, and it **overwrites what is on the
page**. Its source is `Mod/README.template.md`, in Markdown; `semantic-release-steam` converts it to Steam
BBCode (`##` becomes `[h2]`, lists `[list][*]`, links `[url=]`). Edit that file, never the Steam page: a hand
edit there is lost at the next release. `Mod/.steamignore` keeps the template and its compiled twin
`README.md` out of what ships to players, and the release refuses to run if either the template is missing
or the ignore file does not list them.

`About.xml` keeps the plain-text form of the same content. RimWorld sends it only when it creates an item, so
it is what a player sees in the mod list, not what the Workshop page shows: keep the two in step by hand.

One known gap: the settings page lists the integrations it **detects** (Better Architect Menu, Architect Icons,
Float Sub-Menus). Searchable Menus and Categories Dropdowns have no detection, so they are named on the page but
not on that screen, and the page does not claim the screen shows them.

## Screenshots, in the order to upload

Steam shows the first one large: it is the most demonstrative that goes there, not the prettiest. All three are
English, taken by the Pickle presentation scenario (`Tests/Pickle/Mod/Pickle/Features/17-publication-shots.feature`)
with `wsl-deps.studio.map`: no optional Architect Studio integration is staged, while Nelim's dedicated
zen-meadow screenshot colony supplies the intentional map backdrop. This keeps the lists to the game's
own groups rather than another mod's raw defNames. Replace the currently uploaded captures after the
scenario has been replayed and its new media reviewed.

**Upload budget:** use the listed JPEGs at 1280×800, tightly framed around the actual editor window. Each file must be at most **2 MB**, and the three-file
Workshop batch at most **8 MB**. Re-check both limits after regenerating or recompressing a capture.

1. `Art/steam/03-groups.jpg` - **the group editor**, the mod's reason to exist and the first section of the
   description: three columns, a group of three buildings selected, its members with their order arrows, and the
   buildings available to add. One thing in it is left as it is: the *Category* button reads "— none (members
   stay where they …", truncated at that width; it is the real interface.
2. `Art/steam/02-categories.jpg` - **the category editor**: every category with its icon and building count, the
   empty ones greyed out, the up/down arrows among siblings.
3. `Art/steam/01-settings.jpg` - **the settings page**: the two editors, the two toggles, the keyboard-shortcut
   hint and the detected integrations.

## Dependencies and DLC

- **Hard dependency: Harmony only** (`modDependencies`). The code uses `HarmonyLib` and no other third-party
  assembly; every integration is resolved by reflection and the mod works without any of them.
- **Optional, in `loadAfter`** so that they load first when present: Better Architect Menu, Architect Icons,
  Categories Dropdowns, Float Sub-Menus, Searchable Menus, plus the base game and the five expansions for order.
- **No expansion is required.** The one branch on an expansion is `ModsConfig.AnomalyActive` in
  `Source/Runtime/Patches/ResearchLockedVisibility.cs`, guarded, for the research-locked option. There is no
  `LoadFolders.xml`. Supported version: 1.6.

## Content boxes

No adult content. The Preview, the ModIcon and the three screenshots above were opened on 2026-09-21: a workbench
with blueprints and storage crates, a cartoon mascot in a hard hat, and three interface windows over a map.

## After an upload

- `Mod/About/PublishedFileId.txt` is unchanged for an update. `git status` must stay clean.
- Steam creates a **new** item private and RimWorld never calls `SetItemVisibility`; this item is already public.
- The Steam change note is the release notes `semantic-release` generates from the commit messages, converted to BBCode by the release. A commit type that triggers no release (`docs`, `test`, `ci`, `chore`) triggers no upload.
- The tag and the GitHub release are created by the same run, before the Steam upload: `verifyConditions` refuses to start when the description source is missing, precisely so that a tag never exists without its upload. `CHANGELOG.md` is written by hand and by nobody else: no plugin feeds it.

## Thanks to post on the mods' pages

One per page, in the mod's own comments, once they can see the link. Pasting the bare URL of this item gives a
thumbnail: `https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018`. Each is under 1000 characters,
the limit of a Steam comment.

**Better Architect Menu** (ferny)

```
Thank you for Better Architect Menu, and for publishing it under MIT! 🥰 Reading how it hooks the Architect window
is what showed me where the right places were for Architect Studio, a mod that lets players regroup and reorder
that same menu from inside the game. No code was copied, and your licence is reproduced in the repository.
Architect Studio talks to yours by reflection when it is loaded: subcategories, and clearing your display caches
when a player changes something. It works without it, and better with it.
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Colored Categories** (ferny)

```
Colored Categories showed me where a category button gets its tint. 🎨😊 Architect Studio lets players recolour any
category, and does it with a plain Harmony prefix at the same spot; nothing was copied. Thank you for the MIT
licence and for code readable enough to learn from.
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Architect Icons** (bymarcin)

```
Architect Studio lets players pick the icon of any Architect category, and when Architect Icons is loaded it asks
yours for the icon straight away, so the choice shows without a restart. Everything is resolved at runtime and
nothing depends on it. Thank you for making it possible to give the Architect menu icons at all! 🤩
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Float Sub-Menus** (kathanon)

```
When Float Sub-Menus is loaded, the pick menus in Architect Studio get nested subcategories. It works by
reflection and the mod is fine without it, but it is better with it. Thank you for it! 😊
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Searchable Menus** (kathanon)

```
Searchable Menus adds a search field to the pick menus in Architect Studio with nothing done on my side, which
is how such a mod should work. Thank you for improving other mods' menus without asking them for anything! 😄
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Harmony** (Andreas Pardeike)

```
Architect Studio reorganises the Architect menu through Harmony patches, and could not exist without them.
Thank you for the library and for keeping it working across versions. 💛
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**Pickle**

```
[b]Thank you, Pickle![/b] 🥒😸
Your game-driven scenarios and review captures took Architect Studio through its editors, ordering,
translations and optional integrations in the actual RimWorld UI — proper player-facing testing, not just
crossed fingers. Pickle is development-only, never a dependency, but it gave the test suite a lovely,
reproducible route through the features players use. ✨
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**RimLogging**

```
[b]Thank you, RimLogging![/b] 🤓
You make Architect Studio's automated runs readable: every game-driven scenario gets logs to inspect, instead
of a green result being treated as magic proof. That is wonderfully reassuring when a mod has lots of little UI
paths to explore. RimLogging is development-only and never a dependency — just an excellent testing companion!
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```

**RIMMSQOL**

```
[b]Thank you, RIMMSQOL![/b] 🥳
Architect Studio's optional hidden MainButtons path gets a real reveal, hide and restart-persistence workout
through your customization workflow. The mod runs happily without RIMMSQOL, so this is development-only — but
it is such a neat way to make sure that optional path behaves exactly as it should. 🛠️
https://steamcommunity.com/sharedfiles/filedetails/?id=3792784018
```
