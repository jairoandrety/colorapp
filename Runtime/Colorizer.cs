using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [ExecuteAlways]
    public class Colorizer : MonoBehaviour
    {
        public ColorizerData colorizerData = new ColorizerData();

        protected Color GetColor()
        {
            ColorizerHandler active = ColorizerHandler.Active;
            ColorPaletteLibrary library = ColorAppUtils.GetLibrary();

            ColorVariant variant = active != null
                ? active.ActiveVariant
                : (library != null ? library.DefaultVariant : ColorVariant.Primary);

            if (colorizerData.overrideColor)
                return colorizerData.GetOverride(variant);

            if (library == null || !library.HasPalettes)
                return colorizerData.GetOverride(variant);

            // El binding sigue siendo por indice. Si el indice guardado se sale
            // de rango (p. ej. al cambiar a una paleta con menos entradas), la
            // libreria recorta al ultimo valor disponible en vez de lanzar.
            int paletteIndex = active != null
                ? active.colorizerHandlerData.colorPaletteSelected
                : library.DefaultPaletteIndex;

            return library.Resolve(paletteIndex, colorizerData.selectedIndex, variant, colorizerData.GetOverride(variant));
        }

        private void OnEnable()
        {
            // Suscripcion al evento estatico del Handler: no hace falta
            // encontrar ni cachear ninguna instancia. Si el Handler todavia no
            // existe (o se crea despues), el aviso llega igual cuando aparezca.
            ColorizerHandler.PaletteChanged += SetColor;
            SetColor();
        }

        private void OnDisable()
        {
            ColorizerHandler.PaletteChanged -= SetColor;
        }

        public virtual void SetColor() { }
    }
}
