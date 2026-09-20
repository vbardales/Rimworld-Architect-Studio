using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HarmonyLib;
using RimWorks.Pickle;
using UnityEngine;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>
    /// Records what Pickle's tag store is handed for a widget, and what it turns it into.
    ///
    /// The click at 150% interface scale lands off screen while everything the game reports is
    /// right: the GUI space follows the scale, and the Architect window is laid out for it. The
    /// rect Pickle resolves, though, falls below the very window that drew the button. The
    /// conversion is `GUIUtility.GUIToScreenRect`, whose unclipping happens in Unity's native GUI
    /// clip stack - nothing readable from managed code says how it composes the clip origins with
    /// GUI.matrix. So it is measured instead, from inside the repaint that draws the widget, which
    /// is the only moment that stack exists.
    ///
    /// This probe patches Pickle rather than living in it: her Pickle install stays untouched, and
    /// the measurement only runs while a scenario asks for it.
    /// </summary>
    public static class TagProbe
    {
        private static readonly List<string> Recorded = new List<string>();
        private static string watched;
        private static bool patched;

        public static IReadOnlyList<string> Lines => Recorded;

        public static void Watch(PickleContext ctx, string tag)
        {
            Ensure(ctx);
            Recorded.Clear();
            watched = tag;
        }

        public static void Stop()
        {
            watched = null;
        }

        private static void Ensure(PickleContext ctx)
        {
            if (patched)
            {
                return;
            }

            var store = AccessTools.TypeByName("RimWorks.Pickle.Input.TagStore");
            ctx.Require(store != null, "RimWorks.Pickle.Input.TagStore no longer exists: update the probe");
            var record = AccessTools.Method(store, "Record", new[] { typeof(string), typeof(Rect) });
            ctx.Require(record != null, "TagStore.Record(string, Rect) no longer exists: update the probe");

            new Harmony("nelim.architectstudio.pickletests").Patch(record,
                prefix: new HarmonyMethod(AccessTools.Method(typeof(TagProbe), nameof(Capture))));
            patched = true;
        }

        /// <summary>Runs before Pickle stores anything, so the rect is the one the widget was drawn with.</summary>
        public static void Capture(string tag, Rect rect)
        {
            // Record itself keeps only what a repaint hands it; a layout pass would measure another
            // thing entirely.
            if (watched == null || tag != watched || Event.current == null || Event.current.type != EventType.Repaint)
            {
                return;
            }

            if (Recorded.Count >= 3)
            {
                return;
            }

            var window = Find.WindowStack.currentlyDrawnWindow;
            Recorded.Add(
                $"scale {Prefs.UIScale:0.##}, GUI space {UI.screenWidth}x{UI.screenHeight}, window {Screen.width}x{Screen.height}\n" +
                $"  handed to Record  : {rect}\n" +
                $"  GUIToScreenRect   : {GUIUtility.GUIToScreenRect(rect)}\n" +
                $"  origin of the clip: {GUIUtility.GUIToScreenPoint(Vector2.zero)}\n" +
                $"  GUI.matrix scale  : {GUI.matrix.lossyScale}\n" +
                $"  window drawn      : {(window == null ? "none" : window.GetType().Name)} at " +
                $"{(window == null ? "-" : window.windowRect.ToString())}");
        }
    }

    /// <summary>The step that turns the probe on around a repaint, and puts what it saw on the report.</summary>
    [PickleSteps]
    public class TagProbeSteps
    {
        [When("I record what Pickle measures for the Architect Studio button keyed {string}")]
        public async Task Probe(PickleContext ctx, string key)
        {
            ctx.Require(key.CanTranslate(), $"no translation is loaded for the key '{key}'");
            var tag = $"btn:{key.Translate()}";

            TagProbe.Watch(ctx, tag);
            await ctx.WaitFrames(5);
            TagProbe.Stop();

            ctx.Require(TagProbe.Lines.Any(),
                $"nothing recorded '{tag}' in five frames: either the button is not drawn, or Pickle's tag session is off");
            ctx.Attach($"what Pickle measures for '{tag}'", string.Join("\n", TagProbe.Lines));
        }
    }
}
