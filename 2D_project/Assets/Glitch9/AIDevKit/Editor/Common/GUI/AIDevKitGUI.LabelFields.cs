using System;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal partial class AIDevKitGUI
    {
        internal static void LabelField(string label, string value)
        {
            if (string.IsNullOrEmpty(value)) value = "-";

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label, AIDevKitStyles.Label);
                if (GUILayout.Button(value, AIDevKitStyles.Label))
                {
                    EditorGUIUtility.systemCopyBuffer = value;
                    Debug.Log($"Copied '{value}' to clipboard.");
                }
            }
            GUILayout.EndHorizontal();
        }

        internal static void LabelField(string label, GUIContent value)
        {
            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label, AIDevKitStyles.Label);
                if (GUILayout.Button(value, AIDevKitStyles.Label))
                {
                    EditorGUIUtility.systemCopyBuffer = value.text;
                    Debug.Log($"Copied '{value}' to clipboard.");
                }
            }
            GUILayout.EndHorizontal();
        }

        internal static void TokenField(string label, int? value)
        {
            if (value.HasValue && value.Value > 2)
            {
                EditorGUILayout.LabelField(label, $"{value.Value} tokens", AIDevKitStyles.Label);
            }
            else
            {
                EditorGUILayout.LabelField(label, "N/A", AIDevKitStyles.Label);
            }
        }

        internal static void LabelField<T>(string label, T? value) where T : struct, Enum
        {
            string display = value.HasValue ? value.Value.GetInspectorName() : "-";
            LabelField(label, display);
        }

        internal static void LabelField<T>(string label, T value) where T : Enum
        {
            string display = value.GetInspectorName();
            LabelField(label, display);
        }

        internal static void LabelField(string label, int? value)
        {
            string display = value.HasValue ? value.Value.ToString() : "-";
            LabelField(label, display);
        }

        internal static void LabelField(string label, bool? value)
        {
            string display = value.HasValue ? value.Value ? "True" : "False" : "-";
            EditorGUILayout.LabelField(label, display, AIDevKitStyles.Label);
        }

        internal static void CurrencyField(string label, Currency value)
        {
            string display;

            if (value == null)
            {
                display = "-";
            }
            else if (value == -1)
            {
                display = "Free";
            }
            else
            {
                display = value.ToString(value.CurrencyCode);
            }

            GUILayout.BeginHorizontal();
            {
                EditorGUILayout.PrefixLabel(label, AIDevKitStyles.Label);
                EditorGUILayout.LabelField(display, AIDevKitStyles.Label);

                value.CurrencyCode = ExGUILayout.ExtendedEnumPopup(value.CurrencyCode, AIDevKitGUIUtility.SelectedCurrencyCodes, null, null, GUILayout.Width(50));
            }
            GUILayout.EndHorizontal();
        }
    }
}