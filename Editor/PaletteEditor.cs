using Jairoandrety.ColorApp;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using UnityEditor;

# if UNITY_EDITOR
#endif
using UnityEngine;
using UnityEngine.UIElements;

namespace Jairoandrety.ColorAppEditor
{
#if UNITY_EDITOR
    [System.Serializable]
    public class PaletteEditor
    {
        public string paletteName = string.Empty;
        public List<ColorPallete> colors = new List<ColorPallete>();
        //public List<string> labels = new List<string>();
        //public List<Color> colors = new List<Color>();
    }

    [CustomPropertyDrawer(typeof(PaletteEditor))]
    public class PaletteEditorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SerializedProperty paletteName = property.FindPropertyRelative("paletteName");
            //SerializedProperty propertyNames = property.FindPropertyRelative("labels");
            SerializedProperty colors = property.FindPropertyRelative("colors");

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), paletteName);
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            //int maxLength = Mathf.Max(propertyNames.arraySize, colors.arraySize);
            int maxLength = colors.arraySize;

            for (int i = 0; i < maxLength; i++)
            {
                float y = position.y + (lineHeight * i) + (spacing * i) + (lineHeight + spacing);
                //Rect nameRect = new Rect(position.x, y, position.width * 0.5f, lineHeight);
                //Rect colorRect = new Rect(position.x + position.width / 2, y, position.width * 0.5f, lineHeight);
                Rect colorRect = new Rect(position.x, y, position.width, lineHeight);

                //if (i < propertyNames.arraySize)
                //{
                //    EditorGUI.LabelField(nameRect, propertyNames.GetArrayElementAtIndex(i).stringValue);
                //}

                if (i < colors.arraySize)
                {
                    EditorGUI.PropertyField(colorRect, colors.GetArrayElementAtIndex(i), GUIContent.none);
                }
            }

            //float buttonPos = position.y + ((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * names.arraySize);
            //Rect addButtonRect = new Rect(position.x, buttonPos + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width * 0.4975f, lineHeight);
            //Rect removeButtonRect = new Rect(position.x + position.width / 2, buttonPos + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width * 0.4975f, lineHeight);

            //if (GUI.Button(addButtonRect, "Add Color"))
            //{
            //    names.arraySize++;
            //    colors.arraySize++;
            //}

            //if (GUI.Button(removeButtonRect, "Remove Color"))
            //{
            //    names.arraySize--;
            //    colors.arraySize--;
            //}

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calcula la altura necesaria para el Property Drawer
            //SerializedProperty names = property.FindPropertyRelative("labels");
            SerializedProperty colors = property.FindPropertyRelative("colors");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            float height = EditorGUIUtility.singleLineHeight;

            int maxLength = colors.arraySize;

            for (int i = 0; i <maxLength; i++)
            {
                height += lineHeight + padding;
            }

            //height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // Altura del botón

            return height;
        }
    }
}
#endif