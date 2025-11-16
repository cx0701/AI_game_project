
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public enum UploadPurpose
    {
        Unknown,

        [ApiEnum("assistants")]
        Assistants,

        [ApiEnum("assistants_output")]
        AssistantsOutput,

        [ApiEnum("fine-tune")]
        FineTune,

        [ApiEnum("fine-tune-results")]
        FineTuneResults,

        [ApiEnum("vision")]
        Vision,

        [ApiEnum("batch")]
        BatchAPI,
    }

    /// <summary>
    /// The File object represents a document that has been uploaded to OpenAI.
    /// </summary>
    public class File : ModelResponse
    {
        /// <summary>
        /// The size of the file, in bytes
        /// </summary>
        [JsonProperty("bytes")] public int Bytes { get; set; }

        /// <summary>
        /// The name of the file
        /// </summary>
        [JsonProperty("filename")] public string Filename { get; set; }

        /// <summary>
        /// The intended purpose of the file.
        /// Supported values are fine-tune, fine-tune-results, Assistants, and Assistants_Output
        /// </summary>
        [JsonProperty("purpose")] public UploadPurpose? Purpose { get; set; }
    }
}