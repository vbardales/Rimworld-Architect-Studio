using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Editeur des groupes de menus deroulants : trois colonnes - les groupes, les batiments du
    /// groupe selectionne, et de quoi en ajouter. Chaque modification est appliquee immediatement.
    /// La fenetre n'est volontairement ni modale ni bloquante, pour qu'on voie le menu Architecte
    /// se reorganiser en direct derriere.
    /// </summary>
    public class Dialog_DropdownGroups : Window
    {
        private const float RowHeight = 30f;
        private const float ColumnGap = 12f;
        private const float HeaderHeight = 28f;
        private const float SearchHeight = 28f;
        private const float ButtonHeight = 30f;
        private const float RemoveButtonSize = 22f;
        private const int MaxAddResults = 200;

        private DesignatorDropdownGroupDef selectedGroup;
        private string groupSearch = "";
        private string addSearch = "";

        /// <summary>
        /// Filtre de categorie de la colonne d'ajout. Tant que l'utilisateur n'a rien choisi
        /// explicitement, il suit la categorie du groupe selectionne - et retombe sur "toutes"
        /// quand le groupe est encore vide.
        /// </summary>
        private DesignationCategoryDef addCategoryFilter;
        private bool addCategoryFilterSet;

        private Vector2 groupScroll;
        private Vector2 memberScroll;
        private Vector2 addScroll;

        private List<DesignatorDropdownGroupDef> groupsCache;
        private Dictionary<DesignatorDropdownGroupDef, List<BuildableDef>> membersCache;

        /// <summary>
        /// Identifiant du groupe de reordonnancement. Il doit vivre dans un champ, pas dans une
        /// variable locale : <c>NewGroup</c> ne renvoie une valeur qu'au Repaint et -1 partout
        /// ailleurs, or <c>Reorderable</c> memorise cet identifiant au MouseDown pour decider, au
        /// Repaint suivant, si le drag demarre. Avec une locale, il enregistrerait -1 et le
        /// glisser-deposer ne partirait jamais. Meme montage que Page_ConfigureStartingPawns.
        /// </summary>
        private int memberReorderGroup = -1;

        /// <summary>Bornee a l'ecran : sur un 1280x800 de Steam Deck, une taille fixe deborderait.</summary>
        public override Vector2 InitialSize => new Vector2(
            Mathf.Min(1120f, UI.screenWidth - 40f),
            Mathf.Min(740f, UI.screenHeight - 80f));

        public Dialog_DropdownGroups()
        {
            doCloseX = true;
            draggable = true;
            resizeable = true;
            forcePause = false;
            absorbInputAroundWindow = false;
            closeOnClickedOutside = false;
        }

        // ---------------------------------------------------------------- caches

        private void InvalidateCaches()
        {
            groupsCache = null;
            membersCache = null;
        }

        /// <summary>
        /// Un groupe supprime reste dans la DefDatabase - son mod le recree a chaque demarrage - mais
        /// disparait de la liste. Il ressort si jamais il retrouve des membres, pour qu'une suppression
        /// oubliee ne masque pas silencieusement du contenu ajoute plus tard par un mod.
        /// </summary>
        private static bool IsHidden(DesignatorDropdownGroupDef group)
        {
            return ArchitectStudioMod.Settings.hiddenGroupIds.Contains(group.defName);
        }

        private List<DesignatorDropdownGroupDef> Groups =>
            groupsCache ??= DefDatabase<DesignatorDropdownGroupDef>.AllDefsListForReading
                .Where(g => !IsHidden(g) || MembersOf(g).Count > 0)
                .OrderBy(GroupLabel)
                .ToList();

        private Dictionary<DesignatorDropdownGroupDef, List<BuildableDef>> Members
        {
            get
            {
                if (membersCache != null)
                {
                    return membersCache;
                }

                membersCache = new Dictionary<DesignatorDropdownGroupDef, List<BuildableDef>>();
                foreach (var def in DropdownRuntime.AllBuildables())
                {
                    var group = def.designatorDropdown;
                    if (group == null)
                    {
                        continue;
                    }

                    if (!membersCache.TryGetValue(group, out var list))
                    {
                        membersCache[group] = list = new List<BuildableDef>();
                    }

                    list.Add(def);
                }

                // On trie une fois a la construction : l'affichage doit refleter l'ordre reel du menu.
                foreach (var group in membersCache.Keys.ToList())
                {
                    membersCache[group] = DropdownOrderRuntime.SortMembers(group.defName, membersCache[group]);
                }

                return membersCache;
            }
        }

        private List<BuildableDef> MembersOf(DesignatorDropdownGroupDef group)
        {
            if (group == null)
            {
                return new List<BuildableDef>();
            }

            return Members.TryGetValue(group, out var list) ? list : new List<BuildableDef>();
        }

        private static string GroupLabel(DesignatorDropdownGroupDef group)
        {
            return group.label.NullOrEmpty() ? group.defName : group.LabelCap.ToString();
        }

        private static DropdownGroupEntry CustomEntry(DesignatorDropdownGroupDef group)
        {
            return ArchitectStudioMod.Settings.customGroups.FirstOrDefault(e => e.id == group.defName);
        }

        private static bool IsCustom(DesignatorDropdownGroupDef group) => CustomEntry(group) != null;

        // ---------------------------------------------------------------- mutations

        /// <summary>
        /// Affecte un batiment a un groupe. Repasser a la valeur d'origine supprime l'override
        /// plutot que de l'enregistrer, pour que la config ne retienne que les vrais ecarts.
        /// </summary>
        private void Assign(BuildableDef def, DesignatorDropdownGroupDef group)
        {
            var key = DropdownRuntime.KeyOf(def);
            var settings = ArchitectStudioMod.Settings;

            if (group == DropdownRuntime.OriginalGroupOf(def))
            {
                settings.dropdownAssignments.Remove(key);
            }
            else
            {
                settings.dropdownAssignments[key] = group?.defName ?? "";
            }

            ArchitectStudioMod.Instance.WriteSettings();
            DropdownRuntime.Apply();
            InvalidateCaches();
        }

        private void ReorderMember(DesignatorDropdownGroupDef group, int from, int to)
        {
            var members = MembersOf(group).ToList();
            if (from < 0 || from >= members.Count)
            {
                return;
            }

            to = Mathf.Clamp(to, 0, members.Count - 1);
            if (from == to)
            {
                return;
            }

            var moved = members[from];
            members.RemoveAt(from);
            members.Insert(to, moved);

            DropdownOrderRuntime.SetOrder(group.defName, members);
            ArchitectStudioMod.Instance.WriteSettings();
            DropdownRuntime.RebuildCategoriesOf(group);
            InvalidateCaches();
        }

        private void ResetOrder(DesignatorDropdownGroupDef group)
        {
            DropdownOrderRuntime.ClearOrder(group.defName);
            ArchitectStudioMod.Instance.WriteSettings();
            DropdownRuntime.RebuildCategoriesOf(group);
            InvalidateCaches();
        }

        private void CreateGroup(string label)
        {
            var settings = ArchitectStudioMod.Settings;

            var index = 1;
            string id;
            do
            {
                id = "AS_Group_" + index++;
            }
            while (DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(id) != null ||
                   settings.customGroups.Any(e => e.id == id));

            var entry = new DropdownGroupEntry(id, label);
            settings.customGroups.Add(entry);
            ArchitectStudioMod.Instance.WriteSettings();

            DropdownRuntime.EnsureCustomGroupDefs();
            InvalidateCaches();
            selectedGroup = DefDatabase<DesignatorDropdownGroupDef>.GetNamedSilentFail(entry.id);
        }

        private void RenameGroup(DesignatorDropdownGroupDef group, string label)
        {
            var entry = CustomEntry(group);
            if (entry == null)
            {
                return;
            }

            entry.label = label;
            group.label = label;
            ArchitectStudioMod.Instance.WriteSettings();
            InvalidateCaches();
        }

        /// <summary>Change une option d'affichage du groupe et rafraichit les menus concernes.</summary>
        private void UpdateGroupOptions(DesignatorDropdownGroupDef group, Action<DropdownGroupEntry> mutate)
        {
            var entry = CustomEntry(group);
            if (entry == null)
            {
                return;
            }

            mutate(entry);
            group.useGridMenu = entry.useGridMenu;
            group.iconSource = entry.iconSource;

            ArchitectStudioMod.Instance.WriteSettings();
            InvalidateCaches();
        }

        /// <summary>
        /// Supprime un groupe cree par l'utilisateur. Ses batiments reprennent leur groupe d'origine :
        /// on retire leurs overrides plutot que de les laisser pointer dans le vide.
        /// </summary>
        private void DeleteGroup(DesignatorDropdownGroupDef group)
        {
            var settings = ArchitectStudioMod.Settings;

            foreach (var key in settings.dropdownAssignments
                         .Where(pair => pair.Value == group.defName)
                         .Select(pair => pair.Key)
                         .ToList())
            {
                settings.dropdownAssignments.Remove(key);
            }

            settings.customGroups.RemoveAll(e => e.id == group.defName);
            settings.hiddenGroupIds.Remove(group.defName);
            DropdownOrderRuntime.ClearOrder(group.defName);
            ArchitectStudioMod.Instance.WriteSettings();

            if (selectedGroup == group)
            {
                selectedGroup = null;
            }

            DropdownRuntime.Apply();
            InvalidateCaches();
        }

        /// <summary>
        /// Supprime un groupe qu'on ne possede pas. Le def appartient a un autre mod et reviendra au
        /// prochain demarrage : on le vide de ses membres, ce qui suffit a le faire disparaitre du menu
        /// Architecte, et on retient qu'il ne doit plus s'afficher ici.
        /// </summary>
        private void DissolveGroup(DesignatorDropdownGroupDef group)
        {
            var settings = ArchitectStudioMod.Settings;

            foreach (var def in MembersOf(group).ToList())
            {
                var key = DropdownRuntime.KeyOf(def);
                if (DropdownRuntime.OriginalGroupOf(def) == null)
                {
                    settings.dropdownAssignments.Remove(key);
                }
                else
                {
                    settings.dropdownAssignments[key] = "";
                }
            }

            if (!settings.hiddenGroupIds.Contains(group.defName))
            {
                settings.hiddenGroupIds.Add(group.defName);
            }

            DropdownOrderRuntime.ClearOrder(group.defName);
            ArchitectStudioMod.Instance.WriteSettings();

            if (selectedGroup == group)
            {
                selectedGroup = null;
            }

            DropdownRuntime.Apply();
            InvalidateCaches();
        }

        private void RestoreHiddenGroups()
        {
            ArchitectStudioMod.Settings.hiddenGroupIds.Clear();
            ArchitectStudioMod.Instance.WriteSettings();
            InvalidateCaches();
        }

        // ---------------------------------------------------------------- rendu

        public override void DoWindowContents(Rect inRect)
        {
            var anchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;

            var y = inRect.y;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, y, inRect.width, 32f), "ArchitectStudio.Dropdowns.Title".Translate());
            y += 34f;

            Text.Font = GameFont.Small;
            GUI.color = new Color(1f, 1f, 1f, 0.6f);
            Widgets.Label(new Rect(inRect.x, y, inRect.width, 24f), "ArchitectStudio.Dropdowns.Intro".Translate());
            GUI.color = Color.white;
            y += 30f;

            const float bottomBarHeight = 38f;
            var bodyRect = new Rect(inRect.x, y, inRect.width, inRect.height - y - bottomBarHeight);
            var columnWidth = (bodyRect.width - 2f * ColumnGap) / 3f;

            DrawGroupColumn(new Rect(bodyRect.x, bodyRect.y, columnWidth, bodyRect.height));
            DrawMemberColumn(new Rect(bodyRect.x + columnWidth + ColumnGap, bodyRect.y, columnWidth, bodyRect.height));
            DrawAddColumn(new Rect(bodyRect.x + 2f * (columnWidth + ColumnGap), bodyRect.y, columnWidth, bodyRect.height));

            DrawBottomBar(new Rect(inRect.x, bodyRect.yMax + 6f, inRect.width, bottomBarHeight - 6f));

            Text.Anchor = anchor;
        }

        private void DrawBottomBar(Rect rect)
        {
            var settings = ArchitectStudioMod.Settings;
            var overrideCount = settings.dropdownAssignments.Count;
            var hiddenCount = settings.hiddenGroupIds.Count;

            var label = "ArchitectStudio.Dropdowns.OverrideCount".Translate(overrideCount).ToString();
            if (hiddenCount > 0)
            {
                label += "  ·  " + "ArchitectStudio.Dropdowns.HiddenCount".Translate(hiddenCount);
            }

            var labelWidth = rect.width - 340f - (hiddenCount > 0 ? 246f : 0f);
            GUI.color = new Color(1f, 1f, 1f, 0.6f);
            Widgets.Label(new Rect(rect.x, rect.y, labelWidth, rect.height), label);
            GUI.color = Color.white;

            if (hiddenCount > 0 && Widgets.ButtonText(new Rect(rect.xMax - 576f, rect.y, 240f, ButtonHeight),
                    "ArchitectStudio.Dropdowns.RestoreHidden".Translate()))
            {
                RestoreHiddenGroups();
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 330f, rect.y, 200f, ButtonHeight),
                    "ArchitectStudio.Dropdowns.ResetAll".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    "ArchitectStudio.Dropdowns.ConfirmReset".Translate(),
                    delegate
                    {
                        DropdownRuntime.ResetAll();
                        selectedGroup = null;
                        InvalidateCaches();
                    },
                    destructive: true));
            }

            if (Widgets.ButtonText(new Rect(rect.xMax - 120f, rect.y, 120f, ButtonHeight), "CloseButton".Translate()))
            {
                Close();
            }
        }

        private void DrawGroupColumn(Rect rect)
        {
            Widgets.DrawMenuSection(rect);
            var inner = rect.ContractedBy(6f);
            var y = inner.y;

            DrawColumnHeader(new Rect(inner.x, y, inner.width, HeaderHeight),
                "ArchitectStudio.Dropdowns.Groups".Translate());
            y += HeaderHeight;

            groupSearch = Widgets.TextField(new Rect(inner.x, y, inner.width, SearchHeight), groupSearch);
            y += SearchHeight + 6f;

            var newButtonRect = new Rect(inner.x, inner.yMax - ButtonHeight, inner.width, ButtonHeight);
            if (Widgets.ButtonText(newButtonRect, "ArchitectStudio.Dropdowns.NewGroup".Translate()))
            {
                Find.WindowStack.Add(new Dialog_GroupName(null, CreateGroup));
            }

            var filtered = Groups
                .Where(g => groupSearch.NullOrEmpty() ||
                            GroupLabel(g).IndexOf(groupSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                .ToList();

            var listRect = new Rect(inner.x, y, inner.width, newButtonRect.y - y - 6f);
            var viewRect = new Rect(0f, 0f, listRect.width - 16f, filtered.Count * RowHeight);

            Widgets.BeginScrollView(listRect, ref groupScroll, viewRect);
            var rowY = 0f;
            foreach (var group in filtered)
            {
                var row = new Rect(0f, rowY, viewRect.width, RowHeight);
                rowY += RowHeight;

                if (selectedGroup == group)
                {
                    Widgets.DrawHighlightSelected(row);
                }
                else if (Mouse.IsOver(row))
                {
                    Widgets.DrawHighlight(row);
                }

                var count = MembersOf(group).Count;
                var custom = IsCustom(group);

                var countRect = new Rect(row.xMax - 40f, row.y, 36f, row.height);
                var labelRect = new Rect(row.x + 4f, row.y, row.width - 48f, row.height);

                if (custom)
                {
                    GUI.color = new Color(0.65f, 0.9f, 1f);
                }
                else if (count == 0)
                {
                    GUI.color = new Color(1f, 1f, 1f, 0.45f);
                }

                Widgets.Label(labelRect, GroupLabel(group).Truncate(labelRect.width));
                GUI.color = Color.white;

                var anchor = Text.Anchor;
                Text.Anchor = TextAnchor.MiddleRight;
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                Widgets.Label(countRect, count.ToString());
                GUI.color = Color.white;
                Text.Anchor = anchor;

                TooltipHandler.TipRegion(row,
                    group.defName + (custom ? "\n" + "ArchitectStudio.Dropdowns.Custom".Translate() : ""));

                if (Widgets.ButtonInvisible(row))
                {
                    selectedGroup = group;
                    addSearch = "";
                    addCategoryFilterSet = false;
                }
            }

            Widgets.EndScrollView();
        }

        private void DrawMemberColumn(Rect rect)
        {
            Widgets.DrawMenuSection(rect);
            var inner = rect.ContractedBy(6f);
            var y = inner.y;

            var header = selectedGroup == null
                ? "ArchitectStudio.Dropdowns.Members".Translate()
                : "ArchitectStudio.Dropdowns.MembersOf".Translate(GroupLabel(selectedGroup));
            DrawColumnHeader(new Rect(inner.x, y, inner.width, HeaderHeight), header);
            y += HeaderHeight + 4f;

            if (selectedGroup == null)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                Widgets.Label(new Rect(inner.x, y, inner.width, 60f),
                    "ArchitectStudio.Dropdowns.NoSelection".Translate());
                GUI.color = Color.white;
                return;
            }

            var group = selectedGroup;
            var custom = IsCustom(group);

            y = DrawGroupCategory(inner, y, group);

            if (custom)
            {
                y = DrawGroupOptions(inner, y, group);
            }

            y = DrawSplitWarning(inner, y, group);

            // Le pied de colonne s'empile depuis le bas : la liste prend ce qui reste.
            var footerY = inner.yMax;
            Rect? resetOrderRow = null;

            footerY -= ButtonHeight;
            var manageRow = new Rect(inner.x, footerY, inner.width, ButtonHeight);
            footerY -= 6f;

            if (DropdownOrderRuntime.HasOrder(group.defName))
            {
                footerY -= ButtonHeight;
                resetOrderRow = new Rect(inner.x, footerY, inner.width, ButtonHeight);
                footerY -= 6f;
            }

            var members = MembersOf(group);
            var listRect = new Rect(inner.x, y, inner.width, Mathf.Max(RowHeight, footerY - y));
            var viewRect = new Rect(0f, 0f, listRect.width - 16f, members.Count * RowHeight);

            Widgets.BeginScrollView(listRect, ref memberScroll, viewRect);

            // NewGroup et Reorderable convertissent en coordonnees ecran, donc le glisser-deposer
            // fonctionne tel quel a l'interieur de la zone defilante.
            if (Event.current.type == EventType.Repaint)
            {
                memberReorderGroup = ReorderableWidget.NewGroup(
                    // On relit selectedGroup au declenchement plutot que de capturer 'group' :
                    // l'action est invoquee bien apres le Repaint qui l'a enregistree.
                    (from, to) =>
                    {
                        if (selectedGroup != null)
                        {
                            ReorderMember(selectedGroup, from, to);
                        }
                    },
                    ReorderableDirection.Vertical,
                    viewRect);
            }

            var snapshot = members.ToList();
            for (var i = 0; i < snapshot.Count; i++)
            {
                var def = snapshot[i];
                var row = new Rect(0f, i * RowHeight, viewRect.width, RowHeight);

                ReorderableWidget.Reorderable(memberReorderGroup, row);

                if (Mouse.IsOver(row))
                {
                    Widgets.DrawHighlight(row);
                }

                // Trois boutons a droite : monter, descendre, retirer. Le glisser-deposer reste
                // disponible, mais il n'est pas le seul chemin - viser une cible de depot au
                // pointeur d'un Steam Deck est penible.
                var upRect = new Rect(row.xMax - 3f * RemoveButtonSize - 8f, row.y + 4f, RemoveButtonSize, RemoveButtonSize);
                var downRect = new Rect(row.xMax - 2f * RemoveButtonSize - 5f, row.y + 4f, RemoveButtonSize, RemoveButtonSize);
                var removeRect = new Rect(row.xMax - RemoveButtonSize - 2f, row.y + 4f, RemoveButtonSize, RemoveButtonSize);

                DrawBuildableLabel(new Rect(row.x, row.y, upRect.x - row.x - 4f, row.height),
                    def, showCategory: true);

                // Toute mutation invalide l'instantane : on arrete de dessiner, la frame suivante
                // repart d'une liste a jour.
                var index = i;
                if (ArchitectStudioWidgets.ArrowButton(upRect, up: true, index > 0))
                {
                    ReorderMember(group, index, index - 1);
                    break;
                }

                if (ArchitectStudioWidgets.ArrowButton(downRect, up: false, index < snapshot.Count - 1))
                {
                    ReorderMember(group, index, index + 1);
                    break;
                }

                TooltipHandler.TipRegion(removeRect, "ArchitectStudio.Dropdowns.Remove".Translate());
                if (Widgets.ButtonText(removeRect, "ArchitectStudio.Common.RemoveGlyph".Translate()))
                {
                    Assign(def, null);
                    break;
                }
            }

            Widgets.EndScrollView();

            if (resetOrderRow.HasValue &&
                Widgets.ButtonText(resetOrderRow.Value, "ArchitectStudio.Dropdowns.ResetOrder".Translate()))
            {
                ResetOrder(group);
            }

            var deleteRect = manageRow;
            if (custom)
            {
                var half = (manageRow.width - 6f) / 2f;
                deleteRect = new Rect(manageRow.x + half + 6f, manageRow.y, half, ButtonHeight);

                if (Widgets.ButtonText(new Rect(manageRow.x, manageRow.y, half, ButtonHeight),
                        "ArchitectStudio.Dropdowns.Rename".Translate()))
                {
                    Find.WindowStack.Add(new Dialog_GroupName(GroupLabel(group), label => RenameGroup(group, label)));
                }
            }

            if (Widgets.ButtonText(deleteRect, "ArchitectStudio.Dropdowns.Delete".Translate()))
            {
                Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(
                    DeleteConfirmationText(group, custom, members.Count),
                    custom ? (Action)(() => DeleteGroup(group)) : () => DissolveGroup(group),
                    destructive: true));
            }
        }

        private static string DeleteConfirmationText(DesignatorDropdownGroupDef group, bool custom, int memberCount)
        {
            if (custom)
            {
                return "ArchitectStudio.Dropdowns.ConfirmDelete".Translate(GroupLabel(group));
            }

            return memberCount == 0
                ? "ArchitectStudio.Dropdowns.ConfirmDissolveEmpty".Translate(GroupLabel(group))
                : "ArchitectStudio.Dropdowns.ConfirmDissolve".Translate(GroupLabel(group), memberCount);
        }

        /// <summary>
        /// Un groupe n'est rattache a aucune categorie : le jeu regroupe categorie par categorie. Des
        /// membres repartis sur plusieurs categories produisent donc autant de boutons separes, sans
        /// que rien ne le signale en jeu.
        /// </summary>
        private float DrawSplitWarning(Rect inner, float y, DesignatorDropdownGroupDef group)
        {
            var categories = MembersOf(group)
                .Select(d => d.designationCategory)
                .Where(c => c != null)
                .Distinct()
                .ToList();

            if (categories.Count < 2)
            {
                return y;
            }

            const float height = 46f;
            var rect = new Rect(inner.x, y, inner.width, height);

            Widgets.DrawBoxSolid(rect, new Color(0.6f, 0.4f, 0.1f, 0.25f));
            GUI.color = new Color(1f, 0.85f, 0.5f);

            var textRect = rect.ContractedBy(4f);
            var font = Text.Font;
            var anchor = Text.Anchor;
            Text.Font = GameFont.Tiny;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(textRect, "ArchitectStudio.Dropdowns.SplitWarning".Translate(categories.Count));
            Text.Font = font;
            Text.Anchor = anchor;

            GUI.color = Color.white;
            TooltipHandler.TipRegion(rect, "ArchitectStudio.Dropdowns.SplitWarningTip".Translate(
                categories.Select(c => c.LabelCap.ToString()).ToCommaList()));

            return y + height + 4f;
        }

        /// <summary>
        /// Categorie du groupe. Sans elle, un groupe n'a pas de place a lui : il apparait la ou sont
        /// ses membres, et se scinde en plusieurs boutons s'ils sont disperses. La choisir deplace
        /// tous les membres d'un coup, et les suivants suivront.
        /// </summary>
        private float DrawGroupCategory(Rect inner, float y, DesignatorDropdownGroupDef group)
        {
            var current = DropdownRuntime.TargetCategoryOf(group.defName);

            var labelRect = new Rect(inner.x, y, 80f, 26f);
            var buttonRect = new Rect(inner.x + 84f, y, inner.width - 84f, 26f);

            GUI.color = new Color(1f, 1f, 1f, 0.7f);
            Widgets.Label(labelRect, "ArchitectStudio.Dropdowns.GroupCategory".Translate());
            GUI.color = Color.white;

            var currentLabel = current == null
                ? "ArchitectStudio.Dropdowns.GroupCategoryFree".Translate().ToString()
                : CategoryLabel(current);

            TooltipHandler.TipRegion(buttonRect, "ArchitectStudio.Dropdowns.GroupCategoryTip".Translate());
            if (Widgets.ButtonText(buttonRect, currentLabel.Truncate(buttonRect.width - 16f)))
            {
                CategoryMenu.Show(
                    "ArchitectStudio.Dropdowns.GroupCategoryFree".Translate(),
                    category => SetGroupCategory(group, category));
            }

            return y + 30f;
        }

        private void SetGroupCategory(DesignatorDropdownGroupDef group, DesignationCategoryDef category)
        {
            var settings = ArchitectStudioMod.Settings;

            if (category == null)
            {
                settings.groupCategories.Remove(group.defName);
            }
            else
            {
                settings.groupCategories[group.defName] = category.defName;
            }

            ArchitectStudioMod.Instance.WriteSettings();
            DropdownRuntime.Apply();
            InvalidateCaches();
        }

        private static string CategoryLabel(DesignationCategoryDef category)
        {
            var parent = BetterArchitectCompat.ParentCategoryOf(category);
            return parent != null
                ? "ArchitectStudio.Common.CategoryPath".Translate(parent.LabelCap, category.LabelCap).ToString()
                : category.LabelCap.ToString();
        }

        /// <summary>Options d'affichage d'un groupe cree par l'utilisateur. Renvoie le nouveau y.</summary>
        private float DrawGroupOptions(Rect inner, float y, DesignatorDropdownGroupDef group)
        {
            var entry = CustomEntry(group);

            var gridMenu = entry.useGridMenu;
            Widgets.CheckboxLabeled(new Rect(inner.x, y, inner.width, 26f),
                "ArchitectStudio.Dropdowns.GridMenu".Translate(), ref gridMenu);
            if (gridMenu != entry.useGridMenu)
            {
                UpdateGroupOptions(group, e => e.useGridMenu = gridMenu);
            }

            y += 28f;

            var labelRect = new Rect(inner.x, y, 80f, 26f);
            var buttonRect = new Rect(inner.x + 84f, y, inner.width - 84f, 26f);

            GUI.color = new Color(1f, 1f, 1f, 0.7f);
            Widgets.Label(labelRect, "ArchitectStudio.Dropdowns.IconSource".Translate());
            GUI.color = Color.white;

            TooltipHandler.TipRegion(buttonRect, "ArchitectStudio.Dropdowns.IconSourceTip".Translate());
            if (Widgets.ButtonText(buttonRect, IconSourceLabel(entry.iconSource)))
            {
                var options = new List<FloatMenuOption>();
                foreach (DesignatorDropdownGroupDef.IconSource source in
                         Enum.GetValues(typeof(DesignatorDropdownGroupDef.IconSource)))
                {
                    var captured = source;
                    options.Add(new FloatMenuOption(IconSourceLabel(captured),
                        () => UpdateGroupOptions(group, e => e.iconSource = captured)));
                }

                Find.WindowStack.Add(new FloatMenu(options));
            }

            return y + 30f;
        }

        private static string IconSourceLabel(DesignatorDropdownGroupDef.IconSource source)
        {
            return source == DesignatorDropdownGroupDef.IconSource.Cost
                ? "ArchitectStudio.Dropdowns.IconSourceCost".Translate()
                : "ArchitectStudio.Dropdowns.IconSourcePlaced".Translate();
        }

        private void DrawAddColumn(Rect rect)
        {
            Widgets.DrawMenuSection(rect);
            var inner = rect.ContractedBy(6f);
            var y = inner.y;

            DrawColumnHeader(new Rect(inner.x, y, inner.width, HeaderHeight),
                "ArchitectStudio.Dropdowns.Add".Translate());
            y += HeaderHeight;

            if (selectedGroup == null)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                Widgets.Label(new Rect(inner.x, y + 6f, inner.width, 60f),
                    "ArchitectStudio.Dropdowns.NoSelection".Translate());
                GUI.color = Color.white;
                return;
            }

            addSearch = Widgets.TextField(new Rect(inner.x, y, inner.width, SearchHeight), addSearch);
            y += SearchHeight + 4f;

            var category = EffectiveAddCategory();
            y = DrawCategoryFilter(inner, y, category);

            var candidates = DropdownRuntime.AllBuildables()
                .Where(d => d.designatorDropdown != selectedGroup)
                .Where(d => category == null || d.designationCategory == category)
                .Where(d => addSearch.NullOrEmpty() ||
                            d.LabelCap.ToString().IndexOf(addSearch, StringComparison.OrdinalIgnoreCase) >= 0 ||
                            d.defName.IndexOf(addSearch, StringComparison.OrdinalIgnoreCase) >= 0)
                .OrderBy(d => d.LabelCap.ToString())
                .ToList();

            var total = candidates.Count;
            var shown = candidates.Take(MaxAddResults).ToList();

            var footerHeight = total > shown.Count ? 24f : 0f;
            var listRect = new Rect(inner.x, y, inner.width, inner.yMax - y - footerHeight);
            var viewRect = new Rect(0f, 0f, listRect.width - 16f, shown.Count * RowHeight);

            Widgets.BeginScrollView(listRect, ref addScroll, viewRect);
            var rowY = 0f;
            foreach (var def in shown)
            {
                var row = new Rect(0f, rowY, viewRect.width, RowHeight);
                rowY += RowHeight;

                if (Mouse.IsOver(row))
                {
                    Widgets.DrawHighlight(row);
                }

                DrawBuildableLabel(row, def, showCategory: category == null);

                if (Widgets.ButtonInvisible(row))
                {
                    Assign(def, selectedGroup);
                }
            }

            Widgets.EndScrollView();

            if (footerHeight > 0f)
            {
                GUI.color = new Color(1f, 1f, 1f, 0.5f);
                Widgets.Label(new Rect(inner.x, listRect.yMax, inner.width, footerHeight),
                    "ArchitectStudio.Dropdowns.MoreResults".Translate(total - shown.Count));
                GUI.color = Color.white;
            }
        }

        /// <summary>
        /// Categorie effectivement filtree : celle choisie par l'utilisateur, sinon celle du groupe.
        /// Un groupe encore vide n'impose rien, donc on n'affiche pas de filtre implicite.
        /// </summary>
        private DesignationCategoryDef EffectiveAddCategory()
        {
            if (addCategoryFilterSet)
            {
                return addCategoryFilter;
            }

            // Un groupe qui impose sa categorie deplacera ce qu'on y ajoute : filtrer sur elle
            // masquerait justement tout ce qu'on cherche a faire venir.
            if (selectedGroup != null && DropdownRuntime.TargetCategoryOf(selectedGroup.defName) != null)
            {
                return null;
            }

            return MembersOf(selectedGroup).FirstOrDefault()?.designationCategory;
        }

        /// <summary>
        /// Selecteur de categorie. C'est le chemin sans clavier : sur Steam Deck, taper une recherche
        /// impose d'ouvrir le clavier virtuel, alors qu'ici tout se fait au pointeur.
        /// </summary>
        private float DrawCategoryFilter(Rect inner, float y, DesignationCategoryDef current)
        {
            var labelRect = new Rect(inner.x, y, 80f, 26f);
            var buttonRect = new Rect(inner.x + 84f, y, inner.width - 84f, 26f);

            GUI.color = new Color(1f, 1f, 1f, 0.7f);
            Widgets.Label(labelRect, "ArchitectStudio.Dropdowns.Category".Translate());
            GUI.color = Color.white;

            var currentLabel = current == null
                ? "ArchitectStudio.Dropdowns.AllCategories".Translate().ToString()
                : CategoryLabel(current);

            if (Widgets.ButtonText(buttonRect, currentLabel.Truncate(buttonRect.width - 16f)))
            {
                CategoryMenu.Show(
                    "ArchitectStudio.Dropdowns.AllCategories".Translate(),
                    category =>
                    {
                        addCategoryFilter = category;
                        addCategoryFilterSet = true;
                    });
            }

            return y + 30f;
        }

        private static void DrawColumnHeader(Rect rect, string label)
        {
            GUI.color = new Color(1f, 1f, 1f, 0.8f);
            Widgets.Label(rect, label.Truncate(rect.width));
            GUI.color = Color.white;
            Widgets.DrawLineHorizontal(rect.x, rect.yMax - 2f, rect.width);
        }

        private static void DrawBuildableLabel(Rect rect, BuildableDef def, bool showCategory)
        {
            var iconRect = new Rect(rect.x + 2f, rect.y + 3f, 24f, 24f);
            Widgets.DefIcon(iconRect, def);

            var labelRect = new Rect(iconRect.xMax + 6f, rect.y, rect.width - iconRect.width - 12f, rect.height);
            var label = def.LabelCap.ToString();

            if (showCategory && def.designationCategory != null)
            {
                label += " <color=#FFFFFF80>(" + def.designationCategory.LabelCap + ")</color>";
            }

            Widgets.Label(labelRect, label.Truncate(labelRect.width));
            TooltipHandler.TipRegion(rect, def.defName);
        }
    }
}
