using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// A category created by the user. The def only exists in memory: it is rebuilt at every
    /// startup from this record.
    /// </summary>
    public class CustomCategoryEntry : IExposable
    {
        public string id;
        public string label;

        /// <summary>defName of the parent category, or empty for a top-level category.</summary>
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
