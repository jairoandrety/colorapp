using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class ColorPallete
    {
        public string label;
        public Color color;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ColorPallete))]
    public class ColorPaletteDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty propertyLabel = property.FindPropertyRelative("label");
            SerializedProperty propertyColor = property.FindPropertyRelative("color");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            //float y = position.y + lineHeight + spacing;
            float y = position.y;
            Rect nameRect = new Rect(position.x, y, position.width * 0.5f, lineHeight);
            Rect colorRect = new Rect(position.x + position.width / 2, y, position.width * 0.5f, lineHeight);

            EditorGUI.LabelField(nameRect, propertyLabel.stringValue);
            EditorGUI.PropertyField(colorRect, propertyColor, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty names = property.FindPropertyRelative("label");
            SerializedProperty color = property.FindPropertyRelative("color");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight;

            height += lineHeight + padding;           
            return height - lineHeight;
        }
    }
#endif
}