using Glitch9.AIDevKit.ElevenLabs;
using Glitch9.EditorKit;
using Glitch9.ScriptableObjects;
using UnityEditor;

namespace Glitch9.AIDevKit.Editor.ElevenLabs
{
    internal class ElevenLabsSettingsProvider : AIClientSettingsProvider<ElevenLabsSettingsProvider, ElevenLabsSettings>
    {
        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            ElevenLabsSettingsProvider provider = new(AIDevKitEditorConfig.kProviderSettingsElevenLabs)
            {
                deactivateHandler = DeactivateHandler,
                keywords = Keywords
            };
            return provider;
        }

        private static void DeactivateHandler()
        {
            ElevenLabsSettings.Instance.Save();
        }

        private SerializedProperty defaultTTS;
        private SerializedProperty defaultVCM;
        private SerializedProperty defaultVoice;

        public ElevenLabsSettingsProvider(string path) : base(AIProvider.ElevenLabs, true, path) { }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();

            defaultTTS = serializedObject.FindProperty(nameof(defaultTTS));
            defaultVCM = serializedObject.FindProperty(nameof(defaultVCM));
            defaultVoice = serializedObject.FindProperty(nameof(defaultVoice));
        }

        protected override void DrawOptionalSettings()
        {
            if (ExGUILayout.ButtonField("Subscription Details"))
            {
                AIDevKitEditor.ShowElevenLabsSubscriptionWindow();
            }
        }

        protected override void DrawAdditionalSections()
        {
            ExGUILayout.BeginSection(GUIContents.DefaultModelsAndVoicesSectionTitle);
            {
                AIDevKitGUI.TTSPopup(defaultTTS, AIProvider.ElevenLabs, GUIContents.DefaultTTS);
                AIDevKitGUI.VCMPopup(defaultVCM, AIProvider.ElevenLabs, GUIContents.DefaultVCM);
                AIDevKitGUI.VoicePopup(defaultVoice, AIProvider.ElevenLabs, GUIContents.DefaultVoice);
            }
        }

        protected override void DrawUsefulLinks()
        {
            AIDevKitGUI.UrlButtons(
                ("Get API Key Guide", "https://glitch9.gitbook.io/ai-development-kit/elevenlabs/getting-the-api-key"),
                ("Create ElevenLabs Account", "https://elevenlabs.io/app/sign-in?redirect=%2Fapp%2Fsettings%2Fapi-keys"),
                ("Manage ElevenLabs API Keys", "https://elevenlabs.io/app/settings/api-keys")
            );

            AIDevKitGUI.UrlButtons(
                ("Pricing", "https://elevenlabs.io/pricing/api"),
                ("Blog", "https://elevenlabs.io/blog"),
                ("Platform (Console)", "https://elevenlabs.io/app/home"),
                ("Subscription", "https://elevenlabs.io/app/subscription")
            );
        }
    }
}