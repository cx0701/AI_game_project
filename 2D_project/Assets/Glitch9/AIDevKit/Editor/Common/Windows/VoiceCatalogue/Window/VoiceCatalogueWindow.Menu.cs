using Glitch9.EditorKit.IMGUI;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Glitch9.Internal;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public partial class VoiceCatalogueWindow
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
            await VoiceCatalogue.Instance.UpdateCatalogueAsync();
            TreeView.ReloadTreeView(true, true);
        }

        private async void UpdateElevenLabsCustomVoicesAsync()
        {
            await VoiceCatalogue.Instance.UpdateElevenLabsCustomVoicesAsync();
            TreeView.ReloadTreeView(true, true);
        }

        private void DrawFileMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(new GUIContent("Update Voice Catalogue"), false, UpdateCatalogue);
            menu.AddItem(new GUIContent("Update ElevenLabs Voice Library"), false, UpdateElevenLabsCustomVoicesAsync);

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Reload Voice Assets (ScriptableObjects)"), false, VoiceLibrary.FindAssets);
            menu.AddItem(new GUIContent("Remove Invalid Voice Assets (ScriptableObjects)"), false, VoiceLibrary.RemoveInvalidEntries);
            menu.AddItem(new GUIContent("Update Voice Assets (ScriptableObjects)"), false, UpdateAssets);

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Generate OpenAI Voice Snippets"), false, () => VoiceSnippetGenerator.Generate(AIProvider.OpenAI));
            menu.AddItem(new GUIContent("Generate ElevenLabs Voice Snippets"), false, () => VoiceSnippetGenerator.Generate(AIProvider.ElevenLabs));

            menu.DropDown(rect);
        }

        private void DrawViewMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(new GUIContent("Show Deprecated Voices"), VoiceCatalogueSettings.ShowDeprecatedVoices, () =>
            {
                VoiceCatalogueSettings.ShowDeprecatedVoices = !VoiceCatalogueSettings.ShowDeprecatedVoices;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Display Only Official Voices"), VoiceCatalogueSettings.OnlyShowOfficialVoices, () =>
            {
                VoiceCatalogueSettings.OnlyShowOfficialVoices = !VoiceCatalogueSettings.OnlyShowOfficialVoices;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Custom Voices"), VoiceCatalogueSettings.OnlyShowCustomVoices, () =>
            {
                VoiceCatalogueSettings.OnlyShowCustomVoices = !VoiceCatalogueSettings.OnlyShowCustomVoices;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Missing Voices"), VoiceCatalogueSettings.OnlyShowMissingVoices, () =>
            {
                VoiceCatalogueSettings.OnlyShowMissingVoices = !VoiceCatalogueSettings.OnlyShowMissingVoices;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Display Only Default Voices"), VoiceCatalogueSettings.OnlyShowDefaultVoices, () =>
            {
                VoiceCatalogueSettings.OnlyShowDefaultVoices = !VoiceCatalogueSettings.OnlyShowDefaultVoices;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Display Only My Library"), ModelCatalogueSettings.OnlyShowMyLibrary, () =>
            {
                ModelCatalogueSettings.OnlyShowMyLibrary = !ModelCatalogueSettings.OnlyShowMyLibrary;
                TreeView.ReloadTreeView(true, true);
            });

            menu.AddItem(new GUIContent("Show All Voices"), false, () =>
            {
                VoiceCatalogueSettings.ShowDeprecatedVoices = true;
                VoiceCatalogueSettings.OnlyShowOfficialVoices = true;
                VoiceCatalogueSettings.OnlyShowCustomVoices = true;
                VoiceCatalogueSettings.OnlyShowMissingVoices = false;
                VoiceCatalogueSettings.OnlyShowDefaultVoices = false;
                TreeView.ReloadTreeView(true, true);
            });

            menu.DropDown(rect);
        }

        private void DrawHelpMenu(Rect rect)
        {
            GenericMenu menu = new();

            menu.AddItem(GUIContents.OnlineDocument, false, () => Application.OpenURL(AIDevKitEditorConfig.kOnlineDocUrl));
            menu.AddItem(GUIContents.JoinDiscord, false, () => Application.OpenURL(EditorConfig.kDiscordUrl));

            // https://www.openai.fm/ (An interactive demo for developers to try the new text-to-speech model in the OpenAI API.)
            menu.AddItem(new GUIContent("Official OpenAI TTS Demo"), false, () => Application.OpenURL("https://www.openai.fm/"));


            menu.DropDown(rect);
        }

        private void UpdateAssets()
        {
            List<Voice> inMyLibrary = VoiceLibrary.ToList();
            if (inMyLibrary.IsNullOrEmpty())
            {
                Debug.LogWarning("No Model Assets found in the library.");
                return;
            }

            foreach (Voice voice in inMyLibrary)
            {
                if (voice == null)
                {
                    Debug.LogWarning("Model is null. Skipping.");
                    continue;
                }

                VoiceCatalogueEntry serverData = VoiceCatalogue.Instance.GetEntry(voice.Id);
                if (serverData == null)
                {
                    Debug.LogWarning($"Voice {voice.Id} not found in the catalogue. Skipping.");
                    continue;
                }

                voice.SetData(
                    api: serverData.Api,
                    id: serverData.Id,
                    name: serverData.Name,
                    gender: serverData.Gender,
                    age: serverData.Age,
                    language: serverData.Language
                );

                Debug.Log($"Updated {voice.Id} model asset.");
            }
        }
    }
}