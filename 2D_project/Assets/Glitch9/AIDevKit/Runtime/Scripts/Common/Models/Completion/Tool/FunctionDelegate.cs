using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// Represents a common interface for all function delegates.
    /// </summary>
    public interface IFunctionDelegate
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        string FunctionName { get; set; }

        /// <summary>
        /// Executes the function with the provided argument.
        /// </summary>
        /// <param name="argument">The serialized argument for the function.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the serialized function result.</returns>
        UniTask<IResult> Invoke(string argument);
    }

    /// <summary>
    /// Provides an abstract base class for implementing function delegates with specific argument and result types.
    /// </summary>
    /// <typeparam name="TArgument">The type of the function argument.</typeparam>
    /// <typeparam name="TResult">The type of the function result.</typeparam>
    public abstract class FunctionDelegate<TArgument, TResult> : IFunctionDelegate
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Gets or sets the default result to be used in case of failure.
        /// </summary>
        public TResult DefaultResult { get; set; }

        /// <summary>
        /// Executes the function with the provided argument.
        /// </summary>
        /// <param name="argument">The serialized argument for the function.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the serialized function result.</returns>
        public async UniTask<IResult> Invoke(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                return Result<string>.Fail("Argument is null or empty.");
            }

            try
            {
                // Deserialize the argument
                TArgument deserializedArgument = JsonConvert.DeserializeObject<TArgument>(argument);

                // Execute the function and get the result
                TResult result = await Invoke(deserializedArgument);

                // Serialize the result and return as success
                return Result<string>.Success(JsonConvert.SerializeObject(result));
            }
            catch (Exception e)
            {
                // Handle any exceptions and return a failure result with the default result serialized
                return Result<string>.Fail(JsonConvert.SerializeObject(DefaultResult), $"Failed to handle argument: {e.Message}");
            }
        }


        /// <summary>
        /// Executes the function with the specified argument.
        /// </summary>
        /// <param name="argument">The deserialized argument for the function.</param>
        /// <returns>A task representing the asynchronous operation, with a result containing the function result.</returns>
        public abstract UniTask<TResult> Invoke(TArgument argument);
    }
}