#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [ExecuteAlways]
    public class ColorizerHandler : MonoBehaviour
    {
        public Action OnPalleteColorChange;
        public ColorizerHandlerData colorizerHandlerData;

        [ContextMenu("ColorizerAll")]
        public void ColorizerAll()
        {
//#if UNITY_6000_0_OR_NEWER
//            colorizers = FindObjectsByType<Colorizer>(FindObjectsSortMode.None);
//#else
//            colorizers = FindObjectsOfType<Colorizer>();
//#endif
//            foreach (var colorizer in colorizers)
//            {                
//                colorizer.SetColor(ColorAppUtils.GetColorPaletteSetup().palettes[colorizerHandlerData.colorPaletteSelected].colors[colorizer.colorizerData.selectedIndex].color);              
//            }

            OnPalleteColorChange?.Invoke();

#if UNITY_EDITOR
            EditorApplication.QueuePlayerLoopUpdate();
            SceneView.RepaintAll();
#endif
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

            EditorGUILayout.Space(20);
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