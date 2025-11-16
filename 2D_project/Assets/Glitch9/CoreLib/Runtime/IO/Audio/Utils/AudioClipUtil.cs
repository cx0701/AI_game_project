using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using UnityEngine;
using UnityEngine.Networking;

namespace Glitch9.CoreLib.IO.Audio
{
    public static class AudioClipUtil
    {
        public static async UniTask<AudioClip> CreateAsync(string absolutePathOrUrl, PathType? pathType = null)
        {
            if (string.IsNullOrEmpty(absolutePathOrUrl)) return null;

            pathType ??= PathUtil.ResolveType(absolutePathOrUrl);

            if (pathType == PathType.Unknown)
            {
                Debug.LogError($"Unknown path type for file path: {absolutePathOrUrl}");
                return null;
            }

            if (pathType == PathType.Resources)
            {
                ResourceRequest request = Resources.LoadAsync<AudioClip>(absolutePathOrUrl);
                await request.ToUniTask();
                if (request.asset is AudioClip audioClip) return audioClip;
                return null; // 혹은 오류 처리
            }

            if (pathType == PathType.Url)
            {
                return await LoadAsync(absolutePathOrUrl);
            }

            absolutePathOrUrl = absolutePathOrUrl.ToAbsolutePath();
            return await LoadAsync(absolutePathOrUrl);
        }

        public static async UniTask<AudioClip> LoadAsync(string absolutePathOrUrl, AudioType? audioType = null)
        {
#if UNITY_EDITOR
            if (absolutePathOrUrl.Contains(Application.dataPath))
            {
                string relativePath = absolutePathOrUrl.ToRelativePath();
                AudioClip clip = UnityEditor.AssetDatabase.LoadAssetAtPath<AudioClip>(relativePath);
                if (clip != null) return clip;
                Debug.LogWarning($"AudioClip not found in project at path: {relativePath}");
                return null;
            }
#endif

            if (absolutePathOrUrl.Contains(Application.dataPath) && absolutePathOrUrl.Contains("Resources"))
            {
                string resourcesPath = ExtractResourcesPath(absolutePathOrUrl);
                AudioClip clip = Resources.Load<AudioClip>(resourcesPath);
                if (clip != null) return clip;
                Debug.LogWarning($"AudioClip not found in Resources at path: {resourcesPath}");
                return null;
            }

            absolutePathOrUrl = absolutePathOrUrl.ToFilePath();

            audioType ??= UnityAudioTypeUtil.ParseFromPath(absolutePathOrUrl);

            using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(absolutePathOrUrl, audioType.Value);
            await www.SendWebRequest().WithCancellation(CancellationToken.None); // SendWebRequest 대신 await를 사용합니다. 

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
                return null;
            }
            else
            {
                return DownloadHandlerAudioClip.GetContent(www);  // 성공적으로 로드된 경우, 다운로드된 AudioClip을 반환합니다.
            }
        }

        // WebGL 이외 플랫폼용 MPEG 디코딩 처리 (임시파일로 저장 후 AudioClip으로 변환)
        // outputPath가 존재한다면, 파일을 저장하고 싶다는 의미임으로 temp폴더가 아니라 지정된 outputPath에 저장하도록 한다.
        internal static async UniTask<UniAudioFile> NonWebGLDecodeAsyncINTERNAL(byte[] audioBytes, string outputPath, AudioType audioType)
        {
            //Debug.Log("Trying to decode MPEG audio file from byte array.");  
            outputPath = RESTApiUtils.ResolveOutputPath(outputPath, audioType);

            // MP3 파일로 저장
            await AudioEncoder.WriteFile(audioBytes, outputPath);

            // UnityWebRequest로 AudioClip 로드 
            AudioClip clip = await LoadAsync(outputPath, audioType);
            if (clip == null) throw new Exception("Failed to load audio clip.");

            return new UniAudioFile(clip, outputPath);
        }

        // float array 데이터를 이용해 AudioClip 생성
        internal static AudioClip CreateAudioClipFromFloatArray(float[] audioData, int sampleRate, int channels, string name = "decoded_audio_clip")
        {
            AudioClip audioClip = AudioClip.Create(name, audioData.Length, channels, sampleRate, false);
            audioClip.SetData(audioData, 0);
            return audioClip;
        }

        internal static string ParseAudioClipName(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath)) return "temp_audioClip";
            string fileName = Path.GetFileNameWithoutExtension(outputPath);
            if (string.IsNullOrEmpty(fileName)) return "temp_audioClip";
            return fileName;
        }

        private static string ExtractResourcesPath(string fullPath)
        {
            // Assets/Resources/SFX/clip.wav -> SFX/clip (확장자 제거)
            var index = fullPath.IndexOf("Resources", StringComparison.OrdinalIgnoreCase);
            var resPath = fullPath.Substring(index + "Resources".Length + 1); // skip '/'
            return Path.ChangeExtension(resPath, null); // remove .wav etc
        }
    }
}