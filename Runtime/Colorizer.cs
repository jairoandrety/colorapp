#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{ 
    public class Colorizer : MonoBehaviour
    {
        public ColorizerData colorizerData;
        public virtual void SetColor(Color color) { }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(Colorizer))]
    public class ColorizerEditor : Editor
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