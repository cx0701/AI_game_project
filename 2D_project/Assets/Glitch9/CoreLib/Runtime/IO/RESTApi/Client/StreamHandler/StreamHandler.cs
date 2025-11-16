using System;
using Glitch9.CoreLib.IO.Audio;

namespace Glitch9.IO.RESTApi
{
    public class TextStreamHandler : StreamHandler<string>, ITextStreamHandler
    {
        public TextStreamHandler(
            Action onStart = null,
            Action<string> onStream = null,
            Action<string> onError = null,
            Action<float> onProgress = null,
            Action onDone = null) : base(onStart, onStream, onError, onProgress, onDone) { }
    }

    public class BinaryStreamHandler : StreamHandler<byte[]>
    {
        public BinaryStreamHandler(
            Action onStart = null,
            Action<byte[]> onStream = null,
            Action<string> onError = null,
            Action<float> onProgress = null,
            Action onDone = null) : base(onStart, onStream, onError, onProgress, onDone) { }
    }

    public class AudioStreamHandler : StreamHandler<float[]>
    {
        public readonly AudioFormat audioFormat;
        public readonly int headerSize; // size of the header in bytes
        public AudioStreamHandler(
            AudioFormat audioFormat,
            int headerSize = 44, // 44 bytes for WAV header
            Action onStart = null,
            Action<float[]> onStream = null,
            Action<string> onError = null,
            Action<float> onProgress = null,
            Action onDone = null) : base(onStart, onStream, onError, onProgress, onDone)
        {
            this.audioFormat = audioFormat;
            this.headerSize = headerSize;
        }
    }

    public interface IStreamHandler
    {
        void StartStreaming();
        void OnError(string error);
        void OnProgress(float progress);
        void FinishStreaming();
        bool OnProgressEnabled { get; }
    }

    public interface ITextStreamHandler : IStreamHandler
    {
        void Stream(string data);
    }

    public abstract class StreamHandler<T> : IStreamHandler
    {
        public Action onStart;
        public Action<T> onStream;
        public Action<string> onError;
        public Action<float> onProgress;
        public Action onDone;
        public bool OnProgressEnabled => onProgress != null;

        public StreamHandler(
            Action onStart = null,
            Action<T> onStream = null,
            Action<string> onError = null,
            Action<float> onProgress = null,
            Action onDone = null)
        {
            this.onStart += onStart;
            this.onStream += onStream;
            this.onError += onError;
            this.onProgress += onProgress;
            this.onDone += onDone;
        }

        public virtual void StartStreaming() => onStart?.Invoke();
        public virtual void Stream(T data) => onStream?.Invoke(data);
        public virtual void OnError(string error)
        {
            onError?.Invoke(error);
            FinishStreaming();
        }
        public virtual void OnProgress(float progress) => onProgress?.Invoke(progress);
        public virtual void FinishStreaming() => onDone?.Invoke();
    }
}