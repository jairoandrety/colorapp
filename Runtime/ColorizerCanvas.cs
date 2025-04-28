#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.UI;

namespace Jairoandrety.ColorApp
{
    //[ExecuteAlways]
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

//#if UNITY_EDITOR
//    [ExecuteAlways]
//    [CustomEditor(typeof(ColorizerCanvas))]
//    public class ColorizerCanvasEditor : Editor
//    {
//        private int previousIndex = -1;

//        public override void OnInspectorGUI()
//        {
//            serializedObject.Update();
//            DrawDefaultInspector();

//            var colorizer = (Colorizer)target;
//            // Detectar cambio manualmente
//            SerializedProperty dataProp = serializedObject.FindProperty("colorizerData");
//            SerializedProperty indexProp = dataProp.FindPropertyRelative("selectedIndex");

//            if (indexProp != null)
//            {
//                int currentIndex = indexProp.intValue;

//                if (currentIndex != previousIndex)
//                {
//                    previousIndex = currentIndex;

//                    // Ejecutar tu lógica cuando cambie el índice
//                    colorizer.SetColor();

//                    // Marcar objeto sucio para que se actualice en editor
//                    EditorUtility.SetDirty(colorizer);
//                }
//            }

//            EditorGUILayout.Space();
//            if (ColorAppUtils.ColorLabels().Count > 0)
//            {

//            }
//            else
//            {
//                EditorGUILayout.HelpBox("No color palettes found, please set one in the palette editor: Window/ColorApp/ColorAppEditor.", MessageType.Info);
//            }

//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//#endif
}