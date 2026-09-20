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

Pickle patches the game through Concord when Concord is loaded, and through Harmony otherwise. If
Concord fails to start ("Failed to initialize Concord" in the log), none of Pickle's hooks land:
every `I click button` fails with "no tags recorded this frame". Disable Concord for the run.

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

A button is named by its translation key, never by its English text: `I click the Architect Studio
button keyed "ArchitectStudio.ArchitectButton"`. The label drawn is the one of the language the
game runs in, so a scenario spelling out "Groups…" only passes on an English game. The step name
carries the mod's own name because Pickle loads every suite's steps into one namespace, and
Work Studio defines a button-by-key step of its own.

Before clicking, that step moves the pointer and asks `WindowStack.GetWindowAt` which window owns
the point. A click is a real OS click and goes to whoever owns it, so a button covered by another
mod's window would otherwise fail the next step as "the dialog did not open" - a dead button, read
from the report, when the click never reached it. The step now names the covering window and the
assembly it comes from instead.

## Screenshots a person reads

Four features assert nothing and attach screenshots instead, all tagged `@review`: `04b` (arrows at
150%), `15` (the settings page behind the hidden shortcut), `16` (both editors at 100% and 150%, and
an accented category name in the generated keyboard category) and `17` (the same windows, dressed for
the Workshop page). The route is automated; the judgement is not.

`17` differs from `16` in what it is for. `16` shows whatever the mod list and the language happen to
produce, warts included - that is the point of a review shot. `17` sets the scene: it creates and
fills a group of its own rather than borrowing one from another mod, whose raw defName would read as
debug output on a store page.

Both hide the surrounding interface through `I hide the interface around the windows on screen`,
which turns on the game's own screenshot mode. That mode hides everything that is not a window -
the tab bar, the alerts, the colonist bar, the dev toolbar. Windows themselves keep drawing unless
told not to, so the step also clears the flag on Pickle's own runner windows, which would otherwise
sit in the corner of every capture. `I bring the interface back` undoes
it, and an `[AfterScenario]` does too, so a scenario dying in between cannot leave the game without
its interface.

## What stays manual

| TESTING.md | Why |
| --- | --- |
| 3, the pointer starting a drag | A physical drag is not replayed, only the drop |
| 10 and 12, a real restart | One process cannot rebuild the def database; the replay of the settings file is tested |
| 13, without optional mods | Needs its own mod list; tagged `@wip` |
| 15, the RIMMSQOL half | Revealing the button needs RIMMSQOL installed, and its persistence a real restart |
| The language switch | Changing language needs RimWorld restarted; `16` covers whichever language it is already running in |

## When a click lands on someone else's window

Solved on 2026-09-19, and kept here because the symptom is unrecognisable from the report alone.
*Groups… opens the group editor* failed for days as "the dialog did not open", which reads as a dead
button. It was not: RimIris kept a window of its own over the bottom-left corner of the screen, and
the OS click landed there. *Categories…* passed throughout, because it sits further right and cleared
that window. A player on that list could not click the button either - it was never a test artefact.

The click step now says so itself:

    the click at (56.00, 1045.00) would land on 'TutorialSkipWindow' (from RimIris), not on the
    Architect window: something is drawn over the button, and the button itself is not at fault

`TutorialSkipWindow` there is RimIris's own class, not the vanilla one - the assembly name in the
message is what says so. Removing RimIris turned all four scenarios of `02` green.

Nothing in this mod could have dodged it: the Architect window lives in that corner, so whatever we
draw in its bottom row is under whatever else claims the corner. If it happens again with another
mod, the message names it; that is the whole point of naming the assembly.

## Still open: clicks at 150% interface scale

`16`'s *screenshots of both editors at 150 percent* fails with the pointer landing away from the
button. Pickle's own conversion does apply `Prefs.UIScale`, and its tag store already refuses a rect
measured at another scale, so the remaining miss is narrower than "Pickle ignores the scale": the y
in the failure is one that cannot exist in 150% GUI space, so the rect used was measured before the
game had relaid out. `I set the interface scale to {int} percent` writes `Prefs.UIScale` directly,
which is the likely culprit and would be ours to fix, not Pickle's. Unconfirmed - it needs a run
that watches when the tag is recorded.
