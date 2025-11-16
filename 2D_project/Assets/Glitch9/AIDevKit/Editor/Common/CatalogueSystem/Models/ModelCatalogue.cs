using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Glitch9.AIDevKit.ElevenLabs;
using Glitch9.AIDevKit.Google;
using Glitch9.AIDevKit.Ollama;
using Glitch9.AIDevKit.OpenAI;
using Glitch9.AIDevKit.OpenRouter;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;


namespace Glitch9.AIDevKit.Editor.Pro
{
    internal class ModelCatalogue : AssetCatalogue<ModelCatalogue, ModelCatalogueEntry, IModelData>
    {
        protected override string GetCataloguePath() => AIDevKitEditorPath.GetModelCataloguePath().FixDoubleAssets();
        protected override ModelCatalogueEntry CreateEntry(IModelData data) => ModelCatalogueEntry.Create(data);
        protected override List<ModelCatalogueEntry> GetMissingEntries() => ElevenLabsModelMeta.GetMissingModels();
        protected override void ShowCatalogueUpdateWindow(List<IModelData> newEntries, List<IModelData> deprecatedEntries)
            => ModelCatalogueUpdateWindow.ShowWindow(newEntries, deprecatedEntries);

        protected override async UniTask<List<IModelData>> RetrieveAllEntriesAsync()
        {
            List<IModelData> allModels = new();
            bool hasAnyApiKey = false;
            int apiCount = 5;

            try
            {
                if (OpenAISettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    // Start Editor Progress bar
                    EditorUtility.DisplayProgressBar("Retrieving Models", "Retrieving models from OpenAI...", 0f);

                    //var res = await OpenAIClient.DefaultInstance.Models.List();
                    var res = await GENTaskManager.ListModelsAsync(AIProvider.OpenAI, 1, 100);
                    if (res != null && res.Data != null)
                    {
                        allModels.AddRange(res.Data);
                        Debug.Log($"Found {res.Data.Count()} models from OpenAI. HasMore: {res.HasMore}");
                    }
                }

                if (GenerativeAISettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    EditorUtility.DisplayProgressBar("Retrieving Models", "Retrieving models from Google...", 1f / apiCount);

                    //var res = await GenerativeAIClient.DefaultInstance.Models.List(100);
                    var res = await GENTaskManager.ListModelsAsync(AIProvider.Google, 1, 100);
                    if (res != null && res.Data != null)
                    {
                        allModels.AddRange(res.Data);
                        Debug.Log($"Found {res.Data.Count()} models from Google. HasMore: {res.HasMore}");
                    }
                }

                if (ElevenLabsSettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    EditorUtility.DisplayProgressBar("Retrieving Models", "Retrieving models from ElevenLabs...", 2f / apiCount);

                    //var res = await ElevenLabsClient.DefaultInstance.Models.List();
                    var res = await GENTaskManager.ListModelsAsync(AIProvider.ElevenLabs, 1, 100);
                    if (res != null && res.Data != null)
                    {
                        allModels.AddRange(res.Data);
                        Debug.Log($"Found {res.Data.Count()} models from ElevenLabs. HasMore: {res.HasMore}");
                    }
                }

                if (await OllamaSettings.CheckConnectionAsync())
                {
                    hasAnyApiKey = true;

                    EditorUtility.DisplayProgressBar("Retrieving Models", "Retrieving models from Ollama...", 3f / apiCount);

                    //var res = await OllamaClient.DefaultInstance.Models.ListLocalModelsAsync();
                    var res = await GENTaskManager.ListModelsAsync(AIProvider.Ollama, 1, 100);
                    if (res != null)
                    {
                        // allModels.AddRange(res.Models);
                        // Debug.Log($"Found {res.Models.Count()} models from Ollama.");
                        allModels.AddRange(res.Data);
                        Debug.Log($"Found {res.Data.Count()} models from Ollama. HasMore: {res.HasMore}");
                    }
                }

                if (OpenRouterSettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    EditorUtility.DisplayProgressBar("Retrieving Models", "Retrieving models from OpenRouter...", 4f / apiCount);

                    //var res = await OpenRouterClient.DefaultInstance.Models.List();
                    var res = await GENTaskManager.ListModelsAsync(AIProvider.OpenRouter, 1, 100);
                    if (res != null && res.Data != null)
                    {
                        allModels.AddRange(res.Data);
                        Debug.Log($"Found {res.Data.Count()} models from OpenRouter. HasMore: {res.HasMore}");
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            if (!hasAnyApiKey)
            {
                ShowDialog.Error("No API keys found for any model providers. Please check your preferences. (Edit > Preferences > AIDevKit)");
                return allModels;
            }

            return allModels;
        }
    }
}