using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Order of categories and subcategories. Everything goes through
    /// <see cref="DesignationCategoryDef.order"/>: the vanilla menu sorts on it, and so do Better
    /// Architect Menu's subcategory lists. One field to rewrite for both levels.
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
        /// Categories of the same level: those sharing the same parent, or every root category.
        /// Sorted in display order, top to bottom.
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

        /// <summary>Moves a category one step among its siblings. delta -1 = upwards.</summary>
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

            // We renumber the whole sibling set: changing only the two concerned would leave order
            // ties, which the sort would then break by label.
            var overrides = ArchitectStudioMod.Settings.categoryOrders;
            for (var i = 0; i < siblings.Count; i++)
            {
                overrides[siblings[i].defName] = 10000 - i * 10;
            }

            ArchitectStudioMod.Instance.WriteSettings();
            Apply();
            return true;
        }

        // ---------------------------------------------------------------- category contents

        private static Dictionary<string, int> ownCounts;
        private static Dictionary<string, int> totalCounts;

        /// <summary>
        /// To be called back as soon as a building changes category. Without a cache, every row of
        /// the window would re-read the ~30,000 defs on every frame.
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

            // A parent category is often empty in its own right - everything sits in its
            // subcategories. Greying it out would be wrong, so its total includes its children's.
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

        /// <summary>Number of buildings visible in this category, subcategories included.</summary>
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
        /// Tree to display: each root category followed by its subcategories.
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

            // Nothing guarantees nesting stops at two levels: a category whose parent is itself
            // nested is neither a root nor the child of a root, and would vanish from the list. We
            // catch it here rather than leave it unreachable.
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
