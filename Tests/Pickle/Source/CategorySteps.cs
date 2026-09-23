using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RimWorld;
using RimWorks.Pickle;
using UnityEngine;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>Categories: order, creation, appearance.</summary>
    [PickleSteps]
    public class CategorySteps
    {
        /// <summary>
        /// Sibling lists, not the flat tab list: with Better Architect Menu a subcategory is not a
        /// top-level tab, and the arrows move a category among its siblings only.
        /// </summary>
        private sealed class RememberedOrder
        {
            public string Of;
            public List<string> Siblings;
            public List<string> Tabs;
        }

        private static List<string> SiblingNames(DesignationCategoryDef category) =>
            CategoryRuntime.SiblingsOf(category).Select(c => c.defName).ToList();

        [When("I remember where {string} sits among its siblings")]
        public void Remember(PickleContext ctx, string category)
        {
            var def = Driver.Category(ctx, category);
            ctx.Set(new RememberedOrder
            {
                Of = def.defName,
                Siblings = SiblingNames(def),
                Tabs = Driver.ArchitectTabs().Select(c => c.defName).ToList()
            });
        }

        [When("I move the category {string} {word}")]
        public void Move(PickleContext ctx, string category, string direction)
        {
            var delta = direction == "up" ? -1 : direction == "down" ? 1 : 0;
            ctx.Require(delta != 0, $"direction must be 'up' or 'down', not '{direction}'");
            var def = Driver.Category(ctx, category);
            ctx.Require(CategoryRuntime.CanMove(def, delta),
                $"'{category}' cannot move {direction}, its arrow is disabled; siblings: [{string.Join(", ", SiblingNames(def))}]");
            ctx.Assert(CategoryRuntime.Move(def, delta), $"moving '{category}' {direction} reported no change");
        }

        [Then("the category {string} can not move {word}")]
        public void CannotMove(PickleContext ctx, string category, string direction)
        {
            var delta = direction == "up" ? -1 : 1;
            var def = Driver.Category(ctx, category);
            ctx.Assert(!CategoryRuntime.CanMove(def, delta),
                $"'{category}' should be at the end of its list; siblings: [{string.Join(", ", SiblingNames(def))}]");
        }

        [Then("it comes {int} place(s) {word} among its siblings")]
        public void MovedBy(PickleContext ctx, int places, string direction)
        {
            var remembered = ctx.Get<RememberedOrder>();
            var after = SiblingNames(Driver.Category(ctx, remembered.Of));
            var expected = remembered.Siblings.ToList();
            var index = expected.IndexOf(remembered.Of);
            var target = index + (direction == "earlier" ? -places : places);
            ctx.Require(target >= 0 && target < expected.Count, $"'{remembered.Of}' has no room to move {places} {direction}");
            expected.RemoveAt(index);
            expected.Insert(target, remembered.Of);

            // The whole list is compared: a renumbering that moves the right row but disturbs its
            // neighbours fails here too.
            ctx.Assert(after.SequenceEqual(expected),
                $"siblings should be [{string.Join(", ", expected)}]; they are [{string.Join(", ", after)}]");
        }

        [Then("the Architect window draws the siblings in that order")]
        public void WindowAgrees(PickleContext ctx)
        {
            var remembered = ctx.Get<RememberedOrder>();
            var siblings = SiblingNames(Driver.Category(ctx, remembered.Of));
            var tabs = Driver.ArchitectTabs().Select(c => c.defName).Where(siblings.Contains).ToList();
            ctx.Assert(tabs.SequenceEqual(siblings),
                $"the window's tab cache orders them [{string.Join(", ", tabs)}], the editor [{string.Join(", ", siblings)}]");
        }

        // ---------------------------------------------------------------- created categories

        [When("I create the category {string}")]
        public void Create(PickleContext ctx, string label)
        {
            var def = CustomCategoryRuntime.Create(label, null);
            ctx.Assert(def != null, $"creating '{label}' returned no def");
        }

        [When("I delete the category {string}")]
        public void Delete(PickleContext ctx, string label)
        {
            CustomCategoryRuntime.Delete(Driver.CreatedCategory(ctx, label));
        }

        [Then("the Architect menu has a tab labelled {string}")]
        public void HasTab(PickleContext ctx, string label)
        {
            var tabs = Driver.ArchitectTabs();
            ctx.Assert(tabs.Any(c => c.label == label),
                $"no tab is labelled '{label}'; tabs: {string.Join(", ", tabs.Select(c => c.label))}");
        }

        [Then("the Architect menu has no tab labelled {string}")]
        public void HasNoTab(PickleContext ctx, string label)
        {
            var tabs = Driver.ArchitectTabs();
            ctx.Assert(tabs.All(c => c.label != label), $"a tab labelled '{label}' is still there");
        }

        [Then("the category {string} has a key binding category")]
        public void HasBindingCategory(PickleContext ctx, string label)
        {
            var def = Driver.CreatedCategory(ctx, label);
            ctx.Assert(def.bindingCatDef != null, $"'{def.defName}'.bindingCatDef is null: the keyboard configuration will read null");
            ctx.Assert(DefDatabase<KeyBindingCategoryDef>.AllDefsListForReading.Contains(def.bindingCatDef),
                $"'{def.bindingCatDef.defName}' is set on the category but missing from the def database");
            var expectedLabel = "ArchitectStudio.KeyBindings.CategoryLabel".Translate(def.LabelCap).ToString();
            var expectedDescription = "ArchitectStudio.KeyBindings.CategoryDescription".Translate(def.LabelCap).ToString();
            ctx.Assert(def.bindingCatDef.label == expectedLabel,
                $"'{def.bindingCatDef.defName}' label is '{def.bindingCatDef.label}', expected '{expectedLabel}'");
            ctx.Assert(def.bindingCatDef.description == expectedDescription,
                $"'{def.bindingCatDef.defName}' description is '{def.bindingCatDef.description}', expected '{expectedDescription}'");
        }

        [When("I open the keyboard configuration and let it draw")]
        public async Task OpenKeyBindings(PickleContext ctx)
        {
            Find.WindowStack.Add(new Dialog_KeyBindings());
            // Drawing is where a null category used to throw; a few frames cover layout and repaint.
            await ctx.WaitFrames(10);
        }

        // ---------------------------------------------------------------- appearance

        [When("I relabel the category {string} as {string}")]
        public void Relabel(PickleContext ctx, string defName, string label)
        {
            CategoryAppearance.SetLabel(Driver.Category(ctx, defName), label);
        }

        [When("I colour the category {string} with palette colour {int}")]
        public void Colour(PickleContext ctx, string defName, int index)
        {
            ctx.Require(index >= 1 && index <= CategoryAppearance.Palette.Length,
                $"the palette has {CategoryAppearance.Palette.Length} colours");
            CategoryAppearance.SetColor(Driver.Category(ctx, defName), CategoryAppearance.Palette[index - 1]);
        }

        private sealed class ChosenIcon
        {
            public string Path;
        }

        /// <summary>The picker's own list, so the scenario never names a texture some mod list lacks.</summary>
        [When("I give the category {string} the first icon the picker offers")]
        public void Icon(PickleContext ctx, string defName)
        {
            ctx.Require(ArchitectIconsCompat.Available, "icons need Architect Icons, which is not loaded");
            var category = Driver.Category(ctx, defName);
            var current = ArchitectIconsCompat.CurrentIconFor(category);
            var path = ArchitectIconsCompat.AllIconPaths()
                .FirstOrDefault(p => ArchitectIconsCompat.TextureFor(p) != null && ArchitectIconsCompat.TextureFor(p) != current);
            ctx.Require(path != null, "the icon picker offers no icon different from the current one");
            ctx.Set(new ChosenIcon { Path = path });
            CategoryAppearance.SetIcon(category, path);
        }

        [When("I open the category editor")]
        public void OpenCategoryEditor(PickleContext ctx)
        {
            if (Find.WindowStack.WindowOfType<Dialog_Categories>() == null)
            {
                ArchitectStudioUI.ToggleCategoriesDialog();
            }
        }

        [Then("Architect Icons returns that icon for {string} straight away")]
        public void IconShown(PickleContext ctx, string defName)
        {
            var texturePath = ctx.Get<ChosenIcon>().Path;
            var category = Driver.Category(ctx, defName);
            var resources = HarmonyLib.AccessTools.TypeByName("ArchitectIcons.Resources");
            ctx.Require(resources != null, "ArchitectIcons.Resources not found");
            var getter = HarmonyLib.AccessTools.Method(resources, "FindArchitectTabCategoryIcon", new[] { typeof(string) });
            ctx.Require(getter != null,
                "ArchitectIcons.Resources.FindArchitectTabCategoryIcon not found: the scenario reads the icon the way the menu does, update the step");
            var expected = ContentFinder<Texture2D>.Get(texturePath, reportFailure: false);
            ctx.Require(expected != null, $"no texture is loaded at '{texturePath}'");

            // Read the way the mod reads it for its own drawing, so the colour painter is not armed.
            CategoryColorPainter.Suppressed = true;
            Texture2D actual;
            try
            {
                actual = getter.Invoke(null, new object[] { category.defName }) as Texture2D;
            }
            finally
            {
                CategoryColorPainter.Suppressed = false;
                CategoryColorPainter.Disarm();
            }

            ctx.Assert(actual == expected,
                $"Architect Icons should return '{texturePath}' now, not after a restart; it returns '{actual?.name ?? "null"}'");

            // The lookup above is answered by the mod's prefix whatever the cache holds, so the
            // eviction is checked on the cache itself: a stale entry is what a restart would clear.
            if (HarmonyLib.AccessTools.Field(resources, "iconsCache")?.GetValue(null) is System.Collections.Generic.Dictionary<string, Texture2D> cache &&
                cache.TryGetValue(category.defName, out var cached))
            {
                ctx.Assert(cached == expected,
                    $"Architect Icons still caches '{cached?.name ?? "null"}' for '{category.defName}': the eviction missed");
            }
        }
    }
}
