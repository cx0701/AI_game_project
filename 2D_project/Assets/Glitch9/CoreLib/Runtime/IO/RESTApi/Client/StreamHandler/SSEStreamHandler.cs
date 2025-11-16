using System;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace Glitch9.IO.RESTApi
{
    public interface IChunk
    {
        string ToTextDelta();
    }

    public class StreamResponse<TChunk> : Result<TChunk>
        where TChunk : class, IChunk, new()
    {
        public bool IsDone { get; set; } = false;
    }

    public class SSEStreamHandler<TChunk> : TextStreamHandler
        where TChunk : class, IChunk, new()
    {
        private readonly JsonSerializerSettings jsonSettings;
        private readonly SSEParser parser;

        public SSEStreamHandler(
            JsonSerializerSettings jsonSettings,
            SSEParser parser,
            Action onStart = null,
            Action<string> onStream = null,
            Action<string> onError = null,
            Action<float> onProgress = null,
            Action onDone = null) : base(onStart, onStream, onError, onProgress, onDone)
        {
            this.jsonSettings = jsonSettings;
            this.parser = parser;
        }

        public SSEStreamHandler(
            TextStreamHandler textStreamHandler,
            JsonSerializerSettings jsonSettings,
            SSEParser parser) : this(
                jsonSettings,
                parser,
                textStreamHandler.onStart,
                textStreamHandler.onStream,
                textStreamHandler.onError,
                textStreamHandler.onProgress,
                textStreamHandler.onDone)
        { }

        public string StreamingText => _sb.ToString(); // Readonly로 제공
        public TChunk LastChunk { get; set; }
        private readonly StringBuilder _sb = new();

        public override void Stream(string sseString)
        {
            if (string.IsNullOrEmpty(sseString)) return;

            var data = parser.Parse(sseString);

            if (data.IsNullOrEmpty())
            {
                onDone?.Invoke();
                return;
            }

            foreach (var (field, result) in data)
            {
                //AIDevKitDebug.Pink($"SSEStreamHandler: Field: {field}, Result: {result}");
                if (field != SSEField.Data || string.IsNullOrEmpty(result))
                {
                    //AIDevKitDebug.Pink($"SSEStreamHandler: Invalid field {field} or result {result}");
                    continue;
                }

                if (IsFinalMessage(result))
                {
                    //AIDevKitDebug.Pink($"SSEStreamHandler: Final message received: {result}");
                    onDone?.Invoke();
                    _sb.Clear();
                    break;
                }

                TChunk chunk = JsonConvert.DeserializeObject<TChunk>(result, jsonSettings);
                if (chunk == null)
                {
                    Debug.LogWarning($"SSEStreamHandler: Failed to deserialize chunk: {result}");
                    continue;
                }

                //AIDevKitDebug.Pink($"Type is {chunk.GetType()}");

                LastChunk = chunk;

                string text = chunk.ToTextDelta();
                onStream?.Invoke(text);
                _sb.Append(text);

                //GNDebug.Pink(text);
            }
            //AIDevKitDebug.Mark(4);
        }

        bool IsFinalMessage(string result)
        {
            if (string.IsNullOrEmpty(result)) return false;
            return parser.IsDone(result);
        }
    }
}