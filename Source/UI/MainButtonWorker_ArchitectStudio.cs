using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>Optional shortcut; visibility is owned by the Def and customization tools.</summary>
    public sealed class MainButtonWorker_ArchitectStudio : MainButtonWorker
    {
        public override void Activate()
        {
            Find.WindowStack.Add(new Dialog_ModSettings(ArchitectStudioMod.Instance));
        }
    }
}
