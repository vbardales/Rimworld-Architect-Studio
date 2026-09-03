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
            var texture = up ? TexButton.ReorderUp : TexButton.ReorderDown;
            var inner = rect.ContractedBy(2f);

            if (!enabled)
            {
                // Pas de ButtonImage du tout : une fleche inactive ne doit poser aucune zone
                // cliquable, sinon elle avale le clic et joue son son au bout de la liste.
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
