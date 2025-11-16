using System.Collections.Generic;
using Glitch9.IO.Files;

namespace Glitch9.AIDevKit
{
    internal static class GENTaskRecordUtil
    {
        internal static List<ISerializedContent> ConvertToSerializedContent(string content, List<IUniFile> attachedFiles = null)
        {
            if (content == null) return null;
            List<ISerializedContent> contents = new();

            if (!string.IsNullOrWhiteSpace(content))
            {
                contents.Add(new SerializedTextContent(content));
            }

            if (!attachedFiles.IsNullOrEmpty())
            {
                foreach (var f in attachedFiles)
                {
                    if (f is UniFile file)
                    {
                        contents.Add(new SerializedFileContent(file));
                    }
                    else if (f is UniImageFile image)
                    {
                        contents.Add(new SerializedImageContent(image));
                    }
                    else if (f is UniAudioFile audio)
                    {
                        contents.Add(new SerializedAudioContent(audio));
                    }
                    else if (f is UniVideoFile video)
                    {
                        contents.Add(new SerializedVideoContent(video));
                    }
                }
            }

            return contents;
        }

        internal static List<ISerializedContent> ConvertToSerializedContent(string[] textContents)
        {
            if (textContents.IsNullOrEmpty()) return null;
            List<ISerializedContent> contents = new();

            foreach (var text in textContents)
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    contents.Add(new SerializedTextContent(text));
                }
            }

            return contents;
        }

        internal static List<ISerializedContent> ConvertToSerializedContent(Content content, List<IUniFile> attachedFiles = null)
        {
            if (content == null) return null;
            List<ISerializedContent> contents = new();

            if (content.IsString)
            {
                contents.Add(new SerializedTextContent(content));
            }
            else
            {
                foreach (ContentPart part in content.ToPartArray())
                {
                    if (part is TextContentPart text)
                    {
                        contents.Add(new SerializedTextContent(text.ToString()));
                    }
                }
            }

            if (!attachedFiles.IsNullOrEmpty())
            {
                foreach (var f in attachedFiles)
                {
                    if (f is UniFile file)
                    {
                        contents.Add(new SerializedFileContent(file));
                    }
                    else if (f is UniImageFile image)
                    {
                        contents.Add(new SerializedImageContent(image));
                    }
                    else if (f is UniAudioFile audio)
                    {
                        contents.Add(new SerializedAudioContent(audio));
                    }
                    else if (f is UniVideoFile video)
                    {
                        contents.Add(new SerializedVideoContent(video));
                    }
                }
            }

            return contents;
        }
    }
}