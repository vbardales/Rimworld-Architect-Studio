
using System.Collections.Generic;
using Verse;

namespace ArchitectStudio
{
    public class ArchitectStudioSettings : ModSettings
    {
        /// <summary>Configuration schema version, to migrate without breaking existing settings.</summary>
        public int schemaVersion = 1;

        /// <summary>
        /// Shows an opening button in the Architect window. On by default: without a keyboard, it
        /// is the only way into the editor during a game.
        /// </summary>
        public bool showArchitectButton = true;

        /// <summary>Dropdown groups created by the user.</summary>
        public List<DropdownGroupEntry> customGroups = new List<DropdownGroupEntry>();

        /// <summary>
        /// Building key (see <see cref="DropdownRuntime.KeyOf"/>) to group defName.
        /// An empty string explicitly means "no group"; a missing key means "leave the def's
        /// original value alone".
        /// </summary>
        public Dictionary<string, string> dropdownAssignments = new Dictionary<string, string>();

        /// <summary>Wanted display order inside some groups.</summary>
        public List<DropdownOrderEntry> groupOrders = new List<DropdownOrderEntry>();
        /// <summary>
        /// Order forced on some categories: defName to <c>DesignationCategoryDef.order</c> value.
        /// The up/down buttons rewrite this field, which both the vanilla menu and Better Architect
        /// Menu's subcategory lists use to sort.
        /// </summary>
        public Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
        /// <summary>Categories created by the user, recreated at every startup.</summary>
        public List<CustomCategoryEntry> customCategories = new List<CustomCategoryEntry>();

        /// <summary>Replacement label for a category: defName to label.</summary>
        public Dictionary<string, string> categoryLabels = new Dictionary<string, string>();

        /// <summary>Icon chosen for a category: defName to texture path.</summary>
        public Dictionary<string, string> categoryIcons = new Dictionary<string, string>();

        /// <summary>Colour chosen for a category: defName to "r,g,b" in 0-255.</summary>
        public Dictionary<string, string> categoryColors = new Dictionary<string, string>();



        /// <summary>
        /// Category forced on a whole group: group defName to category defName. Without this entry
        /// a group has no category of its own - its members carry theirs, and the group ends up
        /// wherever they are, splitting across several buttons if need be.
        /// </summary>
        public Dictionary<string, string> groupCategories = new Dictionary<string, string>();
        /// <summary>
        /// Shows the buildings and categories research still locks, greyed out and not buildable,
        /// so they can be sorted before being unlocked.
        /// </summary>
        public bool showResearchLocked;


        /// <summary>
        /// Groups shipped by the game or by a mod that the user has deleted. The def itself cannot
        /// be erased: it is read back from its XML at every startup. So we empty it of its members
        /// and remember here that it must no longer appear.
        /// </summary>
        public List<string> hiddenGroupIds = new List<string>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref schemaVersion, "schemaVersion", 1);
            Scribe_Values.Look(ref showArchitectButton, "showArchitectButton", true);
            Scribe_Values.Look(ref showResearchLocked, "showResearchLocked", false);
            Scribe_Collections.Look(ref customGroups, "customGroups", LookMode.Deep);
            Scribe_Collections.Look(ref dropdownAssignments, "dropdownAssignments", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref groupOrders, "groupOrders", LookMode.Deep);
            Scribe_Collections.Look(ref hiddenGroupIds, "hiddenGroupIds", LookMode.Value);
            Scribe_Collections.Look(ref groupCategories, "groupCategories", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref categoryOrders, "categoryOrders", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref customCategories, "customCategories", LookMode.Deep);
            Scribe_Collections.Look(ref categoryLabels, "categoryLabels", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref categoryIcons, "categoryIcons", LookMode.Value, LookMode.Value);
            Scribe_Collections.Look(ref categoryColors, "categoryColors", LookMode.Value, LookMode.Value);

            if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                if (customGroups == null)
                {
                    customGroups = new List<DropdownGroupEntry>();
                }
                if (dropdownAssignments == null)
                {
                    dropdownAssignments = new Dictionary<string, string>();
                }
                if (groupOrders == null)
                {
                    groupOrders = new List<DropdownOrderEntry>();
                }
                if (hiddenGroupIds == null)
                {
                    hiddenGroupIds = new List<string>();
                }
                if (groupCategories == null)
                {
                    groupCategories = new Dictionary<string, string>();
                }
                if (categoryOrders == null)
                {
                    categoryOrders = new Dictionary<string, int>();
                }
                if (customCategories == null)
                {
                    customCategories = new List<CustomCategoryEntry>();
                }
                if (categoryLabels == null)
                {
                    categoryLabels = new Dictionary<string, string>();
                }
                if (categoryIcons == null)
                {
                    categoryIcons = new Dictionary<string, string>();
                }
                if (categoryColors == null)
                {
                    categoryColors = new Dictionary<string, string>();
                }
            }

            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                customGroups.RemoveAll(g => g == null || g.id.NullOrEmpty());
                groupOrders.RemoveAll(o => o == null || o.groupId.NullOrEmpty());
                customCategories.RemoveAll(c => c == null || c.id.NullOrEmpty());
            }
        }
    }
}
