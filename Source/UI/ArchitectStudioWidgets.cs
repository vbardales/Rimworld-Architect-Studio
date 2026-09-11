using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioWidgets
    {
        /// <summary>
        /// Up or down arrow button. The game ships these two textures ready to use, under
        /// <c>Verse.TexButton</c> and not <c>RimWorld.TexButton</c>: drawing a rotated horizontal
        /// arrow instead costs a pivot that has to be multiplied by <c>Prefs.UIScale</c>, and gets
        /// it wrong at any scale other than 100%.
        /// </summary>
        public static bool ArrowButton(Rect rect, bool up, bool enabled)
        {
            var texture = up ? TexButton.ReorderUp : TexButton.ReorderDown;
            var inner = rect.ContractedBy(2f);

            if (!enabled)
            {
                // No ButtonImage at all: a disabled arrow must define no clickable area, otherwise
                // it swallows the click and plays its sound at the end of the list.
                var color = GUI.color;
                GUI.color = new Color(color.r, color.g, color.b, 0.3f);
                GUI.DrawTexture(inner, texture);
                GUI.color = color;
                return false;
            }

            return Widgets.ButtonImage(inner, texture, Color.white, GenUI.MouseoverColor);
        }
    }
}
