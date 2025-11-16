using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System;

namespace Glitch9.AIDevKit.Google.Services
{
    /// <summary>
    /// Create, Delete, GenerateContent, GenerateText, Get, List, Patch, TransferOwnership
    /// </summary>
    public class TunedModelService : CRUDService<GenerativeAI>
    {
        internal const string kEndpoint = "{ver}/tunedModels";
        internal const string kEndpointWithId = "{ver}/tunedModels/{0}";
        public TunedModelPermissionService Permissions { get; }
        public TunedModelService(GenerativeAI client) : base(client, Beta.TUNED_MODELS)
        {
            Permissions = new TunedModelPermissionService(client);
        }

        public async UniTask<TunedModel> Create(TunedModel req)
        {
            try
            {
                if (req == null) throw new ArgumentNullException(nameof(req));

                if (string.IsNullOrEmpty(req?.Name))
                {
                    return await GenerativeAI.CRUD.CreateAsync<TunedModel, TunedModel>(kEndpoint, this, req, PathParam.ID(req?.Name));
                }

                return await GenerativeAI.CRUD.CreateAsync<TunedModel, TunedModel>(kEndpoint, this, req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<bool> Delete(string id, RESTRequestOptions options = null)

        {
            return await GenerativeAI.CRUD.DeleteAsync<TunedModel>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<GenerateContentResponse> GenerateContent(GenerateContentRequest req)
        {
            try
            {
                if (req == null) throw new ArgumentNullException(nameof(req));
                if (req.Model == null) throw new ArgumentNullException(nameof(req.Model));
                string modelName = req.Model.Id;

                return await GenerativeAI.CRUD.CreateAsync<GenerateContentRequest, GenerateContentResponse>(kEndpointWithId,
                    this, req, PathParam.ID(modelName), PathParam.Method(Methods.GENERATE_CONTENT));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<GenerateTextResponse> GenerateText(GenerateTextRequest req)
        {
            try
            {
                if (req == null) throw new ArgumentNullException(nameof(req));
                if (req.Model == null) throw new ArgumentNullException(nameof(req.Model));
                string modelName = req.Model.Id;

                return await GenerativeAI.CRUD.CreateAsync<GenerateTextRequest, GenerateTextResponse>(kEndpointWithId,
                    this, req, PathParam.ID(modelName), PathParam.Method(Methods.GENERATE_TEXT));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<TunedModel> Get(string id, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<TunedModel>(kEndpointWithId, this, options, PathParam.ID(id));
        }

        public async UniTask<QueryResponse<TunedModel>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<TunedModel> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<TunedModel>, TunedModel>(kEndpointWithId, this, req);
        }

        /// <summary>
        /// This is a comma-separated list of fully qualified names of fields. Example: "user.displayName,photo".
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateMasks"></param>
        /// <returns></returns>
        public async UniTask<TunedModel> Patch(string id, RESTRequestOptions options = null, params UpdateMask[] updateMasks)
        {
            return await GenerativeAI.CRUD.PatchAsync<TunedModel>(kEndpointWithId, this, options, updateMasks.ToPathParams(PathParam.ID(id)));
        }

        public async UniTask<bool> TransferOwnership(TransferOwnershipRequest req)
        {
            try
            {
                if (req == null) throw new System.Exception("TransferOwnershipRequest is null");
                if (string.IsNullOrEmpty(req.TunedModelId)) throw new System.Exception("TunedModelId is null");
                RESTResponse res = await GenerativeAI.CRUD.CreateAsync(kEndpointWithId, this, req, PathParam.ID(req.TunedModelId), PathParam.Method(Methods.TRANSFER_OWNERSHIP));
                return res?.HasBody ?? false; // TODO: check if the response is empty or not
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return false;
            }
        }
    }
}