using Glitch9.AIDevKit.Google.Services;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Google
{
    public class GenerativeAIClientSettingsFactory : AIClientSettingsFactory
    {
        protected override CRUDClientSettings CreateSettings()
        {
            return new CRUDClientSettings
            {
                Name = nameof(GenerativeAI),
                Version = GoogleAIConfig.VERSION,
                BetaApiVersion = GoogleAIConfig.BETA_VERSION,
                BaseURL = GoogleAIConfig.BASE_URL,
                AutoApiKey = AutoParam.Query,
                ApiKeyQueryKey = "key",
                ApiKeyGetter = () => GenerativeAISettings.Instance.GetApiKey(),
                AutoVersionParam = AutoParam.Path,
                AutoBetaParam = AutoParam.Path,
            };
        }

        protected override AIClientSerializerSettings CreateSerializerSettings()
        {
            return new AIClientSerializerSettings
            {
                TextCase = TextCase.CamelCase,
                Converters = new List<JsonConverter>
                {
                    new UsageConverter(AIProvider.Google),
                    new ChatRoleConverter(AIProvider.Google),
                    new ContentConverter(AIProvider.Google),
                    new FunctionCallConverter(),
                    new ModalityConverter(),

                    new QueryResponseConverter<GoogleModelData>("models"),
                    new QueryResponseConverter<File>("files"),
                    new QueryResponseConverter<Chunk>("chunks"),
                },
            };
        }
    }

    public class GenerativeAI : AIClient<GenerativeAI>
    {
        // Services
        public CachedContentService CachedContents { get; }
        public CorporaService Corpora { get; }
        public FileService Files { get; }
        public MediaService Media { get; }
        public ModelService Models { get; }
        public TunedModelService TunedModels { get; }

        /// <summary>
        /// The default instance of the GenerativeAI client.
        /// </summary>
        public static GenerativeAI DefaultInstance => _defaultInstance ??= CreateDefault();
        private static GenerativeAI _defaultInstance;

        private static GenerativeAI CreateDefault()
        {
            return new GenerativeAI
            {
                OnException = DefaultExceptionHandler,
            };

            void DefaultExceptionHandler(string endpoint, Exception exception)
            {
                LogService.Error($"{endpoint}: {exception}");
            }
        }

        public GenerativeAI() : base(new GenerativeAIClientSettingsFactory())
        {
            // Initialize services
            CachedContents = new CachedContentService(this);
            Corpora = new CorporaService(this);
            Files = new FileService(this);
            Media = new MediaService(this);
            Models = new ModelService(this);
            TunedModels = new TunedModelService(this);
        }

        internal IEnumerable<ChatDeltaResponse> ExtractDelta(string raw)
        {
            AIDevKitDebug.Blue(raw);
            if (string.IsNullOrWhiteSpace(raw)) yield break;

            const string error = "\"error\":";

            if (raw.Contains(error))
            {
                var errorResponse = JsonConvert.DeserializeObject<List<ErrorResponseWrapper>>(raw, JsonSettings);
                if (errorResponse.IsNullOrEmpty())
                {
                    Logger.Error($"Failed to parse error response: {raw}");
                }
                else
                {
                    yield return new ChatDeltaResponse
                    {
                        IsError = true,
                        ErrorMessage = errorResponse[0].Error?.Message,
                    };
                }
                yield break;
            }

            string trimmedLine = raw.Trim().TrimStart(',').TrimEnd(',');

            bool isDone = false;

            if (!trimmedLine.StartsWith('['))
            {
                trimmedLine = $"[{raw}";
            }

            if (trimmedLine.EndsWith(']'))
            {
                isDone = true;
            }
            else
            {
                trimmedLine = $"{trimmedLine}]";
            }

            if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine == "[]") yield break;

            var list = JsonConvert.DeserializeObject<List<GenerateContentResponse>>(trimmedLine, JsonSettings);

            if (list.IsNullOrEmpty())
            {
                Logger.Error($"Failed to parse response: {trimmedLine}");
                yield break;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var item = list[i];
                if (item == null) continue;

                bool isLast = i == list.Count - 1;

                var delta = item.ToChatDelta();
                AIDevKitDebug.Blue(delta);

                yield return new ChatDeltaResponse
                {
                    IsDone = isLast && isDone,
                    Delta = delta,
                    Usage = item.Usage,
                };
            }
        }
    }
}