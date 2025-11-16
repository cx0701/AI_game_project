using System;
using System.Collections.Generic;
using System.Text;
using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit
{
    public class ChatDeltaResponse : StreamResponse<ChatDelta>
    {
        public static implicit operator ChatDelta(ChatDeltaResponse wrapper) => wrapper?.Delta;
        public static implicit operator ChatDeltaResponse(ChatDelta delta) => new() { Delta = delta };
        public ChatDelta Delta { get; set; }
        public Usage Usage { get; set; }
    }

    public class ChatStreamHandler : ITextStreamHandler
    {
        public Action onStart;
        public Action<string> onStream;
        public Action<ToolCall[]> onToolCall;
        public Action<string> onError;
        public Action<GeneratedText> onDone;
        public bool OnProgressEnabled { get; } = false;

        public ChatStreamHandler(
            Action onStart = null,
            Action<string> onStream = null,
            Action<ToolCall[]> onToolCall = null,
            Action<string> onError = null,
            Action<GeneratedText> onDone = null,
            IGENTask task = null)
        {
            if (onStart != null) this.onStart += onStart;
            if (onStream != null) this.onStream += onStream;
            if (onToolCall != null) this.onToolCall += onToolCall;
            if (onError != null) this.onError += onError;
            if (onDone != null) this.onDone += onDone;
            if (task != null) _task = task;
        }

        // Required.
        internal ChatStreamHandler Initialize(Func<string, IEnumerable<ChatDeltaResponse>> factory)
        {
            _deltaEnumerableFactory = factory;
            return this;
        }

        // Optional.
        internal ChatStreamHandler SetGENTask(IGENTask task)
        {
            _task = task;
            return this;
        }

        private IGENTask _task;
        private Func<string, IEnumerable<ChatDeltaResponse>> _deltaEnumerableFactory;
        private ChatDeltaResponse _lastDeltaResponse;
        private readonly StringBuilder _sb = new();
        private bool _isDone = false;



        public void Stream(string streamData)
        {
            //GNDebug.Mark(streamData);
            //GNDebug.Mark(1);
            if (_isDone) return;
            //GNDebug.Mark(2);
            if (string.IsNullOrEmpty(streamData)) return;
            //GNDebug.Mark(3);
            if (_deltaEnumerableFactory == null) return;
            //GNDebug.Mark(4);

            foreach (var deltaWrapper in _deltaEnumerableFactory(streamData))
            {
                //GNDebug.Mark(5);
                if (deltaWrapper == null) continue;
                //GNDebug.Mark(6);
                if (deltaWrapper.IsDone)
                {
                    FinishStreaming();
                    return;
                }
                //GNDebug.Mark(7);
                if (deltaWrapper.IsError)
                {
                    OnError(deltaWrapper.ErrorMessage);
                    return;
                }
                //GNDebug.Mark(8);
                _lastDeltaResponse = deltaWrapper;

                ChatDelta delta = deltaWrapper.Delta;
                if (delta == null) continue;
                //GNDebug.Mark(9);
                if (!string.IsNullOrEmpty(delta.Content))
                {
                    _sb.Append(delta);
                    onStream?.Invoke(delta);
                    if (!delta.ToolCalls.IsNullOrEmpty()) onToolCall?.Invoke(delta.ToolCalls);
                }
                //GNDebug.Mark(10);
            }
        }

        public void StartStreaming()
        {
            onStart?.Invoke();
        }


        public void OnProgress(float progress)
        {
            // do nothing
        }

        public void OnError(string error)
        {
            if (_isDone) return;
            //GNDebug.Mark("ERROR IS CALLED");
            onError?.Invoke(error);
        }

        public void FinishStreaming()
        {
            if (_isDone) return;
            //GNDebug.Mark("DONE IS CALLED");
            _isDone = true;

            if (_lastDeltaResponse?.Usage != null)
            {
                //GNDebug.Mark($"LAST DELTA USAGE FOUND: {_lastDeltaResponse.Usage}");
            }

            string streamedText = _sb.ToString();
            GeneratedText result = new(streamedText, _lastDeltaResponse?.Usage);

            if (_task != null && _task.enableHistory)
            {
                if (_task is GENTextTask textTask)
                {
                    GENTaskRecord.Create(textTask, result);
                }
                else if (_task is GENChatTask chatTask)
                {
                    GENTaskRecord.Create(chatTask, result);
                }
            }

            onDone?.Invoke(result);
            _sb.Clear();
        }
    }
}