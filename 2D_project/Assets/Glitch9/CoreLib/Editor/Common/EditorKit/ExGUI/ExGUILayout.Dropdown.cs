using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public partial class ExGUILayout
    {
        public static T EnumPopup<T>(T value, params GUILayoutOption[] options) where T : Enum
            => EnumPopup(GUIContent.none, value, options);

        public static T EnumPopup<T>(string label, T value, params GUILayoutOption[] options) where T : Enum
            => EnumPopup(new GUIContent(label), value, options);

        public static T EnumPopupWithDefault<T>(string label, T value, T defaultValue) where T : Enum
            => EnumPopupWithDefault(new GUIContent(label), value, defaultValue);

        public static T EnumPopup<T>(GUIContent label, T value, params GUILayoutOption[] options) where T : Enum
        {
            GUILayout.BeginHorizontal();
            T newValue = EnumPopupINTERNAL(label, value, options);
            GUILayout.EndHorizontal();
            return newValue;
        }

        public static T EnumPopupWithDefault<T>(GUIContent label, T value, T defaultValue) where T : Enum
        {
            GUILayout.BeginHorizontal();
            T newValue = EnumPopupINTERNAL(label, value);
            if (ResetButton()) newValue = defaultValue;
            GUILayout.EndHorizontal();
            return newValue;
        }

        private static T EnumPopupINTERNAL<T>(GUIContent label, T value, params GUILayoutOption[] options) where T : Enum
        {
            Type enumType = typeof(T);
            string[] displayNames = EnumUtils.GetDisplayNames(enumType);
            Array enumValues = Enum.GetValues(enumType);

            // 값이 Enum 정의에 없는 경우 처리
            int enumIndex = Array.IndexOf(enumValues, value);
            if (enumIndex < 0) enumIndex = 0;

            // Index 유효성 체크
            enumIndex = Mathf.Clamp(enumIndex, 0, displayNames.Length - 1);

            // Popup 표시
            int newIndex = EditorGUILayout.Popup(label, enumIndex, displayNames, options);
            newIndex = Mathf.Clamp(newIndex, 0, enumValues.Length - 1);

            return (T)enumValues.GetValue(newIndex);
        }

        public static void EnumPopupWithDefault<T>(string label, SerializedProperty property, T defaultValue) where T : Enum
            => EnumPopupWithDefault(new GUIContent(label), property, defaultValue);

        public static void EnumPopupWithDefault<T>(GUIContent label, SerializedProperty property, T defaultValue) where T : Enum
        {
            if (property == null || property.propertyType != SerializedPropertyType.Enum)
            {
                Debug.LogError("SerializedProperty must be of enum type.");
                return;
            }

            GUILayout.BeginHorizontal();

            string[] displayNames = property.enumDisplayNames;
            int enumIndex = property.enumValueIndex;
            int newEnumIndex = EditorGUILayout.Popup(label, enumIndex, displayNames);

            if (newEnumIndex != enumIndex)
            {
                property.enumValueIndex = newEnumIndex;
            }

            if (ResetButton()) property.enumValueIndex = Convert.ToInt32(defaultValue);

            GUILayout.EndHorizontal();
        }

        private static T GenericDropdownField<T>(T currentValue, IList<T> list, GUIContent label = null, params GUILayoutOption[] options)
        {
            if (list.IsNullOrEmpty())
            {
                EditorGUILayout.HelpBox("No list found.", MessageType.None);
                return default;
            }

            currentValue ??= list.First();

            try
            {
                int index = list.IndexOf(currentValue);
                List<string> stringArray = list.Select(enumValue => enumValue.ToString()).ToList();
                index = EditorGUILayout.Popup(label, index, stringArray.ToArray(), options);
                if (index < 0) index = 0;
                return list[index];
            }
            catch (Exception e)
            {
                EditorGUILayout.HelpBox(e.Message, MessageType.Error);
                return default;
            }
        }


        public static string StringListDropdown(string currentValue, List<string> list, GUIContent label = null, params GUILayoutOption[] options)
            => GenericDropdownField(currentValue, list, label, options);

        public static string StringListDropdown(string currentValue, string[] array, GUIContent label = null, params GUILayoutOption[] options)
            => GenericDropdownField(currentValue, array, label, options);

        public static TEnum ExtendedEnumPopup<TEnum>(TEnum selected, IEnumerable<TEnum> displayOptions, GUIContent label = null, GUIStyle style = null, params GUILayoutOption[] options) where TEnum : Enum
            => ExtendedEnumPopup(selected, displayOptions, null, label, style, options);

        public static TEnum ExtendedEnumPopup<TEnum>(TEnum selected, IEnumerable<TEnum> displayOptions, string[] displayNames, GUIContent label = null, GUIStyle style = null, params GUILayoutOption[] options) where TEnum : Enum
        {
            var values = displayOptions.ToArray();
            // Display only a subset of names if ignoring default 
            string[] displayedOptions = displayOptions.Select(v => v.ToString()).ToArray();
            displayNames ??= displayOptions.Select(v => v.GetInspectorName()).ToArray();

            int selectedIndex = Array.IndexOf(values, selected);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, values.Length - 1);

            style ??= EditorStyles.popup;
            int newIndex;

            if (label == null)
            {
                newIndex = EditorGUILayout.Popup(selectedIndex: selectedIndex, displayedOptions: displayNames, style: style, options);
            }
            else
            {
                newIndex = EditorGUILayout.Popup(label.text, selectedIndex: selectedIndex, displayedOptions: displayNames, style: style, options);
            }

            // Convert back to original enum index
            return (TEnum)Enum.Parse(typeof(TEnum), displayedOptions[newIndex]);
        }

        public static TEnum ExtendedEnumPopup<TEnum>(TEnum selected, GUIContent label = null, bool ignoreDefault = true, params GUILayoutOption[] options) where TEnum : Enum
        {
            label ??= GUIContent.none;

            string[] names = EnumUtils.GetDisplayNames(typeof(TEnum));
            int selectedIndex = Convert.ToInt32(selected);

            if (ignoreDefault && selectedIndex == 0)
                selectedIndex = 1;

            // Display only a subset of names if ignoring default
            string[] displayedNames = ignoreDefault ? names.Skip(1).ToArray() : names;

            int adjustedIndex = ignoreDefault ? selectedIndex - 1 : selectedIndex;

            int newIndex = EditorGUILayout.Popup(label, adjustedIndex, displayedNames, options);

            // Convert back to original enum index
            int actualIndex = ignoreDefault ? newIndex + 1 : newIndex;

            return (TEnum)Enum.ToObject(typeof(TEnum), actualIndex);
        }
    }
}