using Newtonsoft.Json;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Given some input Text, outputs if the model classifies it as potentially harmful across several categories.
    /// Related guide: https://platform.openai.com/docs/guides/moderation
    /// POST https://api.openai.com/v1/moderations
    /// </summary>
    public class Moderation : ModelResponse
    {
        /// <summary>
        /// A list of moderation objects.
        /// </summary>
        [JsonProperty("results")] public List<ModerationResult> Results { get; set; }


        public override string ToString()
        {
            if (Results.IsNullOrEmpty()) return "No moderation results";
            return string.Join("\n", Results);
        }
    }
}