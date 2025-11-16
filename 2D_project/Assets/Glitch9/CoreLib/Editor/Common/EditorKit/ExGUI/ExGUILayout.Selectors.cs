using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public partial class ExGUILayout
    {
        public static T EnumSelector<T>(string label, T enumValue, int startIndex = 0, params GUILayoutOption[] options) where T : Enum
            => EnumSelector(new GUIContent(label), enumValue, startIndex, options);

        public static T EnumSelector<T>(GUIContent label, T enumValue, int startIndex = 0, params GUILayoutOption[] options) where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            //string[] names = Enum.GetNames(typeof(T));
            // get inspector names instead
            List<string> names = new();
            foreach (var value in values)
            {
                T e = (T)value;
                string name = e.GetInspectorName();
                names.Add(name);
            }

            int currentIndex = Array.IndexOf(values, enumValue);
            int selectedIndex = currentIndex;

            EditorGUILayout.BeginHorizontal(options);

            float viewWidth = EditorGUIUtility.currentViewWidth - 21f;

            if (label != null)
            {
                EditorGUILayout.PrefixLabel(label);
                viewWidth -= EditorGUIUtility.labelWidth;
            }

            float btnWidth = viewWidth / (values.Length - startIndex);

            for (int i = startIndex; i < values.Length; i++)
            {
                bool isSelected = i == selectedIndex;

                bool clicked;
                if (i == startIndex)
                {
                    clicked = ToggleLeft(names[i], isSelected, options: GUILayout.Width(btnWidth));
                }
                else if (i == values.Length - 1)
                {
                    clicked = ToggleRight(names[i], isSelected, options: GUILayout.Width(btnWidth));
                }
                else
                {
                    clicked = ToggleMid(names[i], isSelected, options: GUILayout.Width(btnWidth));
                }

                if (clicked)
                {
                    selectedIndex = i;
                }
            }

            EditorGUILayout.EndHorizontal();

            return (T)values.GetValue(selectedIndex);
        }


        public static T FlagSelector<T>(T enumValue, int startIndex = 1, params GUILayoutOption[] options) where T : Enum
        {
            // Get all values and names of the ApiEnumDE
            Array values = Enum.GetValues(typeof(T));
            string[] names = Enum.GetNames(typeof(T));

            // Convert the current selected value to int
            int currentIntValue = Convert.ToInt32(enumValue);
            int newIntValue = currentIntValue;

            EditorGUILayout.BeginHorizontal();

            if (values.Length > startIndex)
            {
                for (int i = startIndex; i < values.Length; i++)
                {
                    // Determine if the current flag is set
                    bool flagSet = (currentIntValue & (int)values.GetValue(i)) != 0;

                    if (i == startIndex)
                    {
                        flagSet = ToggleLeft(names[i], flagSet, options: options);
                    }
                    else if (i == values.Length - 1)
                    {
                        flagSet = ToggleRight(names[i], flagSet, options: options);
                    }
                    else
                    {
                        flagSet = ToggleMid(names[i], flagSet, options: options);
                    }

                    // Update the int value based on toggle button state
                    if (flagSet)
                    {
                        newIntValue |= (int)values.GetValue(i);
                    }
                    else
                    {
                        newIntValue &= ~(int)values.GetValue(i);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            // Convert the int value back to the enum type and return
            return (T)Enum.ToObject(typeof(T), newIntValue);
        }


    }
}

