using HarmonyLib;
using RimWorld;

namespace ArchitectStudio
{
    /// <summary>
    /// A tab the player has hidden answers "not visible". Last in line, so that the research-locked
    /// option, which lifts a "not visible" answer, cannot bring a hidden tab back.
    /// </summary>
    [HarmonyPatch(typeof(DesignationCategoryDef), nameof(DesignationCategoryDef.Visible), MethodType.Getter)]
    public static class DesignationCategoryDef_Hidden_Patch
    {
        [HarmonyPriority(Priority.Last)]
        public static void Postfix(DesignationCategoryDef __instance, ref bool __result)
        {
            if (__result && ArchitectStudioMod.Settings != null && CategoryVisibilityRuntime.IsHidden(__instance))
            {
                __result = false;
            }
        }
    }
}
