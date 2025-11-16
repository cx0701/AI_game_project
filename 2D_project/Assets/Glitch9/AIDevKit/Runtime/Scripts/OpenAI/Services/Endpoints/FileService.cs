
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Files Service: https://platform.openai.com/docs/api-reference/files
    /// </summary>
    public class FileService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/files";
        private const string kEndpointWithId = "{ver}/files/{0}";
        private const string kFileRefEndpoint = "{ver}/files/{0}/content";

        public FileService(OpenAI client) : base(client) { }

        public async UniTask<QueryResponse<File>> List(int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<File>(kEndpoint, this, limit, order, cursor);
        }

        public async UniTask<bool> Delete(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<File>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        /// <summary>
        /// Request to retrieve a file from the OpenAI API.
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async UniTask<FileRef> RetrieveFileContent(string fileId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<FileRef>(kFileRefEndpoint, this, options, PathParam.ID(fileId));
        }

        public async UniTask<File> Retrieve(string objectId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<File>(kEndpointWithId, this, options, PathParam.ID(objectId));
        }

        // Added 2024-05-23 by Munchkin
        /// <summary>
        /// Request to upload a file to the OpenAI API.
        /// </summary>
        /// <param name="req">
        /// The request object to send to the API.
        /// </param>
        /// <returns>
        /// An object containing the FileId.
        /// </returns>
        public async UniTask<File> Upload(FileUploadRequest req)
        {
            return await OpenAI.CRUD.CreateAsync<FileUploadRequest, File>(kEndpoint, this, req);
        }

        /// <summary>
        /// Request to upload a file to the OpenAI API.
        /// </summary>
        /// <param name="file">
        /// The file to upload.
        /// </param>
        /// <param name="purpose">
        /// The purpose of the file.
        /// </param>
        /// <returns>
        /// An object containing the FileId.
        /// </returns>
        public async UniTask<File> Upload(FormFile file, UploadPurpose purpose)
        {
            FileUploadRequest req = new FileUploadRequest.Builder().SetFile(file).SetPurpose(purpose).Build();
            return await Upload(req);
        }
    }
}