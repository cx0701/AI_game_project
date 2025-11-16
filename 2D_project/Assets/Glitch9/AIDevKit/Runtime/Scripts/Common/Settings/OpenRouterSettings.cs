using UnityEngine;

namespace Glitch9.AIDevKit.OpenRouter
{
    [CreateAssetMenu(fileName = nameof(OpenRouterSettings), menuName = AIDevKitConfig.kOpenRouterSettings, order = AIDevKitConfig.kOpenRouterSettingsOrder)]
    public class OpenRouterSettings : AIClientSettings<OpenRouterSettings>
    {
        // general settings
        [SerializeField] private string httpReferer = string.Empty;
        [SerializeField] private string xTitle = string.Empty;

        // default models
        [SerializeField] private string defaultLLM;

        public static string HttpReferer => Instance.httpReferer;
        public static string XTitle => Instance.xTitle;
        public static string DefaultLLM => Instance.defaultLLM;

        public static bool IsDefaultModel(string id, ModelCapability cap)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            if (cap.HasFlag(ModelCapability.TextGeneration) && id == DefaultLLM) return true;

            return false;
        }
    }
}