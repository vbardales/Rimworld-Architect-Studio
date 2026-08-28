using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Ordre des categories et des sous-categories. Tout passe par
    /// <see cref="DesignationCategoryDef.order"/> : le menu vanilla trie dessus, et les listes de
    /// sous-categories de Better Architect Menu aussi. Un seul champ a reecrire pour les deux niveaux.
    /// </summary>
    public static class CategoryRuntime
    {
        private static readonly Dictionary<string, int> originalOrders = new Dictionary<string, int>();
        private static bool originalsCaptured;

        private static void CaptureOriginals()
        {
            if (originalsCaptured)
            {
                return;
            }

            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                originalOrders[category.defName] = category.order;
            }

            originalsCaptured = true;
        }

        public static void Apply()
        {
            CaptureOriginals();

            var overrides = ArchitectStudioMod.Settings.categoryOrders;
            var changed = false;

            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                int desired;
                if (!overrides.TryGetValue(category.defName, out desired) &&
                    !originalOrders.TryGetValue(category.defName, out desired))
                {
                    continue;
                }

                if (category.order != desired)
                {
                    category.order = desired;
                    changed = true;
                }
            }

            if (changed)
            {
                Refresh();
            }
        }

        /// <summary>
        /// Categories de meme niveau : celles qui partagent la meme parente, ou toutes les
        /// categories racines. Rangees dans l'ordre d'affichage, du haut vers le bas.
        /// </summary>
        public static List<DesignationCategoryDef> SiblingsOf(DesignationCategoryDef category)
        {
            var parent = BetterArchitectCompat.ParentCategoryOf(category);

            return DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .Where(c => BetterArchitectCompat.ParentCategoryOf(c) == parent)
                .OrderByDescending(c => c.order)
                .ThenBy(c => c.LabelCap.ToString())
                .ToList();
        }

        public static bool CanMove(DesignationCategoryDef category, int delta)
        {
            var siblings = SiblingsOf(category);
            var index = siblings.IndexOf(category);
            var target = index + delta;
            return index >= 0 && target >= 0 && target < siblings.Count;
        }

        /// <summary>Deplace une categorie d'un cran parmi ses soeurs. delta -1 = vers le haut.</summary>
        public static bool Move(DesignationCategoryDef category, int delta)
        {
            var siblings = SiblingsOf(category);
            var index = siblings.IndexOf(category);
            var target = index + delta;

            if (index < 0 || target < 0 || target >= siblings.Count)
            {
                return false;
            }

            siblings.RemoveAt(index);
            siblings.Insert(target, category);

            // On renumerote toute la fratrie : ne changer que les deux concernees laisserait des
            // egalites d'ordre, que le tri departagerait ensuite par libelle.
            var overrides = ArchitectStudioMod.Settings.categoryOrders;
            for (var i = 0; i < siblings.Count; i++)
            {
                overrides[siblings[i].defName] = 10000 - i * 10;
            }

            ArchitectStudioMod.Instance.WriteSettings();
            Apply();
            return true;
        }

        // ---------------------------------------------------------------- contenu des categories

        private static Dictionary<string, int> ownCounts;
        private static Dictionary<string, int> totalCounts;

        /// <summary>
        /// A rappeler des qu'un batiment change de categorie. Sans cache, chaque ligne de la fenetre
        /// relirait les ~30 000 defs a chaque frame.
        /// </summary>
        public static void InvalidateCounts()
        {
            ownCounts = null;
            totalCounts = null;
        }

        private static void EnsureCounts()
        {
            if (ownCounts != null)
            {
                return;
            }

            ownCounts = new Dictionary<string, int>();
            foreach (var def in DropdownRuntime.AllBuildables())
            {
                var category = def.designationCategory;
                if (category == null)
                {
                    continue;
                }

                ownCounts.TryGetValue(category.defName, out var count);
                ownCounts[category.defName] = count + 1;
            }

            // Une categorie parente est souvent vide en propre - tout est dans ses sous-categories.
            // La griser serait faux, donc son total inclut celui de ses enfants.
            totalCounts = new Dictionary<string, int>(ownCounts);
            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                var parent = BetterArchitectCompat.ParentCategoryOf(category);
                if (parent == null)
                {
                    continue;
                }

                ownCounts.TryGetValue(category.defName, out var childCount);
                totalCounts.TryGetValue(parent.defName, out var parentCount);
                totalCounts[parent.defName] = parentCount + childCount;
            }
        }

        /// <summary>Nombre de batiments visibles dans cette categorie, sous-categories comprises.</summary>
        public static int ContentCountOf(DesignationCategoryDef category)
        {
            EnsureCounts();
            return totalCounts.TryGetValue(category.defName, out var count) ? count : 0;
        }

        public static bool HasOverrides => ArchitectStudioMod.Settings.categoryOrders.Count > 0;

        public static void ResetOrders()
        {
            ArchitectStudioMod.Settings.categoryOrders.Clear();
            ArchitectStudioMod.Instance.WriteSettings();
            Apply();
            Refresh();
        }

        /// <summary>
        /// Arborescence a afficher : chaque categorie racine suivie de ses sous-categories.
        /// </summary>
        public static List<CategoryRow> BuildTree()
        {
            var rows = new List<CategoryRow>();

            var roots = DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .Where(c => BetterArchitectCompat.ParentCategoryOf(c) == null)
                .OrderByDescending(c => c.order)
                .ThenBy(c => c.LabelCap.ToString());

            var placed = new HashSet<DesignationCategoryDef>();

            foreach (var root in roots)
            {
                rows.Add(new CategoryRow(root, 0));
                placed.Add(root);

                var children = DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                    .Where(c => BetterArchitectCompat.ParentCategoryOf(c) == root)
                    .OrderByDescending(c => c.order)
                    .ThenBy(c => c.LabelCap.ToString());

                foreach (var child in children)
                {
                    rows.Add(new CategoryRow(child, 1));
                    placed.Add(child);
                }
            }

            // Rien ne garantit que l'imbrication s'arrete a deux niveaux : une categorie dont la
            // parente est elle-meme imbriquee n'est ni une racine ni l'enfant d'une racine, et
            // disparaitrait de la liste. On la rattrape ici plutot que de la rendre inaccessible.
            var orphans = DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .Where(c => !placed.Contains(c))
                .OrderByDescending(c => c.order)
                .ThenBy(c => c.LabelCap.ToString());

            foreach (var orphan in orphans)
            {
                rows.Add(new CategoryRow(orphan, 1));
            }

            return rows;
        }

        private static void Refresh()
        {
            if (Current.ProgramState == ProgramState.Playing &&
                MainButtonDefOf.Architect.TabWindow is MainTabWindow_Architect architectWindow)
            {
                architectWindow.CacheDesPanels();
            }

            BetterArchitectCompat.InvalidateCaches();
        }

        public struct CategoryRow
        {
            public readonly DesignationCategoryDef def;
            public readonly int depth;

            public CategoryRow(DesignationCategoryDef def, int depth)
            {
                this.def = def;
                this.depth = depth;
            }
        }
    }
}
