using UnityEngine;

namespace Glitch9.AIDevKit.OpenAI
{
    public static class FluentAPIExtensions
    {
        private const string kSizeKey = "openai.size";
        private const string kQualityKey = "openai.quality";
        private const string kStyleKey = "openai.style";
        private const string kMaskKey = "openai.mask";

        /// <summary>
        /// Sets the size of the generated image (OpenAI only).
        /// </summary>
        /// <param name="size">The size of the generated image.</param>
        public static T SetSize<T>(this T task, ImageSize size) where T : IGENMediaTask
        {
            task.SetOption(kSizeKey, size);
            return task;
        }

        internal static ImageSize? GetSize<T>(this T task) where T : IGENMediaTask
        {
            if (task.TryGetOption(kSizeKey, out ImageSize size)) return size;
            return null;
        }

        /// <summary>
        /// Sets the quality of the generated image (OpenAI only).
        /// </summary>
        public static GENImageCreationTask SetQuality(this GENImageCreationTask task, ImageQuality quality)
        {
            task.SetOption(kQualityKey, quality);
            return task;
        }

        internal static ImageQuality? GetQuality(this GENImageCreationTask task)
        {
            if (task.TryGetOption(kQualityKey, out ImageQuality quality)) return quality;
            return null;
        }

        /// <summary>
        /// Sets the visual style of the image (OpenAI only).
        /// </summary>
        public static GENImageCreationTask SetStyle(this GENImageCreationTask task, ImageStyle style)
        {
            task.SetOption(kStyleKey, style);
            return task;
        }

        internal static ImageStyle? GetStyle(this GENImageCreationTask task)
        {
            if (task.TryGetOption(kStyleKey, out ImageStyle style)) return style;
            return null;
        }

        /// <summary>
        /// Sets the mask texture for inpainting (OpenAI only).
        /// </summary>
        public static GENImageEditTask SetMask(this GENImageEditTask task, Texture2D mask)
        {
            task.SetOption(kMaskKey, mask);
            return task;
        }

        internal static Texture2D GetMask(this GENImageEditTask task)
        {
            if (task.TryGetOption(kMaskKey, out Texture2D mask)) return mask;
            return null;
        }
    }
}