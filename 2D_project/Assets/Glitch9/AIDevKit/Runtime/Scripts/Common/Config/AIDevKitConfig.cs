
using Glitch9.CoreLib.IO.Audio;

namespace Glitch9.AIDevKit
{
    internal class AIDevKitConfig
    {
        #region Dynamic Flags 

#if GLITCH9_AIDEVKIT_PRO
        internal const bool IsPro = true;
#else
        internal const bool IsPro = false; 
#endif 


        #endregion Dynamic Flags


        #region Default AIDevKit Settings

        internal const bool kComponentGenerator = true;
        internal const bool kScriptDebugger = true;
        internal const int kDefaultTimeoutInSeconds = 30;
        internal const double kFreePriceMagicNumber = -1;
        internal const int kDefaultRecordingDururationInSec = 30;
        internal const string kRecordingFileNameFormat = "recording_{yyyyMMddHHmmss}.wav";
        internal const SampleRate kDefaultRecordingSampleRate = SampleRate.Hz16000;
        internal const int kDefaultAssistantAPIInitialDelayForRunStateCheckInSec = 5;
        internal const int kDefaultAssistantAPIRecurringRunStateCheckIntervalInSec = 2;
        internal const int kDefaultAssistantAPIRunOperationTimeoutInSec = 90;
        internal const int kDefaultAssistantAPIAssistantsFetchCount = 20;
        internal const int kDefaultAssistantAPIMaxPromptTokensForRunRequests = -1;
        internal const int kDefaultAssistantAPIMaxCompletionTokensForRunRequests = -1;

        #endregion Default AIDevKit Settings


        #region Default AI Models & Voices (for AIDevKit default settings and fallbacks) 

        // Written on 2025-05-01 by Munchkin
        internal const string kDefault_OpenAI_LLM = "gpt-4o";
        internal const string kDefault_OpenAI_IMG = "gpt-image-1";
        internal const string kDefault_OpenAI_TTS = "tts-1";
        internal const string kDefault_OpenAI_STT = "whisper-1";
        internal const string kDefault_OpenAI_EMB = "text-embedding-ada-002";
        internal const string kDefault_OpenAI_MOD = "omni-moderation-latest";
        internal const string kDefault_OpenAI_ASS = "gpt-4o";
        internal const string kDefault_OpenAI_RTM = "gpt-4o-realtime-preview";
        internal const string kDefault_OpenAI_Voice = "alloy";

        internal const string kDefault_Google_LLM = "models/gemini-2.0-flash";
        internal const string kDefault_Google_IMG = "models/gemini-2.0-flash-exp-image-generation";
        internal const string kDefault_Google_EMB = "models/embedding-001";
        internal const string kDefault_Google_VID = "models/veo-2.0-generate-001";

        internal const string kDefault_ElevenLabs_TTS = "eleven_flash_v2_5";
        internal const string kDefault_ElevenLabs_VCM = "eleven_english_sts_v2";
        internal const string kDefault_ElevenLabs_STT = "scribe_v1";
        internal const string kDefault_ElevenLabs_Voice = "21m00Tcm4TlvDq8ikWAM"; // Rachel

        // Editor Chat Summary Model
        internal const string kDefault_Chat_Model = "gpt-4o";
        internal const string kDefault_Chat_SummaryModel = "gpt-4o-mini";

        internal static readonly string[] kAllDefaultModels = new string[]
        {
            kDefault_OpenAI_LLM,
            kDefault_OpenAI_IMG,
            kDefault_OpenAI_TTS,
            kDefault_OpenAI_STT,
            kDefault_OpenAI_EMB,
            kDefault_OpenAI_MOD,
            kDefault_OpenAI_RTM,
            kDefault_Google_LLM,
            kDefault_Google_IMG,
            kDefault_Google_EMB,
            kDefault_ElevenLabs_TTS,
            kDefault_ElevenLabs_VCM,
            kDefault_Chat_Model,
            kDefault_Chat_SummaryModel
        };

        internal static readonly string[] kAllDefaultVoices = new string[]
        {
            kDefault_OpenAI_Voice,
            kDefault_ElevenLabs_Voice
        };

        #endregion Default AI Models   


        #region Completion Request Config 

        internal const float kTemperatureDefault = 1f;
        internal const float kTemperatureMin = 0f;
        internal const float kTemperatureMax = 2f;
        internal const float kTopPDefault = 1f;
        internal const float kTopPMin = 0f;
        internal const float kTopPMax = 1f;

        internal const float kFrequencyPenaltyDefault = 0f;
        internal const float kFrequencyPaneltyMin = -2f;
        internal const float kFrequencyPenaltyMax = 2f;

        #endregion Completion Request Config

        #region TTS Request Config

        internal const float kVoiceSpeedDefault = 1f;
        internal const float kVoiceSpeedMin = 0.25f;
        internal const float kVoiceSpeedMax = 4f;

        #endregion TTS Request Config 

        #region ScriptableObject Config

        private const string kPackageName = "AI DevKit";
        private const string kCreateRoot = kPackageName + "/";

        internal const string kAIDevKitSettings = kCreateRoot + "AIDevKit Settings";
        internal const string kOpenAISettings = kCreateRoot + "OpenAI Settings";
        internal const string kGoogleSettings = kCreateRoot + "Google Settings";
        internal const string kElevenLabsSettings = kCreateRoot + "Eleven Labs Settings";
        internal const string kMubertSettings = kCreateRoot + "Mubert Settings";
        internal const string kOllamaSettings = kCreateRoot + "Ollama Settings";
        internal const string kOpenRouterSettings = kCreateRoot + "OpenRouter Settings";
        internal const string kLogDatabase = kCreateRoot + "Log Database";
        internal const string kModelLibrary = kCreateRoot + "My Model Library";
        internal const string kFileDatabase = kCreateRoot + "My File Library";
        internal const string kVoiceDatabase = kCreateRoot + "My Voice Library";
        internal const string kModelProfile = kCreateRoot + "AI Model Profile";
        internal const string kVoiceProfile = kCreateRoot + "AI Voice Profile";
        internal const string kAIDevKitApiKeys = kCreateRoot + "API Keys";

        internal const int kAIDevKitSettingsOrder = 0;
        internal const int kOpenAISettingsOrder = kAIDevKitSettingsOrder + 1;
        internal const int kGoogleSettingsOrder = kOpenAISettingsOrder + 1;
        internal const int kElevenLabsSettingsOrder = kGoogleSettingsOrder + 1;
        internal const int kMubertSettingsOrder = kElevenLabsSettingsOrder + 1;
        internal const int kOllamaSettingsOrder = kMubertSettingsOrder + 1;
        internal const int kOpenRouterSettingsOrder = kOllamaSettingsOrder + 1;
        internal const int kLogDatabaseOrder = kOpenRouterSettingsOrder + 15;
        internal const int kModelLibraryOrder = kLogDatabaseOrder + 1;
        internal const int kFileLibraryOrder = kModelLibraryOrder + 1;
        internal const int kVoiceLibraryOrder = kFileLibraryOrder + 1;
        internal const int kModelProfileOrder = kVoiceLibraryOrder + 15;
        internal const int kVoiceProfileOrder = kModelProfileOrder + 1;
        internal const int kAIDevKitApiKeysOrder = kVoiceProfileOrder + 1;

        #endregion ScriptableObject Config
    }
}