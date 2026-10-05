# Publication 1.0.6 (2026-10-05)

- Dry-run of `8e8647131a2cb0dbf9bb4a6587e7ae10d00ad317`: run 37342232075, green (preview and description).
- Dry-run of `08803670d6c0fe725fa0ef26dc3f52453313703b` (the commit that sets `stage: prepublished`): run 37344693822, green; description 5413 characters, sha256 `0b38bec8`, preview 538769 bytes.
- Publish run 37344885738 started for `0880367`, options preview and description, waiting for the owner's approval of `steam-production`.
- Fail fast (owner, 2026-10-05): non-regression after the deploy. `minimal-en` on `8e86471`: 35 passed, 0 failed, 30 skipped (conditional). Others in flight: minimal-fr, optionals-en, reviews-en/fr, rimmsqol-en, restart-en. Rollback target `v1.0.5` (`ccad168`).
- Publish run 37344885738 approved by the owner: success, tag `v1.0.6` and release created at 17:17 UTC on `0880367`.
