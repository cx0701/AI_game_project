using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Fine-tuning Service: https://platform.openai.com/docs/api-reference/fine-tuning
    /// </summary>
    public class FineTuningService : CRUDService<OpenAI>
    {
        public FineTuningJobService Jobs { get; }
        public FineTuningService(OpenAI client) : base(client) => Jobs = new FineTuningJobService(client);
    }

    public class FineTuningJobService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/fine_tuning/jobs";
        private const string kEndpointWithId = "{ver}/fine_tuning/jobs/{0}";
        private const string kEventEndpoint = "{ver}/fine_tuning/jobs/{0}/events/{1}";
        public FineTuningJobService(OpenAI client) : base(client) { }

        public async UniTask<FineTuningJob> Create(FineTuningRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<FineTuningRequest, FineTuningJob>(kEndpoint, this, req);
        }

        public async UniTask<QueryResponse<FineTuningJob>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<FineTuningJob>(kEndpoint, this, limit, order, cursor);
        }

        public async UniTask<FineTuningJob> Retrieve(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<FineTuningJob>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        public async UniTask<FineTuningJob> Cancel(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.CancelAsync<FineTuningJob>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        public async UniTask<QueryResponse<FineTuningEvent>> ListEvents(string fineTuningJobId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<FineTuningEvent>(kEventEndpoint, this, limit, order, cursor, PathParam.ID(fineTuningJobId));
        }
    }
}