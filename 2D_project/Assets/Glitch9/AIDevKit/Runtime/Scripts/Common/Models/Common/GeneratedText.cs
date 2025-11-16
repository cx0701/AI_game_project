namespace Glitch9.AIDevKit
{
    public class GeneratedText : GeneratedContent<string>
    {
        public static implicit operator string(GeneratedText generatedText) => generatedText.contents[0];
        public static implicit operator string[](GeneratedText generatedText) => generatedText.contents;
        public static implicit operator GeneratedText(string text) => new(text, null);
        public readonly ToolCall[] toolCalls;
        public GeneratedText(string text, Usage usage) : base(text, usage) { }
        public GeneratedText(string[] texts, Usage usage) : base(texts, usage) { }
        public GeneratedText(string text, ToolCall[] toolCalls, Usage usage = null) : base(text, usage) => this.toolCalls = toolCalls;
        public GeneratedText(string[] texts, ToolCall[] toolCalls, Usage usage = null) : base(texts, usage) => this.toolCalls = toolCalls;
        public static GeneratedText Transcript(string transcript)
        {
            Usage perCharacter = Usage.PerCharacter(transcript?.Length ?? 0);
            return new GeneratedText(transcript, null, perCharacter);
        }

        public override string ToString() => contents[0];
    }
}
