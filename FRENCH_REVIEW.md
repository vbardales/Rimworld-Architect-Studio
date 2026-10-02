# Architect Studio: French review

Generated from revision `b6e1f9f` by `Tests/New-FrenchReview.ps1` (reads the shipped XML; do not edit by hand). Working tree clean for `Mod/Languages` and `Mod/Defs`.

The mod has no source in another language (it is original work, studying two English mods), so the **Original** column repeats the English text.

**Gender agreement.** None of these texts agrees with a pawn: the mod names no colonist, no pawn and no person, so no `{PAWN_gender ? ... }` switch exists or is needed. Counted phrases use `.Zero`, `.One` and `.Many` key families chosen by the count. The French register avoids the bare imperative and the tutoiement: it describes the possibility or uses the infinitive.

The fifth column carries `?` rows: texts the session doubts, with the reason. No review line is written here; it belongs to the reviewer in `STATUS.md`.

## Keyed

Source: `Mod/Languages/French/Keyed/ArchitectStudio.xml` (English: `Mod/Languages/English/Keyed/ArchitectStudio.xml`)

| Key or path | Original | English | French | ? |
|---|---|---|---|---|
| `ArchitectStudio.Settings.Intro` | Reorganise the Architect menu from inside the game. | Reorganise the Architect menu from inside the game. | Réorganiser le menu Architecte depuis le jeu. |  |
| `ArchitectStudio.Settings.OpenDropdowns` | Dropdown groups… | Dropdown groups… | Groupes de menus déroulants… |  |
| `ArchitectStudio.Settings.ShowArchitectButton` | Button in the Architect menu | Button in the Architect menu | Bouton dans le menu Architecte |  |
| `ArchitectStudio.Settings.ShowArchitectButtonTip` | Adds a row at the bottom of the Architect window to open the editor without a keyboard. Reopen the Architect menu for the change to take effect. | Adds a row at the bottom of the Architect window to open the editor without a keyboard. Reopen the Architect menu for the change to take effect. | Ajouter une rangée en bas de la fenêtre Architecte permet d'ouvrir l'éditeur sans clavier. Le changement prend effet à la réouverture du menu Architecte. |  |
| `ArchitectStudio.Settings.KeyHint` | Keyboard shortcut: {0} — change it in Options → Keyboard configuration. | Keyboard shortcut: {0} — change it in Options → Keyboard configuration. | Raccourci clavier : {0} — modifiable dans Options → Configuration du clavier. |  |
| `ArchitectStudio.ArchitectButton` | Groups… | Groups… | Groupes… |  |
| `ArchitectStudio.ArchitectButtonTip` | Opens the dropdown group editor. | Opens the dropdown group editor. | Ouvrir l'éditeur de groupes de menus déroulants. |  |
| `ArchitectStudio.Dropdowns.Title` | Dropdown groups | Dropdown groups | Groupes de menus déroulants |  |
| `ArchitectStudio.Dropdowns.Intro` | Collapse several buildings under a single Architect menu button. Changes apply immediately. Drag a member to reorder it. | Collapse several buildings under a single Architect menu button. Changes apply immediately. Drag a member to reorder it. | Permet de regrouper plusieurs bâtiments sous un seul bouton du menu Architecte. Les changements sont appliqués immédiatement. Glisser un membre permet de le réordonner. |  |
| `ArchitectStudio.Dropdowns.GridMenu` | Icon grid menu | Icon grid menu | Menu en grille d'icônes |  |
| `ArchitectStudio.Dropdowns.IconSource` | Icons: | Icons: | Icônes : |  |
| `ArchitectStudio.Dropdowns.IconSourceCost` | Build material | Build material | Matériau de construction |  |
| `ArchitectStudio.Dropdowns.IconSourcePlaced` | Placed building | Placed building | Bâtiment posé |  |
| `ArchitectStudio.Dropdowns.IconSourceTip` | "Placed building" is the safe choice. With "Build material" and the grid menu, a building whose cost cannot be determined vanishes from the menu. | "Placed building" is the safe choice. With "Build material" and the grid menu, a building whose cost cannot be determined vanishes from the menu. | « Bâtiment posé » est le choix sûr. Avec « Matériau de construction » et le menu en grille, un bâtiment dont le coût est indéterminable disparaît du menu. |  |
| `ArchitectStudio.Dropdowns.ResetOrder` | Reset order | Reset order | Réinitialiser l'ordre |  |
| `ArchitectStudio.Dropdowns.Remove` | Remove from group | Remove from group | Retirer du groupe |  |
| `ArchitectStudio.Dropdowns.Groups` | Groups | Groups | Groupes |  |
| `ArchitectStudio.Dropdowns.NewGroup` | New group… | New group… | Nouveau groupe… |  |
| `ArchitectStudio.Dropdowns.Custom` | Group you created | Group you created | Groupe personnalisé |  |
| `ArchitectStudio.Dropdowns.GroupNameTitle` | Group name | Group name | Nom du groupe |  |
| `ArchitectStudio.Dropdowns.Members` | Members | Members | Membres |  |
| `ArchitectStudio.Dropdowns.MembersOf` | Members of {0} | Members of {0} | Membres de {0} |  |
| `ArchitectStudio.Dropdowns.NoSelection` | Select a group on the left, or create one. | Select a group on the left, or create one. | Sélectionner un groupe à gauche ou en créer un. |  |
| `ArchitectStudio.Dropdowns.Rename` | Rename | Rename | Renommer |  |
| `ArchitectStudio.Dropdowns.Delete` | Delete | Delete | Supprimer |  |
| `ArchitectStudio.Dropdowns.ConfirmDelete` | Delete the group "{0}"? Its buildings will revert to their original group. | Delete the group "{0}"? Its buildings will revert to their original group. | Supprimer le groupe « {0} » ? Ses bâtiments reprendront leur groupe d'origine. |  |
| `ArchitectStudio.Dropdowns.Add` | Add a building | Add a building | Ajouter un bâtiment |  |
| `ArchitectStudio.Dropdowns.Category` | Category: | Category: | Catégorie : |  |
| `ArchitectStudio.Dropdowns.AllCategories` | All categories | All categories | Toutes les catégories |  |
| `ArchitectStudio.Dropdowns.MoreResults.Zero` | …and {0} more. Refine your search. | …and {0} more. Refine your search. | … et {0} autre. La recherche permet d'affiner. | Unused form: the game draws this line only when results exceed the limit (never with 0). Kept for the validator. |
| `ArchitectStudio.Dropdowns.MoreResults.One` | …and {0} more. Refine your search. | …and {0} more. Refine your search. | … et {0} autre. La recherche permet d'affiner. |  |
| `ArchitectStudio.Dropdowns.MoreResults.Many` | …and {0} more. Refine your search. | …and {0} more. Refine your search. | … et {0} autres. La recherche permet d'affiner. |  |
| `ArchitectStudio.Dropdowns.OverrideCount.Zero` | {0} buildings moved from their original settings. | {0} buildings moved from their original settings. | {0} bâtiment déplacé par rapport aux réglages d'origine. |  |
| `ArchitectStudio.Dropdowns.OverrideCount.One` | {0} building moved from its original settings. | {0} building moved from its original settings. | {0} bâtiment déplacé par rapport aux réglages d'origine. |  |
| `ArchitectStudio.Dropdowns.OverrideCount.Many` | {0} buildings moved from their original settings. | {0} buildings moved from their original settings. | {0} bâtiments déplacés par rapport aux réglages d'origine. |  |
| `ArchitectStudio.Dropdowns.ResetAll` | Reset everything | Reset everything | Tout réinitialiser |  |
| `ArchitectStudio.Dropdowns.ConfirmReset` | Erase every group you created and every building move? | Erase every group you created and every building move? | Effacer tous les groupes créés et tous les déplacements de bâtiments ? |  |
| `ArchitectStudio.Dropdowns.ConfirmDissolveEmpty` | Delete "{0}"? This group belongs to another mod: its def will be recreated on next startup, but it will no longer appear in this list. | Delete "{0}"? This group belongs to another mod: its def will be recreated on next startup, but it will no longer appear in this list. | Supprimer « {0} » ? Ce groupe appartient à un autre mod : son def sera recréé au prochain démarrage, mais il ne réapparaîtra plus dans cette liste. |  |
| `ArchitectStudio.Dropdowns.ConfirmDissolve.Zero` | Delete "{1}"? Its {0} buildings will become separate buttons again in the Architect menu, and the group will disappear from this list. | Delete "{1}"? Its {0} buildings will become separate buttons again in the Architect menu, and the group will disappear from this list. | Supprimer « {1} » ? Son {0} bâtiment redeviendra un bouton séparé dans le menu Architecte, et le groupe disparaîtra de cette liste. | Unused form: an empty group uses ConfirmDissolveEmpty instead. Kept for the validator. |
| `ArchitectStudio.Dropdowns.ConfirmDissolve.One` | Delete "{1}"? Its {0} building will become a separate button again in the Architect menu, and the group will disappear from this list. | Delete "{1}"? Its {0} building will become a separate button again in the Architect menu, and the group will disappear from this list. | Supprimer « {1} » ? Son {0} bâtiment redeviendra un bouton séparé dans le menu Architecte, et le groupe disparaîtra de cette liste. |  |
| `ArchitectStudio.Dropdowns.ConfirmDissolve.Many` | Delete "{1}"? Its {0} buildings will become separate buttons again in the Architect menu, and the group will disappear from this list. | Delete "{1}"? Its {0} buildings will become separate buttons again in the Architect menu, and the group will disappear from this list. | Supprimer « {1} » ? Ses {0} bâtiments redeviendront des boutons séparés dans le menu Architecte, et le groupe disparaîtra de cette liste. |  |
| `ArchitectStudio.Dropdowns.HiddenCount.Zero` | {0} groups deleted. | {0} groups deleted. | {0} groupe supprimé. | Unused form: drawn only when at least one group is hidden. |
| `ArchitectStudio.Dropdowns.HiddenCount.One` | {0} group deleted. | {0} group deleted. | {0} groupe supprimé. |  |
| `ArchitectStudio.Dropdowns.HiddenCount.Many` | {0} groups deleted. | {0} groups deleted. | {0} groupes supprimés. |  |
| `ArchitectStudio.Dropdowns.RestoreHidden` | Restore deleted groups | Restore deleted groups | Restaurer les groupes supprimés |  |
| `ArchitectStudio.Dropdowns.SplitWarning` | Group split across {0} categories: the game will make that many separate buttons. | Group split across {0} categories: the game will make that many separate buttons. | Groupe éclaté sur {0} catégories : le jeu en fera autant de boutons séparés. |  |
| `ArchitectStudio.Dropdowns.SplitWarningTip` | Categories involved: {0}. A dropdown group is not tied to any category: the game groups category by category. | Categories involved: {0}. A dropdown group is not tied to any category: the game groups category by category. | Catégories concernées : {0}. Un groupe de menu déroulant n'est rattaché à aucune catégorie : le jeu regroupe catégorie par catégorie. |  |
| `ArchitectStudio.Dropdowns.GroupCategory` | Category: | Category: | Catégorie : |  |
| `ArchitectStudio.Dropdowns.GroupCategoryFree` | — none (members stay where they are) | — none (members stay where they are) | — aucune (les membres restent où ils sont) |  |
| `ArchitectStudio.Dropdowns.GroupCategoryTip` | Forces a category on the whole group: its members are moved there, and any you add later will follow. Without it, the group appears wherever its members are, and splits into several buttons if they are scattered. | Forces a category on the whole group: its members are moved there, and any you add later will follow. Without it, the group appears wherever its members are, and splits into several buttons if they are scattered. | Imposer une catégorie à tout le groupe : ses membres y sont déplacés, et ceux qui seront ajoutés ensuite suivront. Sans cela, le groupe apparaît là où sont ses membres, et se scinde en plusieurs boutons s'ils sont dispersés. |  |
| `ArchitectStudio.Settings.KeyHintUnbound` | No default keyboard shortcut — assign one in Options → Keyboard configuration. | No default keyboard shortcut — assign one in Options → Keyboard configuration. | Aucun raccourci clavier par défaut — assignable dans Options → Configuration du clavier. |  |
| `ArchitectStudio.Settings.OpenCategories` | Categories and subcategories… | Categories and subcategories… | Catégories et sous-catégories… |  |
| `ArchitectStudio.ArchitectButtonCategories` | Categories… | Categories… | Catégories… |  |
| `ArchitectStudio.ArchitectButtonCategoriesTip` | Reorder categories and their subcategories. | Reorder categories and their subcategories. | Réordonner les catégories et leurs sous-catégories. |  |
| `ArchitectStudio.Categories.Title` | Categories | Categories | Catégories |  |
| `ArchitectStudio.Categories.Intro` | The arrows move a category among its siblings. Click its name to change label, colour and icon. | The arrows move a category among its siblings. Click its name to change label, colour and icon. | Les flèches changent l'ordre d'affichage des catégories. Cliquer sur une catégorie permet de modifier son libellé, sa couleur et son icône. |  |
| `ArchitectStudio.Categories.ResetOrder` | Reset order | Reset order | Réinitialiser l'ordre |  |
| `ArchitectStudio.Categories.ConfirmResetOrder` | Put every category back in its original order? | Put every category back in its original order? | Remettre toutes les catégories dans leur ordre d'origine ? |  |
| `ArchitectStudio.Categories.EmptyTip` | Nothing to build here (subcategories included). | Nothing to build here (subcategories included). | Aucun bâtiment à construire ici (sous-catégories comprises). |  |
| `ArchitectStudio.EditCategory.Title` | Appearance: {0} | Appearance: {0} | Apparence : {0} |  |
| `ArchitectStudio.EditCategory.Label` | Label | Label | Libellé |  |
| `ArchitectStudio.EditCategory.Apply` | Apply | Apply | Appliquer |  |
| `ArchitectStudio.EditCategory.Color` | Colour | Colour | Couleur |  |
| `ArchitectStudio.EditCategory.NoColor` | None | None | Aucune |  |
| `ArchitectStudio.EditCategory.Icon` | Icon | Icon | Icône |  |
| `ArchitectStudio.EditCategory.DefaultIcon` | Default icon | Default icon | Icône par défaut |  |
| `ArchitectStudio.EditCategory.NoArchitectIcons` | Architect Icons is not loaded: icon selection is unavailable. | Architect Icons is not loaded: icon selection is unavailable. | Architect Icons n'est pas chargé : le choix d'icône est indisponible. |  |
| `ArchitectStudio.EditCategory.ResetThis` | Reset all | Reset all | Tout réinitialiser |  |
| `ArchitectStudio.Categories.New` | New category… | New category… | Nouvelle catégorie… |  |
| `ArchitectStudio.Categories.TopLevel` | — top-level category | — top-level category | — catégorie de premier niveau |  |
| `ArchitectStudio.EditCategory.Parent` | Parent category | Parent category | Catégorie parente |  |
| `ArchitectStudio.EditCategory.Delete` | Delete this category | Delete this category | Supprimer cette catégorie |  |
| `ArchitectStudio.EditCategory.ConfirmDelete` | Delete the category "{0}"? Its buildings will return to their original category. | Delete the category "{0}"? Its buildings will return to their original category. | Supprimer la catégorie « {0} » ? Ses bâtiments retourneront dans leur catégorie d'origine. |  |
| `ArchitectStudio.Categories.NoNesting` | Without Better Architect Menu there are no subcategories: the category is created at top level. | Without Better Architect Menu there are no subcategories: the category is created at top level. | Sans Better Architect Menu, les sous-catégories n'existent pas : la catégorie est créée au premier niveau. |  |
| `ArchitectStudio.Settings.Integrations` | Detected integrations | Detected integrations | Intégrations détectées |  |
| `ArchitectStudio.Settings.Detected` | detected | detected | détecté |  |
| `ArchitectStudio.Settings.NotDetected` | not detected | not detected | non détecté |  |
| `ArchitectStudio.Settings.ResetEverything` | Reset everything | Reset everything | Tout réinitialiser |  |
| `ArchitectStudio.Settings.ConfirmResetEverything` | Restore default preferences and erase all groups, created categories, orders, labels, colours and icons? | Restore default preferences and erase all groups, created categories, orders, labels, colours and icons? | Rétablir les préférences par défaut et effacer tous les groupes, catégories créées, ordres, libellés, couleurs et icônes ? |  |
| `ArchitectStudio.Common.CategoryPath` | {0} > {1} | {0} > {1} | {0} > {1} |  |
| `ArchitectStudio.Common.RemoveGlyph` | x | x | x | A one-letter glyph, not a word; same in both languages. |
| `ArchitectStudio.Settings.ShowResearchLocked` | Show what research still locks | Show what research still locks | Montrer ce que la recherche verrouille |  |
| `ArchitectStudio.Settings.ShowResearchLockedTip` | Reveals the buildings and categories still hidden behind unfinished research. They stay greyed out and cannot be built: this is for organising, not for bypassing research. | Reveals the buildings and categories still hidden behind unfinished research. They stay greyed out and cannot be built: this is for organising, not for bypassing research. | Fait apparaître les bâtiments et les catégories qu'une technologie non recherchée cache encore. Ils restent grisés et non constructibles : c'est pour ranger, pas pour contourner la recherche. |  |
| `ArchitectStudio.ResearchLocked.Reason` | Research not completed | Research not completed | Technologie non recherchée |  |
| `ArchitectStudio.KeyBindings.CategoryLabel` | {0} tab | {0} tab | Onglet {0} |  |
| `ArchitectStudio.KeyBindings.CategoryDescription` | Key bindings for the "{0}" section of the Architect menu. | Key bindings for the "{0}" section of the Architect menu. | Raccourcis clavier de la section « {0} » du menu Architecte. |  |

## DefInjected: KeyBindingDef

Source: `Mod/Languages/French/DefInjected/KeyBindingDef/KeyBindings.xml` (English: the Def itself, `Mod/Defs/KeyBindings.xml`)

| Key or path | Original | English | French | ? |
|---|---|---|---|---|
| `ArchitectStudio_OpenDropdowns.label` | Architect Studio: dropdown groups | Architect Studio: dropdown groups | Architect Studio : groupes de menus déroulants |  |

## DefInjected: MainButtonDef

Source: `Mod/Languages/French/DefInjected/MainButtonDef/MainButtons.xml` (English: the Def itself, `Mod/Defs/MainButtons.xml`)

| Key or path | Original | English | French | ? |
|---|---|---|---|---|
| `ArchitectStudio_Settings.label` | Architect Studio | Architect Studio | Architect Studio |  |
| `ArchitectStudio_Settings.description` | Open Architect Studio settings and editors. | Open Architect Studio settings and editors. | Ouvrir les réglages et les éditeurs d’Architect Studio. | Typographic apostrophe (’) here, straight (') everywhere else in Keyed. |

## Grammar

No `Languages/French/Strings` or grammar rules ship with this mod.
