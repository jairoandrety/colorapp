using System;
using System.Collections.Generic;

# if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Jairoandrety.ColorApp;

namespace Jairoandrety.ColorAppEditor
{
#if UNITY_EDITOR
    public class ColorAppEditor : EditorWindow
    {
        private static string titleWindow = "Color App Editor";
        private SerializedObject so;

        private ColorPaletteSetup customSetup;
        private ColorPaletteSetup targetSetup;

        private ColorAppData colorAppData;
        //public List<string> colorLabels = new List<string>() { "PrimaryColor", "SecondColor", "ThirdColor", "PrimaryFontColor", "SecondFontColor" };
        public List<string> colorLabels = new List<string>();
            
        public List<PaletteEditor> colorPalettes = new List<PaletteEditor>();
        private Vector2 scrollPos = Vector2.zero;
        private GUISkin currentGUISkin;

        [MenuItem("Window/ColorApp/ColorAppEditor")]
        public static void ShowWindow()
        {
            var window = GetWindow<ColorAppEditor>();
            window.titleContent = new GUIContent(titleWindow);
            window.Focus();
        }

        private void OnEnable()
        {
            ScriptableObject target = this;
            so = new SerializedObject(target);
            
            targetSetup = ColorAppUtils.GetColorPaletteSetup();
            AssetDatabase.Refresh();
            
            if(targetSetup == null)
            {
                LoadDefaultPalettesData();
            }

            currentGUISkin = GetUiStyle();
        }

