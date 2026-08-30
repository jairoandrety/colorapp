using System.Collections.Generic;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    /// <summary>
    /// Paleta de arranque que se ofrece cuando el proyecto todavia no tiene
    /// ninguna. Trae claros y oscuros ya puestos para que la herramienta se
    /// pueda probar en el momento, en vez de recibir al usuario con una lista
    /// vacia y ninguna pista de que hacer.
    /// </summary>
    public static class ColorAppSamplePalette
    {
        private static Color Hex(string hex)
        {
            ColorUtility.TryParseHtmlString(hex, out Color color);
            return color;
        }

        public static ColorPalette Create()
        {
            var palette = new ColorPalette
            {
                displayName = "Sample",
                primaryVariantName = "Light",
                secondaryVariantName = "Dark",
                slots = new List<ColorSlot>
                {
                    //                        key              claro       oscuro
                    new ColorSlot("main_color",      Hex("#2F6FED"), Hex("#5C8DF6")),
                    new ColorSlot("accent_color",    Hex("#F5A623"), Hex("#FFBF4D")),
                    new ColorSlot("background",      Hex("#F5F6F8"), Hex("#16181D")),
                    new ColorSlot("surface",         Hex("#FFFFFF"), Hex("#1E2128")),
                    new ColorSlot("text_primary",    Hex("#16181D"), Hex("#E8EAEE")),
                    new ColorSlot("text_secondary",  Hex("#5A6272"), Hex("#9AA3B2")),
                }
            };

            return palette;
        }
    }
}
