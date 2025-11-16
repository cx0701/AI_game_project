using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Create, Delete, Get, List, Patch, batchCreate, batchDelete, batchUpdate
    /// </summary>
    public class CorporaDocumentChunkService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/corpora/{0}/documents/{1}/chunks";
        private const string kEndpointWithId = "{ver}/corpora/{0}/documents/{1}/chunks/{2}";
        public CorporaDocumentChunkService(GenerativeAI client) : base(client, Beta.CORPORA_DOCUMENTS_CHUNKS)
        {
        }

        public async UniTask<Chunk> Create(Chunk req)
        {
            return await GenerativeAI.CRUD.CreateAsync<Chunk, Chunk>(kEndpoint, this, req);
        }

        public async UniTask<bool> Delete(string corpusId, string documentId, string chunkId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<Chunk>(kEndpointWithId,
                this, options,
                PathParam.ID(corpusId, documentId, chunkId));
        }

        public async UniTask<Chunk> Get(string corpusId, string documentId, string chunkId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<Chunk>(kEndpointWithId,
                this, options,
                PathParam.ID(corpusId, documentId, chunkId));
        }

        public async UniTask<QueryResponse<Chunk>> List(string corpusId, string documentId, int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<Chunk> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<Chunk>, Chunk>(kEndpoint, this, req, PathParam.ID(corpusId, documentId));
        }

        public async UniTask<Chunk> Patch(string corpusId, string documentId, string chunkId, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<Chunk>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(corpusId, documentId, chunkId)));
        }

        public async UniTask<QueryResponse<Chunk>> CreateBatch(ChunkBatchRequest<CreateChunkRequest> req, string corpusId, string documentId)
        {
            return await GenerativeAI.CRUD.CreateAsync<ChunkBatchRequest<CreateChunkRequest>, QueryResponse<Chunk>>(kEndpoint,
                this, req,
                PathParam.ID(corpusId, documentId),
                PathParam.Method(Methods.BATCH_CREATE));
        }

        public async UniTask<bool> DeleteBatch(ChunkBatchRequest<DeleteChunkRequest> req, string corpusId, string documentId)
        {
            RESTResponse deleteResult = await GenerativeAI.CRUD.CreateAsync<ChunkBatchRequest<DeleteChunkRequest>, RESTResponse>(kEndpointWithId,
                this, req,
                PathParam.ID(corpusId, documentId),
                PathParam.Method(Methods.BATCH_DELETE));

            return deleteResult.HasBody;
        }

        public async UniTask<QueryResponse<Chunk>> UpdateBatch(ChunkBatchRequest<UpdateChunkRequest> req, string corpusId, string documentId)
        {
            return await GenerativeAI.CRUD.CreateAsync<ChunkBatchRequest<UpdateChunkRequest>, QueryResponse<Chunk>>(kEndpoint,
                this, req,
                PathParam.ID(corpusId, documentId),
                PathParam.Method(Methods.BATCH_UPDATE));
        }
    }
}