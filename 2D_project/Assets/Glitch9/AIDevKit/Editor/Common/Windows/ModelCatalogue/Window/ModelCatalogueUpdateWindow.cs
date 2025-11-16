using System.Collections.Generic;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal class ModelCatalogueUpdateWindow : EditorWindow
    {
        internal static void ShowWindow(List<IModelData> newModels, List<IModelData> deprecatedModels)
        {
            ModelCatalogueUpdateWindow popup = GetWindow<ModelCatalogueUpdateWindow>(true, "Model Updates", true);
            popup.Initialize(newModels, deprecatedModels);
        }

        private List<IModelData> _newModels;
        private List<IModelData> _deprecatedModels;
        private Vector2 _scrollPosition;

        private void Initialize(List<IModelData> newModels, List<IModelData> deprecatedModels)
        {
            _newModels = newModels;
            _deprecatedModels = deprecatedModels;
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical(ExEditorStyles.popupBody);
            {
                GUILayout.BeginVertical(ExEditorStyles.darkBackground);
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.ExpandHeight(true));
                    {
                        if (_newModels.Count > 0)
                        {
                            GUILayout.Label($"New Models ({_newModels.Count})", ExEditorStyles.bigBoldLabel);

                            foreach (IModelData model in _newModels)
                            {
                                DrawNewModel(model);
                            }
                        }

                        if (_deprecatedModels.Count > 0)
                        {
                            GUILayout.Label($"Deprecated Models ({_deprecatedModels.Count})", ExEditorStyles.bigBoldLabel);

                            foreach (IModelData model in _deprecatedModels)
                            {
                                DrawDeprecatedModel(model);
                            }
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                if (GUILayout.Button("Close", ExEditorStyles.bigButton))
                {
                    Close();
                }
            }
            GUILayout.EndVertical();
        }


        private void DrawNewModel(IModelData model)
        {
            const float kHeight = 20f;

            GUILayout.BeginHorizontal(GUILayout.Height(kHeight));
            {
                // Layout: [Icon] [Name] [CreatedAt] [Status(legacy, deprecated)]
                Texture icon = EditorIcons.StatusCheck;
                string name = model.Name;

                GUILayout.Label(icon, GUILayout.Width(20f), GUILayout.Height(kHeight));
                GUILayout.Label(name, EditorStyles.label, GUILayout.ExpandWidth(true), GUILayout.Height(kHeight));
                GUILayout.Label(model.CreatedAt?.ToString("yyyy-MM-dd"), EditorStyles.label, GUILayout.Width(100f), GUILayout.Height(kHeight));

                if (GUILayout.Button("Add to Library", GUILayout.Width(100f), GUILayout.Height(kHeight)))
                {
                    ModelCatalogueUtil.AddToLibrary(model.Id);
                }
            }
            GUILayout.EndHorizontal();
        }

        private void DrawDeprecatedModel(IModelData model)
        {
            const float kHeight = 20f;

            GUILayout.BeginHorizontal(GUILayout.Height(kHeight));
            {
                // Layout: [Icon] [Name] [CreatedAt] [Status(legacy, deprecated)]
                Texture icon = EditorIcons.StatusObsolete;
                string name = model.Name;

                GUILayout.Label(icon, GUILayout.Width(20f), GUILayout.Height(kHeight));
                GUILayout.Label(name, EditorStyles.label, GUILayout.ExpandWidth(true), GUILayout.Height(kHeight));
                GUILayout.Label(model.CreatedAt?.ToString("yyyy-MM-dd"), EditorStyles.label, GUILayout.Width(100f), GUILayout.Height(kHeight));

                if (GUILayout.Button("Remove from Library", GUILayout.Width(100f), GUILayout.Height(kHeight)))
                {
                    ModelCatalogueUtil.RemoveFromLibrary(model.Api, model.Id);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}