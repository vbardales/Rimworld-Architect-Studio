// Run with NODE_PATH pointing to dependencies containing playwright and sharp.
const fs = require('node:fs');
const path = require('node:path');
const http = require('node:http');
const { chromium } = require('playwright');
const sharp = require('sharp');
const root = path.resolve(__dirname, '..');
const qa = path.join(__dirname, 'preview-qa');
fs.mkdirSync(qa, { recursive: true });
const server = http.createServer((req,res) => {
  const filename = path.resolve(root, '.' + decodeURIComponent(req.url.split('?')[0]));
  if (!filename.startsWith(root + path.sep)) { res.writeHead(403).end(); return; }
  const types = { '.html':'text/html', '.png':'image/png', '.json':'application/json', '.xml':'application/xml' };
  fs.readFile(filename, (error,data) => { if(error) res.writeHead(404).end(); else { res.setHeader('Content-Type',types[path.extname(filename)] || 'text/plain'); res.end(data); } });
});
const luminance = rgb => rgb.map(x => { x/=255; return x <= .04045 ? x/12.92 : ((x+.055)/1.055)**2.4; }).reduce((sum,x,i)=>sum+x*[.2126,.7152,.0722][i],0);
const fromHex = x => x.slice(1).match(/../g).map(x=>parseInt(x,16));
const contrast = (a,b) => (Math.max(a,b)+.05)/(Math.min(a,b)+.05);
(async () => {
  await new Promise(resolve=>server.listen(0,'127.0.0.1',resolve));
  const browser = await chromium.launch({executablePath: process.env.CHROME_PATH || 'C:/Program Files/Google/Chrome/Application/chrome.exe', headless:true});
  try {
    const page = await browser.newPage({viewport:{width:896,height:504},deviceScaleFactor:1});
    await page.goto(`http://127.0.0.1:${server.address().port}/Art/preview.html`);
    const settings = await page.evaluate(()=>window.ready);
    const cdp = await page.context().newCDPSession(page);
    await cdp.send('DOM.enable'); await cdp.send('CSS.enable');
    const {root:dom} = await cdp.send('DOM.getDocument');
    const fonts = {}, rects = {};
    for(const selector of ['h1','.summary','.version']) {
      const {nodeId} = await cdp.send('DOM.querySelector',{nodeId:dom.nodeId,selector});
      fonts[selector] = (await cdp.send('CSS.getPlatformFontsForNode',{nodeId})).fonts;
      if (!fonts[selector].every(x=>/^Segoe UI(?: Semibold)?$/.test(x.familyName))) throw new Error('Unexpected fallback font: ' + JSON.stringify(fonts[selector]));
      rects[selector] = await page.locator(selector).evaluate(el=>{const r=el.getBoundingClientRect(); return {x:r.x,y:r.y,width:r.width,height:r.height};});
    }
    for(const r of Object.values(rects)) if(r.x<0||r.y<0||r.x+r.width>896||r.y+r.height>504) throw new Error('Text outside canvas');
    const final = await page.screenshot();
    await sharp(final).png({compressionLevel:9}).toFile(path.join(root,'Mod/About/Preview.png'));
    await sharp(final).resize(268).png().toFile(path.join(qa,'thumbnail.png'));
    await page.addStyleTag({content:'.copy, .version { visibility: hidden !important; }'});
    const background = await page.screenshot({path:path.join(qa,'background.png')});
    const {data,info} = await sharp(background).removeAlpha().raw().toBuffer({resolveWithObject:true});
    const results = {};
    for(const selector of ['h1','.summary']) {
      const r=rects[selector], ink=luminance(fromHex(settings.palette.inkPrimary));
      let minimum=Infinity, point=null;
      // Every pixel in the text's entire bounding rectangle: stricter than corners or glyphs.
      for(let y=Math.floor(r.y);y<Math.ceil(r.y+r.height);y++) for(let x=Math.floor(r.x);x<Math.ceil(r.x+r.width);x++) {
        const index=(y*info.width+x)*info.channels;
        const ratio=contrast(ink,luminance([...data.subarray(index,index+3)]));
        if(ratio<minimum){minimum=ratio;point={x,y};}
      }
      results[selector]={minimum,point};
      if(minimum<4.5) throw new Error(`Insufficient contrast for ${selector}: ${minimum}`);
    }
    const badgeIndex=(20*info.width+880)*info.channels;
    results.badge={minimum:contrast(luminance(fromHex(settings.palette.badgeInk)),luminance([...data.subarray(badgeIndex,badgeIndex+3)]))};
    if(results.badge.minimum<4.5) throw new Error('Insufficient badge contrast');
    const bytes=fs.statSync(path.join(root,'Mod/About/Preview.png')).size;
    if(bytes>=900000) throw new Error('Preview exceeds 900 KB');
    fs.writeFileSync(path.join(qa,'report.json'),JSON.stringify({canvas:[896,504],thumbnailWidth:268,version:settings.version,fonts,rects,contrast:results,tag:'Absent: original public mod',bytes},null,2)+'\n');
    console.log(JSON.stringify({contrast:results,bytes,fonts},null,2));
  } finally {await browser.close();server.close();}
})().catch(error=>{console.error(error);server.close();process.exitCode=1;});


