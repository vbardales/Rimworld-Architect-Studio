using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Menu de choix d'une categorie. Les sous-categories sont imbriquees sous leur parente quand
    /// Float Sub-Menus est present, listees a plat sinon.
    /// </summary>
    public static class CategoryMenu
    {
        /// <param name="noneLabel">Entree "aucune" en tete, ou null pour ne pas la proposer.</param>
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

                // La parente reste selectionnable : elle peut contenir des batiments en propre.
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

                // Sans Float Sub-Menus : tout a plat, la parente prefixant ses enfants pour que deux
                // sous-categories homonymes restent distinguables.
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
