namespace Glitch9.AIDevKit.Editor
{
    internal class AIDevKitEditorConfig
    {
        internal const string kPackageName = "AI DevKit";
        internal const string kRootUserPreference = "Preferences/";

        internal const string kOnlineDocUrl = "https://glitch9.gitbook.io/ai-development-kit/";
        internal const string kProviderSettingsCore = kRootUserPreference + kPackageName;
        internal const string kProviderSettingsOpenAI = kRootUserPreference + kPackageName + "/OpenAI";
        internal const string kProviderSettingsGoogle = kRootUserPreference + kPackageName + "/Google Gemini";
        internal const string kProviderSettingsElevenLabs = kRootUserPreference + kPackageName + "/Eleven Labs";
        internal const string kProviderSettingsMubert = kRootUserPreference + kPackageName + "/Mubert";
        internal const string kProviderSettingsOllama = kRootUserPreference + kPackageName + "/Ollama";
        internal const string kProviderSettingsOpenRouter = kRootUserPreference + kPackageName + "/OpenRouter";
    }
}