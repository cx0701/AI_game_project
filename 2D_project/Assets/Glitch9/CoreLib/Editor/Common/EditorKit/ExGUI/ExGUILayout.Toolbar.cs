using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public partial class ExGUILayout
    {
        public static int StringListToolbar(int currentId, List<string> list, GUIContent label = null, int maxColumns = 3, params GUILayoutOption[] options)
        {
            if (list == null || list.Count == 0)
            {
                EditorGUILayout.HelpBox("No list found.", MessageType.Warning);
                return -1;
            }

            int index = currentId;

            float totalWidth = 0;
            GUILayout.BeginHorizontal();

            if (label != null)
            {
                GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.labelWidth));
            }

            int columnCount = 0;  // New variable to track the number of columns

            for (int i = 0; i < list.Count; i++)
            {
                float buttonWidth = EditorStyles.toolbarButton.CalcSize(new GUIContent(list[i])).x;
                if (totalWidth + buttonWidth > EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth || columnCount >= maxColumns)
                {
                    // Wrap to the next line if the button will exceed the width of the inspector or if the maxColumns limit is reached.
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    if (label != null)
                    {
                        // Empty space
                        GUILayout.Space(EditorGUIUtility.labelWidth);
                    }

                    totalWidth = 0;
                    columnCount = 0;  // Reset column count for the new line
                }

                // Use toggle style buttons for the toolbar buttons
                bool isActive = GUILayout.Toggle(index == i, list[i], EditorStyles.miniButton, options);
                if (isActive) index = i;

                totalWidth += buttonWidth;
                columnCount++;  // Increment column count
            }
            GUILayout.EndHorizontal();

            return index;
        }

        public static bool EnumListToolbar<T>(List<T> selected, out List<T> updated, params GUILayoutOption[] options) where T : Enum
        {
            updated = selected;
            bool changed = false;

            // Get all values and names of the ApiEnumDE
            Array values = Enum.GetValues(typeof(T));
            string[] names = EnumUtils.GetDisplayNames(typeof(T));

            EditorGUILayout.BeginHorizontal();

            for (int i = 0; i < values.Length; i++)
            {
                // Determine if the current flag is set
                bool flagSet = selected.Contains((T)values.GetValue(i));

                // Use toggle style buttons for the toolbar buttons
                bool newFlagSet = GUILayout.Toggle(flagSet, names[i], EditorStyles.toolbarButton, options);

                if (flagSet != newFlagSet)
                {
                    changed = true;
                    if (newFlagSet) updated.Add((T)values.GetValue(i));
                    else updated.Remove((T)values.GetValue(i));
                }
            }

            EditorGUILayout.EndHorizontal();

            return changed;
        }

        /// <summary>
        /// Enum변수가 변경되면 true를 리턴한다.
        /// </summary>
        public static bool EnumToolbar<T>(T enumValue, out T newEnum, GUIStyle toolbarStyle, params GUILayoutOption[] options) where T : Enum
        {

            string[] enumNames = EnumUtils.GetDisplayNames(typeof(T));
            int currentIndex = Convert.ToInt32(enumValue);

            toolbarStyle ??= new(EditorStyles.toolbarButton);
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            int newIndex = GUILayout.Toolbar(currentIndex, enumNames, toolbarStyle, options);

            GUILayout.EndHorizontal();

            if (newIndex != currentIndex)
            {
                newEnum = (T)Enum.ToObject(typeof(T), newIndex);
                return true;
            }

            newEnum = enumValue;
            return false;
        }

        public static bool EnumToolbar<T>(T enumValue, out T newEnum, params GUILayoutOption[] options) where T : Enum
            => EnumToolbar(enumValue, out newEnum, null, options);
    }
}

