using Glitch9.ScriptableObjects;
using System;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    public abstract class AIClientSettings<TSelf> : ScriptableResource<TSelf>
        where TSelf : AIClientSettings<TSelf>
    {
        [SerializeField] protected string apiKey;
        [SerializeField] protected string encryptedApiKey;
        [SerializeField] protected bool encryptApiKey;

        public string GetApiKey()
        {
            string result = encryptApiKey ? Encrypter.DecryptString(encryptedApiKey) : apiKey;
            if (string.IsNullOrEmpty(result)) throw new ArgumentException("Your API key is missing. Please set it in the user preferences. (Edit > Preferences > AIDevKit)");
            return result;
        }

        public virtual bool HasApiKey()
        {
            string result = encryptApiKey ? Encrypter.DecryptString(encryptedApiKey) : apiKey;
            return !string.IsNullOrEmpty(result);
        }
    }
}