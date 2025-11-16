using UnityEditor;

namespace Glitch9.Setup
{
    [InitializeOnLoad]
    internal static class UniTaskChecker
    {
        private const string kAssemblyName = "UniTask";
        private const string kSymbolDefine = "CYSHARP_UNITASK";
        private const string kPackageName = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask";
        private const string kAssemblyInNeed = "Glitch9.CoreLib.IO";

        static UniTaskChecker()
        {
            DependencyManager.EnsureDependency(kSymbolDefine, kAssemblyName, kPackageName, kAssemblyInNeed);
        }
    }
}
