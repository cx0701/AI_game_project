using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// ReSharper disable All
#pragma warning disable IDE1006

namespace Glitch9.EditorKit
{
    public static partial class ExGUI
    {
        internal const float kTolerance = 0.0001f;
        internal static bool IsDarkMode => EditorGUIUtility.isProSkin;
        private static readonly int kProgressBarHash = "ProgressBar".GetHashCode();

        #region Box Styles
        private static GUIStyle _box;
        public static GUIStyle box
        {
            get
            {
                _box ??= new GUIStyle(GUI.skin.box)
                {
                    margin = new RectOffset(2, 2, 2, 2),
                    border = new RectOffset(10, 10, 10, 10),
                    padding = new RectOffset(6, 6, 6, 6)
                };
                return _box;
            }
        }

        public static GUIStyle Box(int margin, int padding, GUIColor color = GUIColor.None) // Margin goes first. 
            => ExEditorStyles.Box(
                color,
                new RectOffset(margin, margin, margin, margin),
                new RectOffset(padding, padding, padding, padding));

        #endregion Box Styles

        #region Editor Colors

        public static Color green = new(0.4f, 1f, 0.8f, 1f);
        public static Color red = new(1f, 0.5f, 0.6f, 1f);

        #endregion Editor Colors

        #region Fields 

        public static void TitleField(Rect rect, string label)
        {
            rect.y = rect.y;
            EditorGUI.LabelField(rect, label, ExEditorStyles.title);
            float thickness = 1.2f;
            float height = 5f;
            Rect r = new(rect.x, rect.y + rect.height - 5, rect.width, height)
            {
                height = thickness
            };
            r.width -= 4;
            EditorGUI.DrawRect(r, Color.gray);
        }

