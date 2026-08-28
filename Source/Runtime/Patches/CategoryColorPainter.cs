using HarmonyLib;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Couleur des sous-categories dessinees par Better Architect Menu.
    ///
    /// Il ne les dessine pas comme des boutons mais avec un <c>Widgets.Label</c> nu : aucun point
    /// d'accroche propre. En revanche, juste avant chaque ligne, il demande l'icone de la categorie.
    /// On profite de cet appel pour armer la couleur, et on la consomme au premier libellé dessine
    /// ensuite - celui de la categorie. L'armement est a usage unique et la couleur remise a blanc
    /// aussitot, pour ne jamais deteindre sur le reste de l'interface.
    /// </summary>
    public static class CategoryColorPainter
    {
        private static Color? pending;
        private static bool applied;

        /// <summary>
        /// Coupe l'armement pendant nos propres lectures d'icone : sans cela, la fenetre Catategories
        /// armerait la couleur en interrogeant l'icone d'une ligne, et le libelle suivant, sans
        /// rapport, la recevrait.
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

        [HarmonyPatch(typeof(Widgets), nameof(Widgets.Label), typeof(Rect), typeof(string))]
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
