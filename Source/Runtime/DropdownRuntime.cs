using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Applies the dropdown groups chosen by the user on top of the defs. Nothing is written to
    /// disk: we mutate <see cref="BuildableDef.designatorDropdown"/> in memory, then force the
    /// category concerned to rebuild its designators.
    /// </summary>
    public static class DropdownRuntime
    {
        /// <summary>Original value of each building, captured before the first override.</summary>
        private static readonly Dictionary<string, DesignatorDropdownGroupDef> originalGroups =
            new Dictionary<string, DesignatorDropdownGroupDef>();

        /// <summary>Original category of each building, so it can be put back there.</summary>
        private static readonly Dictionary<string, DesignationCategoryDef> originalCategories =
            new Dictionary<string, DesignationCategoryDef>();

        private static bool originalsCaptured;

        /// <summary>Groups the configuration references but which cannot be found, already reported once.</summary>
        private static readonly HashSet<string> warnedMissingGroups = new HashSet<string>();

        private static List<BuildableDef> buildablesCache;

        /// <summary>
        /// Stable key for a building. A ThingDef and a TerrainDef can in theory share a defName:
        /// the prefix keeps an override from spilling from one onto the other.
        /// </summary>
        public static string KeyOf(BuildableDef def)
        {
            return (def is TerrainDef ? "terrain:" : "thing:") + def.defName;
        }

        /// <summary>
        /// Every building the Architect menu can display. We align on the exact filter in
        /// <c>DesignationCategoryDef.ResolveDesignators</c>: outside that perimeter, changing the
        /// group would have no visible effect.
        /// </summary>
        public static List<BuildableDef> AllBuildables()
        {
            return buildablesCache ??= DefDatabase<ThingDef>.AllDefsListForReading.Cast<BuildableDef>()
                .Concat(DefDatabase<TerrainDef>.AllDefsListForReading.Cast<BuildableDef>())
                .Where(d => d.designationCategory != null && d.canGenerateDefaultDesignator)
                .ToList();
        }

        public static void InvalidateBuildablesCache()
        {
            buildablesCache = null;
        }

        private static void CaptureOriginals()
        {
            if (originalsCaptured)
            {
                return;
            }

            foreach (var def in AllBuildables())
            {
                var key = KeyOf(def);
                originalGroups[key] = def.designatorDropdown;
                originalCategories[key] = def.designationCategory;
            }

            originalsCaptured = true;
        }

        public static DesignationCategoryDef OriginalCategoryOf(BuildableDef def)
        {
            CaptureOriginals();
            return originalCategories.TryGetValue(KeyOf(def), out var category) ? category : def.designationCategory;
        }

        /// <summary>
        /// Category forced on a whole group, or null if the group forces none. This is the only way
        /// for a group to have a category of its own: natively it follows its members' and splits
        /// into as many buttons as they occupy categories.
        /// </summary>
        public static DesignationCategoryDef TargetCategoryOf(string groupId)
        {
            if (groupId.NullOrEmpty() ||
                !ArchitectStudioMod.Settings.groupCategories.TryGetValue(groupId, out var categoryName) ||
                categoryName.NullOrEmpty())
            {
                return null;
            }

            return DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(categoryName);
        }

        private static DesignationCategoryDef DesiredCategoryFor(BuildableDef def, DesignatorDropdownGroupDef group)
        {
            var target = group != null ? TargetCategoryOf(group.defName) : null;
            if (target != null)
            {
                return target;
            }

            return originalCategories.TryGetValue(KeyOf(def), out var original) && original != null
                ? original
                : def.designationCategory;
        }

        public static DesignatorDropdownGroupDef OriginalGroupOf(BuildableDef def)
        {
            CaptureOriginals();
            return originalGroups.TryGetValue(KeyOf(def), out var group) ? group : null;
        }

        /// <summary>
        /// Recreates the <see cref="DesignatorDropdownGroupDef"/>s matching the groups in the
        /// configuration. These defs only exist in memory: they are rebuilt at every startup.
        /// </summary>
        public static void EnsureCustomGroupDefs()
        {
            foreach (var entry in ArchitectStudioMod.Settings.customGroups)
            {
                var def = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(entry.id);
                if (def == null)
                {
                    def = new DesignatorDropdownGroupDef { defName = entry.id };
                    def.modContentPack = ArchitectStudioMod.Instance?.Content;
                    DefDatabase<DesignatorDropdownGroupDef>.Add(def);
                    // Add() renames on collision: we realign on the defName it kept.
                    entry.id = def.defName;
                }

                def.label = entry.label;
                def.useGridMenu = entry.useGridMenu;
                def.iconSource = entry.iconSource;
            }
        }

        /// <summary>
        /// A group we do not own (shipped by the game or another mod) is dissolved by adding it to
        /// <c>hiddenGroupIds</c> alone: unlike a per-member override in <c>dropdownAssignments</c>,
        /// that one string reliably survives a settings round-trip through <c>Mod.GetSettings</c>,
        /// which a bulk empty-value dictionary entry per member does not. Anything that would
        /// otherwise resolve to a hidden group falls back to that building's own original group
        /// instead - unless the original group is itself the hidden one, in which case there is
        /// nothing to fall back to and the building goes ungrouped.
        /// </summary>
        private static DesignatorDropdownGroupDef SkipHidden(DesignatorDropdownGroupDef candidate, string key)
        {
            if (candidate == null || !ArchitectStudioMod.Settings.hiddenGroupIds.Contains(candidate.defName))
            {
                return candidate;
            }

            var original = originalGroups.TryGetValue(key, out var value) ? value : null;
            return original != null && original != candidate && !ArchitectStudioMod.Settings.hiddenGroupIds.Contains(original.defName)
                ? original
                : null;
        }

        /// <summary>Group wanted for this building, taking the configuration and defaults into account.</summary>
        private static DesignatorDropdownGroupDef DesiredGroupFor(BuildableDef def)
        {
            var key = KeyOf(def);
            if (!ArchitectStudioMod.Settings.dropdownAssignments.TryGetValue(key, out var groupId))
            {
                var original = originalGroups.TryGetValue(key, out var value) ? value : null;
                return SkipHidden(original, key);
            }

            if (groupId.NullOrEmpty())
            {
                return null;
            }

            var group = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(groupId);
            if (group != null)
            {
                return SkipHidden(group, key);
            }

            // The group has disappeared (mod removed). We fall back to the default rather than
            // ungroup, and keep the assignment in case the mod comes back.
            if (warnedMissingGroups.Add(groupId))
            {
                Log.Warning($"[Architect Studio] Dropdown group not found: '{groupId}'. The buildings " +
                            "assigned to it revert to their original group.");
            }

            return originalGroups.TryGetValue(key, out var fallback) ? fallback : null;
        }

        /// <summary>Applies the whole configuration and rebuilds the categories affected.</summary>
        public static void Apply()
        {
            CaptureOriginals();
            EnsureCustomGroupDefs();

            var dirty = new HashSet<DesignationCategoryDef>();

            foreach (var def in AllBuildables())
            {
                var desiredGroup = DesiredGroupFor(def);
                if (def.designatorDropdown != desiredGroup)
                {
                    def.designatorDropdown = desiredGroup;
                    dirty.Add(def.designationCategory);
                }

                // A category change touches both: the one being left and the one being joined.
                var desiredCategory = DesiredCategoryFor(def, desiredGroup);
                if (desiredCategory != null && def.designationCategory != desiredCategory)
                {
                    dirty.Add(def.designationCategory);
                    def.designationCategory = desiredCategory;
                    dirty.Add(desiredCategory);
                }
            }

            if (dirty.Count == 0)
            {
                return;
            }

            foreach (var category in dirty)
            {
                RebuildCategory(category);
            }

            // Buildings may have changed category: the displayed counts are worth nothing now.
            CategoryRuntime.InvalidateCounts();
            BetterArchitectCompat.InvalidateCaches();
        }

        /// <summary>Erases every override and restores the original groups.</summary>
        public static void ResetAll()
        {
            ArchitectStudioMod.Settings.dropdownAssignments.Clear();
            ArchitectStudioMod.Settings.customGroups.Clear();
            ArchitectStudioMod.Settings.groupOrders.Clear();
            ArchitectStudioMod.Settings.hiddenGroupIds.Clear();
            ArchitectStudioMod.Settings.groupCategories.Clear();
            ArchitectStudioMod.Instance.WriteSettings();
            Apply();
        }

        /// <summary>
        /// Rebuilds every category holding at least one building from this group. Used for changes
        /// that touch no def field - typically the internal order - and which <see cref="Apply"/>
        /// would therefore never see go by.
        /// </summary>
        public static void RebuildCategoriesOf(DesignatorDropdownGroupDef group)
        {
            if (group == null)
            {
                return;
            }

            var categories = AllBuildables()
                .Where(d => d.designatorDropdown == group)
                .Select(d => d.designationCategory)
                .Distinct()
                .ToList();

            foreach (var category in categories)
            {
                RebuildCategory(category);
            }

            BetterArchitectCompat.InvalidateCaches();
        }

        /// <summary>
        /// Forces a category to rebuild its designator list. This is the only cache to purge:
        /// <c>ArchitectCategoryTab.DesignationTabOnGUI</c> re-reads <c>ResolvedAllowedDesignators</c>
        /// on every frame, so the display follows immediately.
        /// </summary>
        public static void RebuildCategory(DesignationCategoryDef category)
        {
            if (category == null)
            {
                return;
            }

            category.ResolveDesignators();
            category.DirtyCache();
            DeselectStaleDesignator();
        }

        /// <summary>
        /// A rebuild throws the old Designator objects away. If the player had one selected, it now
        /// points into the void: better to deselect it than to let it survive outside its menu.
        /// </summary>
        private static void DeselectStaleDesignator()
        {
            if (Current.ProgramState != ProgramState.Playing)
            {
                return;
            }

            var manager = Find.DesignatorManager;
            var selected = manager?.SelectedDesignator;
            if (selected is Designator_Build || selected is Designator_Dropdown)
            {
                manager.Deselect();
            }
        }
    }
}
