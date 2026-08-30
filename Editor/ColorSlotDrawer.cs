using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    /// <summary>
    /// Dibuja un ColorSlot en una fila: key, color de la variante primaria y
    /// color de la secundaria. Las dos columnas son el nucleo del formato 2.0.
    /// </summary>
    [CustomPropertyDrawer(typeof(ColorSlot))]
    public class ColorSlotDrawer : PropertyDrawer
    {
        private const float ColorWidth = 62f;
        private const float Gap = 4f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty keyProp = property.FindPropertyRelative("key");
            SerializedProperty primaryProp = property.FindPropertyRelative("primary");
            SerializedProperty secondaryProp = property.FindPropertyRelative("secondary");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float keyWidth = Mathf.Max(60f, position.width - (ColorWidth * 2) - (Gap * 2));

            Rect keyRect = new Rect(position.x, position.y, keyWidth, lineHeight);
            Rect primaryRect = new Rect(keyRect.xMax + Gap, position.y, ColorWidth, lineHeight);
            Rect secondaryRect = new Rect(primaryRect.xMax + Gap, position.y, ColorWidth, lineHeight);

            EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            EditorGUI.PropertyField(primaryRect, primaryProp, GUIContent.none);
            EditorGUI.PropertyField(secondaryRect, secondaryProp, GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}
