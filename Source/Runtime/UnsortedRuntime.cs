using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Optional "Unsorted" Architect tab: a catch-all for the buildings no tab shows.
    ///
    /// An orphan is a building the game generated a blueprint for (it has a category, which is what
    /// <see cref="BuildableDef.BuildableByPlayer"/> tests) but whose default designator is switched
    /// off (<see cref="BuildableDef.canGenerateDefaultDesignator"/> false) and which no resolved
    /// designator of any other category provides either. A def with no category at all has no
    /// blueprint: it cannot be built, so it is not an orphan. Nothing here changes a def: the tab
    /// only adds designators to its own list, and the def only exists in memory.
    /// </summary>
    public static class UnsortedRuntime
    {
        public const string DefName = "AS_Unsorted";

        private static List<BuildableDef> orphansCache;

        public static bool Enabled => ArchitectStudioMod.Settings?.showUnsorted ?? false;

        public static bool IsUnsorted(DesignationCategoryDef category)
        {
            return category != null && category.defName == DefName;
        }

        /// <summary>Creates or removes the tab to match the preference.</summary>
        public static void Apply()
        {
            var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(DefName);

            if (!Enabled)
            {
                if (def != null)
                {
                    Remove(def);
                }

                return;
            }

            if (def == null)
            {
                def = new DesignationCategoryDef
                {
                    defName = DefName,
                    modContentPack = ArchitectStudioMod.Instance?.Content,
                    order = 0,
                    specialDesignatorClasses = new List<Type>
                    {
                        typeof(Designator_Cancel),
                        typeof(Designator_Deconstruct)
                    }
                };

                DefDatabase<DesignationCategoryDef>.Add(def);
                def.ResolveReferences();
            }

            def.label = "ArchitectStudio.Unsorted.Label".Translate();
            CustomCategoryRuntime.EnsureKeyBindingCategory(def);
            InvalidateOrphans();
            def.ResolveDesignators();
            def.DirtyCache();
            Refresh();
        }

        private static void Remove(DesignationCategoryDef def)
        {
            try
            {
                DefDatabase<DesignationCategoryDef>.Remove(def);
            }
            catch (Exception ex)
            {
                Log.Warning($"[Architect Studio] The Unsorted tab could not be removed from the def " +
                            $"database ({ex.Message}): it will disappear on the next restart.");
            }

            Refresh();
        }

        /// <summary>A tab was hidden or shown: the orphans, the counts and the tab lists change.</summary>
        public static void OnVisibilityChanged()
        {
            InvalidateOrphans();
            var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(DefName);
            if (def != null)
            {
                def.ResolveDesignators();
                def.DirtyCache();
            }

            Refresh();
        }

        public static void InvalidateOrphans()
        {
            orphansCache = null;
        }

        /// <summary>Buildings no other tab provides, in label order.</summary>
        public static List<BuildableDef> Orphans()
        {
            if (orphansCache != null)
            {
                return orphansCache;
            }

            var shown = new HashSet<BuildableDef>();
            var inHiddenTab = new HashSet<BuildableDef>();
            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                if (IsUnsorted(category) || category.AllResolvedDesignators == null)
                {
                    continue;
                }

                // A tab the player has hidden offers nothing: its buildings count as shown nowhere.
                var target = CategoryVisibilityRuntime.IsHidden(category) ? inHiddenTab : shown;
                foreach (var designator in category.AllResolvedDesignators)
                {
                    CollectShown(designator, target);
                }
            }

            orphansCache = DefDatabase<ThingDef>.AllDefsListForReading.Cast<BuildableDef>()
                .Concat(DefDatabase<TerrainDef>.AllDefsListForReading.Cast<BuildableDef>())
                .Where(d => d.BuildableByPlayer && !shown.Contains(d) &&
                            (!d.canGenerateDefaultDesignator || inHiddenTab.Contains(d)))
                .OrderBy(d => d.label ?? d.defName)
                .ThenBy(d => d.defName)
                .ToList();

            return orphansCache;
        }

        private static void CollectShown(Designator designator, HashSet<BuildableDef> shown)
        {
            if (designator is Designator_Dropdown dropdown)
            {
                foreach (var element in dropdown.Elements)
                {
                    CollectShown(element, shown);
                }
            }
            else if (designator is Designator_Build build && build.PlacingDef is BuildableDef def)
            {
                shown.Add(def);
            }
        }

        public static int Count => Enabled ? Orphans().Count : 0;

        /// <summary>Adds the orphans to the tab's designators. Called after the game's own resolution.</summary>
        public static void Fill(DesignationCategoryDef category)
        {
            if (!IsUnsorted(category))
            {
                return;
            }

            // The tab being rebuilt means something may have changed elsewhere.
            InvalidateOrphans();
            foreach (var def in Orphans())
            {
                category.AllResolvedDesignators.Add(new Designator_Build(def));
            }
        }

        private static void Refresh()
        {
            CategoryRuntime.InvalidateCounts();
            BetterArchitectCompat.InvalidateEditModeCaches();
            BetterArchitectCompat.InvalidateCaches();

            if (Current.ProgramState == ProgramState.Playing &&
                MainButtonDefOf.Architect.TabWindow is MainTabWindow_Architect architectWindow)
            {
                architectWindow.CacheDesPanels();
            }
        }
    }
}
