using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    public partial class BetaService
    {
        /// <summary>
        /// Assistants: https: //platform.openai.com/docs/api-reference/assistants
        /// </summary>
        public class AssistantService : CRUDService<OpenAI>
        {
            private const string kEndpoint = "{ver}/assistants";
            private const string kEndpointWithId = "{ver}/assistants/{0}";

            public AssistantService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
            {
            }

            public async UniTask<Assistant> Create(AssistantRequest req)
            {
                return await OpenAI.CRUD.CreateAsync<AssistantRequest, Assistant>(kEndpoint, this, req);
            }

            public async UniTask<Assistant> Retrieve(string objectId, RESTRequestOptions options = null)
            {
                return await OpenAI.CRUD.RetrieveAsync<Assistant>(kEndpointWithId, this, options, PathParam.ID(objectId));
            }

            public async UniTask<Assistant> Update(string objectId, AssistantRequest req)
            {
                return await OpenAI.CRUD.UpdateAsync<AssistantRequest, Assistant>(kEndpointWithId, this, req, PathParam.ID(objectId));
            }

            public async UniTask<QueryResponse<Assistant>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
            {
                return await OpenAIQuery.CRUD.List<Assistant>(kEndpoint, this, limit, order, cursor);
            }

            public async UniTask<bool> Delete(string objectId, RESTRequestOptions options = null)
            {
                return await OpenAI.CRUD.DeleteAsync<Assistant>(kEndpointWithId, this, options, PathParam.ID(objectId));
            }
        }
    }
}