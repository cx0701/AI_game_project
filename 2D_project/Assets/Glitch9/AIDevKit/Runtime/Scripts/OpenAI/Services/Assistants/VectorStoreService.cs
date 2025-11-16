using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Vector Stores: https://platform.openai.com/docs/api-reference/vector-stores
    /// </summary>
    public class VectorStoreService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/vector_stores";
        private const string kEndpointWithId = "{ver}/vector_stores/{0}";

        public VectorStoreFileService Files { get; }
        public VectorStoreFilesBatchService FilesBatches { get; }

        public VectorStoreService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
            Files = new VectorStoreFileService(client, extraHeaders);
            FilesBatches = new VectorStoreFilesBatchService(client, extraHeaders);
        }

        public async UniTask<VectorStore> Create(VectorStoreRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<VectorStoreRequest, VectorStore>(kEndpoint, this, req);
        }

        public async UniTask<VectorStore> Retrieve(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<VectorStore>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        public async UniTask<VectorStore> Update(string vectorStoreId, VectorStoreRequest req)
        {
            return await OpenAI.CRUD.UpdateAsync<VectorStoreRequest, VectorStore>(kEndpointWithId, this, req, PathParam.ID(vectorStoreId));
        }

        public async UniTask<QueryResponse<VectorStore>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<VectorStore>(kEndpoint, this, limit, order, cursor);
        }

        public async UniTask<bool> Delete(string vectorStoreId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<VectorStore>(kEndpointWithId, this, options, PathParam.ID(vectorStoreId));
        }
    }
}