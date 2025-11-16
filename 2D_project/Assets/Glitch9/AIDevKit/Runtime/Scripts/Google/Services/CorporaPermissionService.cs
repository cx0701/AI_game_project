using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Google
{
    /// <summary>
    /// Create, Delete, Get, List, Patch
    /// </summary>
    public class CorporaPermissionService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/corpora/{0}/permissions";
        private const string kEndpointWithId = "{ver}/corpora/{0}/permissions/{1}";

        public CorporaPermissionService(GenerativeAI client) : base(client, Beta.CORPORA_PERMISSIONS)
        {
        }

        public async UniTask<Permission> Create(Permission req)
        {
            return await GenerativeAI.CRUD.CreateAsync<Permission, Permission>(kEndpoint, this, req);
        }

        public async UniTask<bool> Delete(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<Permission>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<Permission> Get(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<Permission>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<QueryResponse<Permission>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<Permission> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<Permission>, Permission>(kEndpoint, this, req);
        }

        public async UniTask<Permission> Patch(string id, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<Permission>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(id)));
        }
    }
}