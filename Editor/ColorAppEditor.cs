using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
    /// <summary>
    /// Ventana principal de ColorApp. Edita el ColorPaletteLibrary directamente
    /// a traves de SerializedObject: no hay copias temporales ni boton de Save,
    /// y Undo funciona como en cualquier inspector.
    /// </summary>
    public class ColorAppEditor : EditorWindow
    {
        private const string TitleWindow = "Color App Editor";
        private const float PaletteListWidth = 150f;
        private const float ColorColumnWidth = 70f;
        private const float RowGap = 4f;

        private ColorPaletteLibrary library;
        private SerializedObject librarySO;
        private int selectedPalette;
        private Vector2 listScroll;
        private Vector2 slotScroll;
        private GUISkin currentGUISkin;

        [MenuItem("Window/ColorApp/ColorAppEditor")]
        public static void ShowWindow()
        {
            var window = GetWindow<ColorAppEditor>();
            window.titleContent = new GUIContent(TitleWindow);
            window.minSize = new Vector2(520f, 320f);
            window.Focus();
        }

        private void OnEnable()
        {
            currentGUISkin = GetUiStyle();
            BindLibrary(ColorAppUtils.GetLibrary());
        }

        private void BindLibrary(ColorPaletteLibrary target)
        {
            library = target;
            librarySO = library != null ? new SerializedObject(library) : null;
            selectedPalette = 0;
        }

        private GUISkin GetUiStyle()
        {
            const string skinRelativePath = "Editor/Resources/GuiSkin/ColorAppGUISkin.guiskin";
            string[] roots = { "Packages/com.jairoandrety.colorapp/", "Assets/ColorApp/" };

            foreach (string root in roots)
            {
                var skin = AssetDatabase.LoadAssetAtPath<GUISkin>(root + skinRelativePath);
                if (skin != null)
                    return skin;
            }

            return null;
        }

        private void OnGUI()
        {
            DrawLogo();
            DrawLibraryField();

            if (library == null)
            {
                DrawNoLibraryState();
                GUILayout.FlexibleSpace();
                DrawFooter();
                return;
            }

            if (librarySO == null)
                BindLibrary(library);

            librarySO.Update();

            DrawActiveSelection();

            EditorGUILayout.Space(4);
            EditorGUILayout.BeginHorizontal();
            DrawPaletteList();
            DrawSelectedPalette();
            EditorGUILayout.EndHorizontal();

            librarySO.ApplyModifiedProperties();

            DrawDiagnostics();
            DrawFooter();
        }

        private void DrawLogo()
        {
            EditorGUILayout.Space(2);

            Texture2D logo = null;
            if (currentGUISkin != null)
            {
                GUIStyle logoStyle = currentGUISkin.FindStyle("Logo");
                if (logoStyle != null)
                    logo = logoStyle.normal.background;
            }

            if (logo != null)
            {
                var imageStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                GUILayout.Label(logo, imageStyle, GUILayout.Height(40));
            }
        }

        private void DrawLibraryField()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUI.BeginChangeCheck();
            var picked = (ColorPaletteLibrary)EditorGUILayout.ObjectField(
                "Palette Library", library, typeof(ColorPaletteLibrary), false);
            if (EditorGUI.EndChangeCheck())
            {
                ColorAppUtils.SetLibrary(picked);
                BindLibrary(picked);
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawNoLibraryState()
        {
            EditorGUILayout.Space(6);
            EditorGUILayout.HelpBox(
                "Todavia no hay ninguna libreria de paletas en el proyecto. " +
                "Empieza con la paleta de ejemplo, o migra tus assets del formato anterior si ya usabas ColorApp.",
                MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("Crear con paleta de ejemplo", GUILayout.Height(24)))
                CreateLibrary(withSample: true);

            if (GUILayout.Button("Crear vacia", GUILayout.Height(24)))
                CreateLibrary(withSample: false);

            if (GUILayout.Button("Migrar assets antiguos", GUILayout.Height(24)))
            {
                ColorAppMigration.MigrateWithDialog();
                ColorAppUtils.InvalidateLibraryCache();
                BindLibrary(ColorAppUtils.GetLibrary());
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(6);
        }

        private void CreateLibrary(bool withSample)
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "Crear libreria de paletas", "ColorPaletteLibrary", "asset",
                "Elige donde guardar la libreria de paletas.");

            if (string.IsNullOrEmpty(path))
                return;

            var created = CreateInstance<ColorPaletteLibrary>();
            if (withSample)
                created.Palettes.Add(ColorAppSamplePalette.Create());

            AssetDatabase.CreateAsset(created, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            ColorAppUtils.InvalidateLibraryCache();
            ColorAppUtils.SetLibrary(created);
            BindLibrary(created);
        }

        /// <summary>
        /// Paleta y variante activas, en dos niveles: el valor por defecto del
        /// proyecto (guardado en la libreria) y, si hay un ColorizerHandler en
        /// la escena abierta, el suyo, que es el que manda en esa escena.
        /// </summary>
        private void DrawActiveSelection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Paleta activa", EditorStyles.boldLabel);

            string[] paletteNames = library.PaletteNames().ToArray();
            if (paletteNames.Length == 0)
            {
                EditorGUILayout.LabelField("Anade una paleta para empezar.", EditorStyles.miniLabel);
                EditorGUILayout.EndVertical();
                return;
            }

            SerializedProperty indexProp = librarySO.FindProperty("_defaultPaletteIndex");
            SerializedProperty variantProp = librarySO.FindProperty("_defaultVariant");

            int projectIndex = Mathf.Clamp(indexProp.intValue, 0, paletteNames.Length - 1);
            ColorPalette projectPalette = library.GetPalette(projectIndex);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Proyecto", GUILayout.Width(70f));
            indexProp.intValue = EditorGUILayout.Popup(projectIndex, paletteNames);
            variantProp.enumValueIndex = EditorGUILayout.Popup(
                Mathf.Clamp(variantProp.enumValueIndex, 0, 1), VariantNames(projectPalette));
            EditorGUILayout.EndHorizontal();

            ColorizerHandler handler = ColorizerHandler.Active;
            if (handler != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(new GUIContent("Escena", $"ColorizerHandler en '{handler.name}'"), GUILayout.Width(70f));

                int handlerIndex = Mathf.Clamp(handler.colorizerHandlerData.colorPaletteSelected, 0, paletteNames.Length - 1);
                ColorPalette handlerPalette = library.GetPalette(handlerIndex);

                EditorGUI.BeginChangeCheck();
                int newHandlerIndex = EditorGUILayout.Popup(handlerIndex, paletteNames);
                int newVariant = EditorGUILayout.Popup((int)handler.ActiveVariant, VariantNames(handlerPalette));

                if (EditorGUI.EndChangeCheck())
                {
                    // Preview en vivo: escribir en el Handler y recolorear.
                    Undo.RecordObject(handler, "Cambiar paleta activa");
                    handler.colorizerHandlerData.colorPaletteSelected = newHandlerIndex;
                    handler.ActiveVariant = (ColorVariant)newVariant;
                    handler.ColorizerAll();
                    EditorUtility.SetDirty(handler);
                }

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.LabelField(
                    "Sin ColorizerHandler en la escena: manda el valor de proyecto.",
                    EditorStyles.miniLabel);
            }

            EditorGUILayout.EndVertical();
        }

        private static string[] VariantNames(ColorPalette palette)
        {
            if (palette == null)
                return new[] { "Primary", "Secondary" };

            return new[]
            {
                palette.VariantName(ColorVariant.Primary),
                palette.VariantName(ColorVariant.Secondary)
            };
        }

        private void DrawPaletteList()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(PaletteListWidth));

            EditorGUILayout.LabelField("Paletas", EditorStyles.boldLabel);

            listScroll = EditorGUILayout.BeginScrollView(listScroll, EditorStyles.helpBox, GUILayout.ExpandHeight(true));

            if (library.PaletteCount == 0)
            {
                EditorGUILayout.LabelField("Ninguna", EditorStyles.miniLabel);
            }
            else
            {
                for (int i = 0; i < library.PaletteCount; i++)
                {
                    ColorPalette palette = library.Palettes[i];
                    string name = palette != null && !string.IsNullOrEmpty(palette.displayName)
                        ? palette.displayName
                        : $"Palette {i}";

                    bool isSelected = i == selectedPalette;
                    bool isActive = i == library.DefaultPaletteIndex;
                    string entry = isActive ? name + "  *" : name;

                    if (GUILayout.Toggle(isSelected, entry, EditorStyles.miniButton) && !isSelected)
                        selectedPalette = i;
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
                AddPalette();

            using (new EditorGUI.DisabledScope(library.PaletteCount == 0))
            {
                if (GUILayout.Button("-"))
                    RemoveSelectedPalette();
            }
            EditorGUILayout.EndHorizontal();

            if (library.PaletteCount > 0)
            {
                using (new EditorGUI.DisabledScope(selectedPalette == library.DefaultPaletteIndex))
                {
                    if (GUILayout.Button("Marcar activa"))
                    {
                        librarySO.FindProperty("_defaultPaletteIndex").intValue = selectedPalette;
                        librarySO.ApplyModifiedProperties();
                        RefreshScene();
                    }
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawSelectedPalette()
        {
            EditorGUILayout.BeginVertical();

            if (library.PaletteCount == 0)
            {
                EditorGUILayout.LabelField("Detalle", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true));
                GUILayout.FlexibleSpace();
                var centered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, wordWrap = true };
                EditorGUILayout.LabelField("No tienes paletas creadas.\nPulsa + para anadir una.", centered);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();
                return;
            }

            selectedPalette = Mathf.Clamp(selectedPalette, 0, library.PaletteCount - 1);

            SerializedProperty palettesProp = librarySO.FindProperty("_palettes");
            SerializedProperty paletteProp = palettesProp.GetArrayElementAtIndex(selectedPalette);
            SerializedProperty nameProp = paletteProp.FindPropertyRelative("displayName");
            SerializedProperty primaryNameProp = paletteProp.FindPropertyRelative("primaryVariantName");
            SerializedProperty secondaryNameProp = paletteProp.FindPropertyRelative("secondaryVariantName");
            SerializedProperty slotsProp = paletteProp.FindPropertyRelative("slots");

            EditorGUILayout.LabelField("Detalle", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.ExpandHeight(true));

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(nameProp, new GUIContent("Nombre"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Variantes", GUILayout.Width(EditorGUIUtility.labelWidth - 4f));
            primaryNameProp.stringValue = EditorGUILayout.TextField(primaryNameProp.stringValue);
            secondaryNameProp.stringValue = EditorGUILayout.TextField(secondaryNameProp.stringValue);
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
                RefreshScene();

            EditorGUILayout.Space(6);

            DrawSlotHeader(primaryNameProp.stringValue, secondaryNameProp.stringValue);

            slotScroll = EditorGUILayout.BeginScrollView(slotScroll, GUILayout.ExpandHeight(true));

            if (slotsProp.arraySize == 0)
            {
                EditorGUILayout.LabelField("Sin colores. Pulsa 'Anadir color'.", EditorStyles.miniLabel);
            }
            else
            {
                EditorGUI.BeginChangeCheck();

                for (int i = 0; i < slotsProp.arraySize; i++)
                    DrawSlotRow(slotsProp, i);

                if (EditorGUI.EndChangeCheck())
                    RefreshScene();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Anadir color"))
                AddSlot();

            using (new EditorGUI.DisabledScope(slotsProp.arraySize == 0))
            {
                if (GUILayout.Button("Quitar ultimo"))
                    RemoveLastSlot();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private void DrawSlotHeader(string primaryName, string secondaryName)
        {
            var headerStyle = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("key", headerStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(
                string.IsNullOrEmpty(primaryName) ? "Primary" : primaryName,
                headerStyle, GUILayout.Width(ColorColumnWidth));
            EditorGUILayout.LabelField(
                string.IsNullOrEmpty(secondaryName) ? "Secondary" : secondaryName,
                headerStyle, GUILayout.Width(ColorColumnWidth));
            GUILayout.Space(22f);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSlotRow(SerializedProperty slotsProp, int index)
        {
            SerializedProperty slotProp = slotsProp.GetArrayElementAtIndex(index);
            SerializedProperty keyProp = slotProp.FindPropertyRelative("key");
            SerializedProperty primaryProp = slotProp.FindPropertyRelative("primary");
            SerializedProperty secondaryProp = slotProp.FindPropertyRelative("secondary");

            EditorGUILayout.BeginHorizontal();

            keyProp.stringValue = EditorGUILayout.TextField(keyProp.stringValue);
            primaryProp.colorValue = EditorGUILayout.ColorField(
                GUIContent.none, primaryProp.colorValue, false, true, false, GUILayout.Width(ColorColumnWidth));
            secondaryProp.colorValue = EditorGUILayout.ColorField(
                GUIContent.none, secondaryProp.colorValue, false, true, false, GUILayout.Width(ColorColumnWidth));

            if (GUILayout.Button("x", EditorStyles.miniButton, GUILayout.Width(20f)))
            {
                slotsProp.DeleteArrayElementAtIndex(index);
                librarySO.ApplyModifiedProperties();
                RefreshScene();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();
            GUILayout.Space(RowGap * 0.5f);
        }

        private void DrawDiagnostics()
        {
            List<ColorAppDiagnostics.Mismatch> issues = ColorAppDiagnostics.Analyze(library);
            if (issues.Count == 0)
                return;

            EditorGUILayout.Space(2);
            foreach (var issue in issues)
                EditorGUILayout.HelpBox(issue.message, MessageType.Warning);
        }

        private void AddPalette()
        {
            Undo.RecordObject(library, "Anadir paleta");

            ColorPalette created = library.PaletteCount == 0
                ? ColorAppSamplePalette.Create()
                : new ColorPalette { displayName = "New Palette" };

            library.Palettes.Add(created);
            EditorUtility.SetDirty(library);
            librarySO.Update();
            selectedPalette = library.PaletteCount - 1;
        }

        private void RemoveSelectedPalette()
        {
            if (library.PaletteCount == 0)
                return;

            Undo.RecordObject(library, "Quitar paleta");
            library.Palettes.RemoveAt(Mathf.Clamp(selectedPalette, 0, library.PaletteCount - 1));
            EditorUtility.SetDirty(library);
            librarySO.Update();
            selectedPalette = Mathf.Clamp(selectedPalette, 0, Mathf.Max(0, library.PaletteCount - 1));
            RefreshScene();
        }

        private void AddSlot()
        {
            ColorPalette palette = library.GetPalette(selectedPalette);
            if (palette == null)
                return;

            Undo.RecordObject(library, "Anadir color");
            palette.slots.Add(new ColorSlot($"color_{palette.SlotCount}", Color.white, Color.white));
            EditorUtility.SetDirty(library);
            librarySO.Update();
            RefreshScene();
        }

        private void RemoveLastSlot()
        {
            ColorPalette palette = library.GetPalette(selectedPalette);
            if (palette == null || palette.SlotCount == 0)
                return;

            Undo.RecordObject(library, "Quitar color");
            palette.slots.RemoveAt(palette.slots.Count - 1);
            EditorUtility.SetDirty(library);
            librarySO.Update();
            RefreshScene();
        }

        /// <summary>Repinta los Colorizer de la escena abierta.</summary>
        private void RefreshScene()
        {
            ColorizerHandler handler = ColorizerHandler.Active;
            if (handler != null)
                handler.ColorizerAll();
        }

        private void DrawFooter()
        {
            EditorGUILayout.BeginVertical(currentGUISkin != null ? currentGUISkin.box : GUI.skin.box);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            GUILayout.Label("Created By Jairoandrety", EditorStyles.boldLabel);
            if (GUILayout.Button("Visit Website", EditorStyles.miniButton, GUILayout.Width(150)))
                Application.OpenURL("https://jairoandrety.wordpress.com");

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}
