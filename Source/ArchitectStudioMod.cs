using HarmonyLib;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    public class ArchitectStudioMod : Mod
    {
        public const string HarmonyId = "nelim.architectstudio";

        public static ArchitectStudioMod Instance { get; private set; }
        public static ArchitectStudioSettings Settings { get; private set; }
        public static Harmony HarmonyInstance { get; private set; }

        public ArchitectStudioMod(ModContentPack content) : base(content)
        {
            Instance = this;
            Settings = GetSettings<ArchitectStudioSettings>();

            HarmonyInstance = new Harmony(HarmonyId);

            // Only the patches whose target type has no static constructor loading resources: this
            // constructor runs on a background thread, and patching a method there triggers its
            // type's static constructor. The rest is applied in StartupInit, on the main thread.
            HarmonyInstance.PatchAll();
        }

        public override string SettingsCategory() => "Architect Studio";

        private static string IntegrationLine(string name, bool active)
        {
            return "   " + name + " : " + (active
                ? "ArchitectStudio.Settings.Detected".Translate()
                : "ArchitectStudio.Settings.NotDetected".Translate());
        }

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

            var showLocked = Settings.showResearchLocked;
            listing.CheckboxLabeled("ArchitectStudio.Settings.ShowResearchLocked".Translate(), ref showLocked,
                "ArchitectStudio.Settings.ShowResearchLockedTip".Translate());
            if (showLocked != Settings.showResearchLocked)
            {
                Settings.showResearchLocked = showLocked;
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

            listing.GapLine();

            // Integration status: without this recap, a missing feature looks like a bug when all
            // that is missing is a mod.
            listing.Label("ArchitectStudio.Settings.Integrations".Translate());
            GUI.color = new Color(1f, 1f, 1f, 0.6f);
            listing.Label(IntegrationLine("Better Architect Menu", BetterArchitectCompat.Active));
            listing.Label(IntegrationLine("Architect Icons", ArchitectIconsCompat.Available));
            listing.Label(IntegrationLine("Float Sub-Menus", FloatSubMenuCompat.Available));
            GUI.color = Color.white;

            listing.Gap();

            if (ArchitectStudioReset.HasAnything &&
                listing.ButtonText("ArchitectStudio.Settings.ResetEverything".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "ArchitectStudio.Settings.ConfirmResetEverything".Translate(),
                    ArchitectStudioReset.All,
                    destructive: true));
            }

            listing.End();
        }
    }
}
