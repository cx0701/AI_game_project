namespace Glitch9.AIDevKit.Google
{
    public class Validator
    {
        public static void CheckResponse(GenerateContentResponse response, bool stream)
        {
            if (response.PromptFeedback?.BlockReason != null)
            {
                throw new BlockedPromptException(response.PromptFeedback);
            }

            if (!stream && response.Candidates[0].FinishReason != FinishReason.Unspecified &&
                response.Candidates[0].FinishReason != FinishReason.Stop &&
                response.Candidates[0].FinishReason != FinishReason.MaxTokens)
            {
                throw new StopCandidateException(response.Candidates[0]);
            }
        }
    }
}