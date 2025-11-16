using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.Files;

namespace Glitch9.IO.RESTApi
{
    internal static class BinaryResponseConverter
    {
        private static readonly Dictionary<MIMEType, Func<byte[], string, AudioFormat, UniTask<UniAudioFile>>> _audioDecoders = new()
        {
            [MIMEType.MPEG] = MPEGDecoder.DecodeAsync,
            [MIMEType.PCM] = PCMDecoder.DecodeAsync,
            [MIMEType.WAV] = WavDecoder.DecodeAsync,
            [MIMEType.uLaw] = G711uLawDecoder.DecodeAsync,
            [MIMEType.aLaw] = G711aLawDecoder.DecodeAsync,
        };

        internal static async UniTask<RESTResponse> ConvertAsync(
            RESTRequest request,
            RESTResponse response,
            byte[] result,
            string contentType,
            RESTClient client)
        {
            if (!request.IgnoreLogs)
            {
                client.Logger.LogRequestDetails("Download Mode: Binary");
            }

            response.BinaryOutput = result;
            MIMEType mimeType = MIMETypeUtil.Parse(contentType);
            string outputPath = request.OutputPath?.ToAbsolutePath();

            // Image 
            if (mimeType == MIMEType.GIF)
            {
                client.Logger.LogResponseError("GIF is not supported. Texture2D will not be created.");
            }
            else if (mimeType.IsImage())
            {
                response.ImageOutput = ImageDecoder.DecodeBytes(result);
            }
            // Audio
            else if (_audioDecoders.TryGetValue(mimeType, out var decoder))
            {
                UniAudioFile file = await decoder(result, outputPath, request.OutputAudioFormat);
                if (file == null)
                {
                    client.Logger.LogResponseError($"Failed to decode {mimeType} audio from the HTTP response.");
                }
                else
                {
                    response.AudioOutput = file.Value;
                    response.OutputPath = file.Path;
                }
            }
            // Unknown
            else
            {
                client.Logger.Warning($"Unsupported MIME type: {mimeType}. Attempting to save as binary file.");
                response.FileOutput = await BinaryUtils.ToUniFile(result, outputPath);
            }

            await UniTask.Delay(1000);
            return response;
        }
    }
}