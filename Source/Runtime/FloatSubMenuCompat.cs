using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Nested submenus through kathanon's Float Sub-Menus library mod. Soft dependency: without it,
    /// we fall back to a flat list. Searchable Menus, by the same author, separately grafts a search
    /// field onto any menu of at least fifteen entries, without asking us anything.
    /// </summary>
    public static class FloatSubMenuCompat
    {
        private const string SubMenuTypeName = "FloatSubMenus.FloatSubMenu";

        private static bool resolved;
        private static ConstructorInfo constructor;

        public static bool Available
        {
            get
            {
                Resolve();
                return constructor != null;
            }
        }

        private static void Resolve()
        {
            if (resolved)
            {
                return;
            }

            resolved = true;

            var type = AccessTools.TypeByName(SubMenuTypeName);
            if (type == null)
            {
                return;
            }

            constructor = AccessTools.Constructor(type, new[]
            {
                typeof(string),
                typeof(List<FloatMenuOption>),
                typeof(MenuOptionPriority),
                typeof(Thing),
                typeof(float),
                typeof(Func<Rect, bool>),
                typeof(WorldObject),
                typeof(bool),
                typeof(int)
            });

            if (constructor == null)
            {
                Log.Warning("[Architect Studio] Float Sub-Menus is loaded but its constructor was not " +
                            "recognised: categories will be listed flat.");
            }
        }

        /// <summary>
        /// Menu entry opening a submenu, or null if Float Sub-Menus is not there.
        /// </summary>
        public static FloatMenuOption TryCreateSubMenu(string label, List<FloatMenuOption> subOptions)
        {
            Resolve();

            if (constructor == null || subOptions.NullOrEmpty())
            {
                return null;
            }

            try
            {
                return (FloatMenuOption)constructor.Invoke(new object[]
                {
                    label, subOptions, MenuOptionPriority.Default, null, 0f, null, null, true, 0
                });
            }
            catch (Exception ex)
            {
                Log.Warning($"[Architect Studio] Could not create a sub-menu: {ex.Message}");
                return null;
            }
        }
    }
}
