using System.Collections.Generic;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Ordre d'affichage voulu a l'interieur d'un groupe. Vaut aussi bien pour un groupe cree par
    /// l'utilisateur que pour un groupe fourni par le jeu ou par un mod, d'ou le stockage separe
    /// de <see cref="DropdownGroupEntry"/>.
    /// </summary>
    public class DropdownOrderEntry : IExposable
    {
        /// <summary>defName du <see cref="DesignatorDropdownGroupDef"/> concerne.</summary>
        public string groupId;

        /// <summary>Cles de batiments (voir <see cref="DropdownRuntime.KeyOf"/>), dans l'ordre voulu.</summary>
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
