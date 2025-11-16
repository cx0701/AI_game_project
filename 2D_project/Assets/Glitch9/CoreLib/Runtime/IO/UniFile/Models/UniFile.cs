using System;
using Cysharp.Threading.Tasks;

namespace Glitch9.IO.Files
{
    [Serializable]
    public class UniFile : UniFileBase<byte[]>
    {
        protected override async UniTask<byte[]> LoadFileAsync() => await BinaryUtils.LoadBytes(Path);
        public override byte[] ToBinaryData() => Value;
        public UniFile() : base(UniFileType.Binary) { }
        public UniFile(string filePath, string url = null) : base(UniFileType.Binary, filePath, url) { }
        public UniFile(byte[] data, string filePath, string url = null) : base(UniFileType.Binary, data, filePath, url) { }
    }
}