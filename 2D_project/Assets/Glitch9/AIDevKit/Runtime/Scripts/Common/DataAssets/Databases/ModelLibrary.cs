using System.Collections.Generic;
using UnityEngine;
using Glitch9.Collections;
using Glitch9.ScriptableObjects;

namespace Glitch9.AIDevKit
{
    [CreateAssetMenu(fileName = nameof(ModelLibrary), menuName = AIDevKitConfig.kModelLibrary, order = AIDevKitConfig.kModelLibraryOrder)]
    public class ModelLibrary : ScriptableDatabase<ModelLibrary.Repo, Model, ModelLibrary>
    {
        public class Repo : Database<Model>
        {
#if UNITY_EDITOR
            public Repo()
            {
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (this == null) return;
                    if (Count == 0)
                    {
                        if (!InitialLoad())
                        {
                            Debug.LogError("There is no model in your library. Please add models to the library.");
                        }
                    }

                    // const string kPrefsKey = "AIDevKit.ModelLibrary.SetupV2";

                    // bool initialSetup = UnityEditor.EditorPrefs.GetBool(kPrefsKey, false);
                    // if (!initialSetup)
                    // {
                    //     if (InitialLoad())
                    //     {
                    //         UnityEditor.EditorPrefs.SetBool(kPrefsKey, true);
                    //     }
                    // }
                };
            }
#endif
        }

        public static Dictionary<AIProvider, List<Model>> GetFilteredRefs(ModelFilter filter)
        {
            Dictionary<AIProvider, List<Model>> map = new();

            foreach (Model model in DB.Values)
            {
                if (model == null) continue;

                if (filter.IsEmpty || filter.Matches(model))
                {
                    if (!map.ContainsKey(model.Api)) map.Add(model.Api, new());
                    map[model.Api].Add(model);
                }
            }

            return map;
        }

        public static List<Model> GetModelsByAPI(AIProvider api)
        {
            List<Model> models = new();

            foreach (Model model in DB.Values)
            {
                if (model == null) continue;
                if (model.Api == api) models.Add(model);
            }

            return models;
        }

        public static List<Model> GetJsonSchemaSupportedModels()
        {
            List<Model> models = new();

            foreach (Model model in DB.Values)
            {
                if (model == null) continue;
                if (model.OutputModality.HasFlag(ModelCapability.StructuredOutputs)) models.Add(model);
            }

            return models;
        }
    }
}