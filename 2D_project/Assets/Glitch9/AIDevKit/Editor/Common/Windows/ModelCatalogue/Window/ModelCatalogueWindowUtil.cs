using System.Collections.Generic;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor.Pro
{
    internal static class ModelCatalogueWindowUtil
    {
        internal static Dictionary<ModelCapability, (string, Texture)> CreateCapabilityMap()
        {
            Dictionary<ModelCapability, (string, Texture)> dict = new();

            foreach (ModelCapability capability in System.Enum.GetValues(typeof(ModelCapability)))
            {
                if (capability == ModelCapability.None) continue;

                string name = capability.GetName().Replace(" ", "\n");
                Texture icon = AIDevKitGUIUtility.GetCapabilityIcon(capability);

                dict.Add(capability, (name, icon));
            }

            return dict;
        }
    }
}