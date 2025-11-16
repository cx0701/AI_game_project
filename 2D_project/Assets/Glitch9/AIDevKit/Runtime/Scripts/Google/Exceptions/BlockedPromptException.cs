using System;

namespace Glitch9.AIDevKit.Google
{
    public class BlockedPromptException : Exception
    {
        public PromptFeedback Feedback { get; }
        public BlockedPromptException(PromptFeedback feedback) : base(feedback.ToString())
        {
            Feedback = feedback;
        }
    }
}