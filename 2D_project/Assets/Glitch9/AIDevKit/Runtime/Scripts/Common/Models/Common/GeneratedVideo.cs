using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.Networking;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    public class GeneratedVideo : GeneratedContent<UniVideoFile>
    {
        public static implicit operator UniVideoFile(GeneratedVideo generatedVideo) => generatedVideo.contents[0];
        public static implicit operator UniVideoFile[](GeneratedVideo generatedVideo) => generatedVideo.contents;

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

        protected GeneratedVideo(UniVideoFile content, Usage usage) : base(content, usage) { }
        protected GeneratedVideo(UniVideoFile[] contents, Usage usage) : base(contents, usage) { }

        public static async UniTask<GeneratedVideo> CreateAsync(IList<string> urls, string outputPath, Model model, MIMEType mimeType, Usage usage = null)
        {
            //if (string.IsNullOrEmpty(outputPath)) throw new ArgumentNullException(nameof(outputPath), "Output path cannot be null or empty.");
            if (urls == null || urls.Count == 0) throw new ArgumentNullException(nameof(urls), "URLs cannot be null or empty.");
            if (model == null) throw new ArgumentNullException(nameof(model), "Model cannot be null.");

            List<UniVideoFile> contents = new();

            if (string.IsNullOrEmpty(outputPath))
            {
                for (int i = 0; i < urls.Count; i++)
                {
                    string url = urls[i];
                    if (string.IsNullOrEmpty(url))
                    {
                        Debug.LogWarning($"URL at index {i} is null or empty. Skipping download.");
                        continue;
                    }
                    contents.Add(UniVideoFile.FromUrl(url));
                }
            }
            else
            {
                List<string> resolvedDlPaths = new();

                string dlPathWithoutIndex = AIDevKitPath.ResolveOutputFilePath(outputPath, model, mimeType);

                for (int i = 0; i < urls.Count; i++)
                {
                    string dlPath = AIDevKitPath.AddIndexToPath(dlPathWithoutIndex, i);
                    resolvedDlPaths.Add(dlPath);
                }

                for (int i = 0; i < urls.Count; i++)
                {
                    string dlPath = resolvedDlPaths[i];
                    string url = urls[i];

                    if (string.IsNullOrEmpty(url))
                    {
                        Debug.LogWarning($"URL at index {i} is null or empty. Skipping download.");
                        continue;
                    }

                    if (await UnityDownloader.DownloadFileAsync(url, dlPath))
                    {
                        contents.Add(new UniVideoFile(dlPath, url));
                    }
                }
            }


            if (contents.Count == 0)
            {
                Debug.LogError("No files were downloaded successfully. Cannot create GeneratedVideo.");
                return null;
            }

            return new(contents.ToArray(), usage);
        }
    }
}
