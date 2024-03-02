using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class Palette
    {
        public string paletteName = string.Empty;
        public List<ColorPallete> colors = new List<ColorPallete>();
        //public List<string> labels = new List<string>();
        //public List<Color> colors = new List<Color>();
    }

//#if UNITY_EDITOR
//    [CustomPropertyDrawer(typeof(Palette))]
//    public class PaletteDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//        {
//            EditorGUI.BeginProperty(position, label, property);
//            SerializedProperty propertyPaletteName = property.FindPropertyRelative("paletteName");
//            //SerializedProperty propertyNames = property.FindPropertyRelative("labels");
//            SerializedProperty propertyColors = property.FindPropertyRelative("colors");

//            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), propertyPaletteName.stringValue);
            
//            float lineHeight = EditorGUIUtility.singleLineHeight;
//            float spacing = EditorGUIUtility.standardVerticalSpacing;

//            //int maxLength = Mathf.Max(propertyNames.arraySize, propertyColors.arraySize);
//            int maxLength = propertyColors.arraySize;

//            for (int i = 0; i < maxLength; i++)
//            {
//                float y = position.y + (lineHeight * i) + (spacing * i);
//                //Rect nameRect = new Rect(position.x, y, position.width * 0.5f, lineHeight);
//                //Rect colorRect = new Rect(position.x + position.width / 2, y, position.width * 0.5f, lineHeight);

//                Rect colorRect = new Rect(position.x, y, position.width, lineHeight);

//                //if (i < propertyNames.arraySize)
//                //{
//                //    EditorGUI.LabelField(nameRect, propertyNames.GetArrayElementAtIndex(i).stringValue);
//                //}

//                //if (i < propertyColors.arraySize)
//                //{
//                //    EditorGUI.PropertyField(colorRect, propertyColors.GetArrayElementAtIndex(i), GUIContent.none);
//                //}

//                if (i < propertyColors.arraySize)
//                {                    
//                    EditorGUI.PropertyField(colorRect, propertyColors.GetArrayElementAtIndex(i), GUIContent.none);
//                }
//            }

//            float buttonPos = position.y + ((EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * propertyColors.arraySize);     
//            EditorGUI.EndProperty();
//        }

//        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//        {
//            //SerializedProperty names = property.FindPropertyRelative("labels");
//            //SerializedProperty colors = property.FindPropertyRelative("colors");

//            SerializedProperty colors = property.FindPropertyRelative("colors");

//            float lineHeight = EditorGUIUtility.singleLineHeight;
//            float padding = EditorGUIUtility.standardVerticalSpacing;
//            float height = EditorGUIUtility.singleLineHeight;

//            for (int i = 0; i < colors.arraySize; i++)
//            {
//                height += lineHeight + padding;
//            }

//            return height;
//        }
//    }
//#endif
}