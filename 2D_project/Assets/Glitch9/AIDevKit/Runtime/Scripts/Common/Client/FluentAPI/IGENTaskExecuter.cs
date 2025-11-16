using System;
using Cysharp.Threading.Tasks;
using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit
{
    public abstract class GENTaskExecuter
    {
        internal abstract AIProvider Api { get; }
        internal virtual UniTask<GeneratedText> GenerateTextAsync(GENTextTask task, Type jsonSchemaType) => throw new UnsupportedGENTaskException(Api, GENTaskType.Completion);
        internal virtual UniTask StreamTextAsync(GENTextTask task, Type jsonSchemaType, ChatStreamHandler streamHandler) => throw new UnsupportedGENTaskException(Api, GENTaskType.Completion);
        internal virtual UniTask<GeneratedContent> GenerateChatAsync(GENChatTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.ChatCompletion);
        internal virtual UniTask StreamChatAsync(GENChatTask task, ChatStreamHandler streamHandler) => throw new UnsupportedGENTaskException(Api, GENTaskType.ChatCompletion);
        internal virtual UniTask<GeneratedImage> GenerateImageAsync(GENImageCreationTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.ImageCreation);
        internal virtual UniTask<GeneratedImage> GenerateImageEditAsync(GENImageEditTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.ImageEdit);
        internal virtual UniTask<GeneratedImage> GenerateImageVariationAsync(GENImageVariationTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.ImageVariation);
        internal virtual UniTask<GeneratedAudio> GenerateSpeechAsync(GENSpeechTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.Speech);
        internal virtual UniTask StreamSpeechAsync(GENSpeechTask task, StreamAudioPlayer streamAudioPlayer) => throw new UnsupportedGENTaskException(Api, GENTaskType.Speech);
        internal virtual UniTask<GeneratedText> GenerateTranscriptAsync(GENTranscriptTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.Transcript);
        internal virtual UniTask<GeneratedText> GenerateTranslationAsync(GENTranslationTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.Translation);
        internal virtual UniTask<GeneratedAudio> GenerateSoundEffectAsync(GENSoundEffectTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.SoundEffect);
        internal virtual UniTask<GeneratedAudio> GenerateVoiceChangeAsync(GENVoiceChangeTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.VoiceChange);
        internal virtual UniTask<GeneratedAudio> GenerateAudioIsolationAsync(GENAudioIsolationTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.AudioIsolation);
        internal virtual UniTask<GeneratedVideo> GenerateVideoAsync(GENVideoTask task) => throw new UnsupportedGENTaskException(Api, GENTaskType.VideoGeneration);

        // internal
        internal virtual UniTask<QueryResponse<IModelData>> ListModelsAsync(int page, int pageSize) => throw new UnsupportedGENTaskException(Api, GENTaskType.ListModels);
        internal virtual UniTask<QueryResponse<IVoiceData>> ListVoicesAsync(int page, int pageSize) => throw new UnsupportedGENTaskException(Api, GENTaskType.ListVoices);
        internal virtual UniTask<QueryResponse<IModelData>> ListCustomModelsAsync(int page, int pageSize) => throw new UnsupportedGENTaskException(Api, GENTaskType.ListCustomModels);
        internal virtual UniTask<QueryResponse<IVoiceData>> ListCustomVoicesAsync(int page, int pageSize) => throw new UnsupportedGENTaskException(Api, GENTaskType.ListCustomVoices);
    }
}