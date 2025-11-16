using System.IO;
using UnityEngine;

namespace Glitch9.IO.Files
{
    public class FileUtil
    {
        public static void RefreshIfEditor()
        {
#if UNITY_EDITOR
            if (Application.isPlaying) return;
            UnityEditor.AssetDatabase.Refresh();
#endif
        }

        public static void ChangeFileName(string fullPathBefore, string fullPathAfter, bool overwrite = true)
        {
            if (string.IsNullOrWhiteSpace(fullPathBefore) || string.IsNullOrWhiteSpace(fullPathAfter))
            {
                LogService.Error($"Invalid path. Origin: {fullPathBefore}, To: {fullPathAfter}");
                return;
            }

            if (!File.Exists(fullPathBefore))
            {
                LogService.Error($"The file '{fullPathBefore}' does not exist.");
                return;
            }

            if (File.Exists(fullPathAfter) && overwrite)
            {
                string backupPath = fullPathAfter + "_backup";
                if (File.Exists(backupPath)) File.Delete(backupPath);
                File.Move(fullPathAfter, backupPath);
                File.Delete(fullPathAfter);
            }
            else
            {
                LogService.Error($"The file '{fullPathAfter}' already exists.");
                return;
            }

            File.Move(fullPathBefore, fullPathAfter);
        }

        public static void RevertFileNameChange(string fullPathBefore, string fullPathAfter)
        {
            if (string.IsNullOrWhiteSpace(fullPathBefore) || string.IsNullOrWhiteSpace(fullPathAfter))
            {
                LogService.Error($"Invalid path. Origin: {fullPathBefore}, To: {fullPathAfter}");
                return;
            }

            if (!File.Exists(fullPathAfter))
            {
                LogService.Error($"The file '{fullPathAfter}' does not exist.");
                return;
            }

            if (File.Exists(fullPathBefore))
            {
                LogService.Error($"The file '{fullPathBefore}' already exists.");
                return;
            }

            File.Move(fullPathAfter, fullPathBefore);

            // 백업이 존재하면 복구
            string backupPath = fullPathAfter + "_backup";
            if (File.Exists(backupPath))
            {
                File.Move(backupPath, fullPathAfter);
            }
        }

        public static bool ValidateFileSize(string localPath)
        {
            // check directory first, because this can cause StackOverflow
            string directory = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(directory))
            {
                LogService.Warning($"Directory does not exist: {directory}");
                return false;
            }

            if (File.Exists(localPath))
            {
                /* 파일 사이즈 체크 */
                FileInfo fileInfo = new(localPath);
                long size = fileInfo.Length;

                if (size < 1024) // 5kb
                {
                    LogService.Warning($"Detected a file with a size of less than 1KB: {localPath}");
                    return false;
                }
                return true;
            }

            return false;
        }
    }
}