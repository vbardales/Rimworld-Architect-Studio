using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Affiche les batiments et les categories que la recherche verrouille encore, pour pouvoir les
    /// ranger avant de les debloquer.
    ///
    /// <c>Designator_Build.Visible</c> est le seul verrou : rien ne revalide la recherche au moment
    /// de poser un plan. Se contenter de rendre le designator visible permettrait donc de construire
    /// sans avoir cherche la technologie. On le marque desactive dans le meme mouvement : il
    /// s'affiche en grise, et un clic repond par un message au lieu de le selectionner.
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
                    // Deja visible : on retire notre desactivation si c'est nous qui l'avions posee,
                    // par exemple quand la recherche vient d'aboutir.
                    ClearOurDisable(__instance);
                    return;
                }

                if (!Enabled || !(__instance.PlacingDef is BuildableDef def) || def.IsResearchFinished)
                {
                    return;
                }

                // Le niveau technologique de la faction est un autre verrou, qui n'a rien a voir avec
                // la recherche : on le laisse cacher ce qu'il cache.
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
        /// Une categorie entiere peut etre verrouillee par la recherche. Sans cela, ses batiments
        /// seraient visibles mais son onglet resterait inaccessible.
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

                // Le palier de monolithe d'Anomaly est un verrou distinct : on ne le leve pas.
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
