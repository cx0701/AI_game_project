using System;
using System.Collections.Generic;
using Glitch9.EditorKit;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal static class AIDevKitGUIUtility
    {
        private static Dictionary<ModelCapability, GUIContent> _capContents;

        private static readonly ModelCapability[] _priorityOrder = new[]
        {
            ModelCapability.TextGeneration,
            ModelCapability.Realtime,
            ModelCapability.ImageGeneration,
            ModelCapability.SpeechRecognition,
            ModelCapability.SpeechGeneration,
            ModelCapability.Moderation,
            ModelCapability.FunctionCalling,
            ModelCapability.Streaming,
            ModelCapability.Caching,
            ModelCapability.ImageInpainting,
            ModelCapability.SoundFXGeneration,
            ModelCapability.VideoGeneration,
            ModelCapability.TextEmbedding,
            ModelCapability.Search,
            ModelCapability.ComputerUse,
            ModelCapability.StructuredOutputs,
            ModelCapability.FineTuning,
        };

        internal static readonly CurrencyCode[] SelectedCurrencyCodes = new[]
        {
            CurrencyCode.USD,
            CurrencyCode.EUR,
            CurrencyCode.GBP,
            CurrencyCode.CAD,
            CurrencyCode.AUD,
            CurrencyCode.JPY,
            CurrencyCode.CNY,
            CurrencyCode.INR,
            CurrencyCode.RUB,
            CurrencyCode.BRL,
            CurrencyCode.KRW,
        };

        private static Dictionary<ModelCapability, GUIContent> CreateCapabilityContents()
        {
            Dictionary<ModelCapability, GUIContent> contents = new();
            var array = Enum.GetValues(typeof(ModelCapability));
            // Sort the array based on the priority order
            List<ModelCapability> sortedArray = new(array.Length);

            foreach (ModelCapability capability in array)
            {
                if (capability == ModelCapability.None) continue;
                sortedArray.Add(capability);
            }

            sortedArray.Sort((x, y) =>
            {
                int indexX = Array.IndexOf(_priorityOrder, x);
                int indexY = Array.IndexOf(_priorityOrder, y);
                return indexX.CompareTo(indexY);
            });

            foreach (ModelCapability cap in sortedArray)
            {
                if (cap == ModelCapability.None) continue;

                GUIContent content = new(GetCapabilityIcon(cap), cap.GetName());
                contents.Add(cap, content);
            }

            return contents;
        }

        internal static List<GUIContent> GetCapabilityContents(ModelCapability cap)
        {
            List<GUIContent> contents = new();

            if (cap == ModelCapability.None)
            {
                contents.Add(new GUIContent("None"));
                return contents;
            }

            _capContents ??= CreateCapabilityContents();

            long capValue = (long)cap;
            foreach (var kvp in _capContents)
            {
                long bit = (long)kvp.Key;
                if ((capValue & bit) == bit)
                {
                    contents.Add(kvp.Value);
                }
            }

            return contents;
        }

        internal static Texture GetCapabilityIcon(ModelCapability cap)
        {
            return cap switch
            {
                ModelCapability.TextGeneration => AIDevKitIcons.Text,
                ModelCapability.StructuredOutputs => AIDevKitIcons.JsonSchema,
                ModelCapability.CodeExecution => AIDevKitIcons.Code,
                ModelCapability.FunctionCalling => AIDevKitIcons.Tool,
                ModelCapability.Caching => AIDevKitIcons.Caching,
                ModelCapability.ImageGeneration => AIDevKitIcons.Image,
                ModelCapability.ImageInpainting => AIDevKitIcons.Inpainting,
                ModelCapability.SpeechGeneration => AIDevKitIcons.TextToSpeech,
                ModelCapability.SpeechRecognition => AIDevKitIcons.SpeechToText,
                ModelCapability.SoundFXGeneration => AIDevKitIcons.SoundFX,
                ModelCapability.VideoGeneration => AIDevKitIcons.Video,
                ModelCapability.TextEmbedding => AIDevKitIcons.Embedding,
                ModelCapability.Moderation => AIDevKitIcons.Moderation,
                ModelCapability.Search => EditorIcons.Search,
                ModelCapability.Realtime => AIDevKitIcons.Realtime,
                ModelCapability.FineTuning => AIDevKitIcons.FineTuning,
                ModelCapability.Streaming => AIDevKitIcons.Streaming,
                ModelCapability.ComputerUse => AIDevKitIcons.Code,

                _ => EditorIcons.Question,
            };
        }

        internal static Texture2D GetProviderIcon(AIProvider provider)
        {
            Texture2D texture = provider switch
            {
                AIProvider.OpenAI => AIDevKitIcons.OpenAI,
                AIProvider.Google => AIDevKitIcons.Google,
                AIProvider.ElevenLabs => AIDevKitIcons.ElevenLabs,
                AIProvider.Mubert => AIDevKitIcons.Mubert,
                AIProvider.Ollama => AIDevKitIcons.Ollama,
                AIProvider.OpenRouter => AIDevKitIcons.OpenRouter,
                _ => AIDevKitIcons.OpenAI
            };

            return texture;
        }
    }
}