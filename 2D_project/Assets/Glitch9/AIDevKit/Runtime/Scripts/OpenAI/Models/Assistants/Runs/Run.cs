using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Represents an execution run on a <see cref="Thread"/>.
    /// <para>Renamed from RunObject to Run (2024.06.14)</para>
    /// </summary>
    public class Run : RunBase
    {
        /// <summary>
        /// Details on the action required to continue the run.
        /// Will be null if no action is required.
        /// </summary>
        [JsonProperty("required_action")] public ActionRequest ActionRequest { get; set; }

        /// <summary>
        /// The instructions that the AssistantObject used for this run.
        /// </summary>
        [JsonProperty("instructions")] public string Instructions { get; set; }

        /// <summary>
        /// The list of tools that the AssistantObject used for this run.
        /// </summary>
        [JsonProperty("tools")] public ToolCall[] Tools { get; set; }

        /// <summary> 
        /// The sampling temperature used for this run. If not set, defaults to 1.
        /// </summary>
        [JsonProperty("temperature")] public float? Temperature { get; set; }

        /// <summary>
        /// The nucleus sampling value used for this run. If not set, defaults to 1.
        /// </summary>
        [JsonProperty("top_p")] public float? TopP { get; set; }

        /// <summary>
        /// The maximum number of prompt tokens specified to have been used over the course of the run.
        /// </summary>
        [JsonProperty("max_prompt_tokens")] public int? MaxPromptTokens { get; set; }

        /// <summary>
        /// The maximum number of completion tokens specified to have been used over the course of the run.
        /// </summary>
        [JsonProperty("max_completion_tokens")] public int? MaxCompletionTokens { get; set; }

        /// <summary>
        /// Details on why the run is incomplete.
        /// Will be null if the run is not incomplete.
        /// </summary>
        [JsonProperty("incomplete_details")] public IncompleteDetails IncompleteDetails { get; set; }

        /// <summary>
        /// Controls for how a thread will be truncated prior to the run. Use this to control the initial context window of the run.
        /// </summary>
        [JsonProperty("truncation_strategy")] public TruncationStrategy TruncationStrategy { get; set; }

        /// <summary>
        /// Controls which (if any) tool is called by the model.
        /// - none: the model will not call any tools and instead generates a message.
        /// - auto: the model can pick between generating a message or calling one or more tools.
        /// - required: the model must call one or more tools before responding to the user.
        /// Specifying a particular tool like {"type": "file_search"} or {"type": "function", "function": {"name": "my_function"}} forces the model to call that tool.
        /// </summary>
        [JsonProperty("tool_choice")] public StringOr<ToolChoice> ToolChoice { get; set; }

        /// <summary>
        /// Specifies the format that the model must output. Compatible with GPT-4o, GPT-4 Turbo, and all GPT-3.5 Turbo models since gpt-3.5-turbo-1106.
        /// Setting to { "type": "json_object" } enables JSON mode, which guarantees the message the model generates is valid JSON.
        /// </summary>
        [JsonProperty("response_format")] public ResponseFormat ResponseFormat { get; set; }
    }

    /// <summary>
    /// Details on why the run is incomplete. 
    /// Will be null if the run is not incomplete.
    /// </summary>
    public class IncompleteDetails
    {
        /// <summary>
        /// The reason why the run is incomplete. 
        /// This will point to which specific token limit was reached over the course of the run.
        /// </summary>
        [JsonProperty("reason")] public string Reason { get; set; }
    }
}
