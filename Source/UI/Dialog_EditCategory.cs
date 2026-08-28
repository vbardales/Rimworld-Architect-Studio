using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Libelle, couleur et icone d'une categorie. Tout se choisit au pointeur - palette de couleurs
    /// et grille d'icones - pour rester utilisable sans clavier.
    /// </summary>
    public class Dialog_EditCategory : Window
    {
        private const float SwatchSize = 30f;
        private const float IconSize = 40f;
        private const float RowHeight = 30f;

        private readonly DesignationCategoryDef category;

        private string labelBuffer;
        private string iconSearch = "";
        private Vector2 iconScroll;

        public override Vector2 InitialSize => new Vector2(
            Mathf.Min(660f, UI.screenWidth - 40f),
            Mathf.Min(680f, UI.screenHeight - 80f));

        public Dialog_EditCategory(DesignationCategoryDef category)
        {
            this.category = category;
            labelBuffer = category.LabelCap;

            doCloseX = true;
            draggable = true;
            resizeable = true;
            absorbInputAroundWindow = true;
            closeOnClickedOutside = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;

            var y = inRect.y;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, y, inRect.width, 32f),
                "ArchitectStudio.EditCategory.Title".Translate(CategoryAppearance.OriginalLabelOf(category)));
            y += 36f;
            Text.Font = GameFont.Small;

            y = DrawLabelRow(inRect, y);

            if (CustomCategoryRuntime.IsCustom(category))
            {
                y = DrawParentRow(inRect, y);
            }

            y = DrawColorRow(inRect, y);
            y = DrawIconSection(inRect, y);

            DrawBottomBar(new Rect(inRect.x, inRect.yMax - 32f, inRect.width, 30f));

            Text.Anchor = anchor;
        }

        private float DrawLabelRow(Rect inRect, float y)
        {
            Widgets.Label(new Rect(inRect.x, y, 90f, 28f), "ArchitectStudio.EditCategory.Label".Translate());

            var fieldRect = new Rect(inRect.x + 94f, y, inRect.width - 94f - 110f, 28f);
            labelBuffer = Widgets.TextField(fieldRect, labelBuffer);

            if (Widgets.ButtonText(new Rect(fieldRect.xMax + 6f, y, 100f, 28f),
                    "ArchitectStudio.EditCategory.Apply".Translate()))
            {
                CategoryAppearance.SetLabel(category, labelBuffer);
            }

            return y + 34f;
        }

        /// <summary>Parente, pour les seules categories qu'on a creees.</summary>
        private float DrawParentRow(Rect inRect, float y)
        {
            Widgets.Label(new Rect(inRect.x, y, 90f, 28f), "ArchitectStudio.EditCategory.Parent".Translate());

            if (!BetterArchitectCompat.SubcategoriesSupported)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.6f);
                Widgets.Label(new Rect(inRect.x + 94f, y, inRect.width - 94f, 28f),
                    "ArchitectStudio.Categories.NoNesting".Translate());
                GUI.color = Color.white;
                return y + 34f;
            }

            var parent = BetterArchitectCompat.ParentCategoryOf(category);
            var label = parent != null
                ? parent.LabelCap.ToString()
                : "ArchitectStudio.Categories.TopLevel".Translate().ToString();

            if (Widgets.ButtonText(new Rect(inRect.x + 94f, y, inRect.width - 94f, 28f), label))
            {
                CategoryMenu.Show("ArchitectStudio.Categories.TopLevel".Translate(), newParent =>
                {
                    // Une categorie ne peut pas etre sa propre parente.
                    if (newParent != category)
                    {
                        CustomCategoryRuntime.SetParent(category, newParent);
                    }
                });
            }

            return y + 34f;
        }

        private float DrawColorRow(Rect inRect, float y)
        {
            Widgets.Label(new Rect(inRect.x, y, 90f, 28f), "ArchitectStudio.EditCategory.Color".Translate());

            var current = CategoryAppearance.ColorOf(category);
            var x = inRect.x + 94f;

            foreach (var color in CategoryAppearance.Palette)
            {
                var rect = new Rect(x, y, SwatchSize, SwatchSize);
                x += SwatchSize + 4f;

                Widgets.DrawBoxSolid(rect, color);

                if (current.HasValue && ApproximatelyEqual(current.Value, color))
                {
                    Widgets.DrawBox(rect.ExpandedBy(2f), 2);
                }
                else if (Mouse.IsOver(rect))
                {
                    Widgets.DrawBox(rect.ExpandedBy(1f));
                }

                if (Widgets.ButtonInvisible(rect))
                {
                    CategoryAppearance.SetColor(category, color);
                }
            }

            var noneRect = new Rect(x + 4f, y, 90f, SwatchSize);
            if (Widgets.ButtonText(noneRect, "ArchitectStudio.EditCategory.NoColor".Translate()))
            {
                CategoryAppearance.SetColor(category, null);
            }

            return y + SwatchSize + 10f;
        }

        private float DrawIconSection(Rect inRect, float y)
        {
            Widgets.Label(new Rect(inRect.x, y, 90f, 28f), "ArchitectStudio.EditCategory.Icon".Translate());

            if (!ArchitectIconsCompat.Available)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.6f);
                Widgets.Label(new Rect(inRect.x + 94f, y, inRect.width - 94f, 28f),
                    "ArchitectStudio.EditCategory.NoArchitectIcons".Translate());
                GUI.color = Color.white;
                return y + 34f;
            }

            var current = ArchitectIconsCompat.CurrentIconFor(category);
            if (current != null)
            {
                Widgets.DrawTextureFitted(new Rect(inRect.x + 94f, y - 2f, 32f, 32f), current, 1f);
            }

            if (Widgets.ButtonText(new Rect(inRect.x + 134f, y, 120f, 28f),
                    "ArchitectStudio.EditCategory.DefaultIcon".Translate()))
            {
                CategoryAppearance.SetIcon(category, null);
            }

            iconSearch = Widgets.TextField(new Rect(inRect.x + 262f, y, inRect.width - 262f, 28f), iconSearch);
            y += 34f;

            var paths = ArchitectIconsCompat.AllIconPaths();
            if (!iconSearch.NullOrEmpty())
            {
                paths = paths
                    .Where(p => p.IndexOf(iconSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            var gridRect = new Rect(inRect.x, y, inRect.width, inRect.yMax - y - 40f);
            var perRow = Mathf.Max(1, Mathf.FloorToInt((gridRect.width - 16f) / (IconSize + 4f)));
            var rowCount = Mathf.CeilToInt(paths.Count / (float)perRow);
            var viewRect = new Rect(0f, 0f, gridRect.width - 16f, rowCount * (IconSize + 4f));

            Widgets.BeginScrollView(gridRect, ref iconScroll, viewRect);

            var chosen = CategoryAppearance.IconPathOf(category);

            for (var i = 0; i < paths.Count; i++)
            {
                var cell = new Rect(
                    i % perRow * (IconSize + 4f),
                    i / perRow * (IconSize + 4f),
                    IconSize, IconSize);

                var texture = ArchitectIconsCompat.TextureFor(paths[i]);
                if (texture == null)
                {
                    continue;
                }

                if (paths[i] == chosen)
                {
                    Widgets.DrawHighlightSelected(cell);
                }
                else if (Mouse.IsOver(cell))
                {
                    Widgets.DrawHighlight(cell);
                }

                Widgets.DrawTextureFitted(cell.ContractedBy(4f), texture, 1f);
                TooltipHandler.TipRegion(cell, paths[i]);

                if (Widgets.ButtonInvisible(cell))
                {
                    CategoryAppearance.SetIcon(category, paths[i]);
                }
            }

            Widgets.EndScrollView();

            return gridRect.yMax;
        }

        private void DrawBottomBar(Rect rect)
        {
            if (CustomCategoryRuntime.IsCustom(category) &&
                Widgets.ButtonText(new Rect(rect.x, rect.y, 220f, rect.height),
                    "ArchitectStudio.EditCategory.Delete".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "ArchitectStudio.EditCategory.ConfirmDelete".Translate(category.LabelCap),
                    delegate
                    {
                        CustomCategoryRuntime.Delete(category);
                        Close();
                    },
                    destructive: true));
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 260f, rect.y, 130f, rect.height),
                    "ArchitectStudio.EditCategory.ResetThis".Translate()))
            {
                CategoryAppearance.SetLabel(category, null);
                CategoryAppearance.SetColor(category, null);
                CategoryAppearance.SetIcon(category, null);
                labelBuffer = CategoryAppearance.OriginalLabelOf(category);
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 120f, rect.y, 120f, rect.height), "CloseButton".Translate()))
            {
                Close();
            }
        }

        private static bool ApproximatelyEqual(Color a, Color b)
        {
            return Mathf.Abs(a.r - b.r) < 0.01f && Mathf.Abs(a.g - b.g) < 0.01f && Mathf.Abs(a.b - b.b) < 0.01f;
        }
    }
}
