using System;
using System.Text;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [Serializable]
    public class ProjectContext
    {
        public enum Platform
        {
            Mobile,
            PC,
            Xbox,
        }

        public enum LanguageTone
        {
            Casual,
            Formal,
        }

        [SerializeField] private Platform defaultPlatform = Platform.Mobile;
        [SerializeField] private LanguageTone tone = LanguageTone.Casual;
        [SerializeField] private bool isGame = true;
        [SerializeField] private string appCategory = "Productivity";
        [SerializeField] private string gameGenre = "Dating Sim";
        [SerializeField] private string gameTheme = "Cyberpunk";
        [SerializeField]
        private string description =
            "Set in a futuristic city ruled by mega-corporations, players build relationships with quirky androids and rebellious hackers. " +
            "The localization system supports tone-aware translation and platform-specific phrasing for smooth, immersive dialogue.";

        // getters
        public Platform DefaultPlatform => defaultPlatform;
        public LanguageTone Tone => tone;
        public bool IsGame => isGame;
        public string Genre => gameGenre;
        public string GameTheme => gameTheme;
        public string Description => description;

        public override string ToString()
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                if (isGame)
                {
                    sb.Append($"I'm developing a {gameGenre} game which takes place in a {gameTheme} world");
                }
                else
                {
                    sb.Append($"I'm developing a {appCategory} app");
                }

                sb.Append(description);

                return sb.ToString();
            }
        }

        public string GetShortDescription()
        {
            // this is followed by something like:
            // "Generate a texture for "
            // so the description should be like: 
            // "a {dating sim} game which takes place in a {cyberpunk} world. {description}"

            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                if (isGame)
                {
                    sb.Append($"a {gameGenre} game which takes place in a {gameTheme} world. ");
                }
                else
                {
                    sb.Append($"a {appCategory} app. ");
                }

                sb.Append(description);

                return sb.ToString();
            }
        }
    }
}