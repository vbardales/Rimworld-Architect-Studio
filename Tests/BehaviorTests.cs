using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using ArchitectStudio;
using RimWorld;
using Verse;

namespace ArchitectStudioTests
{
    // Runs the shipped DLL against installed game assemblies, without a game/UI session.
    public static class BehaviorTests
    {
        private static bool LogError(string text) { throw new Exception(text); }
        private static int checks;
        private static T Def<T>(string name) where T : Verse.Def
        {
            // Graphics-bearing Def constructors require Unity. Only identity is needed here.
            var def = (T)System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof(T));
            def.defName = name;
            return def;
        }
        private static void Check(bool condition, string message)
        {
            if (!condition) throw new Exception("FAIL: " + message);
            checks++;
            Console.WriteLine("PASS: " + message);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public static void Run(string root, string output)
        {
            var prefs = typeof(Prefs).GetField("data", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            prefs.SetValue(null, System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(prefs.FieldType));
            new HarmonyLib.Harmony("architectstudio.tests.log").Patch(
                typeof(Log).GetMethod("Error", new[] { typeof(string) }),
                prefix: new HarmonyLib.HarmonyMethod(typeof(BehaviorTests).GetMethod("LogError", BindingFlags.Static | BindingFlags.NonPublic)));
            // A headless process has no active-mod discovery. Register the serialized types
            // without booting Unity or scanning unrelated game systems and mods.
            typeof(GenTypes).GetField("allTypesCached", BindingFlags.Static | BindingFlags.NonPublic)
                .SetValue(null, new List<Type> { typeof(ArchitectStudioSettings), typeof(DropdownGroupEntry),
                    typeof(DropdownOrderEntry), typeof(CustomCategoryEntry) });
            var typeCache = (System.Collections.IDictionary)typeof(GenTypes)
                .GetField("typeCache", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            var keyType = typeof(GenTypes).GetNestedType("TypeCacheKey", BindingFlags.NonPublic);
            foreach (var type in new[] { typeof(ArchitectStudioSettings), typeof(DropdownGroupEntry),
                typeof(DropdownOrderEntry), typeof(CustomCategoryEntry) })
                typeCache.Add(Activator.CreateInstance(keyType, new object[] { type.FullName, null }), type);
            var settings = new ArchitectStudioSettings();
            Check(settings.showArchitectButton && !settings.showResearchLocked && settings.schemaVersion == 1,
                "clean settings use documented defaults");
            Check(settings.customGroups.Count == 0 && settings.customCategories.Count == 0,
                "clean settings have no custom definitions");
            var group = new DropdownGroupEntry("group", "Étage & atelier");
            Check(!group.useGridMenu && group.iconSource == DesignatorDropdownGroupDef.IconSource.Placed,
                "new groups default to list and placed-building icons");

            settings.showArchitectButton = false;
            settings.showResearchLocked = true;
            group.useGridMenu = true;
            group.iconSource = DesignatorDropdownGroupDef.IconSource.Cost;
            settings.customGroups.Add(group);
            settings.customCategories.Add(new CustomCategoryEntry("custom", "Étage", "Production"));
            settings.dropdownAssignments["thing:Wall"] = "group";
            settings.dropdownAssignments["terrain:Tile"] = "";
            settings.groupOrders.Add(new DropdownOrderEntry("group", new List<string> { "thing:Wall", "terrain:Tile" }));
            settings.hiddenGroupIds.Add("oldGroup");
            settings.groupCategories["group"] = "custom";
            settings.categoryOrders["custom"] = 123;
            settings.categoryLabels["custom"] = "Étage & atelier";
            settings.categoryIcons["custom"] = "UI/ArchitectIcons/Test";
            settings.categoryColors["custom"] = "255,128,0";
            string file = Path.Combine(output, "settings.xml");
            Scribe.saver.InitSaving(file, "settings");
            settings.ExposeData();
            Scribe.saver.FinalizeSaving();
            var loaded = new ArchitectStudioSettings();
            Scribe.loader.InitLoading(file);
            loaded.ExposeData();
            Scribe.loader.FinalizeLoading();
            Check(!loaded.showArchitectButton && loaded.showResearchLocked, "both toggles survive real Scribe round-trip");
            Check(loaded.customGroups.Single().label == group.label && loaded.customGroups[0].useGridMenu
                && loaded.customGroups[0].iconSource == group.iconSource, "group name, grid mode and icon source persist");
            Check(loaded.customCategories.Single().parentId == "Production", "custom category and parent persist");
            Check(loaded.dropdownAssignments["thing:Wall"] == "group" && loaded.dropdownAssignments["terrain:Tile"] == "",
                "assignment and explicit ungrouping persist separately");
            Check(loaded.groupOrders.Single().memberKeys.SequenceEqual(settings.groupOrders[0].memberKeys), "member order persists");
            Check(loaded.hiddenGroupIds.Single() == "oldGroup" && loaded.groupCategories["group"] == "custom", "hidden groups and forced categories persist");
            Check(loaded.categoryOrders["custom"] == 123 && loaded.categoryLabels["custom"] == "Étage & atelier"
                && loaded.categoryIcons["custom"] == "UI/ArchitectIcons/Test" && loaded.categoryColors["custom"] == "255,128,0",
                "category order, label, icon and colour persist");

            string legacyFile = Path.Combine(output, "legacy.xml");
            File.WriteAllText(legacyFile, "<settings />");
            var legacy = new ArchitectStudioSettings();
            Scribe.loader.InitLoading(legacyFile);
            legacy.ExposeData();
            Scribe.loader.FinalizeLoading();
            Check(legacy.showArchitectButton && !legacy.showResearchLocked && legacy.schemaVersion == 1,
                "missing legacy values restore defaults");
            Check(typeof(ArchitectStudioSettings).GetFields().Where(f => f.FieldType.IsGenericType)
                .All(f => f.GetValue(legacy) != null), "missing legacy collections are initialized");

            typeof(ArchitectStudioMod).GetProperty("Settings").GetSetMethod(true).Invoke(null, new object[] { loaded });
            var a = Def<ThingDef>("Wall");
            var b = Def<TerrainDef>("Tile");
            var c = Def<ThingDef>("New");
            var d = Def<ThingDef>("Later");
            Check(DropdownRuntime.KeyOf(a) != DropdownRuntime.KeyOf(Def<TerrainDef>("Wall")),
                "thing and terrain IDs do not collide");
            var members = new List<BuildableDef> { c, b, d, a };
            Check(DropdownOrderRuntime.SortMembers("group", members).SequenceEqual(new BuildableDef[] { a, b, c, d }),
                "stored order applies and new members keep stable relative order");
            DropdownOrderRuntime.SetOrder("group", new BuildableDef[] { b, a });
            Check(loaded.groupOrders.Count == 1 && DropdownOrderRuntime.SortMembers("group", members)[0] == b,
                "editing order updates the existing entry and actual sort result");
            DropdownOrderRuntime.ClearOrder("group");
            Check(!DropdownOrderRuntime.HasOrder("group") && ReferenceEquals(members, DropdownOrderRuntime.SortMembers("group", members)),
                "reset order restores original member sequence");

            Check(SettingsInput.NormalizeName(null) == "" && SettingsInput.NormalizeName(" \t\n ") == "",
                "null and whitespace-only names are rejected by the dialog's empty-name guard");
            Check(SettingsInput.NormalizeName("  Étage & atelier  ") == "Étage & atelier",
                "name validation trims boundaries and preserves accented names");
            string longName = new string('x', 500);
            Check(SettingsInput.NormalizeName(longName) == longName, "long names are not silently truncated");
            Check(loaded.HasNonDefaultPreferences && ArchitectStudioReset.HasAnything,
                "nondefault preferences are detected for reset");
            Check(ResearchLockedVisibility.Enabled, "research visibility reads the persisted enabled preference");
            loaded.ResetPreferences();
            Check(!loaded.HasNonDefaultPreferences && !ResearchLockedVisibility.Enabled && loaded.showArchitectButton,
                "reset restores both toggles and changes the runtime research flag");
            Check(loaded.customGroups.Count == 1, "preference reset leaves group data to the full reset pipeline");

            loaded.customGroups.Add(null);
            loaded.customGroups.Add(new DropdownGroupEntry("", "invalid"));
            loaded.groupOrders.Add(null);
            loaded.groupOrders.Add(new DropdownOrderEntry("", null));
            loaded.customCategories.Add(null);
            loaded.customCategories.Add(new CustomCategoryEntry("", "invalid", null));
            Scribe.mode = LoadSaveMode.PostLoadInit;
            try { loaded.ExposeData(); }
            finally { Scribe.mode = LoadSaveMode.Inactive; }
            Check(loaded.customGroups.Count == 1 && loaded.customCategories.Count == 1 && loaded.groupOrders.Count == 0,
                "post-load cleanup removes null and id-less records without losing valid entries");

            var clean = new ArchitectStudioSettings();
            typeof(ArchitectStudioMod).GetProperty("Settings").GetSetMethod(true).Invoke(null, new object[] { clean });
            Check(!ArchitectStudioReset.HasAnything, "clean configuration has no reset action");
            clean.showArchitectButton = false;
            Check(ArchitectStudioReset.HasAnything, "changing only the Architect toggle enables reset");
            clean.ResetPreferences();
            clean.showResearchLocked = true;
            Check(ArchitectStudioReset.HasAnything, "changing only research visibility enables reset");
            Console.WriteLine("PASS: " + checks + " runtime behavior checks; no game UI session was run.");
        }
    }
}



