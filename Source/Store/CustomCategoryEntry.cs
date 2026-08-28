using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Une categorie creee par l'utilisateur. Le def n'existe qu'en memoire : il est reconstruit a
    /// chaque demarrage a partir de cette fiche.
    /// </summary>
    public class CustomCategoryEntry : IExposable
    {
        public string id;
        public string label;

        /// <summary>defName de la categorie parente, ou vide pour une categorie de premier niveau.</summary>
        public string parentId;

        public CustomCategoryEntry()
        {
        }

        public CustomCategoryEntry(string id, string label, string parentId)
        {
            this.id = id;
            this.label = label;
            this.parentId = parentId;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "id");
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref parentId, "parentId");
        }
    }
}
