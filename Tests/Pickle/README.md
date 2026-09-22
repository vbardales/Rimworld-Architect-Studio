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

- **Through the script**: `Tests/Pickle/Run-Pickle.ps1`, which is the way to prefer. It takes a lock
  no second session can take, keeps the previous report, drives the run and prints the failures with
  their attachments. Without `-Launch` it drives the game already open, through Pickle's dashboard on
  `http://localhost:27750/`; with `-Launch` it starts one, which is what a step assembly built since
  that game started needs. It never closes the game.

Scenario 02 clicks real buttons through OS input: the pointer moves on its own while it runs.

### Without taking the screen: the WSL game

There is a second RimWorld on this machine, a Linux copy under `~/rimworld` in WSL, downloaded with
steamcmd and kept for tests. **It is the one a session may launch**; the Windows install belongs to
its owner and is neither started nor stopped by anyone else. `scripts/stage-pickle-wsl.sh
ArchitectStudio`, in the monorepo, wipes its `Mods/`, copies Pickle, RimLogging, the hard
dependencies, the mod and this suite into it, and writes a `ModsConfig.xml` to match. It prints the
command to run, which drives the game under `xvfb` - a virtual screen, so real clicks happen and
nobody's desktop is taken. The run ends by itself and writes its report.

Two things that path needs. `-pickle-run` matches the companion mod's **display name**, so
`"Architect Studio - Pickle tests"`, exactly; a filter that matches nothing exits 2 without playing
anything. And the lock is still required: one machine, one runner, whichever RimWorld it is.

It stages the **Workshop** Pickle, not `Mods\Pickle-local`. To run against a local Pickle build,
copy it over `Mods/3791648678` after staging - the `packageId` is the same, so `ModsConfig` needs no
change.

To run one feature rather than the whole suite, delete the others from the staged copy under
`~/rimworld/Mods/ArchitectStudioPickleTests/Pickle/Features`. Never from this repo, and there is
nothing to put back: the next staging wipes `Mods/` anyway.

## The reports, and what overwrites them

Pickle writes every run into `PickleReports` and overwrites what was there, screenshots included,
and it writes only when a run ends. Two consequences, both of which have already cost an afternoon
here:

- **A run that does not finish leaves the report of the previous one in place.** A report older than
  the run you think you are reading is the most expensive trap in this suite - check its timestamp
  against the run. While a run is going, its live state is readable on `GET /state`, which carries
  each scenario's outcome, failure message and attachments without waiting for the end.
- **Starting a run destroys the evidence of the one before it.** `Run-Pickle.ps1` moves the previous
  report to `PickleReports-archive\<date>_<hour>` first and keeps the last five, rewriting the paths
  inside it so its screenshots still resolve. Launching the game by hand does not.

An archive is a reprieve, not storage: five runs later it is gone. A session that needs a report -
a failure to quote, a screenshot to compare against - **copies what it needs somewhere of its own,
and cleans up after itself**. Nothing in `PickleReports` survives by default.

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

## Evidence a person reads

The `@review` features automate the route and leave the reviewer only media to inspect. `03` films the
rendered member rows while the first row moves to last; `04b` captures the arrows at 100% and 150%; `15`
captures the settings page behind the hidden shortcut; `16` captures both editors at both scales and an
accented category name in the generated keyboard category; `17` prepares Workshop shots; `19` and `21`
capture RIMMSQOL's list/edit page and the settings route it reveals. A green capture or film proves that
the route ran, not that its pixels are correct.

Play `03`, `04b` and `16` with `wsl-deps.avec-revues.map`; it stages FilmTicks and InterfaceScale. The
film is attached under `screenshots/film/` and is encoded as WebM when `ffmpeg` is available (otherwise
the reviewable frames remain). Play `19` through `22` with `wsl-deps.avec-rimmsqol.map` and `-IncludeWip`.
Their requirements are explicit tags, so an incorrectly staged pass skips instead of claiming coverage.

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

## What belongs here, and what does not

A run confiscates the machine for tens of minutes: real clicks, a pointer moving on its own, the
screen busy. **A scenario that restates what a unit test already proves pays that price at every
run and adds nothing.** Only what a running game can show stays in Gherkin: a real click, a drag
replayed, something drawn, a reload, a capture, a third-party mod answering.

Six scenarios that only read stored state went down to `Tests/BehaviorTests.cs` on 2026-09-20 —
the arrows at the ends of a group, resetting the category order, a subcategory moving among its
siblings, a label reaching its def, that label going back, a colour set and cleared. What stayed
next to them in the same files is what the headless harness cannot reach: `05` compares the order
against the Architect window's own `desPanelsCached`, `04` against the menu's button order, and
`09`'s icon asks Architect Icons itself.

One caveat came out of the move, and it costs nothing: the subcategory test nests a category
through a stub of Better Architect Menu's `NestedCategoryExtension`, resolved by type and field
name the way the mod resolves the real one. It therefore cannot tell that BAM still spells it that
way. Neither could the scenario: a missing type made it skip, not fail.

## No manual scenario checklist

There is no remaining manual interaction procedure for Architect Studio. The suite owns setup, action,
assertions, restarts and cleanup; a person only reviews the media attached by `@review` scenarios. The
physical pointer path for drag starts is not exposed by the current Pickle API, so `03` records the actual
member list before and after the editor's registered drop callback rather than asking somebody to drag a
row. Features `20` through `22` perform the RIMMSQOL restart chain under one launcher lock. English and
French are separate launches selected by `-Language`, never an in-game language switch.

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

