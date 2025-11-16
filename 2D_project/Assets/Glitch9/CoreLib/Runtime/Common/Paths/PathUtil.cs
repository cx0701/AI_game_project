using System.IO;
using UnityEngine;

namespace Glitch9
{
    public static class PathUtil
    {
        /*         
        Path Types Examples

        1. Application.dataPath                : C:/UnityProjects/Glitch9/AI Development Kit/Assets   
        2. Application.streamingAssetsPath     : C:/UnityProjects/Glitch9/AI Development Kit/Assets/StreamingAssets         
        3. Application.dataPath + "/Resources" : C:/UnityProjects/Glitch9/AI Development Kit/Assets/Resources           
        4. Application.persistentDataPath      : C:/Users/Codeqo/AppData/LocalLow/Glitch9/Routina  
        5. Application.temporaryCachePath      : C:/Users/Codeqo/AppData/Local/Temp/Glitch9/Routina        
        6. Application.consoleLogPath          : C:/Users/Codeqo/AppData/Local/Unity/Editor/Editor.log           
        */

        public static string ConvertPath(string path, PathType to)
        {
            PathType from = ResolveType(path);
            // Debug.Log($"ConvertPath: <color=yellow>{path}</color> from <color=cyan>{from}</color> to {to}");
            return ConvertINTERNAL(from, to, path).FixDoubleAssets();
        }

        private static string ConvertINTERNAL(PathType from, PathType to, string path)
        {
            // Just return the path if the from and to types are the same.
            if (from == to) return path.FixSlashes();

            // First convert the path to an absolute path.
            string absolutePath = from switch
            {
                PathType.Assets => Path.Combine(Application.dataPath, path).FixSlashes(),// Assets 타입이라면 Application.dataPath에 결합
                PathType.StreamingAssets => Path.Combine(Application.streamingAssetsPath, path).FixSlashes(),
                PathType.PersistentData => Path.Combine(Application.persistentDataPath, path).FixSlashes(),
                PathType.Absolute => path.FixSlashes(),
                PathType.Url => path,// 변환하지 않음
                _ => path.FixSlashes(),
            };

            // Then convert the absolute path to the desired type.
            return to switch
            {
                PathType.Assets => absolutePath.Replace(Application.dataPath, "Assets").FixSlashes(),
                PathType.StreamingAssets => absolutePath.Replace(Application.streamingAssetsPath, "StreamingAssets").FixSlashes(),
                PathType.PersistentData => absolutePath.Replace(Application.persistentDataPath, "PersistentData").FixSlashes(),
                PathType.Absolute => absolutePath,// 이미 절대 경로가 되었으므로 그대로 반환
                PathType.Url => absolutePath,// 로컬 경로를 웹 URL로 변환하는 건 일반적이지 않으므로 그대로 반환합니다.
                _ => absolutePath,
            };
        }

        /// <summary>
        /// Resolves the path type based on the given file path.
        /// </summary>
        /// <param name="unknownFormattedPath">The file path as a string</param>
        /// <returns>>The resolved path type</returns>
        internal static PathType ResolveType(string unknownFormattedPath)
        {
            if (string.IsNullOrWhiteSpace(unknownFormattedPath))
            {
                Debug.LogError("Path is null or empty.");
                return PathType.Unknown;
            }

            unknownFormattedPath = unknownFormattedPath.FixSlashes();

            // 웹 URL 확인
            if (unknownFormattedPath.StartsWith("http://") || unknownFormattedPath.StartsWith("https://"))
                return PathType.Url;

            // Assets 경로 여부를 판단 (Application.dataPath로 시작하는지 확인) 
            if (unknownFormattedPath.StartsWith("Assets/"))
                return PathType.Assets;

            // StreamingAssets나 Resources 폴더 관련 경로 판단
            if (unknownFormattedPath.Contains("/StreamingAssets/"))
                return PathType.StreamingAssets;
            if (unknownFormattedPath.Contains("/Resources/"))
                return PathType.Resources;

            // 특정 사용자 폴더 (PersistentData 등) 확인
            if (unknownFormattedPath.Contains(":/Users/"))
            {
                if (unknownFormattedPath.Contains("/Temp/"))
                    return PathType.TemporaryCache;
                if (unknownFormattedPath.Contains("/Unity/Editor/"))
                    return PathType.ConsoleLog;
                return PathType.PersistentData;
            }

            // 절대 경로인지 판단 (예: C:/, D:/ 등)
            if (unknownFormattedPath.Contains(":/"))
                return PathType.Absolute;

            return PathType.Unknown;
        }


    }
}