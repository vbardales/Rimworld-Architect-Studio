using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Better Architect Menu does not merely patch the Architect menu: it redraws it and caches its
    /// designators. Our category rebuilds are therefore invisible until its caches are purged - the
    /// change would be applied to the model all right, but would only show after a restart.
    ///
    /// Soft dependency: everything is resolved by reflection, the mod works without BAM.
    /// </summary>
    public static class BetterArchitectCompat
    {
        private const string PatchTypeName = "BetterArchitect.ArchitectCategoryTab_DesignationTabOnGUI_Patch";

        /// <summary>
        /// <c>InvalidateResearchSensitiveCaches</c> is exactly what we need: BAM calls it when
        /// research unlocks buildings, that is, when the contents of the categories change. It
        /// clears the designator cache, the sort cache, the search matches and the selection
        /// overrides - without resetting the scroll position or the open subcategory, which its
        /// public <c>Reset()</c> would do.
        ///
        /// The two below are a fallback should it disappear: they only cover the designator cache
        /// and the sort cache, which is enough for a plain regrouping but not for a change of
        /// category.
        /// </summary>
        private const string PreferredMethod = "InvalidateResearchSensitiveCaches";
        private static readonly string[] FallbackMethods = { "InvalidateDesignatorDataCache", "ClearSortCache" };

        private static bool resolved;
        private static readonly List<MethodInfo> invalidators = new List<MethodInfo>();

        /// <summary>
        /// True if Better Architect Menu is loaded. Triggers resolution: read from the settings, it
        /// would answer false as long as no cache purge had happened yet.
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
                // The two fallbacks complement each other: both are needed, not one or the other.
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
        /// Parent category of a Better Architect Menu subcategory, or null. Its subcategories are
        /// real DesignationCategoryDefs carrying an extension: without reading the parent, two
        /// subcategories sharing a label would be impossible to tell apart in a list.
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
        /// True if subcategories are possible. Vanilla has no nesting mechanism whatsoever: without
        /// Better Architect Menu every category is necessarily top-level, and the interface has to
        /// say so rather than silently create a root category.
        /// </summary>
        public static bool SubcategoriesSupported => ResolveNested();

        /// <summary>
        /// Grafts BAM's nesting extension on, which makes one category the subcategory of another.
        /// Returns false if BAM is not there - there are no subcategories at all then.
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
        /// Purges BAM's parent/child tree. Needed whenever a category appears or disappears: its
        /// hierarchy caches are built once and do not revalidate themselves.
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
