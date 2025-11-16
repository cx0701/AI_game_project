
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Vector Store Files: https://platform.openai.com/docs/api-reference/vector-stores-files
    /// </summary>
    public class VectorStoreFilesBatchService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/vector_stores/{0}/file_batches";
        private const string kEndpointWithId = "{ver}/vector_stores/{0}/file_batches/{1}";

        public VectorStoreFilesBatchService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
        }

        public async UniTask<VectorStoreFilesBatch> Create(string vectorStoreId, VectorStoreFilesBatchRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<VectorStoreFilesBatchRequest, VectorStoreFilesBatch>(kEndpoint, this, req, PathParam.ID(vectorStoreId));
        }

        public async UniTask<VectorStoreFilesBatch> Retrieve(string vectorStoreId, string batchId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<VectorStoreFilesBatch>(kEndpointWithId, this, options, PathParam.ID(vectorStoreId, batchId));
        }

        public async UniTask<QueryResponse<VectorStoreFilesBatch>> List(string vectorStoreId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<VectorStoreFilesBatch>(kEndpoint, this, limit, order, cursor, PathParam.ID(vectorStoreId));
        }

        public async UniTask<VectorStoreFilesBatch> Cancel(string vectorStoreId, string batchId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.CancelAsync<VectorStoreFilesBatch>(kEndpoint, this, options, PathParam.ID(vectorStoreId, batchId));
        }
    }
}