using Glitch9.EditorKit.IMGUI;
using UnityEditor.IMGUI.Controls;

namespace Glitch9.AIDevKit.Editor.Pro
{
    public class ModelCatalogueFilter : TreeViewItemFilter
    {
        public override bool IsVisible(TreeViewItem item)
        {
            if (item is ModelCatalogueTreeViewItem i)
            {
                if (ModelCatalogueSettings.OnlyShowMyLibrary && !i.InMyLibrary) return false;
                if (ModelCatalogueSettings.ApiProvider != AIProvider.All && ModelCatalogueSettings.ApiProvider != i.Api) return false;
                if (ModelCatalogueSettings.OnlyShowMissingModels && i.InMyLibrary) return false;
                if (ModelCatalogueSettings.OnlyShowDefaultModels && !i.IsDefault) return false;
                if (ModelCatalogueSettings.OnlyShowOfficialModels && i.IsCustom) return false;
                if (ModelCatalogueSettings.OnlyShowCustomModels && !i.IsCustom) return false;

                if (ModelCatalogueSettings.TextGeneration && !i.Capability.HasFlag(ModelCapability.TextGeneration)) return false;
                if (ModelCatalogueSettings.StructuredOutput && !i.Capability.HasFlag(ModelCapability.StructuredOutputs)) return false;
                if (ModelCatalogueSettings.FunctionCalling && !i.Capability.HasFlag(ModelCapability.FunctionCalling)) return false;
                if (ModelCatalogueSettings.CodeExecution && !i.Capability.HasFlag(ModelCapability.CodeExecution)) return false;
                if (ModelCatalogueSettings.FineTuning && !i.Capability.HasFlag(ModelCapability.FineTuning)) return false;
                if (ModelCatalogueSettings.Streaming && !i.Capability.HasFlag(ModelCapability.Streaming)) return false;
                if (ModelCatalogueSettings.ImageGeneration && !i.Capability.HasFlag(ModelCapability.ImageGeneration)) return false;
                if (ModelCatalogueSettings.ImageInpainting && !i.Capability.HasFlag(ModelCapability.ImageInpainting)) return false;
                if (ModelCatalogueSettings.SpeechGeneration && !i.Capability.HasFlag(ModelCapability.SpeechGeneration)) return false;
                if (ModelCatalogueSettings.SpeechRecognition && !i.Capability.HasFlag(ModelCapability.SpeechRecognition)) return false;
                if (ModelCatalogueSettings.SoundFXGeneration && !i.Capability.HasFlag(ModelCapability.SoundFXGeneration)) return false;
                if (ModelCatalogueSettings.VoiceChanger && !i.Capability.HasFlag(ModelCapability.VoiceChanger)) return false;
                if (ModelCatalogueSettings.VideoGeneration && !i.Capability.HasFlag(ModelCapability.VideoGeneration)) return false;
                if (ModelCatalogueSettings.TextEmbedding && !i.Capability.HasFlag(ModelCapability.TextEmbedding)) return false;
                if (ModelCatalogueSettings.Moderation && !i.Capability.HasFlag(ModelCapability.Moderation)) return false;
                if (ModelCatalogueSettings.Search && !i.Capability.HasFlag(ModelCapability.Search)) return false;
                if (ModelCatalogueSettings.Realtime && !i.Capability.HasFlag(ModelCapability.Realtime)) return false;
                if (ModelCatalogueSettings.ComputerUse && !i.Capability.HasFlag(ModelCapability.ComputerUse)) return false;
            }

            return base.IsVisible(item);
        }
    }
}