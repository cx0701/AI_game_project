using System.Collections.Generic;
using Glitch9.EditorKit.IMGUI;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal static class VoiceCatalogueWindowUtil
    {
        private static GUIStyle _searchBarStyle;
        private static List<SystemLanguage> _availableLanguages;
        private static string[] _availableLanguegesDisplayNames;
        private static List<AIProvider> _availableApis;

        internal static GUIStyle GetSearchBarStyle()
        {
            if (_searchBarStyle != null) return _searchBarStyle;
            _searchBarStyle = new GUIStyle(TreeViewStyles.BottomBarStyle) { fixedHeight = 56f };
            return _searchBarStyle;
        }

        internal static List<SystemLanguage> GetAvailableLanguages()
        {
            if (_availableLanguages == null)
            {
                List<string> displayNames = new() {
                    "All", // as SystemLanguage.Unknown
                };

                _availableLanguages = new()
                {
                    SystemLanguage.Unknown
                };

                foreach (var entry in VoiceCatalogue.Instance.Entries)
                {
                    if (entry == null) continue;
                    var language = entry.Language;

                    if (!_availableLanguages.Contains(language))
                    {
                        _availableLanguages.Add(language);
                        displayNames.Add(language.ToString());
                    }
                }

                _availableLanguegesDisplayNames = displayNames.ToArray();
            }

            return _availableLanguages;
        }

        internal static string[] GetAvailableLanguagesDisplayNames()
        {
            if (_availableLanguegesDisplayNames == null) GetAvailableLanguages();
            return _availableLanguegesDisplayNames;
        }

        internal static List<AIProvider> GetAvailableApis()
        {
            if (_availableApis == null)
            {
                _availableApis = new List<AIProvider>() { AIProvider.All };
                foreach (var entry in VoiceCatalogue.Instance.Entries)
                {
                    if (entry == null) continue;
                    var provider = entry.Api;

                    if (!_availableApis.Contains(provider))
                    {
                        _availableApis.Add(provider);
                    }
                }
            }

            return _availableApis;
        }
    }
}