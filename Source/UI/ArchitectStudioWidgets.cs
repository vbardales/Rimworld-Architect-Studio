using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioWidgets
    {
        /// <summary>
        /// Bouton fleche haut ou bas. Le jeu ne fournit que des fleches horizontales
        /// (<c>TexUI.ArrowTexRight</c>) : on les fait pivoter au dessin plutot que de dependre d'un
        /// glyphe Unicode, que la police du jeu ne rend pas forcement.
        /// </summary>
        public static bool ArrowButton(Rect rect, bool up, bool enabled)
        {
            var color = GUI.color;
            GUI.color = enabled ? color : new Color(color.r, color.g, color.b, 0.3f);

            if (enabled)
            {
                Widgets.DrawHighlightIfMouseover(rect);
            }

            var matrix = GUI.matrix;
            // ArrowTexRight pointe vers la droite : +90 degres l'envoie vers le bas, -90 vers le haut.
            GUIUtility.RotateAroundPivot(up ? -90f : 90f, rect.center);
            GUI.DrawTexture(rect.ContractedBy(3f), TexUI.ArrowTexRight);
            GUI.matrix = matrix;

            GUI.color = color;

            return enabled && Widgets.ButtonInvisible(rect);
        }
    }
}
