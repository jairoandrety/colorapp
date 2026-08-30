using UnityEngine;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// Una entrada de color dentro de una paleta: una key y sus dos variantes.
    /// La key es propia de cada paleta; no existe ningun listado global.
    /// </summary>
    [System.Serializable]
    public class ColorSlot
    {
        public string key = "new_color";
        public Color primary = Color.white;
        public Color secondary = Color.white;

        public ColorSlot() { }

        public ColorSlot(string key, Color primary, Color secondary)
        {
            this.key = key;
            this.primary = primary;
            this.secondary = secondary;
        }

        public Color Get(ColorVariant variant)
        {
            return variant == ColorVariant.Secondary ? secondary : primary;
        }

        public void Set(ColorVariant variant, Color color)
        {
            if (variant == ColorVariant.Secondary)
                secondary = color;
            else
                primary = color;
        }
    }
}
