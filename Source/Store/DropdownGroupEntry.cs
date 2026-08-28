using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Un groupe de menu deroulant cree par l'utilisateur. Les groupes fournis par le jeu ou par
    /// d'autres mods ne sont pas decrits ici : on les reference directement par leur defName.
    /// </summary>
    public class DropdownGroupEntry : IExposable
    {
        /// <summary>defName du <see cref="DesignatorDropdownGroupDef"/> recree a chaque demarrage.</summary>
        public string id;

        public string label;

        /// <summary>Menu en grille d'icones plutot qu'en liste, comme les groupes de sols vanilla.</summary>
        public bool useGridMenu;

        /// <summary>
        /// Quelle icone represente chaque entree du menu. On prend <c>Placed</c> par defaut, et non
        /// le <c>Cost</c> de vanilla : en mode grille, une entree dont le cout est indeterminable est
        /// silencieusement retiree du menu, alors que l'icone du batiment pose existe toujours.
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
