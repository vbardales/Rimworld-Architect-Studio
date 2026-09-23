using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorld;
using UnityEngine;
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

        /// <summary>
        /// Each optional bridge reports itself available exactly when its mod is loaded: never when it is
        /// absent (a false positive would call into a type that is not there), never when it is present
        /// (a false negative would silently drop the integration). The scenario that uses it runs in every
        /// pass, so the same assertion covers "none of them is loaded" in the minimal pass and "all of
        /// them are" in the pass with the optional mods, with no way to be skipped.
        ///
        /// It attaches which world it ran in, so a report reader sees whether an absent bridge was
        /// checked against an absent mod or a present one.
        /// </summary>
        [Then("each optional integration is reported exactly when its mod is loaded")]
        public void IntegrationsMatchLoadedMods(PickleContext ctx)
        {
            var bridges = new[]
            {
                new { Name = "Better Architect Menu", PackageId = "ferny.betterarchitect", Reported = BetterArchitectCompat.Active },
                new { Name = "Architect Icons", PackageId = "com.bymarcin.architecticons", Reported = ArchitectIconsCompat.Available },
                new { Name = "Float Sub-Menus", PackageId = "kathanon.floatsubmenu", Reported = FloatSubMenuCompat.Available },
            };

            var world = new List<string>();
            foreach (var bridge in bridges)
            {
                var loaded = ModLister.AllInstalledMods.Any(m =>
                    m.Active && string.Equals(m.PackageIdNonUnique, bridge.PackageId, StringComparison.OrdinalIgnoreCase));

                world.Add($"{bridge.Name}: mod {(loaded ? "loaded" : "absent")}, bridge {(bridge.Reported ? "reported" : "not reported")}");
                ctx.Assert(bridge.Reported == loaded,
                    $"{bridge.Name}: the mod is {(loaded ? "loaded" : "absent")} but the bridge {(bridge.Reported ? "reports itself available" : "does not")}");
            }

            ctx.Attach("optional integrations in this pass", string.Join("; ", world));
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

        /// <summary>
        /// By translation key: the label drawn on the button is the one of the language the game
        /// runs in, so a scenario naming the English text only passes on an English game.
        ///
        /// The pointer is moved first and the window under it is read, because a real click is an OS
        /// click and goes to whatever window owns that point. Without this, a button covered by
        /// another mod's window fails the following step as "the dialog did not open", which reads
        /// as a dead button when in truth the click never reached it.
        /// </summary>
        [When("I click the Architect Studio button keyed {string}")]
        public async Task ClickKeyed(PickleContext ctx, string key)
        {
            var tag = $"btn:{Label(ctx, key)}";

            // Pickle names the point it failed to reach, never the space that point was measured
            // in: at another interface scale the two are what tell a stale rect from a bad click.
            ctx.Attach($"geometry before clicking '{tag}'",
                $"{DescribeUiSpace()}, Architect window at {ArchitectWindow(ctx).windowRect}");

            await WaitForModalsToClear(ctx);

            await ctx.Hover(tag);
            // Input.mousePosition is sampled per frame: without this the read is one move behind.
            await ctx.WaitFrames(2);

            var point = UI.MousePositionOnUIInverted;
            var under = Find.WindowStack.GetWindowAt(point);
            ctx.Assert(under == ArchitectWindow(ctx),
                $"the click at {point} would land on {Describe(under)}, not on the Architect window: " +
                "something is drawn over the button, and the button itself is not at fault");

            await ctx.Click(tag);
        }

        // Diagnostics for the Architect-specific keyed click. InterfaceScale owns changing and
        // restoring the scale; this only records the coordinate spaces when a click is reviewed.
        private static string DescribeUiSpace() =>
            $"scale {Prefs.UIScale:0.##}, GUI space {UI.screenWidth}x{UI.screenHeight}, window {Screen.width}x{Screen.height}";

        /// <summary>
        /// A window that absorbs input around itself eats the click wherever it is drawn, so the
        /// button can be plainly visible and unobstructed and the click still never arrive. Mods
        /// that warm up at load do this for a few seconds (MissileGirl's "please wait" box is one),
        /// which used to fail the very first click of the run and nothing after it. Waiting for the
        /// stack to clear costs nothing when no such window is up.
        /// </summary>
        private static async Task WaitForModalsToClear(PickleContext ctx)
        {
            for (var attempt = 0; attempt < 60; attempt++)
            {
                var blocker = Find.WindowStack.Windows.FirstOrDefault(w => w.absorbInputAroundWindow);
                if (blocker == null)
                {
                    return;
                }

                if (attempt == 0)
                {
                    ctx.Attach("waiting for a modal window",
                        $"{Describe(blocker)} absorbs input around itself; the click would be eaten wherever it lands");
                }

                await ctx.WaitFrames(10);
            }

            ctx.Assert(false,
                $"a modal window is still up after 600 frames: {Describe(Find.WindowStack.Windows.FirstOrDefault(w => w.absorbInputAroundWindow))}");
        }

        private static string Describe(Window window)
        {
            if (window == null)
            {
                return "no window at all";
            }

            var mod = window.GetType().Assembly.GetName().Name;
            return $"'{window.GetType().Name}' (from {mod})";
        }

        [Then("no Architect Studio button keyed {string} is drawn")]
        public async Task NotDrawn(PickleContext ctx, string key)
        {
            var label = Label(ctx, key);
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

        private static string Label(PickleContext ctx, string key)
        {
            ctx.Require(key.CanTranslate(), $"no translation is loaded for the key '{key}'");
            return key.Translate().ToString();
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

        /// <summary>By translation key: the text itself depends on the language the game runs in.</summary>
        [Then("the Architect menu shows {string} greyed out with the reason keyed {string}")]
        public void GreyedOut(PickleContext ctx, string defName, string reasonKey)
        {
            ctx.Require(reasonKey.CanTranslate(), $"no translation is loaded for the key '{reasonKey}'");
            var reason = reasonKey.Translate().ToString();
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

        internal sealed class MenuSnapshot
        {
            public Dictionary<string, string> Buildables;
            public Dictionary<string, string> Categories;
        }

        internal static MenuSnapshot Snapshot()
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

        internal static IEnumerable<string> Diff(string kind, Dictionary<string, string> before, Dictionary<string, string> now)
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

        /// <summary>
        /// Opens the settings page the way the hidden MainButtons shortcut does, by running its own
        /// worker: a screenshot of that window is a screenshot of the real shortcut's destination.
        /// Frames, not ticks: Dialog_ModSettings pauses the simulation, so a tick wait placed under
        /// it in a scenario never advances and times out.
        /// </summary>
        [When("I open the Architect Studio settings through the shortcut and let it draw")]
        public async Task OpenSettings(PickleContext ctx)
        {
            var def = DefDatabase<MainButtonDef>.GetNamedSilentFail("ArchitectStudio_Settings");
            ctx.Require(def != null, "the MainButtonDef 'ArchitectStudio_Settings' is not loaded");
            def.Worker.Activate();
            await ctx.WaitFrames(10);
        }

        /// <summary>
        /// The settings window on screen is Architect Studio's own. "window Dialog_ModSettings is open"
        /// is true for any mod's settings page, so a shortcut that opened someone else's would pass it
        /// - and a shortcut revealed by another mod's interface is exactly where that could go wrong.
        /// The window keeps the mod it shows in a field of type Mod; it is found by that type and not
        /// by the field's name, so a rename in the game fails loudly instead of passing silently.
        /// </summary>
        [Then("the settings window open is Architect Studio's own")]
        public void SettingsWindowIsOurs(PickleContext ctx)
        {
            var dialog = Find.WindowStack.Windows.OfType<Dialog_ModSettings>().FirstOrDefault();
            ctx.Require(dialog != null, "no Dialog_ModSettings is open");

            var field = typeof(Dialog_ModSettings)
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .FirstOrDefault(f => f.FieldType == typeof(Mod));
            ctx.Require(field != null,
                "Dialog_ModSettings holds no field of type Mod: the game changed how it remembers what it shows");

            var shown = field.GetValue(dialog) as Mod;
            ctx.Assert(shown is ArchitectStudioMod,
                $"the settings window shows {(shown == null ? "no mod" : shown.GetType().Name)}, not Architect Studio");
        }

        [Then("the mod settings offer nothing to reset")]
        public void NothingToReset(PickleContext ctx)
        {
            ctx.Assert(!ArchitectStudioReset.HasAnything, "the reset button is still offered: something survived the reset");
        }

        /// <summary>
        /// A restart-model check in one process: the runtime is unwound to the unmodded defs, then
        /// the file on disk is read back and replayed as StartupInit does. This checks Architect
        /// Studio's own settings; it makes no claim about another mod's settings persistence.
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
