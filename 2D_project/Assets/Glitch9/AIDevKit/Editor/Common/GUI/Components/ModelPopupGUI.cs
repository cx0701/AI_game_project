using System.Collections.Generic;

namespace Glitch9.AIDevKit.Editor
{
    internal class ModelPopupGUI : AssetPopupGUI<Model, ModelFilter>
    {
        protected override Dictionary<AIProvider, List<Model>> GetFilteredAssets(ModelFilter filter) => ModelLibrary.GetFilteredRefs(filter);
        protected override Model GetDefaultAssetId(ModelFilter filter) => GetDefaultModelId(filter);

        internal static string GetDefaultModelId(ModelFilter filter)
        {
            if (filter.Capability != null)
            {
                ModelCapability cap = filter.Capability.Value;
                AIProvider api = ResolveDefaultApi(cap);
                return ResolveDefaultModelId(api, cap);
            }

            return AIDevKitConfig.kDefault_OpenAI_LLM;
        }

        private static AIProvider ResolveDefaultApi(ModelCapability cap)
        {
            return cap switch
            {
                ModelCapability.TextGeneration => GetApi(AIDevKitSettings.DefaultLLM),
                ModelCapability.ImageGeneration => GetApi(AIDevKitSettings.DefaultIMG),
                ModelCapability.SpeechGeneration => GetApi(AIDevKitSettings.DefaultTTS),
                ModelCapability.SpeechRecognition => GetApi(AIDevKitSettings.DefaultSTT),
                ModelCapability.TextEmbedding => GetApi(AIDevKitSettings.DefaultEMB),
                ModelCapability.Moderation => GetApi(AIDevKitSettings.DefaultMOD),
                ModelCapability.SoundFXGeneration => AIProvider.ElevenLabs,
                ModelCapability.VoiceChanger => AIProvider.ElevenLabs,
                ModelCapability.Realtime => AIProvider.OpenAI,
                _ => AIProvider.OpenAI,
            };
        }

        private static AIProvider GetApi(string modelId)
        {
            if (string.IsNullOrEmpty(modelId)) return AIProvider.OpenAI;
            Model model = modelId;
            if (model == null) return AIProvider.OpenAI;
            return model.Api;
        }


        private static string ResolveDefaultModelId(AIProvider api, ModelCapability cap)
        {
            return api switch
            {
                AIProvider.OpenAI => cap switch
                {
                    ModelCapability.TextGeneration => AIDevKitConfig.kDefault_OpenAI_LLM,
                    ModelCapability.ImageGeneration => AIDevKitConfig.kDefault_OpenAI_IMG,
                    ModelCapability.SpeechGeneration => AIDevKitConfig.kDefault_OpenAI_TTS,
                    ModelCapability.SpeechRecognition => AIDevKitConfig.kDefault_OpenAI_STT,
                    ModelCapability.TextEmbedding => AIDevKitConfig.kDefault_OpenAI_EMB,
                    ModelCapability.Moderation => AIDevKitConfig.kDefault_OpenAI_MOD,
                    _ => AIDevKitConfig.kDefault_OpenAI_LLM,
                },
                AIProvider.Google => cap switch
                {
                    ModelCapability.TextGeneration => AIDevKitConfig.kDefault_Google_LLM,
                    ModelCapability.ImageGeneration => AIDevKitConfig.kDefault_Google_IMG,
                    ModelCapability.TextEmbedding => AIDevKitConfig.kDefault_Google_EMB,
                    _ => AIDevKitConfig.kDefault_Google_LLM,
                },
                AIProvider.ElevenLabs => cap switch
                {
                    ModelCapability.SpeechGeneration => AIDevKitConfig.kDefault_ElevenLabs_TTS,
                    ModelCapability.VoiceChanger => AIDevKitConfig.kDefault_ElevenLabs_VCM,
                    _ => AIDevKitConfig.kDefault_ElevenLabs_TTS,
                },
                _ => null,
            };
        }
    }
}