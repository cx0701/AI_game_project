using Glitch9.Collections;
using Glitch9.ScriptableObjects;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [CreateAssetMenu(fileName = nameof(FileLibrary), menuName = AIDevKitConfig.kFileDatabase, order = AIDevKitConfig.kFileLibraryOrder)]
    public class FileLibrary : ScriptableDatabase<FileLibrary.Repo, FileData, FileLibrary>
    {
        public class Repo : Database<FileData> { }

        public static bool TryGetFileId(string fileName, out string fileId)
        {
            if (LogIfNull())
            {
                fileId = null;
                return false;
            }

            foreach (FileData profile in DB.Values)
            {
                if (profile.Name == fileName)
                {
                    fileId = profile.Id;
                    return true;
                }
            }

            fileId = null;
            return false;
        }
    }
}