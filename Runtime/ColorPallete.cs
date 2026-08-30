using UnityEngine;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// LEGACY. Entrada de color del formato anterior a 2.0 (un solo color por
    /// label). Sustituida por ColorSlot, que tiene dos variantes. Se conserva
    /// para que los assets antiguos deserialicen y puedan migrarse.
    /// </summary>
    [System.Serializable]
    public class ColorPallete
    {
        public string label;
        public Color color;
    }
}
