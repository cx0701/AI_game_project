using System.Threading;
using Glitch9.IO.Networking.WebSocket;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI.Realtime
{
    /// <summary>
    /// Handles all the different types of <c>Realtime API</c> events 
    /// that can be sent to or received from the OpenAI API.
    /// </summary>
    public class RealtimeEvent : IWebSocketEvent
    {
        [JsonIgnore] public CancellationTokenSource CancellationTokenSource { get; set; } = new();

        /// <summary>
        /// <para><c>Requests</c>: Optional client-generated ID used to identify this event.</para>
        /// <para><c>Responses</c>: The unique ID of the realtime event.</para>
        /// </summary>
        [JsonProperty("event_id")] public string EventId { get; set; }

        /// <summary>
        /// The event type.
        /// </summary>
        [JsonProperty("type")] public string Type { get; set; }

        /// <summary>
        /// The session resource.
        /// </summary>
        [JsonProperty("session")] public RealtimeSession Session { get; set; }

        /// <summary>
        /// <para><see cref="RealtimeEventType.ResponseCreate"/>: Configuration for the response.</para>
        /// </summary>
        [JsonProperty("response")] public RealtimeItem Response { get; set; }

        /// <summary>
        /// Details of the error.
        /// </summary>
        [JsonProperty("error")] public ErrorResponse Error { get; set; }

        /// <summary>
        /// The conversation resource.
        /// </summary>
        [JsonProperty("conversation")] public Conversation Conversation { get; set; }

        /// <summary>
        /// The item to add to the conversation.
        /// </summary>
        [JsonProperty("item")] public RealtimeItem Item { get; set; }

        /// <summary>
        /// The ID of the response.
        /// </summary>
        [JsonProperty("response_id")] public string ResponseId { get; set; }

        /// <summary>
        /// The ID of the item or the function call item.
        /// </summary>
        [JsonProperty("item_id")] public string ItemId { get; set; }

        /// <summary>
        /// The ID of the preceding item after which the new item will be inserted.
        /// </summary>
        [JsonProperty("previous_item_id")] public string PreviousItemId { get; set; }

        /// <summary>
        /// The index of the output item in the response.
        /// </summary>
        [JsonProperty("output_index")] public int? OutputIndex { get; set; }

        /// <summary>
        /// The index of the content part in the item's content array.
        /// </summary>
        [JsonProperty("content_index")] public int? ContentIndex { get; set; }

        /// <summary>
        /// The ID of the function call.
        /// </summary>
        [JsonProperty("call_id")] public string CallId { get; set; }

        /// <summary>
        /// <see cref="RealtimeEventType.ResponseAudioTranscriptDone"/>: The final transcript of the audio.
        /// </summary>
        [JsonProperty("transcript")] public string Transcript { get; set; }

        /// <summary>
        /// The final text content.
        /// </summary>
        [JsonProperty("text")] public string Text { get; set; }

        /// <summary>
        /// Base64-encoded audio bytes.
        /// </summary>
        /// <remarks>
        /// This property is only used when the event type is <see cref="RealtimeEventType.InputAudioBufferAppend"/>.
        /// </remarks>
        [JsonProperty("audio")] public string Audio { get; set; }

        /// <summary>
        /// <para><see cref="RealtimeEventType.ResponseContentPartAdded"/>: The content part that was added.</para>
        /// <para><see cref="RealtimeEventType.ResponseContentPartDone"/>: The content part that is done.</para>
        /// </summary>
        [JsonProperty("part")] public RealtimeItemContent Part { get; set; }

        /// <summary>
        /// The final arguments as a JSON string.
        /// </summary>
        [JsonProperty("arguments")] public string Arguments { get; set; }

        /// <summary>
        /// <para><see cref="RealtimeEventType.ResponseTextDelta"/>: The text delta.</para>
        /// <para><see cref="RealtimeEventType.ResponseAudioTranscriptDelta"/>: The transcript delta.</para>
        /// <para><see cref="RealtimeEventType.ResponseAudioDelta"/>: Base64-encoded audio data delta.</para>
        /// <para><see cref="RealtimeEventType.ResponseFunctionCallArgumentsDelta"/>: The arguments delta as a JSON string.</para>
        /// </summary>
        [JsonProperty("delta")] public string Delta { get; set; }

        /// <summary>
        /// List of rate limit information.
        /// </summary>
        [JsonProperty("rate_limits")] public RateLimit[] RateLimits { get; set; }

        /// <summary>
        /// Milliseconds since the session started when speech was detected.
        /// </summary>
        [JsonProperty("audio_start_ms")] public int? AudioStartMs { get; set; }

        /// <summary>
        /// Inclusive duration up to which audio is truncated, in milliseconds.
        /// </summary>
        [JsonProperty("audio_end_ms")] public int? AudioEndMs { get; set; }
    }
}
