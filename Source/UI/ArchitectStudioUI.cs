using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioUI
    {
        /// <summary>
        /// Ouvre l'editeur de groupes deroulants, ou le referme s'il est deja la. Point d'entree
        /// unique du bouton du menu Architecte, du raccourci clavier et des reglages du mod.
        /// </summary>
        public static void ToggleDropdownDialog() => Toggle<Dialog_DropdownGroups>(() => new Dialog_DropdownGroups());

        public static void ToggleCategoriesDialog() => Toggle<Dialog_Categories>(() => new Dialog_Categories());

        private static void Toggle<T>(System.Func<Window> create) where T : Window
        {
            var existing = Find.WindowStack.WindowOfType<T>();
            if (existing != null)
            {
                existing.Close();
            }
            else
            {
                Find.WindowStack.Add(create());
            }
        }
    }
}
