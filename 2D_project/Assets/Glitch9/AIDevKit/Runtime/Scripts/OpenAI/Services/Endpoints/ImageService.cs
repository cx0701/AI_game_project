using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using Glitch9.IO.RESTApi;
using System;
using UnityEngine;

namespace Glitch9.AIDevKit.OpenAI
{
    /// <summary>
    /// Partial Client class of OpenAIClient for Image requests. (Image Creation / Editing / Variation)
    /// Those requests have slightly different formats.
    /// </summary>
    public class ImageService : CRUDService<OpenAI>
    {
        internal const string kBaseUrl = "{ver}/images";
        internal const string kGenerationEndpoint = kBaseUrl + "/generations";
        internal const string kEditsEndpoint = kBaseUrl + "/edits";
        internal const string kVariationsEndpoint = kBaseUrl + "/variations";

        public ImageService(OpenAI client) : base(client) { }

        #region Image Creation

        /// <summary>
        /// Creates an image given a prompt.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async UniTask<GeneratedImage> Generate(ImageCreationRequest req)
        {
            try
            {
                req.Model ??= OpenAISettings.DefaultIMG;
                return await HandleImageCreationRequest(req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<GeneratedImage> Generate(
            string prompt,
            int imageCount,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            ImageQuality quality = OpenAIConfig.DefaultValues.IMAGE_QUALITY,
            ImageStyle style = OpenAIConfig.DefaultValues.IMAGE_STYLE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            ImageCreationRequest.Builder builder = GetCreationBuilder(prompt, model, size, quality, style, imageCount, downloadDir);
            return await Generate(builder.Build());
        }

        #endregion

        #region Image Edit

        /// <summary>
        /// Creates an edited or extended image given an original image and a prompt.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async UniTask<GeneratedImage> Edit(ImageEditRequest req)
        {
            try
            {
                return await HandleImageEditRequest(req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<GeneratedImage> Edit(
            FormFile image,
            string prompt,
            FormFile? mask = null,
            int imageCount = 1,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            ImageEditRequest.Builder builder = GetEditBuilder(image, prompt, mask, model, size, imageCount, downloadDir);
            return await Edit(builder.Build(MIMEType.MultipartForm));
        }

        public async UniTask<GeneratedImage> Edit(
            Texture2D image,
            string prompt,
            Texture2D mask = null,
            int imageCount = 1,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            return await Edit(
                new FormFile(image, $"@{image.name}.png"), prompt, mask != null ?
                    new FormFile(mask, $"{mask.name}.png") : (FormFile?)null, imageCount, model, size, downloadDir);
        }

        public async UniTask<GeneratedImage> Edit(
            Sprite image,
            string prompt,
            Sprite mask = null,
            int imageCount = 1,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            Texture2D maskTex = mask == null ? null : mask.texture;
            return await Edit(image.texture, prompt, maskTex, imageCount, model, size, downloadDir);
        }

        #endregion

        #region Image Variation

        /// <summary>
        /// Creates a variation of a given image.
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async UniTask<GeneratedImage> CreateVariation(ImageVariationRequest req)
        {
            try
            {
                return await HandleImageVariationRequest(req);
            }
            catch (Exception e)
            {
                Client.HandleException(e);
                return null;
            }
        }

        public async UniTask<GeneratedImage> CreateVariation(
            FormFile image,
            int imageCount,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            ImageVariationRequest.Builder builder = GetVariationBuilder(image, imageCount, model, size, downloadDir);
            return await CreateVariation(builder.Build(MIMEType.MultipartForm));
        }

        public async UniTask<GeneratedImage> CreateVariation(
            Texture2D image,
            int imageCount,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            return await CreateVariation(new FormFile(image, $"@{image.name}.png"), imageCount, model, size, downloadDir);
        }

        public async UniTask<GeneratedImage> CreateVariation(
            Sprite image,
            int imageCount,
            Model model = null,
            ImageSize size = OpenAIConfig.DefaultValues.IMAGE_SIZE,
            string downloadDir = null)
        {
            model ??= OpenAISettings.DefaultIMG;
            return await CreateVariation(image.texture, imageCount, model, size, downloadDir);
        }

        #endregion

        #region Request Handlers

        private async UniTask<GeneratedImage> HandleImageCreationRequest(ImageCreationRequest req)
        {
            ValidateRequest(req);
            req.OutputPath ??= AIDevKitPath.ResolveOutputFileName(req.Model, MIMEType.PNG);

            QueryResponse<Image> res = await OpenAI.CRUD.CreateAsync<ImageCreationRequest, QueryResponse<Image>>(kGenerationEndpoint, this, req);
            ThrowIf.ListIsNullOrEmpty(res?.Data, nameof(res));

            return await res!.Data.ToGeneratedImageAsync(req.OutputPath, req.ResponseFormat);
        }

        private async UniTask<GeneratedImage> HandleImageEditRequest(ImageEditRequest req)
        {
            ValidateRequest(req);
            req.OutputPath ??= AIDevKitPath.ResolveOutputFileName(req.Model, MIMEType.PNG);

            QueryResponse<Image> res = await OpenAI.CRUD.CreateAsync<ImageEditRequest, QueryResponse<Image>>(kEditsEndpoint, this, req);
            ThrowIf.ListIsNullOrEmpty(res?.Data, nameof(res));

            return await res!.Data.ToGeneratedImageAsync(req.OutputPath, req.ResponseFormat);
        }

        private async UniTask<GeneratedImage> HandleImageVariationRequest(ImageVariationRequest req)
        {
            ValidateRequest(req);
            req.OutputPath ??= AIDevKitPath.ResolveOutputFileName(req.Model, MIMEType.PNG);

            QueryResponse<Image> res = await OpenAI.CRUD.CreateAsync<ImageVariationRequest, QueryResponse<Image>>(kVariationsEndpoint, this, req);
            ThrowIf.ListIsNullOrEmpty(res.Data, nameof(res.Data));

            return await res!.Data.ToGeneratedImageAsync(req.OutputPath, req.ResponseFormat);
        }

        #endregion

        #region Helper Methods

        private void ValidateRequest(ImageCreationRequest req)
        {
            ThrowIf.ArgumentIsNull(req);
            ThrowIfModelIsNull(req.Model);
            ThrowIfPromptIsNullOrWhitespace(req.Prompt);
            //req.N = FixImageCountOrN(req.Model.ToEnum<DallEModel>(), req.N);
            req.N = FixN(req.N);
        }

        private void ValidateRequest(ImageEditRequest req)
        {
            ThrowIf.ArgumentIsNull(req);
            ThrowIfImageIsNullOrEmpty(req.Image);
            ThrowIfPromptIsNullOrWhitespace(req.Prompt);
            req.N = FixN(req.N);
        }

        private void ValidateRequest(ImageVariationRequest req)
        {
            ThrowIf.ArgumentIsNull(req);
            if (req.Prompt != null) req.Prompt = null; // Prompt is not allowed for ImageVariationRequest
            ThrowIfImageIsNullOrEmpty(req.Image);
            req.N = FixN(req.N);
        }

        private ImageCreationRequest.Builder GetCreationBuilder(string prompt, Model model, ImageSize size, ImageQuality quality, ImageStyle style, int imageCount, string outputPath)
        {
            ImageCreationRequest.Builder builder = new ImageCreationRequest.Builder().SetPrompt(prompt);
            if (model != OpenAIConfig.DefaultValues.DALLE_MODEL) builder.SetModel(model);
            if (size != OpenAIConfig.DefaultValues.IMAGE_SIZE) builder.SetSize(size);
            if (quality != OpenAIConfig.DefaultValues.IMAGE_QUALITY) builder.SetQuality(quality);
            if (imageCount != OpenAIConfig.DefaultValues.IMAGE_COUNT) builder.SetN(imageCount);
            if (model != OpenAIModel.DallE2 && style != OpenAIConfig.DefaultValues.IMAGE_STYLE) builder.SetStyle(style);
            if (!string.IsNullOrWhiteSpace(outputPath)) builder.SetOutputPath(ResolveOutputPath(outputPath, model));
            return builder;
        }

        private ImageEditRequest.Builder GetEditBuilder(FormFile image, string prompt, FormFile? mask, Model model, ImageSize size, int imageCount, string outputPath)
        {
            ImageEditRequest.Builder builder = new ImageEditRequest.Builder().SetPrompt(prompt).SetImage(image);
            if (model != OpenAIConfig.DefaultValues.DALLE_MODEL) builder.SetModel(model);
            if (mask != null && !mask.Value.IsEmpty) builder.SetMask(mask.Value);
            if (imageCount != OpenAIConfig.DefaultValues.IMAGE_COUNT) builder.SetN(imageCount);
            if (size != OpenAIConfig.DefaultValues.IMAGE_SIZE) builder.SetSize(size);
            if (!string.IsNullOrWhiteSpace(outputPath)) builder.SetOutputPath(ResolveOutputPath(outputPath, model));
            return builder;
        }

        private ImageVariationRequest.Builder GetVariationBuilder(FormFile image, int imageCount, Model model, ImageSize size, string outputPath)
        {
            ImageVariationRequest.Builder builder = new ImageVariationRequest.Builder().SetImage(image);
            if (model != OpenAIConfig.DefaultValues.DALLE_MODEL) builder.SetModel(model);
            if (imageCount != OpenAIConfig.DefaultValues.IMAGE_COUNT) builder.SetN(imageCount);
            if (size != OpenAIConfig.DefaultValues.IMAGE_SIZE) builder.SetSize(size);
            if (!string.IsNullOrWhiteSpace(outputPath)) builder.SetOutputPath(ResolveOutputPath(outputPath, model));
            return builder;
        }

        private void ThrowIfModelIsNull(Model model)
        {
            if (model == null) throw new ArgumentNullException("Please set the image model. (example. request.SetModel(ImageModel.DallE3))");
        }

        private void ThrowIfPromptIsNullOrWhitespace(string promptText)
        {
            ThrowIf.IsNullOrWhitespace(promptText, "Prompt");
        }

        private void ThrowIfImageIsNullOrEmpty(FormFile? image)
        {
            if (image == null || image.Value.IsEmpty) throw new ArgumentException("Image (FormFile) is null or empty.");
        }

        // private int? FixImageCountOrN(DallEModel model, int? n)
        // {
        //     if (n == null) return null;

        //     if (n is <= 0)
        //     {
        //         LogService.Warning("N cannot be less than or equal to 0. Setting N to 1.");
        //         return 1;
        //     }

        //     if (model == OpenAIModel.DallE2 && n > OpenAIConfig.DefaultValues.DALLE_2_MAX_N)
        //     {
        //         LogService.Warning($"N is greater than the maximum value for DALL-E 2. Setting N to {OpenAIConfig.DefaultValues.DALLE_2_MAX_N}.");
        //         return OpenAIConfig.DefaultValues.DALLE_2_MAX_N;
        //     }

        //     if (model == OpenAIModel.DallE3 && n > OpenAIConfig.DefaultValues.DALLE_3_MAX_N)
        //     {
        //         LogService.Warning($"N is greater than the maximum value for DALL-E 3. Setting N to {OpenAIConfig.DefaultValues.DALLE_3_MAX_N}.");
        //         return OpenAIConfig.DefaultValues.DALLE_3_MAX_N;
        //     }

        //     return n.Value;
        // }

        private int? FixN(int? n)
        {
            if (n == null) return null;

            if (n is <= 0)
            {
                LogService.Warning("N cannot be less than or equal to 0. Setting N to 1.");
                return 1;
            }

            return n.Value;
        }

        private string ResolveOutputPath(string outputPath, Model model)
        {
            ThrowIf.IsNullOrWhitespace(outputPath, "Download Directory");
            return AIDevKitPath.ResolveOutputFilePath(outputPath, model, MIMEType.PNG);
        }

        #endregion


    }
}


