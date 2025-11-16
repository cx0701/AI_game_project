using System;
using Cysharp.Threading.Tasks;
using Glitch9.CoreLib.IO.Audio;
using Glitch9.IO.Files;
using Newtonsoft.Json;

namespace Glitch9.IO.RESTApi
{
    internal static class TextResponseConverter
    {
        internal static async UniTask<RESTResponse<TResBody>> ConvertAsync<TResBody>(
            RESTRequest request,
            RESTResponse<TResBody> response,
            string textResult,
            string contentType,
            RESTClient client)
        {
            if (!request.IgnoreLogs)
            {
                client.Logger.LogRequestDetails("Download Mode: Text");
                client.Logger.LogResponseBody(textResult);
            }

            response.TextOutput = textResult;
            MIMEType mimeType = MIMETypeUtil.Parse(contentType);
            string outputPath = request.OutputPath?.ToAbsolutePath();

            switch (mimeType)
            {
                case MIMEType.Xml or MIMEType.CSV or MIMEType.HTML or MIMEType.MultipartForm:
                    client.Logger.LogResponseError($"{mimeType} is not supported. Result object will not be created.");
                    return response;

                case MIMEType.MPEG:
                    var mpeg = await MPEGDecoder.DecodeAsync(textResult, outputPath, request.OutputAudioFormat);
                    response.AudioOutput = mpeg?.Value;
                    response.OutputPath = mpeg?.Path;
                    return response;

                case MIMEType.WAV:
                    var wav = await WavDecoder.DecodeAsync(textResult, outputPath, request.OutputAudioFormat);
                    response.AudioOutput = wav?.Value;
                    response.OutputPath = wav?.Path;
                    return response;

                case MIMEType.Json or MIMEType.WWWForm:
                default:  // fallback (e.g. JSON or unknown) 
                    TResBody body = DeserializeResponseBody<TResBody>(textResult, client);
                    response.Body = body;
                    return response;
            }
        }

        private static TResBody DeserializeResponseBody<TResBody>(string jsonString, RESTClient client)
        {
            if (client.JsonSettings == null)
            {
                client.Logger.LogResponseError("Deserialize failed. JSON settings are null.");
                return default;
            }

            if (string.IsNullOrEmpty(jsonString))
            {
                client.Logger.LogResponseError("JSON string is null or empty.");
                return default;
            }

            Type type = typeof(TResBody);
            string typeName;

            if (type.IsGenericType)
            {
                string genericName = type.GetGenericArguments()[0].Name;
                typeName = $"{type.Name.Split('`')[0]}<{genericName}>";
            }
            else
            {
                typeName = type.Name;
            }

            try
            {
                // return JsonConvert.DeserializeObject<TResponse>(jsonString, client.JsonSettings);
                return (TResBody)JsonConvert.DeserializeObject(jsonString, typeof(TResBody), client.JsonSettings);
            }
            catch (JsonReaderException e)
            {
                client.Logger.LogResponseError(
                    $"Failed to deserialize <color=yellow>{typeName}</color> at " +
                    $"line {e.LineNumber}, position {e.LinePosition}.\n" +
                    $"Message: {e.Message}");
                return default;
            }
        }
    }
}