using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal partial class AIDevKitGUI
    {
        private const string kApiKeyLabel = "API Key (Required)";

        internal static void ApiKeyField(AIProvider api, SerializedProperty encryptApiKey, SerializedProperty encryptedApiKey, SerializedProperty apiKey)
        {
            string prefsKey = $"{api}_HideApiKey";
            bool hide = EditorPrefs.GetBool(prefsKey, false);
            bool encrypt = encryptApiKey.boolValue;
            int savedIndentLevel = EditorGUI.indentLevel;

            GUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(kApiKeyLabel);

                EditorGUI.indentLevel = 0;
                GUILayout.Space(savedIndentLevel * 2f);
                DrawPropertyField(encrypt, hide);

                if (encrypt)
                {
                    DrawDeleteButton();
                }
                else
                {
                    DrawHideButton(hide);
                    DrawEncryptButton();
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
                EditorGUI.indentLevel = savedIndentLevel;
            }

            void DrawPropertyField(bool encrypt, bool hide)
            {
                if (encrypt)
                {
                    ExGUILayout.BoxedLabel("Using Encrypted API Key", GUILayout.Height(18f));
                }
                else
                {
                    if (hide)
                    {
                        apiKey.stringValue = EditorGUILayout.PasswordField(apiKey.stringValue);
                    }
                    else
                    {
                        apiKey.stringValue = EditorGUILayout.TextField(apiKey.stringValue);
                    }
                }
            }

            void DrawHideButton(bool hide)
            {
                bool newHide = GUILayout.Toggle(hide, EditorIcons.Hide, EditorStyles.miniButtonMid, GUILayout.Width(20));
                if (newHide != hide) EditorPrefs.SetBool(prefsKey, newHide);
            }

            void DrawEncryptButton()
            {
                if (GUILayout.Button(EditorIcons.Key, EditorStyles.miniButtonRight, GUILayout.Width(20)))
                {
                    if (string.IsNullOrEmpty(apiKey.stringValue))
                    {
                        EditorUtility.DisplayDialog("Error", "API key is empty. Please enter a valid API key.", "OK");
                        return;
                    }

                    if (ShowDialog.Confirm("Are you sure you want to encrypt the API key? You won't be able to decrypt it for safty reasons."))
                    {
                        encryptApiKey.boolValue = true;
                        encryptedApiKey.stringValue = Encrypter.EncryptString(apiKey.stringValue);
                        apiKey.stringValue = string.Empty;
                        EditorUtility.DisplayDialog("Success", "API Key encrypted successfully!", "OK");
                    }
                }
            }

            void DrawDeleteButton()
            {
                if (GUILayout.Button(EditorIcons.Delete, EditorStyles.miniButtonRight, GUILayout.Width(20)))
                {
                    if (EditorUtility.DisplayDialog("Delete Encrypted API Key", "Are you sure you want to delete the encrypted API Key?", "Yes", "No"))
                    {
                        encryptedApiKey.stringValue = null;
                    }
                }
            }
        }
    }
}