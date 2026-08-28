using RimWorld;
using Verse;

namespace ArchitectStudio
{
    [DefOf]
    public static class ArchitectStudioKeyBindingDefOf
    {
        public static KeyBindingDef ArchitectStudio_OpenDropdowns;

        static ArchitectStudioKeyBindingDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(ArchitectStudioKeyBindingDefOf));
        }
    }

    /// <summary>
    /// Ouvre l'editeur en cours de partie. C'est la seule facon de voir le menu Architecte se
    /// reorganiser en direct : passer par les reglages du mod empile une fenetre modale par-dessus.
    /// </summary>
    public class GameComponent_ArchitectStudio : GameComponent
    {
        public GameComponent_ArchitectStudio(Game game)
        {
        }

        public override void GameComponentOnGUI()
        {
            if (!ArchitectStudioKeyBindingDefOf.ArchitectStudio_OpenDropdowns.KeyDownEvent)
            {
                return;
            }

            ArchitectStudioUI.ToggleDropdownDialog();
        }
    }
}
