using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// The category of a rating.
    /// These categories cover various kinds of harms that developers may wish to adjust.
    /// </summary>
    public enum HarmCategory
    {
        /// <summary>
        /// Category is unspecified.
        /// </summary>
        [ApiEnum("Unspecified", "HARM_CATEGORY_UNSPECIFIED")] Unspecified,

        /// <summary>
        /// Negative or harmful comments targeting identity and/or protected attribute.
        /// </summary>
        [ApiEnum("Derogatory", "HARM_CATEGORY_DEROGATORY")] Derogatory,

        /// <summary>
        /// Content that is rude, disrespectful, or profane.
        /// </summary>
        [ApiEnum("Toxicity", "HARM_CATEGORY_TOXICITY")] Toxicity,

        /// <summary>
        /// Describes scenarios depicting violence against an individual or group, or general descriptions of gore.
        /// </summary>
        [ApiEnum("Violence", "HARM_CATEGORY_VIOLENCE")] Violence,

        /// <summary>
        /// Contains references to sexual acts or other lewd content.
        /// </summary>
        [ApiEnum("Sexual", "HARM_CATEGORY_SEXUAL")] Sexual,

        /// <summary>
        /// Promotes unchecked medical advice.
        /// </summary>
        [ApiEnum("Medical", "HARM_CATEGORY_MEDICAL")] Medical,

        /// <summary>
        /// Dangerous content that promotes, facilitates, or encourages harmful acts.
        /// </summary>
        [ApiEnum("Dangerous", "HARM_CATEGORY_DANGEROUS")] Dangerous,

        /// <summary>
        /// Harassment content.
        /// </summary>
        [ApiEnum("Harassment", "HARM_CATEGORY_HARASSMENT")] Harassment,

        /// <summary>
        /// Hate speech and content.
        /// </summary>
        [ApiEnum("Hate Speech", "HARM_CATEGORY_HATE_SPEECH")] HateSpeech,

        /// <summary>
        /// Sexually explicit content.
        /// </summary>
        [ApiEnum("Sexually Explicit", "HARM_CATEGORY_SEXUALLY_EXPLICIT")] SexuallyExplicit,

        /// <summary>
        /// Dangerous content.
        /// </summary>
        [ApiEnum("Dangerous Content", "HARM_CATEGORY_DANGEROUS_CONTENT")] DangerousContent,
    }
}
