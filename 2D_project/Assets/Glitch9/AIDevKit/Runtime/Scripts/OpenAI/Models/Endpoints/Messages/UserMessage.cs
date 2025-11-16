
namespace Glitch9.AIDevKit.OpenAI
{
    public class UserMessage : ThreadMessage
    {
        /// <summary>
        /// The contents of the assistant message is required unless tool_calls or function_call is specified.
        /// </summary>
        public UserMessage(string content, string name = null)
        {
            Role = AIDevKit.ChatRole.User;
            Name = name;
            Content = content;
        }

        public UserMessage(params ContentPartWrapper[] content)
        {
            Role = AIDevKit.ChatRole.User;
            Content = content;
        }

        public UserMessage(string name, params ContentPartWrapper[] content)
        {
            Role = AIDevKit.ChatRole.User;
            Name = name;
            Content = content;
        }
    }
}