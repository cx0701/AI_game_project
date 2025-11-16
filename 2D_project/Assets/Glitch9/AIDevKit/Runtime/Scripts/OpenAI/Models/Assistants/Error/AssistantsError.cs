namespace Glitch9.AIDevKit.OpenAI
{
    public class AssistantsError
    {
        /// <summary>
        /// The error code.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The error message.
        /// </summary>
        public string Message { get; set; }

        public override string ToString()
        {
            return $"{Code}: {Message}";
        }
    }
}