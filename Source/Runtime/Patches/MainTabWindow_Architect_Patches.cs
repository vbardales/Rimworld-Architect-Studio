using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Adds an opening button straight into the Architect window. Indispensable without a keyboard
    /// - on the Steam Deck notably - where the shortcut is out of reach.
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
    /// Reserves the button's height. Everything else in the window - the info panel's position, the
    /// top of the pane - derives from WinHeight, so the adjustment propagates on its own.
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
    /// Draws the button in the row reserved for it, under the vanilla search bar.
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
    /// Category colour. Same hook as Colored Categories: we tint GUI.color just before the button
    /// is drawn, and put it back afterwards.
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
