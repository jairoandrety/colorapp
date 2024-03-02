using Jairoandrety.ColorApp;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace jairoandrety.ColorApp
{
    public class SimpleChangeColor : MonoBehaviour
    {
        public ColorizerHandler colorizerHandler;
        public Toggle toggle;

        void Start()
        {
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool value)
        {
            if (value)
                colorizerHandler.colorizerHandlerData.colorPaletteSelected = 0;
            else
                colorizerHandler.colorizerHandlerData.colorPaletteSelected = 1;

            colorizerHandler.ColorizerAll();
        }
    }
}