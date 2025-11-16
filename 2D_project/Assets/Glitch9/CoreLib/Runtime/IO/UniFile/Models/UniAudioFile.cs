using Cysharp.Threading.Tasks;
using Glitch9.CoreLib.IO.Audio;
using System;
using UnityEngine;

namespace Glitch9.IO.Files
{
    [Serializable]
    public class UniAudioFile : UniFileBase<AudioClip>
    {
        [SerializeField] private float length;
        [SerializeField] private string transcript;

        public string Transcript { get => transcript; set => transcript = value; }
        public float Length => length;
        protected override async UniTask<AudioClip> LoadFileAsync()
        {
            AudioClip clip = null;

            if (!string.IsNullOrEmpty(Path))
            {
                clip = await AudioClipUtil.CreateAsync(Path, PathType.Absolute);
            }

            if (clip == null && !string.IsNullOrEmpty(Url))
            {
                clip = await AudioClipUtil.CreateAsync(Url, PathType.Url);
            }

            if (length != 0f && clip != null) length = clip.length;
            return clip;
        }

        public override byte[] ToBinaryData() => Value.EncodeToWAV();

        public UniAudioFile() : base(UniFileType.Audio) { }
        public UniAudioFile(string filePath, string url = null) : base(UniFileType.Audio, filePath, url) { }
        public UniAudioFile(AudioClip audioClip, string filePath, string url = null) : base(UniFileType.Audio, audioClip, filePath, url)
        {
            if (audioClip != null) length = audioClip.length;
        }
        public UniAudioFile(AudioClip audioClip) : base(UniFileType.Audio, audioClip) { }

        public string EncodeToBase64PCM16WAV()
        {
            if (Value == null) return null;
            return Value.EncodeToBase64PCM16WAV();
        }

    }
}