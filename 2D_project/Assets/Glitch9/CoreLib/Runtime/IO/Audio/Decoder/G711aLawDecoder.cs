using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;

namespace Glitch9.CoreLib.IO.Audio
{
    public static class G711aLawDecoder
    {
        // outputPath가 존재한다면, 파일을 저장하고 싶다는 의미임으로 temp폴더가 아니라 지정된 outputPath에 저장하도록 한다.
        public static async UniTask<UniAudioFile> DecodeAsync(byte[] binaryData, string outputPath, AudioFormat format)
        {
            float[] samples = AudioProcessor.G711aLawToFloatArray(binaryData);
            byte[] pcm = AudioProcessor.FloatTo16BitPCM(samples);
            return await PCMDecoder.DecodeAsync(pcm, outputPath, format);
        }

        public static async UniTask<UniAudioFile> DecodeAsync(string base64Encoded, string outputPath, AudioFormat format)
            => await DecodeAsync(Convert.FromBase64String(base64Encoded), outputPath, format);

    }
}
