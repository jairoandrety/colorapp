#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Jairoandrety.ColorApp
{
    [System.Serializable]
    public class ColorizerData
    {
        public int selectedIndex = 0;

        public bool overrideColor = false;
        public Color customColor = Color.white;
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ColorizerData))]
    public class ColorizerDrawer : PropertyDrawer
    {
        bool enabled = false;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();

            //SerializedProperty colorOptionProperty = property.FindPropertyRelative("selectedIndex");
            //float lineHeight = EditorGUIUtility.singleLineHeight;
            //float popupPos = position.y;
            //enabled = ColorAppUtils.ColorLabels().Count > 0;
            //GUI.enabled = enabled;
            //var labels = ColorAppUtils.ColorLabels().ToArray();
            //Rect popupRect = new Rect(position.x, popupPos + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, lineHeight);
            //EditorGUI.LabelField(new Rect(position.x, popupPos, position.width, lineHeight), label);
            ////colorOptionProperty.intValue = EditorGUI.Popup(popupRect, colorOptionProperty.intValue, labels);

            //int newIndex = EditorGUI.Popup(popupRect, colorOptionProperty.intValue, labels);
            //if (EditorGUI.EndChangeCheck())
            //{
            //    //colorOptionProperty.intValue = newIndex;

            //    //// Apuntar al objeto dueño del property
            //    //var targetObject = property.serializedObject.targetObject as Colorizer;
            //    //if (targetObject != null)
            //    //{
            //    //    targetObject.SetColor();
            //    //    EditorUtility.SetDirty(targetObject);
            //    //}
            //}

            //// Override color
            //SerializedProperty isOverrideColorProperty = property.FindPropertyRelative("isOverrideColor");
            //SerializedProperty overrideColorProperty = property.FindPropertyRelative("overrideColor");
            //Rect overrideColorRect = new Rect(position.x, popupPos + (EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing * 2), position.width, lineHeight);
            //EditorGUI.PropertyField(overrideColorRect, isOverrideColorProperty, new GUIContent("Override Color"));
            //Color color = EditorGUI.ColorField(new Rect(position.x, popupPos + (EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing * 3), position.width, lineHeight), "Override Color", overrideColorProperty.colorValue);

            //if (isOverrideColorProperty.boolValue)
            //{
            //    overrideColorProperty.colorValue = color;
            //}

            //if(EditorGUI.EndChangeCheck())
            //{
            //    colorOptionProperty.intValue = newIndex;

            //    overrideColorProperty.colorValue = color;
            //}

            var selectedIndexProp = property.FindPropertyRelative("selectedIndex");
            var overrideColorProp = property.FindPropertyRelative("overrideColor");
            var customColorProp = property.FindPropertyRelative("customColor");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;

            // Label
            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), label);
            y += lineHeight + spacing;

            // Dropdown de colores
            GUI.enabled = ColorAppUtils.ColorLabels().Count > 0;
            Rect popupRect = new Rect(position.x, position.y + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing), position.width, lineHeight);
            //selectedIndexProp.intValue = EditorGUI.Popup(new Rect(position.x, y, position.width, lineHeight), "Color Index", selectedIndexProp.intValue, ColorAppUtils.ColorLabels().ToArray());
            selectedIndexProp.intValue = EditorGUI.Popup(popupRect, "Color Index", selectedIndexProp.intValue, ColorAppUtils.ColorLabels().ToArray());

            GUI.enabled = true;
            y += lineHeight + spacing;

            // Toggle para override
            overrideColorProp.boolValue = EditorGUI.ToggleLeft(new Rect(position.x, y, position.width, lineHeight), "Override Color", overrideColorProp.boolValue);
            y += lineHeight + spacing;

            // Campo de color si el override está activado
            if (overrideColorProp.boolValue)
            {
                customColorProp.colorValue = EditorGUI.ColorField(new Rect(position.x, y, position.width, lineHeight), "Custom Color", customColorProp.colorValue);
                y += lineHeight + spacing;
            }

            // Detectar cambios
            if (EditorGUI.EndChangeCheck())
            {
                // Llamar a SetColor en el componente padre
                //foreach (var obj in property.serializedObject.targetObjects)
                //{
                //    if (obj is Colorizer colorizer)
                //    {
                //        colorizer.SetColor();
                //        EditorUtility.SetDirty(colorizer);
                //    }
                //}
            }

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