#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    public class ColorizerHandler : MonoBehaviour
    {
        public ColorizerHandlerData colorizerHandlerData;

        Colorizer[] colorizers;
        [ContextMenu("ColorizerAll")]
        public void ColorizerAll()
        {
            colorizers = FindObjectsOfType<Colorizer>();
            foreach (var colorizer in colorizers)
            {                
                colorizer.SetColor(ColorAppUtils.GetColorPaletteSetup().palettes[colorizerHandlerData.colorPaletteSelected].colors[colorizer.colorizerData.selectedIndex].color);              
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ColorizerHandler))]
    public class ColorizerHandlerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ColorizerHandler colorizer = (ColorizerHandler)target;
            DrawDefaultInspector();

            EditorGUILayout.Space();
            if (ColorAppUtils.PaletteNames().Count > 0)
            {
                if (GUILayout.Button("Colorizer All"))
                {
                    colorizer.ColorizerAll();
                }
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