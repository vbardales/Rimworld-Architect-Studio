import { createHash } from 'node:crypto';
import { access, readFile } from 'node:fs/promises';
import { join, resolve } from 'node:path';
import steam from 'semantic-release-steam';
import { compileReadme } from 'semantic-release-steam/lib/readme.mjs';
import { renderSteamBBCode } from 'semantic-release-steam/lib/description.mjs';

const publishing = (context) => context.env.STEAM_PUBLISH === 'true';

// Steam refuses a description or a change note above 8000 bytes of UTF-8.
const LIMIT_BYTES = 8000;

// "documented" mode (pluginConfig.documented): the release is driven by the files of the repository, like the
// manual publish workflow, and no feat:/fix: commit is needed.
//   - the version to release is given (RELEASE_VERSION) and must be the next patch, minor or major of the last
//     tag (or 1.0.0 when there is none): the plugin tells semantic-release which bump leads there;
//   - the GitHub release notes are the "## [<version>]" section of CHANGELOG.md;
//   - the Steam change note is the fenced block under "### <version>" of PUBLICATION.md, sent as written (BBCode).
const documented = (pluginConfig) => pluginConfig.documented === true;

function requestedVersion(context) {
  const version = context.env.RELEASE_VERSION;
  if (!/^\d+\.\d+\.\d+$/.test(version ?? '')) throw new Error(`RELEASE_VERSION must be the version to release, like 1.2.3, got '${version ?? ''}'`);
  return version;
}

// The bump type that leads from the last released version to the requested one.
export function bumpTo(last, requested) {
  if (!last) {
    if (requested !== '1.0.0') throw new Error(`there is no release yet: the first version is 1.0.0, not ${requested}`);
    return 'major';
  }
  const [lastMajor, lastMinor, lastPatch] = last.split('.').map(Number);
  const [major, minor, patch] = requested.split('.').map(Number);
  if (major === lastMajor + 1 && minor === 0 && patch === 0) return 'major';
  if (major === lastMajor && minor === lastMinor + 1 && patch === 0) return 'minor';
  if (major === lastMajor && minor === lastMinor && patch === lastPatch + 1) return 'patch';
  throw new Error(`${requested} is not the next patch, minor or major version after ${last}`);
}

// The fenced block under the first line matching the heading, like the manual workflow reads it.
function fencedBlockUnder(text, heading, label) {
  const lines = text.split(/\r?\n/);
  const start = lines.findIndex((line) => heading.test(line));
  if (start === -1) throw new Error(`no ${label} section found`);
  const open = lines.findIndex((line, i) => i > start && /^```/.test(line));
  const next = lines.findIndex((line, i) => i > start && /^#{1,6} /.test(line));
  if (open === -1 || (next !== -1 && open > next)) throw new Error(`the ${label} section has no fenced block`);
  const close = lines.findIndex((line, i) => i > open && /^```\s*$/.test(line));
  if (close === -1) throw new Error(`the block of ${label} is not closed`);
  const block = lines.slice(open + 1, close).join('\n').trim();
  if (!block) throw new Error(`the block of ${label} is empty`);
  return block;
}

async function changeNote(cwd, version) {
  const text = await readFile(join(cwd, 'PUBLICATION.md'), 'utf8').catch(() => { throw new Error('PUBLICATION.md is missing: it holds the Steam change note'); });
  const note = fencedBlockUnder(text, new RegExp(`^### +${version.replaceAll('.', '\\.')}(\\s|$)`), `"### ${version}" of PUBLICATION.md`);
  const bytes = Buffer.byteLength(note, 'utf8');
  if (bytes > LIMIT_BYTES) throw new Error(`the Steam change note is ${bytes} bytes of UTF-8, above the Steam limit of ${LIMIT_BYTES}`);
  return note;
}

