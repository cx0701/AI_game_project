using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google.Services
{
    /// <summary>
    /// Create, Delete, Get, List, Patch
    /// </summary>
    public class TunedModelPermissionService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/models/{0}/permissions";
        private const string kEndpointWithId = "{ver}/models/{0}/permissions/{1}";

        public TunedModelPermissionService(GenerativeAI client) : base(client, Beta.TUNED_MODELS_PERMISSIONS)
        {
        }

        public async UniTask<Permission> Create(Permission req, string corpusId, string tunedModelId)
        {
            try
            {
                if (req == null) throw new ArgumentNullException(nameof(req));
                if (string.IsNullOrEmpty(corpusId)) throw new ArgumentNullException(nameof(corpusId));
                if (string.IsNullOrEmpty(tunedModelId)) throw new ArgumentNullException(nameof(tunedModelId));
                return await GenerativeAI.CRUD.CreateAsync<Permission, Permission>(kEndpoint, this, req, PathParam.ID(corpusId, tunedModelId));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<bool> Delete(string corpusId, string tunedModelId, string permissionId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<Permission>(kEndpointWithId, this, options, PathParam.ID(corpusId, tunedModelId, permissionId));
        }

        public async UniTask<Permission> Get(string corpusId, string tunedModelId, string permissionId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<Permission>(kEndpointWithId, this, options, PathParam.ID(corpusId, tunedModelId, permissionId));
        }

        public async UniTask<QueryResponse<Permission>> List(string corpusId, string tunedModelId, int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<Permission> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<Permission>, Permission>(kEndpoint, this, req, PathParam.ID(corpusId, tunedModelId));
        }

        public async UniTask<Permission> Patch(string corpusId, string tunedModelId, string permissionId, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<Permission>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(corpusId, tunedModelId, permissionId)));
        }
    }
}