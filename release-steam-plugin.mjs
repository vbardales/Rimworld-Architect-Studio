import { createHash } from 'node:crypto';
import { access, readFile, writeFile } from 'node:fs/promises';
import { join, resolve } from 'node:path';
import { pathToFileURL } from 'node:url';
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
  // semantic-release used to generate the version heading of a note; a block sent as written has only the heading it
  // carries, and Steam shows the entry with no version at all when it has none (Architect Studio 1.0.5). A published
  // note can only be corrected by hand by the owner, so this stops the dry-run instead. The first line is a BBCode
  // line ([b] or [h1] to [h3]) that carries the version, like PUBLISHING.md asks ([b]1.3.0[/b]).
  const first = note.split('\n', 1)[0].trim();
  const escaped = version.replaceAll('.', '\\.');
  if (!new RegExp(`^\\[(b|h[1-3])\\].*(?<![\\d.])${escaped}(?![\\d.]).*\\[/(b|h[1-3])\\]$`).test(first)) {
    throw new Error(`the change note of ${version} must begin with a line that carries the version, like [b]${version}[/b] or [h3]${version}[/h3]: Steam shows the entry with no version otherwise. It begins with: ${first.slice(0, 80)}`);
  }
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

// "descriptionFile" mode (pluginConfig.descriptionFile + descriptionHeading): the Steam description is not compiled
// from <mod>/README.template.md but is the fenced block under the first line of that file matching the heading (a
// regular expression, like "^## Steam description"). The repository holds it in PUBLICATION.md next to the change
// notes, and README.template.md is not needed. descriptionFormat says what the block is: 'markdown' (the standard:
// converted to Steam BBCode by the same converter as a README) or 'bbcode' (the default, sent as written).
const descriptionFromFile = (pluginConfig) => pluginConfig.descriptionFile !== undefined;

async function descriptionFileBlock(pluginConfig, cwd) {
  const file = pluginConfig.descriptionFile;
  if (typeof file !== 'string' || !file || typeof pluginConfig.descriptionHeading !== 'string') {
    throw new Error('descriptionFile needs a repository-relative path, and descriptionHeading the regular expression of the heading above the block');
  }
  const format = pluginConfig.descriptionFormat ?? 'bbcode';
  if (!['bbcode', 'markdown'].includes(format)) throw new Error(`descriptionFormat must be 'bbcode' or 'markdown', got '${format}'`);
  const text = await readFile(resolve(cwd, file), 'utf8').catch(() => { throw new Error(`${file} is missing: it holds the Steam description`); });
  const block = fencedBlockUnder(text, new RegExp(pluginConfig.descriptionHeading), `"${pluginConfig.descriptionHeading}" of ${file}`);
  return { block, format };
}

async function descriptionFileText(pluginConfig, cwd) {
  const { block, format } = await descriptionFileBlock(pluginConfig, cwd);
  return format === 'markdown' ? renderSteamBBCode(block) : block;
}

const exists = (path) => access(path).then(() => true, () => false);

// "aboutDescription" (pluginConfig.aboutDescription: true): the <description> of <mod>/About/About.xml, which the game
// shows in the mod list, is generated as plain text from the same Markdown as the Steam description (README.template.md,
// or the Markdown block of descriptionFile) and every run stops when it differs. Same rules as the manual template's
// about-description.mjs, which tests/about-conversion.test.mjs keeps identical to this copy.
const encodeXml = (text) => text.replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;');
const decodeXml = (text) => text.replaceAll('&lt;', '<').replaceAll('&gt;', '>').replaceAll('&quot;', '"').replaceAll('&apos;', "'").replaceAll('&amp;', '&');

