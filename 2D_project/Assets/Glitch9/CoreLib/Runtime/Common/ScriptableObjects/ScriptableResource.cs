using UnityEngine;

namespace Glitch9.ScriptableObjects
{
    public abstract class ScriptableResource<TSelf> : ScriptableObject
        where TSelf : ScriptableResource<TSelf>
    {
        private static TSelf _instance;
        public static TSelf Instance => CreateInstance();

        private static TSelf CreateInstance()
        {
            if (_instance == null)
            {
                AssetPathAttribute pathAttribute = AttributeCache<AssetPathAttribute>.Get<TSelf>();
                string path;

                if (pathAttribute != null)
                {
                    path = pathAttribute.Path;
                }
                else
                {
                    path = "Resources";
                }

                _instance = ScriptableObjectUtils.LoadSingleton<TSelf>(dirPath: path, create: true);
            }
            return _instance;
        }
    }
}