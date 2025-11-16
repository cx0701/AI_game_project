using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public partial class ExGUILayout
    {

        #region Boxed Label
        public static void BoxedLabel(GUIContent label, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, ExEditorStyles.Box(TextAnchor.MiddleCenter), options);
        public static void BoxedLabel(string label, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, ExEditorStyles.Box(TextAnchor.MiddleCenter), options);
        public static void ErrorLabel(string label)
            => EditorGUILayout.LabelField(new GUIContent(label, EditorIcons.StatusError), ExEditorStyles.Box(TextAnchor.MiddleCenter), GUILayout.Height(18f));

        #endregion

        #region Icon Label - This is very frustrating, but Unity's GUIContent doesn't have a way to put little spaces between the icon and the text. 
        public static void IconLabel(string label, Texture icon, GUIStyle labelStyle = null, GUIStyle iconStyle = null)
        {
            labelStyle ??= ExEditorStyles.label;
            iconStyle ??= ExEditorStyles.labelIcon;

            if (icon != null)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(icon, iconStyle, GUILayout.Height(18f));
                    GUILayout.Label(label, labelStyle, GUILayout.Height(18f));
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.LabelField(label, labelStyle, GUILayout.Height(18f));
            }
        }

        public static void IconLabel(GUIContent label, GUIStyle style = null)
            => IconLabel(label.text, label.image, style);

        #endregion

        #region Selectable Label (For CodeBlocks & etc)
        internal static void SelectableLabel(string text, GUIStyle style) => SelectableLabel(text, -1, style);

        internal static void SelectableLabel(string text, float maxWidth = -1, GUIStyle style = null)
        {
            if (string.IsNullOrEmpty(text)) return;
            if (maxWidth == -1) maxWidth = EditorGUIUtility.currentViewWidth;
            Rect rect = ExGUIUtility.ResolveRect(text, style, maxWidth);
            EditorGUI.SelectableLabel(rect, text, style);
        }
        #endregion

        #region Trailing Button Label
        internal static bool TrailingButtonLabel(GUIContent labelContent, GUIContent buttonContent, GUIStyle labelStyle = null, GUIStyle buttonStyle = null)
        {
            if (labelContent == null || buttonContent == null)
            {
                EditorGUILayout.HelpBox("Label or Button content is null.", MessageType.Error);
                return false;
            }

            buttonStyle ??= ExEditorStyles.miniButton;

            bool clicked;

            GUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(labelContent, labelStyle);
                GUILayout.Space(EditorGUI.indentLevel * 2f);
                clicked = GUILayout.Button(buttonContent, buttonStyle);
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

            return clicked;
        }

        internal static bool TrailingButtonLabel(string labelContent, Texture buttonContent, GUIStyle labelStyle = null, GUIStyle buttonStyle = null)
            => TrailingButtonLabel(new GUIContent(labelContent), new GUIContent(buttonContent), labelStyle, buttonStyle);

        internal static bool TrailingButtonLabel(string labelContent, GUIContent buttonContent, GUIStyle labelStyle = null, GUIStyle buttonStyle = null)
            => TrailingButtonLabel(new GUIContent(labelContent), buttonContent, labelStyle, buttonStyle);

        internal static bool ResetButtonLabel(GUIContent labelContent, GUIStyle labelStyle = null)
        {
            bool clicked;

            GUILayout.BeginHorizontal();
            {
                //float labelWidth = ExGUIUtility.GetLabelWidth();
                GUILayout.Label(labelContent, labelStyle);
                GUILayout.FlexibleSpace();
                clicked = ResetButton();
            }
            GUILayout.EndHorizontal();

            return clicked;
        }

        internal static bool ResetButtonLabel(string labelText, GUIStyle labelStyle = null)
            => ResetButtonLabel(new GUIContent(labelText), labelStyle);

        #endregion 
    }
}