# Attributions

## Mods studied

No code was copied. The mods below served to understand the hooks in the Architect menu API;
Architect Studio's implementations are written from scratch.

Under the **MIT** licence, by **fernyrepos** (author: ferny; C#: Taranchuk, StreetKing):

- [Colored Categories](https://github.com/fernyrepos/Colored-Categories) - showed that
  `MainTabWindow_Architect.DoCategoryButton` is where a category button gets tinted.
  Architect Studio puts a plain Harmony prefix there, where the original uses a transpiler and an
  atlas replacement.
- [Better Architect Menu](https://github.com/fernyrepos/Better-Architect-Menu) - used to identify
  its own display caches and its nesting extension, which Architect Studio talks to by reflection.

The full text of their licence is reproduced in `LICENSE-fernyrepos.txt`
(MIT License, Copyright (c) 2025 fernyrepos).

**Category Manager** (Moriarty) was consulted to compare icon-selection approaches. Nothing was
taken from it: that mod is withdrawn from the Workshop and shipped with neither source nor a
licence file. Architect Studio reads the textures RimWorld has already loaded rather than walking
the disk.

## Integrations by reflection

No third-party assembly is referenced at compile time; everything is resolved at runtime and the
mod works if these mods are absent.

- **Better Architect Menu** (ferny) - subcategories, display cache purging.
- **Architect Icons** (bymarcin) - searching for and choosing a category icon.
- **Float Sub-Menus** (kathanon) - nested submenus in float menus.