        private GUISkin GetUiStyle()
        {
            //bool isInPackage = false;

            //string packageName = "com.jairoandrety.colorapp";
            //string packagePath = Path.Combine("Packages", packageName);

            //isInPackage = Directory.Exists(packagePath);            

            //string GUIStylePath = "Packages/com.jairoandrety.colorapp/Editor/Resources/GuiSkin/ColorAppGUISkin.guiskin";
            string pathInAssetFolder = "Assets/ColorApp/";
            string pathInPackages = "Packages/com.jairoandrety.colorapp/";
            string GUIStylePath = "Editor/Resources/GuiSkin/ColorAppGUISkin.guiskin";

            try
            {
                GUISkin guiSkinPackages = (GUISkin)AssetDatabase.LoadAssetAtPath(pathInPackages + GUIStylePath, typeof(GUISkin));
                GUISkin guiSkinAsset = (GUISkin)AssetDatabase.LoadAssetAtPath(pathInAssetFolder + GUIStylePath, typeof(GUISkin));

                if(guiSkinPackages != null)
                {
                    return guiSkinPackages;
                }
                else if(guiSkinAsset != null)
                {
                    return guiSkinAsset;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return null;
        }

        bool showHelp = false;

        void OnGUI()
        {
            EditorGUILayout.Space(2);
            var imageStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            Texture2D logo = currentGUISkin.GetStyle("Logo").normal.background;

            if (logo != null)
            {
                GUILayout.Label(logo, imageStyle, GUILayout.Height(40));
            }           

            EditorGUILayout.Space(2);
            var targetSetupStyle = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter };

#region Palette Setup
            EditorGUILayout.BeginVertical(targetSetupStyle);
            EditorGUILayout.Space(2);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUILayout.LabelField("Palette Target Setup", EditorStyles.boldLabel);
            if (GUILayout.Button("?", EditorStyles.miniButton, GUILayout.Width(20)))
            {
                showHelp = !showHelp;
                
                // show help dialog
                //EditorUtility.DisplayDialog("Help", "This is the target setup for your color palettes. You can load or save data here.", "OK");
                // Show help box
                //EditorUtility.DisplayDialog("Help", "This is the target setup for your color palettes. You can load or save data here.", "OK");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            string targetSetupInfo = "In this section, you can load or save color palettes.\nBy default, all data will be saved in the 'Target Setup' palette.";
            EditorGUILayout.LabelField(targetSetupInfo, EditorStyles.helpBox);

            if (showHelp)
            {
                EditorGUILayout.HelpBox("How does it work?\n" +
                                        "All changes made in this panel are temporary until you press the save button.\n" +
                                        "All changes will be saved in the 'Target Setup' palette, located in the Resources folder, and will be used for the tool's operation.\n" +
                                        "If you load a custom palette, you can use it to load a custom color palette.\n" +
                                        "If you press the save button, the changes will be applied to both the custom palette and the default palette.", MessageType.None);
            }
            
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            customSetup = (ColorPaletteSetup)EditorGUILayout.ObjectField("Custom Setup", customSetup, typeof(ColorPaletteSetup), false);
            if (customSetup)
            {
                if (GUILayout.Button("Load Custom Palette", GUILayout.Width(position.width * 0.25f)))
                {
                    LoadCustomPalettesData();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            GUI.enabled = false; // Deshabilitar interacción
            targetSetup = (ColorPaletteSetup)EditorGUILayout.ObjectField("Target Setup", targetSetup, typeof(ColorPaletteSetup), false);
            GUI.enabled = true; // Restaurar interacción
            
            if (targetSetup == null)
            {
                targetSetup = ColorAppUtils.GetColorPaletteSetup();
                AssetDatabase.Refresh();
            }
            
            if (targetSetup != null)
            {
                if (GUILayout.Button("Load", GUILayout.Width(position.width * 0.15f)))
                {
                    LoadDefaultPalettesData();
                }

                if (GUILayout.Button("Save", GUILayout.Width(position.width * 0.15f)))
                {
                    SavePalettesData();
                }
            }
           // else
            //{
            //    if (GUILayout.Button("Create Setup", GUILayout.Width(position.width * 0.20f)))
            //    {
            //        CreatePaletteSetup();
            //        targetSetup = ColorAppUtils.GetColorPaletteSetup();
            //        AssetDatabase.Refresh();
            //    }
            //}

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            EditorGUILayout.EndVertical();
#endregion
            
            EditorGUILayout.BeginVertical(targetSetupStyle);
            if (targetSetup == null)
            {
                EditorGUILayout.Space(5);
                var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                EditorGUILayout.LabelField("No tienes un archivo de configuración", style, GUILayout.ExpandWidth(true));
                EditorGUILayout.Space(5);
            }
            else
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.LabelField("Palletes", EditorStyles.boldLabel);
                
                GUI.enabled = false; // Deshabilitar interacción
                colorAppData = (ColorAppData)EditorGUILayout.ObjectField("ColorAppData", colorAppData, typeof(ColorAppData), false);
                GUI.enabled = true; // Restaurar interacción
                if (colorAppData == null)
                {
                    colorAppData = ColorAppUtils.GetColorAppData();
                    AssetDatabase.Refresh();
                }
                
                SerializedProperty listStringProperty = so.FindProperty(nameof(colorLabels));
                EditorGUILayout.PropertyField(listStringProperty, true);

                ShowPalleteButtons();
                scrollPos = GUILayout.BeginScrollView(scrollPos, false, false, GUILayout.ExpandHeight(true));
                if (colorPalettes.Count > 0)
                {
                    SerializedProperty localPalettesProperty = so.FindProperty(nameof(colorPalettes));
                    if (localPalettesProperty.arraySize > 0)
                    {
                        EditorGUILayout.PropertyField(localPalettesProperty, true);
                    }
                }
                else
                {
                    GUILayout.Space(5);
                    var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
                    EditorGUILayout.LabelField("No tienes paletas creadas", style, GUILayout.ExpandWidth(true));
                    EditorGUILayout.Space(5);
                }
                EditorGUILayout.EndHorizontal();

                GUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginVertical(currentGUISkin.box);
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

            GUILayout.Label("Created By Jairoandrety", EditorStyles.boldLabel);
            if(GUILayout.Button("Visit Website", EditorStyles.miniButton, GUILayout.Width(150)))
            {
                Application.OpenURL("https://jairoandrety.wordpress.com");
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            so.ApplyModifiedProperties();
        }      

        #region Buttons Palette Buttons
        private void ShowPalleteButtons()
        {
            var style = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter };

            float buttonWidth = position.width * 0.32f;

            EditorGUILayout.BeginHorizontal(style, GUILayout.ExpandWidth(true));

            if (GUILayout.Button("Add New Palette"))
            {
                //List<string> Labels = new List<string>();
                //List<Color> colors = new List<Color>();
                List<ColorPallete> colors = new List<ColorPallete>();

                for (int i = 0; i < colorLabels.Count; i++)
                {
                    ColorPallete colorPallete = new ColorPallete()
                    {
                        label = colorLabels[i],
                        color = Color.white,
                    };

                    colors.Add(colorPallete);
                }

                PaletteEditor palette = new PaletteEditor()
                {
                    paletteName = "New Palette",
                    colors = colors
                    //labels = Labels,
                    //colors = colors
                };

                colorPalettes.Add(palette);
                so.Update();
            }

            if (GUILayout.Button("Remove Last Palette"))
            {
                if (colorPalettes.Count > 0)
                {
                    colorPalettes.RemoveAt(colorPalettes.Count - 1);
                }

                so.Update();
            }
            if (GUILayout.Button("Clear All Palettes"))
            {
                colorPalettes.Clear();
                so.Update();
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
        }
        #endregion

        private void OnInspectorUpdate()
        {
            if (colorPalettes.Count == 0)
                return;

            for (int i = 0; i < colorPalettes.Count; i++)
            {
                if (colorPalettes[i].colors.Count != colorLabels.Count)
                {
                    if (colorPalettes[i].colors.Count < colorLabels.Count)
                    {
                        //while (colorPalettes[i].labels.Count < colorLabels.Count)
                        //{
                        //    colorPalettes[i].labels.Add(colorLabels[i]);
                        //}

                        //while (colorPalettes[i].colors.Count < colorLabels.Count)
                        //{
                        //    colorPalettes[i].colors.Add(Color.white);
                        //}                       

                        while (colorPalettes[i].colors.Count < colorLabels.Count)
                        {
                            colorPalettes[i].colors.Add(new ColorPallete());
                        }
                    }

                    if (colorPalettes[i].colors.Count > colorLabels.Count)
                    {
                        //while (colorPalettes[i].labels.Count > colorLabels.Count)
                        //{
                        //    colorPalettes[i].labels.RemoveAt(colorPalettes[i].labels.Count - 1);
                        //}

                        //while (colorPalettes[i].colors.Count > colorLabels.Count)
                        //{
                        //    colorPalettes[i].colors.RemoveAt(colorPalettes[i].colors.Count - 1);
                        //}

                        while (colorPalettes[i].colors.Count > colorLabels.Count)
                        {
                            colorPalettes[i].colors.RemoveAt(colorPalettes[i].colors.Count - 1);
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < colorLabels.Count; j++)
                    {
                        colorPalettes[i].colors[j].label = colorLabels[j];
                    }
                }
            }

            so.Update();
        }
              
        public void LoadCustomPalettesData()
        {
            if (customSetup == null)
                return;

            colorLabels.Clear();

            if (customSetup.palettes.Count == 0) 
                return;
            
            if (customSetup.palettes[0].colors.Count > 0)
            {
                for (int i = 0; i < customSetup.palettes[0].colors.Count; i++)
                {
                    colorLabels.Add(customSetup.palettes[0].colors[i].label);
                }
            }
            
            AssignPalettesData(customSetup);
        }
        
        public void LoadDefaultPalettesData()
        {
            if (targetSetup == null)
                return;

            colorLabels.Clear();
            colorAppData = ColorAppUtils.GetColorAppData();

            if (colorAppData.ColorLabels.Count > 0)
            {
                for (int i = 0; i < colorAppData.ColorLabels.Count; i++)
                {
                    colorLabels.Add(colorAppData.ColorLabels[i]);
                }
            }
            
            AssignPalettesData(targetSetup);
        }
        
        public void AssignPalettesData(ColorPaletteSetup setup)
        {
            if (setup == null)
                return;

            colorPalettes.Clear();

            for (int i = 0; i < setup.palettes.Count; i++)
            {
                PaletteEditor paletteEditor = new PaletteEditor();
                paletteEditor.paletteName = setup.palettes[i].paletteName;

                for (int j = 0; j < setup.palettes[i].colors.Count; j++)
                {
                    ColorPallete NewColorPalette = new ColorPallete()
                    {
                        label = setup.palettes[i].colors[j].label,
                        color = setup.palettes[i].colors[j].color
                    };
                    paletteEditor.colors.Add(NewColorPalette);
                }

                colorPalettes.Add(paletteEditor);
            }

            so.Update();
        }

        public void SavePalettesData()
        {
            if (targetSetup == null)
                return;

            if(ColorAppUtils.GetColorAppData().ColorLabels == null)
            {
                ColorAppUtils.GetColorAppData().SetColorLabels(new List<string>());
            }

            ColorAppUtils.GetColorAppData().ColorLabels.Clear();

            for (int i = 0; i < colorLabels.Count; i++)
            {
                ColorAppUtils.GetColorAppData().ColorLabels.Add(colorLabels[i]);
            }

            targetSetup.palettes.Clear();
            if(customSetup != null && customSetup != targetSetup)
            {
                customSetup.palettes.Clear();
            }

            for (int i = 0; i < colorPalettes.Count; i++)
            {
                Palette palette = new Palette();
                string paletteName = colorPalettes[i].paletteName;
                palette.paletteName = paletteName;

                //for (int j = 0; j < colorPalettes[i].labels.Count; j++)
                //{
                //    palette.labels.Add(colorPalettes[i].labels[j]);
                //}

                //for (int k = 0; k < colorPalettes[i].colors.Count; k++)
                //{
                //    palette.colors.Add(colorPalettes[i].colors[k]);
                //}

                for (int j = 0; j < colorPalettes[i].colors.Count; j++)
                {
                    ColorPallete newColorPallete = new ColorPallete()
                    {
                        label = colorPalettes[i].colors[j].label,
                        color = colorPalettes[i].colors[j].color
                    };

                    palette.colors.Add(newColorPallete);
                }

                targetSetup.palettes.Add(palette);
                if(customSetup != null && customSetup != targetSetup)
                {
                    customSetup.palettes.Add(palette);
                }
            }

            //ColorAppUtils.GetColorAppData().SetUseCustomPalette(useCustomSetup);
            //ColorAppUtils.GetColorAppData().SetCustomPalettePath(useCustomSetup ? AssetDatabase.GetAssetPath(targetSetup) : string.Empty);
            so.Update();
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(targetSetup);
            if(customSetup != null)
            {
                EditorUtility.SetDirty(customSetup);
            }
            AssetDatabase.SaveAssets();
        }
    }
}
#endif