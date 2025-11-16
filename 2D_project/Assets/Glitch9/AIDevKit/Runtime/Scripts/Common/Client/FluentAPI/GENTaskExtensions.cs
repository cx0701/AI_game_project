using System;

namespace Glitch9.AIDevKit
{
    public static class GENTaskExtensions
    {
        public static CompletionRequest ToCompletionRequest(this GENTextTask task, Type jsonSchemaType, bool isStreaming)
        {
            var req = new CompletionRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetModelOptions(task.modelOptions)
                .SetReasoningOptions(task.reasoningOptions)
                .SetCancellationToken(task.token)

                // prompt starts
                .SetInstruction(task.instruction)
                .SetPrompt(task.promptText)
                .SetJsonSchema(jsonSchemaType)
                .AttachedFiles(task.attachedFiles);
            // prompt ends 

            if (isStreaming) req.SetStream(true).IncludeUsage();

            return req.Build();
        }

        public static ChatCompletionRequest ToChatCompletionRequest(this GENTextTask task, Type jsonSchemaType, bool isStreaming)
        {
            var req = new ChatCompletionRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetModelOptions(task.modelOptions)
                .SetReasoningOptions(task.reasoningOptions)
                .SetCancellationToken(task.token)

                // prompt starts
                .SetInstruction(task.instruction)
                .SetPrompt(task.promptText)
                .SetJsonSchema(jsonSchemaType)
                .AttachedFiles(task.attachedFiles)
                .SetWebSearchOptions(task.webSearchOptions);
            // prompt ends

            if (task.speechOutputOptions != null) req.SetSpeechOutput(task.speechOutputOptions);
            if (isStreaming) req.SetStream(true).IncludeUsage();

            return req.Build();
        }

        public static ChatCompletionRequest ToChatCompletionRequest(this GENChatTask task, bool isStreaming)
        {
            var req = new ChatCompletionRequest.Builder()
                .SetSender(task.sender)
                .SetIgnoreLogs(task.ignoreLogs)
                .SetModel(task.model.Id)
                .SetModelOptions(task.session.ModelOptions)
                .SetReasoningOptions(task.session.ReasoningOptions)
                .SetCancellationToken(task.token)

                // prompt starts
                .SetInstruction(task.session.SystemInstruction)
                .SetStartingMessage(task.session.StartingMessage)
                .SetMessages(task.session.Messages)
                .SetFunctions(task.functions)
                .AttachedFiles(task.message.AttachedFiles)
                .SetWebSearchOptions(task.session.WebSearchOptions);
            // prompt ends 

            if (task.session.SpeechOutputOptions != null) req.SetSpeechOutput(task.session.SpeechOutputOptions);
            if (isStreaming) req.SetStream(true).IncludeUsage();

            return req.Build();
        }
    }
}