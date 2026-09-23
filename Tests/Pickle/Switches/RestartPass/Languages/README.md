# Why this folder exists

This mod is an on/off switch and holds no content, so RimWorld would log
`Mod Architect Studio - restart pass switch did not load any content` at every start. Any file under
`Languages/` satisfies that check without putting anything into the game: see
`Tests/Pickle/Mod/Languages/README.md` for the mechanism. Do not add a language subfolder here.
