using Glitch9.IO.RESTApi;

namespace Glitch9.AIDevKit.OpenAI.Realtime
{
    /// <summary>
    /// Contains all string constants representing OpenAI Realtime Event types.
    /// </summary>
    public static class RealtimeEventType
    {
        /// <summary>Returned when an error occurs.</summary>
        public const string Error = "error";

        /// <summary>Returned when a session is created. Emitted automatically when a new connection is established.</summary>
        public const string SessionCreated = "session.created";

        /// <summary>Send this event to update the session’s default configuration.</summary>
        public const string SessionUpdate = "session.update";

        /// <summary>Returned when a session is updated.</summary>
        public const string SessionUpdated = "session.updated";

        /// <summary>Send this event to append audio bytes to the input audio buffer.</summary>
        public const string InputAudioBufferAppend = "input_audio_buffer.append";

        /// <summary>Send this event to commit audio bytes to a user message.</summary>
        public const string InputAudioBufferCommit = "input_audio_buffer.commit";

        /// <summary>Send this event to clear the audio bytes in the buffer.</summary>
        public const string InputAudioBufferClear = "input_audio_buffer.clear";

        /// <summary>Returned when an input audio buffer is committed, either by the client or automatically in server VAD mode.</summary>
        public const string InputAudioBufferCommitted = "input_audio_buffer.committed";

        /// <summary>Returned when the input audio buffer is cleared by the client.</summary>
        public const string InputAudioBufferCleared = "input_audio_buffer.cleared";

        /// <summary>Returned in server turn detection mode when speech is detected.</summary>
        public const string InputAudioBufferSpeechStarted = "input_audio_buffer.speech_started";

        /// <summary>Returned in server turn detection mode when speech stops.</summary>
        public const string InputAudioBufferSpeechStopped = "input_audio_buffer.speech_stopped";

        /// <summary>Returned when a conversation is created. Emitted right after session creation.</summary>
        public const string ConversationCreated = "conversation.created";

        /// <summary>Send this event when adding an item to the conversation.</summary>
        public const string ConversationItemCreate = "conversation.item.create";

        /// <summary>Returned when input audio transcription is enabled and a transcription succeeds.</summary>
        public const string ConversationItemInputAudioTranscriptionCompleted = "conversation.item.input_audio_transcription.completed";

        /// <summary>Returned when input audio transcription is configured, and a transcription request for a user message failed.</summary>
        public const string ConversationItemInputAudioTranscriptionFailed = "conversation.item.input_audio_transcription.failed";

        /// <summary>Returned when a conversation item is created.</summary>
        public const string ConversationItemCreated = "conversation.item.created";

        /// <summary>Send this event when you want to truncate a previous assistant message’s audio.</summary>
        public const string ConversationItemTruncate = "conversation.item.truncate";

        /// <summary>Send this event when you want to remove any item from the conversation history.</summary>
        public const string ConversationItemDelete = "conversation.item.delete";

        /// <summary>Returned when an earlier assistant audio message item is truncated by the client.</summary>
        public const string ConversationItemTruncated = "conversation.item.truncated";

        /// <summary>Returned when an item in the conversation is deleted.</summary>
        public const string ConversationItemDeleted = "conversation.item.deleted";

        /// <summary>Send this event to trigger a response generation.</summary>
        public const string ResponseCreate = "response.create";

        /// <summary>Send this event to cancel an in-progress response.</summary>
        public const string ResponseCancel = "response.cancel";

        /// <summary>Returned when a new Response is created. The first event of response creation, where the response is in an initial state of "in_progress".</summary>
        public const string ResponseCreated = "response.created";

        /// <summary>Returned when a Response is done streaming. Always emitted, no matter the final state.</summary>
        public const string ResponseDone = "response.done";

        /// <summary>Returned when a new Item is created during response generation.</summary>
        public const string ResponseOutputItemAdded = "response.output_item.added";

        /// <summary>Returned when an Item is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseOutputItemDone = "response.output_item.done";

        /// <summary>Returned when a new content part is added to an assistant message item during response generation.</summary>
        public const string ResponseContentPartAdded = "response.content_part.added";

        /// <summary>Returned when a content part is done streaming in an assistant message item. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseContentPartDone = "response.content_part.done";

        /// <summary>Returned when the text value of a "text" content part is updated.</summary>
        public const string ResponseTextDelta = "response.text.delta";

        /// <summary>Returned when the text value of a "text" content part is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseTextDone = "response.text.done";

        /// <summary>Returned when the model-generated transcription of audio output is updated.</summary>
        public const string ResponseAudioTranscriptDelta = "response.audio_transcript.delta";

        /// <summary>Returned when the model-generated transcription of audio output is done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseAudioTranscriptDone = "response.audio_transcript.done";

        /// <summary>Returned when the model-generated audio is updated.</summary>
        public const string ResponseAudioDelta = "response.audio.delta";

        /// <summary>Returned when the model-generated audio is done. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseAudioDone = "response.audio.done";

        /// <summary>Returned when the model-generated function call arguments are updated.</summary>
        public const string ResponseFunctionCallArgumentsDelta = "response.function_call_arguments.delta";

        /// <summary>Returned when the model-generated function call arguments are done streaming. Also emitted when a Response is interrupted, incomplete, or cancelled.</summary>
        public const string ResponseFunctionCallArgumentsDone = "response.function_call_arguments.done";

        /// <summary>Emitted after every "response.done" event to indicate the updated rate limits.</summary>
        public const string RateLimitsUpdated = "rate_limits.updated";

        public const string ConversationItemInputAudioTranscriptionDelta = "conversation.item.input_audio_transcription.delta";
    }

}