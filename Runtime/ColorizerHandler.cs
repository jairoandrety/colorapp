using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Jairoandrety.ColorApp
{
    [ExecuteAlways]
    public class ColorizerHandler : MonoBehaviour
    {
        // Evento estatico: los Colorizer se suscriben una sola vez al arrancar
        // y no necesitan encontrar ni cachear una instancia de Handler. Antes
        // cada Colorizer hacia su propio FindAnyObjectByType en OnEnable, lo
        // que costaba un barrido de escena por objeto y, si el Handler se
        // creaba despues, ese Colorizer no volvia a recibir avisos.
        public static event Action PaletteChanged;

        // Ultimo ColorizerHandler habilitado. Con un solo Handler por escena
        // (el caso normal) esto es exactamente ese Handler; con varios, el
        // mas reciente gana, igual que el FindAnyObjectByType original elegia
        // "cualquiera" sin garantia de orden.
        public static ColorizerHandler Active { get; private set; }

        public ColorizerHandlerData colorizerHandlerData = new ColorizerHandlerData();

        [Tooltip("Libreria de paletas usada por esta escena. Al asignarla aqui, " +
                 "una build no necesita que el asset este en una carpeta Resources.")]
        [SerializeField] private ColorPaletteLibrary library;

        [Tooltip("Variante activa. Cada paleta decide como se llaman sus dos variantes.")]
        [SerializeField] private ColorVariant activeVariant = ColorVariant.Primary;

        public ColorPaletteLibrary Library => library;

        public ColorVariant ActiveVariant
        {
            get => activeVariant;
            set
            {
                if (activeVariant == value)
                    return;

                activeVariant = value;
                ColorizerAll();
            }
        }

        private void OnEnable()
        {
            Active = this;

            // Publica la referencia para que los Colorizer (y los drawers del
            // editor) resuelvan sin depender de Resources.Load.
            if (library != null)
                ColorAppUtils.SetLibrary(library);

            // Si algun Colorizer ya se habia habilitado antes que este Handler,
            // ya esta suscrito al evento estatico: este aviso es lo que lo
            // pone al dia con la paleta y variante activas.
            ColorizerAll();
        }

        private void OnDisable()
        {
            if (Active == this)
                Active = null;
        }

        [ContextMenu("ColorizerAll")]
        public void ColorizerAll()
        {
            PaletteChanged?.Invoke();

#if UNITY_EDITOR
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
#endif
        }
    }
}
