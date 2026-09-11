using RimWorld;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Reorders categories and their subcategories with up/down buttons. Deliberately without
    /// drag-and-drop: with a Steam Deck's pointer, aiming at a drop target is a chore, whereas a
    /// button stays a button.
    /// </summary>
    public class Dialog_Categories : Window
    {
        private const float RowHeight = 32f;
        private const float ArrowSize = 24f;
        private const float IndentWidth = 22f;
        private const float ButtonHeight = 30f;

        private Vector2 scroll;

        public override Vector2 InitialSize => new Vector2(
            Mathf.Min(620f, UI.screenWidth - 40f),
            Mathf.Min(720f, UI.screenHeight - 80f));

        public Dialog_Categories()
        {
            doCloseX = true;
            draggable = true;
            resizeable = true;
            forcePause = false;
            absorbInputAroundWindow = false;
            closeOnClickedOutside = false;
        }

        public override void DoWindowContents(Rect inRect)
        {
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;

            var y = inRect.y;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, y, inRect.width, 32f), "ArchitectStudio.Categories.Title".Translate());
            y += 34f;

            Text.Font = GameFont.Small;
            GUI.color = new Color(1f, 1f, 1f, 0.6f);
            Widgets.Label(new Rect(inRect.x, y, inRect.width, 24f), "ArchitectStudio.Categories.Intro".Translate());
            GUI.color = Color.white;
            y += 30f;

            const float bottomBarHeight = 38f;
            var listRect = new Rect(inRect.x, y, inRect.width, inRect.height - y - bottomBarHeight);

            var rows = CategoryRuntime.BuildTree();
            var viewRect = new Rect(0f, 0f, listRect.width - 16f, rows.Count * RowHeight);

            Widgets.BeginScrollView(listRect, ref scroll, viewRect);

            for (var i = 0; i < rows.Count; i++)
            {
                // A move renumbers the whole sibling set: the following rows of this frame would be
                // drawn from a stale order. We stop, and the next frame starts again from an
                // up-to-date tree.
                if (DrawRow(new Rect(0f, i * RowHeight, viewRect.width, RowHeight), rows[i]))
                {
                    break;
                }
            }

            Widgets.EndScrollView();

            DrawBottomBar(new Rect(inRect.x, listRect.yMax + 6f, inRect.width, bottomBarHeight - 6f));

            Text.Anchor = anchor;
        }

        /// <summary>Returns true if this row has just caused a move.</summary>
        private static bool DrawRow(Rect rect, CategoryRuntime.CategoryRow row)
        {
            if (Mouse.IsOver(rect))
            {
                Widgets.DrawHighlight(rect);
            }

            var indent = row.depth * IndentWidth;
            var count = CategoryRuntime.ContentCountOf(row.def);

            var countRect = new Rect(rect.xMax - 2f * ArrowSize - 52f, rect.y, 42f, rect.height);
            var iconRect = new Rect(rect.x + 4f + indent, rect.y + 4f, 24f, 24f);
            var labelRect = new Rect(iconRect.xMax + 6f, rect.y,
                countRect.x - iconRect.xMax - 12f, rect.height);

            var icon = ArchitectIconsCompat.CurrentIconFor(row.def);
            if (icon != null)
            {
                GUI.color = new Color(1f, 1f, 1f, count == 0 ? 0.4f : 1f);
                Widgets.DrawTextureFitted(iconRect, icon, 1f);
                GUI.color = Color.white;
            }

            // A category with nothing to build is greyed out: with BAM's patches, many
            // subcategories only fill up with the mods they target.
            var chosenColor = CategoryAppearance.ColorOf(row.def);
            if (count == 0)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.4f);
            }
            else if (chosenColor.HasValue)
            {
                GUI.color = chosenColor.Value;
            }
            else if (row.depth > 0)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.75f);
            }

            Widgets.Label(labelRect, row.def.LabelCap.ToString().Truncate(labelRect.width));

            GUI.color = new Color(1f, 1f, 1f, count == 0 ? 0.3f : 0.5f);
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(countRect, count.ToString());
            Text.Anchor = anchor;
            GUI.color = Color.white;

            TooltipHandler.TipRegion(rect, count == 0
                ? row.def.defName + "\n" + "ArchitectStudio.Categories.EmptyTip".Translate()
                : row.def.defName);

            // The name itself opens the appearance editor: a large target, not one more button.
            if (Widgets.ButtonInvisible(labelRect))
            {
                Find.WindowStack.Add(new Dialog_EditCategory(row.def));
            }

            var upRect = new Rect(rect.xMax - 2f * ArrowSize - 6f, rect.y + 4f, ArrowSize, ArrowSize);
            var downRect = new Rect(rect.xMax - ArrowSize - 2f, rect.y + 4f, ArrowSize, ArrowSize);

            if (ArchitectStudioWidgets.ArrowButton(upRect, up: true, CategoryRuntime.CanMove(row.def, -1)))
            {
                return CategoryRuntime.Move(row.def, -1);
            }

            if (ArchitectStudioWidgets.ArrowButton(downRect, up: false, CategoryRuntime.CanMove(row.def, 1)))
            {
                return CategoryRuntime.Move(row.def, 1);
            }

            return false;
        }

        private void DrawBottomBar(Rect rect)
        {
            if (Widgets.ButtonText(new Rect(rect.x, rect.y, 200f, ButtonHeight),
                    "ArchitectStudio.Categories.New".Translate()))
            {
                Find.WindowStack.Add(new Dialog_GroupName(null, label =>
                {
                    // Without Better Architect Menu there is no nesting: we do not ask a question
                    // whose answer would be ignored, we explain.
                    if (!BetterArchitectCompat.SubcategoriesSupported)
                    {
                        CustomCategoryRuntime.Create(label, null);
                        Messages.Message("ArchitectStudio.Categories.NoNesting".Translate(),
                            MessageTypeDefOf.CautionInput, false);
                        return;
                    }

                    // The name first, the parent afterwards: two simple questions beat one form,
                    // especially with a pointer.
                    CategoryMenu.Show("ArchitectStudio.Categories.TopLevel".Translate(),
                        parent => CustomCategoryRuntime.Create(label, parent));
                }));
            }

            if (CategoryRuntime.HasOverrides &&
                Widgets.ButtonText(new Rect(rect.xMax - 330f, rect.y, 200f, ButtonHeight),
                    "ArchitectStudio.Categories.ResetOrder".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "ArchitectStudio.Categories.ConfirmResetOrder".Translate(),
                    CategoryRuntime.ResetOrders,
                    destructive: true));
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 120f, rect.y, 120f, ButtonHeight), "CloseButton".Translate()))
            {
                Close();
            }
        }
    }
}
