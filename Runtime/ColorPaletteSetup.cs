using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [CreateAssetMenu(fileName = "ColorPaletteSetup", menuName = "ColorApp/ColorPaletteSetup")]
    public class ColorPaletteSetup : ScriptableObject
    {
        public List<Palette> palettes = new List<Palette>();
    }
}