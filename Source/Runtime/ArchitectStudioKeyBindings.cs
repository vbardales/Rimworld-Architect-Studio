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
    /// Opens the editor during a game. This is the only way to watch the Architect menu reorganise
    /// itself live: going through the mod settings stacks a modal window on top of it.
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
