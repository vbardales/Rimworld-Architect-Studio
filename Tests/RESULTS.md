# Settings verification — 2026-09-13

Revision base: `d1bf712f0016993aff87ee18801ae296b2a55663`, plus the local MainButtons shortcut,
preference reset fix, name-validation extraction, translations and tests in this change.

## Executed

- Release build: successful, zero warnings/errors; shipped `Mod/Assemblies/ArchitectStudio.dll`
  rebuilt from these sources. Build references: RimWorld 1.6.4871, Harmony 2.4.2.
- `pwsh -NoProfile -File Tests/Run-Behavior.ps1`: **27 assertions passed** against the shipped
  DLL and installed RimWorld assemblies. Every PASS line contains the expected condition;
  any failed assertion or engine error terminates the command unsuccessfully.
- `pwsh -NoProfile -File Tests/Validate-Mod.ps1`: **739 static/XML assertions passed**,
  including the hidden shortcut's metadata and EN/FR label/description.
- `Check-DefInjected.ps1 -TransMod Mod`: **11,588 Defs indexed, three paths checked, zero errors**.
- Shipped DLL SHA-256: `F674C2251982ECF06C6132A426535CA54D5EC4681DEF2D78B6BA3FC4ED70CB7C`.

| Scenario | Observed result |
| --- | --- |
| Clean settings and new group | Expected toggle/schema defaults, empty custom definitions, list mode and placed-building icon default. |
| Save/load through real Scribe | Both toggles and every stored collection round-trip: groups, categories/parents, assignments, member/category orders, hidden groups, forced categories, labels, colours and icon paths. Accents and XML-special characters preserved. |
| Legacy empty settings document | Defaults restored and all collections initialized. |
| Post-load invalid records | Null/id-less entries removed; valid groups/categories retained. |
| Stable ordering | Stored order applied to actual BuildableDef inputs; new members remain stable at the end; replacement changes the order without duplicating the entry; clearing restores input order. Thing/terrain keys do not collide. |
| Name validation | Null/whitespace normalize to empty for the dialog's existing rejection guard; accents preserved; 500-character names are not truncated. |
| Preferences/reset | Both toggles reset to defaults; research runtime flag follows the setting; resetting preferences preserves group data for the full reset pipeline. Reset is available when either toggle alone differs from default, absent for clean settings. |

## Headless environment boundaries

The harness loads the real shipped assembly, not a copied implementation or Scribe mock.
It runs under .NET 8, not Unity's Mono runtime. It initializes a minimal preferences object,
registers the four serialized mod types in GenTypes' caches, and creates graphics-free Def
fixtures with identity fields for ordering tests. A harness-only Harmony prefix converts
Verse.Log.Error into a failing exception instead of invoking Unity's native logging stack.
These fixtures and the log hook exist only in the test process; no game files/configuration
are modified. Scribe and the mod's tested methods are not replaced or patched.

Initial experiments under Windows PowerShell and without the fixtures could not complete because
of Unity native ECall calls and missing active-mod discovery. The final suite above passes.
Directly invoking native MainButtonWorker.Visible also requires Unity-backed ModsConfig startup;
that attempted headless check was not counted as passed or retained as a fake implementation.
No game UI was run. Scratch XML and test build outputs are ignored under `.build/`.

## Access and integration review

The shipped MainButtonDef names the compiled worker, starts with `buttonVisible=false`, and
permits use without a current map. The worker uses the native visibility implementation and
opens `new Dialog_ModSettings(ArchitectStudioMod.Instance)`. The inspected game dialog delegates
to `mod.DoSettingsWindowContents` and calls `mod.WriteSettings` on close, so the primary and
shortcut routes share the actual mod instance, controls and persistence path.

Inspected local RIMMSQOL source from Workshop item 1084452457, `SettingsInit.cs`: its Main Buttons
editor enumerates all MainButtonDefs, exposes `buttonVisible`, and writes the selected value
back to that field. The shortcut remains in the Def database and never forces visibility per
frame. This establishes the supported mechanism, not a tested RIMMSQOL UI/version compatibility
claim. No optional integration was interactively tested and none became a dependency.

## Still requires in-game validation

TESTING.md scenarios 1–15, particularly both settings routes, hide/reveal persistence in RIMMSQOL,
UI scale and FR/EN rendering, actual research/build prevention, group/category runtime replay,
full reset of populated game definitions, log review, new game and an existing save.
Graphics, input events, Harmony startup and other mods' runtime interactions require the game
environment. They are not inferred from Scribe/order tests and remain the `done -> tested` gate
under the user's workflow clarification. CI runs build and static checks only; the behavioral
suite requires a local RimWorld installation and is not claimed to run on GitHub CI.
