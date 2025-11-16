using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    public enum ModerationType
    {
        None,

        /// <summary>
        /// Content meant to arouse sexual excitement,
        /// such as the description of sexual activity,
        /// or that promotes sexual services
        /// (excluding sex education and wellness).
        /// </summary>
        [ApiEnum("Sexual", "sexual")]
        Sexual,

        /// <summary>
        /// Content that expresses, incites,
        /// or promotes hate based on race, gender, ethnicity, religion,
        /// nationality, sexual orientation, disability status, or caste.
        /// Hateful content aimed at non-protected groups
        /// (e.g., chess players) is harassment.
        /// </summary>
        [ApiEnum("Hate", "hate")]
        Hate,

        /// <summary>
        /// Content that expresses, incites,
        /// or promotes harassing language towards any target.
        /// </summary>
        [ApiEnum("Harassment", "harassment")]
        Harassment,

        /// <summary>
        /// Content that promotes, encourages, or depicts acts of self-harm,
        /// such as suicide, cutting, and eating disorders.
        /// </summary>
        [ApiEnum("Self-harm", "self-harm")]
        SelfHarm,

        /// <summary>
        /// Sexual content that includes an individual who is under 18 years old.
        /// </summary>
        [ApiEnum("Sexual (Minors)", "sexual/minors")]
        SexualMinors,

        /// <summary>
        /// Hateful content that also includes violence
        /// or serious harm towards the targeted group based on
        /// race, gender, ethnicity, religion, nationality,
        /// sexual orientation, disability status, or caste.
        /// </summary>
        [ApiEnum("Hate (Threatening)", "hate/threatening")]
        HateThreatening,

        /// <summary>
        /// Content that depicts death, violence,
        /// or physical injury in graphic detail.
        /// </summary>
        [ApiEnum("Violence (Graphic)", "violence/graphic")]
        ViolenceGraphic,

        /// <summary>
        /// Content where the speaker expresses
        /// that they are engaging or intend to engage in acts of self-harm,
        /// such as suicide, cutting, and eating disorders.
        /// </summary>
        [ApiEnum("Self-harm (Intent)", "self-harm/intent")]
        SelfHarmIntent,

        /// <summary>
        /// Content that encourages performing acts of self-harm,
        /// such as suicide, cutting, and eating disorders,
        /// or that gives instructions or advice on how to commit such acts.
        /// </summary>
        [ApiEnum("Self-harm (Instructions)", "self-harm/instructions")]
        SelfHarmInstructions,

        /// <summary>
        /// Harassment content that also includes violence
        /// or serious harm towards any target.
        /// </summary>
        [ApiEnum("Harassment (Threatening)", "harassment/threatening")]
        HarassmentThreatening,

        /// <summary>
        /// Content that depicts death, violence, or physical injury.
        /// </summary>
        [ApiEnum("Violence", "violence")]
        Violence
    }
}