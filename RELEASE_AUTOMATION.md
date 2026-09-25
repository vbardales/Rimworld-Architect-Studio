# CI and release automation — deliberately gated

This is a design and a dry-run path, not authorization to publish.  No workflow in this repository
uploads to Steam or creates a release until a maintainer manually dispatches it and approves the
protected `steam-production` GitHub environment.

`PUBLICATION.md` is the existing Workshop/publication source (there is no `AUDIT.md` or
`PUBLISHING.md` in this checkout).  Its requirements remain authoritative where they are stricter.

## Workflows

| Workflow | Trigger | Effect |
| --- | --- | --- |
| `build.yml` | push to `main`, pull request, manual | Builds the mod and validates shipped XML/resources. |
| `game-image.yml` | manual only, exact `PUSH_PRIVATE_IMAGE` confirmation | Builds a managed-assemblies-only RimWorld image and pushes it to private GHCR. It has no schedule or automatic trigger. |
| `release.yml` | manual only | Inputs `ref`, `version`, `mode`. Defaults to a credential-free semantic-release dry run. `publish` needs the full 40-character SHA that the dry-run printed and pauses at `steam-production`; only after approval can it create the tag, the GitHub release and update the existing Workshop item. |

The reference image is `ghcr.io/<owner>/rimworld-game`, built with
`RimWorks/steam-game-image-action@v1`, `runnable: false`, and only
`RimWorldLinux_Data/Managed`.  It contains proprietary game assemblies.  Keep the package private,
do not make it public, and do not use it outside CI.  The image builder's `skip-if-unchanged` gate
means a confirmed run does not fetch or push when Steam's build ID has not changed.

## Required GitHub configuration

Create repository secrets; never place their values in workflow files, logs, issues, or release notes.

| Secret | Used by | Purpose |
| --- | --- | --- |
| `STEAM_USERNAME` | confirmed image build; protected publish | Steam account name for an account that owns RimWorld and can update Workshop item `3792784018`. |
| `STEAM_CONFIG_VDF` | confirmed image build; protected publish | Base64 SteamCMD `config.vdf` with a saved login. Rotate it if the Steam session expires. |
| `GITHUB_TOKEN` | supplied automatically by Actions | GHCR authentication for the image builder and GitHub tag/release creation. No stored PAT is required. |

Create a GitHub Environment named `steam-production`, require named maintainer approval, and restrict
its Steam secrets to that environment.  Do **not** add Steam secrets to the repository-wide secret
scope.  `release-dry-run` must hold no Steam secrets.

## Release procedure and human gates

1. Run `release.yml` in `dry-run` mode with the commit (`ref`) and the `version` to release. It
   rebuilds `Mod/`, compares the rebuilt files with the tracked ones (a difference is named in the run
   summary with both hashes, then the tracked, tested files are put back: what ships is what is
   committed), runs `Tests/Validate-Mod.ps1`, and only checks that the version can be released and
   prints the notes and the Steam description. It stops if the version is not the next patch, minor or
   major of the last tag, or if the `## [<version>]` section of `CHANGELOG.md` or the `### <version>`
   change note of `PUBLICATION.md` is missing. It cannot create a tag, GitHub release, or Steam
   update because `STEAM_PUBLISH` and Steam credentials are absent. The dispatch is
   `Rimworld-Release-Admin/scripts/dispatch-publish.sh vbardales/Rimworld-Architect-Studio release.yml <SHA> <version>`.
2. A human reviews the calculated version/notes, `CHANGELOG.md`, the built DLL, and `git diff`.
3. A human runs the relevant Pickle scenarios and reviews their captured media. Green automation is
   route evidence, not a visual or gameplay sign-off; preserve the report/archive before another run
   overwrites it. See `Tests/Pickle/README.md` and `TESTING.md`.
4. A human checks `Art/steam/01-groups.jpg`, `02-categories.jpg` and `03-settings.jpg` and decides
   whether they should replace Workshop media. This automation never uploads screenshots.
5. Only then dispatch `release.yml` with `publish`, the same `version` and the full SHA of the dry-run
   (the run stops if `main` moved), and approve `steam-production`. The configured
   target is the existing item `3792784018`; it never creates a new Workshop item.
6. After the job, a human verifies Workshop visibility, the subscribed item/update, release notes,
   the live description, and the installed mod in RimWorld. Steam visibility and subscription are not
   API-validated by this workflow.

`semantic-release-steam` needs Node 20+, SteamCMD, and `rsync` in the publishing runner. Its
configuration maps `main` to the `stable` target and maps that target to the existing Workshop ID.
It is loaded only when `STEAM_PUBLISH=true`, so dry runs remain unable to invoke it.

## First activation checks

Before authorizing the first image push, set the GHCR package visibility to **private** and confirm
the repository owner has package write/read access.  Before authorizing the first publish, verify the
Steam account can update item `3792784018`, then create/verify the `steam-production` environment
approval rule.  Leave branch protection and human PR review in place: a merged conventional commit
is still not enough to publish because the release workflow is manual-only.
