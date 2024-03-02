using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [CreateAssetMenu(fileName = "ColorAppData", menuName = "ColorApp/ColorAppData")]
    public class ColorAppData : ScriptableObject
    {
        public string defaultPalettePath = "PaletteTargetSetup";
        public bool useCustomPalette;
        public string customPalettePath;

        public List<string> colorLabels;

        public string CurrentPalettePath => useCustomPalette ? customPalettePath : defaultPalettePath;
        public bool UseCustomPalette => useCustomPalette;

        public List<string> ColorLabels => colorLabels;

        public void SetUseCustomPalette(bool value)
        {
            useCustomPalette = value;
        }

        public void SetCustomPalettePath(string path)
        {
            customPalettePath = path;
        }

        public void SetColorLabels(List<string> labels)
        {
            colorLabels = labels;
        }
    }
}