using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Batch Service: https://platform.openai.com/docs/api-reference/batch
    /// </summary>
    public class BatchService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/batches";
        private const string kEndpointWithId = "{ver}/batches/{0}";

        public BatchService(OpenAI client) : base(client) { }

        public async UniTask<Batch> Create(BatchRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<BatchRequest, Batch>(kEndpoint, this, req);
        }

        public async UniTask<Batch> Retrieve(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<Batch>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        public async UniTask<Batch> Cancel(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.CancelAsync<Batch>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        public async UniTask<QueryResponse<Batch>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<Batch>(kEndpoint, this, limit, order, cursor);
        }
    }
}