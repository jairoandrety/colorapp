using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [CreateAssetMenu(fileName = "ColorPaletteData", menuName = "ColorApp/ColorPaletteData")]
    public class ColorPaletteData : ScriptableObject
    {
        private string defaultPalettePath = "PaletteTargetSetup";
        private string customPalettePath;

        public string DefaultPalettePath => defaultPalettePath;
        public string CustomPalettePath => customPalettePath;

        public void SetCustomPalettePath(string path)
        {
            customPalettePath = path;
        }
    }
}