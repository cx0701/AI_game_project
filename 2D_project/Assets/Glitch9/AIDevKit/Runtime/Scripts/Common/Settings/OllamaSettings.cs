using Cysharp.Threading.Tasks;
using Glitch9.IO.Networking;
using Glitch9.ScriptableObjects;
using UnityEngine;

namespace Glitch9.AIDevKit.Ollama
{
    [CreateAssetMenu(fileName = nameof(OllamaSettings), menuName = AIDevKitConfig.kOllamaSettings, order = AIDevKitConfig.kOllamaSettingsOrder)]
    public class OllamaSettings : ScriptableResource<OllamaSettings>
    {
        // general settings
        [SerializeField] private string endpoint = "localhost";
        [SerializeField] private int port = 11434;

        // default models
        [SerializeField] private string defaultModel;

        public static string DefaultModel => Instance.defaultModel;

        public static string GetEndpoint()
        {
            string endpoint = Instance.endpoint;
            if (string.IsNullOrEmpty(endpoint)) endpoint = "localhost";
            int port = Instance.port;
            if (port <= 0) port = 11434;

            return $"http://{endpoint}:{port}";
        }

        public static UniTask<bool> CheckConnectionAsync()
        {
            string endpoint = GetEndpoint();
            string url = $"{endpoint}/api/tags"; // 가장 가벼운 API
            return NetworkUtils.CheckUrlAsync(url);
        }

        public static bool IsDefaultModel(string id, ModelCapability cap)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            if (cap.HasFlag(ModelCapability.TextGeneration) && id == DefaultModel) return true;

            return false;
        }
    }
}