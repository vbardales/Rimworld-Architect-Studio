# In-game scenarios, run by Pickle

The scenarios of [TESTING.md](../../TESTING.md), written in Gherkin and played inside a running
RimWorld by [Pickle](https://github.com/RimWorks/Rimworld-Pickle) (`rimworks.pickle`,
Workshop 3791648678).

`Mod/` is a companion mod, **Architect Studio - Pickle tests**, never published. It holds the
feature files and the step assembly, so nothing test-related ships in the Workshop folder.

## Setup, once

1. Subscribe to Pickle and RimLogging, and enable both.
2. Link the companion mod into RimWorld's `Mods` folder. A junction needs no elevation:

   ```powershell
   New-Item -ItemType Junction -Path "C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\ArchitectStudioPickleTests" -Target "<repo>\Tests\Pickle\Mod"
   ```

3. Enable it below Architect Studio and Pickle.

## Build

```powershell
dotnet build Tests/Pickle/Source/ArchitectStudio.PickleSteps.csproj -c Release
```

The output goes to `Mod/Pickle/Assemblies/`. It binds to the shipped `Mod/Assemblies/ArchitectStudio.dll`,
so build the mod first. Feature files need no build.

## Run

- **In game**: dev mode on, debug actions menu, *Pickle*. Tick the Architect Studio suite, *Run selected*.
  *Break on failure* pauses on the broken state.
- **Unattended**: `RimWorldWin64.exe "-pickle-run=Architect Studio - Pickle tests"`. The filter is
  the companion mod's name, exactly; without it Pickle also runs its own 27 sample features. Reports
  land in `PickleReports` beside the saves: `report.html`, `junit.xml`, `summary.md`.

Scenario 02 clicks real buttons through OS input: the pointer moves on its own while it runs.

## What the suite does to your settings

Every editor action writes Architect Studio's settings file at once. So before each scenario the
suite copies that file to `Mod_ArchitectStudio_ArchitectStudioMod.xml.pickle-backup`, resets the
mod, and after the scenario restores the file and replays it. If the game dies mid-scenario the
backup stays behind, and the next scenario restores it first. **If a `.pickle-backup` file is ever
left in `Config/` and you do not intend to run the suite again, copy it over the settings file by
hand.**

## How the scenarios reach the mod

The editors keep their mutations in private methods (`CreateGroup`, `Assign`, `ReorderMember`,
`SetGroupCategory`, `DeleteGroup`, `DissolveGroup`, `RestoreHiddenGroups`). The steps call those
by name, so they run the code the buttons run. A renamed method fails the step with its name.

A drag is replayed through the callback the member column registers with `ReorderableWidget` on
its last repaint, captured by a Harmony postfix. The insertion-index conversion tested is the
mod's own lambda.

## What stays manual

| TESTING.md | Why |
| --- | --- |
| 3, the pointer starting a drag | A physical drag is not replayed, only the drop |
| 4, arrows at 150% | `04b` attaches two screenshots, tagged `@review`; a person looks |
| 10 and 12, a real restart | One process cannot rebuild the def database; the replay of the settings file is tested |
| 13, without optional mods | Needs its own mod list; tagged `@wip` |
| 15, RIMMSQOL button | Not written |
| Translation checks | Not written |

## Scenarios expected to fail

`10-delete-foreign-group.feature`, *Restore deleted groups brings its members back*. TESTING.md
says the members come back; `RestoreHiddenGroups` only clears the hidden list and leaves the
"no group" assignments `DissolveGroup` wrote. One of the two is wrong.
