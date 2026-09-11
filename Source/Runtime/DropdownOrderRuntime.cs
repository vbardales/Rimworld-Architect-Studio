using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Forces the chosen display order inside dropdown menus. The game itself fills each
    /// <see cref="Designator_Dropdown"/> in def database order, which we do not control.
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
        /// Sorts a list of buildings according to the order recorded for this group. The sort is
        /// stable: a building added afterwards, missing from the recorded order, ends up at the end
        /// without jostling the others.
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
        /// Reorders the dropdowns of a category we have just resolved. Called from the postfix on
        /// <c>ResolveDesignators</c>, so at startup as well as after every change.
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

                // The Architect menu button shows the active designator: we realign it on the first
                // entry, without marking the choice as explicit so the vanilla "..." is kept.
                dropdown.SetActiveDesignator(dropdown.Elements[0], explicitySet: false);
            }
        }

        /// <summary>
        /// A dropdown's group is stored nowhere: we find it back through the first of its
        /// buildings.
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
