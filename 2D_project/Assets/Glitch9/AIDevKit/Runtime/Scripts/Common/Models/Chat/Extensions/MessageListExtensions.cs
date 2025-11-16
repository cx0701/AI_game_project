using System.Collections.Generic;

namespace Glitch9.AIDevKit
{
    // Extensions for List<LocalMessage>
    internal static class MessageListExtensions
    {
        internal static void SetSystemInstruction(this List<ChatMessage> list, string systemInstruction, ChatRole systemRole = ChatRole.System)
        {
            if (string.IsNullOrEmpty(systemInstruction)) return;
            list ??= new();
            list.Insert(0, new(systemRole, systemInstruction));
        }

        internal static void SetStartingMessage(this List<ChatMessage> list, string startingMessage)
        {
            if (string.IsNullOrWhiteSpace(startingMessage)) return;
            list ??= new();

            if (list.Count == 0)
            {
                list.Add(new(ChatRole.Assistant, startingMessage));
                return;
            }

            int targetIndex = 0;

            while (targetIndex < list.Count && list[targetIndex].Role == ChatRole.System)
            {
                targetIndex++;
            }

            bool insert = targetIndex == list.Count || list[targetIndex].Role != ChatRole.Assistant;

            if (insert)
            {
                list.Insert(targetIndex, new(ChatRole.Assistant, startingMessage));
            }
            else
            {
                list[targetIndex] = new(ChatRole.Assistant, startingMessage);
            }
        }

        internal static void SetSummary(this List<ChatMessage> list, string summary)
        {
            if (string.IsNullOrWhiteSpace(summary)) return;
            list ??= new();

            string parsedSummary = $"Here is a summary of the conversation so far:\n{summary}";

            if (list.Count == 0)
            {
                list.Add(new(ChatRole.System, parsedSummary));
                return;
            }

            int targetIndex = 0;
            if (list[targetIndex].Role == ChatRole.System) targetIndex = 1;

            if (list[targetIndex].Role == ChatRole.System)
            {
                list[targetIndex] = new(ChatRole.System, parsedSummary);
            }
            else
            {
                list.Insert(targetIndex, new(ChatRole.System, parsedSummary));
            }
        }

        internal static void ReplaceLastMessage(this List<ChatMessage> list, ChatMessage message)
        {
            if (list == null || list.Count == 0 || message == null) return;
            list[^1] = message;
        }
    }
}