using System;

namespace Glitch9.AIDevKit
{
    public class EmptyResponseException : Exception
    {
        public EmptyResponseException(Model model) : base($"Model {model.Id} returned null or empty result.") { }
        public EmptyResponseException(int taskType) : base($"{GENTaskType.GetName(taskType)} task returned null or empty result.") { }
    }

    public class DeprecatedModelException : Exception
    {
        public DeprecatedModelException(Model model) : base($"Model {model.Id} is deprecated. Please use a different model.")
        {
        }
    }

    public class UnsupportedCapabilityException : Exception
    {
        public UnsupportedCapabilityException(Model model, ModelCapability cap) : base($"Model {model.Id} does not support {cap} output. Please use a different model.")
        {
        }
    }

    /// <summary>
    /// Thrown when a GenAI provider does not support a specific feature.
    /// </summary>
    public class UnsupportedGENTaskException : NotSupportedException
    {
        /// <summary>
        /// The name of the provider that does not support the feature.
        /// </summary>
        public AIProvider Api { get; }

        /// <summary>
        /// The name of the unsupported feature.
        /// </summary>
        public int TaskType { get; }

        public UnsupportedGENTaskException(AIProvider api, int taskType)
                : base($"{api} does not support {GENTaskType.GetName(taskType)}")
        {
            Api = api;
            TaskType = taskType;
        }
    }
}