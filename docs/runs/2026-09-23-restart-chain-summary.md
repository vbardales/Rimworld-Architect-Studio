# Restart chain, 2026-09-23 20:57-21:00 (set `redemarrage`, English)

Two real game launches under one hold of the machine lock (`-Then`), same profile, staged with the switch
mod `nelim.architectstudio.restartpass`. Full reports stay on disk in
`Tests/Pickle/Evidence/restart-0923-2059/{seq1,seq2}` (not in git, minified to about 100 KB: summary,
junit, messages, and seq1's `Player.log`). seq2 has no `Player.log`: the next ticket moved it. Its ownership rests on `summary.md` and the process ids below.

| Launch | Feature | Outcome | Duration (ms) |
|---|---|---|---|
| seq1 | `20-restart-write` "everything is configured and kept for the next launch" | Passed (1/1) | 14635 |
| seq2 | `21-restart-read` "the new process finds everything back" | Passed (1/1) | 15016 |

Proof of a real restart: seq1 wrote the marker as process `9d1d7fd303d74aca8fe0df8ce53a1807`, seq2 read it
as process `5a830a7af0ab4f508f28733e208de054` (different processes), and the Architect menu (375 buildings,
17 categories, 4 named buildings) was identical to the one kept. Both launches: `exitReason: passed`,
0 failed, 0 skipped, game log silent (asserted by the scenarios).

Not covered: the other passes (minimal, optionals, reviews, studio, RIMMSQOL) have not been replayed on
this revision.
