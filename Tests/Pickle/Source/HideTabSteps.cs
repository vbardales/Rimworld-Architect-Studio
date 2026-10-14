using System.Linq;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>Hiding a whole Architect tab: the tab goes, its buildings stay in the game.</summary>
    [PickleSteps]
    public class HideTabSteps
    {
        private static DesignationCategoryDef Category(PickleContext ctx, string defName)
        {
            var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(defName);
            ctx.Require(def != null, $"there is no Architect tab '{defName}'");
            return def;
        }

        [When("I hide the Architect tab {string}")]
        public void Hide(PickleContext ctx, string defName)
        {
            CategoryVisibilityRuntime.Set(Category(ctx, defName), true);
        }

        [When("I show the Architect tab {string} again")]
        public void Show(PickleContext ctx, string defName)
        {
            CategoryVisibilityRuntime.Set(Category(ctx, defName), false);
        }

        [Then("the Architect menu shows the tab {string}")]
        public void Shows(PickleContext ctx, string defName)
        {
            ctx.Assert(Category(ctx, defName).Visible, $"the tab '{defName}' should be shown");
        }

        [Then("the Architect menu does not show the tab {string}")]
        public void DoesNotShow(PickleContext ctx, string defName)
        {
            ctx.Assert(!Category(ctx, defName).Visible, $"the tab '{defName}' should be hidden");
        }

        [Then("the building {string} still exists and is buildable by the player")]
        public void StillBuildable(PickleContext ctx, string defName)
        {
            var def = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            ctx.Assert(def != null && def.BuildableByPlayer, $"'{defName}' should remain a building the player can build");
        }

        [Then("the editor lists the tab {string} as hidden")]
        public void ListedHidden(PickleContext ctx, string defName)
        {
            ctx.Assert(CategoryVisibilityRuntime.IsHidden(Category(ctx, defName)), $"the editor should list '{defName}' as hidden");
        }

        [Then("the Unsorted tab does not offer the buildings of the tab {string}")]
        public void UnsortedLacksBuildings(PickleContext ctx, string defName)
        {
            var tab = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(UnsortedRuntime.DefName);
            ctx.Require(tab != null, "there is no Unsorted tab");
            var offered = tab.AllResolvedDesignators.OfType<RimWorld.Designator_Build>().Select(b => b.PlacingDef.defName);
            var own = Category(ctx, defName).AllResolvedDesignators.OfType<RimWorld.Designator_Build>().Select(b => b.PlacingDef.defName);
            ctx.Assert(!offered.Intersect(own).Any(), "the Unsorted tab offers buildings of the shown tab");
        }
    }
}
