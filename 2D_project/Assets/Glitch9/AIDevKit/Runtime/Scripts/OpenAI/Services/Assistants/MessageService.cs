using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Messages: https://platform.openai.com/docs/api-reference/messages
    /// </summary>
    public class MessageService : CRUDService<OpenAI>
    {
        private const string kEndpoint = "{ver}/threads/{0}/messages";
        private const string kEndpointWithId = "{ver}/threads/{0}/messages/{1}";

        public MessageService(OpenAI client, params RESTHeader[] extraHeaders) : base(client, extraHeaders)
        {
        }


        public async UniTask<ThreadMessage> Create(string threadId, ThreadMessageRequest req)
        {
            try
            {
                ThrowIf.ArgumentIsNull(req);
                ThrowIf.IsNullOrEmpty(threadId, nameof(threadId));

                if (!req.LocalImages.IsNullOrEmpty())
                {
                    List<string> imageFileIds = new();

                    foreach (UniImageFile image in req.LocalImages)
                    {
                        if (image == null) continue;
                        FileUploadRequest uploadReq = new FileUploadRequest.Builder()
                            .SetImage(image)
                            .Build();

                        File uploadRes = await Client.Files.Upload(uploadReq);
                        if (uploadRes == null) throw new Exception("Failed to upload image file.");
                        imageFileIds.Add(uploadRes.Id);
                    }

                    if (imageFileIds.Count > 0)
                    {
                        req.SetImages(imageFileIds);
                    }
                }

                if (!req.LocalFiles.IsNullOrEmpty())
                {
                    List<string> fileIds = new();

                    foreach (UniFile file in req.LocalFiles)
                    {
                        if (file == null) continue;
                        FileUploadRequest uploadReq = new FileUploadRequest.Builder()
                            .SetFile(file, UploadPurpose.Assistants)
                            .Build();

                        File uploadRes = await Client.Files.Upload(uploadReq) ?? throw new Exception("Failed to upload file.");
                        fileIds.Add(uploadRes.Id);
                    }

                    if (fileIds.Count > 0)
                    {
                        req.SetFiles(fileIds);
                    }
                }

                return await OpenAI.CRUD.CreateAsync<ThreadMessageRequest, ThreadMessage>(kEndpoint, this, req, PathParam.ID(threadId));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<QueryResponse<ThreadMessage>> List(string threadId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            return await OpenAIQuery.CRUD.List<ThreadMessage>(kEndpoint, this, limit, order, cursor, PathParam.ID(threadId));
        }

        public async UniTask<ThreadMessage> Retrieve(string threadId, string messageId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.RetrieveAsync<ThreadMessage>(kEndpointWithId, this, options, PathParam.ID(threadId, messageId));
        }

        public async UniTask<ThreadMessage> Update(string threadId, string messageId, ThreadMessageRequest req)
        {
            return await OpenAI.CRUD.UpdateAsync<ThreadMessageRequest, ThreadMessage>(kEndpointWithId, this, req, PathParam.ID(threadId, messageId));
        }

        public async UniTask<bool> Delete(string threadId, string messageId, RESTRequestOptions options = null)
        {
            return await OpenAI.CRUD.DeleteAsync<ThreadMessage>(kEndpointWithId, this, options, PathParam.ID(threadId, messageId));
        }

        public async UniTask<QueryResponse<MessageFile>> ListFiles(string threadId, string messageId, int limit = OpenAIConfig.kMaxQuery, QueryOrder order = OpenAIConfig.DefaultValues.QUERY_ORDER, QueryCursor cursor = null)
        {
            ThrowIf.IsNullOrEmpty(threadId, nameof(threadId));
            ThrowIf.IsNullOrEmpty(messageId, nameof(messageId));

            return await OpenAIQuery.CRUD.List<MessageFile>(kEndpoint, this, limit, order, cursor, PathParam.ID(threadId, messageId));
        }

        public async UniTask<MessageFile> RetrieveFile(string threadId, string messageId, string fileId, RESTRequestOptions options = null)
        {
            ThrowIf.IsNullOrEmpty(threadId, nameof(threadId));
            ThrowIf.IsNullOrEmpty(messageId, nameof(messageId));
            ThrowIf.IsNullOrEmpty(fileId, nameof(fileId));

            return await OpenAI.CRUD.RetrieveAsync<MessageFile>(kEndpointWithId, this, options, PathParam.ID(threadId, messageId, fileId));
        }
    }
}