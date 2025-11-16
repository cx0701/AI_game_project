using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Generative AI Model
    /// </summary>
    public class GenerativeModel
    {
        public string Model { get; set; }
        public string TunedModelName { get; }
        public bool IsTunedModel { get; }

        public string CachedContent
        {
            get => _cachedContent;
            set => _cachedContent = value;
        }

        public GenerationConfig GenerationConfig => _generationConfig;
        public ILogger Logger => _client.Logger;

        private readonly GenerationConfig _generationConfig;
        private readonly List<SafetySetting> _safetySettings;
        private readonly FunctionLibrary _tools;
        private readonly ToolConfig _toolConfig;
        private readonly string _systemInstruction;
        private readonly GenerativeAI _client;
        private string _cachedContent;

        public GenerativeModel(string model, GenerationConfig generationConfig = null, List<SafetySetting> safetySettings = null, FunctionLibrary tools = null, ToolConfig toolConfig = null,
            string systemInstruction = null, GenerativeAI customClient = null)
        {
            Model = model;
            IsTunedModel = false;
            _generationConfig = generationConfig ?? new GenerationConfig() { Temperature = 0.8f, MaxTokens = 2048 };
            _safetySettings = safetySettings ?? new List<SafetySetting>();
            _systemInstruction = systemInstruction;
            _tools = tools;
            _toolConfig = toolConfig;
            _client = customClient ?? GenerativeAI.DefaultInstance;
        }

        public override string ToString()
        {
            bool maybeText = !string.IsNullOrEmpty(_systemInstruction);

            return $@"
            Google.GenerativeModel(
                model_name='{Model}',
                generation_config={_generationConfig},
                safety_settings={_safetySettings},
                tools={_tools},
                system_instruction={maybeText},
                cached_content={CachedContent}
            )";
        }

        private GenerateContentRequest PrepareRequest(
            GenerateContentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request is null.");
            }

            if (_cachedContent != null && (request.Tools != null || request.ToolConfig != null || _systemInstruction != null))
            {
                throw new ArgumentException("`tools`, `tool_config`, `system_instruction` cannot be set on a model instantiated with `cached_content` as its context.");
            }

            List<SafetySetting> mergedSs = new(_safetySettings);
            if (!request.SafetySettings.IsNullOrEmpty())
            {
                foreach (SafetySetting safetySetting in request.SafetySettings)
                {
                    mergedSs.Add(safetySetting);
                }
            }

            request.Model = IsTunedModel ? TunedModelName : Model;
            request.ToolConfig ??= _toolConfig;
            request.SafetySettings = mergedSs;
            request.SystemInstruction = ContentFactory.CreateSystemInstruction(_systemInstruction);
            request.CachedContent = _cachedContent;
            if (request.Tools.IsNullOrEmpty()) request.Tools = _tools?.ToProto();

            if (request.Contents.Count > 0 && request.Contents[^1].Role != ChatRole.User)
            {
                request.Contents[^1].Role = ChatRole.User;
            }

            return request;
        }

        public FunctionLibrary GetToolsLib(FunctionLibrary tools)
        {
            return tools ?? _tools;
        }

        public async UniTask<GenerativeModel> FromCachedContent(
            string cachedContent,
            GenerationConfig generationConfig = null,
            List<SafetySetting> safetySettings = null)
        {
            CachedContent cached = await _client.CachedContents.Get(cachedContent);

            GenerativeModel model = new(cached.Model.Id, generationConfig, safetySettings)
            {
                CachedContent = cached.Name
            };
            return model;
        }

        public async UniTask<GenerateContentResponse> GenerateContentAsync(
            GenerateContentRequest request,
            ChatStreamHandler streamHandler = null)
        {
            request = PrepareRequest(request);
            if (streamHandler == null) return await _client.Models.GenerateContent(request);
            await _client.Models.StreamGenerateContent(request, streamHandler);
            return null;
        }

        public async UniTask<GenerateContentResponse> GenerateContentAsync(
            string prompt,
            List<SafetySetting> safetySettings = null,
            FunctionLibrary tools = null,
            IEnumerable<UniImageFile> images = null,
            ChatStreamHandler streamHandler = null)
        {
            Content content = ContentFactory.CreateUserContent(prompt, images);

            if (content == null)
            {
                Logger.Warning("No content to generate.");
                return null;
            }

            GenerateContentRequest request = new()
            {
                Contents = new List<Content> { content },
                SafetySettings = safetySettings,
                Tools = tools?.ToProto()
            };

            return await GenerateContentAsync(request, streamHandler);
        }

        public ChatSession StartChat(IEnumerable<Content> history = null, bool enableAutomaticFunctionCalling = false)
        {
            return new ChatSession(this, history, enableAutomaticFunctionCalling);
        }

        /// <summary>
        /// Count Content Tokens
        /// </summary>
        /// <param name="request">CountTokenRequest with Contents array</param>
        /// <returns>Number of token in content</returns>
        public async UniTask<CountTokensResponse> CountTokens(CountTokensRequest request)
        {
            return await _client.Models.CountTokens(request);
        }
    }
}
