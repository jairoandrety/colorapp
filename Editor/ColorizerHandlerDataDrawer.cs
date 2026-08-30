using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    [CustomPropertyDrawer(typeof(ColorizerHandlerData))]
    public class ColorizerHandlerDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty selectedProp = property.FindPropertyRelative("colorPaletteSelected");
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            string[] paletteNames = ColorAppUtils.PaletteNames().ToArray();

            EditorGUI.LabelField(new Rect(position.x, position.y, position.width, lineHeight), label);

            bool previousEnabled = GUI.enabled;
            GUI.enabled = previousEnabled && paletteNames.Length > 0;
            Rect popupRect = new Rect(position.x, position.y + lineHeight + spacing, position.width, lineHeight);

            // Solo se escribe si el usuario cambia el desplegable: asignar
            // siempre igualaba todos los handlers seleccionados a la paleta del
            // primero.
            EditorGUI.showMixedValue = selectedProp.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(popupRect, selectedProp.intValue, paletteNames);
            if (EditorGUI.EndChangeCheck())
                selectedProp.intValue = newIndex;
            EditorGUI.showMixedValue = false;

            GUI.enabled = previousEnabled;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
        }
    }
}
