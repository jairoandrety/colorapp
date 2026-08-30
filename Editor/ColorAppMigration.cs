using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    /// <summary>
    /// Convierte los assets del formato anterior a 2.0 (ColorPaletteSetup +
    /// ColorAppData) en un unico ColorPaletteLibrary.
    ///
    /// Los assets originales NO se tocan ni se borran: la migracion solo lee.
    /// Asi, si el resultado no convence, basta con borrar la libreria generada
    /// y volver a lanzarla.
    /// </summary>
    public static class ColorAppMigration
    {
        private const string DefaultLibraryPath = "Assets/ColorPaletteLibrary.asset";

        [MenuItem("Window/ColorApp/Migrar assets a 2.0", false, 100)]
        public static void MigrateWithDialog()
        {
            List<ColorPaletteSetup> setups = FindLegacySetups();

            if (setups.Count == 0)
            {
                EditorUtility.DisplayDialog(
                    "ColorApp",
                    "No se encontro ningun asset del formato antiguo (ColorPaletteSetup) en el proyecto. " +
                    "Si empiezas de cero no necesitas migrar: crea una libreria desde " +
                    "Assets > Create > ColorApp > Color Palette Library.",
                    "Entendido");
                return;
            }

            int totalPalettes = 0;
            foreach (ColorPaletteSetup setup in setups)
                totalPalettes += setup.palettes != null ? setup.palettes.Count : 0;

            bool proceed = EditorUtility.DisplayDialog(
                "Migrar ColorApp a 2.0",
                $"Se han encontrado {setups.Count} asset(s) antiguos con {totalPalettes} paleta(s) en total. " +
                "Se creara una nueva ColorPaletteLibrary. Los assets originales no se modifican ni se borran. " +
                "El color de cada entrada se copia a las dos variantes (claro y oscuro).",
                "Migrar", "Cancelar");

            if (!proceed)
                return;

            ColorPaletteLibrary library = Migrate(setups);
            if (library == null)
                return;

            Selection.activeObject = library;
            EditorGUIUtility.PingObject(library);
        }

        /// <summary>Busca todos los ColorPaletteSetup del proyecto.</summary>
        public static List<ColorPaletteSetup> FindLegacySetups()
        {
            List<ColorPaletteSetup> result = new List<ColorPaletteSetup>();
            string[] guids = AssetDatabase.FindAssets("t:ColorPaletteSetup");

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                ColorPaletteSetup setup = AssetDatabase.LoadAssetAtPath<ColorPaletteSetup>(path);
                if (setup != null)
                    result.Add(setup);
            }

            return result;
        }

        /// <summary>
        /// Convierte los setups dados en una libreria nueva y la guarda en disco.
        /// El orden de las paletas se conserva, de modo que los indices que ya
        /// tengan guardados los ColorizerHandler de tus escenas siguen valiendo.
        /// </summary>
        public static ColorPaletteLibrary Migrate(List<ColorPaletteSetup> setups)
        {
            if (setups == null || setups.Count == 0)
                return null;

            ColorPaletteLibrary library = ScriptableObject.CreateInstance<ColorPaletteLibrary>();

            foreach (ColorPaletteSetup setup in setups)
            {
                if (setup.palettes == null)
                    continue;

                foreach (Palette legacyPalette in setup.palettes)
                {
                    if (legacyPalette == null)
                        continue;

                    ColorPalette palette = new ColorPalette
                    {
                        displayName = string.IsNullOrEmpty(legacyPalette.paletteName)
                            ? "Palette"
                            : legacyPalette.paletteName
                    };

                    if (legacyPalette.colors != null)
                    {
                        foreach (ColorPallete legacyColor in legacyPalette.colors)
                        {
                            if (legacyColor == null)
                                continue;

                            // El formato viejo solo tenia un color: se copia a
                            // las dos variantes para no inventar valores.
                            palette.slots.Add(new ColorSlot(
                                string.IsNullOrEmpty(legacyColor.label) ? "color" : legacyColor.label,
                                legacyColor.color,
                                legacyColor.color));
                        }
                    }

                    library.Palettes.Add(palette);
                }
            }

            string path = AssetDatabase.GenerateUniqueAssetPath(DefaultLibraryPath);
            AssetDatabase.CreateAsset(library, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ColorAppUtils.InvalidateLibraryCache();

            Debug.Log($"[ColorApp] Migracion completada: {library.PaletteCount} paleta(s) en '{path}'. " +
                      "Los assets antiguos siguen intactos.");

            return library;
        }

        /// <summary>
        /// Comprueba si dos paletas tienen exactamente las mismas keys, en el
        /// mismo orden. Es el caso tipico de tener una paleta "Normal" y otra
        /// "Dark" con los mismos labels.
        /// </summary>
        public static bool CanPairAsVariants(ColorPalette a, ColorPalette b)
        {
            if (a == null || b == null || a.SlotCount == 0 || a.SlotCount != b.SlotCount)
                return false;

            for (int i = 0; i < a.SlotCount; i++)
            {
                if (a.slots[i].key != b.slots[i].key)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Funde dos paletas con las mismas keys en una sola de dos variantes:
        /// los colores de primary pasan a la variante clara y los de secondary
        /// a la oscura.
        /// </summary>
        public static ColorPalette PairAsVariants(ColorPalette primary, ColorPalette secondary, string displayName)
        {
            if (!CanPairAsVariants(primary, secondary))
                return null;

            ColorPalette merged = new ColorPalette
            {
                displayName = string.IsNullOrEmpty(displayName) ? primary.displayName : displayName,
                primaryVariantName = primary.displayName,
                secondaryVariantName = secondary.displayName
            };

            for (int i = 0; i < primary.SlotCount; i++)
            {
                merged.slots.Add(new ColorSlot(
                    primary.slots[i].key,
                    primary.slots[i].Get(ColorVariant.Primary),
                    secondary.slots[i].Get(ColorVariant.Primary)));
            }

            return merged;
        }
    }
}
