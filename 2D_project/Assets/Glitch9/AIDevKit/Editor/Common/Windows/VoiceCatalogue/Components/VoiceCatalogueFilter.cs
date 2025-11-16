using Glitch9.EditorKit.IMGUI;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public class VoiceCatalogueFilter : TreeViewItemFilter
    {
        public override bool IsVisible(TreeViewItem item)
        {
            if (item is VoiceCatalogueTreeViewItem i)
            {
                if (VoiceCatalogueSettings.OnlyShowMyLibrary && !i.InMyLibrary) return false;
                if (VoiceCatalogueSettings.ApiProvider != AIProvider.All && VoiceCatalogueSettings.ApiProvider != i.Api) return false;
                if (VoiceCatalogueSettings.OnlyShowDefaultVoices && !i.IsDefault) return false;
                if (VoiceCatalogueSettings.OnlyShowMissingVoices && i.InMyLibrary) return false;
                if (VoiceCatalogueSettings.OnlyShowOfficialVoices && i.IsCustom) return false;
                if (VoiceCatalogueSettings.OnlyShowCustomVoices && !i.IsCustom) return false;
                if (VoiceCatalogueSettings.OnlyShowFeaturedVoices && !i.IsFeatured) return false;
                if (VoiceCatalogueSettings.VoiceCategory != VoiceCategory.None && VoiceCatalogueSettings.VoiceCategory != i.Category) return false;
                if (VoiceCatalogueSettings.VoiceGender != VoiceGender.None && VoiceCatalogueSettings.VoiceGender != i.Gender) return false;
                if (VoiceCatalogueSettings.VoiceType != VoiceType.None && VoiceCatalogueSettings.VoiceType != i.Type) return false;
                if (VoiceCatalogueSettings.VoiceAge != VoiceAge.None && VoiceCatalogueSettings.VoiceAge != i.Age) return false;
                if (VoiceCatalogueSettings.VoiceLanguage != SystemLanguage.Unknown && VoiceCatalogueSettings.VoiceLanguage != i.Language) return false;
            }

            return base.IsVisible(item);
        }
    }
}