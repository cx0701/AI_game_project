using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Controls for how a <see cref="Thread"/> will be truncated prior to the run. 
    /// Use this to control the intial context window of the run.
    /// </summary>
    public class TruncationStrategy
    {
        /// <summary>
        /// The truncation strategy to use for the <see cref="Thread"/>. 
        /// The default is auto. 
        /// If set to last_messages, the <see cref="Thread"/> will be truncated to the n most recent messages in the <see cref="Thread"/>. 
        /// When set to auto, messages in the middle of the <see cref="Thread"/> will be dropped to fit the context length of the model, max_prompt_tokens.
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; } = OpenAIConfig.AUTO_TYPE;

        /// <summary>
        /// The number of most recent messages from the <see cref="Thread"/> when constructing the context for the <see cref="Run"/>.
        /// </summary>
        [JsonProperty("last_message")] public int? LastMessage { get; set; }
    }
}