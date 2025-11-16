namespace Glitch9.AIDevKit
{
    public class ModerationData
    {
        public ModerationCategory Category { get; private set; }
        public float Score { get; private set; }
        public bool Flagged { get; private set; }

        public ModerationData(ModerationCategory category, float score, bool flagged)
        {
            Category = category;
            Score = score;
            Flagged = flagged;
        }
    }
}