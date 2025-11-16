using System.IO;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    internal static class GENTaskUtil
    {
        internal static AIProvider ResolveApi<TSelf, TResult>(GENTask<TSelf, TResult> task, string modelId)
                where TSelf : GENTask<TSelf, TResult>
        {
            if (task.model == null)
            {
                Model defaultModel = modelId;
                return defaultModel.Api;
            }
            return task.model.Api;
        }

        internal static AIProvider ResolveApi(Model model)
        {
            if (model == null) return AIProvider.None; // default to OpenAI if model is null.
            return model.Api;
        }

        internal static string ResolveKeyword(Model model)
        {
            if (model == null) return "unknown"; // default to unknown if model is null.
            return model.Id;
        }

        internal static AIProvider ResolveLLMApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
             where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultLLM);

        internal static AIProvider ResolveIMGApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
            where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultIMG);

        internal static AIProvider ResolveTTSApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
             where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultTTS);

        internal static AIProvider ResolveSTTApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
            where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultSTT);

        internal static AIProvider ResolveEMBApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
             where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultEMB);

        internal static AIProvider ResolveMODApi<TSelf, TResult>(GENTask<TSelf, TResult> task)
             where TSelf : GENTask<TSelf, TResult> => ResolveApi(task, AIDevKitSettings.DefaultMOD);


        internal static bool TryResolveOutputPath(bool saveOutput, string outputPath, MIMEType outputMimeType, AIProvider api, string keyword, out string resolvedOutputPath)
        {
            resolvedOutputPath = outputPath;

            if (!saveOutput) return false;

            if (string.IsNullOrWhiteSpace(resolvedOutputPath))
            {
                resolvedOutputPath = AIDevKitPath.ResolveOutputDirectory();
            }

            bool hasExtension = Path.HasExtension(resolvedOutputPath);

            if (hasExtension)
            {
                // 사용자가 직접 파일명을 입력했음: 그대로 사용
                return true;
            }

            // 파일 확장자가 없으면, 디렉토리로 간주하고 파일명 생성
            string fileName = AIDevKitPath.ResolveOutputFileName(api, keyword, outputMimeType);
            if (string.IsNullOrEmpty(fileName)) return false;

            resolvedOutputPath = Path.Combine(resolvedOutputPath, fileName);

            AIDevKitDebug.Blue($"Output path: {resolvedOutputPath}");

            return !string.IsNullOrEmpty(resolvedOutputPath);
        }

        internal static bool IsCreatingHistory(IGENTask task)
        {
            if (task == null) return false;
            if (task.enableHistory) return true;
            if (!Application.isPlaying) return true;
            return AIDevKitSettings.PromptHistoryOnRuntime;
        }
    }
}