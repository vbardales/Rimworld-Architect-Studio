using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Applique les groupes de menus deroulants choisis par l'utilisateur par-dessus les defs.
    /// Rien n'est ecrit sur disque : on mute <see cref="BuildableDef.designatorDropdown"/> en memoire,
    /// puis on force la categorie concernee a reconstruire ses designators.
    /// </summary>
    public static class DropdownRuntime
    {
        /// <summary>Valeur d'origine de chaque batiment, capturee avant le premier override.</summary>
        private static readonly Dictionary<string, DesignatorDropdownGroupDef> originalGroups =
            new Dictionary<string, DesignatorDropdownGroupDef>();

        /// <summary>Categorie d'origine de chaque batiment, pour pouvoir l'y remettre.</summary>
        private static readonly Dictionary<string, DesignationCategoryDef> originalCategories =
            new Dictionary<string, DesignationCategoryDef>();

        private static bool originalsCaptured;

        /// <summary>Groupes references par la config mais introuvables, deja signales une fois.</summary>
        private static readonly HashSet<string> warnedMissingGroups = new HashSet<string>();

        private static List<BuildableDef> buildablesCache;

        /// <summary>
        /// Cle stable pour un batiment. Un ThingDef et un TerrainDef peuvent theoriquement partager
        /// un defName : le prefixe evite qu'un override deborde de l'un sur l'autre.
        /// </summary>
        public static string KeyOf(BuildableDef def)
        {
            return (def is TerrainDef ? "terrain:" : "thing:") + def.defName;
        }

        /// <summary>
        /// Tous les batiments que le menu Architecte peut afficher. On s'aligne sur le filtre exact
        /// de <c>DesignationCategoryDef.ResolveDesignators</c> : hors de ce perimetre, changer le
        /// groupe n'aurait aucun effet visible.
        /// </summary>
        public static List<BuildableDef> AllBuildables()
        {
            return buildablesCache ??= DefDatabase<ThingDef>.AllDefsListForReading.Cast<BuildableDef>()
                .Concat(DefDatabase<TerrainDef>.AllDefsListForReading.Cast<BuildableDef>())
                .Where(d => d.designationCategory != null && d.canGenerateDefaultDesignator)
                .ToList();
        }

        public static void InvalidateBuildablesCache()
        {
            buildablesCache = null;
        }

        private static void CaptureOriginals()
        {
            if (originalsCaptured)
            {
                return;
            }

            foreach (var def in AllBuildables())
            {
                var key = KeyOf(def);
                originalGroups[key] = def.designatorDropdown;
                originalCategories[key] = def.designationCategory;
            }

            originalsCaptured = true;
        }

        public static DesignationCategoryDef OriginalCategoryOf(BuildableDef def)
        {
            CaptureOriginals();
            return originalCategories.TryGetValue(KeyOf(def), out var category) ? category : def.designationCategory;
        }

        /// <summary>
        /// Categorie imposee a un groupe entier, ou null si le groupe n'en impose aucune. C'est le
        /// seul moyen qu'un groupe ait une categorie a lui : nativement, il suit celle de ses membres
        /// et se scinde en autant de boutons qu'ils occupent de categories.
        /// </summary>
        public static DesignationCategoryDef TargetCategoryOf(string groupId)
        {
            if (groupId.NullOrEmpty() ||
                !ArchitectStudioMod.Settings.groupCategories.TryGetValue(groupId, out var categoryName) ||
                categoryName.NullOrEmpty())
            {
                return null;
            }

            return DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(categoryName);
        }

        private static DesignationCategoryDef DesiredCategoryFor(BuildableDef def, DesignatorDropdownGroupDef group)
        {
            var target = group != null ? TargetCategoryOf(group.defName) : null;
            if (target != null)
            {
                return target;
            }

            return originalCategories.TryGetValue(KeyOf(def), out var original) && original != null
                ? original
                : def.designationCategory;
        }

        public static DesignatorDropdownGroupDef OriginalGroupOf(BuildableDef def)
        {
            CaptureOriginals();
            return originalGroups.TryGetValue(KeyOf(def), out var group) ? group : null;
        }

        /// <summary>
        /// Recree les <see cref="DesignatorDropdownGroupDef"/> correspondant aux groupes de la config.
        /// Ces defs n'existent qu'en memoire : ils sont reconstruits a chaque demarrage.
        /// </summary>
        public static void EnsureCustomGroupDefs()
        {
            foreach (var entry in ArchitectStudioMod.Settings.customGroups)
            {
                var def = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(entry.id);
                if (def == null)
                {
                    def = new DesignatorDropdownGroupDef { defName = entry.id };
                    def.modContentPack = ArchitectStudioMod.Instance?.Content;
                    DefDatabase<DesignatorDropdownGroupDef>.Add(def);
                    // Add() renomme en cas de collision : on se realigne sur le defName retenu.
                    entry.id = def.defName;
                }

                def.label = entry.label;
                def.useGridMenu = entry.useGridMenu;
                def.iconSource = entry.iconSource;
            }
        }

        /// <summary>Groupe voulu pour ce batiment, en tenant compte de la config et des defauts.</summary>
        private static DesignatorDropdownGroupDef DesiredGroupFor(BuildableDef def)
        {
            var key = KeyOf(def);
            if (!ArchitectStudioMod.Settings.dropdownAssignments.TryGetValue(key, out var groupId))
            {
                return originalGroups.TryGetValue(key, out var original) ? original : null;
            }

            if (groupId.NullOrEmpty())
            {
                return null;
            }

            var group = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(groupId);
            if (group != null)
            {
                return group;
            }

            // Le groupe a disparu (mod retire). On revient au defaut plutot que de degrouper,
            // et on garde l'affectation au cas ou le mod reviendrait.
            if (warnedMissingGroups.Add(groupId))
            {
                Log.Warning($"[Architect Studio] Groupe de menu deroulant introuvable : '{groupId}'. " +
                            "Les batiments concernes reprennent leur groupe d'origine.");
            }

            return originalGroups.TryGetValue(key, out var fallback) ? fallback : null;
        }

        /// <summary>Applique toute la config et reconstruit les categories touchees.</summary>
        public static void Apply()
        {
            CaptureOriginals();
            EnsureCustomGroupDefs();

            var dirty = new HashSet<DesignationCategoryDef>();

            foreach (var def in AllBuildables())
            {
                var desiredGroup = DesiredGroupFor(def);
                if (def.designatorDropdown != desiredGroup)
                {
                    def.designatorDropdown = desiredGroup;
                    dirty.Add(def.designationCategory);
                }

                // Un changement de categorie touche les deux : celle qu'on quitte et celle qu'on rejoint.
                var desiredCategory = DesiredCategoryFor(def, desiredGroup);
                if (desiredCategory != null && def.designationCategory != desiredCategory)
                {
                    dirty.Add(def.designationCategory);
                    def.designationCategory = desiredCategory;
                    dirty.Add(desiredCategory);
                }
            }

            if (dirty.Count == 0)
            {
                return;
            }

            foreach (var category in dirty)
            {
                RebuildCategory(category);
            }

            // Des batiments ont pu changer de categorie : les comptes affiches ne valent plus rien.
            CategoryRuntime.InvalidateCounts();
            BetterArchitectCompat.InvalidateCaches();
        }

        /// <summary>Efface tous les overrides et remet les groupes d'origine.</summary>
        public static void ResetAll()
        {
            ArchitectStudioMod.Settings.dropdownAssignments.Clear();
            ArchitectStudioMod.Settings.customGroups.Clear();
            ArchitectStudioMod.Settings.groupOrders.Clear();
            ArchitectStudioMod.Settings.hiddenGroupIds.Clear();
            ArchitectStudioMod.Settings.groupCategories.Clear();
            ArchitectStudioMod.Instance.WriteSettings();
            Apply();
        }

        /// <summary>
        /// Reconstruit toutes les categories qui contiennent au moins un batiment de ce groupe.
        /// Sert aux changements qui ne touchent aucun champ de def - typiquement l'ordre interne -
        /// et que <see cref="Apply"/> ne verrait donc pas passer.
        /// </summary>
        public static void RebuildCategoriesOf(DesignatorDropdownGroupDef group)
        {
            if (group == null)
            {
                return;
            }

            var categories = AllBuildables()
                .Where(d => d.designatorDropdown == group)
                .Select(d => d.designationCategory)
                .Distinct()
                .ToList();

            foreach (var category in categories)
            {
                RebuildCategory(category);
            }

            BetterArchitectCompat.InvalidateCaches();
        }

        /// <summary>
        /// Force une categorie a reconstruire sa liste de designators. C'est le seul cache a purger :
        /// <c>ArchitectCategoryTab.DesignationTabOnGUI</c> relit <c>ResolvedAllowedDesignators</c>
        /// a chaque frame, donc l'affichage suit immediatement.
        /// </summary>
        public static void RebuildCategory(DesignationCategoryDef category)
        {
            if (category == null)
            {
                return;
            }

            category.ResolveDesignators();
            category.DirtyCache();
            DeselectStaleDesignator();
        }

        /// <summary>
        /// Une reconstruction jette les anciens objets Designator. Si le joueur en avait un de
        /// selectionne, il pointe desormais dans le vide : mieux vaut le deselectionner que de le
        /// laisser survivre en dehors de son menu.
        /// </summary>
        private static void DeselectStaleDesignator()
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }

            var manager = Find.DesignatorManager;
            var selected = manager?.SelectedDesignator;
            if (selected is Designator_Build || selected is Designator_Dropdown)
            {
                manager.Deselect();
            }
        }
    }
}
