using System;

namespace Glitch9.AIDevKit
{
    public class ModelFilter : IEquatable<ModelFilter>, IAIDevKitAssetFilter<Model>
    {
        public AIProvider Api { get; set; }
        public ModelCapability? Capability { get; set; }

        public bool IsEmpty => (Api == AIProvider.All || Api == AIProvider.All) && Capability == null;

        public bool Matches(Model data)
        {
            if (Api != AIProvider.All && data.Api != Api) return false;
            if (Capability != null && !data.Capability.HasFlag(Capability)) return false;
            return true;
        }

        public static ModelFilter LLM(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.TextGeneration };
        public static ModelFilter IMG(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.ImageGeneration };
        public static ModelFilter TTS(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.SpeechGeneration };
        public static ModelFilter STT(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.SpeechRecognition };
        public static ModelFilter EMB(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.TextEmbedding };
        public static ModelFilter MOD(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.Moderation };
        public static ModelFilter SFX(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.SoundFXGeneration };
        public static ModelFilter RTM(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.Realtime };
        public static ModelFilter VCM(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.VoiceChanger };
        public static ModelFilter VID(AIProvider api = AIProvider.All) => new() { Api = api, Capability = ModelCapability.VideoGeneration };


        // Dictionary key를 위한 동등성 비교 구현
        public override bool Equals(object obj) => Equals(obj as ModelFilter);

        public bool Equals(ModelFilter other)
        {
            if (other == null) return false;
            return Api == other.Api && Capability == other.Capability;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Api.GetHashCodeOrDefault();
                hash = hash * 23 + Capability.GetHashCodeOrDefault();
                return hash;
            }
        }
    }
}