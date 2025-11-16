using Cysharp.Threading.Tasks;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Extension methods for the all OpenAI requests that
    /// calls OpenAiClient's DefaultInstance to process the request.
    /// </summary>
    public static class RequestExtensions
    {
        public static async UniTask<ChatCompletion> ExecuteAsync(this ChatCompletionRequest request)
        {
            return await OpenAI.DefaultInstance.Chat.Completions.Create(request);
        }

        public static async UniTask StreamAsync(this ChatCompletionRequest request, ChatStreamHandler streamHandler)
        {
            await OpenAI.DefaultInstance.Chat.Completions.Stream(request, streamHandler);
        }

        public static async UniTask<GeneratedImage> ExecuteAsync(this ImageCreationRequest request)
        {
            return await OpenAI.DefaultInstance.Images.Generate(request);
        }

        public static async UniTask<GeneratedImage> ExecuteAsync(this ImageEditRequest request)
        {
            return await OpenAI.DefaultInstance.Images.Edit(request);
        }

        public static async UniTask<GeneratedImage> ExecuteAsync(this ImageVariationRequest request)
        {
            return await OpenAI.DefaultInstance.Images.CreateVariation(request);
        }

        public static async UniTask<GeneratedAudio> ExecuteAsync(this SpeechRequest request)
        {
            return await OpenAI.DefaultInstance.Audio.Speech.Create(request);
        }

        public static async UniTask<Transcription> ExecuteAsync(this TranscriptionRequest request)
        {
            return await OpenAI.DefaultInstance.Audio.Transcriptions.Create(request);
        }

        public static async UniTask<Translation> ExecuteAsync(this TranslationRequest request)
        {
            return await OpenAI.DefaultInstance.Audio.Translations.Create(request);
        }

        public static async UniTask<Moderation> ExecuteAsync(this ModerationRequest request)
        {
            return await OpenAI.DefaultInstance.Moderations.Create(request);
        }

        public static async UniTask<Embedding> ExecuteAsync(this EmbeddingRequest request)
        {
            return await OpenAI.DefaultInstance.Embeddings.Create(request);
        }

        public static async UniTask<FineTuningJob> ExecuteAsync(this FineTuningRequest request)
        {
            return await OpenAI.DefaultInstance.FineTuning.Jobs.Create(request);
        }
    }
}