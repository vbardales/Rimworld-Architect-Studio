using HarmonyLib;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// The game fills every dropdown menu in def database order. We reorder right afterwards, which
    /// covers the startup resolution and the ones we trigger ourselves in one go.
    /// </summary>
    [HarmonyPatch(typeof(DesignationCategoryDef), "ResolveDesignators")]
    public static class DesignationCategoryDef_ResolveDesignators_Patch
    {
        public static void Postfix(DesignationCategoryDef __instance)
        {
            DropdownOrderRuntime.ApplyOrder(__instance);
        }
    }
}
