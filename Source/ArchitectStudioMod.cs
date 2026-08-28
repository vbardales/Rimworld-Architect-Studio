using HarmonyLib;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    public class ArchitectStudioMod : Mod
    {
        public const string HarmonyId = "vbardales.architectstudio";

        public static ArchitectStudioMod Instance { get; private set; }
        public static ArchitectStudioSettings Settings { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }

        public ArchitectStudioMod(ModContentPack content) : base(content)
        {
            Instance = this;
            Settings = GetSettings<ArchitectStudioSettings>();

            HarmonyInstance = new Harmony(HarmonyId);
            HarmonyInstance.PatchAll();
            ArchitectIconsCompat.ApplyPatch(HarmonyInstance);
        }

        public override string SettingsCategory() => "Architect Studio";

        public override void DoSettingsWindowContents(Rect inRect)
        {
            var listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("ArchitectStudio.Settings.Intro".Translate());
            listing.GapLine();

            if (listing.ButtonText("ArchitectStudio.Settings.OpenDropdowns".Translate()))
            {
                ArchitectStudioUI.ToggleDropdownDialog();
            }

            listing.Gap();

            if (listing.ButtonText("ArchitectStudio.Settings.OpenCategories".Translate()))
            {
                ArchitectStudioUI.ToggleCategoriesDialog();
            }

            listing.Gap();

            var showButton = Settings.showArchitectButton;
            listing.CheckboxLabeled("ArchitectStudio.Settings.ShowArchitectButton".Translate(), ref showButton,
                "ArchitectStudio.Settings.ShowArchitectButtonTip".Translate());
            if (showButton != Settings.showArchitectButton)
            {
                Settings.showArchitectButton = showButton;
                WriteSettings();
            }

            listing.Gap();

            GUI.color = new Color(1f, 1f, 1f, 0.6f);
            var keyDef = ArchitectStudioKeyBindingDefOf.ArchitectStudio_OpenDropdowns;
            var bound = keyDef != null && keyDef.MainKey != KeyCode.None;
            listing.Label(bound
                ? "ArchitectStudio.Settings.KeyHint".Translate(keyDef.MainKeyLabel)
                : "ArchitectStudio.Settings.KeyHintUnbound".Translate());
            GUI.color = Color.white;

            listing.End();
        }
    }
}
