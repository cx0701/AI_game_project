
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    public class FileService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/files";
        private const string kEndpointWithId = "{ver}/files/{0}";

        public FileService(GenerativeAI client) : base(client, Beta.FILES)
        {
        }

        public async UniTask<bool> Delete(string fileId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.DeleteAsync<FileResponse>(kEndpointWithId, this, options, PathParam.ID(fileId));
        }

        public async UniTask<FileResponse> Get(string fileId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<FileResponse>(kEndpointWithId, this, options, PathParam.ID(fileId));
        }

        public async UniTask<QueryResponse<File>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<File> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<File>, File>(kEndpoint, this, req);
        }
    }
}