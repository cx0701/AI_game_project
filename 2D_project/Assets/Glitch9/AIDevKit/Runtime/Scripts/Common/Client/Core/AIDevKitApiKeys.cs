using Glitch9.ScriptableObjects;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    //[CreateAssetMenu(fileName = nameof(AIDevKitSettings), menuName = AIDevKitConfig.kAIDevKitApiKeys, order = AIDevKitConfig.kAIDevKitSettingsOrder)]
    public class AIDevKitApiKeys : ScriptableResource<AIDevKitApiKeys>
    {
        [SerializeField] private ApiKey openAIKey;
        [SerializeField] private ApiKey googleKey;
        [SerializeField] private ApiKey elevenLabsKey;
        [SerializeField] private ApiKey openRouterKey;
        [SerializeField] private ApiKey mubertKey;
    }
}