using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Glitch9.AIDevKit.Google
{
    public class ChatSession
    {
        public GenerativeModel Model { get; }
        public bool EnableAutomaticFunctionCalling { get; }
        public GenerateContentResponse Last => _lastReceived;
        public Exception LastException { get; private set; }
        public List<Content> History
        {
            get
            {
                if (_lastReceived != null && _lastReceived.Candidates[0].FinishReason != FinishReason.Unspecified &&
                    _lastReceived.Candidates[0].FinishReason != FinishReason.Stop &&
                    _lastReceived.Candidates[0].FinishReason != FinishReason.MaxTokens)
                {
                    StopCandidateException error = new(_lastReceived.Candidates[0]);
                    LastException = error;
                }

                if (LastException != null)
                {
                    throw new BrokenResponseException("Unable to build a coherent chat history due to a broken streaming response.", LastException);
                }

                if (_lastSent != null)
                {
                    _history.Add(_lastSent);
                    if (_lastReceived != null) _history.Add(_lastReceived.Candidates[0].Content);
                    _lastSent = null;
                    _lastReceived = null;
                }

                return _history;
            }
            set
            {
                _history = value ?? new List<Content>();
                _lastSent = null;
                _lastReceived = null;
            }
        }

        private List<Content> _history;
        private Content _lastSent;
        private GenerateContentResponse _lastReceived;

        public ChatSession(GenerativeModel model, IEnumerable<Content> history = null, bool enableAutomaticFunctionCalling = false)
        {
            Model = model;
            _history = history?.ToList() ?? new List<Content>();
            EnableAutomaticFunctionCalling = enableAutomaticFunctionCalling;
        }

        public async UniTask<GenerateContentResponse> SendMessageAsync(string prompt,
            IEnumerable<UniImageFile> images = null,
            GenerationConfig generationConfig = null,
            List<SafetySetting> safetySettings = null,
            FunctionLibrary tools = null,
            ToolConfig toolConfig = null,
            ChatStreamHandler streamHandler = null)
        {
            LastException = null;
            Content content = ContentFactory.CreateUserContent(prompt, images);
            return await SendMessageAsync(content, generationConfig, safetySettings, tools, toolConfig, streamHandler);
        }

        public async UniTask<GenerateContentResponse> SendMessageAsync(IList<ContentPart> parts,
            GenerationConfig generationConfig = null,
            List<SafetySetting> safetySettings = null,
            FunctionLibrary tools = null,
            ToolConfig toolConfig = null,
            ChatStreamHandler streamHandler = null)
        {
            LastException = null;
            Content content = ContentFactory.CreateUserContent(parts);
            return await SendMessageAsync(content, generationConfig, safetySettings, tools, toolConfig, streamHandler);
        }

        public async UniTask<GenerateContentResponse> SendMessageAsync(Content content,
            GenerationConfig generationConfig = null,
            List<SafetySetting> safetySettings = null,
            FunctionLibrary tools = null,
            ToolConfig toolConfig = null,
            ChatStreamHandler streamHandler = null)
        {
            LastException = null;
            if (content == null) return null;

            bool stream = streamHandler != null;

            if (EnableAutomaticFunctionCalling && stream)
            {
                throw new NotImplementedException("Unsupported configuration: The `Google` SDK currently does not support the combination of `stream=True` and `enable_automatic_function_calling=True`.");
            }

            content.Role = ChatRole.User;
            FunctionLibrary toolsLib = Model.GetToolsLib(tools);
            generationConfig ??= Model.GenerationConfig;

            if (generationConfig.CandidateCount > 0)
            {
                Model.Logger.Warning("Invalid configuration: The chat functionality does not support `candidate_count` greater than 1. Resetting to 1.");
                generationConfig.CandidateCount = 1;
            }

            List<Content> contents = new();
            contents.AddRange(_history);
            contents.Add(content);

            GenerateContentRequest req = new()
            {
                Contents = contents,
                Config = generationConfig,
                SafetySettings = safetySettings,
                Tools = toolsLib?.ToProto(),
                ToolConfig = toolConfig,
            };

            GenerateContentResponse response = await Model.GenerateContentAsync(req, streamHandler);

            Validator.CheckResponse(response, stream);

            if (EnableAutomaticFunctionCalling && toolsLib != null && !toolsLib.IsEmpty)
            {
                (List<Content> history, Content content, GenerateContentResponse response) result =
                    await HandleAfcAsync(response, _history, generationConfig, safetySettings, stream, toolsLib);
                _history = result.history;
                content = result.content;
                response = result.response;
            }

            _lastSent = content;
            _lastReceived = response;

            return response;
        }

        private List<FunctionCall> GetFunctionCalls(GenerateContentResponse response)
        {
            if (response.Candidates.Length != 1)
            {
                throw new ArgumentException("Invalid number of candidates: Automatic function calling only works with 1 candidate.");
            }

            return response.Candidates[0].Content.Parts
                .Where(part => part != null && part.FunctionCall != null)
                .Select(part => part.FunctionCall)
                .ToList();
        }

        private async UniTask<(List<Content> history, Content content, GenerateContentResponse response)> HandleAfcAsync(
            GenerateContentResponse response,
            List<Content> history,
            GenerationConfig generationConfig,
            List<SafetySetting> safetySettings,
            bool stream,
            FunctionLibrary toolsLib)
        {
            while (true)
            {
                List<FunctionCall> functionCalls = GetFunctionCalls(response);
                if (!functionCalls.All(fc => toolsLib[fc].IsCallable))
                {
                    break;
                }

                history.Add(response.Candidates[0].Content);

                List<ContentPart> functionResponseParts = new();
                foreach (FunctionCall fc in functionCalls)
                {
                    ContentPart fr = await toolsLib.CallAsync(fc);
                    if (fr == null) throw new InvalidOperationException("Unexpected state: The function reference should never be null.");
                    functionResponseParts.Add(fr);
                }

                Content send = new()
                {
                    Role = ChatRole.User,
                    Parts = functionResponseParts.ToArray()
                };

                history.Add(send);

                GenerateContentRequest req = new()
                {
                    Contents = history,
                    Config = generationConfig,
                    SafetySettings = safetySettings,
                    Tools = toolsLib.ToProto(),
                };

                response = await Model.GenerateContentAsync(req);

                Validator.CheckResponse(response, stream);
            }

            Content content = history.Last();
            history.RemoveAt(history.Count - 1);
            return (history, content, response);
        }

        public ChatSession Copy()
        {
            return new ChatSession(Model, new List<Content>(_history), EnableAutomaticFunctionCalling);
        }

        public (Content lastSent, Content lastReceived) Rewind()
        {
            if (_lastReceived == null)
            {
                Content lastSent = _history[_history.Count - 2];
                Content lastReceived = _history.Last();
                _history.RemoveRange(_history.Count - 2, 2);
                return (lastSent, lastReceived);
            }
            else
            {
                Content lastSent = _lastSent;
                Content lastReceived = _lastReceived.Candidates[0].Content;
                _lastSent = null;
                _lastReceived = null;
                return (lastSent, lastReceived);
            }
        }

        public override string ToString()
        {
            string modelString = Model.ToString().Replace("\n", "\n    ");
            string historyString = string.Join(", ", _history.Select(content => $"Content({content})"));

            string lastReceivedString = _lastReceived != null ? (LastException != null ? "<STREAMING ERROR>" : "<STREAMING IN PROGRESS>") : "";

            return $@"
            ChatSession(
                model={modelString},
                history=[{historyString}{lastReceivedString}]
            )";
        }
    }
}
