using Glitch9.EditorKit;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal abstract class AIClientSettingsProvider<TSelf, TSettings> : ExtendedSettingsProvider<TSelf, TSettings>
        where TSelf : AIClientSettingsProvider<TSelf, TSettings>
        where TSettings : AIClientSettings<TSettings>
    {
        protected static HashSet<string> Keywords { get; } = new()
        {
            "API Key",
            "Encrypt API Key",
            "api",
            "auto update",
            "models",
        };

        protected const int kLabelWidth = 240;

        private readonly AIProvider api;
        private readonly bool isProOnly;
        private SerializedProperty encryptApiKey;
        private SerializedProperty encryptedApiKey;
        private SerializedProperty apiKey;

        public AIClientSettingsProvider(AIProvider api, bool isProOnly, string path) : base(path, SettingsScope.User)
        {
            this.api = api;
            this.isProOnly = isProOnly;
        }

        protected override void InitializeSettings()
        {
            encryptApiKey = serializedObject.FindProperty(nameof(encryptApiKey));
            encryptedApiKey = serializedObject.FindProperty(nameof(encryptedApiKey));
            apiKey = serializedObject.FindProperty(nameof(apiKey));
        }

        protected override void DrawSettings()
        {
            bool notAvailable = isProOnly && !AIDevKitConfig.IsPro;

            EditorGUI.BeginDisabledGroup(notAvailable);

            EditorGUIUtility.labelWidth = kLabelWidth;

            ExGUILayout.BeginSection(GUIContents.ApiKeySectionTitle);
            {
                AIDevKitGUI.ApiKeyField(api, encryptApiKey, encryptedApiKey, apiKey);
                DrawOptionalSettings();
            }
            ExGUILayout.EndSection();

            DrawAdditionalSections();

            GUILayout.Space(10);

            ExGUILayout.BeginSection(GUIContents.UsefulLinksSectionTitle);
            {
                DrawUsefulLinks();
            }
            ExGUILayout.EndSection();

            EditorGUIUtility.labelWidth = 0;

            EditorGUI.EndDisabledGroup();

            if (notAvailable) AIDevKitGUI.DrawProRequiredWarning();
        }

        protected virtual void DrawOptionalSettings() { }
        protected virtual void DrawAdditionalSections() { }
        protected abstract void DrawUsefulLinks();
    }
}