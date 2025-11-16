using Glitch9.EditorKit;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal static class AIDevKitIcons
    {
        private const string kIconPath = "{0}/AIDevKit/Editor/Common/Gizmos/Icons";
        private static readonly EditorTextureCache<Texture2D> _iconCache = new(string.Format(kIconPath, EditorPathUtil.FindGlitch9Path()));

        internal static Texture2D You => _iconCache.Get("ic_editor_gpt_you");
        internal static Texture2D EditorChat => _iconCache.Get("ic_editor_gpt_assistant");

        // Model Type Icons
        internal static Texture2D Chat => _iconCache.Get("ic_editor_chat");
        internal static Texture2D SpeechToText => _iconCache.Get("ic_editor_speech_to_text");
        internal static Texture2D TextToSpeech => _iconCache.Get("ic_editor_text_to_speech");
        internal static Texture2D Moderation => _iconCache.Get("ic_editor_moderation");
        internal static Texture2D Tool => _iconCache.Get("ic_editor_tool");
        internal static Texture2D Gemini => _iconCache.Get("ic_gemini");
        internal static Texture2D Realtime => _iconCache.Get("ic_editor_realtime");
        internal static Texture2D Send => _iconCache.Get("ic_send"); // for EditorChatWindow

        // Provider Icons
        internal static Texture2D OpenAI => _iconCache.Get("ic_openai");
        internal static Texture2D Google => _iconCache.Get("ic_google");
        internal static Texture2D ElevenLabs => _iconCache.Get("ic_eleven_labs");
        internal static Texture2D Mubert => _iconCache.Get("ic_mubert");
        internal static Texture2D Ollama => _iconCache.Get("ic_ollama");
        internal static Texture2D OpenRouter => _iconCache.Get("ic_open_router");

        // IO Icons
        internal static Texture2D Text => _iconCache.Get("ic_io_text");
        internal static Texture2D Image => _iconCache.Get("ic_io_images");
        internal static Texture2D Audio => _iconCache.Get("ic_io_audio");
        internal static Texture2D Embedding => _iconCache.Get("ic_io_embeddings");
        internal static Texture2D Video => _iconCache.Get("ic_io_videos");
        internal static Texture2D JsonSchema => _iconCache.Get("ic_json_schema");
        internal static Texture2D File => _iconCache.Get("ic_io_file");
        internal static Texture2D Code => _iconCache.Get("ic_io_code");
        internal static Texture2D FineTuning => _iconCache.Get("ic_fine_tuning");
        internal static Texture2D Streaming => _iconCache.Get("ic_streaming");
        internal static Texture2D SoundFX => _iconCache.Get("ic_io_sound_fx");
        internal static Texture2D Inpainting => _iconCache.Get("ic_editor_ai_image");
        internal static Texture2D Caching => _iconCache.Get("ic_caching");

        internal static Texture2D ModelLibrary => _iconCache.Get("ic_model_library");
    }
}