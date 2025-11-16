using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Create, Delete, Get, List, Patch, Query
    /// </summary>
    public class CorporaService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/corpora";
        private const string kEndpointWithId = "{ver}/corpora/{0}";

        public CorporaDocumentService Document { get; }
        public CorporaPermissionService Permission { get; }

        public CorporaService(GenerativeAI client) : base(client, Beta.CORPORA)
        {
            Document = new CorporaDocumentService(client);
            Permission = new CorporaPermissionService(client);
        }

        public async UniTask<Corpus> Create(RESTRequest<Corpus> req)
        {
            return await GenerativeAI.CRUD.CreateAsync<RESTRequest<Corpus>, Corpus>(kEndpoint, this, req);
        }

        public async UniTask<bool> Delete(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<Corpus>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<Corpus> Get(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<Corpus>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<QueryResponse<Corpus>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<Corpus> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<Corpus>, Corpus>(kEndpoint, this, req);
        }

        public async UniTask<Corpus> Patch(string id, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<Corpus>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(id)));
        }

        public async UniTask<CorporaQueryResponse> Query(CorporaQueryRequest req)
        {
            return await GenerativeAI.CRUD.CreateAsync<CorporaQueryRequest, CorporaQueryResponse>(kEndpoint, this, req);
        }
    }
}