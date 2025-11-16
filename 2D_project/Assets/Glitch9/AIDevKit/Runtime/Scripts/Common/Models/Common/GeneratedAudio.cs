using System;
using System.Collections.Generic;
using System.Linq;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// Represents a generated audio or a collection of generated audios.
    /// This class provides implicit conversions to AudioClip and Sprite types for easy usage.
    /// </summary>
    public class GeneratedAudio : GeneratedContent<AudioClip>
    {
        public static implicit operator AudioClip(GeneratedAudio generatedAudio) => generatedAudio?.contents?.FirstOrDefault();
        public static implicit operator AudioClip[](GeneratedAudio generatedAudio) => generatedAudio?.contents;
        public static implicit operator UniAudioFile(GeneratedAudio generatedAudio) => generatedAudio?.ToFiles()?.FirstOrDefault();
        public static implicit operator List<UniAudioFile>(GeneratedAudio generatedAudio) => generatedAudio?.ToFiles();

        private readonly string[] paths;
        public string[] Paths => paths;

        public string this[string path]
        {
            get
            {
                int index = Array.IndexOf(paths, path);
                if (index < 0 || index >= contents.Length)
                    throw new ArgumentException($"Path '{path}' not found in generated audios.");
                return paths[index];
            }
        }

        public GeneratedAudio(AudioClip audioClip, string path, Usage usage = null) : base(audioClip, usage) => paths = new[] { path };
        public GeneratedAudio(AudioClip[] textures, string[] paths, Usage usage = null) : base(textures, usage) => this.paths = paths;

        public List<UniAudioFile> ToFiles()
        {
            List<UniAudioFile> files = new List<UniAudioFile>(contents.Length);
            for (int i = 0; i < contents.Length; i++)
            {
                string path = paths[i];
                AudioClip audioClip = contents[i];
                if (audioClip == null) continue;
                files.Add(new UniAudioFile(audioClip, path));
            }
            return files;
        }
    }
}