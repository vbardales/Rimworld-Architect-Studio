using System.Collections.Generic;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Wanted display order inside a group. It applies just as well to a group created by the user
    /// as to one shipped by the game or by a mod, hence storing it apart from
    /// <see cref="DropdownGroupEntry"/>.
    /// </summary>
    public class DropdownOrderEntry : IExposable
    {
        /// <summary>defName of the <see cref="DesignatorDropdownGroupDef"/> concerned.</summary>
        public string groupId;

        /// <summary>Building keys (see <see cref="DropdownRuntime.KeyOf"/>), in the wanted order.</summary>
        public List<string> memberKeys = new List<string>();

        public DropdownOrderEntry()
        {
        }

        public DropdownOrderEntry(string groupId, List<string> memberKeys)
        {
            this.groupId = groupId;
            this.memberKeys = memberKeys ?? new List<string>();
        }

        public void ExposeData()
        {
            Scribe_Values.Look(ref groupId, "groupId");
            Scribe_Collections.Look(ref memberKeys, "memberKeys", LookMode.Value);

            if (memberKeys == null)
            {
                memberKeys = new List<string>();
            }
        }
    }
}
