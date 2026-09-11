using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Category picker menu. Subcategories are nested under their parent when Float Sub-Menus is
    /// present, and listed flat otherwise.
    /// </summary>
    public static class CategoryMenu
    {
        /// <param name="noneLabel">Leading "none" entry, or null not to offer it.</param>
        public static void Show(string noneLabel, Action<DesignationCategoryDef> onPick)
        {
            var options = new List<FloatMenuOption>();

            if (noneLabel != null)
            {
                options.Add(new FloatMenuOption(noneLabel, () => onPick(null)));
            }

            var all = DefDatabase<DesignationCategoryDef>.AllDefsListForReading;

            var childrenByParent = new Dictionary<DesignationCategoryDef, List<DesignationCategoryDef>>();
            var roots = new List<DesignationCategoryDef>();

            foreach (var category in all)
            {
                var parent = BetterArchitectCompat.ParentCategoryOf(category);
                if (parent == null)
                {
                    roots.Add(category);
                    continue;
                }

                if (!childrenByParent.TryGetValue(parent, out var children))
                {
                    childrenByParent[parent] = children = new List<DesignationCategoryDef>();
                }

                children.Add(category);
            }

            foreach (var root in roots.OrderByDescending(c => c.order).ThenBy(c => c.LabelCap.ToString()))
            {
                var label = root.LabelCap.ToString();

                if (!childrenByParent.TryGetValue(root, out var children) || children.Count == 0)
                {
                    options.Add(new FloatMenuOption(label, () => onPick(root)));
                    continue;
                }

                var ordered = children.OrderByDescending(c => c.order).ThenBy(c => c.LabelCap.ToString());

                // The parent stays selectable: it can hold buildings of its own.
                var subOptions = new List<FloatMenuOption> { new FloatMenuOption(label, () => onPick(root)) };
                foreach (var child in ordered)
                {
                    var captured = child;
                    subOptions.Add(new FloatMenuOption(captured.LabelCap, () => onPick(captured)));
                }

                var subMenu = FloatSubMenuCompat.TryCreateSubMenu(label, subOptions);
                if (subMenu != null)
                {
                    options.Add(subMenu);
                    continue;
                }

                // Without Float Sub-Menus: everything flat, the parent prefixing its children so
                // that two subcategories sharing a label stay distinguishable.
                options.Add(new FloatMenuOption(label, () => onPick(root)));
                foreach (var child in ordered)
                {
                    var captured = child;
                    options.Add(new FloatMenuOption(
                        "ArchitectStudio.Common.CategoryPath".Translate(label, captured.LabelCap),
                        () => onPick(captured)));
                }
            }

            if (options.Count == 0)
            {
                return;
            }

            Find.WindowStack.Add(new FloatMenu(options));
        }
    }
}
