using HarmonyLib;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Le jeu remplit chaque menu deroulant dans l'ordre de la base de defs. On reordonne juste apres,
    /// ce qui couvre d'un coup la resolution du demarrage et celles qu'on declenche nous-memes.
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
