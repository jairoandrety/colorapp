using System.Collections.Generic;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// Asset unico que contiene todas las paletas del proyecto. Sustituye a
    /// ColorPaletteSetup (paletas) y a ColorAppData (lista global de labels),
    /// que quedan solo como formato legacy para la migracion.
    /// </summary>
    [CreateAssetMenu(fileName = "ColorPaletteLibrary", menuName = "ColorApp/Color Palette Library")]
    public class ColorPaletteLibrary : ScriptableObject
    {
        [SerializeField] private List<ColorPalette> _palettes = new List<ColorPalette>();
        [SerializeField] private int _defaultPaletteIndex = 0;
        [SerializeField] private ColorVariant _defaultVariant = ColorVariant.Primary;

        public List<ColorPalette> Palettes => _palettes;
        public int PaletteCount => _palettes != null ? _palettes.Count : 0;
        public bool HasPalettes => PaletteCount > 0;

        public ColorVariant DefaultVariant
        {
            get => _defaultVariant;
            set => _defaultVariant = value;
        }

        public int DefaultPaletteIndex
        {
            get => ClampPaletteIndex(_defaultPaletteIndex);
            set => _defaultPaletteIndex = value;
        }

        /// <summary>
        /// Recorta un indice de paleta al rango valido. Devuelve -1 si no hay
        /// ninguna paleta.
        /// </summary>
        public int ClampPaletteIndex(int index)
        {
            if (PaletteCount == 0)
                return -1;

            if (index < 0)
                return 0;

            if (index >= _palettes.Count)
                return _palettes.Count - 1;

            return index;
        }

        public ColorPalette GetPalette(int index)
        {
            int clamped = ClampPaletteIndex(index);
            return clamped < 0 ? null : _palettes[clamped];
        }

        public ColorPalette DefaultPalette => GetPalette(_defaultPaletteIndex);

        /// <summary>
        /// Resuelve un color. Si el indice de paleta o el de slot se salen de
        /// rango se usa el ultimo valor disponible; si no hay nada que resolver
        /// se devuelve el fallback.
        /// </summary>
        public Color Resolve(int paletteIndex, int slotIndex, ColorVariant variant, Color fallback)
        {
            ColorPalette palette = GetPalette(paletteIndex);
            if (palette == null)
                return fallback;

            ColorSlot slot = palette.GetSlot(slotIndex);
            if (slot == null)
                return fallback;

            return slot.Get(variant);
        }

        public List<string> PaletteNames()
        {
            List<string> names = new List<string>();
            if (_palettes == null)
                return names;

            for (int i = 0; i < _palettes.Count; i++)
                names.Add(_palettes[i] != null ? _palettes[i].displayName : string.Empty);

            return names;
        }
    }
}
