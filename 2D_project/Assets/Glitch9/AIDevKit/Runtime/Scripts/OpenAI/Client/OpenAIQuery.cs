using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    public class OpenAIQuery
    {
        public static class CRUD
        {
            public static async UniTask<QueryResponse<T>> List<T>(
                string endpoint,
                CRUDService<OpenAI> service,
                int limit = OpenAIConfig.kMaxQuery,
                QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER,
                QueryCursor cursor = null,
                params IPathParam[] pathParams)
                where T : class, new()
            {
                QueryRequest<T> query = new QueryRequest<T>.Builder()
                    .SetLimit(limit)
                    .SetOrder(order)
                    .SetCursor(cursor)
                    .Build();

                return await OpenAI.CRUD.ListAsync<QueryRequest<T>, T>(endpoint, service, query, pathParams);
            }
        }
    }
}