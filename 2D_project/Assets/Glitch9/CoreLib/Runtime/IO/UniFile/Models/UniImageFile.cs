using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Glitch9.IO.Files
{
    [Serializable]
    public class UniImageFile : UniFileBase<Texture2D>
    {
        protected override async UniTask<Texture2D> LoadFileAsync()
        {
            return await Texture2DLoader.LoadAsync(Path, PathType.Absolute);
        }
        public override byte[] ToBinaryData() => Value.EncodeToPNG();

        public UniImageFile() : base(UniFileType.Image) { }
        public UniImageFile(string filePath, string url = null) : base(UniFileType.Image, filePath, url) { }
        public UniImageFile(Texture2D texture, string filePath, string url = null) : base(UniFileType.Image, texture, filePath, url) { }
        public UniImageFile(Texture2D texture) : base(UniFileType.Image, texture) { }
        public static UniImageFile FromUrl(string url) => new(null, url);


        public async UniTask<Sprite> GetSpriteAsync()
        {
            Texture2D tex = await LoadAsync();
            if (tex == null)
            {
                Debug.LogWarning("Failed to load image at (the file may not exist) " + Path);
                return null;
            }
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }

        public string EncodeToBase64PNG()
        {
            if (Value == null) return null;
            return Value.EncodeToBase64PNG();
        }
    }
}