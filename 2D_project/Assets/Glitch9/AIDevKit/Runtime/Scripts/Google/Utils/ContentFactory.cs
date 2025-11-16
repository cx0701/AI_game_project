using System;
using System.Collections.Generic;
using System.Linq;
using Glitch9.IO.Files;

namespace Glitch9.AIDevKit.Google
{
    internal class ContentFactory
    {
        internal static Content CreateSystemInstruction(string @params)
        {
            ContentPart[] parts = new[] { new ContentPart() { Text = @params } };
            return new Content(ChatRole.System, parts);
        }

        public static Content CreateUserContent(string prompt, IEnumerable<UniImageFile> imageFiles = null)
        {
            bool invalidPrompt = string.IsNullOrEmpty(prompt);

            List<ContentPart> parts = new();

            if (!invalidPrompt)
            {
                ContentPart textPart = new()
                {
                    Text = prompt
                };
                parts.Add(textPart);
            }

            if (imageFiles != null)
            {
                foreach (UniImageFile imageFile in imageFiles)
                {
                    ContentPart imagePart = new()
                    {
                        InlineData = new Blob()
                        {
                            MimeType = MIMETypeUtil.ParseFromPath(imageFile.Name),
                            Data = Convert.ToBase64String(imageFile.ToBinaryData())
                        }
                    };
                    parts.Add(imagePart);
                }
            }

            return new Content()
            {
                Parts = parts.ToArray(),
                Role = ChatRole.User
            };
        }

        public static Content CreateUserContent(IEnumerable<ContentPart> parts)
        {
            return new Content()
            {
                Parts = parts.ToArray(),
                Role = ChatRole.User
            };
        }
    }
}
