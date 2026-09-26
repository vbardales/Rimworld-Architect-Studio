// The Steam plugin is wrapped by release-steam-plugin.mjs. Without STEAM_PUBLISH=true it only
// checks the description sources, so `semantic-release --dry-run` stays credential-free and
// incapable of contacting Steam.
//
// documented mode: the wrapper decides the release. The version is the `version` input of the workflow (it must be
// the next patch, minor or major of the last tag), the GitHub release notes are the "## [<version>]" section of
// CHANGELOG.md and the Steam change note is the fenced block under "### <version>" of PUBLICATION.md, sent as
// written. Both are checked before any tag exists. So no commit-analyzer or release-notes-generator, and no
// feat:/fix: commit is needed.
const plugins = [
  [
    '@semantic-release/github',
    // The GitHub release carries the mod's name, like the ones made by hand before the pipeline.
    { releaseNameTemplate: 'Architect Studio <%= nextRelease.version %>' },
  ],
  [
    './release-steam-plugin.mjs',
    {
      documented: true,
      // The one source of the description: the Markdown block under this heading of PUBLICATION.md, converted to BBCode for
      // Steam and to plain text for the <description> of Mod/About/About.xml, which every run checks.
      descriptionFile: 'PUBLICATION.md',
      descriptionHeading: '^## Steam description$',
      descriptionFormat: 'markdown',
      aboutDescription: true,
      appId: '294100',
      branchTargets: { main: 'stable' },
      mods: [{
        name: 'Architect Studio',
        path: 'Mod',
        workshopIds: { stable: '3792784018' },
      }],
    },
  ],
];

export default {
  branches: ['main'],
  plugins,
};
