using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public class ModerationRequest : ModelRequest
    {
        /// <summary>
        /// [Required] The input Text to classify
        /// </summary>
        [JsonProperty("input")] public StringOr<string> Input { get; set; }

        /*
         * Two content moderations models are available: Text-moderation-stable and Text-moderation-latest.
         *
         * The default is Text-moderation-latest which will be automatically upgraded over time.
         * This ensures you are always using our most accurate model.
         * If you use Text-moderation-stable, we will provide advanced notice Before updating the model.
         * Accuracy of Text-moderation-stable may be slightly lower than for Text-moderation-latest.
         *
         */

        public class Builder : ModelRequestBuilder<Builder, ModerationRequest>
        {
            public Builder SetInput(params string[] inputs)
            {
                _req.Input = new StringOr<string>(inputs);
                return this;
            }
        }
    }
}
