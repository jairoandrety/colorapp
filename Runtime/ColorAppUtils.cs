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
        private static string _resourcePath = "Assets/Resources";
        private static string _colorAppDataPath = "ColorAppData";
        private static string _defaultPalettePath = "PaletteTargetSetup";
        
        //public static bool ValidatePaletteSetup()
        //{
        //    return Resources.Load<ColorPaletteSetup>(_defaultPalettePath) != null;
        //}

        public static ColorPaletteSetup GetColorPaletteSetup()
        {
            VerifyResourcesFolder();
            ColorPaletteSetup paletteLoaded = Resources.Load<ColorPaletteSetup>(_defaultPalettePath);
#if UNITY_EDITOR
            if (paletteLoaded == null)
            {
                paletteLoaded = CreatePaletteSetup();
            }
#endif
            return paletteLoaded;
        }
        
        public static ColorPaletteSetup CreatePaletteSetup()
        {
            VerifyResourcesFolder();
            ColorPaletteSetup newPalette = ScriptableObject.CreateInstance<ColorPaletteSetup>();
            AssetDatabase.CreateAsset(newPalette, string.Format("{0}/{1}.asset", _resourcePath, _defaultPalettePath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newPalette; 
        }

        public static void VerifyResourcesFolder()
        {
            bool folderResourcesExist = Directory.Exists(_resourcePath);
            if (!folderResourcesExist)
            {
                Directory.CreateDirectory(_resourcePath);
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
            VerifyResourcesFolder();
            ColorAppData colorAppDataLoaded = Resources.Load<ColorAppData>(_colorAppDataPath);
#if UNITY_EDITOR
            if (colorAppDataLoaded == null)
            {
                colorAppDataLoaded = CreateColorAppData();
            }
#endif
            return colorAppDataLoaded;
        }
        
#if UNITY_EDITOR
        public static ColorAppData CreateColorAppData()
        {
            VerifyResourcesFolder();
            ColorAppData newColorAppData = ScriptableObject.CreateInstance<ColorAppData>();
            AssetDatabase.CreateAsset(newColorAppData, string.Format("{0}/{1}.asset", _resourcePath, _colorAppDataPath));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return newColorAppData;
        }
#endif
        
        //public static ColorAppData GetColorAppDataFromPath(string path)
        //{
            //string pathInAssetFolder = "Assets/ColorApp/";
            //string pathInPackages = "Packages/com.jairoandrety.colorapp/";
            //ColorAppData objInPackeages = AssetDatabase.LoadAssetAtPath<ColorAppData>(pathInPackages + path);
            //ColorAppData objInAssetFolder = AssetDatabase.LoadAssetAtPath<ColorAppData>(pathInAssetFolder + path);
            //return objInPackeages != null ? objInPackeages : objInAssetFolder;
        //}

        public static string GetPackagePath()
        {
            string pathInAssetFolder = "Assets/ColorApp/";
            string pathInPackages = "Packages/com.jairoandrety.colorapp/";
            
            if(Directory.Exists(pathInAssetFolder))
                return pathInAssetFolder;
            else if (Directory.Exists(pathInPackages))
                return pathInPackages;
            else
                return null;
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
    }
}