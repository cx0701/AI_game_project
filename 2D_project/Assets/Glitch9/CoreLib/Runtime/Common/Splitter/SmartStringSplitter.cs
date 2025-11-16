using System;
using System.Collections.Generic;
using System.Text;

namespace Glitch9
{
    public static class SmartStringSplitter
    {
        // List of common conjunctions/connectors
        private static readonly HashSet<string> Conjunctions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "and", "but", "or", "so", "because", "then", "however", "therefore", "thus", "moreover", "although", "though", "yet"
        };

        /// <summary>
        /// Splits a long string into multiple lines in a human-friendly way.
        /// Prioritizes punctuation, commas, conjunctions, then spaces.
        /// </summary>
        /// <param name="input">The original string to split.</param>
        /// <param name="maxLineLength">The maximum number of characters per line.</param>
        /// <returns>A nicely formatted string with newline characters inserted.</returns>
        public static string SplitSmartAsString(string input, int maxLineLength = 80)
        {
            if (string.IsNullOrEmpty(input) || maxLineLength <= 0) return input;

            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                int lastSplitIndex = 0;

                for (int i = 0; i < input.Length; i++)
                {
                    if (i - lastSplitIndex >= maxLineLength)
                    {
                        int splitAt = FindBestSplit(input, lastSplitIndex, i);

                        if (splitAt <= lastSplitIndex)
                            splitAt = i; // Fallback: force split

                        sb.AppendLine(input.Substring(lastSplitIndex, splitAt - lastSplitIndex).Trim());
                        lastSplitIndex = splitAt;
                    }
                }

                if (lastSplitIndex < input.Length)
                {
                    sb.AppendLine(input.Substring(lastSplitIndex).Trim());
                }

                return sb.ToString();
            }
        }

        public static string[] SplitSmart(string input, int maxLineLength = 80)
        {
            if (string.IsNullOrEmpty(input) || maxLineLength <= 0) return new[] { input };

            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                List<string> lines = new List<string>();
                int lastSplitIndex = 0;

                for (int i = 0; i < input.Length; i++)
                {
                    if (i - lastSplitIndex >= maxLineLength)
                    {
                        int splitAt = FindBestSplit(input, lastSplitIndex, i);

                        if (splitAt <= lastSplitIndex)
                            splitAt = i; // Fallback: force split

                        lines.Add(input.Substring(lastSplitIndex, splitAt - lastSplitIndex).Trim());
                        lastSplitIndex = splitAt;
                    }
                }

                if (lastSplitIndex < input.Length)
                {
                    lines.Add(input.Substring(lastSplitIndex).Trim());
                }

                return lines.ToArray();
            }
        }

        private static int FindBestSplit(string text, int start, int end)
        {
            // Priority 1: Punctuation
            for (int i = end; i > start; i--)
            {
                if (IsPunctuation(text[i - 1]))
                    return i;
            }

            // Priority 2: Comma
            for (int i = end; i > start; i--)
            {
                if (text[i - 1] == ',')
                    return i;
            }

            // Priority 3: Conjunction (only after spaces)
            for (int i = end; i > start + 1; i--)
            {
                if (text[i - 1] == ' ')
                {
                    string word = GetWordAfterSpace(text, i);
                    if (Conjunctions.Contains(word))
                        return i;
                }
            }

            // Priority 4: Space
            for (int i = end; i > start; i--)
            {
                if (text[i - 1] == ' ')
                    return i;
            }

            return -1;
        }

        private static bool IsPunctuation(char c)
        {
            return c == '.' || c == '!' || c == '?' || c == ':';
        }

        private static string GetWordAfterSpace(string text, int spaceIndex)
        {
            int end = text.IndexOf(' ', spaceIndex);
            if (end == -1) end = text.Length;

            return text.Substring(spaceIndex, end - spaceIndex).Trim();
        }
    }
}