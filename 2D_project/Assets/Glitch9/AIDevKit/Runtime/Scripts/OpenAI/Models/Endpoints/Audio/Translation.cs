using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public class Translation : ModelResponse
    {
        /// <summary>
        /// Translated text
        /// </summary>
        [JsonProperty("text")] public string Text { get; set; }
    }
}