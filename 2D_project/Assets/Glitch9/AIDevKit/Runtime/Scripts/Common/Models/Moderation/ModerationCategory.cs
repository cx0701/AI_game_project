namespace Glitch9.AIDevKit
{
    public enum ModerationCategory
    {
        None,

        /// <summary>
        /// Content that is not classified into any specific category.
        /// </summary>
        Unspecified,

        /// <summary>
        /// Negative or harmful comments targeting identity and/or protected attribute.
        /// </summary>
        Derogatory,

        /// <summary>
        /// Content that is rude, disrespectful, or profane.
        /// </summary>
        Toxicity,

        /// <summary>
        /// Describes scenarios depicting violence against an individual or group, or general descriptions of gore.
        /// </summary>
        Violence,

        /// <summary>
        /// Contains references to sexual acts or other lewd content.
        /// </summary>
        Sexual,

        /// <summary>
        /// Promotes unchecked medical advice.
        /// </summary>
        Medical,

        /// <summary>
        /// Dangerous content that promotes, facilitates, or encourages harmful acts.
        /// </summary>
        Dangerous,

        /// <summary>
        /// Harassment content.
        /// </summary>
        Harassment,

        /// <summary>
        /// Hate speech and content.
        /// </summary>
        HateSpeech,

        /// <summary>
        /// Sexually explicit content.
        /// </summary>
        SexuallyExplicit,

        /// <summary>
        /// Dangerous content.
        /// </summary>
        DangerousContent,

        /// <summary>
        /// Content meant to arouse sexual excitement, such as the description of sexual activity, or that promotes sexual services (excluding sex education and wellness).
        /// </summary>
        SexualContent,

        /// <summary>
        /// Content that expresses, incites, or promotes hate based on race, gender, ethnicity, religion, nationality, sexual orientation, disability status, or caste.
        /// </summary>
        Hate,

        /// <summary>
        /// Content that promotes, encourages, or depicts acts of self-harm, such as suicide, cutting, and eating disorders.
        /// </summary>
        SelfHarm,

        /// <summary>
        /// Sexual content that includes an individual who is under 18 years old.
        /// </summary>
        SexualMinors,

        /// <summary>
        /// Hateful content that also includes violence or serious harm towards the targeted group based on race, gender, ethnicity, religion, nationality, sexual orientation, disability status, or caste.
        /// </summary>
        HateThreatening,

        /// <summary>
        /// Content that depicts death, violence, or physical injury in graphic detail.
        /// </summary>
        ViolenceGraphic,

        /// <summary>
        /// Content where the speaker expresses that they are engaging or intend to engage in acts of self-harm, such as suicide, cutting, and eating disorders.
        /// </summary>
        SelfHarmIntent,

        /// <summary>
        /// Content that encourages performing acts of self-harm, such as suicide, cutting, and eating disorders, or that gives instructions or advice on how to commit such acts.
        /// </summary>
        SelfHarmInstructions,

        /// <summary>
        /// Harassment content that also includes violence or serious harm towards any target.
        /// </summary>
        HarassmentThreatening
    }
}