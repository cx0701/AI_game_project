using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    public class RunStepService : CRUDService<OpenAI>
    {
        private const string kListEndpoint = "{ver}/threads/{0}/runs/{1}/steps";
        private const string kGetEndpoint = "{ver}/threads/{0}/runs/{1}/steps/{2}";

        public RunStepService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
        }

        public async UniTask<RunStep[]> List(string threadId, string runId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            QueryResponse<RunStep> queryResponse = await OpenAIQuery.CRUD.List<RunStep>(kListEndpoint, this, limit, order, cursor, PathParam.ID(threadId, runId));
            return queryResponse?.Data;
        }

        public async UniTask<RunStep> Retrieve(string threadId, string runId, string stepId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<RunStep>(kGetEndpoint, this, options, PathParam.ID(threadId, runId, stepId));
        }
    }
}
