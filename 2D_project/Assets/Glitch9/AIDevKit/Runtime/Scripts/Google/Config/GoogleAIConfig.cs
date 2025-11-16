namespace Glitch9.AIDevKit.Google
{
    internal class GoogleAIConfig
    {
        internal const string PACKAGE_VERSION = "0.0.1";

        internal const string VERSION = "v1";
        internal const string BETA_VERSION = "v1beta";
        internal const string BASE_URL = "https://generativelanguage.googleapis.com";

        internal const int kMaxQuery = 10;
    }

    /// <summary>
    /// {URL}?{QueryParamsKey}={QueryParamsValue}
    /// Example: https://generativelanguage.googleapis.com/v1beta/{name=corpora/*}?updateMask=permissions
    /// </summary>
    internal class QueryParams
    {
        internal const string UPDATE_MASK = "updateMask";
    }

    /// <summary>
    /// {URL}:{Method}
    /// <para>Example: https://generativelanguage.googleapis.com/v1beta/{name=corpora/*}:query</para>
    /// </summary>
    internal class Methods
    {
        internal const string QUERY = "query";
        internal const string BATCH_CREATE = "batchCreate";
        internal const string BATCH_DELETE = "batchDelete";
        internal const string BATCH_UPDATE = "batchUpdate";
        internal const string BATCH_EMBED_CONTENTS = "batchEmbedContents";
        internal const string COUNT_TOKENS = "countTokens";
        internal const string EMBED_CONTENT = "embedContent";
        internal const string GENERATE_ANSWER = "generateAnswer";
        internal const string GENERATE_CONTENT = "generateContent";
        internal const string GENERATE_TEXT = "generateText";
        internal const string STREAM_GENERATE_CONTENT = "streamGenerateContent";
        internal const string TRANSFER_OWNERSHIP = "transferOwnership";

        // Added 2025.03.30
        internal const string PREDICT = "predict";

        // Added 2025.05.05
        internal const string PREDICT_LONG_RUNNING = "predictLongRunning";
    }

    // All true for now
    internal class Beta
    {
        internal const bool CACHED_CONTENTS = true;
        internal const bool CORPORA = true;
        internal const bool CORPORA_DOCUMENTS = true;
        internal const bool CORPORA_DOCUMENTS_CHUNKS = true;
        internal const bool CORPORA_PERMISSIONS = true;
        internal const bool FILES = true;
        internal const bool MEDIA_UPLOAD = true;
        internal const bool MEDIA_UPLOAD_METADATA = true;
        internal const bool MODELS = true;
        internal const bool TUNED_MODELS = true;
        internal const bool TUNED_MODELS_PERMISSIONS = true;
    }
}