using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Shows the buildings and categories research still locks, so they can be sorted before being
    /// unlocked.
    ///
    /// <c>Designator_Build.Visible</c> is the only lock: nothing revalidates research at the moment
    /// a blueprint is placed. Merely making the designator visible would therefore allow building
    /// without having researched the technology. We mark it disabled in the same move: it shows
    /// greyed out, and a click answers with a message instead of selecting it.
    /// </summary>
    public static class ResearchLockedVisibility
    {
        private static string reasonCached;

        private static string Reason => reasonCached ??= "ArchitectStudio.ResearchLocked.Reason".Translate();

        public static bool Enabled => ArchitectStudioMod.Settings?.showResearchLocked ?? false;

        [HarmonyPatch(typeof(Designator_Build), nameof(Designator_Build.Visible), MethodType.Getter)]
        public static class Designator_Build_Visible_Patch
        {
            public static void Postfix(Designator_Build __instance, ref bool __result)
            {
                if (__result)
                {
                    // Already visible: we remove our disabling if we are the ones who set it, for
                    // instance when the research has just completed.
                    ClearOurDisable(__instance);
                    return;
                }

                if (!Enabled || !(__instance.PlacingDef is BuildableDef def) || def.IsResearchFinished)
                {
                    return;
                }

                // The faction's tech level is another lock, unrelated to research: we let it hide
                // what it hides.
                var techLevel = Faction.OfPlayer?.def?.techLevel ?? TechLevel.Undefined;
                if (def.minTechLevelToBuild != TechLevel.Undefined && techLevel < def.minTechLevelToBuild)
                {
                    return;
                }

                if (def.maxTechLevelToBuild != TechLevel.Undefined && techLevel > def.maxTechLevelToBuild)
                {
                    return;
                }

                __result = true;
                __instance.disabled = true;
                __instance.disabledReason = Reason;
            }

            private static void ClearOurDisable(Designator_Build designator)
            {
                if (designator.disabled && designator.disabledReason == Reason)
                {
                    designator.disabled = false;
                    designator.disabledReason = null;
                }
            }
        }

        /// <summary>
        /// A whole category can be locked by research. Without this, its buildings would be visible
        /// but its tab would stay out of reach.
        /// </summary>
        [HarmonyPatch(typeof(DesignationCategoryDef), nameof(DesignationCategoryDef.Visible), MethodType.Getter)]
        public static class DesignationCategoryDef_Visible_Patch
        {
            public static void Postfix(DesignationCategoryDef __instance, ref bool __result)
            {
                if (__result || !Enabled || __instance.researchPrerequisites.NullOrEmpty())
                {
                    return;
                }

                // Anomaly's monolith tier is a separate lock: we do not lift it.
                if (ModsConfig.AnomalyActive && Find.Anomaly != null &&
                    Find.Anomaly.HighestLevelReached < __instance.minMonolithLevel && Find.Anomaly.GenerateMonolith)
                {
                    return;
                }

                if (__instance.researchPrerequisites.Any(r => !r.IsFinished))
                {
                    __result = true;
                }
            }
        }
    }
}
