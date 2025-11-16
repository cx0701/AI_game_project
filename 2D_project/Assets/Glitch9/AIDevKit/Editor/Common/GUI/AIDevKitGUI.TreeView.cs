using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal partial class AIDevKitGUI
    {
        internal static void DrawCapability(Rect cellRect, ModelCapability cap)
        {
            List<GUIContent> contents = AIDevKitGUIUtility.GetCapabilityContents(cap);

            if (contents.Count == 0)
            {
                GUI.Label(cellRect, "None");
                return;
            }

            for (int i = 0; i < contents.Count; i++)
            {
                var content = contents[i];
                if (content == null) continue;
                DrawIconContentINTERNAL(i, cellRect, content);
            }
        }

        internal static void DrawProvider(Rect cellRect, AIProvider provider)
        {
            string displayName = provider.GetInspectorName();
            Texture2D icon = AIDevKitGUIUtility.GetProviderIcon(provider);

            if (icon != null)
            {
                EditorGUI.LabelField(cellRect, new GUIContent(displayName, AIDevKitGUIUtility.GetProviderIcon(provider)));
            }
            else
            {
                EditorGUI.LabelField(cellRect, displayName);
            }
        }


        internal static void DrawDisplayName(Rect cellRect, AIProvider provider, string displayName)
        {
            displayName ??= "Unknown";
            Texture2D icon = AIDevKitGUIUtility.GetProviderIcon(provider);

            if (icon != null)
            {
                EditorGUI.LabelField(cellRect, new GUIContent(displayName, AIDevKitGUIUtility.GetProviderIcon(provider)));
            }
            else
            {
                EditorGUI.LabelField(cellRect, displayName);
            }
        }

        private static void DrawIconContentINTERNAL(int index, Rect rect, GUIContent content)
        {
            if (content == null) return;
            const float ICON_SIZE = 18f;
            float x = rect.x + index * ICON_SIZE;
            rect = new Rect(x, rect.y, ICON_SIZE, ICON_SIZE);
            EditorGUI.LabelField(rect, new GUIContent(content));
        }

        internal static void DrawTokenCost(Rect cellRect, double value)
        {
            if (value == AIDevKitConfig.kFreePriceMagicNumber) // -99
            {
                EditorGUI.LabelField(cellRect, "Free");
                return;
            }

            if (value < 0)
            {
                EditorGUI.LabelField(cellRect, "-");
                return;
            }

            string display = value.ToString("C2", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
            EditorGUI.LabelField(cellRect, display);
        }
    }
}