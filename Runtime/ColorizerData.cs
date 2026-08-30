using UnityEngine;
using UnityEngine.Serialization;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class ColorizerData
    {
        public int selectedIndex = 0;

        public bool overrideColor = false;

        // Antes era un unico "customColor". Con dos variantes por paleta, un
        // override de un solo color se quedaba congelado al cambiar a la
        // variante oscura; ahora el override tiene su propio par.
        [FormerlySerializedAs("customColor")]
        public Color overridePrimary = Color.white;
        public Color overrideSecondary = Color.white;

        public Color GetOverride(ColorVariant variant)
        {
            return variant == ColorVariant.Secondary ? overrideSecondary : overridePrimary;
        }
    }
}
