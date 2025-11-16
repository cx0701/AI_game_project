using Glitch9.IO.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Response object wrapped with helper methods.
    /// </summary>
    public static class ResponseExtensions
    {
        public static string GetOutputText(this GenerateContentResponse res)
        {
            return res?.Candidates?[0].Content?.Parts?[0].Text;
        }

        public static void SetOutputText(this GenerateContentResponse res, string text)
        {
            if (res == null) throw new ArgumentNullException(nameof(res));
            res.Candidates = new Candidate[] { new() { Content = new Content() { Parts = new ContentPart[] { new() } } } };
            res.Candidates[0].Content.Parts[0].Text = text;
        }

        public static FunctionCall GetFunction(this GenerateContentResponse res)
        {
            return res?.Candidates?[0].Content?.Parts?[0].FunctionCall;
        }

        /// <summary>
        /// Returns the input text from the first message in the request.
        /// </summary>
        public static string GetInputText(this GenerateContentRequest req)
        {
            return req?.Contents?[0].Parts?[0].Text;
        }

        public static (string text, List<UniImageFile> images) GetInputTextAndImages(this GenerateContentRequest req)
        {
            try
            {
                List<Content> input = req.Contents;
                string text = input.GetOutputText();
                List<UniImageFile> images = input.GetOutputImages();
                return (text, images);
            }
            catch (Exception e)
            {
                LogService.Exception(e);
                return (string.Empty, null);
            }
        }

        /// <summary>
        /// Extracts the text value from the given array of MessageContent.
        /// </summary>
        /// <param name="contents">Array of MessageContent</param>
        /// <returns>Extracted text value as a string</returns>
        public static string GetOutputText(this List<Content> contents)
        {
            if (contents.IsNullOrEmpty()) return null;

            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                foreach (Content content in contents)
                {
                    if (content == null || content.Parts.IsNullOrEmpty()) continue;
                    foreach (ContentPart part in content.Parts)
                    {
                        if (part == null || string.IsNullOrEmpty(part.Text)) continue;
                        sb.Append(part.Text);
                    }
                }

                return sb.ToString();
            }
        }

        /// <summary>
        /// Extracts all image files from the given array of MessageContent.
        /// </summary>
        /// <param name="contents">Array of MessageContent</param>
        /// <returns>List of extracted UnityImageFile objects</returns>
        public static List<UniImageFile> GetOutputImages(this List<Content> contents)
        {
            if (contents.IsNullOrEmpty()) return null;

            List<UniImageFile> images = new();
            foreach (Content content in contents)
            {
                if (content == null || content.Parts.IsNullOrEmpty()) continue;
                foreach (ContentPart part in content.Parts)
                {
                    if (part == null || !part.IsImage) continue;
                    images.Add(part.ImageContent);
                }
            }

            return images;
        }


        public static GenerateContentRequest AddParts(this GenerateContentRequest req, ChatRole role, params ContentPart[] parts)
        {
            if (role == ChatRole.Unset)
            {
                LogService.Error("Role is not set.");
                return req;
            }

            Content content = new()
            {
                Role = role,
                Parts = parts
            };

            return req.AddContent(content);
        }

        public static GenerateContentRequest AddParts(this GenerateContentRequest req, ChatRole role, IList<ContentPart> parts)
        {
            if (role == ChatRole.Unset)
            {
                LogService.Error("Role is not set.");
                return req;
            }

            Content content = new()
            {
                Role = role,
                Parts = parts.ToArray()
            };

            return req.AddContent(content);
        }

        public static GenerateContentRequest AddContent(this GenerateContentRequest req, params Content[] contents)
        {
            req?.Contents.AddRange(contents);
            return req;
        }

        public static List<ISerializedContent> ToContentRecords(this Content content, string outputPath)
        {
            if (content == null || content.Parts.IsNullOrEmpty()) return null;

            List<ISerializedContent> resources = new();

            for (int i = 0; i < content.Parts.Length; i++)
            {
                ContentPart part = content.Parts[i];

                if (part == null)
                {
                    Debug.LogWarning($"Part {i} is null.");
                    continue;
                }

                //Debug.Log($"Part {i}: memeType{part.InlineData?.MimeType} text: {part.Text}");

                if (part.IsImage)
                {
                    //Debug.Log($"Image {i}: {part.ImageContent}");

                    string finalPath = AIDevKitPath.AddIndexToPath(outputPath.ToAbsolutePath(), i);
                    resources.Add(new SerializedImageContent(new UniImageFile(finalPath)));

                    //Debug.LogWarning($"Image {i} saved to {finalPath}");
                }
                else if (!string.IsNullOrEmpty(part.Text))
                {
                    resources.Add(new SerializedTextContent(part.Text));
                }
            }

            return resources;
        }
    }
}
