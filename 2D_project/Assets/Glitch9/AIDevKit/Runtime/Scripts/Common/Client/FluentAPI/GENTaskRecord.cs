using Glitch9.IO.Files;
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Glitch9.AIDevKit
{
    [Serializable]
    public class GENTaskRecord : IData
    {
        // log info -------------------------------------------------------------
        [SerializeField] private string id = Guid.NewGuid().ToString();
        [SerializeField] private int taskType;
        [SerializeField] private string sender;
        [SerializeField] private UnixTime createdAt = UnixTime.Now;

        // request details 1: model info ------------------------------------------------------------
        [SerializeField] private AIProvider api;
        [SerializeField] private string modelId;
        [SerializeField] private string modelName;

        // request details 2: others ----------------------------------------------------------------
        [SerializeField, FormerlySerializedAs("metadata")] private Metadata requestOptions;
        [SerializeField] private Usage usage;
        [SerializeField] private Currency price;

        // input/output contents -------------------------------------------------
        [SerializeReference] private List<ISerializedContent> inputContents;
        [SerializeReference] private List<ISerializedContent> outputContents;

        public string Id => id;
        public string Name => modelName;
        public int TaskType => taskType;
        public string Sender => sender;
        public UnixTime CreatedAt => createdAt;
        public AIProvider Api => api;
        public string ModelId => modelId;
        public string ModelName => modelName;
        public List<ISerializedContent> InputContents => inputContents;
        public List<ISerializedContent> OutputContents => outputContents;

        public string InputText => GetFirstInputText();
        public string OutputText => GetFirstOutputText();

        public Metadata RequestOptions => requestOptions;
        public Usage Usage { get => usage; set => usage = value; }
        public Currency Price { get => price; set => price = value; }

        public GENTaskRecord() { }
        private GENTaskRecord InitializeCommon(int taskType, Model model, string sender, Usage usage)
        {
            this.taskType = taskType;
            this.sender = sender;
            this.usage = usage;

            if (model != null)
            {
                api = model.Api;
                modelId = model.Id;
                modelName = model.Name;
                if (usage != null) price = model.EstimatePrice(usage);
            }

            inputContents = new();
            outputContents = new();

            return this;
        }

        internal static GENTaskRecord Create(GENTextTask task, GeneratedText result)
        {
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.Completion, task.model, task.sender, result.Usage)
                .AddPromptText(task.promptText)
                .AddInputFiles(task.attachedFiles)
                .AddRequestOptions(task.options);

            r.inputContents = GENTaskRecordUtil.ConvertToSerializedContent(task.promptText, task.attachedFiles);
            r.outputContents = GENTaskRecordUtil.ConvertToSerializedContent(result);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENChatTask task, GeneratedText result)
        {
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.ChatCompletion, task.model, task.sender, result.Usage)
                .AddPromptText(task.message.Content.ToString())
                .AddInputFiles(task.message.AttachedFiles)
                .AddRequestOptions(task.options);

            ChatMessage input = task.message;

            r.inputContents = GENTaskRecordUtil.ConvertToSerializedContent(input.Content, input.AttachedFiles);
            r.outputContents = GENTaskRecordUtil.ConvertToSerializedContent(result);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENChatTask task, GeneratedContent result)
        {
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.ChatCompletion, task.model, task.sender, result.Usage)
                .AddPromptText(task.message.Content.ToString())
                .AddInputFiles(task.message.AttachedFiles)
                .AddRequestOptions(task.options);

            ChatMessage input = task.message;

            r.inputContents = GENTaskRecordUtil.ConvertToSerializedContent(input.Content, input.AttachedFiles);
            r.outputContents = GENTaskRecordUtil.ConvertToSerializedContent(result.Content);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENImageCreationTask task, GeneratedImage result)
        {
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.ImageCreation, task.model, task.sender, result.Usage)
                .AddPromptText(task.promptText)
                .AddOutputImages(result.ToFiles())
                .AddRequestOptions(task.options);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENImageEditTask task, GeneratedImage result)
        {
            // TODO: Add input image
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.ImageEdit, task.model, task.sender, result.Usage)
                .AddPromptText(task.promptText)
                .AddOutputImages(result.ToFiles())
                .AddRequestOptions(task.options);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENImageVariationTask task, GeneratedImage result)
        {
            // TODO: Add input image
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.ImageVariation, task.model, task.sender, result.Usage)
                .AddPromptText(task.promptText)
                .AddOutputImages(result.ToFiles())
                .AddRequestOptions(task.options);

            return r.AddToDatabase();
        }

        internal static GENTaskRecord Create(GENSpeechTask task, GeneratedAudio result)
        {
            var r = new GENTaskRecord()
                .InitializeCommon(GENTaskType.Speech, task.model, task.sender, result.Usage)
                .AddPromptText(task.promptText)
                .AddOutputAudio(result.ToFiles())
                .AddRequestOption("Voice", task.voice.Name)
                .AddRequestOption("Speed", task.speed)
                .AddRequestOption("Seed", task.seed)
                .AddRequestOptions(task.options);

            return r.AddToDatabase();
        }


        #region Getters

        public string GetMetadata(string key)
        {
            if (requestOptions == null) return null;
            return requestOptions.TryGetValue(key, out string metadata1) ? metadata1 : string.Empty;
        }

        private string _inputText;
        private string _outputText;
        internal string GetFirstInputText() => _inputText ??= GetFirstInputContent<SerializedTextContent>();
        internal string GetFirstOutputText() => _outputText ??= GetFirstOutputContent<SerializedTextContent>();
        internal UniAudioFile GetFirstInputAudio() => GetFirstInputContent<SerializedAudioContent>();
        internal UniAudioFile GetFirstOutputAudio() => GetFirstOutputContent<SerializedAudioContent>();

        internal List<UniImageFile> GetInputImages()
        {
            if (!inputContents.IsNullOrEmpty())
            {
                List<UniImageFile> images = new();

                foreach (ISerializedContent content in inputContents)
                {
                    if (content is SerializedImageContent imageResource &&
                        imageResource.Value != null)
                    {
                        images.Add(imageResource.Value);
                    }
                }

                return images;
            }

            return new();
        }

        internal List<UniImageFile> GetOutputImages()
        {
            if (!outputContents.IsNullOrEmpty())
            {
                List<UniImageFile> images = new();

                foreach (ISerializedContent content in outputContents)
                {
                    if (content is SerializedImageContent imageResource &&
                        imageResource.Value != null)
                    {
                        images.Add(imageResource.Value);
                    }
                }

                return images;
            }

            return new();
        }

        private T GetFirstInputContent<T>() where T : ISerializedContent => GetFirstContent<T>(inputContents);
        private T GetFirstOutputContent<T>() where T : ISerializedContent => GetFirstContent<T>(outputContents);
        private T GetFirstContent<T>(List<ISerializedContent> list) where T : ISerializedContent
        {
            if (!list.IsNullOrEmpty())
            {
                foreach (ISerializedContent content in list)
                {
                    if (content is T res && !res.IsNull) return res;
                }
            }

            return default;
        }



        #endregion Getters

        #region Setters 


        private GENTaskRecord AddPromptText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                inputContents.Add(new SerializedTextContent(text));
            return this;
        }

        private GENTaskRecord AddOutputTexts(string[] outputs)
        {
            if (outputs.IsNullOrEmpty()) return this;
            foreach (var text in outputs)
                if (!string.IsNullOrWhiteSpace(text))
                    outputContents.Add(new SerializedTextContent(text));
            return this;
        }

        private GENTaskRecord AddInputFiles(List<IUniFile> files)
        {
            if (files.IsNullOrEmpty()) return this;
            foreach (var f in files)
            {
                if (f is UniFile file)
                {
                    inputContents.Add(new SerializedFileContent(file));
                }
                else if (f is UniImageFile image)
                {
                    inputContents.Add(new SerializedImageContent(image));
                }
                else if (f is UniAudioFile audio)
                {
                    inputContents.Add(new SerializedAudioContent(audio));
                }
                else
                {
                    Debug.LogWarning($"Unsupported file type: {f.GetType()}");
                }
            }
            return this;
        }

        private GENTaskRecord AddOutputFiles(List<IUniFile> files)
        {
            if (files.IsNullOrEmpty()) return this;
            foreach (var f in files)
            {
                if (f is UniFile file)
                {
                    outputContents.Add(new SerializedFileContent(file));
                }
                else if (f is UniImageFile image)
                {
                    outputContents.Add(new SerializedImageContent(image));
                }
                else if (f is UniAudioFile audio)
                {
                    outputContents.Add(new SerializedAudioContent(audio));
                }
                else
                {
                    Debug.LogWarning($"Unsupported file type: {f.GetType()}");
                }
            }
            return this;
        }

        private GENTaskRecord AddOutputImages(List<UniImageFile> images)
        {
            if (images.IsNullOrEmpty()) return this;
            foreach (var image in images)
                if (image != null)
                    outputContents.Add(new SerializedImageContent(image));
            return this;
        }

        private GENTaskRecord AddOutputAudio(List<UniAudioFile> clips)
        {
            if (clips.IsNullOrEmpty()) return this;
            foreach (var c in clips)
                if (c != null)
                    outputContents.Add(new SerializedAudioContent(c));
            return this;
        }

        internal GENTaskRecord AddRequestOption(string key, string value)
        {
            ThrowIf.IsNullOrEmpty(key, "Prompt Record Metadata Key");

            requestOptions ??= new();
            requestOptions.AddOrUpdate(key, value);

            return this;
        }

        internal GENTaskRecord AddRequestOptions(Dictionary<string, object> map)
        {
            if (map.IsNullOrEmpty()) return this;

            foreach (var kvp in map)
            {
                if (kvp.Value is string strValue)
                    AddRequestOption(kvp.Key, strValue);
                else if (kvp.Value is int intValue)
                    AddRequestOption(kvp.Key, intValue);
                else if (kvp.Value is float floatValue)
                    AddRequestOption(kvp.Key, floatValue);
                else if (kvp.Value is Enum enumValue)
                    AddRequestOption(kvp.Key, enumValue);
            }

            return this;
        }

        internal GENTaskRecord AddRequestOption(string key, int? value)
        {
            if (value == null) return this;
            return AddRequestOption(key, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        internal GENTaskRecord AddRequestOption(string key, float? value)
        {
            if (value == null) return this;
            return AddRequestOption(key, value.Value.ToString(CultureInfo.InvariantCulture));
        }

        internal GENTaskRecord AddRequestOption<TEnum>(TEnum value) where TEnum : Enum
            => AddRequestOption(value.GetType().Name.ToPascalCase(), value);

        internal GENTaskRecord AddRequestOption<TEnum>(string key, TEnum value) where TEnum : Enum
        {
            if (value == null) return this;
            return AddRequestOption(key, value.GetInspectorName());
        }



        #endregion Setters

        #region Utility Methods

        internal GENTaskRecord AddToDatabase()
        {
            PromptHistory.Add(this);
            return this;
        }

        #endregion Utility Methods

        #region IEquatable Implementation

        public bool Equals(GENTaskRecord other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((GENTaskRecord)obj);
        }

        public override int GetHashCode()
        {
            return id != null ? id.GetHashCode() : 0;
        }

        public static bool operator ==(GENTaskRecord left, GENTaskRecord right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (ReferenceEquals(left, null)) return false;
            if (ReferenceEquals(right, null)) return false;
            return left.Equals(right);
        }

        public static bool operator !=(GENTaskRecord left, GENTaskRecord right)
        {
            return !(left == right);
        }

        #endregion IEquatable Implementation
    }
}