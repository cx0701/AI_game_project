using System;
using UnityEngine;

namespace Glitch9.IO.Files
{
    public static class ImageDecoder
    {
        /// <summary>
        /// This can convert JPEG, PNG, BMP, TGA, and TIFF formats.
        /// GIF is not supported.
        /// </summary>
        /// <param name="binaryData"></param>
        /// <returns></returns>
        public static Texture2D DecodeBytes(byte[] binaryData)
        {
            Texture2D texture = new(2, 2); // doesn't matter what size
            texture.LoadImage(binaryData);  // this will auto-resize the texture dimensions.
            return texture;
        }

        public static Texture2D DecodeBase64(string base64Encoded)
        {
            byte[] binaryData = Convert.FromBase64String(base64Encoded);
            return DecodeBytes(binaryData);
        }
    }
}