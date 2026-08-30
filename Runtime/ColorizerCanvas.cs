using UnityEngine;
using UnityEngine.UI;

namespace Jairoandrety.ColorApp
{
    public class ColorizerCanvas : Colorizer
    {
        public override void SetColor()
        {
            base.SetColor();

            Graphic graphic = GetComponent<Graphic>();
            if (graphic != null)
                graphic.color = GetColor();
        }
    }
}
