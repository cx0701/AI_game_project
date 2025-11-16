using Glitch9.EditorKit;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal static class VoiceCatalogueSettings
    {
        private static readonly EPrefs<AIProvider> kApiProvider = new("AIDevKit.VoiceCatalogue.ApiProvider", AIProvider.None);
        private static readonly EPrefs<VoiceCategory> kVoiceCategory = new("AIDevKit.VoiceCatalogue.VoiceCategory", VoiceCategory.None);
        private static readonly EPrefs<VoiceGender> kVoiceGender = new("AIDevKit.VoiceCatalogue.VoiceGender", VoiceGender.None);
        private static readonly EPrefs<VoiceType> kVoiceType = new("AIDevKit.VoiceCatalogue.VoiceType", VoiceType.None);
        private static readonly EPrefs<VoiceAge> kVoiceAge = new("AIDevKit.VoiceCatalogue.VoiceAge", VoiceAge.None);
        private static readonly EPrefs<SystemLanguage> kVoiceLanguage = new("AIDevKit.VoiceCatalogue.VoiceLanguage", SystemLanguage.Unknown);
        private static readonly EPrefs<bool> kFeatured = new("AIDevKit.VoiceCatalogue.Featured", false);

        internal static AIProvider ApiProvider
        {
            get => kApiProvider.Value;
            set => kApiProvider.Value = value;
        }

        internal static VoiceCategory VoiceCategory
        {
            get => kVoiceCategory.Value;
            set => kVoiceCategory.Value = value;
        }

        internal static VoiceGender VoiceGender
        {
            get => kVoiceGender.Value;
            set => kVoiceGender.Value = value;
        }

        internal static VoiceType VoiceType
        {
            get => kVoiceType.Value;
            set => kVoiceType.Value = value;
        }

        internal static VoiceAge VoiceAge
        {
            get => kVoiceAge.Value;
            set => kVoiceAge.Value = value;
        }

        internal static SystemLanguage VoiceLanguage
        {
            get => kVoiceLanguage.Value;
            set => kVoiceLanguage.Value = value;
        }

        internal static bool OnlyShowFeaturedVoices
        {
            get => kFeatured.Value;
            set => kFeatured.Value = value;
        }

        private static readonly EPrefs<bool> kShowDeprecatedVoices = new("AIDevKit.VoiceCatalogue.ShowDeprecatedVoices", true);
        private static readonly EPrefs<bool> kOnlyShowOfficialVoices = new("AIDevKit.VoiceCatalogue.OnlyShowOfficialVoices", false);
        private static readonly EPrefs<bool> kOnlyShowCustomVoices = new("AIDevKit.VoiceCatalogue.OnlyShowCustomVoices", false);
        private static readonly EPrefs<bool> kOnlyShowMissingVoices = new("AIDevKit.VoiceCatalogue.OnlyShowMissingVoices", false);
        private static readonly EPrefs<bool> kOnlyShowDefaultVoices = new("AIDevKit.VoiceCatalogue.OnlyShowDefaultVoices", false);
        private static readonly EPrefs<bool> kShowMyLibrary = new("AIDevKit.VoiceCatalogue.ShowMyLibrary", false);

        internal static bool ShowDeprecatedVoices
        {
            get => kShowDeprecatedVoices.Value;
            set => kShowDeprecatedVoices.Value = value;
        }

        internal static bool OnlyShowOfficialVoices
        {
            get => kOnlyShowOfficialVoices.Value;
            set => kOnlyShowOfficialVoices.Value = value;
        }

        internal static bool OnlyShowCustomVoices
        {
            get => kOnlyShowCustomVoices.Value;
            set => kOnlyShowCustomVoices.Value = value;
        }

        internal static bool OnlyShowMissingVoices
        {
            get => kOnlyShowMissingVoices.Value;
            set => kOnlyShowMissingVoices.Value = value;
        }

        internal static bool OnlyShowDefaultVoices
        {
            get => kOnlyShowDefaultVoices.Value;
            set => kOnlyShowDefaultVoices.Value = value;
        }

        internal static bool OnlyShowMyLibrary
        {
            get => kShowMyLibrary.Value;
            set => kShowMyLibrary.Value = value;
        }
    }
}