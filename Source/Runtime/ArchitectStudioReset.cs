using System.Linq;
using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioReset
    {
        /// <summary>
        /// Remet tout a zero. Les categories creees passent par leur suppression normale, pour que
        /// leurs batiments soient rendus a leur categorie d'origine avant que le def ne disparaisse.
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
