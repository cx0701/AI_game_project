
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Create, Delete, Get, List, Patch, Query
    /// </summary>
    public class CorporaDocumentService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/corpora/{0}/documents";
        private const string kEndpointWithId = "{ver}/corpora/{0}/documents/{1}";

        public CorporaDocumentChunkService Chunks { get; }
        public CorporaDocumentService(GenerativeAI client) : base(client, Beta.CORPORA_DOCUMENTS)
        {
            Chunks = new CorporaDocumentChunkService(client);
        }

        public async UniTask<Document> Create(Document req, string corpusId)
        {
            return await GenerativeAI.CRUD.CreateAsync<Document, Document>(kEndpoint, this, req, PathParam.ID(corpusId));
        }

        public async UniTask<bool> Delete(string corpusId, string documentId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<Document>(kEndpointWithId, this, options, PathParam.ID(corpusId, documentId));
        }


        public async UniTask<Document> Get(string corpusId, string documentId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<Document>(kEndpointWithId, this, options, PathParam.ID(corpusId, documentId));
        }

        public async UniTask<QueryResponse<Document>> List(string corpusId, int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<Document> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<Document>, Document>(kEndpoint, this, req, PathParam.ID(corpusId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <param name="corpusId"></param>
        /// <param name="documentId"></param>
        /// <param name="updateMasks">
        /// Required. The list of fields to update. Currently, this only supports updating displayName and customMetadata.
        /// This is a comma-separated list of fully qualified names of fields. Example: "user.displayName,photo".
        /// </param>
        /// <returns></returns>
        public async UniTask<Document> Patch(string corpusId, string documentId, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<Document>(kEndpointWithId,
                this, options,
                updateMasks.ToPathParams(PathParam.ID(corpusId, documentId)));
        }

        public async UniTask<CorporaQueryResponse> Query(CorporaQueryRequest req)
        {
            return await GenerativeAI.CRUD.CreateAsync<CorporaQueryRequest, CorporaQueryResponse>(kEndpoint, this, req);
        }
    }
}