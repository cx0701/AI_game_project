using System.Collections.Generic;
using Glitch9.AIDevKit.Ollama;
using Glitch9.EditorKit;
using Glitch9.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Ollama
{
    internal class OllamaSettingsProvider : ExtendedSettingsProvider<OllamaSettingsProvider, OllamaSettings>
    {
        private const int kLabelWidth = 240;

        [SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            OllamaSettingsProvider provider = new(AIDevKitEditorConfig.kProviderSettingsOllama)
            {
                deactivateHandler = DeactivateHandler,//OllamaSettings.Instance.Save,
                keywords = Keywords
            };
            return provider;
        }

        private static HashSet<string> Keywords { get; } = new()
        {
            "api",
            "auto update",
            "models",
        };

        private static void DeactivateHandler()
        {
            OllamaSettings.Instance.Save();
        }

        private readonly static GUIContent kTitle = new("Server Configuration", "Configuration for the Ollama server.");
        private readonly static GUIContent kEndpointLabel = new("Endpoint (Required)", "The endpoint for the Ollama API.");
        private readonly static GUIContent kPortLabel = new("Port (Required)", "The port for the Ollama API.");

        private SerializedProperty endpoint;
        private SerializedProperty port;
        private SerializedProperty defaultModel;

        public OllamaSettingsProvider(string path) : base(path, SettingsScope.User) { }

        protected override void InitializeSettings()
        {
            endpoint = serializedObject.FindProperty(nameof(endpoint));
            port = serializedObject.FindProperty(nameof(port));
            defaultModel = serializedObject.FindProperty(nameof(defaultModel));
        }

        protected override void DrawSettings()
        {
            bool notAvailable = !AIDevKitConfig.IsPro;

            EditorGUI.BeginDisabledGroup(notAvailable);

            EditorGUIUtility.labelWidth = kLabelWidth;

            DrawGeneralSettings();

            DrawDefaultModels();

            DrawUsefulLinks();

            EditorGUIUtility.labelWidth = 0;

            EditorGUI.EndDisabledGroup();

            if (notAvailable) AIDevKitGUI.DrawProRequiredWarning();
        }

        protected void DrawGeneralSettings()
        {
            ExGUILayout.BeginSection(kTitle);
            {
                EditorGUILayout.PropertyField(endpoint, kEndpointLabel);
                EditorGUILayout.PropertyField(port, kPortLabel);
                if (ExGUILayout.ButtonField("Use Default Configuration"))
                {
                    endpoint.stringValue = "localhost";
                    port.intValue = 11434;
                }
            }
            ExGUILayout.EndSection();
        }

        protected void DrawDefaultModels()
        {
            ExGUILayout.BeginSection(GUIContents.DefaultModelsSectionTitle);
            {
                AIDevKitGUI.LLMPopup(defaultModel, AIProvider.Ollama, GUIContents.DefaultLLM);
            }
            ExGUILayout.EndSection();
        }


        protected void DrawUsefulLinks()
        {
            ExGUILayout.BeginSection(GUIContents.UsefulLinksSectionTitle);
            {
                AIDevKitGUI.UrlButtons(
                    ("Ollama Website", "https://ollama.com/"),
                    ("Ollama Download", "https://ollama.com/download"),
                    ("Ollama Models", "https://ollama.com/library")
                );
            }
            ExGUILayout.EndSection();
        }
    }
}