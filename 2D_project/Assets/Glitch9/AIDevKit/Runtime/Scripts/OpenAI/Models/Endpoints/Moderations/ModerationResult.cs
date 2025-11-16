using System.Collections.Generic;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.OpenAI
{
    public class ModerationResult
    {
        /// <summary>
        /// Whether any of the below categories are flagged.
        /// </summary>
        [JsonProperty("flagged")] public bool Flagged { get; set; }

        /// <summary>
        /// A list of the categories, and whether they are flagged or not.
        /// </summary>
        [JsonProperty("categories")] public Dictionary<string, bool> Categories { get; set; }

        /// <summary>
        /// A list of the categories along with their scores as predicted by model.
        /// </summary>
        [JsonProperty("category_scores")] public Dictionary<string, float> CategoryScores { get; set; }

        public override string ToString()
        {
            return $"Flagged: {Flagged}\nCategories: {string.Join(", ", Categories)}\nCategory Scores: {string.Join(", ", CategoryScores)}";
        }
    }
}