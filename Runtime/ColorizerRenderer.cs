using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerRenderer : Colorizer
    {
        // _BaseColor (URP Lit/Unlit) primero, _Color (Built-in/Standard,
        // Sprites-Default) como fallback. Ninguna de las dos requiere
        // preguntarle al material shared cual usar en cada frame: se resuelve
        // una vez por cambio de material.
        private static readonly int BaseColorId = Shader.PropertyToID("_BaseColor");
        private static readonly int ColorId = Shader.PropertyToID("_Color");

        private MaterialPropertyBlock propertyBlock;
        private Material lastMaterial;
        private int resolvedColorId;

        public override void SetColor()
        {
            base.SetColor();

            Renderer renderer = GetComponent<Renderer>();
            if (renderer == null || renderer.sharedMaterial == null)
                return;

            // MaterialPropertyBlock nunca instancia un material propio, a
            // diferencia de renderer.material: cero clones, cero fugas.
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();

            if (renderer.sharedMaterial != lastMaterial)
            {
                lastMaterial = renderer.sharedMaterial;
                resolvedColorId = lastMaterial.HasProperty(BaseColorId) ? BaseColorId : ColorId;
            }

            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(resolvedColorId, GetColor());
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
