using UnityEditor;
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class ColorizerData
    {
        public int selectedIndex = 0;
    }

    [CustomPropertyDrawer(typeof(ColorizerData))]
    public class ColorizerDrawer : PropertyDrawer
    {
        bool enabled = false;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty colorOptionProperty = property.FindPropertyRelative("selectedIndex");

            //EditorGUI.PropertyField(position, colorOptionProperty, label);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float popupPos = position.y;
            enabled = ColorAppUtils.ColorLabels().Count > 0;
            GUI.enabled = enabled;
            Rect popupRect = new Rect(position.x, popupPos + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, lineHeight);
            EditorGUI.LabelField(new Rect(position.x, popupPos, position.width, lineHeight), label);
            colorOptionProperty.intValue = EditorGUI.Popup(popupRect, colorOptionProperty.intValue, ColorAppUtils.ColorLabels().ToArray());
            GUI.enabled = true;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property, label);
            return height;
        }
    }
}