using System.Collections.Generic;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    /// <summary>
    /// Avisos sobre el punto flojo de resolver el color por indice: como cada
    /// paleta define sus propias keys, el indice 2 de una paleta y el 2 de otra
    /// pueden ser roles distintos. Cambiar de paleta recolorea entonces cosas de
    /// forma inesperada. El clamp cubre el desborde, no la desalineacion, asi
    /// que conviene avisar antes de que se note en pantalla.
    /// </summary>
    public static class ColorAppDiagnostics
    {
        public struct Mismatch
        {
            public string message;
            public bool isCountMismatch;
        }

        public static List<Mismatch> Analyze(ColorPaletteLibrary library)
        {
            var results = new List<Mismatch>();
            if (library == null || library.PaletteCount < 2)
                return results;

            ColorPalette reference = library.Palettes[0];

            for (int i = 1; i < library.PaletteCount; i++)
            {
                ColorPalette other = library.Palettes[i];
                if (other == null)
                    continue;

                if (other.SlotCount != reference.SlotCount)
                {
                    results.Add(new Mismatch
                    {
                        isCountMismatch = true,
                        message = $"'{other.displayName}' tiene {other.SlotCount} color(es) y " +
                                  $"'{reference.displayName}' tiene {reference.SlotCount}. Al cambiar entre ellas, " +
                                  "los indices que se salgan tomaran el ultimo color."
                    });
                    continue;
                }

                var differing = new List<string>();
                for (int j = 0; j < reference.SlotCount; j++)
                {
                    if (reference.slots[j].key != other.slots[j].key)
                        differing.Add($"#{j} '{reference.slots[j].key}' vs '{other.slots[j].key}'");
                }

                if (differing.Count > 0)
                {
                    results.Add(new Mismatch
                    {
                        isCountMismatch = false,
                        message = $"'{other.displayName}' no coincide con '{reference.displayName}' en: " +
                                  string.Join(", ", differing) + "."
                    });
                }
            }

            return results;
        }
    }
}
