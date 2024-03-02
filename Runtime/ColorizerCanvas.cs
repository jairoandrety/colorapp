#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace Jairoandrety.ColorApp
{
    public class ColorizerCanvas : Colorizer
    {
        public override void SetColor(Color color)
        {
            base.SetColor(color);
            Graphic graphic = GetComponent<Graphic>();
            if (graphic != null)
                graphic.color = color;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ColorizerCanvas))]
    public class ColorizerCanvasEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            EditorGUILayout.Space();
            if (ColorAppUtils.ColorLabels().Count > 0)
            {

            }
            else
            {
                EditorGUILayout.HelpBox("No color palettes found, please set one in the palette editor: Window/ColorApp/ColorAppEditor.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}