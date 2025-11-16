using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Glitch9.AIDevKit.Components;
using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.Files;
using Newtonsoft.Json;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    public interface IGENTask
    {
#pragma warning disable IDE1006
        bool enableHistory { get; }
#pragma warning restore IDE1006
    }


    public interface IGENMediaTask
    {
        // set option, and try get option
        void SetOption(string key, object value);
        bool TryGetOption<T>(string key, out T value);
    }

    /// <summary>
    /// Abstract base class for all generative AI tasks (text, image, audio).
    /// Supports text, image, and audio prompts with fluent configuration methods.
    /// </summary>
    public abstract class GENTask<TSelf, TResult> : IGENTask
        where TSelf : GENTask<TSelf, TResult>
    {
        // public static implicit operator UniTask<TResult>(GENTask<TSelf, TResult> task) => task.ExecuteAsync();
        // public UniTask<TResult>.Awaiter GetAwaiter() => ExecuteAsync().GetAwaiter();

        // Task State Management --------------------------------------------------------------------------------------------------  
        private readonly CancellationTokenSource _cts = new();

#pragma warning disable IDE1006
        public CancellationToken token => _cts.Token;
        public bool enableHistory => _enableHistory ?? AIDevKitSettings.PromptHistoryOnRuntime;
        public bool isCanceled => _cts.IsCancellationRequested;
        public virtual MIMEType outputMimeType { get; set; } = MIMEType.Json; // default to JSON

#pragma warning restore IDE1006
        public void Cancel() => _cts.Cancel();

        // HTTP Properties -------------------------------------------------------------------------------------------------------
        internal string sender;
        internal string outputPath;
        internal bool saveOutput = false;
        internal bool ignoreLogs = false;

        // AI API Properties ----------------------------------------------------------------------------------------------------- 
        internal Model model;
        internal int outputCount = 1;
        private bool? _enableHistory;

        // For Custom Options that exist in other assemblies (e.g., OpenAI, Gemini) -----------------------------------------------
        internal Dictionary<string, object> options;

        public void SetOption(string key, object value)
        {
            options ??= new();
            options[key] = value;
        }

        public bool TryGetOption<T>(string key, out T value)
        {
            if (options != null && options.TryGetValue(key, out var obj) && obj is T t)
            {
                value = t;
                return true;
            }

            value = default;
            return false;
        }

        // Fluent API Methods --------------------------------------------------------------------------------------------------

        /// <summary>
        /// Sets the AI model to be used for generation.
        /// </summary>
        public TSelf SetModel(Model model)
        {
            this.model = model;
            return this as TSelf;
        }

        /// <summary>
        /// Sets the number of outputs to generate (e.g., number of images or responses).
        /// </summary>
        public TSelf SetCount(int count)
        {
            this.outputCount = count;
            return this as TSelf;
        }

        /// <summary>
        /// Sets the download path for the generated content(s).
        /// This is the path where the generated content(s) will be saved.
        /// </summary>
        /// <param name="outputPath">The path where the generated content(s) will be saved.</param>
        public TSelf SetOutputPath(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath)) throw new ArgumentNullException(nameof(outputPath), "Output path is null or empty.");
            this.outputPath = outputPath;
            saveOutput = true; // set saveOutput to true if outputPath is set.
            return this as TSelf;
        }

        /// <summary>
        /// Sets the sender ID for tracking or attribution purposes.
        /// </summary>
        public TSelf SetSender(string sender)
        {
            this.sender = sender;
            return this as TSelf;
        }

        public TSelf SetIgnoreLogs(bool ignoreLogs)
        {
            this.ignoreLogs = ignoreLogs;
            return this as TSelf;
        }

        public TSelf EnablePromptHistory(bool enableHistory = true)
        {
            _enableHistory = enableHistory;
            return this as TSelf;
        }

        // Internal Methods --------------------------------------------------------------------------------------------------
        internal virtual TSelf ResolveOutputPath(AIProvider? api = null, string keyword = null)
        {
            api ??= GENTaskUtil.ResolveApi(model);
            keyword ??= GENTaskUtil.ResolveKeyword(model);

            if (GENTaskUtil.TryResolveOutputPath(saveOutput, outputPath, outputMimeType, api.Value, keyword, out string path))
            {
                outputPath = path;
            }
            else
            {
                throw new InvalidOperationException($"Failed to resolve output path for {api}-{keyword}.");
            }

            return this as TSelf;
        }

        // Execution Method --------------------------------------------------------------------------------------------------

        /// <summary>
        /// Executes the task and returns the generated result.
        /// </summary>
        public abstract UniTask<TResult> ExecuteAsync();
    }

    public abstract class GENContentTaskBase<TSelf, TResult> : GENTask<TSelf, TResult>
        where TSelf : GENContentTaskBase<TSelf, TResult>
    {
        internal readonly string promptText;
        internal readonly List<IUniFile> attachedFiles = new();
        public GENContentTaskBase(string promptText) => this.promptText = promptText;

        internal string instruction;
        internal ModelOptions modelOptions;
        internal ReasoningOptions reasoningOptions;
        internal WebSearchOptions webSearchOptions;
        internal SpeechOutputOptions speechOutputOptions;


        // Fluent API Methods --------------------------------------------------------------------------------------------------

        public TSelf AddAttachment(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException(nameof(filePath), "File path is null or empty.");
            if (!File.Exists(filePath)) throw new FileNotFoundException("File not found.", filePath);
            attachedFiles.Add(new UniFile(filePath));
            return this as TSelf;
        }

        public TSelf AddAttachment(Texture2D texture)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture), "Texture is null.");
            attachedFiles.Add(new UniImageFile(texture));
            return this as TSelf;
        }

        public TSelf AddAttachment(AudioClip audioClip)
        {
            if (audioClip == null) throw new ArgumentNullException(nameof(audioClip), "AudioClip is null.");
            attachedFiles.Add(new UniAudioFile(audioClip));
            return this as TSelf;
        }

        public TSelf AddAttachment(IUniFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file), "File is null.");
            attachedFiles.Add(file);
            return this as TSelf;
        }

        /// <summary>
        /// Sets the instruction for the task. This is a specific command or request for the model to follow.
        /// </summary>
        /// <param name="instruction"></param>
        /// <returns></returns>
        public TSelf SetInstruction(string instruction)
        {
            this.instruction = instruction;
            return this as TSelf;
        }

        /// <summary>
        /// Sets the generation options for the task.
        /// </summary>
        public TSelf SetModelOptions(ModelOptions options)
        {
            modelOptions = options ?? throw new ArgumentNullException(nameof(options), "CompletionOptions is null.");
            return this as TSelf;
        }

        public TSelf SetReasoningEffort(ReasoningEffort effort)
        {
            reasoningOptions ??= new ReasoningOptions();
            reasoningOptions.Effort = effort;
            return this as TSelf;
        }

        public TSelf SetSpeechOutput(SpeechOutputOptions options = null)
        {
            speechOutputOptions = options ?? new SpeechOutputOptions();
            return this as TSelf;
        }

        public TSelf SetWebSearchOptions(WebSearchOptions webSearchOptions)
        {
            this.webSearchOptions = webSearchOptions ?? throw new ArgumentNullException(nameof(webSearchOptions), "WebSearchOptions is null.");
            return this as TSelf;
        }
    }

    /// <summary>
    /// Task for generating text using an LLM model. Supports instructions and role-based prompts.
    /// </summary>
    public class GENTextTask : GENContentTaskBase<GENTextTask, GeneratedText>
    {
        public GENTextTask(string promptText) : base(promptText) { }
        private ChatStreamHandler streamHandler;

        // Fluent API Methods --------------------------------------------------------------------------------------------------
        public GENTextTask OnStreamText(Action<string> onTextReceived)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onStream += onTextReceived;
            return this;
        }

        public GENTextTask OnStreamError(Action<string> onError)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onError += onError;
            return this;
        }

        public GENTextTask OnStreamComplete(Action<GeneratedText> onComplete)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onDone += onComplete;
            return this;
        }

        public GENTextTask OnStreamToolCall(Action<ToolCall[]> onToolCall)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onToolCall += onToolCall;
            return this;
        }

        // Execution Method --------------------------------------------------------------------------------------------------

        /// <summary>
        /// Executes the text generation and returns the full response as a string.
        /// </summary>
        public override UniTask<GeneratedText> ExecuteAsync() => GENTaskManager.GenerateTextAsync(this, null);

        /// <summary>
        /// Streams text generation output in real time as it's received from the model.
        /// </summary> 
        public UniTask StreamAsync(ChatStreamHandler streamHandler = null) => GENTaskManager.StreamTextAsync(this, null, this.streamHandler?.SetGENTask(this) ?? streamHandler.SetGENTask(this));
    }

    /// <summary>
    /// [BETA]
    /// Task for generating structured output (e.g., JSON) using an LLM model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GENObjectTask<T> : GENContentTaskBase<GENObjectTask<T>, T>
    {
        public GENObjectTask(string promptText) : base(promptText) { }
        // Execution Method --------------------------------------------------------------------------------------------------
        public override async UniTask<T> ExecuteAsync()
        {
            GENTextTask textGenTask = new(promptText)
            {
                model = model,
                instruction = instruction,
                outputCount = outputCount,
                outputPath = outputPath,
                sender = sender
            };

            string result = await GENTaskManager.GenerateTextAsync(textGenTask, typeof(T));
            return JsonConvert.DeserializeObject<T>(result);
        }

        public UniTask StreamAsync(ChatStreamHandler streamHandler)
        {
            GENTextTask textGenTask = new(promptText)
            {
                model = model,
                instruction = instruction,
                outputCount = outputCount,
                outputPath = outputPath,
                sender = sender
            };

            return GENTaskManager.StreamTextAsync(textGenTask, typeof(T), streamHandler);
        }
    }

    public abstract class GENMediaTask<TSelf, TResult> : GENTask<TSelf, TResult>, IGENMediaTask
        where TSelf : GENMediaTask<TSelf, TResult>
    {
        public override MIMEType outputMimeType { get; set; } = MIMEType.PNG;
        internal readonly string promptText;
        internal readonly Texture2D promptImage;
        public GENMediaTask(string promptText) => this.promptText = promptText;
        public GENMediaTask(Texture2D promptImage) => this.promptImage = promptImage;
        public GENMediaTask(string promptText, Texture2D promptImage) : this(promptText) => this.promptImage = promptImage;
        public GENMediaTask(Texture2D promptImage, string promptText) : this(promptText) => this.promptImage = promptImage;
    }

    /// <summary>
    /// Task for generating image(s) from text using supported models (e.g., OpenAI DALL·E, Google Imagen).
    /// </summary>
    public class GENImageCreationTask : GENMediaTask<GENImageCreationTask, GeneratedImage>
    {
        public GENImageCreationTask(string promptText) : base(promptText) { }
        public override UniTask<GeneratedImage> ExecuteAsync() => GENTaskManager.GenerateImageAsync(this);

        /// <summary>
        /// Allows to set N more than the selected model allows.
        /// This is useful for streaming the images one by one.
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<GeneratedImage> YieldAsync()
        {
            int maxOutput = model.MaxImageOutput;
            if (maxOutput <= 0 || outputCount <= 0)
                yield break;

            int remaining = outputCount;
            while (remaining > 0)
            {
                int batch = Mathf.Min(remaining, maxOutput);

                var taskCopy = MemberwiseClone() as GENImageCreationTask;
                taskCopy.outputCount = batch;

                var results = await GENTaskManager.GenerateImageAsync(taskCopy);
                yield return results;

                remaining -= batch;
            }
        }
    }

    /// <summary>
    /// Task for editing an existing image based on a text prompt and optional mask (OpenAI or Google Gemini).
    /// </summary>
    public class GENImageEditTask : GENMediaTask<GENImageEditTask, GeneratedImage>
    {
        public GENImageEditTask(string promptText, Texture2D promptImage) : base(promptText, promptImage) { }
        public override UniTask<GeneratedImage> ExecuteAsync() => GENTaskManager.GenerateImageEditAsync(this);
    }

    /// <summary>
    /// Task for generating visual variations of an input image (OpenAI or Google Gemini).
    /// </summary>
    public class GENImageVariationTask : GENMediaTask<GENImageVariationTask, GeneratedImage>
    {
        public GENImageVariationTask(Texture2D promptImage, string promptText = null) : base(promptText, promptImage) { }
        public override UniTask<GeneratedImage> ExecuteAsync() => GENTaskManager.GenerateImageVariationAsync(this);
    }



    public abstract class GENAudioTaskBase<TSelf> : GENTask<TSelf, GeneratedAudio>, IGENMediaTask
       where TSelf : GENAudioTaskBase<TSelf>
    {
        public override MIMEType outputMimeType { get; set; } = MIMEType.MPEG; // default to MP3 

        public GENSpeechTask SetAudioEncoding(AudioEncoding encoding)
        {
            outputMimeType = encoding.ToMIMEType();
            return this as GENSpeechTask;
        }
    }

    /// <summary>
    /// Task for generating synthetic speech (text-to-speech) using the specified model.
    /// </summary>
    public class GENSpeechTask : GENAudioTaskBase<GENSpeechTask>
    {
        internal readonly string promptText;
        public GENSpeechTask(string promptText) => this.promptText = promptText;

        internal Voice voice;
        internal float? speed;
        internal uint? seed; // not available on OpenAI


        // Fluent API Methods -------------------------------------------------------------------------------------------------- 
        /// <summary>
        /// Sets the voice preset name (string-based, for future compatibility).
        /// </summary>
        public GENSpeechTask SetVoice(Voice voice)
        {
            this.voice = voice;
            return this;
        }

        /// <summary>
        /// Sets the playback speed of the synthesized voice.
        /// </summary>
        public GENSpeechTask SetSpeed(float speed)
        {
            this.speed = speed;
            return this;
        }

        /// <summary>
        /// Sets a fixed seed to make generation deterministic and repeatable.
        /// </summary>
        public GENSpeechTask SetSeed(uint seed)
        {
            this.seed = seed;
            return this;
        }


        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedAudio> ExecuteAsync() => GENTaskManager.GenerateSpeechAsync(this);
        public UniTask StreamAsync(StreamAudioPlayer streamAudioPlayer) => GENTaskManager.StreamSpeechAsync(this, streamAudioPlayer);
    }

    /// <summary>
    /// Task for converting speech audio into text (speech-to-text).
    /// </summary>
    public class GENTranscriptTask : GENTask<GENTranscriptTask, GeneratedText>
    {
        internal AudioClip promptAudio; // input audio for chat, speech to text  

        // --- OpenAI only ---
        internal SystemLanguage? language;
        public GENTranscriptTask(AudioClip promptAudio) => this.promptAudio = promptAudio; // input audio for chat, speech to text

        // Fluent API Methods -------------------------------------------------------------------------------------------------- 

        /// <summary>
        /// Optionally sets the language hint to improve transcription accuracy.
        /// </summary>
        public GENTranscriptTask SetLanguage(SystemLanguage language)
        {
            this.language = language;
            return this;
        }

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedText> ExecuteAsync() => GENTaskManager.GenerateTranscriptionAsync(this);
    }

    /// <summary>
    /// Task for translating speech into English text using the speech translation model.
    /// </summary>
    public class GENTranslationTask : GENTask<GENTranslationTask, GeneratedText>
    {
        internal AudioClip promptAudio;
        public GENTranslationTask(AudioClip promptAudio) => this.promptAudio = promptAudio; // input audio for chat, speech to text

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedText> ExecuteAsync() => GENTaskManager.GenerateTranslationAsync(this);
    }

    /// <summary>
    /// Task for generating sound effects based on a text prompt.
    /// </summary>
    public class GENSoundEffectTask : GENAudioTaskBase<GENSoundEffectTask>
    {
        internal readonly string promptText;

        // --- ElevenLabs only ---        
        internal double? durationSeconds;
        internal double? promptInfluence;
        public GENSoundEffectTask(string promptText) => this.promptText = promptText;

        // Fluent API Methods --------------------------------------------------------------------------------------------------

        /// <summary>
        /// Sets the duration of the generated sound effect in seconds.
        /// </summary>
        public GENSoundEffectTask SetDuration(double durationSeconds)
        {
            this.durationSeconds = durationSeconds;
            return this;
        }

        /// <summary>
        /// Sets the influence of the prompt on the generated sound effect.
        /// </summary>
        /// <param name="promptInfluence">Value between 0 and 1.</param> 
        public GENSoundEffectTask SetPromptInfluence(double promptInfluence)
        {
            this.promptInfluence = promptInfluence;
            return this;
        }

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedAudio> ExecuteAsync() => GENTaskManager.GenerateSoundEffectAsync(this);
    }

    public class GENVoiceChangeTask : GENAudioTaskBase<GENVoiceChangeTask>
    {
        internal readonly AudioClip promptAudio; // input audio for chat, speech to text  

        // --- ElevenLabs only ---
        internal Voice voice;
        internal uint? seed;
        internal bool? removeBackgroundNoise;
        public GENVoiceChangeTask(AudioClip promptAudio) => this.promptAudio = promptAudio;

        // Fluent API Methods --------------------------------------------------------------------------------------------------

        public GENVoiceChangeTask SetVoice(Voice voice)
        {
            this.voice = voice;
            return this;
        }

        /// <summary>
        /// Sets a fixed seed to make generation deterministic and repeatable.
        /// </summary>
        public GENVoiceChangeTask SetSeed(uint seed)
        {
            this.seed = seed;
            return this;
        }

        /// <summary>
        /// Sets whether to remove background noise from the input audio.
        /// </summary>
        public GENVoiceChangeTask SetRemoveBackgroundNoise(bool removeBackgroundNoise)
        {
            this.removeBackgroundNoise = removeBackgroundNoise;
            return this;
        }

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedAudio> ExecuteAsync() => GENTaskManager.GenerateVoiceChangeAsync(this);
    }

    public class GENAudioIsolationTask : GENAudioTaskBase<GENAudioIsolationTask>
    {
        internal readonly AudioClip promptAudio; // input audio for chat, speech to text  
        public GENAudioIsolationTask(AudioClip promptAudio) => this.promptAudio = promptAudio;

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedAudio> ExecuteAsync() => GENTaskManager.GenerateAudioIsolationAsync(this);
    }

    /// <summary>
    /// Task for generating text using an LLM model. Supports instructions and role-based prompts.
    /// </summary>
    public class GENChatTask : GENTask<GENChatTask, GeneratedContent>
    {
        public GENChatTask(ChatSession chatSession, ChatMessage chatMessage)
        {
            chatSession.PushMessage(chatMessage);
            session = chatSession;
            message = chatMessage;
        }

        internal readonly ChatSession session;
        internal readonly ChatMessage message;
        internal FunctionDeclaration[] functions;
        private ChatStreamHandler streamHandler;

        // Fluent API Methods --------------------------------------------------------------------------------------------------
        public GENChatTask SetFunctionManager(FunctionManager functionManager)
        {
            if (functionManager == null) throw new ArgumentNullException(nameof(functionManager), "FunctionManager is null.");
            functions = functionManager.GetFunctions();
            return this;
        }

        public GENChatTask SetFunctions(FunctionDeclaration[] functions)
        {
            this.functions = functions ?? throw new ArgumentNullException(nameof(functions), "Functions are null.");
            return this;
        }

        public GENChatTask OnStreamText(Action<string> onTextReceived)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onStream += onTextReceived;
            return this;
        }

        public GENChatTask OnStreamError(Action<string> onError)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onError += onError;
            return this;
        }

        public GENChatTask OnStreamComplete(Action<GeneratedText> onComplete)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onDone += onComplete;
            return this;
        }

        public GENChatTask OnStreamToolCall(Action<ToolCall[]> onToolCall)
        {
            streamHandler ??= new ChatStreamHandler();
            streamHandler.onToolCall += onToolCall;
            return this;
        }

        // Execution Method --------------------------------------------------------------------------------------------------  
        public override UniTask<GeneratedContent> ExecuteAsync() => GENTaskManager.GenerateChatAsync(this);
        public UniTask StreamAsync(ChatStreamHandler streamHandler = null) => GENTaskManager.StreamChatAsync(this, this.streamHandler?.SetGENTask(this) ?? streamHandler.SetGENTask(this));
    }

    public class GENVideoTask : GENMediaTask<GENVideoTask, GeneratedVideo>
    {
        public override MIMEType outputMimeType { get; set; } = MIMEType.MP4; // default to MP4
        public GENVideoTask(string promptText) : base(promptText) { }
        public GENVideoTask(Texture2D promptImage) : base(promptImage) { }

        // Execution Method -------------------------------------------------------------------------------------------------- 
        public override UniTask<GeneratedVideo> ExecuteAsync() => GENTaskManager.GenerateVideoAsync(this);
    }
}