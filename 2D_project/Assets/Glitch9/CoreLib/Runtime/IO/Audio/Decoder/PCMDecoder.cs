using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Glitch9.IO.Files;
using UnityEngine;

namespace Glitch9.CoreLib.IO.Audio
{
    public static class PCMDecoder
    {
        public static async UniTask<UniAudioFile> DecodeAsync(byte[] binaryData, string outputPath, AudioFormat format)
        {
            binaryData = WavUtil.EnsureFileHeader(binaryData, format?.SampleRate ?? SampleRate.Hz44100);
            return await AudioClipUtil.NonWebGLDecodeAsyncINTERNAL(binaryData, outputPath, AudioType.WAV);
        }

        public static async UniTask<UniAudioFile> DecodeAsync(string base64Encoded, string outputPath, AudioFormat format)
            => await DecodeAsync(Convert.FromBase64String(base64Encoded), outputPath, format);
    }


    internal readonly struct PcmHeader
    {
        #region Public types & data

        public int BitDepth { get; }
        public int AudioSampleSize { get; }
        public int AudioSampleCount { get; }
        public ushort Channels { get; }
        public int SampleRate { get; }
        public int AudioStartIndex { get; }
        public int ByteRate { get; }
        public ushort BlockAlign { get; }

        #endregion

        #region Constructors

        private PcmHeader(int bitDepth, int audioSize, int audioStartIndex, ushort channels, int sampleRate, int byteRate, ushort blockAlign)
        {
            BitDepth = bitDepth;
            _negativeDepth = Mathf.Pow(2f, BitDepth - 1f);
            _positiveDepth = _negativeDepth - 1f;

            AudioSampleSize = bitDepth / 8;
            AudioSampleCount = Mathf.FloorToInt(audioSize / (float)AudioSampleSize);
            AudioStartIndex = audioStartIndex;

            Channels = channels;
            SampleRate = sampleRate;
            ByteRate = byteRate;
            BlockAlign = blockAlign;
        }

        #endregion

        #region Public Methods

        public static PcmHeader FromBytes(byte[] pcmBytes)
        {
            using var memoryStream = new MemoryStream(pcmBytes);
            return FromStream(memoryStream);
        }

        public static PcmHeader FromStream(Stream pcmStream)
        {
            using var reader = new BinaryReader(pcmStream);

            pcmStream.Position = 20;
            ushort audioFormatCode = reader.ReadUInt16();
            string audioFormat = GetAudioFormatFromCode(audioFormatCode);

            if (audioFormatCode != 1 && audioFormatCode != 65534)
            {
                Debug.LogWarning($"⚠️ Unsupported or unknown audio format code: {audioFormatCode}. Attempting to decode anyway.");
            }

            ushort channelCount = reader.ReadUInt16();
            int sampleRate = reader.ReadInt32();
            int byteRate = reader.ReadInt32();
            ushort blockAlign = reader.ReadUInt16();
            ushort bitDepth = reader.ReadUInt16();

            pcmStream.Position = 12;

            int audioStartIndex = -1;
            int audioSize = -1;

            while (pcmStream.Position < pcmStream.Length - 8)
            {
                string chunkId = new string(reader.ReadChars(4));
                int chunkSize = reader.ReadInt32();

                if (chunkId == "data")
                {
                    audioStartIndex = (int)pcmStream.Position;
                    audioSize = chunkSize;
                    break;
                }

                pcmStream.Position += chunkSize;
            }

            if (audioStartIndex < 0 || audioSize <= 0)
                throw new InvalidDataException("WAV 'data' chunk not found or empty.");

            return new PcmHeader(bitDepth, audioSize, audioStartIndex, channelCount, sampleRate, byteRate, blockAlign);
        }

        public float NormalizeSample(float rawSample)
        {
            float sampleDepth = rawSample < 0 ? _negativeDepth : _positiveDepth;
            return rawSample / sampleDepth;
        }

        #endregion

        #region Private Methods

        private static string GetAudioFormatFromCode(ushort code) => code switch
        {
            1 => "PCM",
            2 => "ADPCM",
            3 => "IEEE Float",
            4 => "IBM CVSD",
            6 => "A-Law",
            7 => "Mu-Law",
            65534 => "WaveFormatExtensible",
            _ => $"Unknown ({code})"
        };

        #endregion

        #region Private data

        private const int SizeIndex = 16;
        private readonly float _positiveDepth;
        private readonly float _negativeDepth;

        #endregion
    }

    internal readonly struct PcmData
    {
        #region Public data

        public float[] Value { get; }
        public int Length { get; }
        public int Channels { get; }
        public int SampleRate { get; }

        #endregion

        #region Constructor

        private PcmData(float[] value, int channels, int sampleRate)
        {
            Value = value;
            Length = value.Length;
            Channels = channels;
            SampleRate = sampleRate;
        }

        #endregion

        #region Public Methods

        public static PcmData FromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));

            PcmHeader pcmHeader = PcmHeader.FromBytes(bytes);

            if (pcmHeader.BitDepth != 8 && pcmHeader.BitDepth != 16 && pcmHeader.BitDepth != 32)
                throw new ArgumentOutOfRangeException(nameof(pcmHeader.BitDepth), pcmHeader.BitDepth, "Supported values are: 8, 16, 32");

            float[] samples = new float[pcmHeader.AudioSampleCount];

            for (int i = 0; i < samples.Length; ++i)
            {
                int byteIndex = pcmHeader.AudioStartIndex + i * pcmHeader.AudioSampleSize;
                float rawSample = pcmHeader.BitDepth switch
                {
                    8 => bytes[byteIndex],
                    16 => BitConverter.ToInt16(bytes, byteIndex),
                    32 => BitConverter.ToInt32(bytes, byteIndex),
                    _ => throw new ArgumentOutOfRangeException(nameof(pcmHeader.BitDepth))
                };

                samples[i] = pcmHeader.NormalizeSample(rawSample);
            }

            return new PcmData(samples, pcmHeader.Channels, pcmHeader.SampleRate);
        }

        #endregion
    }
}