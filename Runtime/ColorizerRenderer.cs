using Jairoandrety.ColorApp;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerRenderer : Colorizer
    {
        // Start is called before the first frame update
        public override void SetColor(Color color)
        {
            base.SetColor(color);
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = color;
        }
    }
}