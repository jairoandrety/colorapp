using Jairoandrety.ColorApp;
using UnityEngine;
using UnityEngine.UI;

namespace Jairoandrety.ColorApp
{
    /// <summary>
    /// Ejemplo minimo: un Toggle que cambia entre la variante clara y la
    /// oscura de la paleta activa. En 2.0, claro/oscuro es una variante
    /// dentro de la misma paleta (ver ColorSlot), no dos paletas distintas.
    /// </summary>
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
            colorizerHandler.ActiveVariant = value ? ColorVariant.Primary : ColorVariant.Secondary;
        }
    }
}
