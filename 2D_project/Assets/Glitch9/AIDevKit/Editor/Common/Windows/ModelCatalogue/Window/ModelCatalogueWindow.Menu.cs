using Glitch9.EditorKit.IMGUI;
using Glitch9.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public partial class ModelCatalogueWindow
    {
        protected override IEnumerable<ITreeViewMenuEntry> CreateMenuEntries()
        {
            yield return new TreeViewMenuDropdown("File", DrawFileMenu);
            yield return new TreeViewMenuDropdown("View", DrawViewMenu);
            yield return new TreeViewMenuDropdown("Help", DrawHelpMenu);
            yield return new TreeViewMenuDropdown("Preferences", (_) => AIDevKitEditor.OpenPreferences());
#if !GLITCH9_AIDEVKIT_PRO
            yield return new TreeViewMenuDropdown("Upgrade to Pro", (_) => AIDevKitEditor.OpenProURL());
#endif  
            yield return new TreeViewMenuSearchField();
        }

        private async void UpdateCatalogue()
        {
            await ModelCatalogue.Instance.UpdateCatalogueAsync();
            TreeView.ReloadTreeView(true, true);
        }

        private void DrawFileMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(new GUIContent("Update Model Catalogue"), false, UpdateCatalogue);

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Reload Model Assets (ScriptableObjects)"), false, ModelLibrary.FindAssets);
            menu.AddItem(new GUIContent("Remove Invalid Model Assets (ScriptableObjects)"), false, ModelLibrary.RemoveInvalidEntries);
            menu.AddItem(new GUIContent("Update Model Assets (ScriptableObjects)"), false, UpdateAssets);

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Generate OpenAI Model Snippets"), false, () => ModelSnippetGenerator.Generate(AIProvider.OpenAI));
            menu.AddItem(new GUIContent("Generate Google Model Snippets"), false, () => ModelSnippetGenerator.Generate(AIProvider.Google));
            menu.AddItem(new GUIContent("Generate ElevenLabs Model Snippets"), false, () => ModelSnippetGenerator.Generate(AIProvider.ElevenLabs));
            menu.AddItem(new GUIContent("Generate Ollama Model Snippets"), false, () => ModelSnippetGenerator.Generate(AIProvider.Ollama));
            menu.AddItem(new GUIContent("Generate OpenRouter Model Snippets"), false, () => ModelSnippetGenerator.Generate(AIProvider.OpenRouter));

            menu.DropDown(rect);
        }

        private void DrawViewMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(new GUIContent("Show Legacy Models"), ModelCatalogueSettings.ShowLegacyModels, () =>
            {
                ModelCatalogueSettings.ShowLegacyModels = !ModelCatalogueSettings.ShowLegacyModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Show Deprecated Models"), ModelCatalogueSettings.ShowDeprecatedModels, () =>
            {
                ModelCatalogueSettings.ShowDeprecatedModels = !ModelCatalogueSettings.ShowDeprecatedModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Display Only Official Models"), ModelCatalogueSettings.OnlyShowOfficialModels, () =>
            {
                ModelCatalogueSettings.OnlyShowOfficialModels = !ModelCatalogueSettings.OnlyShowOfficialModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Custom Models"), ModelCatalogueSettings.OnlyShowCustomModels, () =>
            {
                ModelCatalogueSettings.OnlyShowCustomModels = !ModelCatalogueSettings.OnlyShowCustomModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Missing Models"), ModelCatalogueSettings.OnlyShowMissingModels, () =>
            {
                ModelCatalogueSettings.OnlyShowMissingModels = !ModelCatalogueSettings.OnlyShowMissingModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Default Models"), ModelCatalogueSettings.OnlyShowDefaultModels, () =>
            {
                ModelCatalogueSettings.OnlyShowDefaultModels = !ModelCatalogueSettings.OnlyShowDefaultModels;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Display Only My Library"), ModelCatalogueSettings.OnlyShowMyLibrary, () =>
            {
                ModelCatalogueSettings.OnlyShowMyLibrary = !ModelCatalogueSettings.OnlyShowMyLibrary;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Show All Models"), false, () =>
            {
                ModelCatalogueSettings.ShowLegacyModels = true;
                ModelCatalogueSettings.ShowDeprecatedModels = true;
                ModelCatalogueSettings.OnlyShowOfficialModels = true;
                ModelCatalogueSettings.OnlyShowCustomModels = true;
                ModelCatalogueSettings.OnlyShowMissingModels = false;
                ModelCatalogueSettings.OnlyShowDefaultModels = false;
                TreeView.ReloadTreeView(true, true);
            });

            menu.DropDown(rect);
        }

        private void DrawHelpMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(GUIContents.OnlineDocument, false, () => Application.OpenURL(AIDevKitEditorConfig.kOnlineDocUrl));
            menu.AddItem(GUIContents.JoinDiscord, false, () => Application.OpenURL(EditorConfig.kDiscordUrl));

            menu.DropDown(rect);
        }

        private void UpdateAssets()
        {
            List<Model> inMyLibrary = ModelLibrary.ToList();
            if (inMyLibrary.IsNullOrEmpty())
            {
                Debug.LogWarning("No Model Assets found in the library.");
                return;
            }

            foreach (Model model in inMyLibrary)
            {
                if (model == null)
                {
                    Debug.LogWarning("Model is null. Skipping.");
                    continue;
                }

                ModelCatalogueEntry serverData = ModelCatalogue.Instance.GetEntry(model.Id);
                if (serverData == null)
                {
                    Debug.LogWarning($"Model {model.Id} not found in the catalogue. Skipping.");
                    continue;
                }

                model.SetData(
                    api: serverData.Api,
                    id: serverData.Id,
                    name: serverData.Name,
                    capability: serverData.Capability,
                    family: serverData.Family,
                    inputModality: serverData.InputModality,
                    outputModality: serverData.OutputModality,
                    legacy: serverData.IsLegacy,
                    familyVersion: serverData.FamilyVersion,
                    inputTokenLimit: serverData.InputTokenLimit,
                    outputTokenLimit: serverData.OutputTokenLimit,
                    modelVersion: serverData.Version,
                    fineTuned: serverData.IsFineTuned,
                    prices: serverData.GetPrices()
                );

                Debug.Log($"Updated {model.Id} model asset.");
            }
        }
    }
}