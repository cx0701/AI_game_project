using System;
using System.Text;

namespace Glitch9
{
    public static class SentenceSplitter
    {
        public static string SplitToParagraphs(string input, int linebreaks = 2)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                int parenDepth = 0;
                int consecutiveLineBreaks = 0;
                bool justAddedBreak = false;
                bool paragraphStart = true;
                string breaks = new('\n', linebreaks);

                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];

                    if (c == '\n')
                    {
                        consecutiveLineBreaks++;
                        if (consecutiveLineBreaks <= linebreaks)
                            sb.Append(c);

                        if (consecutiveLineBreaks >= linebreaks)
                            paragraphStart = true;

                        continue;
                    }
                    else
                    {
                        consecutiveLineBreaks = 0;
                    }

                    // 문단 시작 시 선행 공백 제거
                    if (paragraphStart && char.IsWhiteSpace(c))
                        continue;

                    paragraphStart = false;
                    sb.Append(c);

                    if (c == '(') parenDepth++;
                    else if (c == ')') parenDepth = Math.Max(parenDepth - 1, 0);

                    if (parenDepth == 0 && (c == '.' || c == '!' || c == '?'))
                    {
                        if (i + 1 >= input.Length || char.IsWhiteSpace(input[i + 1]))
                        {
                            if (!justAddedBreak)
                            {
                                sb.Append(breaks);
                                justAddedBreak = true;
                                paragraphStart = true;
                            }
                        }
                        else
                        {
                            justAddedBreak = false;
                        }
                    }
                    else
                    {
                        justAddedBreak = false;
                    }
                }

                return sb.ToString().TrimEnd('\n', '\r');
            }
        }

    }
}