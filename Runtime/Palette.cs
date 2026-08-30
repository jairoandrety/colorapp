using System.Collections.Generic;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// LEGACY. Paleta del formato anterior a 2.0. Sustituida por ColorPalette.
    /// Se conserva para que los assets antiguos deserialicen y puedan migrarse.
    /// </summary>
    [System.Serializable]
    public class Palette
    {
        public string paletteName = string.Empty;
        public List<ColorPallete> colors = new List<ColorPallete>();
    }
}
