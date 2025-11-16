using Glitch9.AIDevKit.Google;
using Glitch9.EditorKit;
using Glitch9.ScriptableObjects;
using UnityEditor;

namespace Glitch9.AIDevKit.Editor
{
    internal class GenerativeAISettingsProvider : AIClientSettingsProvider<GenerativeAISettingsProvider, GenerativeAISettings>
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            GenerativeAISettingsProvider provider = new(AIDevKitEditorConfig.kProviderSettingsGoogle)
            {
                deactivateHandler = DeactivateHandler,
                keywords = Keywords
            };
            return provider;
        }

        private static void DeactivateHandler()
        {
            GenerativeAISettings.Instance.Save();
        }

        private SerializedProperty projectId;
        private SerializedProperty defaultLLM;
        private SerializedProperty defaultEMB;
        private SerializedProperty defaultIMG;
        private SerializedProperty defaultVID;

        public GenerativeAISettingsProvider(string path) : base(AIProvider.Google, false, path) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();

            projectId = serializedObject.FindProperty(nameof(projectId));

            defaultLLM = serializedObject.FindProperty(nameof(defaultLLM));
            defaultEMB = serializedObject.FindProperty(nameof(defaultEMB));
            defaultIMG = serializedObject.FindProperty(nameof(defaultIMG));
            defaultVID = serializedObject.FindProperty(nameof(defaultVID));
        }

        protected override void DrawOptionalSettings()
        {
            EditorGUILayout.PropertyField(projectId, GUIContents.ApiProjectIdLabel);
        }

        protected override void DrawAdditionalSections()
        {
            ExGUILayout.BeginSection(GUIContents.DefaultModelsSectionTitle);
            {
                AIDevKitGUI.LLMPopup(defaultLLM, AIProvider.Google, GUIContents.DefaultLLM);
                AIDevKitGUI.EMBPopup(defaultEMB, AIProvider.Google, GUIContents.DefaultEMB);
                AIDevKitGUI.IMGPopup(defaultIMG, AIProvider.Google, GUIContents.DefaultIMG);
                AIDevKitGUI.VIDPopup(defaultVID, AIProvider.Google, GUIContents.DefaultVID);
            }
        }

        protected override void DrawUsefulLinks()
        {
            AIDevKitGUI.UrlButtons(
                ("Get API Key Guide", "https://ai.google.dev/gemini-api/docs/api-key"),
                ("Create Google Account", "https://myaccount.google.com/"),
                ("Manage Google API Keys", "https://aistudio.google.com/app/apikey")
            );

            AIDevKitGUI.UrlButtons(
                ("Google Cloud Console", "https://console.cloud.google.com/"),
                ("Google AI Studio", "https://aistudio.google.com/")
            );
        }
    }
}