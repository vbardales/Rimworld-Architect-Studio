# Pass matrix after 1.0.4 (one line per run)

Format: date, pass (language), revision, result. Full reports are on disk under `Tests/Pickle/Evidence/`
(not in git). All runs: `exitReason: passed`, the report's `Player.log` carrying the expected `-pickle-run=`.

- 2026-09-23 restart chain (en) `84b7476`: 2/2 passed in two launches, different process ids. Evidence deleted, superseded by the 09-25 line below.
- 2026-09-24 minimal (en) `84b7476`: 32 passed, 16 skipped (conditional), 0 failed.
- 2026-09-24 optionals (en) `84b7476`: 33 passed, 15 skipped, 0 failed.
- 2026-09-24 reviews (en) `84b7476`: 39 passed, 9 skipped, 0 failed; film and five captures read.
- 2026-09-24 reviews (fr) `84b7476`: cut at 23/48 by a game hang loading the fixture save (`watchdog-timeout`), not a mod defect.
- 2026-09-24 studio (en) `84b7476`: 3/3, three captures read.
- 2026-09-24 RIMMSQOL (en) `84b7476`: 3/3, three captures read.
- 2026-09-25 minimal (fr) `84b7476`: 32 passed, 16 skipped, 0 failed, no error logged.
- 2026-09-25 reviews (fr) `84b7476`, replay of the cut run: 39 passed, 9 skipped, 0 failed; captures read.
- 2026-09-25 restart chain (en) `b7f8833`: 2/2 passed, written by `e2fb…` and read by `eb29…`, 375 buildings, 17 categories, 4 named buildings. Three earlier requests failed without a report: a CR in `wsl-deps.redemarrage.map` (fixed in `7d808d8`).
- 2026-09-25 French editors capture (fr) `cf410b0`: 1/1, the reworded intros read on screen.
