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
- 2026-09-25 reset (en) `68fe26f`: 3/3 passed, the two new 15.4 scenarios included.
- 2026-09-25 very long accented name (en, fr) `68fe26f`: 1/1 each; captures read (names cut by an ellipsis, nothing overflows).
- 2026-09-25 reviews (fr) `68fe26f`, full pass after the 1.0.5 wording: 51 scenarios, 42 passed, 0 failed, 9 skipped (conditional, played in their own passes).
- 2026-09-25 publication: 1.0.5 on `ccad168` (dry-run 36167737673, publish 36167923552), no red afterwards, rollback target `v1.0.4` not needed.
- 2026-09-25 very long English accented names (en, fr) `3fb0b93`: 1/1 each; the names read as English in both languages, cut by an ellipsis.
- 2026-09-26 counted-phrase footer (en, fr) `4e86e04`: 1/1 each; the `.Zero` form read in both languages ("0 buildings moved…", "0 bâtiment déplacé…"), no `(s)` left.
