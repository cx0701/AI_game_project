using UnityEditor;

namespace Glitch9.Setup
{
    [InitializeOnLoad]
    internal static class NewtonsoftJsonChecker
    {
        private const string kAssemblyName = "Newtonsoft.Json";
        private const string kSymbolDefine = "UNITY_NEWTONSOFT_JSON";
        private const string kPackageName = "com.unity.nuget.newtonsoft-json";

        static NewtonsoftJsonChecker()
        {
            DependencyManager.EnsureDependency(kSymbolDefine, kAssemblyName, kPackageName);
        }
    }
}
