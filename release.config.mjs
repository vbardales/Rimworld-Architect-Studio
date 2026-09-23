// The Steam plugin is wrapped by release-steam-plugin.mjs. Without STEAM_PUBLISH=true it only
// checks the description sources, so `semantic-release --dry-run` stays credential-free and
// incapable of contacting Steam. In a real run it checks them before any tag or GitHub release
// exists, and converts the release notes to Steam BBCode.
const plugins = [
  '@semantic-release/commit-analyzer',
  '@semantic-release/release-notes-generator',
  [
    '@semantic-release/github',
    // The GitHub release carries the mod's name, like the ones made by hand before the pipeline.
    { releaseNameTemplate: 'Architect Studio <%= nextRelease.version %>' },
  ],
  [
    './release-steam-plugin.mjs',
    {
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
