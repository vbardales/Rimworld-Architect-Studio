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

            // Passer par Widgets.DrawTextureRotated, et non par GUIUtility.RotateAroundPivot :
            // le jeu applique l'echelle d'interface via GUI.matrix, et son propre
            // UI.RotateAroundPivot multiplie le pivot par Prefs.UIScale. Sans ce facteur, la
            // rotation est juste a 100 % et part ailleurs des 125 %.
            // ArrowTexRight pointe vers la droite : +90 degres l'envoie vers le bas, -90 vers le haut.
            Widgets.DrawTextureRotated(rect.ContractedBy(3f), TexUI.ArrowTexRight, up ? -90f : 90f);

            GUI.color = color;

            return enabled && Widgets.ButtonInvisible(rect);
        }
    }
}
