using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// LEGACY. Guardaba la lista de labels compartida por todas las paletas.
    /// En 2.0 cada paleta define sus propias keys (ver ColorPalette), asi que
    /// esta clase solo sigue existiendo para que los assets antiguos puedan
    /// deserializarse durante la migracion. No la uses en codigo nuevo.
    /// </summary>
    [System.Obsolete("Formato legacy. Usa ColorPaletteLibrary; esta clase solo sirve para migrar assets antiguos.", false)]
    public class ColorAppData : ScriptableObject
    {
        [SerializeField] private List<string> _colorLabels;
        public List<string> ColorLabels => _colorLabels;
        public void SetColorLabels(List<string> labels)
        {
            _colorLabels = labels;
        }
    }
}