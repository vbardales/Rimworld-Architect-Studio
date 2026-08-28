using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Impose l'ordre d'affichage choisi a l'interieur des menus deroulants. Le jeu, lui, remplit
    /// chaque <see cref="Designator_Dropdown"/> dans l'ordre de la base de defs, qu'on ne controle pas.
    /// </summary>
    public static class DropdownOrderRuntime
    {
        public static List<string> OrderFor(string groupId)
        {
            return ArchitectStudioMod.Settings.groupOrders
                .FirstOrDefault(o => o.groupId == groupId)?.memberKeys;
        }

        public static bool HasOrder(string groupId)
        {
            var order = OrderFor(groupId);
            return order != null && order.Count > 0;
        }

        public static void SetOrder(string groupId, IEnumerable<BuildableDef> members)
        {
            var keys = members.Select(DropdownRuntime.KeyOf).ToList();
            var entry = ArchitectStudioMod.Settings.groupOrders.FirstOrDefault(o => o.groupId == groupId);

            if (entry == null)
            {
                ArchitectStudioMod.Settings.groupOrders.Add(new DropdownOrderEntry(groupId, keys));
            }
            else
            {
                entry.memberKeys = keys;
            }
        }

        public static void ClearOrder(string groupId)
        {
            ArchitectStudioMod.Settings.groupOrders.RemoveAll(o => o.groupId == groupId);
        }

        /// <summary>
        /// Trie une liste de batiments selon l'ordre enregistre pour ce groupe. Le tri est stable :
        /// un batiment ajoute apres coup, absent de l'ordre enregistre, se retrouve a la fin sans
        /// bousculer les autres.
        /// </summary>
        public static List<BuildableDef> SortMembers(string groupId, List<BuildableDef> members)
        {
            var order = OrderFor(groupId);
            if (order == null || order.Count == 0)
            {
                return members;
            }

            return members
                .OrderBy(def =>
                {
                    var index = order.IndexOf(DropdownRuntime.KeyOf(def));
                    return index < 0 ? int.MaxValue : index;
                })
                .ToList();
        }

        /// <summary>
        /// Reordonne les dropdowns d'une categorie qu'on vient de resoudre. Appele depuis le postfix
        /// sur <c>ResolveDesignators</c>, donc aussi bien au demarrage qu'apres chaque modification.
        /// </summary>
        public static void ApplyOrder(DesignationCategoryDef category)
        {
            if (category?.AllResolvedDesignators == null)
            {
                return;
            }

            foreach (var designator in category.AllResolvedDesignators)
            {
                if (!(designator is Designator_Dropdown dropdown) || dropdown.Elements.Count < 2)
                {
                    continue;
                }

                var group = GroupOf(dropdown);
                if (group == null || !HasOrder(group.defName))
                {
                    continue;
                }

                var order = OrderFor(group.defName);
                var sorted = dropdown.Elements
                    .OrderBy(element =>
                    {
                        var def = (element as Designator_Build)?.PlacingDef;
                        if (def == null)
                        {
                            return int.MaxValue;
                        }

                        var index = order.IndexOf(DropdownRuntime.KeyOf(def));
                        return index < 0 ? int.MaxValue : index;
                    })
                    .ToList();

                dropdown.Elements.Clear();
                dropdown.Elements.AddRange(sorted);

                // Le bouton du menu Architecte affiche le designator actif : on le realigne sur la
                // premiere entree, sans marquer le choix comme explicite pour garder les "..." vanilla.
                dropdown.SetActiveDesignator(dropdown.Elements[0], explicitySet: false);
            }
        }

        /// <summary>
        /// Le groupe d'un dropdown n'est stocke nulle part : on le retrouve via le premier de ses
        /// batiments.
        /// </summary>
        private static DesignatorDropdownGroupDef GroupOf(Designator_Dropdown dropdown)
        {
            foreach (var element in dropdown.Elements)
            {
                var group = (element as Designator_Build)?.PlacingDef?.designatorDropdown;
                if (group != null)
                {
                    return group;
                }
            }

            return null;
        }
    }
}
