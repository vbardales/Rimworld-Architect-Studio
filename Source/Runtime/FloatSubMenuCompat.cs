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
    /// Sous-menus imbriques via le mod bibliotheque Float Sub-Menus de kathanon. Dependance souple :
    /// sans lui, on retombe sur une liste plate. Searchable Menus, du meme auteur, greffe de son cote
    /// un champ de recherche sur tout menu d'au moins quinze entrees, sans rien nous demander.
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
        /// Entree de menu ouvrant un sous-menu, ou null si Float Sub-Menus n'est pas la.
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
