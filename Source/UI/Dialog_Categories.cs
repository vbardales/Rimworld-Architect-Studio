using RimWorld;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Reordonne les categories et leurs sous-categories avec des boutons haut/bas. Volontairement
    /// sans glisser-deposer : au pointeur d'un Steam Deck, viser une cible de depot est penible,
    /// alors qu'un bouton reste un bouton.
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
                // Un deplacement renumerote toute la fratrie : les lignes suivantes de cette frame
                // seraient dessinees a partir d'un ordre perime. On s'arrete, la frame suivante
                // repart d'un arbre a jour.
                if (DrawRow(new Rect(0f, i * RowHeight, viewRect.width, RowHeight), rows[i]))
                {
                    break;
                }
            }

            Widgets.EndScrollView();

            DrawBottomBar(new Rect(inRect.x, listRect.yMax + 6f, inRect.width, bottomBarHeight - 6f));

            Text.Anchor = anchor;
        }

        /// <summary>Renvoie vrai si cette ligne vient de provoquer un deplacement.</summary>
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

            // Une categorie sans rien a construire est grisee : avec les patches de BAM, beaucoup de
            // sous-categories ne se remplissent qu'avec les mods qu'elles ciblent.
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

            // Le nom lui-meme ouvre l'editeur d'apparence : une grande cible, pas un bouton de plus.
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
                    // Sans Better Architect Menu, l'imbrication n'existe pas : on ne pose pas une
                    // question dont la reponse serait ignoree, on explique.
                    if (!BetterArchitectCompat.SubcategoriesSupported)
                    {
                        CustomCategoryRuntime.Create(label, null);
                        Messages.Message("ArchitectStudio.Categories.NoNesting".Translate(),
                            MessageTypeDefOf.CautionInput, false);
                        return;
                    }

                    // Le nom d'abord, la parente ensuite : deux questions simples valent mieux qu'un
                    // formulaire, surtout au pointeur.
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
