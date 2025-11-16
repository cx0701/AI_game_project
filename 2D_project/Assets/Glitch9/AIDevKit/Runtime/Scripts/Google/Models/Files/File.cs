using System;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Google
{
    public class UploadFileRequest : GenerativeAIRequest
    {
        [JsonProperty("file")] public File File { get; set; }
    }

    /// <summary>
    /// A file uploaded to the API.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Immutable. Identifier. 
        /// <para>
        /// The <see cref="File"/> resource name. The ID (name excluding the "files/" prefix) can contain up to 40 characters 
        /// that are lowercase alphanumeric or dashes (-). 
        /// </para>
        /// <para>
        /// The ID cannot start or end with a dash. 
        /// If the name is empty on create, a unique name will be generated. 
        /// </para>
        /// <para>Example: files/123-456</para>
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }

        /// <summary>
        /// Optional. 
        /// <para>
        /// The human-readable display name for the <see cref="File"/>. 
        /// The display name must be no more than 512 characters in length, 
        /// including spaces. Example: "Welcome Image"
        /// </para>
        /// </summary>
        [JsonProperty("displayName")] public string DisplayName { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>MIME type of the <see cref="File"/>.</para>
        /// </summary>
        [JsonProperty("mimeType")] public string MimeType { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>Size of the <see cref="File"/> in bytes.</para>
        /// </summary>
        [JsonProperty("sizeBytes")] public string SizeBytes { get; set; }

        /// <summary>
        /// Output only.
        /// <para>The timestamp of when the <see cref="File"/> was created.</para>
        /// </summary>
        [JsonProperty("createTime")] public ZuluTime CreateTime { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>The timestamp of when the <see cref="File"/> was last updated.</para>
        /// </summary>
        [JsonProperty("updateTime")] public string UpdateTime { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>
        /// The timestamp of when the <see cref="File"/> will be deleted. 
        /// Only set if the <see cref="File"/> is scheduled to expire.
        /// </para>
        /// </summary>
        [JsonProperty("expirationTime")] public string ExpirationTime { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>SHA-256 hash of the uploaded bytes.</para>
        /// </summary>
        /// <remarks>A base64-encoded string.</remarks>
        [JsonProperty("sha256Hash")] public string Sha256Hash { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>The uri of the <see cref="File"/>.</para>
        /// </summary>
        [JsonProperty("uri")] public string Uri { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>Processing state of the <see cref="File"/>.</para>
        /// </summary>
        [JsonProperty("state")] public FileState State { get; set; }

        /// <summary>
        /// Output only. 
        /// <para>Error status if <see cref="File"/> processing failed.</para>
        /// </summary>
        [JsonProperty("error")] public Status Error { get; set; }

        /// <summary>
        /// Output only.
        /// <para>Metadata for a video.</para>
        /// </summary>
        [JsonProperty("videoMetadata")] public VideoMetadata VideoMetadata { get; set; }

        public void Delete()
        {
            // Implement delete logic
        }
    }
    public class FileDataDict
    {
        [JsonProperty("mime_type")]
        public string MimeType { get; set; }

        [JsonProperty("file_uri")]
        public string FileUri { get; set; }
    }

    public class FileDataType
    {
        public static FileDataDict ToFileData(object fileData)
        {
            if (fileData is Dictionary<string, object> dict)
            {
                if (dict.TryGetValue("file_uri", out object value))
                {
                    return new FileDataDict
                    {
                        MimeType = dict["mime_type"].ToString(),
                        FileUri = value.ToString()
                    };
                }
            }

            throw new ArgumentException("Invalid input type. Failed to convert input to `FileData`.");
        }
    }

    /// <summary>
    /// States for the lifecycle of a File.
    /// </summary>
    public enum FileState
    {
        /// <summary>
        /// The default value. This value is used if the state is omitted.
        /// </summary>
        [ApiEnum("STATE_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// File is being processed and cannot be used for inference yet.
        /// </summary>
        [ApiEnum("PROCESSING")] Processing,

        /// <summary>
        /// File is processed and available for inference.
        /// </summary>
        [ApiEnum("ACTIVE")] Active,

        /// <summary>
        /// File failed processing.
        /// </summary>
        [ApiEnum("FAILED")] Failed
    }

    /// <summary>
    /// The Status type defines a logical error model that is suitable for different programming environments, 
    /// including REST APIs and RPC APIs. It is used by gRPC. 
    /// Each Status message contains three pieces of data: error code, error message, and error details.
    /// </summary>
    /// <remarks>
    /// You can find out more about this error model and how to work with it in the <see href="https://cloud.google.com/apis/design/errors"> API Design Guide</see>.
    /// </remarks>
    public class Status
    {
        /* Json Representation
        {
            "code": integer,
            "message": string,
            "details": [
                {
                "@type": string,
                field1: ...,
                ...
                }
            ]
        }
        */

        /// <summary>
        /// The status code, which should be an enum value of google.rpc.Code.
        /// </summary>
        [JsonProperty("code")] public int Code { get; set; }

        /// <summary>
        /// A developer-facing error message, which should be in English. 
        /// Any user-facing error message should be localized and sent in the 
        /// <see href="https://ai.google.dev/api/rest/Shared.Types/Operation#Status.FIELDS.details"> google.rpc.Status.details</see> field, 
        /// or localized by the client.
        /// </summary>
        [JsonProperty("message")] public string Message { get; set; }

        /// <summary>
        /// A list of messages that carry the error details. There is a common set of message types for APIs to use.
        /// An object containing fields of an arbitrary type. An additional field "@type" contains a URI identifying the type. 
        /// <para>Example: { "id": 1234, "@type": "types.example.com/standard/id" }.</para>
        /// </summary>
        [JsonProperty("details")] public Dictionary<string, string> Details { get; set; }
    }

    /// <summary>
    /// Metadata for a video File.
    /// </summary>
    public class VideoMetadata
    {
        /* Json Representation
        {
            "videoDuration": string
        }
        */

        /// <summary>
        /// Duration of the video.
        /// </summary>
        /// <remarks>
        /// A duration in seconds with up to nine fractional digits, ending with 's'. Example: "3.5s".
        /// </remarks>
        [JsonProperty("videoDuration")] public string VideoDuration { get; set; }
    }
}