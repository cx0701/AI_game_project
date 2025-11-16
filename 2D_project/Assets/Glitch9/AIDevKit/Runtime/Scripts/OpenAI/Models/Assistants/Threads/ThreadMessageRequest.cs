using Glitch9.IO.Files;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Glitch9.AIDevKit.OpenAI
{
    public class ThreadMessageRequest : ModelRequest
    {
        /// <summary>
        /// Required. The role of the entity that is creating the message. 
        /// Currently only User is supported.
        /// </summary>
        [JsonProperty("role")] public ChatRole Role => ChatRole.User;

        /// <summary>
        /// Required. The content of the message.
        /// </summary>
        [JsonProperty("content")] public Content Content { get; set; }

        /// <summary>
        /// A list of files attached to the message, and the tools they should be added to.
        /// </summary>
        [JsonProperty("attachments")] public List<Attachment> Attachments { get; set; }

        /// <summary>
        /// Locally stored images for the message.
        /// </summary>
        [JsonIgnore] public List<UniImageFile> LocalImages { get; set; }

        /// <summary>
        /// Locally stored files for the message.
        /// </summary>
        [JsonIgnore] public List<UniFile> LocalFiles { get; set; }


        public void SetImages(IEnumerable<string> imageFileIds)
        {
            Content ??= new();
            Content.AddPartRange(imageFileIds.Select(imageFileId => ImageContentPart.FromId(imageFileId)));
        }

        public void SetFiles(IEnumerable<string> attachmentFileIds)
        {
            Attachments ??= new();
            Attachments.AddRange(attachmentFileIds.Select(attachmentFileId => new Attachment(attachmentFileId)));
        }

        public class Builder : ModelRequestBuilder<Builder, ThreadMessageRequest>
        {
            public Builder SetPrompt(string prompt)
            {
                _req.Content ??= new();
                _req.Content.AddPart(new TextContentPart(prompt));
                return this;
            }

            public Builder SetImageUrls(params string[] imageUrls)
            {
                _req.Content ??= new();
                _req.Content.AddPartRange(imageUrls.Select((imageUrl) => ImageContentPart.FromUrl(imageUrl)));
                return this;
            }

            public Builder SetFileIds(params string[] fileIds)
            {
                _req.Attachments ??= new();
                _req.Attachments.AddRange(fileIds.Select(fileId => new Attachment(fileId)));
                return this;
            }

            public Builder SetTools(params ToolCall[] tools)
            {
                _req.Attachments ??= new();
                _req.Attachments.Add(tools);
                return this;
            }

            public Builder SetImages(params UniImageFile[] imageFiles)
            {
                foreach (UniImageFile imageFile in imageFiles)
                {
                    imageFile.MimeType = IO.Files.MIMEType.Jsonl;
                }
                _req.LocalImages = imageFiles.ToList();
                return this;
            }

            public Builder SetFiles(params UniFile[] files)
            {
                _req.LocalFiles = files.ToList();
                return this;
            }
        }
    }
}
