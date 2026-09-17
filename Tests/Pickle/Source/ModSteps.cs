using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>Loading, the Architect window, the research option, reset and restart.</summary>
    [PickleSteps]
    public class ModSteps
    {
        // ---------------------------------------------------------------- loading

        [Then("the Architect Studio assembly carries the access-check waiver")]
        public void Waiver(PickleContext ctx)
        {
            var found = typeof(ArchitectStudioMod).Assembly.GetCustomAttributesData().Any(a =>
                a.AttributeType.Name == "IgnoresAccessChecksToAttribute" &&
                a.ConstructorArguments.Any(arg => (arg.Value as string) == "Assembly-CSharp"));
            ctx.Assert(found,
                "IgnoresAccessChecksTo(\"Assembly-CSharp\") is not applied: the build lost Source/AccessChecks.cs, and every non-public access will throw at first use");
        }

        [When("the access check probe runs")]
        public void Probe(PickleContext ctx)
        {
            // An error it logs fails the scenario on its own; Pickle watches the log.
            AccessCheckProbe.Run();
        }

        [Then("Architect Studio patched {string}")]
        public void Patched(PickleContext ctx, string target)
        {
            var parts = target.Split(new[] { "::" }, StringSplitOptions.None);
            ctx.Require(parts.Length == 2, $"write the target as Type::Member, not '{target}'");
            var type = AccessTools.TypeByName(parts[0]);
            ctx.Require(type != null, $"type '{parts[0]}' not found");
            MethodBase method = AccessTools.DeclaredPropertyGetter(type, parts[1]) ?? (MethodBase)AccessTools.DeclaredMethod(type, parts[1]);
            ctx.Require(method != null, $"'{target}' not found");

            var info = Harmony.GetPatchInfo(method);
            var owners = info?.Owners ?? (IEnumerable<string>)new string[0];
            ctx.Assert(owners.Contains(ArchitectStudioMod.HarmonyId),
                $"'{target}' carries no Architect Studio patch; owners: {string.Join(", ", owners)}");
        }

        [Then("the game log holds nothing from Architect Studio since startup")]
        public void LogSilent(PickleContext ctx)
        {
            var lines = Log.Messages.Where(m => m.text != null && m.text.Contains("[Architect Studio]")).ToList();
            ctx.Assert(lines.Count == 0,
                "Architect Studio logged at startup:\n" + string.Join("\n", lines.Select(m => $"{m.type}: {m.text}")));
        }

        // ---------------------------------------------------------------- the Architect window

        [When("I turn {word} the button in the Architect menu")]
        public void ToggleButton(PickleContext ctx, string onOff)
        {
            ctx.Require(onOff == "on" || onOff == "off", $"write on or off, not '{onOff}'");
            ctx.Set(new WindowHeight { Value = ArchitectWindow(ctx).WinHeight });
            // What the settings checkbox does.
            ArchitectStudioMod.Settings.showArchitectButton = onOff == "on";
            ArchitectStudioMod.Instance.WriteSettings();
        }

        [Then("the Architect window got {float} pixels {word}")]
        public void HeightChanged(PickleContext ctx, float pixels, string direction)
        {
            var before = ctx.Get<WindowHeight>().Value;
            var after = ArchitectWindow(ctx).WinHeight;
            var expected = direction == "shorter" ? before - pixels : before + pixels;
            ctx.Assert(Math.Abs(after - expected) < 0.5f,
                $"the Architect window should be {expected}px tall; it is {after}px (was {before}px)");
        }

        [Then("no button {string} is drawn")]
        public async Task NotDrawn(PickleContext ctx, string label)
        {
            try
            {
                await ctx.Hover($"btn:{label}");
            }
            catch (Exception)
            {
                return;
            }

            ctx.Assert(false, $"a button labelled '{label}' is still drawn");
        }

        [When("I set the interface scale to {int} percent")]
        public void UiScale(PickleContext ctx, int percent)
        {
            // Never saved: Prefs.Save is not called, and the hook below puts the value back.
            if (!uiScaleBefore.HasValue)
            {
                uiScaleBefore = Prefs.UIScale;
            }

            Prefs.UIScale = percent / 100f;
        }

        private static float? uiScaleBefore;

        [AfterScenario]
        public void RestoreUiScale(PickleContext ctx)
        {
            if (uiScaleBefore.HasValue)
            {
                Prefs.UIScale = uiScaleBefore.Value;
                uiScaleBefore = null;
            }
        }

        private sealed class WindowHeight
        {
            public float Value;
        }

        private static MainTabWindow_Architect ArchitectWindow(PickleContext ctx)
        {
            var window = MainButtonDefOf.Architect.TabWindow as MainTabWindow_Architect;
            ctx.Require(window != null,
                $"the Architect tab window is {MainButtonDefOf.Architect.TabWindow?.GetType().Name ?? "null"}, not MainTabWindow_Architect");
            return window;
        }

        // ---------------------------------------------------------------- research-locked option

        [Given("research {string} is not finished")]
        public void Unfinish(PickleContext ctx, string projectDefName)
        {
            var project = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(projectDefName);
            ctx.Require(project != null, $"no research project is named '{projectDefName}'");
            var progress = (Dictionary<ResearchProjectDef, float>)typeof(ResearchManager)
                .GetField("progress", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Find.ResearchManager);
            progress[project] = 0f;
            ctx.Require(!project.IsFinished, $"'{projectDefName}' still reads as finished");
        }

        [When("I turn {word} showing what research still locks")]
        public void ToggleLocked(PickleContext ctx, string onOff)
        {
            ArchitectStudioMod.Settings.showResearchLocked = onOff == "on";
            ArchitectStudioMod.Instance.WriteSettings();
        }

        [Then("the Architect menu shows {string} greyed out with the reason {string}")]
        public void GreyedOut(PickleContext ctx, string defName, string reason)
        {
            var designator = BuildDesignator(ctx, defName);
            var visible = designator.Visible;
            ctx.Assert(visible && Disabled(designator) && designator.disabledReason == reason,
                $"'{defName}' should be visible and disabled with '{reason}'; visible={visible}, " +
                $"disabled={Disabled(designator)}, reason='{designator.disabledReason}'. A visible designator that is not " +
                "disabled lets a blueprint be placed without the research: nothing else checks it");
        }

        [Then("the Architect menu shows {string} as buildable")]
        public void Buildable(PickleContext ctx, string defName)
        {
            var designator = BuildDesignator(ctx, defName);
            var visible = designator.Visible;
            ctx.Assert(visible && !Disabled(designator),
                $"'{defName}' should be visible and enabled; visible={visible}, disabled={Disabled(designator)}, reason='{designator.disabledReason}'");
        }

        [Then("the Architect menu hides {string}")]
        public void Hidden(PickleContext ctx, string defName)
        {
            var designator = BuildDesignator(ctx, defName);
            ctx.Assert(!designator.Visible, $"'{defName}' should be hidden; it is visible (disabled={Disabled(designator)})");
        }

        /// <summary><c>Gizmo.disabled</c> is protected; the mod reaches it through the publicizer, the steps by reflection.</summary>
        private static bool Disabled(Gizmo gizmo)
        {
            return (bool)typeof(Gizmo).GetField("disabled", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .GetValue(gizmo);
        }

        private static Designator_Build BuildDesignator(PickleContext ctx, string defName)
        {
            var def = Driver.Buildable(ctx, defName);
            var all = def.designationCategory.AllResolvedDesignators
                .SelectMany(d => d is Designator_Dropdown dropdown ? dropdown.Elements : new List<Designator> { d })
                .OfType<Designator_Build>();
            var designator = all.FirstOrDefault(b => b.PlacingDef == def);
            ctx.Require(designator != null, $"no build designator for '{defName}' in '{def.designationCategory.defName}'");
            return designator;
        }

        // ---------------------------------------------------------------- reset and restart

        private sealed class MenuSnapshot
        {
            public Dictionary<string, string> Buildables;
            public Dictionary<string, string> Categories;
        }

        private static MenuSnapshot Snapshot()
        {
            return new MenuSnapshot
            {
                Buildables = DropdownRuntime.AllBuildables().ToDictionary(DropdownRuntime.KeyOf,
                    d => $"{d.designationCategory?.defName} / {d.designatorDropdown?.defName ?? "-"}"),
                Categories = DefDatabase<DesignationCategoryDef>.AllDefsListForReading.ToDictionary(c => c.defName,
                    c => $"order {c.order}, label '{c.label}', colour {CategoryAppearance.ColorOf(c)?.ToString() ?? "-"}")
            };
        }

        [When("I remember the Architect menu")]
        public void RememberMenu(PickleContext ctx) => ctx.Set(Snapshot());

        [Then("the Architect menu is as remembered")]
        public void MenuAsRemembered(PickleContext ctx)
        {
            var before = ctx.Get<MenuSnapshot>();
            var now = Snapshot();
            var diffs = Diff("building", before.Buildables, now.Buildables)
                .Concat(Diff("category", before.Categories, now.Categories)).Take(20).ToList();
            ctx.Assert(diffs.Count == 0, "the Architect menu differs:\n" + string.Join("\n", diffs));
        }

        private static IEnumerable<string> Diff(string kind, Dictionary<string, string> before, Dictionary<string, string> now)
        {
            foreach (var key in before.Keys.Union(now.Keys))
            {
                before.TryGetValue(key, out var a);
                now.TryGetValue(key, out var b);
                if (a != b)
                {
                    yield return $"  {kind} {key}: was {a ?? "absent"}, now {b ?? "absent"}";
                }
            }
        }

        [When("I reset everything from the mod settings")]
        public void ResetAll(PickleContext ctx) => ArchitectStudioReset.All();

        [Then("the mod settings offer nothing to reset")]
        public void NothingToReset(PickleContext ctx)
        {
            ctx.Assert(!ArchitectStudioReset.HasAnything, "the reset button is still offered: something survived the reset");
        }

        /// <summary>
        /// A restart in one process: the runtime is unwound to the unmodded defs, then the file on
        /// disk is read back and replayed, as StartupInit does. What it cannot reproduce is a def
        /// database rebuilt from XML, which is why the full restart stays a manual check.
        /// </summary>
        [When("Architect Studio starts again from its settings file")]
        public void Restart(PickleContext ctx)
        {
            ArchitectStudioMod.Instance.WriteSettings();
            var path = (string)typeof(LoadedModManager).GetMethod("GetSettingsFilename", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Invoke(null, new object[] { ArchitectStudioMod.Instance.Content.FolderName, ArchitectStudioMod.Instance.GetType().Name });
            var saved = File.ReadAllText(path);

            ArchitectStudioReset.All();
            File.WriteAllText(path, saved);
            SettingsSandbox.ReloadFromDisk();
        }
    }
}
