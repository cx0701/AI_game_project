using Glitch9.EditorKit;
using Glitch9.EditorKit.IMGUI;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public partial class ModelCatalogueWindow
    {
        private GUIStyle SearchBarStyle => _searchBarStyle ??= new GUIStyle(TreeViewStyles.BottomBarStyle)
        {
            fixedHeight = 56f,
        };
        private GUIStyle _searchBarStyle;

        protected override void BottomBar()
        {
            if (TreeView == null) return;

            GUILayout.BeginHorizontal(SearchBarStyle);
            try
            {
                DrawSearchBar();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        private void ResetFilters()
        {
            ModelCatalogueSettings.ApiProvider = AIProvider.All;
            ModelCatalogueSettings.ModelProvider = AIProvider.All;
            ModelCatalogueSettings.OnlyShowMissingModels = false;
            ModelCatalogueSettings.OnlyShowDefaultModels = false;
            ModelCatalogueSettings.OnlyShowOfficialModels = false;
            ModelCatalogueSettings.OnlyShowCustomModels = false;
            ModelCatalogueSettings.OnlyShowMyLibrary = false;

            ModelCatalogueSettings.ShowDeprecatedModels = true;
            ModelCatalogueSettings.ShowLegacyModels = true;

            TreeView.Filter.SearchText = string.Empty;
            TreeView.ReloadTreeView(true);
        }

        private void DrawSearchBar()
        {
            const float kMinWidth = 130f;
            const float kSpace = 10f;
            const float kBtnWidth = 80f;
            const float kBtnHeight = 40f;

            GUILayout.BeginVertical(GUILayout.MinWidth(kMinWidth));
            {
                EditorGUIUtility.labelWidth = 120f;

                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label($"Displaying {TreeView.ShowingCount}/{TreeView.TotalCount}", EditorStyles.boldLabel, GUILayout.Height(18f), GUILayout.MaxWidth(156f));

                    if (GUILayout.Button(new GUIContent(EditorIcons.Reset, "Reset Filters"), ExEditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        ResetFilters();
                    }
                }
                finally
                {
                    GUILayout.EndHorizontal();
                }

                EditorGUIUtility.labelWidth = 24f;

                AIProvider apiProvider = ExGUILayout.ExtendedEnumPopup(ModelCatalogueSettings.ApiProvider, new GUIContent("API"), true, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(180f));
                if (apiProvider != ModelCatalogueSettings.ApiProvider)
                {
                    ModelCatalogueSettings.ApiProvider = apiProvider;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            EditorGUIUtility.labelWidth = 110f;
            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                bool textGeneration = EditorGUILayout.ToggleLeft("Text Generation", ModelCatalogueSettings.TextGeneration, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (textGeneration != ModelCatalogueSettings.TextGeneration)
                {
                    ModelCatalogueSettings.TextGeneration = textGeneration;
                    TreeView.ReloadTreeView(true);
                }

                bool embedding = EditorGUILayout.ToggleLeft("Text Embedding", ModelCatalogueSettings.TextEmbedding, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (embedding != ModelCatalogueSettings.TextEmbedding)
                {
                    ModelCatalogueSettings.TextEmbedding = embedding;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                bool imageGeneration = EditorGUILayout.ToggleLeft("Image Generation", ModelCatalogueSettings.ImageGeneration, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (imageGeneration != ModelCatalogueSettings.ImageGeneration)
                {
                    ModelCatalogueSettings.ImageGeneration = imageGeneration;
                    TreeView.ReloadTreeView(true);
                }

                bool moderation = EditorGUILayout.ToggleLeft("Moderation", ModelCatalogueSettings.Moderation, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (moderation != ModelCatalogueSettings.Moderation)
                {
                    ModelCatalogueSettings.Moderation = moderation;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                bool speechGeneration = EditorGUILayout.ToggleLeft("Text-to-Speech", ModelCatalogueSettings.SpeechGeneration, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (speechGeneration != ModelCatalogueSettings.SpeechGeneration)
                {
                    ModelCatalogueSettings.SpeechGeneration = speechGeneration;
                    TreeView.ReloadTreeView(true);
                }

                bool functionCalling = EditorGUILayout.ToggleLeft("Function Calling", ModelCatalogueSettings.FunctionCalling, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (functionCalling != ModelCatalogueSettings.FunctionCalling)
                {
                    ModelCatalogueSettings.FunctionCalling = functionCalling;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            EditorGUIUtility.labelWidth = 130f;

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                bool speechRecognition = EditorGUILayout.ToggleLeft("Speech-to-Text", ModelCatalogueSettings.SpeechRecognition, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (speechRecognition != ModelCatalogueSettings.SpeechRecognition)
                {
                    ModelCatalogueSettings.SpeechRecognition = speechRecognition;
                    TreeView.ReloadTreeView(true);
                }

                bool structuredOutput = EditorGUILayout.ToggleLeft("Structured Output", ModelCatalogueSettings.StructuredOutput, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (structuredOutput != ModelCatalogueSettings.StructuredOutput)
                {
                    ModelCatalogueSettings.StructuredOutput = structuredOutput;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                bool voiceChanger = EditorGUILayout.ToggleLeft("Voice Changer", ModelCatalogueSettings.VoiceChanger, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (voiceChanger != ModelCatalogueSettings.VoiceChanger)
                {
                    ModelCatalogueSettings.VoiceChanger = voiceChanger;
                    TreeView.ReloadTreeView(true);
                }

                bool realtime = EditorGUILayout.ToggleLeft("Realtime", ModelCatalogueSettings.Realtime, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (realtime != ModelCatalogueSettings.Realtime)
                {
                    ModelCatalogueSettings.Realtime = realtime;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            EditorGUIUtility.labelWidth = 0f;

            if (GUILayout.Button("Update\nCatalogue", GUILayout.Height(kBtnHeight), GUILayout.Width(kBtnWidth)))
            {
                if (EditorUtility.DisplayDialog("Update Model Catalogue", "This may take a while, do you want to continue?", "Yes", "No"))
                {
                    ModelCatalogue.Instance.UpdateCatalogue((success) =>
                    {
                        if (success)
                        {
                            TreeView.ReloadTreeView(true);
                        }
                    });
                }
            }
        }
    }
}