async function changelogSection(cwd, version) {
  const text = await readFile(join(cwd, 'CHANGELOG.md'), 'utf8').catch(() => { throw new Error('CHANGELOG.md is missing: its "## [<version>]" section is the GitHub release notes'); });
  const lines = text.split(/\r?\n/);
  const start = lines.findIndex((line) => line.startsWith(`## [${version}]`));
  if (start === -1) throw new Error(`CHANGELOG.md has no "## [${version}]" section`);
  const end = lines.findIndex((line, i) => i > start && line.startsWith('## ['));
  const body = lines.slice(start + 1, end === -1 ? undefined : end).join('\n').trim();
  if (!body) throw new Error(`the "## [${version}]" section of CHANGELOG.md is empty`);
  return body;
}

async function checkMod(mod, pluginConfig, cwd, logger) {
  const modPath = resolve(cwd, mod.path);
  try {
    await access(join(modPath, 'README.template.md'));
  } catch {
    throw new Error(`${mod.path}/README.template.md is missing: the Steam plugin would fail after the tag and GitHub release were created`);
  }
  let ignore = '';
  try {
    ignore = await readFile(join(modPath, '.steamignore'), 'utf8');
  } catch {
    // reported below with the other missing entries
  }
  const lines = ignore.split(/\r?\n/).map((line) => line.trim());
  const missing = ['README.template.md', 'README.md'].filter((name) => !lines.includes(`/${name}`) && !lines.includes(name));
  if (missing.length > 0) {
    throw new Error(`${mod.path}/.steamignore must list /${missing.join(' and /')} (anchored to the mod root), or they ship to players`);
  }
  const description = renderSteamBBCode(await compileReadme({
    modPath,
    header: pluginConfig.descriptionHeader ?? '',
    footer: pluginConfig.descriptionFooter ?? '',
    assetDirNameTransform: pluginConfig.assetDirNameTransform,
  }));
  const bytes = Buffer.byteLength(description, 'utf8');
  if (bytes > LIMIT_BYTES) throw new Error(`the Steam description of ${mod.name} is ${bytes} bytes of UTF-8, above the Steam limit of ${LIMIT_BYTES}`);
  logger.log(`Steam description for ${mod.name}: ${description.length} characters of BBCode (${bytes} bytes), sha256 ${createHash('sha256').update(description).digest('hex')}`);
  // The dry-run is the only review of what will replace the page, so it prints the whole text.
  logger.log(`Steam description as it will be sent (converted from ${mod.path}/README.template.md):\n${description}`);
}

export async function verifyConditions(pluginConfig, context) {
  const cwd = context.cwd ?? process.cwd();
  for (const mod of pluginConfig.mods) {
    await checkMod(mod, pluginConfig, cwd, context.logger);
  }
  if (documented(pluginConfig)) {
    // Checked before any tag exists, so a missing note cannot leave a release with no Steam update.
    const version = requestedVersion(context);
    await changelogSection(cwd, version);
    context.logger.log(`Steam change note for ${version} (from PUBLICATION.md, sent as written):\n${await changeNote(cwd, version)}`);
  }
  if (publishing(context)) await steam.verifyConditions(pluginConfig, context);
}

export async function analyzeCommits(pluginConfig, context) {
  if (!documented(pluginConfig)) return undefined;
  return bumpTo(context.lastRelease?.version, requestedVersion(context));
}

export async function generateNotes(pluginConfig, context) {
  if (!documented(pluginConfig)) return undefined;
  return changelogSection(context.cwd ?? process.cwd(), context.nextRelease?.version ?? requestedVersion(context));
}

export async function publish(pluginConfig, context) {
  if (!publishing(context)) return undefined;
  const cwd = context.cwd ?? process.cwd();
  const notes = documented(pluginConfig)
    ? await changeNote(cwd, context.nextRelease.version)
    : (context.nextRelease.notes ? renderSteamBBCode(context.nextRelease.notes) : context.nextRelease.version);
  return steam.publish(pluginConfig, { ...context, nextRelease: { ...context.nextRelease, notes } });
}
