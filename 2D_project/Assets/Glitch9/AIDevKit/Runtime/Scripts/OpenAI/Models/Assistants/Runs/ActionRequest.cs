using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Details on the action required to continue the <see cref="Run"/>.
    /// <para>Will be null if no action is required.</para>
    /// </summary>
    public class ActionRequest
    {
        /// <summary>
        /// For now, this is always <see cref="ActionRequestType.SubmitToolOutputs"/>.
        /// </summary>
        [JsonProperty("type")] public ActionRequestType Type { get; set; }

        /// <summary>
        /// Details on the tool outputs needed for this run to continue.
        /// </summary>
        [JsonProperty("submit_tool_outputs")] public ToolOutputsSubmission SubmitToolOutputs { get; set; }
    }

    /// <summary>
    /// When a run has the status: <see cref="RunStatus.RequiresAction"/> and <see cref="ActionRequest.Type"/> is <see cref="ActionRequestType.SubmitToolOutputs"/>,
    /// this endpoint can be used to submit the outputs from the tool calls once they're all completed.
    /// <para>All outputs must be submitted in a single request.</para>
    /// </summary>
    /// <remarks>
    /// <see href="https://platform.openai.com/docs/api-reference/runs/submitToolOutputs">Submit tool outputs to run</see>
    /// </remarks>
    public class ToolOutputsSubmission
    {
        /// <summary>
        /// A list of tools for which the outputs are being submitted.
        /// </summary>
        [JsonProperty("tool_calls")] public ToolCall[] ToolCalls { get; set; }

        /// <summary>
        /// If true, returns a stream of events that happen during the <see cref="Run"/> as server-sent events,
        /// terminating when the <see cref="Run"/> enters a terminal state with a data: [DONE] message.
        /// </summary>
        [JsonProperty("stream")] public bool? Stream { get; set; }
    }

    public enum ActionRequestType
    {
        [ApiEnum("submit_tool_outputs")]
        SubmitToolOutputs,
    }
}