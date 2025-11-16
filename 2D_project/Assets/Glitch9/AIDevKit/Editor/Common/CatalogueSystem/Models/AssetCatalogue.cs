using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal abstract class AssetCatalogue<TSelf, TEntry, TInterface>
        where TSelf : AssetCatalogue<TSelf, TEntry, TInterface>, new()
        where TEntry : class, TInterface
        where TInterface : class, IData
    {
        public static TSelf Instance => _instance ??= new TSelf();
        private static TSelf _instance;
        internal List<TEntry> Entries { get; set; }
        private readonly string _assetName;
        private readonly string _assetNamePlural;

        protected AssetCatalogue()
        {
            string filePath = GetCataloguePath();

            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                Entries = JsonConvert.DeserializeObject<List<TEntry>>(json, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Formatting.Indented,
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    Converters = new List<JsonConverter>
                    {
                        new ModalityStringListConverter(),
                    },
                });
            }

            Entries ??= new();

            if (this is ModelCatalogue)
            {
                _assetName = "Model";
                _assetNamePlural = "models";
            }
            else if (this is VoiceCatalogue)
            {
                _assetName = "Voice";
                _assetNamePlural = "voices";
            }
            else
            {
                throw new System.Exception($"Unknown AssetCatalogue type: {typeof(TSelf).Name}");
            }
        }

        //internal abstract UniTask CheckForUpdatesAsync();
        internal TEntry GetEntry(string id) => Entries.FirstOrDefault(x => x.Id == id);
        internal bool HasEntry(string id) => Entries.Any(x => x.Id == id);
        protected abstract string GetCataloguePath();
        protected abstract UniTask<List<TInterface>> RetrieveAllEntriesAsync();
        protected abstract TEntry CreateEntry(TInterface softRef);
        protected abstract List<TEntry> GetMissingEntries();
        protected abstract void ShowCatalogueUpdateWindow(List<TInterface> newEntries, List<TInterface> deprecatedEntries);

        internal async UniTask CheckForUpdatesAsync()
        {
            List<TInterface> allEntries = await RetrieveAllEntriesAsync();

            if (allEntries.Count == 0)
            {
                Debug.LogError($"Critical Error: No {_assetNamePlural} found. Please check your API keys and settings.");
                return;
            }

            List<TInterface> newEntries = new();
            List<TInterface> deprecatedEntries = new();

            foreach (TInterface model in allEntries)
            {
                bool isNewModel = !Entries.Any(x => x.Id == model.Id);
                if (isNewModel) newEntries.Add(model);
            }

            foreach (TEntry entry in Entries)
            {
                bool isDeprecated = allEntries.All(x => x.Id != entry.Id);
                if (isDeprecated) newEntries.Add(entry);
            }

            if (newEntries.Count > 0 || deprecatedEntries.Count > 0)
            {
                ApplyEntriesToCatalogue(newEntries, true);
                ApplyEntriesToCatalogue(deprecatedEntries, true);
                ShowCatalogueUpdateWindow(newEntries, deprecatedEntries);
            }
        }

        public async void UpdateCatalogue(Action<bool> onComplete)
        {
            try
            {
                await UpdateCatalogueAsync();
                onComplete?.Invoke(true);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error updating catalogue: {e.Message}");
                onComplete?.Invoke(false);
            }
        }

        public async UniTask UpdateCatalogueAsync()
        {
            Debug.Log($"Updating {_assetName} Catalogue...");

            List<TInterface> allEntries = await RetrieveAllEntriesAsync();

            if (allEntries.Count == 0)
            {
                Debug.LogError($"Critical Error: No {_assetNamePlural} found. Please check your API keys and settings.");
                return;
            }

            Debug.Log($"Found {allEntries.Count} {_assetNamePlural}.");

            ApplyEntriesToCatalogue(allEntries);
        }

        protected void ApplyEntriesToCatalogue(List<TInterface> entries, bool overwrite = false, bool addMissing = false)
        {
            foreach (TInterface data in entries)
            {
                if (data == null)
                {
                    Debug.LogError($"Critical Error: {data} is null.");
                    continue;
                }

                if (overwrite)
                {
                    if (HasEntry(data.Id))
                    {
                        TEntry entry = CreateEntry(data);
                        Entries.Add(entry);
                    }
                }
                else
                {
                    TEntry entry = CreateEntry(data);
                    Entries.Add(entry);
                }
            }

            if (addMissing)
            {
                List<TEntry> missingEntries = GetMissingEntries();

                if (missingEntries != null && missingEntries.Count > 0)
                {
                    foreach (var entry in missingEntries)
                    {
                        // check if the model is already in the catalogue
                        if (Entries.All(x => x.Id != entry.Id))
                        {
                            Entries.Add(entry);
                            Debug.Log($"Found missing {_assetName.ToLower()} (missing form the response): {entry.Name} ({entry.Id})");
                        }
                        else
                        {
                            Debug.Log($"{_assetName} already exists in the catalogue: {entry.Name} ({entry.Id})");
                        }
                    }
                }
            }

            Save();
        }

        protected void Save()
        {
            string filePath = GetCataloguePath();
            string json = JsonConvert.SerializeObject(Entries, Formatting.Indented);
            string directory = System.IO.Path.GetDirectoryName(filePath);
            if (!System.IO.Directory.Exists(directory)) System.IO.Directory.CreateDirectory(directory);
            System.IO.File.WriteAllText(filePath, json);
        }
    }
}