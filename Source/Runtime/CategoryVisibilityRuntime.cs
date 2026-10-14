using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Hides whole Architect tabs. The def stays in the database and keeps its buildings: only the
    /// tab's <see cref="DesignationCategoryDef.Visible"/> answer changes (see
    /// <c>DesignationCategoryDef_Hidden_Patch</c>), which is what the Architect window and Better
    /// Architect Menu ask. The choice lives in the mod settings, so nothing is written to a def.
    /// </summary>
    public static class CategoryVisibilityRuntime
    {
        public static bool HasAny => ArchitectStudioMod.Settings.hiddenCategories.Count > 0;

        public static bool IsHidden(DesignationCategoryDef category)
        {
            return category != null && !UnsortedRuntime.IsUnsorted(category) &&
                   ArchitectStudioMod.Settings.hiddenCategories.Contains(category.defName);
        }

        /// <summary>The Unsorted tab has its own switch in the settings: it cannot be hidden here.</summary>
        public static bool CanHide(DesignationCategoryDef category)
        {
            return category != null && !UnsortedRuntime.IsUnsorted(category);
        }

        public static void Set(DesignationCategoryDef category, bool hidden)
        {
            if (!CanHide(category) || IsHidden(category) == hidden)
            {
                return;
            }

            var list = ArchitectStudioMod.Settings.hiddenCategories;
            if (hidden)
            {
                list.Add(category.defName);
            }
            else
            {
                list.Remove(category.defName);
            }

            ArchitectStudioMod.Instance.WriteSettings();
            Refresh();
        }

        public static void ResetAll()
        {
            if (!HasAny)
            {
                return;
            }

            ArchitectStudioMod.Settings.hiddenCategories.Clear();
            Refresh();
        }

        /// <summary>
        /// The buildings of a hidden tab are no longer offered anywhere, so the Unsorted tab has to
        /// look again; the Architect window and Better Architect Menu rebuild their tab lists.
        /// </summary>
        private static void Refresh()
        {
            UnsortedRuntime.OnVisibilityChanged();
        }
    }
}
