using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Google.Services
{
    /// <summary>
    /// Service to Query Generative AI Models
    /// Get, List only
    /// </summary>
    public class ModelService : CRUDService<GenerativeAI>
    {
        private const string kEndpoint = "{ver}/models";
        private const string kEndpointWithId = "{ver}/models/{0}";
        public ModelService(GenerativeAI client) : base(client, Beta.MODELS) { }

        public async UniTask<BatchEmbedContentsResponse> BatchEmbedContents(BatchEmbedContentsRequest request)
        {
            if (request.Model == null) request.Model = GenerativeAISettings.DefaultEMB;
            return await GenerativeAI.CRUD.CreateAsync<BatchEmbedContentsRequest, BatchEmbedContentsResponse>(kEndpointWithId, this, request,
                PathParam.Method(Methods.BATCH_EMBED_CONTENTS), PathParam.ID(request.GetModelName()));
        }

        public async UniTask<CountTokensResponse> CountTokens(CountTokensRequest request)
        {
            return await GenerativeAI.CRUD.CreateAsync<CountTokensRequest, CountTokensResponse>(kEndpointWithId, this, request,
                PathParam.Method(Methods.COUNT_TOKENS), PathParam.ID(request.GetModelName()));
        }

        public async UniTask<EmbedContentResponse> EmbedContent(EmbedContentRequest request)
        {
            if (request.Model == null) request.Model = GenerativeAISettings.DefaultEMB;
            return await GenerativeAI.CRUD.CreateAsync<EmbedContentRequest, EmbedContentResponse>(kEndpointWithId, this, request,
                PathParam.Method(Methods.EMBED_CONTENT), PathParam.ID(request.GetModelName()));
        }

        public async UniTask<GenerateAnswerResponse> GenerateAnswer(GenerateAnswerRequest req)
        {
            if (req.Model == null) req.Model = GenerativeAISettings.DefaultLLM;
            return await GenerativeAI.CRUD.CreateAsync<GenerateAnswerRequest, GenerateAnswerResponse>(kEndpointWithId, this, req,
                PathParam.Method(Methods.GENERATE_ANSWER), PathParam.ID(req.GetModelName()));
        }

        public async UniTask<GenerateContentResponse> GenerateContent(GenerateContentRequest req)
        {
            try
            {
                if (req.Model == null) req.Model = GenerativeAISettings.DefaultLLM;
                GenerateContentResponse res = await GenerativeAI.CRUD.CreateAsync<GenerateContentRequest, GenerateContentResponse>(kEndpointWithId,
                    this, req, PathParam.Method(Methods.GENERATE_CONTENT), PathParam.ID(req.GetModelName()));
                Validator.CheckResponse(res, false);
                return res;
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask StreamGenerateContent(GenerateContentRequest req, ChatStreamHandler streamhandler)
        {
            try
            {
                if (req.Model == null) req.Model = GenerativeAISettings.DefaultLLM;

                req.StreamHandler = streamhandler.Initialize(Client.ExtractDelta);
                await GenerativeAI.CRUD.CreateAsync<GenerateContentRequest, GenerateContentResponse>(kEndpointWithId,
                    this, req, PathParam.Method(Methods.STREAM_GENERATE_CONTENT), PathParam.ID(req.GetModelName()));
            }
            catch (Exception e)
            {
                Client.HandleException(e);
            }
        }

        public async UniTask StreamGenerateContent(Model model, string prompt, ChatStreamHandler streamHandler)
        {
            GenerateContentRequest req = new GenerateContentRequest.Builder().SetModel(model).AddContent(ChatRole.User, prompt).Build();
            await StreamGenerateContent(req, streamHandler);
        }

        public async UniTask<GoogleModelData> Get(string modelId, RESTRequestOptions options = null)
        {
            return await GenerativeAI.CRUD.RetrieveAsync<GoogleModelData>(kEndpointWithId, this, options, PathParam.ID(modelId));
        }

        public async UniTask<QueryResponse<GoogleModelData>> List(int pageSize = GoogleAIConfig.kMaxQuery, string pageToken = null)
        {
            QueryRequest<GoogleModelData> req = new(pageSize, pageToken);
            return await GenerativeAI.CRUD.ListAsync<QueryRequest<GoogleModelData>, GoogleModelData>(kEndpoint, this, req);
        }

        // Added 2025.03.30  
        public async UniTask<PredictionsResponse> GenerateImages(GenerateMediaRequest request)
        {
            try
            {
                if (request.Model == null) request.Model = GenerativeAISettings.DefaultIMG;
                PredictionsResponse res = await GenerativeAI.CRUD.CreateAsync<GenerateMediaRequest, PredictionsResponse>(kEndpointWithId,
                      this, request, PathParam.Method(Methods.PREDICT), PathParam.ID(request.GetModelName()));
                res.Validate<PredictionsResponse>();
                return res;
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<GeneratedVideo> GenerateVideos(GenerateMediaRequest request)
        {
            try
            {
                if (request.Model == null) request.Model = GenerativeAISettings.DefaultVID;
                Dictionary<string, object> res = await GenerativeAI.CRUD.CreateAsync<GenerateMediaRequest, Dictionary<string, object>>(kEndpointWithId,
                      this, request, PathParam.Method(Methods.PREDICT_LONG_RUNNING), PathParam.ID(request.GetModelName()));

                if (res == null) return null;

                string operationName = res["name"] as string;
                if (string.IsNullOrEmpty(operationName)) return null;

                string apiKey = GenerativeAISettings.Instance.GetApiKey();

                string pollUrl = $"{GoogleAIConfig.BASE_URL}/{GoogleAIConfig.BETA_VERSION}/{operationName}?key={apiKey}";

                JObject pollRes = await UniTaskPolling.PollAsync(pollUrl, (elapsedTime) =>
                {
                    Client.Logger.Info($"Generating video... {elapsedTime} seconds elapsed.");
                });

                /* Response JSON Example:

                {
                    "name": "models/veo-2.0-generate-001/operations/fwatcyy2qffd",
                    "done": true,
                    "response": {
                        "@type": "type.googleapis.com/google.ai.generativelanguage.v1beta.PredictLongRunningResponse",
                        "generateVideoResponse": {
                        "generatedSamples": [
                            {
                            "video": {
                                "uri": "https://generativelanguage.googleapis.com/v1beta/files/w2j2gmodxzib:download?alt=media"
                            }
                            }
                        ]
                        }
                    }
                }

                */

                if (pollRes == null) return null;

                List<string> urls = new();

                foreach (var sample in pollRes["response"]["generateVideoResponse"]["generatedSamples"])
                {
                    string videoUri = sample["video"]["uri"].ToString();
                    if (string.IsNullOrEmpty(videoUri)) continue;
                    urls.Add($"{videoUri}&key={apiKey}");
                }

                return await GeneratedVideo.CreateAsync(urls, request.OutputPath, request.Model, MIMEType.MP4, null);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }
    }
}
