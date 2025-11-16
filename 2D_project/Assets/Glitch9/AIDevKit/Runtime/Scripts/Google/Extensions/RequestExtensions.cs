using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
    public static class RequestExtensions
    {
        public static string GetModelName(this GenerativeAIRequest request)
        {
            string modelName = request.Model.Id;
            if (modelName.Contains('/')) modelName = modelName.Split('/')[1];
            return modelName;
        }

        public static Dataset AddTrainingData(this Dataset dataset, params TuningExample[] trainingData)
        {
            dataset ??= new Dataset();
            dataset.AddRange(trainingData);
            return dataset;
        }

        public static async UniTask<GenerateContentResponse> ExecuteAsync(this GenerateContentRequest request)
        {
            return await GenerativeAI.DefaultInstance.Models.GenerateContent(request);
        }

        public static async UniTask StreamAsync(this GenerateContentRequest request, ChatStreamHandler streamHandler)
        {
            await GenerativeAI.DefaultInstance.Models.StreamGenerateContent(request, streamHandler);
        }

        // Added 2025.03.30
        public static async UniTask<PredictionsResponse> GenerateImagesAsync(this GenerateMediaRequest request)
        {
            return await GenerativeAI.DefaultInstance.Models.GenerateImages(request);
        }

        public static async UniTask<GeneratedVideo> GenerateVideosAsync(this GenerateMediaRequest request)
        {
            return await GenerativeAI.DefaultInstance.Models.GenerateVideos(request);
        }
    }
}