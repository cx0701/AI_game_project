using System;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.CoreLib.IO.Audio
{
    public static class WavDecoder
    {
        // outputPath가 존재한다면, 파일을 저장하고 싶다는 의미임으로 temp폴더가 아니라 지정된 outputPath에 저장하도록 한다.
        public static async UniTask<UniAudioFile> DecodeAsync(byte[] binaryData, string outputPath, AudioFormat format)
        {
            //binaryData = WavUtil.EnsureWavHeader(binaryData, format?.SampleRate ?? SampleRate.Hz44100);
            //return await AudioClipUtil.NonWebGLDecodeAsyncINTERNAL(binaryData, outputPath, AudioType.WAV);
            await UniTask.Yield();
            var name = AudioClipUtil.ParseAudioClipName(outputPath);
            var pcmData = PcmData.FromBytes(binaryData);
            //var clip = WavUtil.ToAudioClip(binaryData, 0, AudioClipUtil.ParseAudioClipName(outputPath));

            var clip = AudioClip.Create(name, pcmData.Length, pcmData.Channels, pcmData.SampleRate, false);
            clip.SetData(pcmData.Value, 0);
            await AudioEncoder.WriteFileIfValidPath(binaryData, outputPath);
            return new(clip, outputPath);
        }

        public static async UniTask<UniAudioFile> DecodeAsync(string base64Encoded, string outputPath, AudioFormat format)
            => await DecodeAsync(Convert.FromBase64String(base64Encoded), outputPath, format);
    }
}
