using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    [CustomPropertyDrawer(typeof(ApiKey))]
    public class ApiKeyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            const float kVisibleBtnWidth = 60f;
            const float kEncryptBtnWidth = 60f;

            //var apiProp = property.FindPropertyRelative("api");
            var keyProp = property.FindPropertyRelative("key");
            var encryptProp = property.FindPropertyRelative("encrypt");
            var visibleProp = property.FindPropertyRelative("visible");

            float indent = EditorGUI.indentLevel * 15f + -2f;
            float labelWidth = EditorGUIUtility.labelWidth - indent;
            Rect labelRect = new(position.x, position.y, labelWidth, position.height);
            Rect rectLeftOver = new(position.x + labelWidth, position.y, position.width - labelWidth, position.height);

            Rect[] rects = rectLeftOver.SplitHorizontallyFixedReversed(kVisibleBtnWidth, kEncryptBtnWidth);

            Rect keyRect = rects[0];
            Rect encryptBtnRect = rects[1];
            Rect visibleBtnRect = rects[2];


            string labelText = label.text;

            if (encryptProp.boolValue)
            {
                labelText += " (Encrypted)";
            }

            EditorGUI.LabelField(labelRect, labelText, EditorStyles.boldLabel);

            if (visibleProp.boolValue)
            {
                EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
            }
            else
            {
                string newApiKey = EditorGUI.PasswordField(keyRect, GUIContent.none, keyProp.stringValue);
                if (newApiKey != keyProp.stringValue) keyProp.stringValue = newApiKey;
            }

            if (!encryptProp.boolValue)
            {
                if (GUI.Button(encryptBtnRect, "Encrypt", EditorStyles.miniButtonRight))
                {
                    if (string.IsNullOrEmpty(keyProp.stringValue))
                    {
                        EditorUtility.DisplayDialog("Error", "API key is empty. Please enter a valid API key.", "OK");
                        return;
                    }

                    if (ShowDialog.Confirm("Are you sure you want to encrypt the API key? You won't be able to decrypt it for safty reasons."))
                    {
                        encryptProp.boolValue = true;
                        keyProp.stringValue = Encrypter.EncryptString(keyProp.stringValue);
                    }
                }
            }
            else
            {
                if (GUI.Button(encryptBtnRect, "Clear", EditorStyles.miniButtonRight))
                {
                    if (ShowDialog.Confirm("Are you sure you want to clear the API key?"))
                    {
                        keyProp.stringValue = string.Empty;
                        encryptProp.boolValue = false;
                    }
                }
            }

            if (GUI.Button(visibleBtnRect, visibleProp.boolValue ? "Hide" : "Reveal", EditorStyles.miniButtonLeft))
            {
                visibleProp.boolValue = !visibleProp.boolValue;
            }

            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}