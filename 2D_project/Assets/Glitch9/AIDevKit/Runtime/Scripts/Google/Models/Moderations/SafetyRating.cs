using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// A safety rating associated with a {@link GenerateContentCandidate}
    /// </summary>
    public class SafetyRating
    {
        [JsonProperty("category")] public HarmCategory Category { get; set; }
        [JsonProperty("probability")] public HarmProbability Probability { get; set; }

        public override string ToString()
        {
            return $"{Category}({Probability})";
        }
    }
}