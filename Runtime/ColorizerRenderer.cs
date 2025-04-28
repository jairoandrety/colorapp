using Jairoandrety.ColorApp;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerRenderer : Colorizer
    {
        public override void SetColor()
        {
            base.SetColor();
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = GetColor();
        }
    }
}