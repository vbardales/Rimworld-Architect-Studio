using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Applique la config une fois les defs charges. On passe par ExecuteWhenFinished pour se placer
    /// apres les ResolveDesignators que le jeu met lui-meme en file pendant ResolveReferences.
    /// L'ordre reste sans consequence : on mute le champ du def, donc toute re-resolution ulterieure,
    /// la notre ou celle du jeu, produit le meme resultat.
    /// </summary>
    [StaticConstructorOnStartup]
    public static class StartupInit
    {
        static StartupInit()
        {
            LongEventHandler.ExecuteWhenFinished(delegate
            {
                // Les categories creees doivent exister avant tout le reste : l'ordre, les libelles
                // et les groupes peuvent les referencer.
                CustomCategoryRuntime.EnsureDefs();
                CategoryAppearance.ApplyLabels();
                CategoryRuntime.Apply();
                DropdownRuntime.Apply();
            });
        }
    }
}
