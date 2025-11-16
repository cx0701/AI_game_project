using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.RESTApi;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// Fluent API for generating text and images using all generative AI services supported by AIDevKit.
    /// </summary>
    internal static partial class GENTaskManager
    {
        private static readonly Dictionary<AIProvider, GENTaskExecuter> _taskExecuters = new();

        #region Global Internal Method [IMPORTANT]

        // Register executers from each assembly by 
        // - Editor: using static constructor 
        // - Runtime: using [UnityEngine.RuntimeInitializeOnLoadMethod]
        internal static void RegisterTaskExecuter(AIProvider provider, GENTaskExecuter executer)
        {
            if (_taskExecuters.ContainsKey(provider))
            {
                Debug.LogWarning($"Task executer for {provider} is already registered.");
                return;
            }

            _taskExecuters.Add(provider, executer);
        }

        #endregion

        #region Utility Method
        private static GENTaskExecuter GetTaskExecuter(AIProvider provider)
        {
            if (_taskExecuters.TryGetValue(provider, out var executer)) return executer;

            throw new NotImplementedException(
                $"No task executer found for provider {provider}." +
                $"\n - The module for {provider} may not be implemented yet." +
                $"\n - You don't have the {provider} module installed." +
                $"\n - It's some unexpected error.");
        }

        #endregion

        internal static async UniTask<GeneratedText> GenerateTextAsync(GENTextTask task, Type jsonSchemaType)
        {
            AIProvider api = GENTaskUtil.ResolveLLMApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedText result = await executer.GenerateTextAsync(task, jsonSchemaType);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask StreamTextAsync(GENTextTask task, Type jsonSchemaType, ChatStreamHandler streamHandler)
        {
            AIProvider api = GENTaskUtil.ResolveLLMApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            await executer.StreamTextAsync(task, jsonSchemaType, streamHandler);
        }

        internal static async UniTask<GeneratedContent> GenerateChatAsync(GENChatTask task)
        {
            AIProvider api = GENTaskUtil.ResolveLLMApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedContent result = await executer.GenerateChatAsync(task);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask StreamChatAsync(GENChatTask task, ChatStreamHandler streamHandler)
        {
            AIProvider api = GENTaskUtil.ResolveLLMApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            await executer.StreamChatAsync(task, streamHandler);
        }

        internal static async UniTask<GeneratedImage> GenerateImageAsync(GENImageCreationTask task)
        {
            AIProvider api = GENTaskUtil.ResolveIMGApi(task);
            task.ResolveOutputPath();
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedImage result = await executer.GenerateImageAsync(task);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask<GeneratedImage> GenerateImageEditAsync(GENImageEditTask task)
        {
            AIProvider api = GENTaskUtil.ResolveIMGApi(task);
            task.ResolveOutputPath();
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedImage result = await executer.GenerateImageEditAsync(task);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask<GeneratedImage> GenerateImageVariationAsync(GENImageVariationTask task)
        {
            AIProvider api = GENTaskUtil.ResolveIMGApi(task);
            task.ResolveOutputPath();
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedImage result = await executer.GenerateImageVariationAsync(task);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask<GeneratedAudio> GenerateSpeechAsync(GENSpeechTask task)
        {
            AIProvider api = GENTaskUtil.ResolveTTSApi(task);
            task.ResolveOutputPath();
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedAudio result = await executer.GenerateSpeechAsync(task);
            if (GENTaskUtil.IsCreatingHistory(task)) GENTaskRecord.Create(task, result);
            return result;
        }

        internal static async UniTask StreamSpeechAsync(GENSpeechTask task, StreamAudioPlayer streamAudioPlayer)
        {
            AIProvider api = GENTaskUtil.ResolveTTSApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            await executer.StreamSpeechAsync(task, streamAudioPlayer);
        }

        internal static async UniTask<GeneratedText> GenerateTranscriptionAsync(GENTranscriptTask task)
        {
            AIProvider api = GENTaskUtil.ResolveSTTApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedText result = await executer.GenerateTranscriptAsync(task);
            return result;
        }

        internal static async UniTask<GeneratedText> GenerateTranslationAsync(GENTranslationTask task)
        {
            AIProvider api = GENTaskUtil.ResolveSTTApi(task);
            GENTaskExecuter executer = GetTaskExecuter(api);
            GeneratedText result = await executer.GenerateTranslationAsync(task);
            return result;
        }

        internal static async UniTask<GeneratedAudio> GenerateSoundEffectAsync(GENSoundEffectTask task)
        {
            task.ResolveOutputPath(AIProvider.ElevenLabs, "sfx");
            GENTaskExecuter executer = GetTaskExecuter(AIProvider.ElevenLabs);
            return await executer.GenerateSoundEffectAsync(task);
        }

        internal static async UniTask<GeneratedAudio> GenerateVoiceChangeAsync(GENVoiceChangeTask task)
        {
            if (task.promptAudio == null) throw new ArgumentNullException(nameof(task.promptAudio), "Input audio clip is null.");
            task.ResolveOutputPath(AIProvider.ElevenLabs, "voice_change");
            GENTaskExecuter executer = GetTaskExecuter(AIProvider.ElevenLabs);
            return await executer.GenerateVoiceChangeAsync(task);
        }

        internal static async UniTask<GeneratedAudio> GenerateAudioIsolationAsync(GENAudioIsolationTask task)
        {
            if (task.promptAudio == null) throw new ArgumentNullException(nameof(task.promptAudio), "Input audio clip is null.");
            task.ResolveOutputPath(AIProvider.ElevenLabs, "isolation");
            GENTaskExecuter executer = GetTaskExecuter(AIProvider.ElevenLabs);
            return await executer.GenerateAudioIsolationAsync(task);
        }

        // Added new on 2025.05.05
        internal static async UniTask<GeneratedVideo> GenerateVideoAsync(GENVideoTask task)
        {
            GENTaskExecuter executer = GetTaskExecuter(AIProvider.Google);
            return await executer.GenerateVideoAsync(task);
        }

        // Added new on 2025.05.11
        internal static async UniTask<QueryResponse<IModelData>> ListModelsAsync(AIProvider api, int page, int pageSize)
        {
            GENTaskExecuter executer = GetTaskExecuter(api);
            return await executer.ListModelsAsync(page, pageSize);
        }

        internal static async UniTask<QueryResponse<IModelData>> ListCustomModelsAsync(AIProvider api, int page, int pageSize)
        {
            GENTaskExecuter executer = GetTaskExecuter(api);
            return await executer.ListCustomModelsAsync(page, pageSize);
        }

        internal static async UniTask<QueryResponse<IVoiceData>> ListVoicesAsync(AIProvider api, int page, int pageSize)
        {
            GENTaskExecuter executer = GetTaskExecuter(api);
            return await executer.ListVoicesAsync(page, pageSize);
        }

        internal static async UniTask<QueryResponse<IVoiceData>> ListCustomVoicesAsync(AIProvider api, int page, int pageSize)
        {
            GENTaskExecuter executer = GetTaskExecuter(api);
            return await executer.ListCustomVoicesAsync(page, pageSize);
        }
    }
}