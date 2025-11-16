using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public class FileRequest : ModelRequest
    {
        [JsonProperty("file_id")] string FileId { get; set; }

        public FileRequest(string fileId)
        {
            FileId = fileId;
        }
    }
}