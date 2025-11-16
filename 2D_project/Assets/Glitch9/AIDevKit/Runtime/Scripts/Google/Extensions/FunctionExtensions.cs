using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Glitch9.AIDevKit.Components;
using Glitch9.IO.Json.Schema;
using Glitch9.IO.RESTApi;
using Newtonsoft.Json;

namespace Glitch9.AIDevKit.Google
{
    public static class FunctionExtensions
    {
        private static RESTLogger Logger => GenerativeAI.DefaultInstance.Logger;

        public static async UniTask<FunctionResponse> Invoke(this FunctionDeclaration functionDeclaration, FunctionCall call)
        {
            if (functionDeclaration == null || !functionDeclaration.IsCallable)
            {
                throw new InvalidOperationException("Function is not callable.");
            }

            if (call.Args.IsNullOrEmpty())
            {
                throw new ArgumentException("Function call arguments cannot be null or empty.", nameof(call));
            }

            string serializedArg = JsonConvert.SerializeObject(call.Args);

            IResult res = await functionDeclaration.Delegate.Invoke(serializedArg);

            if (res.IsFailure) throw new InvalidOperationException(res.ErrorMessage);
            if (res is not Result<string> stringResult) throw new InvalidOperationException("Unexpected result type.");

            string resultObjectAsString = stringResult.Value;

            if (string.IsNullOrEmpty(resultObjectAsString))
            {
                throw new InvalidOperationException("Function response is empty.");
            }

            return new()
            {
                Name = functionDeclaration.Name,
                Response = JsonConvert.DeserializeObject(resultObjectAsString)
            };
        }

        public static FunctionLibrary GetFunctionLibrary(this FunctionManager functionManager)
        {
            if (functionManager == null || functionManager.Functions.IsNullOrEmpty())
                return null;

            List<FunctionDeclaration> declarations = new();

            foreach (FunctionReference function in functionManager.Functions)
            {
                if (function == null || string.IsNullOrEmpty(function.MethodName))
                {
                    Logger.Error($"Function '{function?.MethodName}' is null or empty.");
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

                var declaration = new FunctionDeclaration
                {
                    Name = function.MethodName,
                    Description = function.Description,
                    Parameters = schema
                };

                declarations.Add(declaration);

                //GNDebug.Pink($"Function '{function.MethodName}' added to the session.");
            }

            return new FunctionLibrary(new Tool(declarations));
        }

    }
}
