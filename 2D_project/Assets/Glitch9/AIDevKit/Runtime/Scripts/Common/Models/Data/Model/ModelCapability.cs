using System;
using System.Collections.Generic;

namespace Glitch9.AIDevKit
{
    [Flags]
    public enum ModelCapability
    {
        None = 0,

        // Core Language Capabilities
        TextGeneration = 1 << 0,
        FineTuning = 1 << 1,
        Streaming = 1 << 2,
        StructuredOutputs = 1 << 3,
        CodeExecution = 1 << 4,
        FunctionCalling = 1 << 5,
        Caching = 1 << 6,

        // Media Capabilities
        ImageGeneration = 1 << 7,
        ImageInpainting = 1 << 8,
        SpeechGeneration = 1 << 9,
        SpeechRecognition = 1 << 10,
        SoundFXGeneration = 1 << 11,
        VoiceChanger = 1 << 12,
        VideoGeneration = 1 << 13,

        // Other
        TextEmbedding = 1 << 14,
        Moderation = 1 << 15,
        Search = 1 << 16,
        Realtime = 1 << 17,
        ComputerUse = 1 << 18,

        // Added
        VoiceIsolation = 1 << 19,
    }

    public static class ModelCapabilityExtensions
    {
        private static readonly Dictionary<ModelCapability, string> _names = new()
        {
            { ModelCapability.TextGeneration, "Text Generation" },
            { ModelCapability.ImageGeneration, "Image Generation" },
            { ModelCapability.SpeechRecognition, "Speech Recognition" },
            { ModelCapability.SpeechGeneration, "Speech Generation" },
            { ModelCapability.Moderation, "Moderation" },
            { ModelCapability.FunctionCalling, "Function Calling" },
            { ModelCapability.Realtime, "Real-time" },
            { ModelCapability.FineTuning, "Fine Tuning" },
            { ModelCapability.Streaming, "Streaming" },
            { ModelCapability.Caching, "Caching" },
            { ModelCapability.ImageInpainting, "Image Inpainting" },
            { ModelCapability.SoundFXGeneration, "Sound FX Generation" },
            { ModelCapability.VideoGeneration, "Video Generation" },
            { ModelCapability.TextEmbedding, "Text Embedding" },
            { ModelCapability.Search, "Search" },
            { ModelCapability.ComputerUse, "Computer Use" },
            { ModelCapability.StructuredOutputs, "Structured Outputs" },
            { ModelCapability.CodeExecution, "Code Execution" }
        };

        public static string GetName(this ModelCapability capability)
        {
            if (_names.TryGetValue(capability, out string name)) return name;
            return capability.ToString(); // Fallback to the enum name if not found
        }
    }
}