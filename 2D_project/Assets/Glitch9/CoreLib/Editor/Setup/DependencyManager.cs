using System;
using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Glitch9.Setup
{
    public static class DependencyManager
    {
        public static void EnsureDependency(string symbolDefine, string assemblyName, string packageName, string assemblyInNeed = null)
        {
            if (!HasDefine(symbolDefine))
            {
                if (IsAssemblyPresent(assemblyName))
                {
                    AddDefine(symbolDefine);
                    Debug.Log($"✔ {symbolDefine} define added!");
                }
                else
                {
                    // if (EditorUtility.DisplayDialog("Package Required", $"The {assemblyName} package is not installed. Would you like to install it?", "Yes", "No"))
                    // {
                    //     try
                    //     {
                    //         InstallPackage(packageName);
                    //         Debug.Log($"✔ {assemblyName} package installed!");
                    //     }
                    //     catch (Exception e)
                    //     {
                    //         Debug.LogError($"✘ Failed to install {assemblyName} package: {e.Message}");
                    //     }
                    // }

                    bool installPackage = true;
                    if (!string.IsNullOrEmpty(assemblyInNeed)) installPackage = IsAssemblyPresent(assemblyInNeed);

                    if (installPackage)
                    {
                        // don't ask the user to install the package, just install it directly
                        // user has to install it anyway. extra step can be annoying
                        InstallPackage(packageName);
                        Debug.Log($"✔ {assemblyName} package installed!");
                    }
                }
            }
        }

        static bool IsAssemblyPresent(string assemblyName)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Any(asm => asm.FullName.Contains(assemblyName));
        }

        static bool HasDefine(string define)
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            return PlayerSettings.GetScriptingDefineSymbolsForGroup(target).Contains(define);
        }

        static void AddDefine(string define)
        {
            var target = EditorUserBuildSettings.selectedBuildTargetGroup;
            var defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);
            if (!defines.Contains(define))
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(target, defines + ";" + define);
            }
        }

        private static void InstallPackage(string packageName)
        {
            Debug.Log($"Installing {packageName} package...");
            Client.Add(packageName);
        }
    }
}
