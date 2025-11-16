using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Embeddings Service: https://platform.openai.com/docs/api-reference/embeddings
    /// </summary>
    public class EmbeddingService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/embeddings";
        public EmbeddingService(OpenAI client) : base(client) { }

        /// <summary>
        /// Creates an embedding vector representing the input text.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async UniTask<Embedding> Create(EmbeddingRequest req)
        {
            req.Model ??= OpenAISettings.DefaultEMB;
            return await OpenAI.CRUD.CreateAsync<EmbeddingRequest, Embedding>(kEndpoint, this, req);
        }
    }
}