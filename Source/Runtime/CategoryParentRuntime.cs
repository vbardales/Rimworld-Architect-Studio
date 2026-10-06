using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Moves a category that is not ours under another one, as a subcategory. Better Architect Menu
    /// nests with a <c>NestedCategoryExtension</c> on any <see cref="DesignationCategoryDef"/>: it
    /// does not care who owns the def, so grafting the same extension on a tab added by another mod
    /// is enough for the tab to leave the bar and appear in its new parent. Nothing is written to
    /// the def's file: the choice is stored in the settings and reapplied at startup.
    ///
    /// First step only: a category with subcategories of its own is not moved (BAM shows two levels).
    /// </summary>
    public static class CategoryParentRuntime
    {
        /// <summary>Parent each moved category had before we touched it (empty: top level).</summary>
        private static readonly Dictionary<string, string> originalParents = new Dictionary<string, string>();

        public static bool HasChildren(DesignationCategoryDef category)
        {
            return DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .Any(c => BetterArchitectCompat.ParentCategoryOf(c) == category);
        }

        /// <summary>True when the parent row is offered for a category that is not ours.</summary>
        public static bool CanHaveParent(DesignationCategoryDef category)
        {
            return category != null && BetterArchitectCompat.SubcategoriesSupported;
        }

        /// <summary>A parent must be a top-level tab, and not the category itself.</summary>
        public static bool CanBeParent(DesignationCategoryDef category, DesignationCategoryDef candidate)
        {
            return candidate != null && candidate != category &&
                   BetterArchitectCompat.ParentCategoryOf(candidate) == null;
        }

        public static void SetParent(DesignationCategoryDef category, DesignationCategoryDef parent)
        {
            if (!CanHaveParent(category) || HasChildren(category) ||
                (parent != null && !CanBeParent(category, parent)))
            {
                return;
            }

            var settings = ArchitectStudioMod.Settings;
            Remember(category);

            var original = originalParents[category.defName];
            var originalDef = original.NullOrEmpty() ? null : DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(original);

            // Back where it started (the same parent, or none): nothing left to store.
            if (parent == originalDef)
            {
                settings.categoryParents.Remove(category.defName);
            }
            else
            {
                settings.categoryParents[category.defName] = parent?.defName ?? "";
            }

            BetterArchitectCompat.TryAttachParent(category, parent);
            ArchitectStudioMod.Instance.WriteSettings();
            CustomCategoryRuntime.Refresh();
        }

        /// <summary>Reapplies the stored parents. A category or a parent that no longer exists is skipped.</summary>
        public static void Apply()
        {
            foreach (var pair in ArchitectStudioMod.Settings.categoryParents.ToList())
            {
                var category = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(pair.Key);
                if (category == null)
                {
                    continue;
                }

                var parent = pair.Value.NullOrEmpty() ? null : DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(pair.Value);
                if (!pair.Value.NullOrEmpty() && parent == null)
                {
                    continue;
                }

                Remember(category);
                BetterArchitectCompat.TryAttachParent(category, parent);
            }
        }

        /// <summary>A category we are about to delete: its adopted children go back where they were.</summary>
        public static void ForgetParent(DesignationCategoryDef deleted)
        {
            var settings = ArchitectStudioMod.Settings;

            foreach (var key in settings.categoryParents
                         .Where(pair => pair.Value == deleted.defName)
                         .Select(pair => pair.Key)
                         .ToList())
            {
                settings.categoryParents.Remove(key);
                var child = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(key);
                if (child != null)
                {
                    Restore(child);
                }
            }
        }

        public static void ResetAll()
        {
            var restored = false;

            foreach (var key in ArchitectStudioMod.Settings.categoryParents.Keys.ToList())
            {
                var category = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(key);
                if (category != null)
                {
                    Restore(category);
                    restored = true;
                }
            }

            ArchitectStudioMod.Settings.categoryParents.Clear();

            // The def is back where it was, but Better Architect Menu's tab and tree caches still
            // show it where we had put it until they are purged: only a created category's deletion
            // refreshed them, and a reset with nothing else to delete did not.
            if (restored)
            {
                CustomCategoryRuntime.Refresh();
            }
        }

        private static void Remember(DesignationCategoryDef category)
        {
            if (!originalParents.ContainsKey(category.defName))
            {
                originalParents[category.defName] =
                    BetterArchitectCompat.ParentCategoryOf(category)?.defName ?? "";
            }
        }

        private static void Restore(DesignationCategoryDef category)
        {
            if (!originalParents.TryGetValue(category.defName, out var original))
            {
                return;
            }

            var parent = original.NullOrEmpty() ? null : DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(original);
            BetterArchitectCompat.TryAttachParent(category, parent);
        }
    }
}
