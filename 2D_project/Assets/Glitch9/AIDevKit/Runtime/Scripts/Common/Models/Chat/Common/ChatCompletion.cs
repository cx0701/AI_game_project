using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit
{
    public class ChatCompletion : ModelResponse, IChunk
    {
        /// <summary>
        /// A list of chat completion choices. Can be more than one if n is greater than 1.
        /// <para>
        /// If this is 'ChatCompletionChunk' which is a streamed response,
        /// This can also be empty for the last chunk if you set stream_options: {"include_usage": true}.
        /// </para>
        /// </summary>
        [JsonProperty("choices")] public ChatChoice[] Choices { get; set; }

        /// <summary>
        /// This fingerprint represents the backend configuration that the model runs with.
        /// Can be used in conjunction with the seed request parameter to understand when backend changes have been made that might impact determinism.
        /// </summary>
        [JsonProperty("system_fingerprint")] public string SystemFingerprint { get; set; }

        public override string ToString() => Choices.GetMessageText();
        public string ToTextDelta() => Choices.GetDeltaText();
        public ToolCall[] GetToolCalls() => Choices.GetToolCalls();
        public Content GetContent() => Choices.GetContent();
    }
}
