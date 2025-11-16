using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    public class CachedContentService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/cachedContents";
        private const string kEndpointWithId = "{ver}/cachedContents/{0}";

        public CachedContentService(GenerativeAI client) : base(client, Beta.CACHED_CONTENTS)
        {
        }

        public async UniTask<CachedContent> Create(CachedContentRequest req)
        {
            return await GenerativeAI.CRUD.CreateAsync<CachedContentRequest, CachedContent>(kEndpoint, this, req);
        }

        public async UniTask<bool> Delete(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<CachedContent>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<CachedContent> Get(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<CachedContent>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<QueryResponse<CachedContent>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<CachedContent> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<CachedContent>, CachedContent>(kEndpoint, this, req);
        }

        public async UniTask<CachedContent> Patch(string id, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<CachedContent>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(id)));
        }
    }
}