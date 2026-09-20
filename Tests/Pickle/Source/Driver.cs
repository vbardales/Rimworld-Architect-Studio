using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>
    /// Drives the two editors the way their buttons do. The mutations live in private methods of
    /// the dialogs, so they are called by name: a renamed method fails the step saying which one,
    /// instead of the suite testing a copy of the logic that the mod no longer runs.
    /// </summary>
    public static class Driver
    {
        private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // ---------------------------------------------------------------- lookups

        /// <summary>
        /// Letters a scenario gives the buildings it picked from the running game, mapped to their
        /// defNames. A scenario that needs four buildings of one category cannot name vanilla ones:
        /// Better Architect Menu alone spreads the vanilla chairs over two subcategories.
        /// </summary>
        public sealed class Aliases
        {
            public readonly Dictionary<string, BuildableDef> ByName = new Dictionary<string, BuildableDef>();
        }

        private static Aliases AliasesOf(PickleContext ctx)
        {
            try
            {
                return ctx.Get<Aliases>();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>The defName behind a name used in a scenario: an alias if one was given, the name itself otherwise.</summary>
        public static string DefName(PickleContext ctx, string name) =>
            AliasesOf(ctx)?.ByName.TryGetValue(name, out var def) == true ? def.defName : name;

        /// <summary>How a scenario calls a def: its alias when it has one.</summary>
        public static string NameOf(PickleContext ctx, BuildableDef def) =>
            AliasesOf(ctx)?.ByName.FirstOrDefault(p => p.Value == def).Key ?? def.defName;

        public static BuildableDef Buildable(PickleContext ctx, string name)
        {
            var defName = DefName(ctx, name);
            BuildableDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            def ??= DefDatabase<TerrainDef>.GetNamedSilentFail(defName);
            ctx.Require(def != null, $"no building or floor is named '{defName}'");
            ctx.Require(DropdownRuntime.AllBuildables().Contains(def),
                $"'{defName}' exists but the Architect menu cannot show it, so no group can hold it");
            return def;
        }

        public static DesignationCategoryDef Category(PickleContext ctx, string defName)
        {
            var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(defName);

            // A category created by a scenario is named by its label: its defName depends on what
            // the player created before.
            var created = ArchitectStudioMod.Settings.customCategories.LastOrDefault(e => e.label == defName);
            def ??= created != null ? DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(created.id) : null;

            ctx.Require(def != null, $"no Architect category is named or labelled '{defName}'; known: " +
                string.Join(", ", DefDatabase<DesignationCategoryDef>.AllDefsListForReading.Select(c => c.defName)));
            return def;
        }

        /// <summary>Scenarios name their groups by label: the defName a created group gets depends on what exists already.</summary>
        public static DesignatorDropdownGroupDef Group(PickleContext ctx, string labelOrDefName)
        {
            var groups = DefDatabase<DesignatorDropdownGroupDef>.AllDefsListForReading;
            var def = groups.FirstOrDefault(g => g.defName == labelOrDefName) ??
                      groups.LastOrDefault(g => g.label == labelOrDefName);
            ctx.Require(def != null, $"no dropdown group is labelled or named '{labelOrDefName}'");
            return def;
        }

        public static DesignationCategoryDef CreatedCategory(PickleContext ctx, string label)
        {
            var entry = ArchitectStudioMod.Settings.customCategories.LastOrDefault(e => e.label == label);
            ctx.Require(entry != null, $"no created category is labelled '{label}'");
            return Category(ctx, entry.id);
        }

        public static List<BuildableDef> MembersOf(DesignatorDropdownGroupDef group)
        {
            var members = DropdownRuntime.AllBuildables().Where(d => d.designatorDropdown == group).ToList();
            return DropdownOrderRuntime.SortMembers(group.defName, members);
        }

        public static string Names(PickleContext ctx, IEnumerable<BuildableDef> defs) =>
            "[" + string.Join(", ", defs.Select(d => NameOf(ctx, d))) + "]";

        // ---------------------------------------------------------------- the Architect menu as drawn

        /// <summary>
        /// Every button the category's tab would draw that holds a member of the group. This is the
        /// list the Architect menu iterates, so a count of two is the "split group" players see.
        /// </summary>
        public static List<Designator_Dropdown> MenuButtonsOf(DesignatorDropdownGroupDef group)
        {
            return DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .SelectMany(c => c.AllResolvedDesignators)
                .OfType<Designator_Dropdown>()
                .Where(d => d.Elements.OfType<Designator_Build>().Any(b => b.PlacingDef.designatorDropdown == group))
                .ToList();
        }

        public static List<BuildableDef> MenuOrderOf(Designator_Dropdown button)
        {
            return button.Elements.OfType<Designator_Build>().Select(b => b.PlacingDef).ToList();
        }

        /// <summary>Whether the category's tab draws the building as a button of its own.</summary>
        public static bool IsStandaloneIn(BuildableDef def, DesignationCategoryDef category)
        {
            return category.AllResolvedDesignators.OfType<Designator_Build>().Any(b => b.PlacingDef == def);
        }

        /// <summary>Tabs in the order the Architect window lays them out, after it rebuilds its cache.</summary>
        public static List<DesignationCategoryDef> ArchitectTabs()
        {
            var window = MainButtonDefOf.Architect.TabWindow as MainTabWindow_Architect;
            if (window == null)
            {
                return DefDatabase<DesignationCategoryDef>.AllDefs.OrderByDescending(c => c.order).ToList();
            }

            typeof(MainTabWindow_Architect).GetMethod("CacheDesPanels", Instance).Invoke(window, null);
            var panels = (List<ArchitectCategoryTab>)typeof(MainTabWindow_Architect)
                .GetField("desPanelsCached", Instance).GetValue(window);
            return panels.Select(p => p.def).ToList();
        }

        // ---------------------------------------------------------------- the group editor

        public static Dialog_DropdownGroups GroupEditor(PickleContext ctx)
        {
            var dialog = Find.WindowStack.WindowOfType<Dialog_DropdownGroups>();
            if (dialog == null)
            {
                ArchitectStudioUI.ToggleDropdownDialog();
                dialog = Find.WindowStack.WindowOfType<Dialog_DropdownGroups>();
            }

            ctx.Require(dialog != null, "the group editor did not open");
            return dialog;
        }

        public static object Call(PickleContext ctx, object target, string method, params object[] args)
        {
            var info = target.GetType().GetMethod(method, Instance);
            ctx.Require(info != null,
                $"{target.GetType().Name}.{method} no longer exists: the scenario drives the editor through it, update the steps");
            try
            {
                return info.Invoke(target, args);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public static void Select(PickleContext ctx, Dialog_DropdownGroups dialog, DesignatorDropdownGroupDef group)
        {
            var field = typeof(Dialog_DropdownGroups).GetField("selectedGroup", Instance);
            ctx.Require(field != null, "Dialog_DropdownGroups.selectedGroup no longer exists: update the steps");
            field.SetValue(dialog, group);
        }

        public static int MemberReorderGroup(Dialog_DropdownGroups dialog)
        {
            return (int)typeof(Dialog_DropdownGroups).GetField("memberReorderGroup", Instance).GetValue(dialog);
        }
    }

    /// <summary>
    /// Keeps the callback each <c>ReorderableWidget.NewGroup</c> registers. The game clears its
    /// groups at the end of every repaint, so this is the only way to reach the lambda the editor
    /// hands over - and that lambda holds the insertion-index conversion that has already been wrong
    /// once. Replaying a drag through it tests the mod's conversion, not a copy of it.
    /// </summary>
    [HarmonyPatch(typeof(ReorderableWidget), nameof(ReorderableWidget.NewGroup))]
    public static class ReorderCapture
    {
        public static readonly Dictionary<int, Action<int, int>> Actions = new Dictionary<int, Action<int, int>>();

        public static void Postfix(int __result, Action<int, int> reorderedAction)
        {
            if (__result >= 0)
            {
                Actions[__result] = reorderedAction;
            }
        }

        private static bool patched;

        public static void Ensure()
        {
            if (patched)
            {
                return;
            }

            new Harmony("nelim.architectstudio.pickletests").CreateClassProcessor(typeof(ReorderCapture)).Patch();
            patched = true;
        }
    }
}
