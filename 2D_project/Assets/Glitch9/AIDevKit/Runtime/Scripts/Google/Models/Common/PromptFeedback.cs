using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// A set of the feedback metadata the prompt specified in <see cref="GenerateContentRequest.Contents"/>.
    /// </summary>
    public class PromptFeedback
    {
        [JsonProperty("blockReason")] public BlockReason BlockReason { get; set; }
        [JsonProperty("safetyRatings")] public SafetyRating[] SafetyRatings { get; set; }


        public override string ToString()
        {
            if (BlockReason == BlockReason.Safety) return $"BlockReason: {BlockReason}, SafetyRatings: {SafetyRatings}";
            return $"BlockReason: {BlockReason}";
        }
    }
}