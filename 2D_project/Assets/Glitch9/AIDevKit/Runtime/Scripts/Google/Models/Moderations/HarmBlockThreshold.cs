using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Block at and beyond a specified harm probability.
    /// </summary>
    public enum HarmBlockThreshold
    {
        /// <summary>
        /// Threshold is unspecified.
        /// </summary>
        [ApiEnum("Unspecified", "HARM_BLOCK_THRESHOLD_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// Content with NEGLIGIBLE will be allowed.
        /// </summary>
        [ApiEnum("Block Low and Above", "BLOCK_LOW_AND_ABOVE")] BlockLowAndAbove,

        /// <summary>
        /// Content with NEGLIGIBLE and LOW will be allowed.
        /// </summary>
        [ApiEnum("Block Medium and Above", "BLOCK_MEDIUM_AND_ABOVE")] BlockMediumAndAbove,

        /// <summary>
        /// Content with NEGLIGIBLE, LOW, and MEDIUM will be allowed.
        /// </summary>
        [ApiEnum("Block Only High", "BLOCK_ONLY_HIGH")] BlockOnlyHigh,

        /// <summary>
        /// All content will be allowed.
        /// </summary>
        [ApiEnum("Block None", "BLOCK_NONE")] BlockNone
    }
}