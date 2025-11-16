using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.AIDevKit.ElevenLabs;
using Glitch9.AIDevKit.OpenAI;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal class VoiceCatalogue : AssetCatalogue<VoiceCatalogue, VoiceCatalogueEntry, IVoiceData>
    {
        protected override string GetCataloguePath() => AIDevKitEditorPath.GetVoiceCataloguePath().FixDoubleAssets();
        protected override VoiceCatalogueEntry CreateEntry(IVoiceData data) => VoiceCatalogueEntry.Create(data);
        protected override List<VoiceCatalogueEntry> GetMissingEntries() => null;
        protected override void ShowCatalogueUpdateWindow(List<IVoiceData> newEntries, List<IVoiceData> deprecatedEntries)
            => VoiceCatalogueUpdateWindow.ShowWindow(newEntries, deprecatedEntries);

        protected override async UniTask<List<IVoiceData>> RetrieveAllEntriesAsync()
        {
            List<IVoiceData> allVoices = new();
            bool hasAnyApiKey = false;

            try
            {
                if (OpenAISettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    // Start Editor Progress bar
                    EditorUtility.DisplayProgressBar("Retrieving Voices", "Retrieving voices from OpenAI...", 0f);

                    //var res = await OpenAI.OpenAI.DefaultInstance.Voices.List();
                    //if (res != null && res.Data != null) allVoices.AddRange(res.Data); 
                    allVoices.AddRange(OpenAIVoiceMeta.List); // Hard-code OpenAI voices for now, as the API doesn't exist
                }

                if (ElevenLabsSettings.Instance.HasApiKey())
                {
                    hasAnyApiKey = true;

                    EditorUtility.DisplayProgressBar("Retrieving Voices", "Retrieving voices from ElevenLabs...", 0.5f);

                    // Max list size is 100, so we need to paginate
                    //var res = await ElevenLabsClient.DefaultInstance.Voices.List(100);
                    var res = await GENTaskManager.ListVoicesAsync(AIProvider.ElevenLabs, 1, 100);
                    if (res != null && res.Data != null) allVoices.AddRange(res.Data);
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            if (!hasAnyApiKey)
            {
                ShowDialog.Error("No API keys found for any voice providers. Please check your preferences. (Edit > Preferences > AIDevKit)");
                return allVoices;
            }

            return allVoices;
        }

        private async UniTask<List<IVoiceData>> RetrieveAllElevenLabsCustomVoicesAsync()
        {
            List<IVoiceData> allVoices = new();

            try
            {
                if (ElevenLabsSettings.Instance.HasApiKey())
                {
                    // Start Editor Progress bar
                    EditorUtility.DisplayProgressBar("Retrieving Voices", "Retrieving voice library from ElevenLabs...", 0f);

                    // var cReq = new ElevenLabsQueryRequest<ElevenLabsSharedVoiceData>.Builder()
                    //     .SetPage(1)
                    //     .SetPageSize(100)
                    //     .Build();                    

                    //var cRes = await ElevenLabsClient.DefaultInstance.VoiceLibrary.List(cReq);

                    int currentPage = 1;
                    var cRes = await GENTaskManager.ListCustomVoicesAsync(AIProvider.ElevenLabs, currentPage, 100);
                    if (cRes != null)
                    {
                        if (cRes.Data != null) allVoices.AddRange(cRes.Data);
                        while (cRes.HasMore)
                        {
                            currentPage++;
                            EditorUtility.DisplayProgressBar("Retrieving Voices", $"Retrieving page {currentPage} of shared voices...", currentPage / 20f);
                            //Debug.Log($"Retrieving page {cReq.Page} of shared voices...");
                            //cRes = await ElevenLabsClient.DefaultInstance.VoiceLibrary.List(cReq);
                            //if (cRes != null && cRes.Data != null) allVoices.AddRange(cRes.Data);
                            cRes = await GENTaskManager.ListCustomVoicesAsync(AIProvider.ElevenLabs, currentPage, 100);
                            if (cRes != null && cRes.Data != null) allVoices.AddRange(cRes.Data);
                        }
                    }
                }
                else
                {
                    ShowDialog.Error("ElevenLabs API key not found. Please check your preferences. (Edit > Preferences > AIDevKit)");
                    return allVoices;
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }


            return allVoices;
        }

        internal async UniTask UpdateElevenLabsCustomVoicesAsync()
        {
            Debug.Log("Updating ElevenLabs Custom Voices...");

            List<IVoiceData> allVoices = await RetrieveAllElevenLabsCustomVoicesAsync();

            if (allVoices.Count == 0)
            {
                Debug.LogError("Critical Error: No voices found. Please check your API keys and settings.");
                return;
            }

            Debug.Log($"Found {allVoices.Count} voices.");

            ApplyEntriesToCatalogue(allVoices);
        }
    }
}