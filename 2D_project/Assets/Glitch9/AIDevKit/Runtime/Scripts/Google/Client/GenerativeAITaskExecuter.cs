using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.Google
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class GenerativeAITaskExecuter : GENTaskExecuter
    {
#if UNITY_EDITOR 
        static GenerativeAITaskExecuter()
        {
            GENTaskManager.RegisterTaskExecuter(AIProvider.Google, new GenerativeAITaskExecuter());
        }
#else 
        [UnityEngine.RuntimeInitializeOnLoadMethod]
        private static void ResisterTaskExecuter()
        {
            GENTaskManager.RegisterTaskExecuter(AIProvider.Google, new GenerativeAITaskExecuter());
        }
#endif

        internal override AIProvider Api => AIProvider.Google;

        internal override async UniTask<GeneratedText> GenerateTextAsync(GENTextTask task, Type jsonSchemaType)
        {
            GenerateContentRequest req = CreateTextRequest(task, jsonSchemaType);
            GenerateContentResponse result = await req.ExecuteAsync();
            return new(result.ToString(), result.GetToolCalls(), result.Usage);
        }

        internal override async UniTask StreamTextAsync(GENTextTask task, Type jsonSchemaType, ChatStreamHandler streamHandler)
        {
            GenerateContentRequest req = CreateTextRequest(task, jsonSchemaType);
            await req.StreamAsync(streamHandler);
        }

        internal override async UniTask<GeneratedContent> GenerateChatAsync(GENChatTask task)
        {
            GenerateContentRequest req = CreateChatRequest(task);
            GenerateContentResponse result = await req.ExecuteAsync();
            return new(result.ToChatContent(), result.GetToolCalls(), result.Usage);
        }

        internal override async UniTask StreamChatAsync(GENChatTask task, ChatStreamHandler streamHandler)
        {
            GenerateContentRequest req = CreateChatRequest(task);
            await req.StreamAsync(streamHandler);
        }

        internal override async UniTask<GeneratedImage> GenerateImageAsync(GENImageCreationTask task)
        {
            if (task.model.Family == ModelFamily.Imagen)
            {
                GenerateMediaRequest.Builder builder = new GenerateMediaRequest.Builder()
                    .SetSender(task.sender)
                    .SetIgnoreLogs(task.ignoreLogs)
                    .SetModel(task.model.Id)
                    .SetPrompt(task.promptText)
                    .SetNumberOfImages(task.outputCount)
                    .SetOutputPath(task.outputPath)
                    .SetCancellationToken(task.token);

                AspectRatio? aspectRatio = task.GetAspectRatio();
                PersonGeneration? personGeneration = task.GetPersonGeneration();

                if (aspectRatio != null) builder.SetAspectRatio(aspectRatio.Value);
                if (personGeneration != null) builder.SetPersonGeneration(personGeneration.Value);

                PredictionsResponse result = await builder.Build().GenerateImagesAsync();

                if (result == null || result.GeneratedImages.IsNullOrEmpty()) throw new EmptyResponseException(task.model);

                return await result.ToGeneratedImageAsync(task.outputPath);
            }
            else
            {
                GenerateContentRequest.Builder builder = new GenerateContentRequest.Builder()
                    .SetSender(task.sender)
                    .SetIgnoreLogs(task.ignoreLogs)
                    .SetModel(task.model.Id)
                    .SetPrompt(task.promptText)
                    .SetResponseModalities(Modality.Text, Modality.Image)
                    .SetResponseCount(task.outputCount)
                    .SetOutputPath(task.outputPath)
                    .SetCancellationToken(task.token);

                GenerateContentResponse result = await builder.Build().ExecuteAsync();

                if (result == null || result.Candidates.IsNullOrEmpty()) throw new EmptyResponseException(task.model);

                return await result.ToGeneratedImageAsync(task.outputPath);
            }
        }

        internal override async UniTask<GeneratedImage> GenerateImageEditAsync(GENImageEditTask task)
        {
            if (task.model.Family == ModelFamily.Imagen)
            {
                throw new NotImplementedException($"Model {task.model.Id} does not support image editing.");
            }
            else
            {
                GenerateContentRequest.Builder builder = new GenerateContentRequest.Builder()
                    .SetSender(task.sender)
                    .SetIgnoreLogs(task.ignoreLogs)
                    .SetImageToEdit(task.promptText, task.promptImage)
                    .SetModel(task.model.Id)
                    .SetResponseModalities(Modality.Text, Modality.Image)
                    .SetResponseCount(task.outputCount)
                    .SetOutputPath(task.outputPath)
                    .SetCancellationToken(task.token);

                GenerateContentResponse result = await builder.Build().ExecuteAsync();

                if (result == null || result.Candidates.IsNullOrEmpty()) throw new EmptyResponseException(task.model);

                return await result.ToGeneratedImageAsync(task.outputPath);
            }
        }

        internal override async UniTask<GeneratedImage> GenerateImageVariationAsync(GENImageVariationTask task)
        {
            if (task.model.Family == ModelFamily.Imagen)
            {
                throw new NotImplementedException($"Model {task.model.Id} does not support image editing.");
            }
            else
            {
                string promptText = "Generate image similar to the provided image.";

                GenerateContentRequest.Builder builder = new GenerateContentRequest.Builder()
                    .SetSender(task.sender)
                    .SetIgnoreLogs(task.ignoreLogs)
                    .SetImageToEdit(promptText, task.promptImage)
                    .SetModel(task.model.Id)
                    .SetResponseModalities(Modality.Text, Modality.Image)
                    .SetResponseCount(task.outputCount)
                    .SetOutputPath(task.outputPath)
                    .SetCancellationToken(task.token);

                GenerateContentResponse result = await builder.Build().ExecuteAsync();

                if (result == null || result.Candidates.IsNullOrEmpty()) throw new EmptyResponseException(task.model);

                return await result.ToGeneratedImageAsync(task.outputPath);
            }
        }

        // AspectRadio: Supported values are "16:9" and "9:16". The default is "16:9". 
        internal override async UniTask<GeneratedVideo> GenerateVideoAsync(GENVideoTask task)
        {
            GenerateMediaRequest.Builder builder = new GenerateMediaRequest.Builder()
                   .SetSender(task.sender)
                   .SetIgnoreLogs(task.ignoreLogs)
                   .SetModel(task.model.Id)
                   .SetPrompt(task.promptText)
                   .SetNumberOfImages(task.outputCount)
                   .SetOutputPath(task.outputPath)
                   .SetCancellationToken(task.token);

            AspectRatio? aspectRatio = task.GetAspectRatio();
            PersonGeneration? personGeneration = task.GetPersonGeneration();

            if (aspectRatio != null)
            {
                if (aspectRatio.Value != AspectRatio.Vertical && aspectRatio.Value != AspectRatio.Horizontal)
                {
                    //throw new ArgumentOutOfRangeException(nameof(aspectRatio), "Aspect ratio must be either 16:9 (Horizontal) or 9:16 (Vertical).");
                    builder.SetAspectRatio(AspectRatio.Horizontal); // 기본값으로 설정
                }
                else
                {
                    builder.SetAspectRatio(aspectRatio.Value);
                }
            }
            if (personGeneration != null) builder.SetPersonGeneration(personGeneration.Value);

            GeneratedVideo result = await builder.Build().GenerateVideosAsync();


            return result; // 리턴 형태를 알수없음으로 테스팅 필요 
        }

        private GenerateContentRequest CreateTextRequest(GENTextTask task, Type jsonSchemaType)
        {
            var builder = new GenerateContentRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetInstruction(task.instruction)
                .SetPrompt(task.promptText)
                .SetJsonSchema(jsonSchemaType)
                .SetCancellationToken(task.token);

            // if (task.temperature != null) builder.SetTemperature(task.temperature.Value);
            // if (task.topP != null) builder.SetTopP(task.topP.Value);
            // if (task.frequencyPenalty != null) builder.SetFrequencyPenalty(task.frequencyPenalty.Value);
            // if (task.presencePenalty != null) builder.SetPresencePenalty(task.presencePenalty.Value);
            // if (task.maxTokens != null) builder.SetMaxOutputTokens(task.maxTokens.Value);
            // if (task.seed != null) builder.SetSeed(task.seed.Value);

            return builder.Build();
        }

        private GenerateContentRequest CreateChatRequest(GENChatTask task)
        {
            var builder = new GenerateContentRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetMessages(task.session.Messages)
                .SetInstruction(task.session.SystemInstruction)
                .SetStartingMessage(task.session.StartingMessage)
                //.SetIncludeUsage(task.session.IncludeUsage)
                .SetCancellationToken(task.token);

            if (!task.functions.IsNullOrEmpty()) builder.SetFunctions(task.functions);
            // if (task.session.Temperature != null) builder.SetTemperature(task.session.Temperature.Value);
            // if (task.session.TopP != null) builder.SetTopP(task.session.TopP.Value);
            // if (task.session.FrequencyPenalty != null) builder.SetFrequencyPenalty(task.session.FrequencyPenalty.Value);
            // if (task.session.PresencePenalty != null) builder.SetPresencePenalty(task.session.PresencePenalty.Value);
            // if (task.session.MaxTokens != null) builder.SetMaxOutputTokens(task.session.MaxTokens.Value);
            // if (task.session.Seed != null) builder.SetSeed(task.session.Seed.Value);

            return builder.Build();
        }

        internal override async UniTask<QueryResponse<IModelData>> ListModelsAsync(int page, int pageSize)
        {
            QueryResponse<GoogleModelData> res = await GenerativeAI.DefaultInstance.Models.List(pageSize: pageSize);
            return res.ToSoftRef<GoogleModelData, IModelData>();
        }
    }
}