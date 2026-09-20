using System.Linq;
using System.Threading.Tasks;
using RimWorks.Pickle;
using Verse;

namespace ArchitectStudio.PickleSteps
{
    /// <summary>Dropdown groups: creating, filling, ordering, forcing a category, deleting.</summary>
    [PickleSteps]
    public class GroupSteps
    {
        // ---------------------------------------------------------------- setup and mutations

        [When("I create the group {string}")]
        public void CreateGroup(PickleContext ctx, string label)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Call(ctx, dialog, "CreateGroup", label);
            ctx.Assert(ArchitectStudioMod.Settings.customGroups.Any(e => e.label == label),
                $"the group '{label}' should be in the settings; groups there: " +
                string.Join(", ", ArchitectStudioMod.Settings.customGroups.Select(e => e.label)));
        }

        [When("I add {string} to the group {string}")]
        public void AddMember(PickleContext ctx, string defName, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Call(ctx, dialog, "Assign", Driver.Buildable(ctx, defName), Driver.Group(ctx, groupLabel));
        }


        /// <summary>
        /// What clicking a group's row in the left column does. A capture taken without this shows
        /// both right-hand columns on their "select a group" placeholder, which says nothing about
        /// what the editor is for.
        /// </summary>
        [When("I select the group {string} in the editor")]
        public async Task SelectGroup(PickleContext ctx, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Select(ctx, dialog, Driver.Group(ctx, groupLabel));
            await ctx.WaitFrames(3);
        }
        [When("I remove {string} from its group")]
        public void RemoveMember(PickleContext ctx, string defName)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Call(ctx, dialog, "Assign", Driver.Buildable(ctx, defName), null);
        }

        [When("I force the category {string} on the group {string}")]
        public void ForceCategory(PickleContext ctx, string categoryDefName, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Call(ctx, dialog, "SetGroupCategory", Driver.Group(ctx, groupLabel), Driver.Category(ctx, categoryDefName));
        }

        [When("I clear the forced category of the group {string}")]
        public void ClearCategory(PickleContext ctx, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            Driver.Call(ctx, dialog, "SetGroupCategory", Driver.Group(ctx, groupLabel), null);
        }

        [When("I delete the group {string}")]
        public void DeleteGroup(PickleContext ctx, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            var group = Driver.Group(ctx, groupLabel);
            var custom = ArchitectStudioMod.Settings.customGroups.Any(e => e.id == group.defName);
            // The editor's delete button picks between the two the same way.
            Driver.Call(ctx, dialog, custom ? "DeleteGroup" : "DissolveGroup", group);
        }

        [When("I restore the deleted groups")]
        public void RestoreHidden(PickleContext ctx)
        {
            // Exactly what the button calls, nothing more: whether the members come back is the
            // question the scenario asks.
            Driver.Call(ctx, Driver.GroupEditor(ctx), "RestoreHiddenGroups");
        }

        /// <summary>
        /// Replays a drop through the callback the editor registered on its last repaint. The index
        /// is where ReorderableWidget says the row lands: an insertion index, counted before the
        /// dragged row is removed, so dropping below the last of four rows is 4.
        /// </summary>
        /// <summary>
        /// Setup only. Without a saved order a group lists its members in def database order, which
        /// depends on the mod list; the scenarios need a known starting order. Same calls as the
        /// editor's ReorderMember makes once it has computed the list.
        /// </summary>
        [Given("the group {string} is ordered {string}")]
        public void SetOrder(PickleContext ctx, string groupLabel, string commaSeparated)
        {
            var group = Driver.Group(ctx, groupLabel);
            var members = Driver.MembersOf(group);
            var wanted = commaSeparated.Split(',').Select(s => Driver.DefName(ctx, s.Trim())).ToList();
            ctx.Require(wanted.Count == members.Count && wanted.All(n => members.Any(m => m.defName == n)),
                $"'{groupLabel}' holds {Driver.Names(ctx, members)}, which is not the set [{commaSeparated}]");

            DropdownOrderRuntime.SetOrder(group.defName, wanted.Select(n => members.First(m => m.defName == n)));
            ArchitectStudioMod.Instance.WriteSettings();
            DropdownRuntime.RebuildCategoriesOf(group);

            // The editor caches its member lists; drop them so it redraws from the new order.
            Driver.Call(ctx, Driver.GroupEditor(ctx), "InvalidateCaches");
        }

        [When("I drag member {int} of the group {string} to insertion index {int}")]
        public async Task Drag(PickleContext ctx, int from, string groupLabel, int insertion)
        {
            ReorderCapture.Ensure();
            var dialog = Driver.GroupEditor(ctx);
            Driver.Select(ctx, dialog, Driver.Group(ctx, groupLabel));

            // Let the member column repaint with the group selected, so it registers its callback.
            await ctx.WaitFrames(3);

            var id = Driver.MemberReorderGroup(dialog);
            ctx.Require(id >= 0 && ReorderCapture.Actions.ContainsKey(id),
                $"the member column registered no reorder callback (group id {id}): the drag could never start, " +
                "which is the local-variable bug scenario 3 describes");

            // Members are 1-based in the scenario text, as a player counts rows.
            ReorderCapture.Actions[id](from - 1, insertion);
        }

        [When("I press the {word} arrow on member {int} of the group {string}")]
        public void Arrow(PickleContext ctx, string direction, int member, string groupLabel)
        {
            var dialog = Driver.GroupEditor(ctx);
            var index = member - 1;
            var target = direction == "up" ? index - 1 : direction == "down" ? index + 1 : int.MinValue;
            ctx.Require(target != int.MinValue, $"direction must be 'up' or 'down', not '{direction}'");
            Driver.Call(ctx, dialog, "ReorderMember", Driver.Group(ctx, groupLabel), index, target);
        }

        // ---------------------------------------------------------------- checks

        [Then("the group {string} holds {string} in that order")]
        public void MembersInOrder(PickleContext ctx, string groupLabel, string commaSeparated)
        {
            var expected = commaSeparated.Split(',').Select(s => s.Trim()).ToList();
            var actual = Driver.MembersOf(Driver.Group(ctx, groupLabel)).Select(d => Driver.NameOf(ctx, d)).ToList();
            ctx.Assert(actual.SequenceEqual(expected),
                $"the editor should list [{string.Join(", ", expected)}]; it lists [{string.Join(", ", actual)}]");
        }

        [Then("the Architect menu shows the group {string} as {int} button(s)")]
        public void ButtonCount(PickleContext ctx, string groupLabel, int count)
        {
            var group = Driver.Group(ctx, groupLabel);
            var buttons = Driver.MenuButtonsOf(group);
            ctx.Assert(buttons.Count == count,
                $"the group '{groupLabel}' should draw {count} button(s); it draws {buttons.Count}: " +
                string.Join(" / ", buttons.Select(b => Driver.Names(ctx, Driver.MenuOrderOf(b)))));
        }

        [Then("the Architect menu lists the group {string} in the same order as the editor")]
        public void MenuMatchesEditor(PickleContext ctx, string groupLabel)
        {
            var group = Driver.Group(ctx, groupLabel);
            var buttons = Driver.MenuButtonsOf(group);
            ctx.Require(buttons.Count == 1, $"expected the group on one button to compare orders; it is on {buttons.Count}");
            var menu = Driver.MenuOrderOf(buttons[0]);
            var editor = Driver.MembersOf(group);
            ctx.Assert(menu.SequenceEqual(editor),
                $"menu order {Driver.Names(ctx, menu)} should match editor order {Driver.Names(ctx, editor)}");
        }

        [Then("{string} is a button of its own in its category")]
        public void Standalone(PickleContext ctx, string defName)
        {
            var def = Driver.Buildable(ctx, defName);
            ctx.Assert(def.designatorDropdown == null && Driver.IsStandaloneIn(def, def.designationCategory),
                $"'{defName}' should be its own button in '{def.designationCategory?.defName}'; it is in group " +
                $"'{def.designatorDropdown?.defName ?? "none"}'");
        }

        [Then("{string} is back in its original category")]
        public void BackHome(PickleContext ctx, string defName)
        {
            var def = Driver.Buildable(ctx, defName);
            var original = DropdownRuntime.OriginalCategoryOf(def);
            ctx.Assert(def.designationCategory == original,
                $"'{defName}' should be back in '{original?.defName}'; it is in '{def.designationCategory?.defName}'");
        }

        [Then("{string} sits in the category {string}")]
        public void InCategory(PickleContext ctx, string defName, string categoryDefName)
        {
            var def = Driver.Buildable(ctx, defName);
            ctx.Assert(def.designationCategory?.defName == categoryDefName,
                $"'{defName}' should be in '{categoryDefName}'; it is in '{def.designationCategory?.defName}'");
        }

        [Given("{string} and {string} start in different categories")]
        public void DifferentCategories(PickleContext ctx, string a, string b)
        {
            var defA = Driver.Buildable(ctx, a);
            var defB = Driver.Buildable(ctx, b);
            ctx.Require(defA.designationCategory != defB.designationCategory,
                $"this scenario needs two categories, but another mod put '{a}' and '{b}' both in " +
                $"'{defA.designationCategory?.defName}': pick other buildings for this mod list");
            ctx.Set(defA.designationCategory);
        }

        /// <summary>
        /// Picks the buildings from the running game instead of naming vanilla ones, which mods move
        /// around. The first category, by defName, holding four buildings that each draw their own
        /// button; within it the first four by defName. Same mod list, same four buildings.
        /// </summary>
        [Given("four buildings {string}, {string}, {string} and {string} from one category, in no group")]
        public void FourOfOneCategory(PickleContext ctx, string a, string b, string c, string d)
        {
            var names = new[] { a, b, c, d };
            var picked = DropdownRuntime.AllBuildables()
                .Where(x => x.designationCategory != null && x.designatorDropdown == null &&
                            Driver.IsStandaloneIn(x, x.designationCategory))
                .GroupBy(x => x.designationCategory)
                .OrderBy(g => g.Key.defName)
                .Select(g => g.OrderBy(x => x.defName).Take(names.Length).ToList())
                .FirstOrDefault(g => g.Count == names.Length);
            ctx.Require(picked != null, "no Architect category holds four buildings outside any group");

            var aliases = new Driver.Aliases();
            for (var i = 0; i < names.Length; i++)
            {
                aliases.ByName[names[i]] = picked[i];
            }

            ctx.Set(aliases);
            ctx.Attach("buildings picked", string.Join(", ", names.Select((n, i) => $"{n} = {picked[i].defName}")) +
                $" in {picked[0].designationCategory.defName}");
        }

        [Then("no building belongs to the group {string}")]
        public void Empty(PickleContext ctx, string groupLabel)
        {
            var members = Driver.MembersOf(Driver.Group(ctx, groupLabel));
            ctx.Assert(members.Count == 0, $"the group should be empty; it holds {Driver.Names(ctx, members)}");
        }

        [Then("the group {string} has members again")]
        public void HasMembers(PickleContext ctx, string groupLabel)
        {
            var members = Driver.MembersOf(Driver.Group(ctx, groupLabel));
            ctx.Assert(members.Count > 0, "the group should have its members back; it holds none");
        }

        [Then("the group {string} is remembered as deleted")]
        public void Hidden(PickleContext ctx, string groupLabel)
        {
            var group = Driver.Group(ctx, groupLabel);
            ctx.Assert(ArchitectStudioMod.Settings.hiddenGroupIds.Contains(group.defName),
                $"'{group.defName}' should be in hiddenGroupIds; that list holds: " +
                string.Join(", ", ArchitectStudioMod.Settings.hiddenGroupIds));
        }

        [Given("the group {string} has at least {int} members")]
        public void AtLeast(PickleContext ctx, string groupLabel, int count)
        {
            var members = Driver.MembersOf(Driver.Group(ctx, groupLabel));
            ctx.Require(members.Count >= count, $"'{groupLabel}' needs {count} members here; it has {Driver.Names(ctx, members)}");
        }
    }
}
