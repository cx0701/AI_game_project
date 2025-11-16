using System;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit
{
    public enum ChatRole
    {
        Unset,

        /// <summary>
        /// Developer-provided instructions that the model should follow, 
        /// regardless of messages sent by the user.  
        /// 
        /// Supported by 
        /// <see cref="AIProvider.OpenAI"/>, 
        /// <see cref="AIProvider.Google"/>, 
        /// <see cref="AIProvider.Ollama"/>
        /// </summary> 
        /// <remarks>
        /// [OpenAI] With o1 models and newer, use developer messages for this purpose instead.
        /// </remarks>
        [ApiEnum("system")] System,

        /// <summary>
        /// Messages sent by an end user, containing prompts or additional context information.
        /// 
        /// Supported by
        /// <see cref="AIProvider.OpenAI"/>, 
        /// <see cref="AIProvider.Google"/>, 
        /// <see cref="AIProvider.Ollama"/>
        /// </summary>
        [ApiEnum("user")] User,

        /// <summary>
        /// Messages sent by the model in response to user messages.
        /// 
        /// Supported by
        /// <see cref="AIProvider.OpenAI"/>,  
        /// <see cref="AIProvider.Ollama"/>
        /// </summary>
        [ApiEnum("assistant")] Assistant,

        /// <summary>
        /// Supported by
        /// <see cref="AIProvider.OpenAI"/>,  
        /// <see cref="AIProvider.Ollama"/>
        /// </summary>
        [ApiEnum("tool")] Tool,

        /// <summary>
        /// Supported by 
        /// <see cref="AIProvider.Google"/> 
        /// 
        /// Deprecated for
        /// <see cref="AIProvider.OpenAI"/> (changed to "tool") 
        /// </summary>
        ///[ApiEnum("function")] Function,

        /// <summary>
        /// Messages sent by the model in response to user messages.
        /// 
        /// Supported by 
        /// <see cref="AIProvider.Google"/> 
        /// 
        /// Deprecated for
        /// <see cref="AIProvider.OpenAI"/> (changed to "assistant") 
        /// </summary>
        ///[ApiEnum("model")] Model,

        /// <summary>
        /// Developer-provided instructions that the model should follow, 
        /// regardless of messages sent by the user.  
        /// 
        /// Supported by 
        /// <see cref="AIProvider.OpenAI"/>
        /// </summary> 
        /// <remarks>
        /// [OpenAI] With o1 models and newer, developer messages replace the previous system messages.
        /// </remarks>
        [ApiEnum("developer")] Developer,
    }

    public class ChatRoleConverter : JsonConverter<ChatRole>
    {
        private readonly AIProvider _api;
        internal ChatRoleConverter(AIProvider api) => _api = api;

        public override ChatRole ReadJson(JsonReader reader, Type objectType, ChatRole existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return ChatRole.Unset;

            string value = reader.Value.ToString();

            switch (value)
            {
                case "system":
                    if (_api == AIProvider.Google) return ChatRole.Assistant;
                    return ChatRole.System;
                case "user":
                    return ChatRole.User;
                case "assistant":
                case "model":
                    return ChatRole.Assistant;
                case "tool":
                case "function":
                    return ChatRole.Tool;
                case "developer":
                    return ChatRole.Developer;
                default:
                    break;
            }

            throw new ArgumentException($"Invalid value for {nameof(ChatRole)}: {value}");
        }

        public override void WriteJson(JsonWriter writer, ChatRole value, JsonSerializer serializer)
        {
            switch (value)
            {
                case ChatRole.System:

                    if (_api == AIProvider.Google)
                    {
                        writer.WriteValue("assistant");
                        break;
                    }
                    else
                    {
                        writer.WriteValue("system");
                        break;
                    }

                case ChatRole.User:
                    writer.WriteValue("user");
                    break;
                case ChatRole.Assistant:
                    writer.WriteValue(_api == AIProvider.Google ? "model" : "assistant");
                    break;
                case ChatRole.Tool:
                    writer.WriteValue(_api == AIProvider.Google ? "function" : "tool");
                    break;
                case ChatRole.Developer:
                    writer.WriteValue("developer");
                    break;
                default:
                    writer.WriteNull();
                    break;
            }
        }
    }
}