#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [ExecuteAlways]
    public class Colorizer : MonoBehaviour
    {
        public ColorizerData colorizerData;

        protected ColorizerHandler colorizerHandler;

        protected Color GetColor()
        {
            if (colorizerData.overrideColor)
                return colorizerData.customColor;

            return ColorAppUtils.GetColorPaletteSetup().palettes[colorizerHandler.colorizerHandlerData.colorPaletteSelected].colors[colorizerData.selectedIndex].color;
        }

        private void OnEnable()
        {
#if UNITY_6000_0_OR_NEWER
            colorizerHandler = FindAnyObjectByType<ColorizerHandler>();
#else
            colorizerHandler = FindObjectOfType<ColorizerHandler>();
#endif
            if (colorizerHandler != null)
            {
                colorizerHandler.OnPalleteColorChange += SetColor;
                SetColor();
            }
        }

        private void OnDisable()
        {
            if(colorizerHandler != null)
            {
                colorizerHandler.OnPalleteColorChange -= SetColor;
            }
        }

        public virtual void SetColor() { }
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    [CustomEditor(typeof(Colorizer), true)]
    public class ColorizerEditor : Editor
    {
        private int previousIndex = -1;
        bool previousOverride = false;
        Color previousColor = Color.white;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            var colorizer = (Colorizer)target;
            // Detectar cambio manualmente
            SerializedProperty dataProp = serializedObject.FindProperty("colorizerData");

            if(dataProp != null)
            {
                //SerializedProperty indexProp = dataProp.FindPropertyRelative("selectedIndex");
                var indexProp = dataProp.FindPropertyRelative("selectedIndex");
                var overrideProp = dataProp.FindPropertyRelative("overrideColor");
                var colorProp = dataProp.FindPropertyRelative("customColor");

                //int currentIndex = indexProp.intValue;

                //if (currentIndex != previousIndex)
                //{
                //    previousIndex = currentIndex;

                //    // Ejecutar tu lógica cuando cambie el índice
                //    colorizer.SetColor();

                //    // Marcar objeto sucio para que se actualice en editor
                //    EditorUtility.SetDirty(colorizer);
                //}

                bool changed = false;

                if (indexProp != null && indexProp.intValue != previousIndex)
                {
                    previousIndex = indexProp.intValue;
                    changed = true;
                }

                if (overrideProp != null && overrideProp.boolValue != previousOverride)
                {
                    previousOverride = overrideProp.boolValue;
                    changed = true;
                }

                if (colorProp != null && colorProp.colorValue != previousColor)
                {
                    previousColor = colorProp.colorValue;
                    changed = true;
                }

                if (changed)
                {
                    colorizer.SetColor();
                    EditorUtility.SetDirty(colorizer);
                }
            }

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