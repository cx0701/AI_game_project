using System.Collections.Generic;

namespace Glitch9.AIDevKit.Editor
{
    internal class VoicePopupGUI : AssetPopupGUI<Voice, VoiceFilter>
    {
        protected override Dictionary<AIProvider, List<Voice>> GetFilteredAssets(VoiceFilter filter) => VoiceLibrary.GetFilteredRefs(filter);
        protected override Voice GetDefaultAssetId(VoiceFilter filter) => GetDefaultVoiceId(filter.Api);
        private static string GetDefaultVoiceId(AIProvider api)
        {
            return api switch
            {
                AIProvider.OpenAI => AIDevKitConfig.kDefault_OpenAI_Voice,
                AIProvider.ElevenLabs => AIDevKitConfig.kDefault_ElevenLabs_Voice,
                _ => AIDevKitConfig.kDefault_OpenAI_Voice,
            };
        }
    }
}