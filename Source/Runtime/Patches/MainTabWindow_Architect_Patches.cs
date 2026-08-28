using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Ajoute un bouton d'ouverture directement dans la fenetre Architecte. Indispensable sans
    /// clavier - sur Steam Deck notamment - ou le raccourci n'est pas accessible.
    /// </summary>
    public static class ArchitectStudioButton
    {
        public const float RowHeight = 26f;

        public static bool Enabled => ArchitectStudioMod.Settings?.showArchitectButton ?? false;

        public static void Draw(Rect inRect)
        {
            if (!Enabled)
            {
                return;
            }

            var row = new Rect(inRect.x + 1f, inRect.yMax - RowHeight + 1f, inRect.width - 2f, RowHeight - 3f);
            var half = (row.width - 3f) / 2f;

            var font = Text.Font;
            Text.Font = GameFont.Tiny;

            var groupsRect = new Rect(row.x, row.y, half, row.height);
            TooltipHandler.TipRegion(groupsRect, "ArchitectStudio.ArchitectButtonTip".Translate());
            if (Widgets.ButtonText(groupsRect, "ArchitectStudio.ArchitectButton".Translate()))
            {
                ArchitectStudioUI.ToggleDropdownDialog();
            }

            var categoriesRect = new Rect(row.x + half + 3f, row.y, half, row.height);
            TooltipHandler.TipRegion(categoriesRect, "ArchitectStudio.ArchitectButtonCategoriesTip".Translate());
            if (Widgets.ButtonText(categoriesRect, "ArchitectStudio.ArchitectButtonCategories".Translate()))
            {
                ArchitectStudioUI.ToggleCategoriesDialog();
            }

            Text.Font = font;
        }
    }

    /// <summary>
    /// Reserve la hauteur du bouton. Tout le reste de la fenetre - position du panneau d'info,
    /// haut du volet - derive de WinHeight, donc l'ajustement se propage tout seul.
    /// </summary>
    [HarmonyPatch(typeof(MainTabWindow_Architect), nameof(MainTabWindow_Architect.WinHeight), MethodType.Getter)]
    public static class MainTabWindow_Architect_WinHeight_Patch
    {
        public static void Postfix(ref float __result)
        {
            if (ArchitectStudioButton.Enabled)
            {
                __result += ArchitectStudioButton.RowHeight;
            }
        }
    }

    /// <summary>
    /// Dessine le bouton dans la rangee reservee, sous la barre de recherche vanilla.
    /// </summary>
    [HarmonyPatch(typeof(MainTabWindow_Architect), nameof(MainTabWindow_Architect.DoWindowContents))]
    public static class MainTabWindow_Architect_DoWindowContents_Patch
    {
        public static void Postfix(Rect inRect)
        {
            ArchitectStudioButton.Draw(inRect);
        }
    }
    /// <summary>
    /// Couleur de categorie. Meme point d'accroche que Colored Categories : on teinte GUI.color
    /// juste avant que le bouton ne soit dessine, et on le remet apres.
    /// </summary>
    [HarmonyPatch(typeof(MainTabWindow_Architect), "DoCategoryButton")]
    public static class MainTabWindow_Architect_DoCategoryButton_Patch
    {
        public static void Prefix(ArchitectCategoryTab panel)
        {
            var color = CategoryAppearance.ColorOf(panel.def);
            if (color.HasValue)
            {
                GUI.color = color.Value;
            }
        }

        public static void Postfix()
        {
            GUI.color = Color.white;
        }
    }

}
