using UnityEngine;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// Extension methods that let you start AI generation tasks in a fluent, chainable way.
    /// Example: 
    ///     "Describe a cat.".GENText().SetModel(OpenAIModel.GPT4o).ExecuteAsync();
    ///     audioClip.GENTranscription().ExecuteAsync();
    /// </summary>
    public static class GENTaskFluentExtensions
    {
        /// <summary>
        /// Creates a text generation task (like ChatGPT) using this string as the prompt.
        /// 
        /// Example:
        ///     "Tell me a joke.".GENText().SetModel(OpenAIModel.GPT4o).ExecuteAsync();
        /// </summary>
        public static GENTextTask GENText(this string promptText) => new(promptText);

        /// <summary>
        /// Creates a text generation task using this chat session and message as the prompt.
        /// 
        /// Example:
        ///     chatSession.GENChat(chatMessage).SetModel(OpenAIModel.GPT4o).ExecuteAsync();
        /// </summary> 
        public static GENChatTask GENChat(this ChatSession chatSession, ChatMessage chatMessage) => new(chatSession, chatMessage);

        /// <summary>
        /// Creates an image generation task using this string as the image prompt.
        /// 
        /// Example:
        ///     "A cat surfing a wave".GENImage().SetModel(ImageModel.DallE3).ExecuteAsync();
        /// </summary>
        public static GENImageCreationTask GENImage(this string promptText) => new(promptText);

        /// <summary>
        /// Creates an image editing task by applying the given prompt to this image.
        /// 
        /// Note:
        ///     - DALL·E 3 image editing is not supported with the OpenAI API yet. (Only available in ChatGPT)
        /// 
        /// Example:
        ///     texture.GENImageEdit("Add sunglasses").SetModel(ImageModel.DallE2).ExecuteAsync();
        /// </summary>
        public static GENImageEditTask GENImageEdit(this Texture2D promptImage, string promptText) => new(promptText, promptImage);

        /// <summary>
        /// Creates a task to generate variations (remixes) of the given image.
        /// 
        /// Note:
        ///    - DALL·E 3 image variation is not supported with the OpenAI API yet. (Only available in ChatGPT)
        /// 
        /// Example:
        ///     texture.GENImageVariation().SetModel(ImageModel.DallE2).ExecuteAsync();
        /// </summary>
        public static GENImageVariationTask GENImageVariation(this Texture2D promptImage) => new(promptImage);

        /// <summary>
        /// Creates a text-to-speech (TTS) task that reads this string aloud using a realistic AI voice.
        /// 
        /// Example:
        ///     "Hello there!".GENSpeech().SetVoice(ElevenLabsVoice.Rachel).ExecuteAsync();
        /// </summary>
        public static GENSpeechTask GENSpeech(this string promptText) => new(promptText);

        /// <summary>
        /// Creates a speech-to-text (STT) task that transcribes spoken words from this audio clip.
        /// 
        /// Example:
        ///     audioClip.GENTranscript().ExecuteAsync();
        /// </summary>
        public static GENTranscriptTask GENTranscript(this AudioClip promptAudio) => new(promptAudio);

        /// <summary>
        /// Translates the speech in this audio clip into English text.
        /// Useful for multilingual voice input.
        /// 
        /// Example:
        ///     audioClip.GENTranslation().ExecuteAsync();
        /// </summary>
        public static GENTranslationTask GENTranslation(this AudioClip promptAudio) => new(promptAudio);

        /// <summary>
        /// Generates structured JSON output by interpreting the text as instructions for a specific object type.
        /// Type T should be decorated with OpenAIJsonSchemaResponseAttribute or JsonSchemaAttribute.
        /// 
        /// Example:
        ///     "Create a product listing".GENObject<Product>().ExecuteAsync();
        /// </summary>
        public static GENObjectTask<T> GENObject<T>(this string promptText) => new(promptText);

        /// <summary>
        /// Generates a sound effect based on this prompt text.
        /// 
        /// Example:
        ///     "Footsteps on snow".GENSoundEffect().ExecuteAsync();
        /// </summary>
        public static GENSoundEffectTask GENSoundEffect(this string promptText) => new(promptText);

        /// <summary>
        /// Changes the voice in this audio clip using AI voice transformation.
        /// 
        /// Example:
        ///     audioClip.GENVoiceChange().SetVoice(ElevenLabsVoice.Rachel).ExecuteAsync();
        /// </summary>
        public static GENVoiceChangeTask GENVoiceChange(this AudioClip promptAudio) => new(promptAudio);

        /// <summary>
        /// Isolates vocals or removes background noise from this audio clip.
        /// 
        /// Example:
        ///     audioClip.GENAudioIsolation().ExecuteAsync();
        /// </summary>
        public static GENAudioIsolationTask GENAudioIsolation(this AudioClip promptAudio) => new(promptAudio);

        /// <summary>
        /// Generates a video based on this prompt text.
        /// 
        /// Example:
        ///    "A cat surfing a wave".GENVideoGen().ExecuteAsync();
        /// </summary>
        public static GENVideoTask GENVideo(this string promptText) => new(promptText);

        /// <summary>
        /// Generates a video based on this prompt image.
        /// 
        /// Example:
        ///   texture.GENVideoGen().ExecuteAsync();
        /// </summary> 
        public static GENVideoTask GENVideo(this Texture2D promptImage) => new(promptImage);
    }
}
