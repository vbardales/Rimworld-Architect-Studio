using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Applies the configuration once the defs are loaded. ExecuteWhenFinished puts us after the
    /// ResolveDesignators calls the game queues itself during ResolveReferences. The order carries
    /// no consequence: we mutate the def's field, so any later re-resolution, ours or the game's,
    /// produces the same result.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class StartupInit
    {
        static StartupInit()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                // First, so that a runtime refusing non-public access is named in the log before
                // anything that depends on it starts failing.
                AccessCheckProbe.Run();

                // Patches deferred until here: these callbacks run on the main thread - it is the
                // same mechanism the game uses to call StaticConstructorOnStartupUtility.CallAll -
                // whereas the mod constructor runs on a background thread, where opening any
                // resource fails.
                ArchitectIconsCompat.ApplyPatch(ArchitectStudioMod.HarmonyInstance);
                CategoryColorPainter.ApplyPatch(ArchitectStudioMod.HarmonyInstance);

                // Created categories must exist before anything else: ordering, labels and groups
                // can all reference them.
                CustomCategoryRuntime.EnsureDefs();
                CategoryAppearance.ApplyLabels();
                CategoryRuntime.Apply();
                DropdownRuntime.Apply();
            });
        }
    }
}
