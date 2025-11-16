using System;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    public interface ISerializedContent
    {
        string Purpose { get; }
        bool IsNull { get; }
    }

    [Serializable]
    public abstract class SerializedContent<T> : ISerializedContent
    {
        [SerializeField] protected string purpose;
        [SerializeReference] protected T content;

        public string Purpose => purpose;
        public T Value => content;
        public bool IsNull => content == null;
    }

    [Serializable]
    public class SerializedTextContent : SerializedContent<string>
    {
        public static implicit operator SerializedTextContent(string value) => new(value);
        public static implicit operator string(SerializedTextContent value) => value.Value;
        public SerializedTextContent(string content) => this.content = content;
    }

    [Serializable]
    public class SerializedImageContent : SerializedContent<UniImageFile>
    {
        public static implicit operator SerializedImageContent(UniImageFile value) => new(value);
        public static implicit operator UniImageFile(SerializedImageContent value) => value.Value;
        public SerializedImageContent(UniImageFile imageFile) => content = imageFile;
    }

    [Serializable]
    public class SerializedAudioContent : SerializedContent<UniAudioFile>
    {
        public static implicit operator SerializedAudioContent(UniAudioFile value) => new(value);
        public static implicit operator UniAudioFile(SerializedAudioContent value) => value.Value;
        public SerializedAudioContent(UniAudioFile audioFile) => content = audioFile;
    }

    [Serializable]
    public class SerializedFileContent : SerializedContent<UniFile>
    {
        public static implicit operator SerializedFileContent(UniFile value) => new(value);
        public static implicit operator UniFile(SerializedFileContent value) => value.Value;
        public SerializedFileContent(UniFile file) => content = file;
        public SerializedFileContent(string path) => content = new(path);
    }

    [Serializable]
    public class SerializedVideoContent : SerializedContent<UniVideoFile>
    {
        public static implicit operator SerializedVideoContent(UniVideoFile value) => new(value);
        public static implicit operator UniVideoFile(SerializedVideoContent value) => value.Value;
        public SerializedVideoContent(UniVideoFile videoFile) => content = videoFile;
    }
}