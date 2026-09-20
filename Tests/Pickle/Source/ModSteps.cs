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

        /// <summary>
        /// The scale the way the Options page changes it, minus the confirmation dialog and the
        /// save. Writing <c>Prefs.UIScale</c> alone is not enough: widgets are laid out in
        /// <c>UI.screenWidth</c>/<c>UI.screenHeight</c>, two cached fields that only
        /// <c>Root.OnGUI</c> recomputes, and a window already on screen keeps the rect it was given
        /// in the old space until something tells it the resolution moved. A widget drawn from such
        /// a rect is recorded by Pickle's tag store at a place the new GUI space has not got, and
        /// the click that follows lands off screen.
        ///
        /// So: write the scale, drop the label widths measured at the old one, let frames pass for
        /// the fields, then do what <c>WindowStack.AdjustWindowsIfResolutionChanged</c> does and
        /// lay every open window out again. The sizes before and after ride on the report, and the
        /// step fails on the spot if the GUI space did not follow - a scale that did not take is
        /// otherwise only visible as a click that misses, several steps later.
        /// </summary>
        [When("I set the interface scale to {int} percent")]
        public async Task UiScale(PickleContext ctx, int percent)
        {
            // Never saved: Prefs.Save is not called, and the hook below puts the value back.
            if (!uiScaleBefore.HasValue)
            {
                uiScaleBefore = Prefs.UIScale;
            }

            var before = DescribeUiSpace();

            Prefs.UIScale = percent / 100f;
            GenUI.ClearLabelWidthCache();

            // Root.OnGUI runs UI.ApplyUIScale every frame: that is what moves UI.screenWidth and
            // UI.screenHeight to the new scale. Nothing a step can call does it off a frame.
            await ctx.WaitFrames(2);

            foreach (var window in Find.WindowStack.Windows.ToList())
            {
                window.Notify_ResolutionChanged();
            }

            await ctx.WaitFrames(5);

            var after = DescribeUiSpace();
            ctx.Attach("interface scale", $"before: {before}\nafter:  {after}");

            var expected = Mathf.RoundToInt(Screen.height / Prefs.UIScale);
            ctx.Require(UI.screenHeight == expected,
                $"the GUI space did not follow the scale: UI.screenHeight is {UI.screenHeight}, and {Screen.height} " +
                $"pixels at {Prefs.UIScale:0.##} make {expected}. Every rect measured now belongs to the old layout");
        }

        private static string DescribeUiSpace() =>
            $"scale {Prefs.UIScale:0.##}, GUI space {UI.screenWidth}x{UI.screenHeight}, window {Screen.width}x{Screen.height}";

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

        // ---------------------------------------------------------------- screenshots a person reads

        private static readonly Dictionary<Window, bool> hiddenForCapture = new Dictionary<Window, bool>();

        /// <summary>
        /// Turns on the game's own screenshot mode, which hides everything that is not a window - the
        /// tab bar, the alerts, the colonist bar, the dev toolbar - and hides Pickle's own runner
        /// windows on top of that, because a window draws in that mode unless it is told not to. The
        /// capture then carries the editor over the map and nothing else.
        /// </summary>
        [When("I hide the interface around the windows on screen")]
        public async Task HideInterface(PickleContext ctx)
        {
            var flag = AccessTools.Field(typeof(Window), "drawInScreenshotMode");
            ctx.Require(flag != null, "Window.drawInScreenshotMode no longer exists: update the step");

            foreach (var window in Find.WindowStack.Windows)
            {
                var fromPickle = window.GetType().Assembly.GetName().Name.StartsWith("RimWorks.Pickle");
                var wanted = !fromPickle;
                var current = (bool)flag.GetValue(window);
                if (current == wanted)
                {
                    continue;
                }

                hiddenForCapture[window] = current;
                flag.SetValue(window, wanted);
            }

            Find.UIRoot.screenshotMode.Active = true;
            await ctx.WaitFrames(3);
        }

        [When("I bring the interface back")]
        public void ShowInterface(PickleContext ctx)
        {
            RestoreInterface();
        }

        /// <summary>
        /// A scenario that dies between the two steps would otherwise leave the game without its
        /// interface, and no report would explain why.
        /// </summary>
        [AfterScenario]
        public void RestoreInterfaceAfterScenario(PickleContext ctx)
        {
            RestoreInterface();
        }

        private static void RestoreInterface()
        {
            var flag = AccessTools.Field(typeof(Window), "drawInScreenshotMode");
            foreach (var pair in hiddenForCapture)
            {
                flag.SetValue(pair.Key, pair.Value);
            }

            hiddenForCapture.Clear();

            if (Find.UIRoot?.screenshotMode != null)
            {
                Find.UIRoot.screenshotMode.Active = false;
            }
        }

        /// <summary>
        /// The generated category lands at the end of the binding list, out of the first screenful:
        /// a capture taken without this shows the camera bindings and proves nothing.
        /// </summary>
        [When("I scroll the keyboard configuration to the bottom")]
        public async Task ScrollKeyBindings(PickleContext ctx)
        {
            var dialog = Find.WindowStack.WindowOfType<Dialog_KeyBindings>();
            ctx.Require(dialog != null, "the keyboard configuration is not open");

            var field = AccessTools.Field(typeof(Dialog_KeyBindings), "scrollPosition");
            ctx.Require(field != null, "Dialog_KeyBindings.scrollPosition no longer exists: update the step");
            // Far past the end; the scroll view clamps it to the last screenful.
            field.SetValue(dialog, new Vector2(0f, 100000f));

            await ctx.WaitFrames(3);
        }
    }
}
