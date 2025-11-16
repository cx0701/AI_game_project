using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Probability that a prompt or candidate matches a harm category.
    /// </summary>
    public enum HarmProbability
    {
        [ApiEnum("HARM_PROBABILITY_UNSPECIFIED")] Unspecified,
        [ApiEnum("NEGLIGIBLE")] Negligible,
        [ApiEnum("LOW")] Low,
        [ApiEnum("MEDIUM")] Medium,
        [ApiEnum("HIGH")] High
    }
}