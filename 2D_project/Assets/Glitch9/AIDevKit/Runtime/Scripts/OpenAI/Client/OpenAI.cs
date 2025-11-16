using Glitch9.IO.RESTApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Glitch9.AIDevKit.OpenAI
{
    public class OpenAIClientSettingsFactory : AIClientSettingsFactory
    {
        protected override CRUDClientSettings CreateSettings()
        {
            List<RESTHeader> additionalHeaders = new();
            string organization = OpenAISettings.Organization;
            string project = OpenAISettings.ProjectId;

            if (!string.IsNullOrEmpty(organization)) additionalHeaders.Add(new(OpenAIConfig.kOrganizationHeaderName, organization));
            if (!string.IsNullOrEmpty(project)) additionalHeaders.Add(new(OpenAIConfig.kProjectHeaderName, project));

            return new CRUDClientSettings
            {
                Name = nameof(OpenAI),
                Version = OpenAIConfig.VERSION,
                BetaApiVersion = OpenAIConfig.ASSISTANTS_API_VERSION,
                BaseURL = OpenAIConfig.BASE_URL,
                AutoApiKey = AutoParam.Header,
                ApiKeyGetter = () => OpenAISettings.Instance.GetApiKey(),
                AutoVersionParam = AutoParam.Path,
                AutoBetaParam = AutoParam.Header,
                BetaHeader = new RESTHeader(OpenAIConfig.BETA_HEADER_NAME, OpenAIConfig.BETA_HEADER_ASSISTANTS),
                AdditionalHeaders = additionalHeaders.ToArray(),
            };
        }

        protected override AIClientSerializerSettings CreateSerializerSettings()
        {
            return new AIClientSerializerSettings
            {
                Converters = new List<JsonConverter>
                {
                    new CompletionRequestConverter(AIProvider.OpenAI),
                    new ChatMessageConverter(AIProvider.OpenAI),
                    new UsageConverter(AIProvider.OpenAI),
                    new ChatRoleConverter(AIProvider.OpenAI),
                    new ContentConverter(AIProvider.OpenAI),
                    new ToolCallConverter(),

                    new QueryResponseConverter<OpenAIModelData>(),
                    new QueryResponseConverter<File>(),
                    new QueryResponseConverter<Batch>(),
                    new QueryResponseConverter<Image>(),
                    new QueryResponseConverter<FineTuningJob>(),
                    new QueryResponseConverter<FineTuningEvent>(),

                    new QueryResponseConverter<Assistant>(),
                    new QueryResponseConverter<Thread>(),
                    new QueryResponseConverter<ThreadMessage>(),
                    new QueryResponseConverter<Run>(),
                    new QueryResponseConverter<RunStep>(),

                    new QueryResponseConverter<VectorStore>(),
                    new QueryResponseConverter<VectorStoreFile>(),
                    new QueryResponseConverter<VectorStoreFilesBatch>(),
                },
            };
        }
    }

    // File History
    //
    // 2024-06-02 Changes by Munchkin:
    // - Changed the class name from OpenAIClient to OpenAI.
    //   (This is due to eliminate the confusion between this asset and the examples on the official OpenAI documentation.)

    /// <summary>
    /// Represents the OpenAI service with methods to interact with the API.
    /// </summary>
    public class OpenAI : AIClient<OpenAI>
    {
        /// <summary>
        /// The default instance of the OpenAI client.
        /// </summary>
        public static OpenAI DefaultInstance => _defaultInstance ??= CreateDefault();
        private static OpenAI _defaultInstance;

        public static RESTHeader AssistantsApiHeader = new(OpenAIConfig.BETA_HEADER_NAME, OpenAIConfig.BETA_HEADER_ASSISTANTS);
        public static RESTHeader RealtimeApiHeader = new(OpenAIConfig.BETA_HEADER_NAME, OpenAIConfig.BETA_HEADER_REALTIME);

        /// <summary>
        /// Gets or sets a value indicating whether OpenAI stream events should be logged.
        /// </summary>
        public bool LogOpenAIStreamEvents { get; set; }

        // Services 
        /// <summary>
        /// Learn how to turn audio into text or text into audio.
        /// </summary>
        public AudioService Audio { get; }

        /// <summary>
        /// Given a list of messages comprising a conversation, the model will return a response.
        /// </summary>
        public ChatService Chat { get; }

        /// <summary>
        /// Get a vector representation of a given input that can be easily consumed by machine learning models and algorithms.
        /// </summary>
        public EmbeddingService Embeddings { get; }

        /// <summary>
        /// Manage fine-tuning jobs to tailor a model to your specific training data.
        /// </summary>
        public FineTuningService FineTuning { get; }

        /// <summary>
        /// Create large batches of API requests for asynchronous processing.
        /// The Batch API returns completions within 24 hours for a 50% discount.
        /// </summary>
        public BatchService Batch { get; }

        /// <summary>
        /// Files are used to upload documents that can be used with features like Assistants, Fine-tuning, and Batch API.
        /// </summary>
        public FileService Files { get; }

        /// <summary>
        /// Given a prompt and/or an input image, the model will generate a new image.
        /// </summary>
        public ImageService Images { get; }

        /// <summary>
        /// List and describe the various models available in the API.
        /// You can refer to the Models documentation to understand what models are available and the differences between them.
        /// </summary>
        public ModelService Models { get; }

        /// <summary>
        /// Given some input text, outputs if the model classifies it as potentially harmful across several categories.
        /// </summary>
        public ModerationService Moderations { get; }

        /// <summary>
        /// Beta features that are not yet available in the main API.
        /// </summary>
        public BetaService Beta { get; }


        private static OpenAI CreateDefault()
        {
            return new OpenAI
            {
                OnException = DefaultExceptionHandler,
            };

            static void DefaultExceptionHandler(string endpoint, Exception exception)
            {
                LogService.Error($"{endpoint}: {exception}");
            }
        }

        public OpenAI() : base(new OpenAIClientSettingsFactory())
        {
            // Initialize services
            Audio = new AudioService(this);
            Chat = new ChatService(this);
            Embeddings = new EmbeddingService(this);
            FineTuning = new FineTuningService(this);
            Batch = new BatchService(this);
            Files = new FileService(this);
            Images = new ImageService(this);
            Models = new ModelService(this);
            Moderations = new ModerationService(this);
            Beta = new BetaService(this);
        }

        protected override bool IsDeletedPredicate(RESTResponse res)
        {
            if (res is RESTResponse<DeletionStatus> deletionStatus)
            {
                return deletionStatus.Body.Deleted;
            }
            return res.HasBody;
        }

        private class DeletionStatus : ModelResponse
        {
            [JsonProperty("deleted")] public bool Deleted { get; set; }
        }

        internal IEnumerable<ChatDeltaResponse> ExtractDelta(string sseString)
        {
            if (string.IsNullOrEmpty(sseString)) yield break;

            const string error = "\"error\":";

            if (sseString.Contains(error))
            {
                ErrorResponseWrapper errorResponse = JsonConvert.DeserializeObject<ErrorResponseWrapper>(sseString, JsonSettings);
                if (errorResponse == null)
                {
                    Logger.Error($"Failed to parse error response: {sseString}");
                }
                else
                {
                    yield return new ChatDeltaResponse
                    {
                        IsError = true,
                        ErrorMessage = errorResponse.Error?.Message,
                    };
                }
                yield break;
            }

            var data = SSEParser.Parse(sseString);

            if (data.IsNullOrEmpty()) yield break;


            foreach (var (field, result) in data)
            {
                if (field == SSEField.Error)
                {
                    yield return new ChatDeltaResponse
                    {
                        IsError = true,
                        ErrorMessage = result,
                    };

                    //GNDebug.Mark(6);
                    yield break;

                }
                //GNDebug.Mark(7);
                if (field != SSEField.Data || string.IsNullOrEmpty(result))
                {
                    yield return null;
                }

                if (SSEParser.IsDone(result))
                {
                    yield return new ChatDeltaResponse
                    {
                        IsDone = true,
                    };

                    yield break;
                }

                ChatCompletion c = JsonConvert.DeserializeObject<ChatCompletion>(result, JsonSettings);

                yield return new ChatDeltaResponse
                {
                    Delta = c?.Choices?.FirstOrDefault()?.Delta,
                    Usage = c?.Usage,
                };
            }
        }
    }
}

