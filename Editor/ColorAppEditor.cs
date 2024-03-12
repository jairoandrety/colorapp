using System;
using System.Collections.Generic;

# if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Jairoandrety.ColorApp;
using System.IO;

namespace Jairoandrety.ColorAppEditor
{
#if UNITY_EDITOR
    public class ColorAppEditor : EditorWindow
    {
        private static string titleWindow = "Color App Editor";
        private SerializedObject so;

        private ColorPaletteSetup targetSetup;

        public List<string> colorLabels = new List<string>() { "PrimaryColor", "SecondColor", "ThirdColor", "PrimaryFontColor", "SecondFontColor" };

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

            if (ColorAppUtils.ValidatePaletteSetup())
            {
                LoadCurrentSetup();
            }
            else
            {
                CreatePaletteSetup();
                LoadCurrentSetup();
            }
            
            if(targetSetup == null)
            {
                LoadPalettesData();
            }

            currentGUISkin = GetUiStyle();
        }

        private GUISkin GetUiStyle()
        {
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

        void OnGUI()
        {
            EditorGUILayout.Space(2);
            var imageStyle = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
            Texture2D miImagen = currentGUISkin.GetStyle("Logo").normal.background;

            if (miImagen != null)
            {
                GUILayout.Label(miImagen, imageStyle, GUILayout.Height(40));
            }           

            EditorGUILayout.Space(2);
            var targetSetupStyle = new GUIStyle(GUI.skin.box) { alignment = TextAnchor.MiddleCenter };

            EditorGUILayout.BeginVertical(targetSetupStyle);
            EditorGUILayout.Space(2);
            EditorGUILayout.LabelField("Palette Target Setup", EditorStyles.boldLabel);
            string targetSetupInfo = "In this section you can load or save data and assign it to a data container of your choice.";
            EditorGUILayout.LabelField(targetSetupInfo, EditorStyles.wordWrappedLabel);

            //lastValue = useCustomSetup;           

            //if(!useCustomSetup)
            //{
            //    LoadCurrentSetup();               
            //}

            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            targetSetup = (ColorPaletteSetup)EditorGUILayout.ObjectField("Target Setup", targetSetup, typeof(ColorPaletteSetup), false);
            if (targetSetup != null)
            {
                if (GUILayout.Button("Load", GUILayout.Width(position.width * 0.15f)))
                {
                    LoadPalettesData();
                }

                if (GUILayout.Button("Save", GUILayout.Width(position.width * 0.15f)))
                {
                    SavePalettesData();
                }
            }
            else
            {
                if (GUILayout.Button("Create Setup", GUILayout.Width(position.width * 0.20f)))
                {
                    CreatePaletteSetup();
                    LoadCurrentSetup();
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
            EditorGUILayout.EndVertical();

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

        #region Load and Save Setup Asset
        private void ValidateAndCreateResoucesFolder()
        {
            string resourcesPath = "Assets/Resources";

            if (!AssetDatabase.IsValidFolder(resourcesPath))
            {
                AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void CreatePaletteSetup()
        {
            ValidateAndCreateResoucesFolder();
            ColorAppUtils.CreatePaletteSetup();
            AssetDatabase.Refresh();
        }

        private void LoadCurrentSetup()
        {
            targetSetup = ColorAppUtils.GetColorPaletteSetup();
            AssetDatabase.Refresh();
        }
        #endregion
              
        public void LoadPalettesData()
        {
            if (targetSetup == null)
                return;

            colorPalettes.Clear();
            colorLabels.Clear();

            for (int i = 0; i < ColorAppUtils.GetColorAppData().ColorLabels.Count; i++)
            {
                colorLabels.Add(ColorAppUtils.GetColorAppData().ColorLabels[i]);
            }

            for (int i = 0; i < targetSetup.palettes.Count; i++)
            {
                PaletteEditor paletteEditor = new PaletteEditor();

                paletteEditor.paletteName = targetSetup.palettes[i].paletteName;

                //for (int j = 0; j < targetSetup.palettes[i].labels.Count; j++)
                //{
                //    paletteEditor.labels.Add(targetSetup.palettes[i].labels[j]);
                //}

                //for (int k = 0; k < targetSetup.palettes[i].colors.Count; k++)
                //{
                //    paletteEditor.colors.Add(targetSetup.palettes[i].colors[k]);
                //}

                for (int j = 0; j < targetSetup.palettes[i].colors.Count; j++)
                {
                    ColorPallete NewColorPalette = new ColorPallete()
                    {
                        label = targetSetup.palettes[i].colors[j].label,
                        color = targetSetup.palettes[i].colors[j].color
                    };
                    paletteEditor.colors.Add(NewColorPalette);
                }

                colorPalettes.Add(paletteEditor);
            }

            so.Update();
        }

        public void SavePalettesData()
        {
            if (targetSetup == null) return;

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
            }

            //ColorAppUtils.GetColorAppData().SetUseCustomPalette(useCustomSetup);
            //ColorAppUtils.GetColorAppData().SetCustomPalettePath(useCustomSetup ? AssetDatabase.GetAssetPath(targetSetup) : string.Empty);
            so.Update();
            EditorUtility.SetDirty(this);
            EditorUtility.SetDirty(targetSetup);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif