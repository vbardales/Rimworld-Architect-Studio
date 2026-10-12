using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>The optional Unsorted tab: buildings no other tab provides.</summary>
    [PickleSteps]
    public class UnsortedSteps
    {
        /// <summary>Default-designator flag of each def a scenario switched off, put back after it.</summary>
        private static readonly Dictionary<BuildableDef, bool> flagsBefore = new Dictionary<BuildableDef, bool>();

        private static DesignationCategoryDef Tab => DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(UnsortedRuntime.DefName);

        /// <summary>
        /// A building the game offers no default designator for, and nothing else provides: the
        /// orphan of the request. A vanilla building is switched off for the length of one scenario
        /// and its own tab rebuilt, so that the game really stops showing it.
        /// </summary>
        [Given("the building {string} has no default designator")]
        public void NoDefaultDesignator(PickleContext ctx, string defName)
        {
            BuildableDef def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            ctx.Require(def != null && def.BuildableByPlayer, $"'{defName}' is not a building the player can build");
            if (!flagsBefore.ContainsKey(def))
            {
                flagsBefore[def] = def.canGenerateDefaultDesignator;
            }

            def.canGenerateDefaultDesignator = false;
            DropdownRuntime.InvalidateBuildablesCache();
            DropdownRuntime.RebuildCategory(def.designationCategory);
            UnsortedRuntime.InvalidateOrphans();
        }

        [AfterScenario]
        public void PutDesignatorsBack(PickleContext ctx)
        {
            foreach (var pair in flagsBefore)
            {
                pair.Key.canGenerateDefaultDesignator = pair.Value;
            }

            var categories = flagsBefore.Keys.Select(d => d.designationCategory).Distinct().ToList();
            flagsBefore.Clear();
            DropdownRuntime.InvalidateBuildablesCache();
            foreach (var category in categories)
            {
                DropdownRuntime.RebuildCategory(category);
            }

            UnsortedRuntime.InvalidateOrphans();
        }

        [When("I turn {word} the Unsorted tab")]
        public void Toggle(PickleContext ctx, string onOff)
        {
            ctx.Require(onOff == "on" || onOff == "off", $"write on or off, not '{onOff}'");
            ArchitectStudioMod.Settings.showUnsorted = onOff == "on";
            ArchitectStudioMod.Instance.WriteSettings();
            UnsortedRuntime.Apply();
        }

        [Then("the Architect menu has no Unsorted tab")]
        public void NoTab(PickleContext ctx)
        {
            ctx.Assert(Tab == null, "the Unsorted tab exists although the preference is off");
        }

        [Then("the Architect menu has an Unsorted tab")]
        public void HasTab(PickleContext ctx)
        {
            ctx.Assert(Tab != null, "the Unsorted tab is missing although the preference is on");
        }

        [Then("the Unsorted tab offers {string}")]
        public void Offers(PickleContext ctx, string defName)
        {
            ctx.Require(Tab != null, "there is no Unsorted tab");
            var offered = Tab.AllResolvedDesignators.OfType<Designator_Build>().Select(b => b.PlacingDef.defName);
            ctx.Assert(offered.Contains(defName), $"the Unsorted tab should offer '{defName}'; it offers: {string.Join(", ", offered)}");
        }

        [Then("no other tab offers {string}")]
        public void NoOtherTab(PickleContext ctx, string defName)
        {
            var holders = DefDatabase<DesignationCategoryDef>.AllDefsListForReading
                .Where(c => !UnsortedRuntime.IsUnsorted(c))
                .Where(c => c.AllResolvedDesignators
                    .SelectMany(d => d is Designator_Dropdown dd ? dd.Elements : new List<Designator> { d })
                    .OfType<Designator_Build>().Any(b => b.PlacingDef.defName == defName))
                .Select(c => c.defName).ToList();
            ctx.Assert(holders.Count == 0, $"'{defName}' is also offered by: {string.Join(", ", holders)}");
        }

        [Then("the category editor counts as many buildings in the Unsorted tab as it offers")]
        public void CountMatches(PickleContext ctx)
        {
            ctx.Require(Tab != null, "there is no Unsorted tab");
            var offered = Tab.AllResolvedDesignators.OfType<Designator_Build>().Count();
            var counted = CategoryRuntime.ContentCountOf(Tab);
            ctx.Assert(offered > 0 && counted == offered, $"the editor counts {counted}; the tab offers {offered}");
        }

        [Then("the Unsorted tab does not offer {string}")]
        public void DoesNotOffer(PickleContext ctx, string defName)
        {
            ctx.Require(Tab != null, "there is no Unsorted tab");
            var offered = Tab.AllResolvedDesignators.OfType<Designator_Build>().Select(b => b.PlacingDef.defName);
            ctx.Assert(!offered.Contains(defName), $"the Unsorted tab offers '{defName}', which another tab already provides");
        }
    }
}
