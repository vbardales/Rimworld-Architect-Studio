using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace ArchitectStudio
{
    /// <summary>
    /// Libelle, icone et couleur des categories. Rien n'est ecrit sur disque : les libelles sont
    /// reappliques sur les defs au demarrage, l'icone est interceptee a la lecture, la couleur est
    /// posee au dessin.
    /// </summary>
    public static class CategoryAppearance
    {
        private static readonly Dictionary<string, string> originalLabels = new Dictionary<string, string>();
        private static bool labelsCaptured;

        /// <summary>Couleurs analysees, pour ne pas reparser une chaine a chaque frame.</summary>
        private static readonly Dictionary<string, Color?> colorCache = new Dictionary<string, Color?>();

        // ---------------------------------------------------------------- libelles

        private static void CaptureLabels()
        {
            if (labelsCaptured)
            {
                return;
            }

            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                originalLabels[category.defName] = category.label;
            }

            labelsCaptured = true;
        }

        public static string OriginalLabelOf(DesignationCategoryDef category)
        {
            CaptureLabels();
            return originalLabels.TryGetValue(category.defName, out var label) ? label : category.label;
        }

        public static void ApplyLabels()
        {
            CaptureLabels();

            var overrides = ArchitectStudioMod.Settings.categoryLabels;

            foreach (var category in DefDatabase<DesignationCategoryDef>.AllDefsListForReading)
            {
                var desired = overrides.TryGetValue(category.defName, out var label) && !label.NullOrEmpty()
                    ? label
                    : OriginalLabelOf(category);

                if (category.label != desired)
                {
                    category.label = desired;
                }
            }
        }

        public static void SetLabel(DesignationCategoryDef category, string label)
        {
            var overrides = ArchitectStudioMod.Settings.categoryLabels;
            var original = OriginalLabelOf(category);

            if (label.NullOrEmpty() || label == original)
            {
                overrides.Remove(category.defName);
            }
            else
            {
                overrides[category.defName] = label;
            }

            ApplyLabels();
            ArchitectStudioMod.Instance.WriteSettings();
            BetterArchitectCompat.InvalidateCaches();
        }

        // ---------------------------------------------------------------- icones

        public static string IconPathOf(DesignationCategoryDef category)
        {
            return ArchitectStudioMod.Settings.categoryIcons.TryGetValue(category.defName, out var path)
                ? path
                : null;
        }

        public static void SetIcon(DesignationCategoryDef category, string iconPath)
        {
            var overrides = ArchitectStudioMod.Settings.categoryIcons;

            if (iconPath.NullOrEmpty())
            {
                overrides.Remove(category.defName);
            }
            else
            {
                overrides[category.defName] = iconPath;
            }

            ArchitectStudioMod.Instance.WriteSettings();

            // Architect Icons memorise l'icone d'une categorie definitivement : sans eviction, le
            // changement n'apparaitrait qu'au prochain demarrage.
            ArchitectIconsCompat.InvalidateIcon(category.defName);
            BetterArchitectCompat.InvalidateCaches();
        }

        // ---------------------------------------------------------------- couleurs

        public static Color? ColorOf(DesignationCategoryDef category)
        {
            var defName = category.defName;

            if (colorCache.TryGetValue(defName, out var cached))
            {
                return cached;
            }

            Color? parsed = null;
            if (ArchitectStudioMod.Settings.categoryColors.TryGetValue(defName, out var raw))
            {
                parsed = Parse(raw);
            }

            colorCache[defName] = parsed;
            return parsed;
        }

        public static void SetColor(DesignationCategoryDef category, Color? color)
        {
            var overrides = ArchitectStudioMod.Settings.categoryColors;

            if (color == null)
            {
                overrides.Remove(category.defName);
            }
            else
            {
                var c = color.Value;
                overrides[category.defName] = string.Join(",",
                    Mathf.RoundToInt(c.r * 255f), Mathf.RoundToInt(c.g * 255f), Mathf.RoundToInt(c.b * 255f));
            }

            colorCache.Remove(category.defName);
            ArchitectStudioMod.Instance.WriteSettings();
            BetterArchitectCompat.InvalidateCaches();
        }

        private static Color? Parse(string raw)
        {
            if (raw.NullOrEmpty())
            {
                return null;
            }

            var parts = raw.Split(',');
            if (parts.Length != 3)
            {
                return null;
            }

            var values = new int[3];
            for (var i = 0; i < 3; i++)
            {
                if (!int.TryParse(parts[i].Trim(), out values[i]))
                {
                    return null;
                }

                values[i] = Mathf.Clamp(values[i], 0, 255);
            }

            return new Color(values[0] / 255f, values[1] / 255f, values[2] / 255f);
        }

        /// <summary>Palette proposee dans l'editeur. Choisir au pointeur, sans saisir de RGB.</summary>
        public static readonly Color[] Palette =
        {
            new Color(0.85f, 0.30f, 0.30f), new Color(0.90f, 0.50f, 0.25f),
            new Color(0.90f, 0.75f, 0.30f), new Color(0.60f, 0.80f, 0.35f),
            new Color(0.35f, 0.75f, 0.45f), new Color(0.35f, 0.75f, 0.70f),
            new Color(0.35f, 0.65f, 0.90f), new Color(0.40f, 0.45f, 0.85f),
            new Color(0.60f, 0.40f, 0.85f), new Color(0.85f, 0.45f, 0.75f),
            new Color(0.70f, 0.55f, 0.40f), new Color(0.55f, 0.55f, 0.55f)
        };

        public static void ResetAll()
        {
            var settings = ArchitectStudioMod.Settings;

            foreach (var defName in settings.categoryIcons.Keys.ToList())
            {
                ArchitectIconsCompat.InvalidateIcon(defName);
            }

            settings.categoryLabels.Clear();
            settings.categoryIcons.Clear();
            settings.categoryColors.Clear();
            colorCache.Clear();

            ApplyLabels();
            ArchitectStudioMod.Instance.WriteSettings();
            BetterArchitectCompat.InvalidateCaches();
        }

        public static bool HasOverrides
        {
            get
            {
                var settings = ArchitectStudioMod.Settings;
                return settings.categoryLabels.Count > 0 ||
                       settings.categoryIcons.Count > 0 ||
                       settings.categoryColors.Count > 0;
            }
        }
    }
}
