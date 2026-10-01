// Scratch script: cuts ModIcon.png out with a border flood-fill (never a global colour threshold,
// so a dark pixel inside the icon's own artwork is never mistaken for background), then composites
// it, rotated, into a corner of Mod/About/Preview.png as if it were leaning out of the frame.
import { createRequire } from 'node:module';
import { writeFile } from 'node:fs/promises';
const sharp = createRequire(import.meta.url)('C:/Users/nelim/AppData/Roaming/npm/node_modules/sharp');

const TOLERANCE = 28; // Euclidean RGB distance allowed between a filled pixel and its filled neighbour.

async function floodCutout(path) {
  const { data, info } = await sharp(path).removeAlpha().raw().toBuffer({ resolveWithObject: true });
  const { width, height, channels } = info; // channels === 3 (RGB), colour only — mask carries the cut, not the pixel data.
  const visited = new Uint8Array(width * height); // 1 = background, reached by the flood fill from the border.
  const stack = [];
  const idx = (x, y) => y * width + x;
  for (let x = 0; x < width; x++) { stack.push([x, 0]); stack.push([x, height - 1]); }
  for (let y = 0; y < height; y++) { stack.push([0, y]); stack.push([width - 1, y]); }

  const colourAt = (p) => { const o = p * channels; return [data[o], data[o + 1], data[o + 2]]; };
  const dist = (a, b) => Math.sqrt((a[0] - b[0]) ** 2 + (a[1] - b[1]) ** 2 + (a[2] - b[2]) ** 2);

  while (stack.length) {
    const [x, y] = stack.pop();
    if (x < 0 || y < 0 || x >= width || y >= height) continue;
    const p = idx(x, y);
    if (visited[p]) continue;
    visited[p] = 1;
    const here = colourAt(p);
    for (const [dx, dy] of [[1, 0], [-1, 0], [0, 1], [0, -1]]) {
      const nx = x + dx, ny = y + dy;
      if (nx < 0 || ny < 0 || nx >= width || ny >= height) continue;
      const np = idx(nx, ny);
      if (visited[np]) continue;
      if (dist(here, colourAt(np)) <= TOLERANCE) stack.push([nx, ny]);
    }
  }

  // Mask: 255 keeps the pixel, 0 removes it. Built separately from the colour data, then joined
  // as the alpha channel — the flood fill only ever decides the mask, never touches pixel colour.
  const mask = new Uint8Array(width * height);
  let removed = 0;
  for (let p = 0; p < width * height; p++) {
    if (visited[p]) removed++; else mask[p] = 255;
  }
  console.log(`flood-fill removed ${removed} / ${width * height} pixels (${(100 * removed / (width * height)).toFixed(1)}%)`);
  return sharp(data, { raw: { width, height, channels } })
    .joinChannel(Buffer.from(mask), { raw: { width, height, channels: 1 } })
    .png()
    .toBuffer();
}

const cutout = await floodCutout('Mod/About/ModIcon.png');
await writeFile('Art/preview-qa/badge-cutout.png', cutout);

const badgeSize = 200;
const rotated = await sharp(cutout).resize(badgeSize, badgeSize).rotate(15, { background: { r: 0, g: 0, b: 0, alpha: 0 } }).png().toBuffer();
const rmeta = await sharp(rotated).metadata();
await writeFile('Art/preview-qa/badge-rotated.png', rotated);

const base = sharp('Mod/About/Preview.png');
const { width: cw, height: ch } = await base.metadata();
// Bottom-left: the background art's left half is empty floor, so the badge leans out of that corner.
// Side (left) and bottom edges overflow the frame; top and right stay inside.
const left = -Math.round(rmeta.width * 0.32);
const top = ch - rmeta.height + Math.round(rmeta.height * 0.32);
const out = await base.composite([{ input: rotated, left, top }]).png().toBuffer();
await writeFile('Art/preview-qa/preview-with-badge.png', out);
console.log('badge', rmeta.width, rmeta.height, 'placed at', left, top, 'canvas', cw, ch);
