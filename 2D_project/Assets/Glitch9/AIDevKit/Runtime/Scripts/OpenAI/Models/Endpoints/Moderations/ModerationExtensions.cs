using System.Collections.Generic;

namespace Glitch9.AIDevKit.OpenAI
{
    public static class ModerationExtensions
    {
        private static ModerationType GetModerationType(string rawName)
        {
            return rawName switch
            {
                "sexual" => ModerationType.Sexual,
                "hate" => ModerationType.Hate,
                "harassment" => ModerationType.Harassment,
                "self-harm" => ModerationType.SelfHarm,
                "sexual/minors" => ModerationType.SexualMinors,
                "hate/threatening" => ModerationType.HateThreatening,
                "violence/graphic" => ModerationType.ViolenceGraphic,
                "self-harm/intent" => ModerationType.SelfHarmIntent,
                "self-harm/instructions" => ModerationType.SelfHarmInstructions,
                "harassment/threatening" => ModerationType.HarassmentThreatening,
                "violence" => ModerationType.Violence,
                _ => ModerationType.None
            };
        }

        public static ModerationCategory GetModerationCategory(ModerationType moderationType)
        {
            return moderationType switch
            {
                ModerationType.None => ModerationCategory.None,
                ModerationType.Sexual => ModerationCategory.Sexual,
                ModerationType.Hate => ModerationCategory.Hate,
                ModerationType.Harassment => ModerationCategory.Harassment,
                ModerationType.SelfHarm => ModerationCategory.SelfHarm,
                ModerationType.SexualMinors => ModerationCategory.SexualMinors,
                ModerationType.HateThreatening => ModerationCategory.HateThreatening,
                ModerationType.ViolenceGraphic => ModerationCategory.ViolenceGraphic,
                ModerationType.SelfHarmIntent => ModerationCategory.SelfHarmIntent,
                ModerationType.SelfHarmInstructions => ModerationCategory.SelfHarmInstructions,
                ModerationType.HarassmentThreatening => ModerationCategory.HarassmentThreatening,
                ModerationType.Violence => ModerationCategory.Violence,
                _ => ModerationCategory.None
            };
        }

        public static bool TryGetResult(this Moderation moderation, out List<ModerationData> results)
        {
            results = new();

            if (moderation == null || moderation.Results.IsNullOrEmpty())
            {
                return false;
            }

            bool flagged = false;

            foreach (ModerationResult result in moderation.Results)
            {
                if (result.Flagged)
                {
                    flagged = true;
                }

                foreach (KeyValuePair<string, bool> category in result.Categories)
                {
                    if (category.Value)
                    {
                        ModerationType type = GetModerationType(category.Key);
                        if (type != ModerationType.None)
                        {
                            ModerationCategory categoryValue = GetModerationCategory(type);
                            results.Add(new ModerationData(categoryValue, result.CategoryScores[category.Key], category.Value));
                        }
                    }
                }
            }

            return flagged;
        }

        public static List<ModerationData> ParseResult(this Moderation moderation)
        {
            List<ModerationData> results = new();

            if (moderation == null || moderation.Results.IsNullOrEmpty())
            {
                return results;
            }

            foreach (ModerationResult result in moderation.Results)
            {
                foreach (KeyValuePair<string, bool> category in result.Categories)
                {
                    ModerationType type = GetModerationType(category.Key);
                    if (type != ModerationType.None)
                    {
                        ModerationCategory categoryValue = GetModerationCategory(type);
                        results.Add(new ModerationData(categoryValue, result.CategoryScores[category.Key], category.Value));
                    }
                }
            }

            return results;
        }
    }
}