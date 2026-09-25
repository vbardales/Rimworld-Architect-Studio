# Protocols read

Which documents this mod's session read, in which version, and which were of no use. Kept so that a
document that moves is re-read for what changed and one that never helped is not opened again.
"Version" is the last commit that touched the file (`git log -1 --format='%h %ad' -- <file>`, its
repository named) plus the first eight characters of its blob hash; the blob hash is the one to compare,
because the monorepo commit hash is shared by files that did not change.

Read on 2026-09-25, by the session `Architect Studio / done` (`local_da0ac2a6-…`), after a compaction.
"Full" means every line was read; "partial" names what was read.

## Read, useful

| File | Version | Read | What it changed here |
| --- | --- | --- | --- |
| `AGENTS.md` | monorepo `90d51374` 09-25 15:25, blob `bb4c08c1` | full | Evidence rule (latest report per revision, one text line per run in `docs/runs/`, never folders); CI publishing rules |
| `AUDIT.md` | `90d51374`, blob `4db3571e` | full | `done → tested` criteria; fail fast; requests carry no SHA; session title; no monitors |
| `PUBLISHING.md` | `90d51374`, blob `b9d6db1c` | full | Fail fast; everything on GitHub in English; gallery folder numbered in upload order; thanks and attribution copies; CI section |
| `TRANSLATIONS.md` | `90d51374`, blob `8970fe6c` | full | A change to French text resets `translation_fr` until revalidated; runtime checks stay `unverified` until done |
| `Rimworld-Ticket-Dispatcher/docs/WELCOME.md` | ticket-dispatcher `79668cc` 09-25 17:16, blob `eb72657f` | full (a 48-line earlier version was read on 09-24) | Submit requests, never launch; no watchers; small tickets; filter terms; `-DepMap` file name only; keep the tree frozen until `RUN_DONE`; this journal |
| `Rimworld-Ticket-Dispatcher/docs/SUBMIT.md` | `79668cc`, blob `a86821ac` | full | Options, exit codes, `-EvidenceDir` rules, what a request does not carry |
| `Rimworld-Release-Admin/docs/OPERATIONS.md` | release-admin `d403592` 09-25 16:33, blob `2bb7a32d` | full | Current release template (`ref`/`version` inputs, SHA guard, documented mode); this repository's workflow is behind it |
| `PickleTools/README.md` | pickletools `2b7b6d0` 09-25 17:22, blob `5b617e40` | full | Tool table and `path:` pass maps |
| `PickleTools/Headless/README.md` | pickletools `b2712fc` 09-25 15:03, blob `9e4bf0ba` | partial: lines 159-258 (settings seed, several passes, waiting, sleep and archive), 283-387 (`-Then`, hangs, reports), 438-487 (traps), all headings | Restart chain rules; the game logs in UTC; nothing is edited while a run goes |
| `./STATUS.md` | this repo `b7f8833`+, blob changes with every entry | front matter, headings, first sections; edited | It is the file kept up to date |
| `./PUBLICATION.md` | this repo `42d09cc` 09-23 20:50, blob `9f1c4022` (read before PR 4; PR 4 rewrote it: re-read "The description is sent by the release" and "After an upload") | full | Gallery order and file names; since PR 4 the release reads the description and change note from this file |
| `./TESTING.md` | this repo `2c076f8` 09-23 21:02, blob `d7b01926` | lines 1-36, 231-330, headings | Sections 12 to 15: restart, reset, MainButtons, the two open manual checks (15.4, 15.5) |
| `./CHANGELOG.md` | this repo `84b7476` 09-23 21:54, blob `8b9ebd95` | full (written by this session) | |
| `./docs/runs/` | this repo | full (written by this session) | One line per run |
| `./Tests/Pickle/README.md` | this repo | full (written by this session) | Passes, restart pass, proofs to keep |

## Read, of no use to this mod for now (do not reopen unless the file changes for a reason that concerns it)

| File | Version | Read | Why it did not help |
| --- | --- | --- | --- |
| `STYLE_RIMWORLD.md` | `90d51374`, blob `83c8a141` | headings only (484 lines) | Graphic style of Preview and ModIcon; only the owner generates them, and this mod's images exist |
| `scripts/SEARCHING.md` | `90d51374`, blob `93d971dc` | headings only (168 lines) | Searching the Workshop corpus for other mods' defs; no port or def collision to check |
| `PickleTools/Docs/steps.md` | pickletools `7268217` 09-25 17:22, blob `a79d9390` | header and the `ScreenshotMode` section | A generated catalogue: open it when a new scenario needs a step, not before |
| `./README.md` | this repo `ad2525f` 09-13, blob `7dc3e89d` | first 25 lines | Player-facing summary, no rule for this work |
| `./ATTRIBUTION.md`, `./LICENSE` | `ea985b0` 09-02, `96502b1` 09-19 | ATTRIBUTION full, LICENSE header | Identical to their copies in `Mod/` (checked with `diff`); nothing to do |
| `./Mod/About/About.xml` | this repo | the diff since `v1.0.3` | Credits changed for 1.0.4; nothing else |

## Named but absent or not read

`./BACKLOG.md`, `./NOTES.md` and `./BUGS.md` do not exist in this repository (the monorepo's `BACKLOG.md` was
excluded on purpose). `MOD_SETTINGS.md` is referenced by `AGENTS.md` and `TRANSLATIONS.md` but was not in the
list and was not read; `settings_audit` is `complete` from an earlier audit.

## Gaps this reading found (also in `STATUS.md`, `remaining`)

- The release workflow predated the current template: fixed by PR 4 (`b3776cc`); its first dry-run is pending.
- The gallery images were numbered `01-settings`, `02-categories`, `03-groups` while the upload order is groups first: renamed to `01-groups`, `02-categories`, `03-settings`.
- `translation_fr` is `partial` again: French strings changed after it was `complete` and two tooltips are unseen.
- TESTING.md 15.4 and 15.5 are neither automated nor justified as not applicable.
- Requests carry no revision: the tree changed while requests were queued (noted in `STATUS.md`).
