# Architect Studio

A RimWorld 1.6 mod that organises the Architect menu from inside the game, without restarting.

- **Dropdown groups** — create a group, put buildings into it, order its members, and force a
  category on the whole group so its members follow.
- **Categories and subcategories** — create them, reorder them, and change their label, colour
  and icon, vanilla ones included.

Nothing is written to the game's def files, nor to another mod's: everything is stored in the mod
settings and reapplied on startup. The mod can be added to or removed from a game in progress.

Designed to stay usable without a keyboard, with the Steam Deck in mind: every action is reachable
with a pointer, and no feature is keyboard-only.

## Works with

None is required — the mod works on its own, and the settings screen shows which ones were
detected. All are resolved by reflection; no third-party assembly is referenced at build time.

| Mod | What it adds |
|---|---|
| [Better Architect Menu](https://steamcommunity.com/sharedfiles/filedetails/?id=3563882422) | Subcategories, and invalidation of its display caches |
| [Architect Icons](https://steamcommunity.com/sharedfiles/filedetails/?id=1195427067) | Category icon picking |
| [Float Sub-Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2864015430) | Nested subcategories in the pick menus |
| [Searchable Menus](https://steamcommunity.com/sharedfiles/filedetails/?id=2928608119) | Adds a search field to those menus by itself |

## Also recommended

Not integrated with, but they pair well — the icon packs feed the picker, and the dropdown packs
give you groups worth editing.

| Mod | Why |
|---|---|
| [Architect Icons: Improved](https://steamcommunity.com/sharedfiles/filedetails/?id=2879451234) | More icons for the picker to offer |
| [Optional Icons for Architect Icons](https://steamcommunity.com/sharedfiles/filedetails/?id=1966995052) | More again |
| [Bradson's Main Button Icons (Forked + Expanded)](https://steamcommunity.com/sharedfiles/filedetails/?id=3532359201) | The same treatment for the bottom bar |
| [Basic Dropdowns](https://steamcommunity.com/sharedfiles/filedetails/?id=3455529827) | ~100 ready-made dropdown groups, which this mod lets you edit |
| [Basic Dropdowns - Extended](https://steamcommunity.com/sharedfiles/filedetails/?id=3562304092) | More of them |
| [Even More Linkables Dropdown Patch](https://steamcommunity.com/sharedfiles/filedetails/?id=3150535403) | Dropdowns for linkable buildings |

## Building

Requires the .NET SDK. Reference assemblies come from NuGet, so a RimWorld installation is not
needed to compile.

```bash
dotnet build Source/ArchitectStudio.csproj -c Release
```

The assembly is written straight into `Mod/Assemblies/`.

For a quick iteration loop, make `RimWorld/Mods/ArchitectStudio` an NTFS junction pointing at
**`Mod/`** — not at the repository root: a rebuild then lands in the game with nothing to copy.

## Layout

Only `Mod/` is published. RimWorld's uploader hands Steam the mod directory as-is
(`SteamUGC.SetItemContent` on `ModMetaData.RootDir`, with no filtering whatsoever), so anything
sitting in that folder is downloaded by every subscriber. Sources and build intermediates
therefore live outside it.

```
Mod/            <- the published mod, and the junction target
  About/          metadata, Workshop preview and mod icon
  Assemblies/     the built assembly
  Defs/           key binding definition
  Languages/      English and French
  LICENSE         MIT requires the notice to travel with the distribution
Source/         C# sources, never published
.build/         build intermediates, git-ignored, never published
```

## Licence

MIT — see [LICENSE](LICENSE).

This mod studies, but does not copy, several MIT-licensed mods; what was learned from each is
detailed in [ATTRIBUTION.md](ATTRIBUTION.md).

Its code was written with Claude Code (Anthropic) and its images generated with DALL·E (OpenAI),
under human direction, review and testing.
