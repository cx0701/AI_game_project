using UnityEngine;

namespace Glitch9.AIDevKit.ElevenLabs
{
    [CreateAssetMenu(fileName = nameof(ElevenLabsSettings), menuName = AIDevKitConfig.kElevenLabsSettings, order = AIDevKitConfig.kElevenLabsSettingsOrder)]
    public partial class ElevenLabsSettings : AIClientSettings<ElevenLabsSettings>
    {
        // default models
        [SerializeField] private string defaultTTS = AIDevKitConfig.kDefault_ElevenLabs_TTS;
        [SerializeField] private string defaultVCM = AIDevKitConfig.kDefault_ElevenLabs_VCM;
        [SerializeField] private string defaultSTT = AIDevKitConfig.kDefault_ElevenLabs_STT;

        // default voice
        [SerializeField] private string defaultVoice = AIDevKitConfig.kDefault_ElevenLabs_Voice;

        public static string DefaultTTS => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultTTS, AIDevKitConfig.kDefault_ElevenLabs_TTS);
        public static string DefaultVCM => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultVCM, AIDevKitConfig.kDefault_ElevenLabs_VCM);
        public static string DefaultSTT => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultSTT, AIDevKitConfig.kDefault_ElevenLabs_STT);
        public static string DefaultVoice => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultVoice, AIDevKitConfig.kDefault_ElevenLabs_Voice);

        public static bool IsDefaultModel(string id, ModelCapability cap)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            if (cap.HasFlag(ModelCapability.SpeechGeneration) && id == DefaultTTS) return true;
            if (cap.HasFlag(ModelCapability.VoiceChanger) && id == DefaultVCM) return true;

            return false;
        }

        public static bool IsDefaultVoice(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            if (id == DefaultVoice) return true;

            return false;
        }
    }
}