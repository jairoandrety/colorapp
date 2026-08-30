using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    [CustomPropertyDrawer(typeof(ColorizerData))]
    public class ColorizerDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var selectedIndexProp = property.FindPropertyRelative("selectedIndex");
            var overrideColorProp = property.FindPropertyRelative("overrideColor");
            var overridePrimaryProp = property.FindPropertyRelative("overridePrimary");
            var overrideSecondaryProp = property.FindPropertyRelative("overrideSecondary");

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;
            float y = position.y;

            EditorGUI.LabelField(new Rect(position.x, y, position.width, lineHeight), label);
            y += lineHeight + spacing;

            // Las keys son las de la paleta activa: cada paleta define las
            // suyas, ya no hay lista global de labels.
            string[] slotKeys = ColorAppUtils.DefaultPaletteSlotKeys().ToArray();
            bool previousEnabled = GUI.enabled;
            GUI.enabled = previousEnabled && slotKeys.Length > 0;

            // Popup y ToggleLeft devuelven un valor cada repintado, asi que
            // asignarlo siempre escribe siempre. Con varios objetos
            // seleccionados eso estampa el valor del PRIMERO en todos los demas
            // sin que nadie toque nada. De ahi showMixedValue + BeginChangeCheck:
            // se escribe solo cuando el usuario cambia el control de verdad.
            EditorGUI.showMixedValue = selectedIndexProp.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUI.Popup(
                new Rect(position.x, y, position.width, lineHeight),
                "Color Index", selectedIndexProp.intValue, slotKeys);
            if (EditorGUI.EndChangeCheck())
                selectedIndexProp.intValue = newIndex;
            EditorGUI.showMixedValue = false;

            GUI.enabled = previousEnabled;
            y += lineHeight + spacing;

            EditorGUI.showMixedValue = overrideColorProp.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();
            bool newOverride = EditorGUI.ToggleLeft(
                new Rect(position.x, y, position.width, lineHeight),
                "Override Color", overrideColorProp.boolValue);
            if (EditorGUI.EndChangeCheck())
                overrideColorProp.boolValue = newOverride;
            EditorGUI.showMixedValue = false;
            y += lineHeight + spacing;

            // Con la seleccion mezclada tambien se muestran: si no, el override
            // de los que lo tienen activo quedaria inaccesible. PropertyField ya
            // pinta el guion de "valores distintos" por su cuenta.
            if (overrideColorProp.boolValue || overrideColorProp.hasMultipleDifferentValues)
            {
                float half = position.width * 0.5f;
                EditorGUI.PropertyField(new Rect(position.x, y, half - 2f, lineHeight), overridePrimaryProp, new GUIContent("Primary"));
                EditorGUI.PropertyField(new Rect(position.x + half + 2f, y, half - 2f, lineHeight), overrideSecondaryProp, new GUIContent("Secondary"));
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            int rows = 3;
            var overrideColorProp = property.FindPropertyRelative("overrideColor");
            if (overrideColorProp != null && (overrideColorProp.boolValue || overrideColorProp.hasMultipleDifferentValues))
                rows++;

            return (lineHeight + spacing) * rows;
        }
    }
}
