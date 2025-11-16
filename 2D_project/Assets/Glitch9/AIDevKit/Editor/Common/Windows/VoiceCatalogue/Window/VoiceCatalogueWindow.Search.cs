using System;
using UnityEditor;
using UnityEngine;
using Glitch9.EditorKit;
using System.Collections.Generic;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public partial class VoiceCatalogueWindow
    {
        private GUIStyle SearchBarStyle => VoiceCatalogueWindowUtil.GetSearchBarStyle();
        private List<SystemLanguage> AvailableLanguages => VoiceCatalogueWindowUtil.GetAvailableLanguages();
        private string[] AvailableLanguagesDisplayNames => VoiceCatalogueWindowUtil.GetAvailableLanguagesDisplayNames();
        private List<AIProvider> AvailableApis => VoiceCatalogueWindowUtil.GetAvailableApis();

        protected override void BottomBar()
        {
            if (TreeView == null) return;

            GUILayout.BeginHorizontal(SearchBarStyle);
            {
                DrawSearchBar();
            }
            GUILayout.EndHorizontal();
        }

        private static bool VoiceAgeCheckEnabled(Enum value)
        {
            return value switch
            {
                VoiceAge.Child => false,
                _ => true,
            };
        }

        private void ResetFilters()
        {
            VoiceCatalogueSettings.ApiProvider = AIProvider.All;

            VoiceCatalogueSettings.OnlyShowMissingVoices = false;
            VoiceCatalogueSettings.OnlyShowDefaultVoices = false;
            VoiceCatalogueSettings.OnlyShowOfficialVoices = false;
            VoiceCatalogueSettings.OnlyShowCustomVoices = false;
            VoiceCatalogueSettings.OnlyShowFeaturedVoices = false;
            VoiceCatalogueSettings.OnlyShowMyLibrary = false;

            VoiceCatalogueSettings.VoiceType = VoiceType.None;
            VoiceCatalogueSettings.VoiceCategory = VoiceCategory.None;
            VoiceCatalogueSettings.VoiceGender = VoiceGender.None;
            VoiceCatalogueSettings.VoiceAge = VoiceAge.None;
            VoiceCatalogueSettings.VoiceLanguage = SystemLanguage.Unknown;

            VoiceCatalogueSettings.ShowDeprecatedVoices = true;

            TreeView.Filter.SearchText = string.Empty;
            TreeView.ReloadTreeView(true);
        }

        private void DrawSearchBar()
        {
            const float kMinWidth = 130f;
            const float kSpace = 10f;
            const float kToggleMinWidth = 80f;
            const float kBtnWidth = 80f;
            const float kBtnHeight = 40f;

            GUILayout.BeginVertical(GUILayout.MinWidth(kMinWidth));
            {
                EditorGUIUtility.labelWidth = 120f;

                GUILayout.BeginHorizontal();
                try
                {
                    GUILayout.Label($"Displaying {TreeView.ShowingCount}/{TreeView.TotalCount}", EditorStyles.boldLabel, GUILayout.Height(18f), GUILayout.MaxWidth(156f));

                    if (GUILayout.Button(new GUIContent(EditorIcons.Reset, "Reset Filters"), ExEditorStyles.miniButton, GUILayout.Width(20f)))
                    {
                        ResetFilters();
                    }
                }
                finally
                {
                    GUILayout.EndHorizontal();
                }

                //GUILayout.Label($"Displaying {TreeView.ShowingCount}/{TreeView.TotalCount} Voices", EditorStyles.boldLabel, GUILayout.Height(18f));

                EditorGUIUtility.labelWidth = 24f;

                AIProvider api = ExGUILayout.ExtendedEnumPopup(
                    selected: VoiceCatalogueSettings.ApiProvider,
                    displayOptions: AvailableApis,
                    label: new GUIContent("API"),
                    style: null,
                     GUILayout.ExpandWidth(true),
                    GUILayout.MaxWidth(180f));

                if (api != VoiceCatalogueSettings.ApiProvider)
                {
                    VoiceCatalogueSettings.ApiProvider = api;
                    TreeView.ReloadTreeView(true);
                }
            }
            GUILayout.EndVertical();

            EditorGUIUtility.labelWidth = 70f;
            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                VoiceType voiceType = (VoiceType)EditorGUILayout.EnumPopup("Type", VoiceCatalogueSettings.VoiceType, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (voiceType != VoiceCatalogueSettings.VoiceType) VoiceCatalogueSettings.VoiceType = voiceType;

                VoiceCategory voiceCategory = (VoiceCategory)EditorGUILayout.EnumPopup(
                    label: new GUIContent("Category"),
                    selected: VoiceCatalogueSettings.VoiceCategory,
                    GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true)
                );

                if (voiceCategory != VoiceCatalogueSettings.VoiceCategory) VoiceCatalogueSettings.VoiceCategory = voiceCategory;
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                VoiceGender voiceGender = (VoiceGender)EditorGUILayout.EnumPopup("Gender", VoiceCatalogueSettings.VoiceGender, GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true));
                if (voiceGender != VoiceCatalogueSettings.VoiceGender) VoiceCatalogueSettings.VoiceGender = voiceGender;

                VoiceAge voiceAge = (VoiceAge)EditorGUILayout.EnumPopup(
                    label: new GUIContent("Age"),
                    selected: VoiceCatalogueSettings.VoiceAge,
                    checkEnabled: VoiceAgeCheckEnabled,
                    includeObsolete: false,
                    GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true)
                );

                if (voiceAge != VoiceCatalogueSettings.VoiceAge) VoiceCatalogueSettings.VoiceAge = voiceAge;
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            GUILayout.BeginVertical();
            {
                if (!AvailableLanguages.IsNullOrEmpty())
                {
                    SystemLanguage voiceLanguage = ExGUILayout.ExtendedEnumPopup(
                        selected: VoiceCatalogueSettings.VoiceLanguage,
                        displayOptions: AvailableLanguages,
                        displayNames: AvailableLanguagesDisplayNames,
                        label: new GUIContent("Language"),
                        style: null,
                        GUILayout.MinWidth(kMinWidth), GUILayout.ExpandWidth(true)
                    );

                    if (voiceLanguage != VoiceCatalogueSettings.VoiceLanguage) VoiceCatalogueSettings.VoiceLanguage = voiceLanguage;
                }

                bool voiceFeatured = EditorGUILayout.Toggle("Featured", VoiceCatalogueSettings.OnlyShowFeaturedVoices, GUILayout.MinWidth(kToggleMinWidth), GUILayout.ExpandWidth(true));
                if (voiceFeatured != VoiceCatalogueSettings.OnlyShowFeaturedVoices) VoiceCatalogueSettings.OnlyShowFeaturedVoices = voiceFeatured;
            }
            GUILayout.EndVertical();

            GUILayout.Space(kSpace);

            EditorGUIUtility.labelWidth = 0f;

            if (GUILayout.Button("Update\nCatalogue", GUILayout.Height(kBtnHeight), GUILayout.Width(kBtnWidth)))
            {
                if (EditorUtility.DisplayDialog("Update Voice Catalogue", "This may take a while, do you want to continue?", "Yes", "No"))
                {
                    VoiceCatalogue.Instance.UpdateCatalogue((success) =>
                    {
                        if (success)
                        {
                            TreeView.ReloadTreeView(true);
                        }
                    });
                }
            }
        }
    }
}