        public static UnixTime UnixTimeField(Rect rect, GUIContent label, UnixTime unixTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false)
        {
            float originalX = rect.x;
            const float SMALL_SPACE = 10f;
            const float LARGE_SPACE = 20f;

            if (label != null)
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, labelWidth, rect.height), label);
                rect.x += labelWidth;
            }

            int YY = unixTime.Year;
            int MM = unixTime.Month;
            int DD = unixTime.Day;
            int hh = unixTime.Hour;
            int mm = unixTime.Minute;
            int ss = unixTime.Second;

            if (year)
            {
                rect.width = 50;
                YY = EditorGUI.IntField(rect, YY);
                rect.x += rect.width + SMALL_SPACE;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, SMALL_SPACE, rect.height), "-");
                rect.x += SMALL_SPACE;
            }

            if (month)
            {
                rect.width = 30;
                MM = EditorGUI.IntField(rect, MM);
                rect.x += rect.width + SMALL_SPACE;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, SMALL_SPACE, rect.height), "-");
                rect.x += SMALL_SPACE;
            }

            if (day)
            {
                rect.width = 30;
                DD = EditorGUI.IntField(rect, DD);
                rect.x += rect.width;
            }

            if (hour)
            {
                rect.x += LARGE_SPACE;
                rect.width = 30;
                hh = EditorGUI.IntField(rect, hh);
                rect.x += rect.width + SMALL_SPACE;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, SMALL_SPACE, rect.height), ":");
                rect.x += SMALL_SPACE;
            }

            if (minute)
            {
                rect.width = 30;
                mm = EditorGUI.IntField(rect, mm);
                rect.x += rect.width + SMALL_SPACE;
                EditorGUI.LabelField(new Rect(rect.x, rect.y, SMALL_SPACE, rect.height), ":");
                rect.x += SMALL_SPACE;
            }

            if (second)
            {
                rect.width = 30;
                ss = EditorGUI.IntField(rect, ss);
            }

            return new UnixTime(YY, MM, DD, hh, mm, ss);
        }

        #endregion Fields

        #region List Popups

        private static GUIStyle GetResizablePopupStyle(float height) => new(EditorStyles.popup) { fixedHeight = height };
        private static Rect DrawPrefixLabel(Rect rect, GUIContent label) => label != null ? EditorGUI.PrefixLabel(rect, label) : rect;

        public static T DrawPopup<T>(Rect rect, T currentValue, IList<T> options, GUIContent label = null)
        {
            if (options == null || options.Count == 0)
            {
                EditorGUI.HelpBox(rect, "No options available.", MessageType.None);
                return default;
            }

            rect = DrawPrefixLabel(rect, label);

            int index = options.IndexOf(currentValue);
            string[] labels = options.Select(o => o?.ToString() ?? "(null)").ToArray();
            int newIndex = EditorGUI.Popup(rect, Mathf.Max(index, 0), labels);
            return options[Mathf.Clamp(newIndex, 0, options.Count - 1)];
        }

        public static string StringPopup(Rect rect, string currentValue, IList<string> list, GUIContent label = null)
            => DrawPopup(rect, currentValue, list, label);

        public static T ResizablePopup<T>(Rect rect, T currentValue, IList<T> options, GUIContent label = null)
        {
            if (options == null || options.Count == 0)
            {
                EditorGUI.HelpBox(rect, "No options available.", MessageType.None);
                return default;
            }

            rect = DrawPrefixLabel(rect, label);

            int index = options.IndexOf(currentValue);
            string[] optionLabels = options.Select(o => o.ToString()).ToArray();
            GUIStyle style = GetResizablePopupStyle(rect.height);

            T selected = options[Mathf.Max(index, 0)];

            if (GUI.Button(rect, new GUIContent(optionLabels[Mathf.Max(index, 0)]), style))
            {
                GenericMenu menu = new();
                for (int i = 0; i < options.Count; i++)
                {
                    int iCopy = i;
                    menu.AddItem(new GUIContent(optionLabels[i]), i == index, () => selected = options[iCopy]);
                }
                menu.DropDown(rect);
            }

            return selected;
        }

        public static int ResizableIntPopup(Rect rect, int selectedValue, int[] values, string prefix = null, GUIContent label = null, GUIStyle style = null)
        {
            rect = DrawPrefixLabel(rect, label);
            int index = Array.IndexOf(values, selectedValue);
            if (index < 0) index = 0;

            string[] labels = values.Select(v => v.ToString()).ToArray();
            string display = string.IsNullOrEmpty(prefix) ? labels[index] : $"{prefix} {labels[index]}";

            GUIStyle finalStyle = style ?? GetResizablePopupStyle(rect.height);
            int selected = selectedValue;

            if (GUI.Button(rect, new GUIContent(display), finalStyle))
            {
                GenericMenu menu = new();
                for (int i = 0; i < values.Length; i++)
                {
                    int local = values[i];
                    menu.AddItem(new GUIContent(labels[i]), i == index, () => selected = local);
                }
                menu.DropDown(rect);
            }

            return selected;
        }

        public static TEnum ResizableEnumPopup<TEnum>(Rect rect, TEnum selected, GUIContent label = null) where TEnum : Enum
        {
            string[] names = Enum.GetNames(typeof(TEnum));
            rect = DrawPrefixLabel(rect, label);

            GUIStyle style = GetResizablePopupStyle(rect.height);
            TEnum selectedCopy = selected;

            if (GUI.Button(rect, new GUIContent(selected.ToString()), style))
            {
                GenericMenu menu = new();
                foreach (string name in names)
                {
                    TEnum enumValue = (TEnum)Enum.Parse(typeof(TEnum), name);
                    menu.AddItem(new GUIContent(name), Equals(selected, enumValue), () => selectedCopy = enumValue);
                }
                menu.DropDown(rect);
            }

            return selectedCopy;
        }

        #endregion

        #region Customized ProgressBar 

        public static bool ProgressBar(
            Rect position,
            float value,
            GUIContent content,
            GUIStyle progressBarBackgroundStyle,
            GUIStyle progressBarStyle,
            GUIStyle progressBarTextStyle)
        {
            bool isHover = position.Contains(Event.current.mousePosition);
            switch (Event.current.GetTypeForControl(GUIUtility.GetControlID(kProgressBarHash, FocusType.Keyboard, position)))
            {
                case EventType.MouseDown:
                    if (isHover)
                    {
                        Event.current.Use();
                        return true;
                    }
                    break;
                case EventType.Repaint:
                    progressBarBackgroundStyle.Draw(position, isHover, false, false, false);
                    if ((double)value > 0.0)
                    {
                        value = Mathf.Clamp01(value);
                        Rect position1 = new(position);
                        position1.width *= value;
                        if ((double)position1.width >= 1.0)
                            progressBarStyle.Draw(position1, GUIContent.none, isHover, false, false, false);
                    }
                    else if ((double)value == -1.0)
                    {
                        float width = position.width * 0.2f;
                        float num1 = width / 2f;
                        float num2 = Mathf.Cos((float)EditorApplication.timeSinceStartup * 2f);
                        float num3 = position.x + num1;
                        float num4 = (float)(((double)(position.xMax - num1) - (double)num3) / 2.0);
                        float num5 = num4 * num2;
                        Rect position2 = new(position.x + num5 + num4, position.y, width, position.height);
                        progressBarStyle.Draw(position2, GUIContent.none, isHover, false, false, false);
                    }
                    GUIContent content1 = content;
                    float x = progressBarTextStyle.CalcSize(content1).x;
                    if ((double)x > (double)position.width)
                    {
                        int num6 = (int)((double)position.width / (double)x * (double)content.text.Length);
                        int num7 = 0;
                        do
                        {
                            int length = num6 / 2 - 2 - num7;
                            content1.text = content.text.Substring(0, length) + "..." + content.text.Substring(content.text.Length - length, length);
                            ++num7;
                        }
                        while ((double)progressBarTextStyle.CalcSize(content1).x > (double)position.width);
                    }
                    progressBarTextStyle.Draw(position, content1, isHover, false, false, false);
                    break;
            }
            return false;
        }

        #endregion Customized ProgressBar  
    }
}
#pragma warning restore IDE1006