## Fixed, not yet upstream: clicks at 150% interface scale

`16`'s *screenshots of both editors at 150 percent* used to fail with the pointer landing away from
the button. **Measured on 2026-09-20 by `18-tag-geometry.feature`, and the cause was in Pickle, not
here** - an earlier note in this file blamed our own scale step, which the measurement cleared.
That probe scenario and its `TagProbe.cs` were removed on 2026-09-21, once the fix was upstream:
it printed the `GUIToScreenRect` it computed itself rather than what `Record` stored, so it read
the same before and after and could never show the fix working. The numbers below are its record.
Anything measuring the store again should read back through `TryGet`.
The fix is written and the scenario passes; it lives in `Mods\Pickle-local` and has not been sent
upstream.

`TagStore.Record` stores `GUIUtility.GUIToScreenRect(rect)`. That function composes two spaces:
it adds the clip origin **unscaled** and the local offset **scaled**. On a 1920x1080 window, for
the same button drawn at the same place in its window:

| | clip origin | local rect | what `Record` stores | true GUI rect |
| --- | --- | --- | --- | --- |
| 100% | `(0, 735)` | `y 285` | `y 1020` | `y 1020` |
| 150% | `(0, 375)` | `y 285` | `y 802.5` | `y 660` |

`375 + 285 x 1.5 = 802.5`, and that is neither GUI space (660) nor screen space (990). The
consumers then treat it as GUI space: `InputBackends.ToScreen` multiplies by `Prefs.UIScale`
again, giving `1203.75` on a screen 1080 tall, and the pointer is sent off the bottom. At scale 1
the two spaces coincide and the double conversion is invisible - which is why only this one
scenario is red.

Ruled out, with numbers, so nobody re-does it: our `I set the interface scale to {int} percent`
does everything the Options do and the GUI space follows (`1920x1080` to `1280x720`); the Architect
window is correctly laid out for the new scale (same size, only `y` moves, as a bottom anchor
should); the rect is not stale; and indexing the tag store's guard on `UI.screenWidth`/`screenHeight`
instead of `Prefs.UIScale` would change nothing, since scale and dimensions agree at both ends.

The fix is one statement in `TagStore.Record` - keep the rect in GUI space, which is what every
consumer wants:

```csharp
Vector2 clipOrigin = GUIUtility.GUIToScreenPoint(Vector2.zero);  // the clip origin, in GUI space
Rect guiRect = new(rect.position + clipOrigin, rect.size);
```

`GUIToScreenPoint(Vector2.zero)` is exact for this because the scaled term vanishes at zero. It
leaves 100% untouched (`0 + 285` is still `1020`) and gives `660` at 150%, which `ToScreen` turns
into the right `990`. It also makes the stored rect coherent: before, its position was
half-converted while its width and height stayed in GUI space.

### What was run

Written, built and deployed into `Mods\Pickle-local\Assemblies` on 2026-09-20, then replayed in
the WSL game (see *Without taking the screen*, above), twice, changing only which Pickle was
staged:

| Pickle staged | 100% | 150% |
| --- | --- | --- |
| `Pickle-local`, with the fix | passed | **passed** |
| Workshop copy, without it | passed | **failed** |

The 100% column is the control: it rules out the environment, since only the 150% row moves. The
failure in the second run names the defect exactly - *the pointer never reached (50.25, 814.00):
the OS reports x:75 y:1079* - a y of 814 in a GUI space 720 tall, which the OS then clamps to the
bottom edge of the screen.

That scenario is not a capture that happens to succeed: it goes `I click the Architect Studio
button keyed "ArchitectStudio.ArchitectButton"` and then `Then window "Dialog_DropdownGroups" is
open`, so a real click has to land on the button for it to pass.

Two things that green does **not** say, and should not be read into it. `16` is `@review`: it
asserts nothing about the images, so the 150% screenshots now exist and still need a person to
read them. And the fix is with RimWorks as PR 23: it is `fix/tag-rect-interface-scale`, 122f21f in
the fork at `github.com/vbardales/Rimworld-Pickle`, so a Workshop Pickle - anyone else's, and the
WSL staging unless told otherwise - still has the defect.

### The trap in rebuilding Pickle from that checkout

The first build deployed here carried the fix but silently dropped `3f514b2`, "ignore a tagged
rect measured at another interface scale", because the checkout it came from sits on
`feat/clear-the-screen`, which branches before that commit. The assembly already in that
checkout's `Assemblies/` had been built *before* the branch switch, so its md5 matching
`Pickle-local` proved only that the two binaries agreed - not that either matched the working
tree. Rebuilding from the tree therefore removed a guard from the machine that nothing asked to
remove. It was replaced the same evening by a build made on top of `3f514b2`.

So: after building Pickle from a shared checkout, **check the branch, and check the deployed
binary** - `get_UiScale` and `HeldAtAnotherScale` present, `GUIToScreenPoint` present,
`GUIToScreenRect` gone. Check it **case-sensitively**: a case-insensitive search for `UiScale`
also matches `Prefs.UIScale` and answers yes on every build, guard or no guard.
