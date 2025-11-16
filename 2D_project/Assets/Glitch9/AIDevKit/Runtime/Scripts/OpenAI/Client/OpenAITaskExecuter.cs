using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.RESTApi;
using UnityEngine;

namespace Glitch9.AIDevKit.OpenAI
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class OpenAITaskExecuter : GENTaskExecuter
    {
#if UNITY_EDITOR
        static OpenAITaskExecuter()
        {
            GENTaskManager.RegisterTaskExecuter(AIProvider.OpenAI, new OpenAITaskExecuter());
        }
#else 
        [UnityEngine.RuntimeInitializeOnLoadMethod]
        private static void ResisterTaskExecuter()
        {
            GENTaskManager.RegisterTaskExecuter(AIProvider.OpenAI, new OpenAITaskExecuter());
        }
#endif

        internal override AIProvider Api { get; } = AIProvider.OpenAI;

        internal override async UniTask<GeneratedText> GenerateTextAsync(GENTextTask task, Type jsonSchemaType)
        {
            ChatCompletionRequest req = task.ToChatCompletionRequest(jsonSchemaType, false);
            ChatCompletion result = await req.ExecuteAsync();
            return new(result.ToString(), result.GetToolCalls(), result.Usage);
        }

        internal override async UniTask StreamTextAsync(GENTextTask task, Type jsonSchemaType, ChatStreamHandler streamHandler)
        {
            ChatCompletionRequest req = task.ToChatCompletionRequest(jsonSchemaType, true);
            await req.StreamAsync(streamHandler);
        }

        internal override async UniTask<GeneratedContent> GenerateChatAsync(GENChatTask task)
        {
            ChatCompletionRequest req = task.ToChatCompletionRequest(false);
            ChatCompletion result = await req.ExecuteAsync();
            return new(result.GetContent(), result.GetToolCalls(), result.Usage);
        }

        internal override async UniTask StreamChatAsync(GENChatTask task, ChatStreamHandler streamHandler)
        {
            ChatCompletionRequest req = task.ToChatCompletionRequest(true);
            await req.StreamAsync(streamHandler);
        }

        internal override async UniTask<GeneratedImage> GenerateImageAsync(GENImageCreationTask task)
        {
            ImageCreationRequest.Builder builder = new ImageCreationRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetPrompt(task.promptText)
                .SetN(task.outputCount)
                .SetOutputPath(task.outputPath)
                .SetCancellationToken(task.token);

            ImageSize? size = task.GetSize();
            ImageQuality? quality = task.GetQuality();
            ImageStyle? style = task.GetStyle();

            if (size != null) builder.SetSize(size.Value);
            if (quality != null) builder.SetQuality(quality.Value);
            if (style != null) builder.SetStyle(style.Value);

            GeneratedImage result = await builder.Build().ExecuteAsync();

            if (result != null)
            {
                size ??= OpenAIUtils.GetDefaultImageSize(task.model);
                quality ??= OpenAIUtils.GetDefaultImageQuality(task.model);
                Usage usage = OpenAIUtils.CreateImageUsage(size.Value, quality.Value, task.outputCount);
                result.Usage = usage;
            }

            return result;
        }

        internal override async UniTask<GeneratedImage> GenerateImageEditAsync(GENImageEditTask task)
        {
            ImageEditRequest.Builder builder = new ImageEditRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetImage(task.promptImage)
                .SetModel(task.model.Id)
                .SetPrompt(task.promptText)
                .SetN(task.outputCount)
                .SetCancellationToken(task.token);

            Texture2D mask = task.GetMask();
            ImageSize? size = task.GetSize();

            if (size != null) builder.SetSize(size.Value);
            if (mask != null) builder.SetMask(mask);

            builder.SetOutputPath(task.outputPath);

            GeneratedImage result = await builder.Build().ExecuteAsync();

            if (result != null)
            {
                size ??= OpenAIUtils.GetDefaultImageSize(task.model);
                ImageQuality quality = OpenAIUtils.GetDefaultImageQuality(task.model);
                Usage usage = OpenAIUtils.CreateImageUsage(size.Value, quality, task.outputCount);
                result.Usage = usage;
            }

            return result;
        }

        internal override async UniTask<GeneratedImage> GenerateImageVariationAsync(GENImageVariationTask task)
        {
            ImageVariationRequest.Builder builder = new ImageVariationRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetImage(task.promptImage)
                .SetModel(task.model.Id)
                .SetN(task.outputCount)
                .SetOutputPath(task.outputPath)
                .SetCancellationToken(task.token);

            ImageSize? size = task.GetSize();
            if (size != null) builder.SetSize(size.Value);

            GeneratedImage result = await builder.Build().ExecuteAsync();

            if (result != null)
            {
                size ??= OpenAIUtils.GetDefaultImageSize(task.model);
                ImageQuality quality = OpenAIUtils.GetDefaultImageQuality(task.model);
                Usage usage = OpenAIUtils.CreateImageUsage(size.Value, quality, task.outputCount);
                result.Usage = usage;
            }

            return result;
        }

        internal override async UniTask<GeneratedAudio> GenerateSpeechAsync(GENSpeechTask task)
        {

            SpeechRequest.Builder builder = new SpeechRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetPrompt(task.promptText)
                .SetVoice(task.voice)
                .SetOutputPath(task.outputPath)
                .SetResponseFormat(task.outputMimeType)
                .SetCancellationToken(task.token);

            if (task.speed != null) builder.SetSpeed(task.speed.Value);

            GeneratedAudio result = await builder.Build().ExecuteAsync();

            if (result != null && task.promptText != null)
            {
                Usage usage = Usage.PerCharacter(task.promptText.Length);
                result.Usage = usage;
            }

            return result;
        }

        internal override async UniTask<GeneratedText> GenerateTranscriptAsync(GENTranscriptTask task)
        {
            TranscriptionRequest.Builder builder = new TranscriptionRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetFile(task.promptAudio)
                .SetCancellationToken(task.token);

            if (task.language != null) builder.SetLanguage(task.language.Value);

            Transcription result = await builder.Build().ExecuteAsync();

            return GeneratedText.Transcript(result.ToString());
        }

        internal override async UniTask<GeneratedText> GenerateTranslationAsync(GENTranslationTask task)
        {
            TranslationRequest.Builder builder = new TranslationRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetFile(task.promptAudio)
                .SetCancellationToken(task.token);

            Translation result = await builder.Build().ExecuteAsync();

            return GeneratedText.Transcript(result.ToString());
        }

        internal override async UniTask<QueryResponse<IModelData>> ListModelsAsync(int page, int pageSize)
        {
            QueryResponse<OpenAIModelData> res = await OpenAI.DefaultInstance.Models.List(limit: pageSize);
            return res.ToSoftRef<OpenAIModelData, IModelData>();
        }
    }
}