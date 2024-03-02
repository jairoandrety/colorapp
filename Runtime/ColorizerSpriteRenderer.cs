using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerSpriteRenderer : Colorizer
    {
        public override void SetColor(Color color)
        {
            base.SetColor(color);

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
        }
    }
}