using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public static class ColorAppUtils
    {
        private static ColorPaletteLibrary _cachedLibrary;

        /// <summary>
        /// Referencia usada en runtime. La asigna el ColorizerHandler desde su
        /// campo serializado, de modo que una build no necesita carpeta Resources.
        /// </summary>
        public static void SetLibrary(ColorPaletteLibrary library)
        {
            if (library != null)
                _cachedLibrary = library;
        }

        /// <summary>
        /// Devuelve la libreria de paletas del proyecto, o null si todavia no
        /// existe ninguna. Nunca crea el asset: la creacion es siempre explicita
        /// (migracion o boton del editor), porque crear assets durante un OnGUI
        /// dispara un reimport en mitad del repintado.
        /// </summary>
        public static ColorPaletteLibrary GetLibrary()
        {
            if (_cachedLibrary != null)
                return _cachedLibrary;

#if UNITY_EDITOR
            string[] guids = AssetDatabase.FindAssets("t:ColorPaletteLibrary");
            if (guids.Length > 0)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                _cachedLibrary = AssetDatabase.LoadAssetAtPath<ColorPaletteLibrary>(path);

                if (guids.Length > 1)
                {
                    Debug.LogWarning($"[ColorApp] Se encontraron {guids.Length} assets ColorPaletteLibrary en el proyecto. " +
                                     $"Se usara '{path}'. Conviene dejar solo uno.");
                }
            }
#else
            _cachedLibrary = Resources.Load<ColorPaletteLibrary>("ColorPaletteLibrary");
#endif
            return _cachedLibrary;
        }

        /// <summary>Fuerza a que la proxima llamada a GetLibrary vuelva a buscar.</summary>
        public static void InvalidateLibraryCache()
        {
            _cachedLibrary = null;
        }

        public static bool HasLibrary()
        {
            ColorPaletteLibrary library = GetLibrary();
            return library != null && library.HasPalettes;
        }

        public static List<string> PaletteNames()
        {
            ColorPaletteLibrary library = GetLibrary();
            return library != null ? library.PaletteNames() : new List<string>();
        }

        /// <summary>
        /// Keys de la paleta indicada. Sustituye a la antigua ColorLabels(), que
        /// devolvia una lista global compartida por todas las paletas.
        /// </summary>
        public static List<string> SlotKeys(int paletteIndex)
        {
            ColorPaletteLibrary library = GetLibrary();
            if (library == null)
                return new List<string>();

            ColorPalette palette = library.GetPalette(paletteIndex);
            return palette != null ? palette.SlotKeys() : new List<string>();
        }

        /// <summary>Keys de la paleta marcada como activa en la libreria.</summary>
        public static List<string> DefaultPaletteSlotKeys()
        {
            ColorPaletteLibrary library = GetLibrary();
            return library != null ? SlotKeys(library.DefaultPaletteIndex) : new List<string>();
        }

        public static string GetPackagePath()
        {
            string pathInAssetFolder = "Assets/ColorApp/";
            string pathInPackages = "Packages/com.jairoandrety.colorapp/";

            if (Directory.Exists(pathInAssetFolder))
                return pathInAssetFolder;
            else if (Directory.Exists(pathInPackages))
                return pathInPackages;
            else
                return null;
        }
    }
}
