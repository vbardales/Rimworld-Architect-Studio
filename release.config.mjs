const steamPublishEnabled = process.env.STEAM_PUBLISH === 'true';

const plugins = [
  '@semantic-release/commit-analyzer',
  '@semantic-release/release-notes-generator',
  '@semantic-release/github',
];

// The Steam plugin is deliberately absent unless the protected production job opts in.
// This keeps `semantic-release --dry-run` credential-free and incapable of contacting Steam.
if (steamPublishEnabled) {
  plugins.push([
    'semantic-release-steam',
    {
      appId: '294100',
      branchTargets: { main: 'stable' },
      mods: [{
        name: 'Architect Studio',
        path: 'Mod',
        workshopIds: { stable: '3792784018' },
      }],
    },
  ]);
}

export default {
  branches: ['main'],
  plugins,
};
