using Glitch9.IO.Files;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    public class FileResponse
    {
        [JsonProperty("file")] public File File { get; set; }
    }

    /// <summary>
    /// URI based data.
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// Required. URI.
        /// </summary>
        [JsonProperty("fileUri")] public string Uri { get; set; }

        /// <summary>
        /// Optional.
        /// The IANA standard MIME type of the source data.
        /// </summary>
        [JsonProperty("mimeType")] public MIMEType? MimeType { get; set; }

        public FileData() { }
        public FileData(string fileUri) => Uri = fileUri;
    }
}