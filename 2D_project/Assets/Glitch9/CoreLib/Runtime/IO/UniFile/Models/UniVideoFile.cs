using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Glitch9.IO.Files
{
    [Serializable]
    public class UniVideoFile : UniFileBase<VideoClip>
    {
        /// <summary>
        /// The file must be in the Resources folder.
        /// </summary> 
        protected override async UniTask<VideoClip> LoadFileAsync()
        {
            // check if the file is in the Resources folder
            if (string.IsNullOrEmpty(Path) || (!Path.Contains(Application.dataPath) && !Path.Contains("Resources")))
                throw new ArgumentException("The file must be in the Resources folder or a valid path.", nameof(Path));

            if (!System.IO.File.Exists(Path))
                throw new ArgumentException($"The file does not exist at the path: {Path}", nameof(Path));

            await UniTask.Yield();

            string fileName = System.IO.Path.GetFileNameWithoutExtension(Path);
            return Resources.Load<VideoClip>(fileName);
        }
        public override byte[] ToBinaryData() => null; // how?

        public UniVideoFile() : base(UniFileType.Video) { }
        public UniVideoFile(string filePath, string url = null) : base(UniFileType.Video, filePath, url) { }
        public UniVideoFile(VideoClip videoClip, string filePath, string url = null) : base(UniFileType.Video, videoClip, filePath, url) { }
        public UniVideoFile(VideoClip videoClip) : base(UniFileType.Video, videoClip) { }
        public static UniVideoFile FromUrl(string url) => new(null, url);
    }
}