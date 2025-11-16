using Glitch9.IO.Json.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Glitch9.AIDevKit.Components
{

    [Serializable]
    public class FunctionParameter
    {
        [SerializeField] private JsonSchemaType type;
        [SerializeField] private string name;
        [SerializeField] private string description;
        [SerializeField] private bool isRequired = true;
        [SerializeField] private string[] enumValues;
        [SerializeField] private JsonSchemaType elementType;

        public JsonSchemaType Type => type;
        public string Name => name;
        public string Description => description;
        public bool IsRequired => isRequired;
        public string[] EnumValues => enumValues;
        public JsonSchemaType ElementType => elementType;

        public FunctionParameter(JsonSchemaType type, string name, JsonSchemaType? elementType = null)
        {
            this.type = type;
            this.name = name;
            this.elementType = elementType ?? JsonSchemaType.String;
        }

        public FunctionParameter(string name, Type paramType)
        {
            this.name = name;
            this.type = JsonSchemaTypes.ConvertType(paramType);

            if (paramType.IsEnum)
            {
                enumValues = Enum.GetNames(paramType);
            }
            else if (type == JsonSchemaType.Enum)
            {
                Debug.LogError($"Type {paramType} marked as Enum but is not an enum. Name: {name}");
            }
            else if (type == JsonSchemaType.Array)
            {
                Type elementType = paramType.IsArray
                    ? paramType.GetElementType()
                    : paramType.GetGenericArguments().FirstOrDefault();

                if (elementType != null && elementType.IsEnum)
                {
                    enumValues = Enum.GetNames(elementType);
                }
            }
        }
    }

    [Serializable]
    public class FunctionReference
    {
        [SerializeField] private MonoBehaviour target;
        [SerializeField] private string methodName;
        [SerializeField] private string description;
        [SerializeReference] private List<FunctionParameter> parameters = new();

        public string MethodName => methodName;
        public string Description => description;
        public MonoBehaviour Target => target;
        public List<FunctionParameter> Parameters => parameters;
    }


    public class FunctionManager : MonoBehaviour
    {
        [SerializeField] private List<FunctionReference> functions = new();
        public List<FunctionReference> Functions => functions;

        public FunctionDeclaration[] GetFunctions()
        {
            List<FunctionDeclaration> functionTools = new();

            foreach (FunctionReference function in Functions)
            {
                if (function == null) continue;
                if (string.IsNullOrEmpty(function.MethodName))
                {
                    Debug.LogError($"Function '{function.MethodName}' is null or empty.");
                    continue;
                }

                JsonSchema schema = null;

                if (!function.Parameters.IsNullOrEmpty())
                {
                    schema = new();

                    foreach (FunctionParameter parameter in function.Parameters)
                    {
                        if (parameter == null) continue;

                        JsonSchemaType? arrayItemType =
                            parameter.Type == JsonSchemaType.Array
                            ? parameter.ElementType
                            : null;

                        schema.AddParameter(
                            parameter.Type,
                            parameter.Name,
                            parameter.Description,
                            parameter.IsRequired,
                            parameter.EnumValues,
                            arrayItemType
                        );
                    }
                }

                FunctionDeclaration functionTool = new()
                {
                    Name = function.MethodName,
                    Description = function.Description,
                    Parameters = schema,
                };

                functionTools.Add(functionTool);
                //GNDebug.Pink($"Function '{function.MethodName}' added to the session.");
            }

            return functionTools.ToArray();
        }

        // CallFunction Ver4 - Enum support added 
        public void CallFunction(string methodName, string jsonArguments)
        {
            foreach (FunctionReference function in functions)
            {
                if (function.MethodName != methodName)
                    continue;

                if (function.Target == null)
                {
                    Debug.LogWarning($"Function '{methodName}' has no target.");
                    return;
                }

                MethodInfo method = function.Target.GetType().GetMethod(methodName);
                if (method == null)
                {
                    Debug.LogWarning($"Method '{methodName}' not found in {function.Target.name}.");
                    return;
                }

                ParameterInfo[] parameters = method.GetParameters();
                object[] args;

                try
                {
                    if (parameters.Length == 0)
                    {
                        args = Array.Empty<object>();
                    }
                    else if (parameters.Length == 1)
                    {
                        var paramType = parameters[0].ParameterType;

                        if (jsonArguments.TrimStart().StartsWith("{"))
                        {
                            JObject jObj = JObject.Parse(jsonArguments);
                            JToken token = jObj.GetValue(parameters[0].Name, StringComparison.OrdinalIgnoreCase);
                            args = new object[] { token.ToObject(paramType) };
                        }
                        else
                        {
                            args = new object[] { JsonConvert.DeserializeObject(jsonArguments, paramType) };
                        }
                    }
                    else
                    {
                        JObject jObj = JObject.Parse(jsonArguments);
                        args = new object[parameters.Length];

                        for (int i = 0; i < parameters.Length; i++)
                        {
                            var param = parameters[i];

                            JToken token = jObj.GetValue(param.Name, StringComparison.OrdinalIgnoreCase) ??
                                throw new ArgumentException($"Missing argument: {param.Name}");

                            args[i] = token.ToObject(param.ParameterType);
                        }
                    }

                    method.Invoke(function.Target, args);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[FunctionManager] Failed to call '{methodName}': {ex.Message}\nArgs: {jsonArguments}");
                }

                return;
            }

            Debug.LogWarning($"Function '{methodName}' not registered in FunctionManager.");
        }
    }
}