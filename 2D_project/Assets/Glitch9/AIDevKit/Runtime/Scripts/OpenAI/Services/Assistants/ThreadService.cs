using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Threads: https://platform.openai.com/docs/api-reference/threads
    /// </summary>
    public class ThreadService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/threads";
        private const string kEndpointWithId = "{ver}/threads/{0}";

        /// <summary>
        /// Create messages within threads
        /// </summary>
        public MessageService Messages { get; }

        /// <summary>
        /// Represents an execution run on a thread.
        /// </summary>
        public RunService Runs { get; }

        public ThreadService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
            Messages = new MessageService(client, extraHeaders);
            Runs = new RunService(client, extraHeaders);
        }

        public async UniTask<Thread> Create(ThreadRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<ThreadRequest, Thread>(kEndpoint, this, req);
        }

        public async UniTask<Thread> Retrieve(string threadId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<Thread>(kEndpointWithId, this, options, PathParam.ID(threadId));
        }

        public async UniTask<Thread> Update(string threadId, ThreadRequest req)
        {
            return await OpenAI.CRUD.UpdateAsync<ThreadRequest, Thread>(kEndpointWithId, this, req, PathParam.ID(threadId));
        }

        public async UniTask<QueryResponse<Thread>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<Thread>(kEndpoint, this, limit, order, cursor);
        }

        public async UniTask<bool> Delete(string threadId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<Thread>(kEndpointWithId, this, options, PathParam.ID(threadId));
        }

        public async UniTask<Thread> Create(List<ThreadMessage> messages = null, Dictionary<string, string> metadata = null)
        {
            try
            {
                ThreadRequest req = new ThreadRequest.Builder().SetMessages(messages).SetMetadata(metadata).Build();
                return await OpenAI.CRUD.CreateAsync<ThreadRequest, Thread>(kEndpoint, this, req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<Thread> Update(string threadId, Dictionary<string, string> metadata)
        {
            try
            {
                ThrowIf.IsNullOrEmpty(threadId, nameof(threadId));
                ThrowIf.CollectionIsNullOrEmpty(metadata, nameof(metadata));

                ThreadRequest req = new ThreadRequest.Builder().SetMetadata(metadata).Build();
                return await OpenAI.CRUD.UpdateAsync<ThreadRequest, Thread>(kEndpointWithId, this, req, PathParam.ID(threadId));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }
    }
}