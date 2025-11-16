using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Run: https://platform.openai.com/docs/api-reference/runs
    /// </summary>
    public class RunService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/threads/{0}/runs";
        private const string kEndpointWithId = "{ver}/threads/{0}/runs/{1}";
        private const string kCancelEndpoint = "{ver}/threads/{0}/runs/{1}/cancel";

        /// <summary>
        /// Represents the steps (model and tool calls) taken during the run.
        /// </summary>
        public RunStepService Steps { get; }

        public RunService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
            Steps = new RunStepService(client, extraHeaders);
        }

        public async UniTask<Run> Create(string threadId, RunRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<RunRequest, Run>(kEndpoint, this, req, PathParam.ID(threadId));
        }

        public async UniTask<QueryResponse<Run>> List(string threadId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<Run>(kEndpoint, this, limit, order, cursor, PathParam.ID(threadId));
        }


        public async UniTask<Run> Retrieve(string threadId, string runId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<Run>(kEndpointWithId, this, options, PathParam.ID(threadId, runId));
        }


        public async UniTask<Run> Update(string threadId, string runId, RunRequest req)
        {
            return await OpenAI.CRUD.UpdateAsync<RunRequest, Run>(kEndpointWithId, this, req, PathParam.ID(threadId, runId));
        }

        public async UniTask<Run> Cancel(string threadId, string runId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.CancelAsync<Run>(kCancelEndpoint, this, options, PathParam.ID(threadId, runId));
        }


        public async UniTask<Run> SubmitToolOutputsToRun(string threadId, string runId, ToolOutputsRequest req)
        {
            try
            {
                const string CHILD_PATH = "submit_tool_outputs";
                ThrowIf.ArgumentIsNull(req);
                ThrowIf.IsNullOrEmpty(threadId, nameof(threadId));
                ThrowIf.IsNullOrEmpty(runId, nameof(runId));

                return await OpenAI.CRUD.CreateAsync<ToolOutputsRequest, Run>(kEndpointWithId, this, req, PathParam.ID(threadId, runId), PathParam.Child(CHILD_PATH));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }
    }
}
