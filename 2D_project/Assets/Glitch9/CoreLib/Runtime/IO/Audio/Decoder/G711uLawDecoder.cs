using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;

namespace Glitch9.CoreLib.IO.Audio
{
    public static class G711uLawDecoder
    {
        public static async UniTask<UniAudioFile> DecodeAsync(byte[] binaryData, string outputPath, AudioFormat format)
        {
            float[] samples = AudioProcessor.G711uLawToFloatArray(binaryData);
            byte[] pcm = AudioProcessor.FloatTo16BitPCM(samples);
            return await PCMDecoder.DecodeAsync(pcm, outputPath, format);
        }

        public static async UniTask<UniAudioFile> DecodeAsync(string base64Encoded, string outputPath, AudioFormat format)
            => await DecodeAsync(Convert.FromBase64String(base64Encoded), outputPath, format);

    }
}
