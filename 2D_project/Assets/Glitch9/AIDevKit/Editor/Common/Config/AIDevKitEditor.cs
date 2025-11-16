using System;
using Cysharp.Threading.Tasks;
using Glitch9.Internal;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal static class AIDevKitEditor
    {
        internal static class Preferences
        {
            private const string Root = "Preferences/" + AIDevKitEditorConfig.kPackageName;

            internal const string Core = Root;
            internal const string OpenAI = Root + "/OpenAI";
            internal const string GoogleGemini = Root + "/Google Gemini";
            internal const string ElevenLabs = Root + "/Eleven Labs";
            internal const string Mubert = Root + "/Mubert";
        }

        internal static class Orders
        {
            private const int Start = 0;

            internal const int CommonSettings = Start;
            internal const int OpenAISettings = CommonSettings + 1;
            internal const int GeminiSettings = OpenAISettings + 1;
            internal const int LogsRepo = GeminiSettings + 1;
            internal const int ModelsRepo = LogsRepo + 1;
            internal const int FilesRepo = ModelsRepo + 1;
            internal const int ModelMetadata = FilesRepo + 1;
        }

        internal static class Labels
        {
            // --- Editor Tools ---
            internal const string EditorChat = "Editor Chat";
            internal const string EditorVision = "Editor Vision";
            internal const string EditorSpeech = "Editor Speech";

            // --- Shared Managing Tools ---
            internal const string ModelCatalogue = "Model Library";
            internal const string VoiceCatalogue = "Voice Library";

            // --- OpenAI Tools ---
            internal const string OpenAIAssistantManager = "OpenAI Assistant Manager";
            internal const string OpenAIFileManager = "OpenAI File Manager";

            // --- Supports ---
            internal const string OnlineDoc = "Online Documentation";
            internal const string JoinDiscord = "Join Discord Server";

            // --- Preferences --- 
            internal const string PromptHistory = "Prompt History";
            internal const string Preferences = "Preferences";
        }

        internal static class Paths
        {
            private const string Tools = EditorConfig.kToolsPath + AIDevKitEditorConfig.kPackageName + "/";

            // --- Editor Tools ---
            internal const string EditorChat = Tools + Labels.EditorChat;
            internal const string EditorVision = Tools + Labels.EditorVision;
            internal const string EditorSpeech = Tools + Labels.EditorSpeech;

            // --- Shared Managing Tools --- 
            internal const string ModelCatalogue = Tools + Labels.ModelCatalogue;
            internal const string VoiceCatalogue = Tools + Labels.VoiceCatalogue;

            // --- OpenAI Tools ---
            internal const string OpenAIAssistantManager = Tools + Labels.OpenAIAssistantManager;
            internal const string OpenAIFileManager = Tools + Labels.OpenAIFileManager;

            // --- Supports ---
            internal const string OnlineDoc = Tools + Labels.OnlineDoc;
            internal const string JoinDiscord = Tools + Labels.JoinDiscord;

            // --- Preferences ---
            internal const string PromptHistory = Tools + Labels.PromptHistory;
            internal const string Preferences = Tools + Labels.Preferences;
        }

        internal static class Priorities
        {
            private const int Start = EditorConfig.kAIDevKitPriority;

            // --- Editor Tools ---
            internal const int EditorChat = Start;
            internal const int EditorVision = EditorChat + 1;
            internal const int EditorSpeech = EditorVision + 1;

            // --- Shared Managing Tools ---
            internal const int ModelCatalogue = EditorSpeech + 15;
            internal const int VoiceCatalogue = ModelCatalogue + 1;

            // --- OpenAI Tools ---
            internal const int OpenAIAssistantManager = VoiceCatalogue + 15;
            internal const int OpenAIFileManager = OpenAIAssistantManager + 1;

            // --- Supports ---
            internal const int OnlineDoc = OpenAIFileManager + 15;
            internal const int JoinDiscord = OnlineDoc + 1;

            // --- Preferences ---
            internal const int PromptHistory = JoinDiscord + 15;
            internal const int Preferences = PromptHistory + 1;
        }


        [MenuItem(Paths.EditorChat, priority = Priorities.EditorChat)]
        public static void OpenEditorChat() => ShowProWindow(onShowEditorChatWindow);

        [MenuItem(Paths.EditorVision, priority = Priorities.EditorVision)]
        public static void OpenEditorVision() => ShowProWindow(onShowEditorVisionWindow);

        [MenuItem(Paths.EditorSpeech, priority = Priorities.EditorSpeech)]
        public static void OpenEditorSpeech() => ShowProWindow(onShowEditorSpeechWindow);

        [MenuItem(Paths.OpenAIAssistantManager, priority = Priorities.OpenAIAssistantManager)]
        public static void OpenOpenAIAssistantManager() => ShowProWindow(onShowOpenAIAssistantManagerWindow);

        [MenuItem(Paths.OpenAIFileManager, priority = Priorities.OpenAIFileManager)]
        public static void OpenOpenAIFileManager() => ShowProWindow(onShowOpenAIFileManagerWindow);

        [MenuItem(Paths.OnlineDoc, priority = Priorities.OnlineDoc)]
        public static void OpenDocumentURL() => Application.OpenURL(AIDevKitEditorConfig.kOnlineDocUrl);

        [MenuItem(Paths.JoinDiscord, priority = Priorities.JoinDiscord)]
        public static void OpenDiscordURL() => Application.OpenURL(EditorConfig.kDiscordUrl);

        [MenuItem(Paths.PromptHistory, priority = Priorities.PromptHistory)]
        public static void OpenPromptHistory() => ShowProWindow(onShowPromptHistoryWindow);

        [MenuItem(Paths.Preferences, priority = Priorities.Preferences)]
        public static void OpenPreferences() => SettingsService.OpenUserPreferences(AIDevKitEditorConfig.kProviderSettingsCore);



        #region Pro Version Delegates (delegates for using Pro version assembly)

#pragma warning disable IDE1006
        internal static event Action onShowEditorChatWindow;
        internal static event Action onShowEditorVisionWindow;
        internal static event Action onShowEditorSpeechWindow;
        internal static event Action onShowPromptHistoryWindow;
        internal static event Action onShowOpenAIFileManagerWindow;
        internal static event Action onShowOpenAIAssistantManagerWindow;
        internal static event Action onShowElevenLabsSubscriptionWindow;
        internal static event Func<UniTask<bool>> isElevenLabsFreeTierPredicateAsync;

#pragma warning restore IDE1006

        #endregion Pro Version Delegates  
        private static bool? _isElevenLabsFreeTier;

        internal static async UniTask<bool> IsElevenLabsFreeTierAsync()
        {
#if GLITCH9_AIDEVKIT_PRO
            if (_isElevenLabsFreeTier.HasValue) return _isElevenLabsFreeTier.Value;

            if (isElevenLabsFreeTierPredicateAsync == null) throw new NullReferenceException("isElevenLabsFreeTierPredicateAsync is null");

            _isElevenLabsFreeTier = await isElevenLabsFreeTierPredicateAsync.Invoke();
            if (_isElevenLabsFreeTier == null) throw new NullReferenceException("isElevenLabsFreeTierPredicateAsync returned null");

            return _isElevenLabsFreeTier.Value;
#else
            await UniTask.Yield();
            LogProVersionRequired();
            return false;
#endif
        }

        internal static void SetIsElevenLabsFreeTier(bool isFreeTier) => _isElevenLabsFreeTier = isFreeTier;

        internal static void ShowProWindow(Action delegateAction)
        {
#if GLITCH9_AIDEVKIT_PRO
            delegateAction?.Invoke();
#else
            ShowNoProVersionDialog();
#endif
        }

        internal static void ShowElevenLabsSubscriptionWindow()
        {
#if GLITCH9_AIDEVKIT_PRO
            onShowElevenLabsSubscriptionWindow?.Invoke();
#else
            ShowNoProVersionDialog();
#endif
        }

        internal static void ShowNoProVersionDialog()
        {
            if (EditorUtility.DisplayDialog("Pro Version Required", "This feature is only available in the Pro version of the AI Dev Kit.", "Get Pro", "Cancel"))
            {
                OpenProURL();
            }
        }

        internal static void OpenProURL()
        {
            Application.OpenURL("https://assetstore.unity.com/packages/tools/ai-ml-integration/ai-development-kit-gpt4o-assistants-api-v2-281225");
        }

        private static void LogProVersionRequired()
        {
            Debug.LogWarning("This feature is only available in the Pro version of the AI DevKit.");
        }
    }
}