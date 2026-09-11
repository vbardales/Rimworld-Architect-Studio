using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Interception of Architect Icons' category icon. Its lookup is public and so is its cache, so
    /// there is no need to copy PNGs around: we answer ahead of it, and evict the cache entry when
    /// the choice changes.
    ///
    /// Soft dependency: without Architect Icons, picking an icon is simply unavailable.
    /// </summary>
    public static class ArchitectIconsCompat
    {
        private const string ResourcesTypeName = "ArchitectIcons.Resources";

        private static Type resourcesType;
        private static FieldInfo cacheField;
        private static bool resolved;

        private static List<string> iconPathsCache;

        public static bool Available
        {
            get
            {
                Resolve();
                return resourcesType != null;
            }
        }

        private static void Resolve()
        {
            if (resolved)
            {
                return;
            }

            resolved = true;
            resourcesType = AccessTools.TypeByName(ResourcesTypeName);
            if (resourcesType != null)
            {
                cacheField = AccessTools.Field(resourcesType, "iconsCache");
            }
        }

        /// <summary>Hooks the prefix up. Called from the mod constructor, assemblies already loaded.</summary>
        public static void ApplyPatch(Harmony harmony)
        {
            Resolve();
            if (resourcesType == null)
            {
                return;
            }

            var target = AccessTools.Method(resourcesType, "FindArchitectTabCategoryIcon", new[] { typeof(string) });
            if (target == null)
            {
                Log.Warning("[Architect Studio] Architect Icons is loaded but its icon lookup was not " +
                            "recognised: choosing a category icon will have no effect.");
                return;
            }

            harmony.Patch(target, prefix: new HarmonyMethod(typeof(ArchitectIconsCompat), nameof(FindIconPrefix)));
        }

        /// <summary>Answers in place of Architect Icons when an icon has been chosen.</summary>
        private static bool FindIconPrefix(string categoryDefName, ref Texture2D __result)
        {
            // This call comes immediately before Better Architect Menu draws a subcategory row:
            // it is our only chance to tint that label.
            CategoryColorPainter.Arm(categoryDefName);

            if (categoryDefName.NullOrEmpty() ||
                !ArchitectStudioMod.Settings.categoryIcons.TryGetValue(categoryDefName, out var path) ||
                path.NullOrEmpty())
            {
                return true;
            }

            var texture = ContentFinder<Texture2D>.Get(path, false);
            if (texture == null)
            {
                return true;
            }

            __result = texture;
            return false;
        }

        public static void InvalidateIcon(string categoryDefName)
        {
            Resolve();

            if (cacheField?.GetValue(null) is Dictionary<string, Texture2D> cache)
            {
                cache.Remove(categoryDefName);
            }
        }

        /// <summary>
        /// Paths of every available category icon. We read what RimWorld has already loaded for the
        /// active mods rather than walking the disk: no I/O, and never an icon offered that would
        /// not actually be loadable.
        /// </summary>
        public static List<string> AllIconPaths()
        {
            if (iconPathsCache != null)
            {
                return iconPathsCache;
            }

            var paths = new HashSet<string>();

            foreach (var mod in LoadedModManager.RunningModsListForReading)
            {
                var holder = mod.GetContentHolder<Texture2D>();
                if (holder?.contentList == null)
                {
                    continue;
                }

                foreach (var path in holder.contentList.Keys)
                {
                    if (path.IndexOf("ArchitectIcons", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        paths.Add(path);
                    }
                }
            }

            iconPathsCache = paths.OrderBy(p => p).ToList();
            return iconPathsCache;
        }

        public static Texture2D TextureFor(string path)
        {
            return path.NullOrEmpty() ? null : ContentFinder<Texture2D>.Get(path, false);
        }

        /// <summary>Icon currently shown for this category, chosen or default.</summary>
        public static Texture2D CurrentIconFor(DesignationCategoryDef category)
        {
            var chosen = TextureFor(CategoryAppearance.IconPathOf(category));
            if (chosen != null)
            {
                return chosen;
            }

            Resolve();
            if (resourcesType == null)
            {
                return null;
            }

            var find = AccessTools.Method(resourcesType, "FindArchitectTabCategoryIcon", new[] { typeof(string) });

            // Read for our own drawing: it must not arm the colour.
            CategoryColorPainter.Suppressed = true;
            try
            {
                return find?.Invoke(null, new object[] { category.defName }) as Texture2D;
            }
            finally
            {
                CategoryColorPainter.Suppressed = false;
                CategoryColorPainter.Disarm();
            }
        }
    }
}
