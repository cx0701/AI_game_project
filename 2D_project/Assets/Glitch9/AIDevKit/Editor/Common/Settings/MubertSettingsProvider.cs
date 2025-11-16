using Glitch9.AIDevKit.Mubert;
using Glitch9.ScriptableObjects;
using UnityEditor;

namespace Glitch9.AIDevKit.Editor.Mubert
{
    internal class MubertSettingsProvider : AIClientSettingsProvider<MubertSettingsProvider, MubertSettings>
    {
        //[SettingsProvider]
        public static SettingsProvider CreateSettingsProvider()
        {
            MubertSettingsProvider provider = new(AIDevKitEditorConfig.kProviderSettingsMubert)
            {
                deactivateHandler = DeactivateHandler,
                keywords = Keywords
            };
            return provider;
        }

        private static void DeactivateHandler()
        {
            MubertSettings.Instance.Save();
        }

        public MubertSettingsProvider(string path) : base(AIProvider.Mubert, true, path)
        {
        }

        protected override void InitializeSettings()
        {
            base.InitializeSettings();
        }

        protected override void DrawOptionalSettings()
        {

        }
        protected override void DrawAdditionalSections()
        {

        }
        protected override void DrawUsefulLinks()
        {
            AIDevKitGUI.UrlButtons(
                ("Get API Key Guide", ""),
                ("Create Mubert Account", "https://mubertapp.typeform.com/to/p6CzphzX?utm_source=Website&typeform-source=landing.mubert.com#page=Website"),
                ("Manage Mubert API Keys", "https://landing.mubert.com/")
            );

            AIDevKitGUI.UrlButtons(
                ("Pricing", "https://landing.mubert.com/#Pricing"),
                ("Platform", "https://mubert.com/render?utm_source=redirect&utm_medium=typeform&utm_campaign=api_form")
            );
        }


    }
}