using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System;
using System.Linq;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Chat: https://platform.openai.com/docs/api-reference/chat
    /// </summary>
    public class ChatService : CRUDService<OpenAI>
    {
        public CompletionService Completions { get; }
        public ChatService(OpenAI client) : base(client) => Completions = new CompletionService(client);
    }

    public class CompletionService : CRUDService<OpenAI>
    {
        private const string kCreateEndpoint = "{ver}/chat/completions";
        private const string kLegacyCreateEndpoint = "{ver}/completions";
        public CompletionService(OpenAI client) : base(client) { }

        public async UniTask<ChatCompletion> Create(ChatCompletionRequest req)
        {
            try
            {
                if (req.Model == null) req.Model = OpenAISettings.DefaultLLM;

                if (req.Model.IsLegacy)
                {
                    string inputMessage = req.Messages?.Count > 0 ? req.Messages.Last()?.Content : string.Empty;
                    if (string.IsNullOrEmpty(inputMessage)) throw new Exception("Input message is empty.");
                    req.Prompt = inputMessage;
                    req.Messages = null;
                    return await OpenAI.CRUD.CreateAsync<ChatCompletionRequest, ChatCompletion>(kLegacyCreateEndpoint, this, req);
                }

                return await OpenAI.CRUD.CreateAsync<ChatCompletionRequest, ChatCompletion>(kCreateEndpoint, this, req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask Stream(ChatCompletionRequest req, ChatStreamHandler streamHandler)
        {
            req.StreamHandler = streamHandler.Initialize(Client.ExtractDelta);
            req.Stream = true;

            await Create(req);
        }
    }
}