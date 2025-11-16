using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

namespace Glitch9.IO.Files
{
    [JsonConverter(typeof(IUniFileConverter))]
    public interface IUniFile
    {
        string Path { get; set; }
        string Url { get; set; }
        MIMEType MimeType { get; set; }
        string Name { get; }
        bool IsLoading { get; }
        bool IsLoaded { get; }
        bool IsError { get; }
        string LastError { get; }
        bool IsValid { get; }
        UniFileType FileType { get; set; }
    }

    public class IUniFileConverter : JsonConverter<IUniFile>
    {
        public override void WriteJson(JsonWriter writer, IUniFile value, JsonSerializer serializer)
        {
            if (value == null) return;
            writer.WriteStartObject();
            if (!string.IsNullOrEmpty(value.Path))
            {
                writer.WritePropertyName("path");
                writer.WriteValue(value.Path);
            }
            if (!string.IsNullOrEmpty(value.Url))
            {
                writer.WritePropertyName("url");
                writer.WriteValue(value.Url);
            }

            writer.WritePropertyName("mime_type");
            writer.WriteValue(value.MimeType.ToString());
            writer.WritePropertyName("file_type");
            writer.WriteValue(value.FileType.ToString());

            if (value is UniAudioFile audioFile)
            {
                if (!string.IsNullOrEmpty(audioFile.Transcript))
                {
                    writer.WritePropertyName("transcript");
                    writer.WriteValue(audioFile.Transcript);
                }
            }

            writer.WriteEndObject();
        }

        public override IUniFile ReadJson(JsonReader reader, Type objectType, IUniFile existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null || reader.TokenType == JsonToken.EndArray)
                return null;

            // 명확하게 StartObject를 요구
            if (reader.TokenType != JsonToken.StartObject)
            {
                reader.Read(); // 강제 전진

                if (reader.TokenType != JsonToken.StartObject)
                    throw new JsonSerializationException($"Expected StartObject but got {reader.TokenType}");
            }

            UniFileType fileType = UniFileType.Binary;
            string path = null;
            string url = null;
            string transcript = null;
            MIMEType mimeType = MIMEType.Unknown;

            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;

                if (reader.TokenType != JsonToken.PropertyName)
                    continue;

                string propertyName = (string)reader.Value;
                if (!reader.Read()) break;

                switch (propertyName)
                {
                    case "file_type":
                        Enum.TryParse(reader.Value?.ToString(), true, out fileType);
                        break;
                    case "path":
                        path = reader.Value?.ToString();
                        break;
                    case "url":
                        url = reader.Value?.ToString();
                        break;
                    case "transcript":
                        transcript = reader.Value?.ToString();
                        break;
                    case "mime_type":
                        Enum.TryParse(reader.Value?.ToString(), true, out mimeType);
                        break;
                }
            }

            return fileType switch
            {
                UniFileType.Image => new UniImageFile(path, url) { MimeType = mimeType },
                UniFileType.Audio => new UniAudioFile(path, url) { MimeType = mimeType, Transcript = transcript },
                UniFileType.Video => new UniVideoFile(path, url) { MimeType = mimeType },
                _ => new UniFile(path, url) { MimeType = mimeType },
            };
        }

    }

    [Serializable]
    public abstract class UniFileBase<T> : IUniFile
    {
        private const int kStopLoadingDelay = 500;

        [SerializeField] private string path;
        [SerializeField] private string url; // Added 2025.04.23 to support URL loading in case of path is null or empty. 
        [SerializeField] private MIMEType mimeType;
        [SerializeField] private UniFileType fileType;
        [JsonIgnore] private T _value;

        /// <summary>
        /// Returns the absolute path of the file if it's a local file.
        /// Returns the URL if it's a web file.
        /// </summary>
        [JsonIgnore]
        public string Path
        {
            get => path;
            set => path = value;
        }

        [JsonIgnore]
        public string Url
        {
            get => url;
            set => url = value;
        }

        [JsonIgnore]
        public T Value
        {
            get => _value;
            set => _value = value;
        }

        [JsonIgnore]
        public MIMEType MimeType
        {
            get => mimeType;
            set => mimeType = value;
        }

        [JsonIgnore]
        public UniFileType FileType
        {
            get => fileType;
            set => fileType = value;
        }

        private string _fileName;

        [JsonIgnore]
        public string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    _fileName = System.IO.Path.GetFileName(Path);
                }
                return _fileName;
            }
        }

        [JsonIgnore] public bool IsLoading { get; private set; } = false;
        [JsonIgnore] public bool IsLoaded => _value != null;
        [JsonIgnore] public bool IsError { get; private set; } = false;
        [JsonIgnore] public string LastError { get; private set; }
        [JsonIgnore] public bool IsValid => !string.IsNullOrWhiteSpace(Path) && !IsLoading;

        protected UniFileBase(UniFileType fileType) => FileType = fileType;

        protected UniFileBase(UniFileType fileType, string filePath, string url = null) : this(fileType)
        {
            Path = filePath.ToAbsolutePath();
            Url = url;
        }

        protected UniFileBase(UniFileType fileType, T value, string filePath, string url = null) : this(fileType)
        {
            Path = filePath.ToAbsolutePath();
            Url = url;
            _value = value;
        }

        protected UniFileBase(UniFileType fileType, T value) : this(fileType)
        {
            _value = value;

#if UNITY_EDITOR 
            if (value is UnityEngine.Object unityObject)
            {
                Path = UnityEditor.AssetDatabase.GetAssetPath(unityObject);
                Url = Path.ToAbsolutePath();
            }
            else
            {
                Debug.LogError($"Unsupported type: {value.GetType()}");
            }
#endif
        }

        public async UniTask<T> LoadAsync(bool forceReload = false, Action<T> onLoaded = null)
        {
            if (forceReload) Debug.Log($"Reloading file at <color=yellow>{Path}</color>");

            if (string.IsNullOrWhiteSpace(Path))
                return default;

            if (IsLoaded && !forceReload)
                return _value;

            if (IsLoading)
                return _value;

            if (IsError)
            {
                LastError = $"Error loading file. Path:{Path}";
                return _value;
            }

            return await LoadAsyncINTERNAL(onLoaded);
        }

        private async UniTask<T> LoadAsyncINTERNAL(Action<T> onLoaded)
        {
            IsLoading = true;
            IsError = false;

            if (string.IsNullOrWhiteSpace(Path))
            {
                LastError = "File path is null or empty.";
                IsError = true;
                IsLoading = false;
                Debug.LogError(LastError);
            }
            else
            {
                try
                {
                    _value = await LoadFileAsync();
                    onLoaded?.Invoke(_value);
                }
                catch (Exception e)
                {
                    LastError = e.Message;
                    IsError = true;
                    IsLoading = false;
                    Debug.LogError(e);
                }
            }

            StopLoading().Forget();
            return _value;
        }

        private async UniTask StopLoading()
        {
            await UniTask.Delay(kStopLoadingDelay);
            IsLoading = false;
        }

        protected abstract UniTask<T> LoadFileAsync();
        public abstract byte[] ToBinaryData();

        public string EncodeToBase64()
        {
            if (Value == null) return null;
            return Convert.ToBase64String(ToBinaryData());
        }
    }
}