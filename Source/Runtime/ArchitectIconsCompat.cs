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
    /// Interception de l'icone de categorie d'Architect Icons. Sa recherche est publique et son cache
    /// aussi, donc pas besoin de recopier des PNG : on repond avant lui, et on evince l'entree du
    /// cache quand le choix change.
    ///
    /// Dependance souple : sans Architect Icons, le choix d'icone est simplement indisponible.
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

        /// <summary>Branche le prefixe. Appele depuis le constructeur du mod, assemblies deja chargees.</summary>
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
                Log.Warning("[Architect Studio] Architect Icons est charge mais sa recherche d'icone n'a pas " +
                            "ete reconnue : le choix d'icone de categorie sera sans effet.");
                return;
            }

            harmony.Patch(target, prefix: new HarmonyMethod(typeof(ArchitectIconsCompat), nameof(FindIconPrefix)));
        }

        /// <summary>Repond a la place d'Architect Icons quand une icone a ete choisie.</summary>
        private static bool FindIconPrefix(string categoryDefName, ref Texture2D __result)
        {
            // Cet appel precede immediatement le dessin d'une ligne de sous-categorie chez Better
            // Architect Menu : c'est notre seule occasion d'en teinter le libelle.
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
        /// Chemins de toutes les icones de categorie disponibles. On lit ce que RimWorld a deja charge
        /// pour les mods actifs, plutot que de parcourir le disque : pas d'E/S, et jamais d'icone
        /// proposee qui ne serait pas reellement chargeable.
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

        /// <summary>Icone actuellement affichee pour cette categorie, choix ou defaut.</summary>
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

            // Lecture pour notre propre affichage : elle ne doit pas armer la couleur.
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
