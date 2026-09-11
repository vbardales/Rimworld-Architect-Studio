using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Categories created by the user. The def only exists in memory and is rebuilt at every
    /// startup: nothing is written into the game's files nor into another mod's.
    /// </summary>
    public static class CustomCategoryRuntime
    {
        public static bool IsCustom(DesignationCategoryDef category)
        {
            return category != null &&
                   ArchitectStudioMod.Settings.customCategories.Any(e => e.id == category.defName);
        }

        /// <summary>Recreates the missing defs and realigns label and parent on the configuration.</summary>
        public static void EnsureDefs()
        {
            foreach (var entry in ArchitectStudioMod.Settings.customCategories)
            {
                var def = DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(entry.id);

                if (def == null)
                {
                    def = new DesignationCategoryDef
                    {
                        defName = entry.id,
                        modContentPack = ArchitectStudioMod.Instance?.Content,
                        // Without at least these two orders, a brand new category is a dead page.
                        specialDesignatorClasses = new List<Type>
                        {
                            typeof(Designator_Cancel),
                            typeof(Designator_Deconstruct)
                        }
                    };

                    DefDatabase<DesignationCategoryDef>.Add(def);
                    // Add() renames on collision: we realign on the defName it kept.
                    entry.id = def.defName;

                    def.ResolveReferences();
                    def.ResolveDesignators();
                }

                def.label = entry.label;
                EnsureKeyBindingCategory(def);
                ApplyParent(def, entry);
            }
        }

        /// <summary>
        /// The game only builds a category's <see cref="KeyBindingCategoryDef"/> when defs are
        /// generated, at startup. A category created during a game would have it null: we redo here
        /// what <c>KeyBindingDefGenerator</c> does.
        /// </summary>
        private static void EnsureKeyBindingCategory(DesignationCategoryDef category)
        {
            if (category.bindingCatDef != null)
            {
                return;
            }

            var defName = "Architect_" + category.defName;

            var binding = DefDatabase<KeyBindingCategoryDef>.GetNamedSilentFail(defName);
            if (binding == null)
            {
                binding = new KeyBindingCategoryDef
                {
                    defName = defName,
                    label = category.label + " tab",
                    description = "Key bindings for the \"" + category.LabelCap + "\" section of the Architect menu",
                    modContentPack = category.modContentPack
                };

                var universal = DefDatabase<KeyBindingCategoryDef>.AllDefsListForReading
                    .Where(d => d.isGameUniversal)
                    .ToList();

                binding.checkForConflicts.AddRange(universal);
                foreach (var other in universal)
                {
                    other.checkForConflicts.Add(binding);
                }

                DefDatabase<KeyBindingCategoryDef>.Add(binding);
            }

            category.bindingCatDef = binding;
        }

        private static void ApplyParent(DesignationCategoryDef category, CustomCategoryEntry entry)
        {
            var parent = entry.parentId.NullOrEmpty()
                ? null
                : DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(entry.parentId);

            if (parent == null)
            {
                BetterArchitectCompat.DetachParent(category);
                return;
            }

            BetterArchitectCompat.TryAttachParent(category, parent);
        }

        public static DesignationCategoryDef Create(string label, DesignationCategoryDef parent)
        {
            var settings = ArchitectStudioMod.Settings;

            var index = 1;
            string id;
            do
            {
                id = "AS_Cat_" + index++;
            }
            while (DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(id) != null ||
                   settings.customCategories.Any(e => e.id == id));

            var entry = new CustomCategoryEntry(id, label, parent?.defName ?? "");
            settings.customCategories.Add(entry);

            EnsureDefs();
            ArchitectStudioMod.Instance.WriteSettings();
            Refresh();

            return DefDatabase<DesignationCategoryDef>.GetNamedSilentFail(entry.id);
        }

        public static void SetParent(DesignationCategoryDef category, DesignationCategoryDef parent)
        {
            var entry = ArchitectStudioMod.Settings.customCategories.FirstOrDefault(e => e.id == category.defName);
            if (entry == null)
            {
                return;
            }

            entry.parentId = parent?.defName ?? "";
            ApplyParent(category, entry);
            ArchitectStudioMod.Instance.WriteSettings();
            Refresh();
        }

        /// <summary>
        /// Deletes a created category. We first erase everything that names it, then reapply the
        /// groups - which sends its buildings back to their original category - and only then
        /// remove the def, so as never to leave a dangling reference.
        /// </summary>
        public static void Delete(DesignationCategoryDef category)
        {
            var settings = ArchitectStudioMod.Settings;

            settings.customCategories.RemoveAll(e => e.id == category.defName);

            foreach (var child in settings.customCategories.Where(e => e.parentId == category.defName))
            {
                child.parentId = "";
            }

            foreach (var key in settings.groupCategories
                         .Where(pair => pair.Value == category.defName)
                         .Select(pair => pair.Key)
                         .ToList())
            {
                settings.groupCategories.Remove(key);
            }

            settings.categoryOrders.Remove(category.defName);
            settings.categoryLabels.Remove(category.defName);
            settings.categoryIcons.Remove(category.defName);
            settings.categoryColors.Remove(category.defName);

            ArchitectStudioMod.Instance.WriteSettings();

            DropdownRuntime.Apply();
            EnsureDefs();

            try
            {
                DefDatabase<DesignationCategoryDef>.Remove(category);
            }
            catch (Exception ex)
            {
                Log.Warning($"[Architect Studio] Category '{category.defName}' could not be removed from " +
                            $"the def database ({ex.Message}): it will disappear on the next restart.");
            }

            Refresh();
        }

        private static void Refresh()
        {
            CategoryRuntime.InvalidateCounts();
            BetterArchitectCompat.InvalidateEditModeCaches();
            BetterArchitectCompat.InvalidateCaches();

            if (Current.ProgramState == ProgramState.Playing &&
                MainButtonDefOf.Architect.TabWindow is MainTabWindow_Architect architectWindow)
            {
                architectWindow.CacheDesPanels();
            }
        }
    }
}
