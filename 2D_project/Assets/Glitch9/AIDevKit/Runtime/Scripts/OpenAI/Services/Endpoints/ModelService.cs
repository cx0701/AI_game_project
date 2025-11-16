using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Models Service: https://platform.openai.com/docs/api-reference/models
    /// </summary>
    public class ModelService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/models";
        private const string kEndpointWithId = "{ver}/models/{0}";
        public ModelService(OpenAI client) : base(client) { }

        /// <summary>
        /// Lists the currently available models, and provides basic information about each one such as the owner and availability.
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="order"></param>
        /// <param name="cursor"></param>
        /// <returns></returns>
        public async UniTask<QueryResponse<OpenAIModelData>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<OpenAIModelData>(kEndpoint, this, limit, order, cursor);
        }

        /// <summary>
        /// Retrieves a model instance, providing basic information about the model such as the owner and permissioning.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async UniTask<OpenAIModelData> Retrieve(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<OpenAIModelData>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }


        /// <summary>
        /// Delete a fine-tuned model. You must have the Owner role in your organization to delete a model.
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        public async UniTask<bool> Delete(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<OpenAIModelData>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }
    }
}