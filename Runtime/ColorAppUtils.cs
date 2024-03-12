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
        private static string _palettePath = "PaletteTargetSetup";
        public static string PalettePath => _palettePath;

        public static bool ValidatePaletteSetup()
        {
            return Resources.Load<ColorPaletteSetup>(PalettePath) != null;
        }

        public static ColorPaletteSetup GetColorPaletteSetup()
        {
            ColorPaletteSetup paleteLoaded = Resources.Load<ColorPaletteSetup>(PalettePath);
#if UNITY_EDITOR
            if (paleteLoaded == null)
            {
                CreatePaletteSetup();
            }
#endif
            return paleteLoaded;
        }

#if UNITY_EDITOR
        public static void CreatePaletteSetup()
        {
            VerifyResourcesFolder();
            ColorPaletteSetup newPalette = ScriptableObject.CreateInstance<ColorPaletteSetup>();
            AssetDatabase.CreateAsset(newPalette, string.Format("Assets/Resources/{0}.asset", PalettePath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        private static void VerifyResourcesFolder()
        {
            bool folderExist = Directory.Exists("Assets/Resources");
            if (!folderExist)
            {
                Directory.CreateDirectory("Assets/Resources");
            }
        }

        //public static void SaveColorPaletteSetup(ColorPaletteSetup colorPaletteSetup)
        //{
        //    if (colorPaletteSetup != null)
        //    {
        //        ColorPaletteSetup paleteLoaded = Resources.Load<ColorPaletteSetup>("PaletteTargetSetup");
        //        if (paleteLoaded != null)
        //        {
        //            paleteLoaded.palettes = colorPaletteSetup.palettes;
        //            paleteLoaded.colorLabels = colorPaletteSetup.colorLabels;
        //        }
        //        else
        //        {
        //            paleteLoaded = ScriptableObject.CreateInstance<ColorPaletteSetup>();
        //            paleteLoaded.palettes = colorPaletteSetup.palettes;
        //            paleteLoaded.colorLabels = colorPaletteSetup.colorLabels;
        //        }

        //        AssetDatabase.CreateAsset(paleteLoaded, "Assets/Resources/PaletteTargetSetup.asset");
        //        AssetDatabase.SaveAssets();
        //        AssetDatabase.Refresh();
        //    }
        //}

        public static ColorAppData GetColorAppData()
        {
            string path = "Runtime/Data/ColorAppData.asset";
            ColorAppData ColorAppDataLoaded = GetColorAppDataFromPath(path);            

            if (ColorAppDataLoaded != null)
            {
                return ColorAppDataLoaded;
            }

            return null;
        }

        public static ColorAppData GetColorAppDataFromPath(string path)
        {
            string pathInAssetFolder = "Assets/ColorApp/";
            string pathInPackages = "Packages/com.jairoandrety.colorapp/";

            ColorAppData objInPackeages = AssetDatabase.LoadAssetAtPath<ColorAppData>(pathInPackages + path);
            ColorAppData objInAssetFolder = AssetDatabase.LoadAssetAtPath<ColorAppData>(pathInAssetFolder + path);

            return objInPackeages != null ? objInPackeages : objInAssetFolder;
        }

        public static List<string> ColorLabels()
        {
            List<string> labels = new List<string>();
            if (GetColorAppData().ColorLabels != null)
            {
                if (GetColorAppData().ColorLabels.Count > 0)
                {
                    foreach (var item in GetColorAppData().ColorLabels)
                    {
                        labels.Add(item);
                    }
                }
            }

            return labels;
        }

        public static List<string> PaletteNames()
        {
            List<string> names = new List<string>();
            ColorPaletteSetup colorPaletteSetup = GetColorPaletteSetup();
            if (colorPaletteSetup != null)
            {
                foreach (var item in colorPaletteSetup.palettes)
                {
                    names.Add(item.paletteName);
                }
            }
            return names;
        }
#endif
    }
}