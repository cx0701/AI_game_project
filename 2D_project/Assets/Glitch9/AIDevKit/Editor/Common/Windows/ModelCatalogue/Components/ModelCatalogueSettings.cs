using Glitch9.EditorKit;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal static class ModelCatalogueSettings
    {
        private static readonly EPrefs<AIProvider> kApiProvider = new("AIDevKit.ModelCatalogue.ApiProvider", AIProvider.All);
        private static readonly EPrefs<AIProvider> kModelProvider = new("AIDevKit.ModelCatalogue.ModelProvider", AIProvider.All);

        // only-show options
        private static readonly EPrefs<bool> kOnlyShowMissingModels = new("AIDevKit.ModelCatalogue.OnlyShowMissingModels", false);
        private static readonly EPrefs<bool> kOnlyShowDefaultModels = new("AIDevKit.ModelCatalogue.OnlyShowDefaultModels", false);
        private static readonly EPrefs<bool> kOnlyShowMyLibrary = new("AIDevKit.ModelCatalogue.OnlyShowMyLibrary", false);
        private static readonly EPrefs<bool> kOnlyShowOfficialModels = new("AIDevKit.ModelCatalogue.OnlyShowOfficialModels", false);
        private static readonly EPrefs<bool> kOnlyShowCustomModels = new("AIDevKit.ModelCatalogue.OnlyShowCustomModels", false);

        // show options
        private static readonly EPrefs<bool> kShowLegacyModels = new("AIDevKit.ModelCatalogue.ShowLegacyModels", true);
        private static readonly EPrefs<bool> kShowDeprecatedModels = new("AIDevKit.ModelCatalogue.ShowDeprecatedModels", true);

        // filter options
        private static readonly EPrefs<bool> kTextGeneration = new("AIDevKit.ModelCatalogue.TextGeneration", true);
        private static readonly EPrefs<bool> kStructuredOutput = new("AIDevKit.ModelCatalogue.StructuredOutput", true);
        private static readonly EPrefs<bool> kFunctionCalling = new("AIDevKit.ModelCatalogue.FunctionCalling", true);
        private static readonly EPrefs<bool> kCodeExecution = new("AIDevKit.ModelCatalogue.CodeExecution", true);
        private static readonly EPrefs<bool> kFineTuning = new("AIDevKit.ModelCatalogue.FineTuning", true);
        private static readonly EPrefs<bool> kStreaming = new("AIDevKit.ModelCatalogue.Streaming", true);
        private static readonly EPrefs<bool> kImageGeneration = new("AIDevKit.ModelCatalogue.ImageGeneration", true);
        private static readonly EPrefs<bool> kImageInpainting = new("AIDevKit.ModelCatalogue.ImageInpainting", true);
        private static readonly EPrefs<bool> kSpeechGeneration = new("AIDevKit.ModelCatalogue.SpeechGeneration", true);
        private static readonly EPrefs<bool> kSpeechRecognition = new("AIDevKit.ModelCatalogue.SpeechRecognition", true);
        private static readonly EPrefs<bool> kSoundFXGeneration = new("AIDevKit.ModelCatalogue.SoundFXGeneration", true);
        private static readonly EPrefs<bool> kVoiceChanger = new("AIDevKit.ModelCatalogue.VoiceChanger", true);
        private static readonly EPrefs<bool> kVideoGeneration = new("AIDevKit.ModelCatalogue.VideoGeneration", true);
        private static readonly EPrefs<bool> kTextEmbedding = new("AIDevKit.ModelCatalogue.TextEmbedding", true);
        private static readonly EPrefs<bool> kModeration = new("AIDevKit.ModelCatalogue.Moderation", true);
        private static readonly EPrefs<bool> kSearch = new("AIDevKit.ModelCatalogue.Search", true);
        private static readonly EPrefs<bool> kRealtime = new("AIDevKit.ModelCatalogue.Realtime", true);
        private static readonly EPrefs<bool> kComputerUse = new("AIDevKit.ModelCatalogue.ComputerUse", true);

        internal static AIProvider ApiProvider { get => kApiProvider.Value; set => kApiProvider.Value = value; }
        internal static AIProvider ModelProvider { get => kModelProvider.Value; set => kModelProvider.Value = value; }
        internal static bool OnlyShowMissingModels { get => kOnlyShowMissingModels.Value; set => kOnlyShowMissingModels.Value = value; }
        internal static bool OnlyShowDefaultModels { get => kOnlyShowDefaultModels.Value; set => kOnlyShowDefaultModels.Value = value; }
        internal static bool OnlyShowMyLibrary { get => kOnlyShowMyLibrary.Value; set => kOnlyShowMyLibrary.Value = value; }
        internal static bool ShowDeprecatedModels { get => kShowDeprecatedModels.Value; set => kShowDeprecatedModels.Value = value; }
        internal static bool ShowLegacyModels { get => kShowLegacyModels.Value; set => kShowLegacyModels.Value = value; }
        internal static bool OnlyShowOfficialModels { get => kOnlyShowOfficialModels.Value; set => kOnlyShowOfficialModels.Value = value; }
        internal static bool OnlyShowCustomModels { get => kOnlyShowCustomModels.Value; set => kOnlyShowCustomModels.Value = value; }
        internal static bool TextGeneration { get => kTextGeneration.Value; set => kTextGeneration.Value = value; }
        internal static bool StructuredOutput { get => kStructuredOutput.Value; set => kStructuredOutput.Value = value; }
        internal static bool FunctionCalling { get => kFunctionCalling.Value; set => kFunctionCalling.Value = value; }
        internal static bool CodeExecution { get => kCodeExecution.Value; set => kCodeExecution.Value = value; }
        internal static bool FineTuning { get => kFineTuning.Value; set => kFineTuning.Value = value; }
        internal static bool Streaming { get => kStreaming.Value; set => kStreaming.Value = value; }
        internal static bool ImageGeneration { get => kImageGeneration.Value; set => kImageGeneration.Value = value; }
        internal static bool ImageInpainting { get => kImageInpainting.Value; set => kImageInpainting.Value = value; }
        internal static bool SpeechGeneration { get => kSpeechGeneration.Value; set => kSpeechGeneration.Value = value; }
        internal static bool SpeechRecognition { get => kSpeechRecognition.Value; set => kSpeechRecognition.Value = value; }
        internal static bool SoundFXGeneration { get => kSoundFXGeneration.Value; set => kSoundFXGeneration.Value = value; }
        internal static bool VoiceChanger { get => kVoiceChanger.Value; set => kVoiceChanger.Value = value; }
        internal static bool VideoGeneration { get => kVideoGeneration.Value; set => kVideoGeneration.Value = value; }
        internal static bool TextEmbedding { get => kTextEmbedding.Value; set => kTextEmbedding.Value = value; }
        internal static bool Moderation { get => kModeration.Value; set => kModeration.Value = value; }
        internal static bool Search { get => kSearch.Value; set => kSearch.Value = value; }
        internal static bool Realtime { get => kRealtime.Value; set => kRealtime.Value = value; }
        internal static bool ComputerUse { get => kComputerUse.Value; set => kComputerUse.Value = value; }

    }
}
