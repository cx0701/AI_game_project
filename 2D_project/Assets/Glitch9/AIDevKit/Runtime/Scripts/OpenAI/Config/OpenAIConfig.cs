using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    internal class OpenAIConfig
    {
        internal const string VERSION = "v1";
        internal const string ASSISTANTS_API_VERSION = "v2";
        internal const string REALTIME_API_VERSION = "v1";

        internal const string BASE_URL = "https://api.openai.com";

        internal const string kOrganizationHeaderName = "OpenAI-Organization";
        internal const string kProjectHeaderName = "OpenAI-Project";

        internal const string AUTO_TYPE = "auto";
        internal const string BETA_HEADER_NAME = "OpenAI-Beta";
        internal const string BETA_HEADER_ASSISTANTS = "assistants=" + ASSISTANTS_API_VERSION;
        internal const string BETA_HEADER_REALTIME = "realtime=" + REALTIME_API_VERSION;

        internal const int kMaxQuery = 100;

        /// <summary>
        /// OpenAI official default values
        /// </summary>
        internal static class DefaultValues
        {
            // Image
            internal const int DALLE_2_MAX_N = 10;
            internal const int DALLE_3_MAX_N = 1;
            internal const int IMAGE_COUNT = 1;
            internal const string DALLE_MODEL = OpenAIModel.DallE2;
            internal const ImageSize IMAGE_SIZE = ImageSize._1024x1024;
            internal const ImageQuality IMAGE_QUALITY = ImageQuality.Standard;
            internal const ImageStyle IMAGE_STYLE = ImageStyle.Vivid;


            // Query 
            internal const QueryOrder QUERY_ORDER = QueryOrder.Descending;

            // Assistants API 
            internal const int MINUTES_UNTIL_RUN_EXPIRATION = 10;
        }
    }
}
