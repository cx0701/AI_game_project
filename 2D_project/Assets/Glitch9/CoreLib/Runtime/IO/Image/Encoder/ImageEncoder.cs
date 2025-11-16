using System;
using UnityEngine;

namespace Glitch9.IO.Files
{
    public static class ImageEncoder
    {
        public static string EncodeToBase64PNG(this Texture2D texture)
        {
            byte[] binaryData = texture.EncodeToPNG();
            return Convert.ToBase64String(binaryData);
        }
    }
}