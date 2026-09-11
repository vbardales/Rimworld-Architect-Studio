using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// A dropdown group created by the user. Groups shipped by the game or by other mods are not
    /// described here: they are referenced directly by their defName.
    /// </summary>
    public class DropdownGroupEntry : IExposable
    {
        /// <summary>defName of the <see cref="DesignatorDropdownGroupDef"/> rebuilt at every startup.</summary>
        public string id;

        public string label;

        /// <summary>Icon grid menu rather than a list, like the vanilla floor groups.</summary>
        public bool useGridMenu;

        /// <summary>
        /// Which icon stands for each menu entry. We take <c>Placed</c> by default rather than
        /// vanilla's <c>Cost</c>: in grid mode, an entry whose cost cannot be determined is
        /// silently dropped from the menu, whereas the placed building's icon always exists.
        /// </summary>
        public DesignatorDropdownGroupDef.IconSource iconSource = DesignatorDropdownGroupDef.IconSource.Placed;

        public DropdownGroupEntry()
        {
        }

        public DropdownGroupEntry(string id, string label)
        {
            this.id = id;
            this.label = label;
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref id, "id");
            Scribe_Values.Look(ref label, "label");
            Scribe_Values.Look(ref useGridMenu, "useGridMenu", false);
            Scribe_Values.Look(ref iconSource, "iconSource", DesignatorDropdownGroupDef.IconSource.Placed);
        }
    }
}
