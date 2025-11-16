using System;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [Serializable]
    public class ApiKey
    {
        [SerializeField] private string key;
        [SerializeField] private bool encrypt;
        [SerializeField] private bool visible = true;

        public string GetKey()
        {
            string result = encrypt ? Encrypter.DecryptString(key) : key;
            if (string.IsNullOrEmpty(result)) throw new ArgumentException($"This API key is invalid. Please set a valid API key in the user preferences. (Edit > Preferences > AIDevKit)");
            return result;
        }
    }
}