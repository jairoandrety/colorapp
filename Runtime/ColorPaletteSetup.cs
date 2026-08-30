using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// LEGACY. Contenedor de paletas anterior a 2.0. Sustituido por
    /// ColorPaletteLibrary. Se conserva (mismo archivo y mismo GUID de script)
    /// para que los .asset ya existentes sigan deserializando y puedan migrarse.
    /// </summary>
    [System.Obsolete("Formato legacy. Usa ColorPaletteLibrary; esta clase solo sirve para migrar assets antiguos.", false)]
    public class ColorPaletteSetup : ScriptableObject
    {
        public List<Palette> palettes = new List<Palette>();
    }
}