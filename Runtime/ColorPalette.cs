using System.Collections.Generic;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// Una paleta independiente: define sus propios slots y como se llaman sus
    /// dos variantes. Dos paletas no tienen por que compartir keys ni longitud.
    /// </summary>
    [System.Serializable]
    public class ColorPalette
    {
        public string displayName = "New Palette";
        public string primaryVariantName = "Light";
        public string secondaryVariantName = "Dark";
        public List<ColorSlot> slots = new List<ColorSlot>();

        public int SlotCount => slots != null ? slots.Count : 0;

        public string VariantName(ColorVariant variant)
        {
            string name = variant == ColorVariant.Secondary ? secondaryVariantName : primaryVariantName;
            return string.IsNullOrEmpty(name) ? variant.ToString() : name;
        }

        /// <summary>
        /// Devuelve el slot en el indice dado. Si el indice se sale de rango se
        /// usa el ultimo slot disponible en vez de lanzar una excepcion.
        /// </summary>
        public ColorSlot GetSlot(int index)
        {
            if (SlotCount == 0)
                return null;

            if (index < 0)
                index = 0;
            else if (index >= slots.Count)
                index = slots.Count - 1;

            return slots[index];
        }

        public int IndexOfKey(string key)
        {
            if (string.IsNullOrEmpty(key) || slots == null)
                return -1;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i] != null && slots[i].key == key)
                    return i;
            }

            return -1;
        }

        public List<string> SlotKeys()
        {
            List<string> keys = new List<string>();
            if (slots == null)
                return keys;

            for (int i = 0; i < slots.Count; i++)
                keys.Add(slots[i] != null ? slots[i].key : string.Empty);

            return keys;
        }
    }
}
