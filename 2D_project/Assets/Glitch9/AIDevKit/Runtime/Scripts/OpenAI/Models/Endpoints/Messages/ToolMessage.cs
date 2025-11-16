using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public class ToolMessage : ThreadMessage
    {
        /// <summary>
        /// [Required] Tool call that this message is responding to.
        /// </summary>
        [JsonProperty("tool_call_id")] public string ToolCallId { get; set; }

        public ToolMessage(string toolCallId, string content)
        {
            Role = ChatRole.Tool;
            ToolCallId = toolCallId;
            Content = content;
        }
    }
}