namespace Glitch9.AIDevKit
{
    internal static class ChoiceArrayExtensions
    {
        internal static string GetMessageText(this ChatChoice[] choices)
        {
            if (choices.IsNullOrEmpty()) return string.Empty;

            foreach (var choice in choices)
            {
                var message = choice.Message;

                if (message != null && !string.IsNullOrEmpty(message))
                {
                    return message;
                }
            }

            return string.Empty;
        }

        internal static ToolCall[] GetToolCalls(this ChatChoice[] choices)
        {
            if (choices.IsNullOrEmpty()) return null;

            foreach (var choice in choices)
            {
                var toolCalls = choice?.Message?.Tools;

                if (toolCalls != null && toolCalls.Length > 0)
                {
                    return toolCalls;
                }
            }

            return null;
        }

        internal static Content GetContent(this ChatChoice[] choices)
        {
            if (choices.IsNullOrEmpty()) return null;

            foreach (var choice in choices)
            {
                var content = choice?.Message?.Content;

                if (content != null) return content;
            }

            return null;
        }

        internal static string GetDeltaText(this ChatChoice[] choices)
        {
            if (choices.IsNullOrEmpty()) return string.Empty;

            foreach (var choice in choices)
            {
                var delta = choice.Delta;

                if (delta != null && !string.IsNullOrEmpty(delta))
                {
                    return delta;
                }
            }

            return string.Empty;
        }
    }
}