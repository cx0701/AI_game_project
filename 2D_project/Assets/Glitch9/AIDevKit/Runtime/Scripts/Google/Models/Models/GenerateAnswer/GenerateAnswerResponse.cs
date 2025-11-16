
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Response from the model for a grounded answer.
    /// If successful, the response body contains data with the following structure:
    /// </summary>
    public class GenerateAnswerResponse
    {
        /// <summary>
        /// Candidate answer from the model.
        /// </summary>
        /// <remarks>
        /// Note: The model always attempts to provide a grounded answer,
        /// even when the answer is unlikely to be answerable from the given passages.
        /// In that case, a low-quality or ungrounded answer may be provided,
        /// along with a low answerableProbability.
        /// </remarks>
        [JsonProperty("answer")] public Candidate Answer { get; set; }

        /// <summary>
        /// Output only.
        /// The model's estimate of the probability that its answer is correct and grounded in the input passages.
        /// <para>A low answerableProbability indicates that the answer might not be grounded in the sources.</para>
        /// <para>When answerableProbability is low, some clients may wish to:</para>
        /// <para>- Display a message to the effect of "We couldn’t answer that question" to the user.</para>
        /// <para>- Fall back to a general-purpose LLM that answers the question from world knowledge.
        /// The threshold and nature of such fallbacks will depend on individual clients’ use cases.
        /// 0.5 is a good starting threshold.</para>
        /// </summary>
        [JsonProperty("answerableProbability")] public float AnswerableProbability { get; set; }

        /// <summary>
        /// Output only. Feedback related to the input data used to answer the question, as opposed to model-generated response to the question.
        /// <para>"Input data" can be one or more of the following:</para>
        /// <para>- Question specified by the last entry in GenerateAnswerRequest.content</para>
        /// <para>- Conversation history specified by the other entries in GenerateAnswerRequest.content</para>
        /// <para>- Grounding sources (GenerateAnswerRequest.semantic_retriever or GenerateAnswerRequest.inline_passages)</para>
        /// </summary>
        [JsonProperty("inputFeedback")] public InputFeedback InputFeedback { get; set; }
    }

    /// <summary>
    /// Feedback related to the input data used to answer the question, as opposed to model-generated response to the question.
    /// </summary>
    public class InputFeedback
    {
        /// <summary>
        /// Ratings for safety of the input. There is at most one rating per category.
        /// </summary>
        [JsonProperty("safetyRatings")] public SafetyRating[] SafetyRatings { get; set; }

        /// <summary>
        /// Optional. If set, the input was blocked and no candidates are returned. Rephrase your input.
        /// </summary>
        [JsonProperty("blockReason")] public BlockReason BlockReason { get; set; }
    }

    /// <summary>
    /// Specifies what was the reason why input was blocked.
    /// </summary>
    public enum BlockReason
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        [ApiEnum("BLOCK_REASON_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// Input was blocked due to safety reasons. You can inspect safetyRatings to understand which safety category blocked it.
        /// </summary>
        [ApiEnum("SAFETY")] Safety,

        /// <summary>
        /// Input was blocked due to other reasons.
        /// </summary>
        [ApiEnum("OTHER")] Other
    }
}