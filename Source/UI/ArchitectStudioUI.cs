using Verse;

namespace ArchitectStudio
{
    public static class ArchitectStudioUI
    {
        /// <summary>
        /// Opens the dropdown group editor, or closes it if it is already there. Single entry point
        /// for the Architect menu button, the key binding and the mod settings.
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
