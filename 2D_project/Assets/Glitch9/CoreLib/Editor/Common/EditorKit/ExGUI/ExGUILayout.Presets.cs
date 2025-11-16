using System;
using Glitch9.Internal;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public partial class ExGUILayout
    {
        public static bool ResetButton(GUIStyle style = null)
            => GUILayout.Button(EditorIcons.Reset, style ?? ExEditorStyles.miniButton, GUILayout.Width(20));
        public static bool IconButton(Texture icon, GUIStyle style = null)
            => GUILayout.Button(icon, style ?? ExEditorStyles.miniButton, GUILayout.Width(20));
        public static bool FolderPanelButton(GUIStyle style = null)
            => GUILayout.Button("../", style ?? ExEditorStyles.miniButton, GUILayout.Width(20));
        public static bool OpenFolderButton(GUIStyle style = null)
            => GUILayout.Button(EditorIcons.Folder, style ?? ExEditorStyles.miniButton, GUILayout.Width(20));

        internal static void DrawReloadWindow(string message, Action onReload)
        {
            GUILayout.FlexibleSpace(); // 화면 위쪽 여백 

            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginVertical(GUILayout.Width(400)); // 원하는 너비 설정
                {
                    GUILayout.Label("There was an error while loading this window.", ExEditorStyles.bigLabel);
                    ExGUILayout.StatusBox(message, MessageStatus.Error);

                    GUILayout.Space(20);

                    if (GUILayout.Button("Reload Window", ExEditorStyles.bigButton))
                    {
                        onReload?.Invoke();
                    }
                }
                GUILayout.EndVertical();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            GUILayout.FlexibleSpace(); // 화면 아래쪽 여백
        }

        internal static void ComponentTitle(Texture icon, string title, string subtitle)
        {
            GUILayout.BeginHorizontal(ExGUI.box);
            try
            {
                GUILayout.Label(icon, GUILayout.Width(32), GUILayout.Height(32));

                GUILayout.BeginVertical();
                {
                    GUILayout.Label(title, ExEditorStyles.componentTitle);
                    GUILayout.Label(subtitle, ExEditorStyles.componentSubtitle);
                }
                GUILayout.EndVertical();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        internal static void BeginSection(string title)
        {
            GUILayout.Label(title, EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
        }

        internal static void BeginSection(GUIContent title)
        {
            GUILayout.Label(title, EditorStyles.boldLabel);
            EditorGUI.indentLevel = 1;
        }

        internal static void BeginSection(string title, Action trailing)
        {
            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label(title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                trailing?.Invoke();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = 1;
        }

        internal static void BeginSection(GUIContent title, Action trailing)
        {
            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Label(title, EditorStyles.boldLabel);
                GUILayout.FlexibleSpace();
                trailing?.Invoke();
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel = 1;
        }


        internal static void EndSection()
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.Space();
        }

        internal static void TitleField(string title)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(title, ExEditorStyles.title);
            ExGUIUtility.DrawTitleLine();
        }

        internal static void ProVersion(string featureName, string storeUrl)
        {
            GUILayout.BeginHorizontal(ExEditorStyles.helpBox);
            {
                ExGUILayout.TextureField(EditorIcons.ProBadge, Vector2.one * 32);

                GUILayout.Space(10);

                GUILayout.Label($"{featureName} is a Pro feature.\r\nPlease upgrade to the Pro version to access this feature.",
                    EditorStyles.wordWrappedLabel, GUILayout.ExpandWidth(true), GUILayout.MaxWidth(800));

                if (GUILayout.Button("Upgrade to Pro", GUILayout.Width(100), GUILayout.Height(30)))
                {
                    Application.OpenURL(storeUrl);
                }
            }
            GUILayout.EndHorizontal();
        }

        internal static void DrawTroubleShootings(string docUrl, string githubUrl)
        {
            GUILayout.BeginHorizontal();
            try
            {
                if (GUILayout.Button("Documentation", GUILayout.Height(24f), GUILayout.Width(200), GUILayout.ExpandWidth(true)))
                {
                    Application.OpenURL(docUrl);
                }

                if (GUILayout.Button("Discord", GUILayout.Height(24f), GUILayout.Width(200), GUILayout.ExpandWidth(true)))
                {
                    Application.OpenURL(EditorConfig.kDiscordUrl);
                }

                if (GUILayout.Button("Report An Issue (Github)", GUILayout.Height(24f), GUILayout.Width(200), GUILayout.ExpandWidth(true)))
                {
                    Application.OpenURL(githubUrl);
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }
    }
}