using Glitch9.IO.Files;
using Glitch9.IO.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    // OpenAI and Gemini can have more than one 'content', how am I going to handle that?
    // The bottom line is that one message can have all the following:
    //
    // 1. Sending & Receiving Texts (text)
    //  - You can send a chat message as a string (text).
    //  - You can receive a chat message as a string (text).
    //
    // 2. Sending Images
    //  - You can send an image as a base64-encoded string (OpenAI and Gemini both support this).
    //  - You can send an image as a URL (OpenAI only; Gemini requires inline_data).
    //
    // 3. Receiving Images
    //  - You can receive an image as a base64-encoded string.
    //  - You can receive an image as a URL.
    //  - You can receive an image as a file ID in the following conditions:
    //    (a) When using the Assistants API's code_interpreter tool to generate a file
    //    (b) When using the Assistants API's file_search tool to locate a document
    //    (c) When using the Assistants API's retrieval tool to reference an uploaded file
    //  (Note: This does not mean the model *generates* new images like Stable Diffusion; images are provided as part of tool outputs.)
    //
    // 4. Sending & Receiving Files (Only supported via the OpenAI Assistants API)
    //  - You can send a file as a file ID.
    //  - You can receive a file as a file ID. 

    // Can there be more than one Text inside Content?
    //
    // Answer
    //  Yes, there can be multiple Text parts inside Content. (Especially when using vision models)
    //
    // Why?
    //  According to the official OpenAI spec (gpt-4o, gpt-4-vision-preview),
    //  Content can be sent as an array, mixing text parts and image parts.
    //
    // It's completely allowed to have multiple "text" types inside the array.
    //
    // Example
    // This is a completely valid request:
    // {
    //     "role": "user",
    //     "content": [
    //         { "type": "text", "text": "First question." },
    //         { "type": "image", "image_url": { "url": "https://example.com/image.png" } },
    //         { "type": "text", "text": "Second question. Please further analyze the image." }
    //     ]
    // }
    //
    // In this case:
    // - Text → Image → Text sequence is composed inside Content array.
    // - There are two separate "text" parts.
    //
    // This is perfectly valid.
    //
    // Possibility summary:
    // - Can there be multiple Text parts in Content? Yes
    // - Can there be multiple Image parts in Content? Yes
    // - Can Text and Image parts be mixed? Yes
    // - Is the order preserved? Yes (processed in array order)
    //
    // Note:
    // - This is only possible when Content is an array.
    // - If you send Content as a plain string, obviously only one Text will exist.
    // - So when designing your model, **always assume Content will be a list**.
    // - (Most vision and multimodal models already enforce Content as a list.)
    //


    [JsonObject]
    public class ChatMessage
    {
        public static implicit operator string(ChatMessage message) => new(message.ToString());

        /// <summary>
        /// The role of the messages author.
        /// </summary>
        [JsonProperty("role")] public ChatRole Role { get; set; }

        /// <summary>
        /// The contents of the user message.
        /// </summary>
        [JsonProperty("content")] public Content Content { get; set; }

        /// <summary> 
        /// Optional. An optional name for the participant.
        /// Provides the model information to differentiate between participants of the same role.
        /// </summary>
        [JsonProperty("name")] public string Name { get; set; }

        /// <summary>
        /// Optional. A list of tool calls (functions) the model wants to use.
        /// </summary>
        [JsonProperty("tool_calls")] public ToolCall[] Tools { get; set; }

        // --- Local variables ------------------------------------------------------------ 
        [JsonIgnore] public List<IUniFile> AttachedFiles { get; set; }
        [JsonIgnore] public List<ModerationData> Moderations { get; set; }
        [JsonIgnore] public UnixTime CreatedAt { get; private set; } = UnixTime.Now;
        [JsonIgnore] public Usage Usage { get; set; }
        [JsonIgnore] public bool IsHidden { get; set; } = false;

        public override string ToString() => Content?.ToString() ?? string.Empty;

        [JsonConstructor] public ChatMessage() { }
        public ChatMessage(ChatRole role, string content)
        {
            Role = role;
            Content = content;
        }

        public void AddAttachment<T>(T file) where T : IUniFile
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            AttachedFiles ??= new();
            AttachedFiles.Add(file);
        }

        public void AddAttachments<T>(IList<T> files) where T : IUniFile
        {
            if (files == null) throw new ArgumentNullException(nameof(files));
            AttachedFiles ??= new();
            foreach (T file in files)
            {
                if (file == null) continue;
                AttachedFiles.Add(file);
            }
        }

        public UniImageFile GetFirstImageFile()
        {
            if (AttachedFiles == null) return null;
            foreach (IUniFile file in AttachedFiles)
            {
                if (file is UniImageFile imgFile) return imgFile;
            }
            return null;
        }

        public UniAudioFile GetFirstAudioFile()
        {
            if (AttachedFiles == null) return null;
            foreach (IUniFile file in AttachedFiles)
            {
                if (file is UniAudioFile audioFile) return audioFile;
            }
            return null;
        }

        public UniFile GetFirstFile()
        {
            if (AttachedFiles == null) return null;
            foreach (IUniFile file in AttachedFiles)
            {
                if (file is UniFile uniFile) return uniFile;
            }
            return null;
        }
    }

    internal class ChatMessageConverter : JsonConverter<ChatMessage>
    {
        private readonly AIProvider _provider;
        internal ChatMessageConverter(AIProvider provider) => _provider = provider;

        public override ChatMessage ReadJson(JsonReader reader, Type objectType, ChatMessage existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            if (_provider == AIProvider.None)
            {
                return new ChatMessage
                {
                    Role = obj["role"]?.ToObject<ChatRole>(serializer) ?? ChatRole.User,
                    Content = obj["content"]?.ToObject<Content>(serializer),
                    Tools = obj["tools"]?.ToObject<ToolCall[]>(serializer),
                    Name = obj["name"]?.ToString(),
                    AttachedFiles = obj["attachments"]?.ToObject<List<IUniFile>>(serializer),
                    Moderations = obj["moderations"]?.ToObject<List<ModerationData>>(serializer),
                    Usage = obj["usage"]?.ToObject<Usage>(serializer)
                };
            }
            else
            {
                JToken contentToken = obj["content"];

                Content content = null;

                if (contentToken != null && contentToken.Type != JTokenType.Null)
                {
                    if (contentToken.Type == JTokenType.Object || contentToken.Type == JTokenType.Array)
                    {
                        try
                        {
                            content = contentToken.ToObject<Content>(serializer);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"[ChatMessageConverter] Failed to deserialize ChatContent: {ex.Message}");
                        }
                    }
                    if (contentToken.Type == JTokenType.String)
                    {
                        content = new Content(contentToken.ToString());
                    }
                    else
                    {
                        Debug.LogWarning($"[ChatMessageConverter] Unexpected content token type: {contentToken.Type}");
                    }
                }

                ToolCall[] toolCalls;

                if (obj["tools"] != null && obj["tools"].Type != JTokenType.Null)
                {
                    toolCalls = obj["tools"].ToObject<ToolCall[]>(serializer);
                }
                else
                {
                    toolCalls = null;
                }

                return new ChatMessage
                {
                    Role = obj["role"]?.ToObject<ChatRole>(serializer) ?? ChatRole.User,
                    //Content = obj["content"]?.ToObject<ChatContent>(serializer),
                    Content = content,
                    Name = obj["name"]?.ToString(),
                    //Tools = obj["tool_calls"]?.ToObject<ToolCall[]>(serializer),
                    Tools = toolCalls,
                };
            }
        }

        public override void WriteJson(JsonWriter writer, ChatMessage value, JsonSerializer serializer)
        {
            JObject obj;

            if (_provider == AIProvider.None)
            {
                obj = new()
                {
                    ["role"] = JToken.FromObject(value.Role, serializer),
                    ["content"] = value.Content != null ? JToken.FromObject(value.Content, serializer) : null,
                    ["name"] = value.Name != null ? JToken.FromObject(value.Name, serializer) : null,
                    ["attachments"] = value.AttachedFiles != null ? JArray.FromObject(value.AttachedFiles, serializer) : null,
                    ["moderations"] = value.Moderations != null ? JArray.FromObject(value.Moderations, serializer) : null,
                    ["usage"] = value.Usage != null ? JToken.FromObject(value.Usage, serializer) : null,
                    ["tools"] = value.Tools != null ? JArray.FromObject(value.Tools, serializer) : null,
                };
            }
            else
            {
                obj = new()
                {
                    ["role"] = JToken.FromObject(value.Role, serializer),
                    ["content"] = value.Content != null ? JToken.FromObject(value.Content, serializer) : null,
                    ["name"] = value.Name != null ? JToken.FromObject(value.Name, serializer) : null,
                };
            }

            obj.RemoveNulls(); // null 제거 
            obj.WriteTo(writer);
        }
    }
}