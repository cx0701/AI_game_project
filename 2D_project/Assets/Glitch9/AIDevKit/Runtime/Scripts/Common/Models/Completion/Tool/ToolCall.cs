using System;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Glitch9.AIDevKit
{
    public enum ToolType
    {
        [ApiEnum("none")] None,
        [ApiEnum("auto")] Auto,
        [ApiEnum("function")] Function,
        [ApiEnum("tool_calls")] ToolCalls,
        [ApiEnum("code_interpreter")] CodeInterpreter,
        [ApiEnum("file_search")] FileSearch,
    }

    public abstract class ToolCall
    {
        /// <summary>
        /// Required. The unique identifier of the tool.
        /// </summary>
        [JsonProperty("id")] public string Id { get; set; }

        /// <summary>
        /// Required. The type of the tool.
        /// </summary>
        [JsonProperty("type")] public ToolType Type { get; set; }
    }
}