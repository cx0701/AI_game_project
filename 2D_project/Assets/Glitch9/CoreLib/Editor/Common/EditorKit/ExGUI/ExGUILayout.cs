using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public enum MessageStatus { None, Okay, Error, Warning, }
    public partial class ExGUILayout
    {
        private const float kToggleSelectorXOffset = 0.5f;

        #region Toggles
        public static bool Toggle(GUIContent content, bool isOn, params GUILayoutOption[] options)
        {
            if (isOn) GUI.backgroundColor = new Color(0.5f, 0.9f, 0.9f);
            if (GUILayout.Button(content, GUI.skin.button, options)) isOn = !isOn;
            GUI.backgroundColor = Color.white;
            return isOn;
        }
        public static bool Toggle(string label, bool isOn, params GUILayoutOption[] options)
            => Toggle(new GUIContent(label), isOn, options);
        public static bool Toggle(Texture2D tex, bool isOn, params GUILayoutOption[] options)
            => Toggle(new GUIContent(tex), isOn, options);
        public static bool ToggleMid(GUIContent content, bool isOn, params GUILayoutOption[] options)
            => GUILayout.Toggle(isOn, content, EditorStyles.miniButtonMid, options);
        public static bool ToggleMid(string label, bool isOn, params GUILayoutOption[] options)
            => ToggleMid(new GUIContent(label), isOn, options);
        public static bool ToggleMid(Texture2D tex, bool isOn, params GUILayoutOption[] options)
            => ToggleMid(new GUIContent(tex), isOn, options);
        public static bool ToggleLeft(GUIContent content, bool isOn, params GUILayoutOption[] options)
            => GUILayout.Toggle(isOn, content, EditorStyles.miniButtonLeft, options);
        public static bool ToggleLeft(string label, bool isOn, params GUILayoutOption[] options)
            => ToggleLeft(new GUIContent(label), isOn, options);
        public static bool ToggleLeft(Texture2D tex, bool isOn, params GUILayoutOption[] options)
            => ToggleLeft(new GUIContent(tex), isOn, options);
        public static bool ToggleRight(GUIContent content, bool isOn, params GUILayoutOption[] options)
            => GUILayout.Toggle(isOn, content, EditorStyles.miniButtonRight, options);
        public static bool ToggleRight(string label, bool isOn, params GUILayoutOption[] options)
            => ToggleRight(new GUIContent(label), isOn, options);
        public static bool ToggleRight(Texture2D tex, bool isOn, params GUILayoutOption[] options)
            => ToggleRight(new GUIContent(tex), isOn, options);

        #endregion Toggles   

        #region Texture & Sprite Fields 
        public static void TextureField(Texture texture, Vector2? size = null, float yOffset = 0)
        {
            try
            {
                size ??= new Vector2(texture.width, texture.height);
                Rect rect = GUILayoutUtility.GetRect(size.Value.x, size.Value.y + yOffset);
                GUI.DrawTexture(rect, texture != null ? texture : EditorIcons.NoImageHighRes, ScaleMode.ScaleToFit);
            }
            catch
            {
                GUILayout.Label(EditorIcons.NoImageHighRes, GUILayout.Width(size.Value.x), GUILayout.Height(size.Value.y));
            }
        }

        #endregion

        #region DateTime & UnixTime Fields

        public static DateTime DateTimeField(GUIContent label, DateTime dateTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);

            if (label != null)
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            int YY = 2000;
            int MM = 1;
            int DD = 1;
            int hh = 0;
            int mm = 0;
            int ss = 0;

            const float SMALL_SPACE = 6f;
            const float LARGE_SPACE = 20f;

            if (year)
            {
                YY = EditorGUILayout.IntField(dateTime.Year, GUILayout.Width(50), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("-", GUILayout.MaxWidth(SMALL_SPACE));
            }

            if (month)
            {
                MM = EditorGUILayout.IntField(dateTime.Month, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField("-", GUILayout.MaxWidth(SMALL_SPACE));
            }

            if (day)
            {
                DD = EditorGUILayout.IntField(dateTime.Day, GUILayout.MaxWidth(30), GUILayout.ExpandWidth(true));
            }

            if (hour)
            {
                GUILayout.Space(LARGE_SPACE);
                hh = EditorGUILayout.IntField(dateTime.Hour, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(":", GUILayout.MaxWidth(SMALL_SPACE));
            }

            if (minute)
            {
                mm = EditorGUILayout.IntField(dateTime.Minute, GUILayout.Width(30), GUILayout.ExpandWidth(true));
                EditorGUILayout.LabelField(":", GUILayout.MaxWidth(SMALL_SPACE));
            }

            if (second)
            {
                ss = EditorGUILayout.IntField(dateTime.Second, GUILayout.Width(30), GUILayout.ExpandWidth(true));
            }

            EditorGUILayout.EndHorizontal();
            return new DateTime(YY, MM, DD, hh, mm, ss);
        }

        public static DateTime DateTimeField(string label, DateTime dateTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
            => DateTimeField(new GUIContent(label), dateTime, year, month, day, hour, minute, second, options);
        public static DateTime DateTimeField(DateTime dateTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
            => DateTimeField(GUIContent.none, dateTime, year, month, day, hour, minute, second, options);
        public static UnixTime UnixTimeField(string label, UnixTime unixTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
            => UnixTimeField(new GUIContent(label), unixTime, year, month, day, hour, minute, second, options);
        public static UnixTime UnixTimeField(GUIContent label, UnixTime unixTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
        {
            DateTime dateTime = unixTime.ToDateTime();
            return new UnixTime(DateTimeField(label, dateTime, year, month, day, hour, minute, second, options));
        }

        #endregion DateTime & UnixTime Fields

        #region ExColorPicker
        public static Color ColorPicker(GUIContent label, Color selected, IList<Color> colorOptions)
        {
            Color defaultColor = GUI.backgroundColor;
            GUIStyle style = ExEditorStyles.colorPickerButton;

            GUILayout.BeginHorizontal();

            //label
            EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth));

            foreach (Color color in colorOptions)
            {
                // Change the background texture for the selected color entry
                style.normal.background = color == selected ? EditorTextures.ToolBarButtonOn : EditorTextures.ToolBarButtonOff;
                GUI.backgroundColor = color;

                if (GUILayout.Button("", style))
                {
                    selected = color;
                }
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = defaultColor;
            return selected;
        }
        public static Color ColorPicker(string label, Color selected, IList<Color> colorOptions)
            => ColorPicker(label, selected, colorOptions);

        #endregion

        #region Path Fields
        public static string PathField(string label, string value, string rootPath, string defaultPath)
            => PathField(new GUIContent(label), value, rootPath, defaultPath);
        public static string PathField(GUIContent label, string value, string rootPath, string defaultPath)
            => DrawPathFieldINTERNAL(label, value, rootPath, defaultPath);
        private static string DrawPathFieldINTERNAL(GUIContent label, string value, string rootPath, string defaultPath)
        {
            GUILayout.BeginHorizontal();

            if (string.IsNullOrEmpty(value))
            {
                value = defaultPath.FixSlashes();
            }

            string displayValue = value.Replace(rootPath, "");
            EditorGUILayout.LabelField(label, new GUIContent(displayValue), EditorStyles.textField, GUILayout.MinWidth(20));

            if (ResetButton(EditorStyles.miniButtonMid))
            {
                value = defaultPath.FixSlashes();
            }

            if (FolderPanelButton(EditorStyles.miniButtonMid))
            {
                value = ExGUIUtility.OpenFolderPanel(value, rootPath);
            }

            if (OpenFolderButton(EditorStyles.miniButtonRight))
            {
                ExGUIUtility.OpenFolder(value, defaultPath);
            }

            GUILayout.EndHorizontal();

            return value;
        }

        #endregion Path Fields 


        #region Toggle Selectors
        public static void TrueOrFalseSelector(string label, Action<bool> action) => ToggleSelector(label, action, "True", "False");
        public static void SetOrUnsetSelector(string label, Action<bool> action) => ToggleSelector(label, action, "Set", "Unset");

        public static void ToggleSelector(string label, Action<bool> action, string yes, string no)
        {
            GUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(label);

                float buttonWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) / 2;
                buttonWidth -= kToggleSelectorXOffset * 2;

                if (GUILayout.Button(yes, GUILayout.Width(buttonWidth)))
                {
                    action(true);
                }
                if (GUILayout.Button(no, GUILayout.Width(buttonWidth)))
                {
                    action(false);
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        public static void Selector(string label, params GUIButtonEntry[] buttonEntries)
        {
            GUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(label);

                float buttonWidth = (EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth) / buttonEntries.Length;
                buttonWidth -= kToggleSelectorXOffset * buttonEntries.Length;

                foreach (GUIButtonEntry entry in buttonEntries)
                {
                    if (GUILayout.Button(entry.Label, GUILayout.Width(buttonWidth)))
                    {
                        entry.OnClick?.Invoke();
                    }
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        #endregion Toggle Selectors

        #region Status Box (Improved Help Box)

        public static void StatusBox(string message, MessageStatus status, params GUILayoutOption[] options)
        {
            Texture stateIcon = status switch
            {
                MessageStatus.Okay => EditorIcons.StatusCheck,
                MessageStatus.Error => EditorIcons.StatusError,
                MessageStatus.Warning => EditorIcons.StatusWarning,
                _ => null,
            };

            GUILayout.BeginHorizontal(EditorStyles.helpBox, options);
            {
                if (stateIcon != null)
                {
                    GUILayout.Label(stateIcon, GUILayout.Width(30), GUILayout.Height(30));
                }

                GUILayout.Label(message, ExEditorStyles.statusBoxText, GUILayout.MinHeight(30), GUILayout.ExpandHeight(true));
            }
            GUILayout.EndHorizontal();
        }

        #endregion Status Box (Improved Help Box) 

        #region NullableFields

        public static string NullableTextField(string value, string defaultValue, GUIContent label)
        {
            const float toggleWidth = 14f;
            float padding = 4f;
            float prevLabelWidth = EditorGUIUtility.labelWidth;

            GUILayout.BeginHorizontal();

            bool enabled = value != null;
            bool newEnabled = EditorGUILayout.Toggle(GUIContent.none, enabled, GUILayout.Width(toggleWidth));
            if (newEnabled != enabled) value = newEnabled ? defaultValue : null;
            EditorGUIUtility.labelWidth = Mathf.Max(0, prevLabelWidth - toggleWidth - padding);

            EditorGUI.BeginDisabledGroup(!enabled);
            string newValue = EditorGUILayout.TextField(label, value);
            EditorGUI.EndDisabledGroup();

            EditorGUIUtility.labelWidth = prevLabelWidth;
            GUILayout.EndHorizontal();

            if (value == null) return null;
            value = newValue;
            return value;
        }

        public static T? NullableField<T>(string label, T? value, T defaultValue, Func<T, T> drawer) where T : struct
            => NullableField(new GUIContent(label), value, defaultValue, drawer);

        public static T? NullableField<T>(GUIContent label, T? value, T defaultValue, Func<T, T> drawer) where T : struct
        {
            const float toggleWidth = 14f;
            float padding = 4f;
            float prevLabelWidth = EditorGUIUtility.labelWidth;
            T newValue;

            GUILayout.BeginHorizontal();
            try
            {
                EditorGUIUtility.labelWidth = Mathf.Max(0, prevLabelWidth - toggleWidth - padding - (EditorGUI.indentLevel * 13f));

                bool enabled = value != null;
                bool newEnabled = EditorGUILayout.Toggle(GUIContent.none, enabled, GUILayout.Width(toggleWidth));
                if (newEnabled != enabled) value = newEnabled ? defaultValue : null;

                EditorGUILayout.PrefixLabel(label);

                EditorGUI.BeginDisabledGroup(!enabled);
                newValue = drawer.Invoke(value ?? defaultValue);
                EditorGUI.EndDisabledGroup();
            }
            finally
            {
                EditorGUIUtility.labelWidth = prevLabelWidth;
                GUILayout.EndHorizontal();
            }

            if (value == null) return null;
            value = newValue;
            return value;
        }

        public static T? NullableField<T>(T? value, T defaultValue, Func<T, T> drawer) where T : struct
        {
            const float toggleWidth = 14f;
            float padding = 4f;
            float prevLabelWidth = EditorGUIUtility.labelWidth;

            GUILayout.BeginHorizontal();

            bool enabled = value != null;
            bool newEnabled = EditorGUILayout.Toggle(GUIContent.none, enabled, GUILayout.Width(toggleWidth));
            if (newEnabled != enabled) value = newEnabled ? defaultValue : null;
            EditorGUIUtility.labelWidth = Mathf.Max(0, prevLabelWidth - toggleWidth - padding);

            EditorGUI.BeginDisabledGroup(!enabled);
            T newValue = drawer.Invoke(value ?? defaultValue);
            EditorGUI.EndDisabledGroup();

            EditorGUIUtility.labelWidth = prevLabelWidth;
            GUILayout.EndHorizontal();

            if (value == null) return null;
            value = newValue;
            return value;
        }

        #endregion NullableFields

        #region ButtonField

        public static bool ButtonField(string label)
        {
            bool pressed = false;

            GUILayout.BeginHorizontal();
            try
            {
                GUILayout.Space(EditorGUIUtility.labelWidth);
                GUILayout.Space(EditorGUI.indentLevel * 2f);
                pressed = GUILayout.Button(label, EditorStyles.miniButton);
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

            return pressed;
        }

        #endregion ButtonField

        #region ExpandableTextField

        public static string ExpandableTextField(string label, string value, GUIStyle style = null, params GUILayoutOption[] options)
            => ExpandableTextField(new GUIContent(label), value, style, options);
        public static string ExpandableTextField(GUIContent label, string value, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            try
            {
                value = EditorGUILayout.TextField(label, value, style ?? GUI.skin.textField, options);
                if (GUILayout.Button(EditorIcons.Pick, ExEditorStyles.miniButton))
                {
                    EditTextWindow.Show(label.text, value, onEdited, new Vector2(1200, 1200));
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }

            return value;

            void onEdited(string newValue) => value = newValue;
        }


        #endregion ExpandableTextField
    }
}

