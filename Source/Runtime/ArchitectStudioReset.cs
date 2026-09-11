using System.Linq;
using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioReset
    {
        /// <summary>
        /// Resets everything. Created categories go through their normal deletion, so that their
        /// buildings are handed back to their original category before the def disappears.
        /// </summary>
        public static void All()
        {
            foreach (var entry in ArchitectStudioMod.Settings.customCategories.ToList())
            {
                var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(entry.id);
                if (def != null)
                {
                    CustomCategoryRuntime.Delete(def);
                }
            }

            ArchitectStudioMod.Settings.customCategories.Clear();

            CategoryAppearance.ResetAll();
            CategoryRuntime.ResetOrders();
            DropdownRuntime.ResetAll();

            ArchitectStudioMod.Instance.WriteSettings();
        }

        public static bool HasAnything
        {
            get
            {
                var s = ArchitectStudioMod.Settings;
                return s.customGroups.Count > 0 ||
                       s.dropdownAssignments.Count > 0 ||
                       s.groupOrders.Count > 0 ||
                       s.hiddenGroupIds.Count > 0 ||
                       s.groupCategories.Count > 0 ||
                       s.categoryOrders.Count > 0 ||
                       s.customCategories.Count > 0 ||
                       CategoryAppearance.HasOverrides;
            }
        }
    }
}
