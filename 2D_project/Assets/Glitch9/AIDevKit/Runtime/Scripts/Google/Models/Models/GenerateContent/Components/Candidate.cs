using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// A response candidate generated from the model.
    /// </summary>
    public class Candidate
    {
        /// <summary>
        /// Output only.
        /// Index of the candidate in the list of candidates.
        /// </summary>
        [JsonProperty("index")] public int? Index { get; set; }

        /// <summary>
        /// Output only.
        /// Generated content returned from the model.
        /// </summary>
        [JsonProperty("content")] public Content Content { get; set; }

        /// <summary>
        /// Optional. Output only.
        /// The reason why the model stopped generating tokens.
        /// If empty, the model has not stopped generating the tokens.
        /// </summary>
        [JsonProperty("finishReason")] public FinishReason? FinishReason { get; set; }

        /// <summary>
        /// List of ratings for the safety of a response candidate.
        /// There is at most one rating per category.
        /// </summary>
        [JsonProperty("safetyRatings")] public SafetyRating[] SafetyRatings { get; set; }

        /// <summary>
        /// Output only.
        /// Citation information for model-generated candidate.
        /// <para>
        /// This field may be populated with recitation information for any text included in the content.
        /// These are passages that are "recited" from copyrighted material in the foundational LLM's training data.
        /// </para>
        /// </summary>
        [JsonProperty("citationMetadata")] public CitationMetadata CitationMetadata { get; set; }

        /// <summary>
        /// Output only. Token count for this candidate.
        /// </summary>
        [JsonProperty("tokenCount")] public int TokenCount { get; set; }

        /// <summary>
        /// Output only. Token count for this candidate.
        /// This field is populated for GenerateAnswer calls.
        /// </summary>
        [JsonProperty("groundingAttributions")] public GroundingAttribution[] GroundingAttributions { get; set; }

        public string GetErrorMessage()
        {
            if (FinishReason == null) return "The model has not stopped generating the tokens.";

            return FinishReason switch
            {
                Google.FinishReason.Unspecified => "The model has not stopped generating the tokens.",
                Google.FinishReason.Stop => "Natural stop point of the model or provided stop sequence.",
                Google.FinishReason.MaxTokens => "The model reached the maximum token limit.",
                Google.FinishReason.Safety => "The candidate content was flagged for safety reasons.",
                Google.FinishReason.Recitation => "The candidate content was flagged for recitation reasons.",
                Google.FinishReason.Other => "Unknown reason.",
                _ => "The model has not stopped generating the tokens.",
            };
        }
    }

    /// <summary>
    /// Citation metadata that may be found on a {@link GenerateContentCandidate}.
    /// </summary>
    public class CitationMetadata
    {
        [JsonProperty("citationMetadata")] public CitationSource[] CitationSources { get; set; }
    }

    /// <summary>
    /// A single citation source.
    /// </summary>
    public class CitationSource
    {
        [JsonProperty("startIndex")] public int? StartIndex { get; set; }
        [JsonProperty("endIndex")] public int? EndIndex { get; set; }
        [JsonProperty("uri")] public string Uri { get; set; }
        [JsonProperty("license")] public string License { get; set; }
    }

    /// <summary>
    /// Attribution for a source that contributed to an answer.
    /// </summary>
    public class GroundingAttribution
    {
        /// <summary>
        /// Output only.
        /// Identifier for the source contributing to this attribution.
        /// </summary>
        [JsonProperty("sourceId")] public AttributionSourceId SourceId { get; set; }

        /// <summary>
        /// Grounding source content that makes up this attribution.
        /// </summary>
        [JsonProperty("content")] public Content Content { get; set; }
    }


    /// <summary>
    /// Identifier for the source contributing to this attribution.
    /// </summary>
    public class AttributionSourceId
    {
        /// <summary>
        /// Identifier for an inline passage.
        /// </summary>
        [JsonProperty("groundingPassage")] public GroundingPassageId GroundingPassage { get; set; }

        /// <summary>
        /// Identifier for a Chunk fetched via Semantic Retriever.
        /// </summary>
        [JsonProperty("semanticRetrieverChunk")] public SemanticRetrieverChunk SemanticRetrieverChunk { get; set; }
    }

    /// <summary>
    /// Identifier for a part within a <see cref="GroundingPassageId"/>.
    /// </summary>
    public class GroundingPassageId
    {
        /// <summary>
        /// Output only.
        /// ID of the passage matching the GenerateAnswerRequest's GroundingPassage.id.
        /// </summary>
        [JsonProperty("passageId")] public string PassageId { get; set; }

        /// <summary>
        /// Output only.
        /// Index of the part within the GenerateAnswerRequest's GroundingPassage.content.
        /// </summary>
        [JsonProperty("partIndex")] public int PartIndex { get; set; }
    }

    /// <summary>
    /// Identifier for a Chunk retrieved via Semantic Retriever specified in the <see cref="GenerateAnswerRequest"/> using <see cref="SemanticRetrieverConfig"/>.
    /// </summary>
    public class SemanticRetrieverChunk
    {
        /// <summary>
        /// Output only.
        /// Name of the source matching the request's SemanticRetrieverConfig.source.
        /// <para>Example: corpora/123 or corpora/123/documents/abc</para>
        /// </summary>
        [JsonProperty("source")] public string Source { get; set; }

        /// <summary>
        /// Output only.
        /// Name of the Chunk containing the attributed text.
        /// <para>Example: corpora/123/documents/abc/chunks/xyz</para>
        /// </summary>
        [JsonProperty("chunk")] public string Chunk { get; set; }
    }

    /// <summary>
    /// Reason that a candidate finished.
    /// </summary>
    public enum FinishReason
    {
        /// <summary>
        /// Default value. This value is unused.
        /// </summary>
        [ApiEnum("FINISH_REASON_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// Natural stop point of the model or provided stop sequence.
        /// </summary>
        [ApiEnum("STOP")] Stop,

        /// <summary>
        /// The maximum number of tokens as specified in the request was reached.
        /// </summary>
        [ApiEnum("MAX_TOKENS")] MaxTokens,

        /// <summary>
        /// The candidate content was flagged for safety reasons.
        /// </summary>
        [ApiEnum("SAFETY")] Safety,

        /// <summary>
        /// The candidate content was flagged for recitation reasons
        /// </summary>
        [ApiEnum("RECITATION")] Recitation,

        /// <summary>
        /// Unknown reason.
        /// </summary>
        [ApiEnum("OTHER")] Other
    }
}