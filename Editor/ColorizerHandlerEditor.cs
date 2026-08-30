using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    [CustomEditor(typeof(ColorizerHandler))]
    [CanEditMultipleObjects]
    public class ColorizerHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.Space(20);
            if (ColorAppUtils.PaletteNames().Count > 0)
            {
                if (GUILayout.Button("Colorizer All"))
                {
                    // Una sola llamada basta aunque haya varios handlers
                    // seleccionados: ColorizerAll levanta el evento ESTATICO
                    // PaletteChanged, que avisa a todos los colorizers de la
                    // escena, y estos leen ColorizerHandler.Active, no el
                    // handler que pulso el boton.
                    ((ColorizerHandler)target).ColorizerAll();
                }
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "No color palettes found, please set one in the palette editor: Window/ColorApp/ColorAppEditor.",
                    MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
