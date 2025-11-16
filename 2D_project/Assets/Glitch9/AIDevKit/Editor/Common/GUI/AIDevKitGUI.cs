using Glitch9.EditorKit;
using System;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal partial class AIDevKitGUI
    {
        private const float kBigBtnHeight = 24f;

        private static class TextSamples
        {
            internal const string SystemInstruction = "You are a chatbot that can help the user with anything.";
            internal const string StartingMessage = "Hello, how can I help you today?";
        }

        internal static void HistoryButton()
        {
            if (GUILayout.Button(new GUIContent(EditorIcons.History, "Open Prompt History"), ExEditorStyles.headerButton))
            {
                AIDevKitEditor.OpenPromptHistory();
            }
        }

        internal static void UrlButtons(params (string, string)[] labelUrlPairs)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(15f); // for indentation
                foreach (var (label, url) in labelUrlPairs)
                {
                    if (GUILayout.Button(label, GUILayout.Height(kBigBtnHeight), GUILayout.Width(120), GUILayout.ExpandWidth(true)))
                    {
                        Application.OpenURL(url);
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        internal static void NoContentGenerated()
        {
            EditorGUILayout.LabelField("No content generated yet.", ExEditorStyles.centeredBlueBoldLabel, GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
        }

        internal static bool LinkButton(string label)
        {
            return GUILayout.Button(label, AIDevKitStyles.LinkButton, GUILayout.Height(16));
        }

        internal static void ReloadScreen(Action onReload)
        {
            GUILayout.FlexibleSpace(); // 화면 위쪽 여백 

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(ExEditorStyles.helpBox, GUILayout.Width(400)); // 원하는 너비 설정
                {
                    GUILayout.Label("There was an error while loading the tool.\n\n" +
                                   "If you don't have necessary API key to use this tool," +
                                   "please set your API key in the user preferences.",
                                   ExEditorStyles.bigLabel);

                    GUILayout.Space(20);


                    if (GUILayout.Button("Reload Window", ExEditorStyles.bigButton))
                    {
                        onReload?.Invoke();
                    }

                    if (GUILayout.Button("Open Preferences", ExEditorStyles.bigButton))
                    {
                        SettingsService.OpenUserPreferences(AIDevKitEditorConfig.kProviderSettingsCore);
                    }

                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace(); // 화면 아래쪽 여백
        }

        internal static void SystemInstructionTextArea(SerializedProperty systemInstruction, float height)
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(GUIContents.SystemInstruction);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Sample"))
                {
                    systemInstruction.stringValue = TextSamples.SystemInstruction;
                }
            }
            GUILayout.EndHorizontal();
            systemInstruction.stringValue = EditorGUILayout.TextArea(systemInstruction.stringValue, GUILayout.Height(height));
        }

        internal static void InitialMessageTextArea(SerializedProperty initialMessage, float height)
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(GUIContents.StartingMessage);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Sample"))
                {
                    initialMessage.stringValue = TextSamples.StartingMessage;
                }
            }
            GUILayout.EndHorizontal();
            initialMessage.stringValue = EditorGUILayout.TextArea(initialMessage.stringValue, GUILayout.Height(height));
        }

        internal static void StreamToggle(SerializedProperty stream) => EditorGUILayout.PropertyField(stream, GUIContents.Stream);

        internal static void OfficialDocumentButton(AIProvider provider, string docUrl)
        {
            Texture2D icon = AIDevKitGUIUtility.GetProviderIcon(provider);
            if (GUILayout.Button(new GUIContent("  Official Documentation", icon), GUILayout.Height(30f)))
            {
                Application.OpenURL(docUrl);
            }
        }

        internal static ImageSize ImageSizePopup(ImageSize imageSize) => ExGUILayout.EnumPopup(GUIContents.ImageSize, imageSize);
        internal static ImageQuality ImageQualityPopup(ImageQuality imageQuality) => ExGUILayout.EnumPopup(GUIContents.ImageQuality, imageQuality);
        internal static ImageStyle ImageStylePopup(ImageStyle imageStyle) => ExGUILayout.EnumPopup(GUIContents.ImageStyle, imageStyle);

        internal static ImageSize DallE2ImageSizeField(ImageSize value)
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField("Size", GUILayout.Width(EditorGUIUtility.labelWidth - 2f));

                if (ExGUILayout.ToggleLeft("256x256", value == ImageSize._256x256))
                {
                    value = ImageSize._256x256;
                }

                if (ExGUILayout.ToggleMid("512x512", value == ImageSize._512x512))
                {
                    value = ImageSize._512x512;
                }

                if (ExGUILayout.ToggleRight("1024x1024", value == ImageSize._1024x1024))
                {
                    value = ImageSize._1024x1024;
                }
            }
            GUILayout.EndHorizontal();

            return value;
        }

        internal static void DrawProRequiredWarning()
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            {
                GUILayout.Label(EditorIcons.ProBadge, GUILayout.Width(30), GUILayout.Height(30));
                GUILayout.Label("This feature is only available in AI DevKit Pro.", ExEditorStyles.statusBoxText, GUILayout.Height(30));
                if (GUILayout.Button("Upgrade", GUILayout.Height(30), GUILayout.Width(100))) AIDevKitEditor.OpenProURL();
            }
            GUILayout.EndHorizontal();
        }
    }
}