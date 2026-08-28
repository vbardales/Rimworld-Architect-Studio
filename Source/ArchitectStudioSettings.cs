
using System.Collections.Generic;
using Verse;

namespace ArchitectStudio
{
    public class ArchitectStudioSettings : ModSettings
    {
        /// <summary>Version du schema de configuration, pour migrer sans casser les reglages existants.</summary>
        public int schemaVersion = 1;

        /// <summary>
        /// Affiche un bouton d'ouverture dans la fenetre Architecte. Active par defaut : sans clavier,
        /// c'est le seul acces a l'editeur en cours de partie.
        /// </summary>
        public bool showArchitectButton = true;

        /// <summary>Groupes de menus deroulants crees par l'utilisateur.</summary>
        public List<DropdownGroupEntry> customGroups = new List<DropdownGroupEntry>();

        /// <summary>
        /// Cle de batiment (voir <see cref="DropdownRuntime.KeyOf"/>) vers defName du groupe.
        /// Une chaine vide signifie explicitement "aucun groupe" ; une cle absente signifie
        /// "laisser la valeur d'origine du def".
        /// </summary>
        public Dictionary<string, string> dropdownAssignments = new Dictionary<string, string>();

        /// <summary>Ordre d'affichage voulu a l'interieur de certains groupes.</summary>
        public List<DropdownOrderEntry> groupOrders = new List<DropdownOrderEntry>();
        /// <summary>
        /// Ordre impose a certaines categories : defName vers valeur de <c>DesignationCategoryDef.order</c>.
        /// Les boutons haut/bas reecrivent ce champ, que le menu vanilla comme les listes de
        /// sous-categories de Better Architect Menu utilisent pour trier.
        /// </summary>
        public Dictionary<string, int> categoryOrders = new Dictionary<string, int>();
        /// <summary>Categories creees par l'utilisateur, recreees a chaque demarrage.</summary>
        public List<CustomCategoryEntry> customCategories = new List<CustomCategoryEntry>();

        /// <summary>Libelle de remplacement d'une categorie : defName vers libelle.</summary>
        public Dictionary<string, string> categoryLabels = new Dictionary<string, string>();

        /// <summary>Icone choisie pour une categorie : defName vers chemin de texture.</summary>
        public Dictionary<string, string> categoryIcons = new Dictionary<string, string>();

        /// <summary>Couleur choisie pour une categorie : defName vers "r,g,b" en 0-255.</summary>
        public Dictionary<string, string> categoryColors = new Dictionary<string, string>();



        /// <summary>
        /// Categorie imposee a un groupe entier : defName du groupe vers defName de la categorie.
        /// Sans cette entree, un groupe n'a pas de categorie propre - ce sont ses membres qui portent
        /// la leur, et le groupe se retrouve la ou ils sont, quitte a se scinder en plusieurs boutons.
        /// </summary>
        public Dictionary<string, string> groupCategories = new Dictionary<string, string>();

        /// <summary>
        /// Groupes fournis par le jeu ou par un mod que l'utilisateur a supprimes. On ne peut pas
        /// effacer le def lui-meme : il est relu de son XML a chaque demarrage. On le vide donc de
        /// ses membres et on retient ici qu'il ne doit plus apparaitre.
        /// </summary>
        public List<string> hiddenGroupIds = new List<string>();

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref schemaVersion, "schemaVersion", 1);
            Scribe_Values.Look(ref showArchitectButton, "showArchitectButton", true);
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