export function markdownToPlainText(markdown) {
  const lines = markdown.replace(/\r\n?/g, '\n').split('\n').map((raw) => {
    let line = raw.replace(/\s+$/, '');
    if (/^\s*([-*_])(\s*\1){2,}\s*$/.test(line)) return '';
    line = line.replace(/^\s{0,3}#{1,6}\s+/, '').replace(/\s+#+$/, '');
    line = line.replace(/^(\s*)>\s?/, '$1');
    line = line.replace(/^(\s*)[-*+]\s+/, '$1- ');
    line = line.replace(/!\[([^\]]*)\]\(([^)\s]+)(?:\s+"[^"]*")?\)/g, (_, alt, url) => (alt && alt !== url ? `${alt} (${url})` : url));
    line = line.replace(/\[([^\]]+)\]\(([^)\s]+)(?:\s+"[^"]*")?\)/g, (_, text, url) => (text === url ? url : `${text} (${url})`));
    line = line.replace(/\*\*(.+?)\*\*/g, '$1').replace(/__(.+?)__/g, '$1');
    line = line.replace(/(?<![\w*])\*(?!\s)(.+?)(?<!\s)\*(?![\w*])/g, '$1').replace(/(?<![\w_])_(?!\s)(.+?)(?<!\s)_(?![\w_])/g, '$1');
    line = line.replace(/`([^`]+)`/g, '$1');
    return line;
  });
  return lines.join('\n').replace(/\n{3,}/g, '\n\n').trim();
}

function locateDescription(xml) {
  const masked = xml.replace(/<!--[\s\S]*?-->/g, (comment) => ' '.repeat(comment.length));
  const match = masked.match(/<description>([\s\S]*?)<\/description>/);
  if (!match) throw new Error('About.xml has no <description>');
  return { start: match.index + '<description>'.length, end: match.index + '<description>'.length + match[1].length };
}

export function readAboutDescription(xml) {
  const { start, end } = locateDescription(xml);
  const inner = xml.slice(start, end);
  const cdata = inner.match(/^\s*<!\[CDATA\[([\s\S]*?)\]\]>\s*$/);
  return (cdata ? cdata[1] : decodeXml(inner)).replace(/\r\n?/g, '\n').trim();
}

export function replaceAboutDescription(xml, text) {
  const { start, end } = locateDescription(xml);
  return xml.slice(0, start) + encodeXml(text) + xml.slice(end);
}

async function aboutState(mod, pluginConfig, cwd) {
  let markdown;
  if (descriptionFromFile(pluginConfig)) {
    const { block, format } = await descriptionFileBlock(pluginConfig, cwd);
    if (format !== 'markdown') throw new Error("aboutDescription needs a Markdown description: set descriptionFormat: 'markdown' (the block of descriptionFile is BBCode otherwise)");
    markdown = block;
  } else {
    markdown = await readFile(join(resolve(cwd, mod.path), 'README.template.md'), 'utf8');
  }
  const expected = markdownToPlainText(markdown);
  if (!expected) throw new Error('aboutDescription: the description is empty, so About.xml cannot be generated from it');
  const path = join(mod.path, 'About', 'About.xml');
  const xml = await readFile(resolve(cwd, path), 'utf8').catch(() => { throw new Error(`${path} is missing`); });
  return { expected, current: readAboutDescription(xml), path, next: replaceAboutDescription(xml, expected) };
}

async function checkMod(mod, pluginConfig, cwd, logger) {
  const modPath = resolve(cwd, mod.path);
  const fromFile = descriptionFromFile(pluginConfig);
  if (!fromFile && !(await exists(join(modPath, 'README.template.md')))) {
    throw new Error(`${mod.path}/README.template.md is missing: the Steam plugin would fail after the tag and GitHub release were created`);
  }
  let ignore = '';
  try {
    ignore = await readFile(join(modPath, '.steamignore'), 'utf8');
  } catch {
    // reported below with the other missing entries
  }
  const lines = ignore.split(/\r?\n/).map((line) => line.trim());
  // With a description file the READMEs are not required, but one that is there must not ship to players.
  const readmes = ['README.template.md', 'README.md'];
  const guarded = fromFile ? (await Promise.all(readmes.map(async (name) => ((await exists(join(modPath, name))) ? name : null)))).filter(Boolean) : readmes;
  const missing = guarded.filter((name) => !lines.includes(`/${name}`) && !lines.includes(name));
  if (missing.length > 0) {
    throw new Error(`${mod.path}/.steamignore must list /${missing.join(' and /')} (anchored to the mod root), or they ship to players`);
  }
  const description = fromFile
    ? await descriptionFileText(pluginConfig, cwd)
    : renderSteamBBCode(await compileReadme({
      modPath,
      header: pluginConfig.descriptionHeader ?? '',
      footer: pluginConfig.descriptionFooter ?? '',
      assetDirNameTransform: pluginConfig.assetDirNameTransform,
    }));
  const bytes = Buffer.byteLength(description, 'utf8');
  if (bytes > LIMIT_BYTES) throw new Error(`the Steam description of ${mod.name} is ${bytes} bytes of UTF-8, above the Steam limit of ${LIMIT_BYTES}`);
  logger.log(`Steam description for ${mod.name}: ${description.length} characters of BBCode (${bytes} bytes), sha256 ${createHash('sha256').update(description).digest('hex')}`);
  // The dry-run is the only review of what will replace the page, so it prints the whole text.
  logger.log(`Steam description as it will be sent (${fromFile ? (pluginConfig.descriptionFormat === 'markdown' ? `Markdown block of ${pluginConfig.descriptionFile} converted to BBCode` : `taken as written from ${pluginConfig.descriptionFile}`) : `converted from ${mod.path}/README.template.md`}):\n${description}`);
}

export async function verifyConditions(pluginConfig, context) {
  const cwd = context.cwd ?? process.cwd();
  for (const mod of pluginConfig.mods) {
    await checkMod(mod, pluginConfig, cwd, context.logger);
    if (pluginConfig.aboutDescription === true) {
      const { expected, current, path } = await aboutState(mod, pluginConfig, cwd);
      if (current !== expected) throw new Error(`the <description> of ${path} is not the plain text of the description source: run "node release-steam-plugin.mjs --sync-about --write", read the diff and commit it`);
      context.logger.log(`About.xml: the description of ${mod.name} is the plain text of the description source`);
    }
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
  // semantic-release-steam takes these two functions from the context (its own test seam): the description it
  // would compile from README.template.md is replaced by the block of the description file, unconverted.
  const fromFile = descriptionFromFile(pluginConfig)
    ? { compileReadme: async () => '', buildSteamDescription: async () => descriptionFileText(pluginConfig, cwd) }
    : {};
  return steam.publish(pluginConfig, { ...context, ...fromFile, nextRelease: { ...context.nextRelease, notes } });
}

// node release-steam-plugin.mjs --sync-about [--write]   (from the root of the repository)
// Says whether the <description> of About.xml is the plain text of the description source (exit 1 when it is not), and
// with --write rewrites that one element and nothing else.
if (process.argv[1] && import.meta.url === pathToFileURL(resolve(process.argv[1])).href && process.argv.includes('--sync-about')) {
  const cwd = process.cwd();
  const config = (await import(pathToFileURL(join(cwd, 'release.config.mjs')).href)).default;
  const options = config.plugins.find((entry) => Array.isArray(entry) && String(entry[0]).includes('release-steam-plugin'))?.[1];
  if (!options?.aboutDescription) throw new Error('release.config.mjs does not set aboutDescription: true for this plugin');
  let drift = false;
  for (const mod of options.mods) {
    const { expected, current, path, next } = await aboutState(mod, options, cwd);
    if (current === expected) console.log(`${path}: the description is already the plain text of the description source.`);
    else if (process.argv.includes('--write')) { await writeFile(resolve(cwd, path), next); console.log(`${path}: description rewritten. Read the diff, then commit.`); }
    else { console.log(`${path}: the description differs from the plain text of the description source. Run with --write.`); drift = true; }
  }
  if (drift) process.exit(1);
}
