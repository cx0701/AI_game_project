using System.Collections.Generic;

namespace Glitch9.AIDevKit
{
    public static class GENTaskType
    {
        public const int Unknown = -1;

        // Text
        public const int Completion = 0;
        public const int ChatCompletion = 1;
        public const int JsonSchema = 2; // structured outputs

        // Image
        public const int ImageCreation = 10;
        public const int ImageEdit = 11;
        public const int ImageVariation = 12;

        // Audio
        public const int Speech = 20;
        public const int Transcript = 21;
        public const int Translation = 22;
        public const int SoundEffect = 23;
        public const int VoiceChange = 24;
        public const int AudioIsolation = 25;

        // Video
        public const int VideoGeneration = 30;

        // List
        public const int ListModels = 100;
        public const int ListVoices = 101;
        public const int ListCustomModels = 102;
        public const int ListCustomVoices = 103;

        private readonly static Dictionary<int, string> _names = new()
        {
            { GENTaskType.Unknown, "Unknown" },
            { GENTaskType.Completion, "Completion" },
            { GENTaskType.ChatCompletion, "Chat Completion" },
            { GENTaskType.ImageCreation, "Image Generation" },
            { GENTaskType.ImageEdit, "Image Edit" },
            { GENTaskType.ImageVariation, "Image Variation" },
            { GENTaskType.Speech, "Text to Speech" },
            { GENTaskType.Transcript, "Speech to Text" },
            { GENTaskType.Translation, "Speech to Text (Translation)" },
            { GENTaskType.JsonSchema, "Custom Object (JSONSchema)" },
            { GENTaskType.SoundEffect, "Sound Effect" },
            { GENTaskType.VoiceChange, "Voice Change" },
            { GENTaskType.AudioIsolation, "Audio Isolation" },
            { GENTaskType.VideoGeneration, "Video Generation" },
        };

        internal static string GetName(int taskType)
        {
            if (_names.TryGetValue(taskType, out var name)) return name;
            return "Unknown";
        }

        internal static bool HasTextInput(int taskType)
        {
            return taskType == GENTaskType.Completion
            || taskType == GENTaskType.ChatCompletion
            || taskType == GENTaskType.ImageCreation
            || taskType == GENTaskType.ImageEdit
            || taskType == GENTaskType.Speech
            || taskType == GENTaskType.JsonSchema
            || taskType == GENTaskType.SoundEffect
            || taskType == GENTaskType.VideoGeneration;
        }

        internal static bool HasTextOutput(int taskType)
        {
            return taskType == GENTaskType.Completion
            || taskType == GENTaskType.ChatCompletion
            || taskType == GENTaskType.Transcript
            || taskType == GENTaskType.Translation
            || taskType == GENTaskType.JsonSchema;
        }

        internal static bool HasAudioInput(int taskType)
        {
            return taskType == GENTaskType.Transcript
            || taskType == GENTaskType.Translation
            || taskType == GENTaskType.VoiceChange
            || taskType == GENTaskType.AudioIsolation;
        }

        internal static bool HasAudioOutput(int taskType)
        {
            return taskType == GENTaskType.Speech
            || taskType == GENTaskType.SoundEffect
            || taskType == GENTaskType.VoiceChange
            || taskType == GENTaskType.AudioIsolation;
        }

        internal static bool HasImageInput(int taskType)
        {
            return taskType == GENTaskType.ImageEdit
            || taskType == GENTaskType.ImageVariation;
        }

        internal static bool HasImageOutput(int taskType)
        {
            return taskType == GENTaskType.ImageCreation
            || taskType == GENTaskType.ImageEdit
            || taskType == GENTaskType.ImageVariation;
        }
    }
}