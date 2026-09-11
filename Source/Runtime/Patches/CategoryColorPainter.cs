using HarmonyLib;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Colour of the subcategories Better Architect Menu draws.
    ///
    /// It draws them not as buttons but with a bare <c>Widgets.Label</c>: no hook of their own. On
    /// the other hand, just before each row, it asks for the category's icon. We take advantage of
    /// that call to arm the colour, and consume it on the first label drawn afterwards - the
    /// category's. Arming is single-use and the colour is reset to white at once, so it never
    /// bleeds onto the rest of the interface.
    /// </summary>
    public static class CategoryColorPainter
    {
        private static Color? pending;
        private static bool applied;

        /// <summary>
        /// Suppresses arming during our own icon reads: without this, the Categories window would
        /// arm the colour by asking for a row's icon, and the next, unrelated label would receive
        /// it.
        /// </summary>
        public static bool Suppressed { get; set; }

        public static void Arm(string categoryDefName)
        {
            if (Suppressed || categoryDefName.NullOrEmpty())
            {
                return;
            }

            var category = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(categoryDefName);
            pending = category == null ? null : CategoryAppearance.ColorOf(category);
        }

        public static void Disarm()
        {
            pending = null;
        }

        /// <summary>
        /// Applies the patch. Deliberately no [HarmonyPatch] attribute: PatchAll runs from the mod
        /// constructor, on a background thread. Patching a method forces the JIT to compile it,
        /// which triggers its type's static constructor - and <c>Verse.Widgets</c>' loads textures,
        /// which Unity forbids off the main thread.
        /// </summary>
        public static void ApplyPatch(Harmony harmony)
        {
            var target = AccessTools.Method(typeof(Widgets), nameof(Widgets.Label),
                new[] { typeof(Rect), typeof(string) });

            if (target == null)
            {
                return;
            }

            harmony.Patch(target,
                prefix: new HarmonyMethod(typeof(Widgets_Label_Patch), nameof(Widgets_Label_Patch.Prefix)),
                postfix: new HarmonyMethod(typeof(Widgets_Label_Patch), nameof(Widgets_Label_Patch.Postfix)));
        }

        public static class Widgets_Label_Patch
        {
            public static void Prefix()
            {
                applied = false;

                if (pending == null)
                {
                    return;
                }

                GUI.color = pending.Value;
                applied = true;
                pending = null;
            }

            public static void Postfix()
            {
                if (applied)
                {
                    GUI.color = Color.white;
                    applied = false;
                }
            }
        }
    }
}
