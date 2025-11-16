using System.Collections.Generic;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal class OpenAIVoiceData : IVoiceData
    {
        public AIProvider Api => AIProvider.OpenAI;
        public string OwnedBy => AIProvider.OpenAI.ToString();
        public string PreviewPath => AIDevKitEditorPath.GetVoiceSampleAbsolutePath(Api, Id);
        public bool IsCustom => false;
        public bool? IsFree => true;

        public string Id { get; set; }
        public string Name { get; set; }
        public UnixTime? CreatedAt { get; set; }
        public string Description { get; set; }
        public VoiceCategory? Category { get; set; }
        public VoiceGender? Gender { get; set; }
        public VoiceType? Type { get; set; }
        public VoiceAge? Age { get; set; }
        public SystemLanguage Language { get; set; }
        public string Accent { get; set; }
    }

    /// <summary>
    /// OpenAI does not have an API for retrieving voices, so we need to hard-code them for now.
    /// </summary>
    internal static class OpenAIVoiceMeta
    {
        internal static string[] AllVoiceIds => new[]
        {
            Alloy,
            Echo,
            Fable,
            Onyx,
            Nova,
            Shimmer,
            Ash,
            Coral,
            Sage,
            Ballad
        };

        internal const string Alloy = "alloy";
        internal const string Echo = "echo";
        internal const string Fable = "fable";
        internal const string Onyx = "onyx";
        internal const string Nova = "nova";
        internal const string Shimmer = "shimmer";

        // New voice actors added 2025.01.23
        internal const string Ash = "ash";
        internal const string Coral = "coral";
        internal const string Sage = "sage";

        // New voice actors added 2025.04.14
        internal const string Ballad = "ballad";

        internal static readonly List<OpenAIVoiceData> List = new()
        {
            new OpenAIVoiceData
            {
                Id = Alloy,
                Name = "Alloy",
                Gender = VoiceGender.Male,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Narration,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Echo,
                Name = "Echo",
                Gender = VoiceGender.Male,
                Age = VoiceAge.MiddleAged,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Narration,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Fable,
                Name = "Fable",
                Gender = VoiceGender.Female,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.SocialMedia,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Onyx,
                Name = "Onyx",
                Gender = VoiceGender.Male,
                Age = VoiceAge.MiddleAged,
                Category = VoiceCategory.Premade,
                Type = VoiceType.News,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Nova,
                Name = "Nova",
                Gender = VoiceGender.Female,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Narration,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Shimmer,
                Name = "Shimmer",
                Gender = VoiceGender.Female,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.SocialMedia,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Ash,
                Name = "Ash",
                Gender = VoiceGender.Male,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Narration,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Coral,
                Name = "Coral",
                Gender = VoiceGender.Female,
                Age = VoiceAge.Young,
                Category = VoiceCategory.Premade,
                Type = VoiceType.SocialMedia,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Sage,
                Name = "Sage",
                Gender = VoiceGender.NonBinary,
                Age = VoiceAge.MiddleAged,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Narration,
                Language = SystemLanguage.English,
                Accent = "american"
            },
            new OpenAIVoiceData
            {
                Id = Ballad,
                Name = "Ballad",
                Gender = VoiceGender.Male,
                Age = VoiceAge.Senior,
                Category = VoiceCategory.Premade,
                Type = VoiceType.Characters,
                Language = SystemLanguage.English,
                Accent = "american"
            }
        };
    }
}