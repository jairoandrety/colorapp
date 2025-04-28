#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class ColorizerHandlerData
    {
        public int colorPaletteSelected = 0;
    }
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ColorizerHandlerData))]
    public class ColorizerHanlerDataDrawer: PropertyDrawer
    {
        bool enabled = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty colorPaletteSelectedProperty = property.FindPropertyRelative("colorPaletteSelected");
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float popupPos = position.y;

            enabled = ColorAppUtils.PaletteNames().Count > 0;
            GUI.enabled = enabled;
            Rect popupRect = new Rect(position.x, popupPos + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, lineHeight);
            EditorGUI.LabelField(new Rect(position.x, popupPos, position.width, lineHeight), label);
            colorPaletteSelectedProperty.intValue = EditorGUI.Popup(popupRect, colorPaletteSelectedProperty.intValue, ColorAppUtils.PaletteNames().ToArray());
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label);
            return height;
        }
    }
#endif
}