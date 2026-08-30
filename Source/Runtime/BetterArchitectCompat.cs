using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Better Architect Menu ne se contente pas de patcher le menu Architecte : il le redessine et
    /// met ses designators en cache. Nos reconstructions de categorie sont donc invisibles tant que
    /// ses caches ne sont pas purges - la modification serait bien appliquee au modele, mais
    /// n'apparaitrait qu'apres un redemarrage.
    ///
    /// Dependance souple : on resout tout par reflexion, le mod fonctionne sans BAM.
    /// </summary>
    public static class BetterArchitectCompat
    {
        private const string PatchTypeName = "BetterArchitect.ArchitectCategoryTab_DesignationTabOnGUI_Patch";

        /// <summary>
        /// <c>InvalidateResearchSensitiveCaches</c> est exactement ce qu'il nous faut : BAM l'appelle
        /// quand une recherche debloque des batiments, c'est-a-dire quand le contenu des categories
        /// change. Elle vide le cache de designators, celui de tri, les correspondances de recherche
        /// et les overrides de selection - sans remettre a zero le defilement ni la sous-categorie
        /// ouverte, ce que ferait son <c>Reset()</c> public.
        ///
        /// Les deux suivantes sont un repli si elle disparait : elles ne couvrent que le cache de
        /// designators et celui de tri, ce qui suffit a un simple regroupement mais pas a un
        /// changement de categorie.
        /// </summary>
        private const string PreferredMethod = "InvalidateResearchSensitiveCaches";
        private static readonly string[] FallbackMethods = { "InvalidateDesignatorDataCache", "ClearSortCache" };

        private static bool resolved;
        private static readonly List<MethodInfo> invalidators = new List<MethodInfo>();

        /// <summary>
        /// Vrai si Better Architect Menu est charge. Declenche la resolution : lu depuis les reglages,
        /// il repondrait faux tant qu'aucune purge de cache n'aurait encore eu lieu.
        /// </summary>
        public static bool Active
        {
            get
            {
                Resolve();
                return active;
            }
        }

        private static bool active;

        private static void Resolve()
        {
            if (resolved)
            {
                return;
            }

            resolved = true;

            var type = AccessTools.TypeByName(PatchTypeName);
            if (type == null)
            {
                return;
            }

            active = true;

            var preferred = Resolve(type, PreferredMethod);
            if (preferred != null)
            {
                invalidators.Add(preferred);
            }
            else
            {
                // Les deux replis se completent : il faut les deux, pas l'une ou l'autre.
                foreach (var name in FallbackMethods)
                {
                    var fallback = Resolve(type, name);
                    if (fallback != null)
                    {
                        invalidators.Add(fallback);
                    }
                }
            }

            if (invalidators.Count == 0)
            {
                var reset = Resolve(type, "Reset");
                if (reset != null)
                {
                    invalidators.Add(reset);
                }
            }

            if (invalidators.Count == 0)
            {
                Log.Warning("[Architect Studio] Better Architect Menu is loaded, but none of its cache " +
                            "invalidation entry points were recognised. Dropdown group changes may only " +
                            "show up after a restart.");
            }
        }

        private const string NestedExtensionTypeName = "BetterArchitect.NestedCategoryExtension";

        private static bool nestedResolved;
        private static Type nestedExtensionType;
        private static FieldInfo parentCategoryField;

        /// <summary>
        /// Categorie parente d'une sous-categorie de Better Architect Menu, ou null. Ses sous-categories
        /// sont de vraies DesignationCategoryDef portant une extension : sans lire le parent, deux
        /// sous-categories homonymes seraient impossibles a distinguer dans une liste.
        /// </summary>
        public static DesignationCategoryDef ParentCategoryOf(DesignationCategoryDef category)
        {
            if (category?.modExtensions == null)
            {
                return null;
            }

            if (!ResolveNested())
            {
                return null;
            }

            foreach (var extension in category.modExtensions)
            {
                if (extension != null && nestedExtensionType.IsInstanceOfType(extension))
                {
                    return parentCategoryField.GetValue(extension) as DesignationCategoryDef;
                }
            }

            return null;
        }

        /// <summary>
        /// Vrai si les sous-categories sont possibles. Vanilla n'a aucun mecanisme d'imbrication :
        /// sans Better Architect Menu, toute categorie est forcement de premier niveau, et l'interface
        /// doit le dire plutot que de creer une categorie racine en silence.
        /// </summary>
        public static bool SubcategoriesSupported => ResolveNested();

        /// <summary>
        /// Greffe l'extension d'imbrication de BAM, ce qui fait d'une categorie la sous-categorie
        /// d'une autre. Renvoie faux si BAM n'est pas la - il n'y a alors pas de sous-categories.
        /// </summary>
        public static bool TryAttachParent(DesignationCategoryDef category, DesignationCategoryDef parent)
        {
            if (!ResolveNested())
            {
                return false;
            }

            DetachParent(category);

            if (parent == null)
            {
                return true;
            }

            if (!(Activator.CreateInstance(nestedExtensionType) is DefModExtension extension))
            {
                return false;
            }

            parentCategoryField.SetValue(extension, parent);

            if (category.modExtensions == null)
            {
                category.modExtensions = new List<DefModExtension>();
            }

            category.modExtensions.Add(extension);
            return true;
        }

        public static void DetachParent(DesignationCategoryDef category)
        {
            if (!ResolveNested() || category.modExtensions == null)
            {
                return;
            }

            category.modExtensions.RemoveAll(e => e != null && nestedExtensionType.IsInstanceOfType(e));
        }

        /// <summary>
        /// Purge l'arborescence parent/enfant de BAM. Necessaire quand une categorie apparait ou
        /// disparait : ses caches de hierarchie sont construits une fois et ne se revalident pas seuls.
        /// </summary>
        public static void InvalidateEditModeCaches()
        {
            var type = AccessTools.TypeByName("BetterArchitect.EditModeRuntime");
            var method = type == null ? null : Resolve(type, "InvalidateAllCaches");

            try
            {
                method?.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Log.Warning("[Architect Studio] Could not invalidate Better Architect Menu's category " +
                            $"tree: {ex.Message}");
            }
        }

        private static bool ResolveNested()
        {
            if (!nestedResolved)
            {
                nestedResolved = true;
                nestedExtensionType = AccessTools.TypeByName(NestedExtensionTypeName);
                if (nestedExtensionType != null)
                {
                    parentCategoryField = AccessTools.Field(nestedExtensionType, "parentCategory");
                }
            }

            return nestedExtensionType != null && parentCategoryField != null;
        }

        private static MethodInfo Resolve(Type type, string name)
        {
            var method = AccessTools.Method(type, name);
            return method != null && method.GetParameters().Length == 0 ? method : null;
        }

        public static void InvalidateCaches()
        {
            Resolve();

            foreach (var method in invalidators)
            {
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception ex)
                {
                    Log.Warning($"[Architect Studio] Better Architect Menu's '{method.Name}' cache " +
                                $"invalidation failed: {ex.Message}");
                }
            }
        }
    }
}
