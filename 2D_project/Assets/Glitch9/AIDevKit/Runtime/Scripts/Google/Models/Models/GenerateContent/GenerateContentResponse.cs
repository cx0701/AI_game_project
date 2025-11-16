using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Individual response from {@link GenerativeModel.generateContent} and
    /// {@link GenerativeModel.generateContentStream}.
    /// `generateContentStream()` will return one in each chunk until
    /// the stream is done.
    /// </summary>
    /// <remarks>
    /// This is Google's equivalent of the ChatCompletion object in OpenAI.
    /// </remarks>
    public class GenerateContentResponse : IChunk
    {
        [JsonProperty("candidates")] public Candidate[] Candidates { get; set; }
        [JsonProperty("promptFeedback")] public PromptFeedback PromptFeedback { get; set; }
        [JsonProperty("usageMetadata")] public Usage Usage { get; set; }
        public override string ToString() => this.GetOutputText();
        public string ToTextDelta() => this.GetOutputText();

        [JsonIgnore]
        public List<ContentPart> Parts
        {
            get
            {
                if (_parts != null) return _parts;

                _parts = new List<ContentPart>();
                if (Candidates == null || Candidates.Length == 0) return _parts;

                foreach (Candidate candidate in Candidates)
                {
                    if (candidate.Content == null) continue;
                    foreach (ContentPart part in candidate.Content.Parts)
                    {
                        _parts.Add(part);
                    }
                }
                return _parts;
            }
        }

        private List<ContentPart> _parts;

        public async UniTask<GeneratedImage> ToGeneratedImageAsync(string outputPath)
        {
            if (Parts.IsNullOrEmpty()) throw new System.Exception("No images generated.");

            var textures = new List<Texture2D>();
            var paths = new List<string>();
            string savePath = outputPath.ToAbsolutePath();

            for (int i = 0; i < Parts.Count; i++)
            {
                var part = Parts[i];
                if (part == null || part.InlineData == null) continue;
                var texture = ImageDecoder.DecodeBase64(part.InlineData.Data);
                string finalPath = AIDevKitPath.AddIndexToPath(savePath, i);
                await texture.SaveTextureToFileAsync(finalPath);
                textures.Add(texture);
                paths.Add(finalPath);
            }

            return new GeneratedImage(textures.ToArray(), paths.ToArray());
        }

        public ChatDelta ToChatDelta()
        {
            Candidate firstCandidate = Candidates.FirstOrDefault();

            if (firstCandidate?.Content == null)
            {
                Debug.LogError("No candidates found in the response.");
                return null;
            }

            string content = string.Empty;

            if (Parts.IsNullOrEmpty()) return null;

            foreach (ContentPart part in Parts)
            {
                if (!string.IsNullOrEmpty(part.Text))
                {
                    content += part.Text;
                }
            }

            return new ChatDelta
            {
                Role = firstCandidate.Content.Role,
                Content = content,
                ToolCalls = GetToolCalls(),
            };
        }

        public ToolCall[] GetToolCalls()
        {
            if (Parts.IsNullOrEmpty()) return null;

            List<ToolCall> toolCalls = new();

            foreach (ContentPart part in Parts)
            {
                if (part.FunctionCall != null)
                {
                    toolCalls.Add(part.FunctionCall);
                }
            }

            return toolCalls.ToArray();
        }

        public AIDevKit.Content ToChatContent()
        {
            if (Parts.IsNullOrEmpty()) return null;

            // 멀티파트 구성
            List<AIDevKit.ContentPart> parts = new();

            foreach (ContentPart part in Parts)
            {
                if (!string.IsNullOrEmpty(part.Text))
                {
                    parts.Add(new TextContentPart(part.Text));
                }
                else if (part.FileData?.Uri != null)
                {
                    parts.Add(ImageContentPart.FromUrl(part.FileData.Uri));
                }
                else if (part.InlineData?.Data != null)
                {
                    var mime = part.InlineData.MimeType;

                    if (mime.IsImage())
                    {
                        parts.Add(ImageContentPart.FromBase64(part.InlineData.Data));
                    }
                    else if (mime.IsAudio())
                    {
                        parts.Add(AudioContentPart.FromBase64(part.InlineData.Data, mime));
                    }
                    else
                    {
                        parts.Add(FileContentPart.FromBase64(part.InlineData.Data, mime));
                    }
                }
                else if (part.FunctionCall != null)
                {
                    // 필요시 FunctionCall 처리
                }
                else if (part.FunctionResponse != null)
                {
                    // 필요시 FunctionResponse 처리
                }
            }

            return AIDevKit.Content.FromParts(parts);
        }

    }
}