using System.Collections.Generic;
using System.Linq;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal abstract class AssetPopupGUI<TAsset, TFilter>
        where TAsset : AIDevKitAsset
        where TFilter : IAIDevKitAssetFilter<TAsset>
    {
        private readonly Dictionary<TFilter, Dictionary<AIProvider, List<TAsset>>> _cache = new();
        private static bool _forceUpdateCache = false;

        private Dictionary<AIProvider, List<TAsset>> GetCachedAssets(TFilter filter)
        {
            if (_forceUpdateCache || !_cache.TryGetValue(filter, out var result))
            {
                result = GetFilteredAssets(filter);
                _cache[filter] = result;
                _forceUpdateCache = false;
            }
            return result;
        }

        internal static void ForceUpdateCache() => _forceUpdateCache = true;
        protected abstract Dictionary<AIProvider, List<TAsset>> GetFilteredAssets(TFilter filter);
        protected abstract TAsset GetDefaultAssetId(TFilter filter);

        internal TAsset Draw(TAsset selected, TFilter filter, GUIContent label = null, GUIStyle style = null, float offset = 2f)
        {
            selected ??= GetDefaultAssetId(filter);
            TAsset model = null;
            int savedIndent = EditorGUI.indentLevel;

            GUILayout.BeginHorizontal();
            try
            {
                DrawLabelINTERNAL(label);
                EditorGUI.indentLevel = 0;
                GUILayout.Space(offset);
                model = DrawFieldINTERNAL(selected, filter, style);
            }
            finally
            {
                // restore indent level
                EditorGUI.indentLevel = savedIndent;
                GUILayout.EndHorizontal();
            }

            return model;
        }

        private void DrawLabelINTERNAL(GUIContent label)
        {
            if (label == null) return;
            EditorGUILayout.PrefixLabel(label);
        }

        private TAsset DrawFieldINTERNAL(TAsset selected, TFilter filter, GUIStyle style)
        {
            var allAssets = GetCachedAssets(filter);

            if (allAssets.Count == 0)
            {
                DrawNoModelsAvailable();
                return null;
            }

            // Step 1: Fallback to first valid model if selected is null
            if (selected == null || string.IsNullOrEmpty(selected.Id))
            {
                selected = allAssets.First().Value.FirstOrDefault();
            }

            // Step 2: Ensure selected.Api is valid
            if (selected == null || !allAssets.ContainsKey(selected.Api))
            {
                selected = allAssets.First().Value.FirstOrDefault();
            }

            // Step 3: Fallback if selected.Api exists but its list is empty
            if (selected == null || allAssets[selected.Api].IsNullOrEmpty())
            {
                selected = allAssets.FirstOrDefault(kv => kv.Value?.Count > 0).Value?.FirstOrDefault();
            }

            // If still null after all fallback attempts
            if (selected == null)
            {
                DrawNoModelsAvailable();
                return null;
            }

            List<string> displayOptions = allAssets[selected.Api].Select(m => m.Name).ToList();
            bool apiSpecified = filter.Api != AIProvider.All;

            if (!apiSpecified)
            {
                AIProvider newApi = ExGUILayout.ExtendedEnumPopup(
                    selected: selected.Api,
                    displayOptions: allAssets.Keys,
                    label: null,
                    style: style,
                    GUILayout.Width(100f));

                if (newApi != selected.Api && allAssets[newApi].Count > 0)
                {
                    selected = allAssets[newApi][0];
                }
            }

            style ??= EditorStyles.popup;
            int selectedAssetIndex = allAssets[selected.Api].FindIndex(m => m.Id == selected.Id);
            if (selectedAssetIndex < 0) selectedAssetIndex = 0;
            int newAssetIndex = EditorGUILayout.Popup(selectedAssetIndex, displayOptions.ToArray(), style, GUILayout.ExpandWidth(true));

            if (newAssetIndex != selectedAssetIndex)
            {
                selected = allAssets[selected.Api][newAssetIndex];
            }

            return selected;
        }

        private void DrawNoModelsAvailable() => ExGUILayout.ErrorLabel($"Please add {typeof(TAsset).Name}s to your library");
    }
}