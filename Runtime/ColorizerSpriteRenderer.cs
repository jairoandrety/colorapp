using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerSpriteRenderer : Colorizer
    {
        public override void SetColor()
        {
            base.SetColor();

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.color = GetColor();
        }
    }
}