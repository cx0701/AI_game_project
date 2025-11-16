using System;
using System.Collections.Generic;
using System.Linq;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// Represents a generated image or a collection of generated images.
    /// This class provides implicit conversions to Texture2D and Sprite types for easy usage.
    /// </summary>
    public class GeneratedImage : GeneratedContent<Texture2D>
    {
        public static implicit operator Texture2D(GeneratedImage generatedImage) => generatedImage?.contents?.FirstOrDefault();
        public static implicit operator Texture2D[](GeneratedImage generatedImage) => generatedImage?.contents;
        public static implicit operator Sprite(GeneratedImage generatedImage) => generatedImage?.contents[0].ToSprite();
        public static implicit operator Sprite[](GeneratedImage generatedImage) => Array.ConvertAll(generatedImage?.contents, t => t.ToSprite());
        public static implicit operator UniImageFile(GeneratedImage generatedImage) => generatedImage?.ToFiles()?.FirstOrDefault();
        public static implicit operator List<UniImageFile>(GeneratedImage generatedImage) => generatedImage?.ToFiles();

        private readonly string[] paths;
        public string[] Paths => paths;

        public string this[string path]
        {
            get
            {
                int index = Array.IndexOf(paths, path);
                if (index < 0 || index >= contents.Length)
                    throw new ArgumentException($"Path '{path}' not found in generated images.");
                return paths[index];
            }
        }

        public GeneratedImage(Texture2D texture, string path, Usage usage = null) : base(texture, usage) => paths = new[] { path };
        public GeneratedImage(Texture2D[] textures, string[] paths, Usage usage = null) : base(textures, usage) => this.paths = paths;
        public List<UniImageFile> ToFiles()
        {
            if (contents.Length == 0) return null;
            List<UniImageFile> files = new(contents.Length);
            for (int i = 0; i < contents.Length; i++)
            {
                string path = paths[i];
                Texture2D texture = contents[i];
                if (texture == null) continue;
                files.Add(new UniImageFile(texture, path));
            }
            return files;
        }
    }

    public static class GeneratedImageExtensions
    {
        public static Sprite ToSprite(this Texture2D tex)
        {
            if (tex == null) return null;
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
    }
}