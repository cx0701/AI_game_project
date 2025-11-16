using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    public class MediaService : CRUDService<GenerativeAI>
    {
        internal const string kUploadEndpoint = "upload/{ver}/files";
        internal const string kUploadMetadataEndpoint = "{ver}/files";

        public MediaService(GenerativeAI client) : base(client, Beta.MEDIA_UPLOAD)
        {
        }

        public async UniTask<FileResponse> Upload(UploadFileRequest req)
        {
            return await GenerativeAI.CRUD.CreateAsync<UploadFileRequest, FileResponse>(kUploadEndpoint, this, req);
        }

        public async UniTask<FileResponse> UploadMetadata(UploadFileRequest req)
        {
            return await GenerativeAI.CRUD.CreateAsync<UploadFileRequest, FileResponse>(kUploadMetadataEndpoint, this, req);
        }
    }
}