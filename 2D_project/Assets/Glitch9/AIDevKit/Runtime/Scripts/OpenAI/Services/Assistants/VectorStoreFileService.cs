using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Vector Store Files: https://platform.openai.com/docs/api-reference/vector-stores-files
    /// </summary>
    public class VectorStoreFileService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/vector_stores/{0}/files";
        private const string kEndpointWithId = "{ver}/vector_stores/{0}/files/{1}";

        public VectorStoreFileService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
        }

        public async UniTask<VectorStoreFile> Create(string vectorStoreId, VectorStoreFileRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<VectorStoreFileRequest, VectorStoreFile>(kEndpoint, this, req, PathParam.ID(vectorStoreId));
        }

        public async UniTask<VectorStoreFile> Retrieve(string vectorStoreId, string fileId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<VectorStoreFile>(kEndpointWithId, this, options, PathParam.ID(vectorStoreId, fileId));
        }

        public async UniTask<QueryResponse<VectorStoreFile>> List(string vectorStoreId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<VectorStoreFile>(kEndpoint, this, limit, order, cursor, PathParam.ID(vectorStoreId));
        }

        public async UniTask<bool> Delete(string vectorStoreId, string fileId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<VectorStoreFile>(kEndpointWithId, this, options, PathParam.ID(vectorStoreId, fileId));
        }
    }
}