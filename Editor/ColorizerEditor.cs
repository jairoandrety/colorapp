using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    // Sin CanEditMultipleObjects, Unity ni siquiera llama a OnInspectorGUI al
    // seleccionar dos o mas colorizers: pinta "Multi-object editing is not
    // supported". Y como el CustomEditor lleva editorForChildClasses en true,
    // esto cubre tambien ColorizerCanvas, ColorizerRenderer y
    // ColorizerSpriteRenderer.
    [CustomEditor(typeof(Colorizer), true)]
    [CanEditMultipleObjects]
    public class ColorizerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            DrawDefaultInspector();
            bool changed = EditorGUI.EndChangeCheck();

            EditorGUILayout.Space();
            if (!ColorAppUtils.HasLibrary())
            {
                EditorGUILayout.HelpBox(
                    "No color palettes found, please set one in the palette editor: Window/ColorApp/ColorAppEditor.",
                    MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();

            if (!changed)
                return;

            // Recolorea CADA objeto seleccionado. Usar target repintaba solo el
            // primero y dejaba al resto con el color viejo hasta el siguiente
            // ColorizerAll.
            foreach (Object obj in targets)
            {
                if (obj is Colorizer colorizer)
                {
                    colorizer.SetColor();
                    EditorUtility.SetDirty(colorizer);
                }
            }
        }
    }
}
