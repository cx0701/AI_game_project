using System.IO;
using Glitch9.IO.RESTApi;
using Glitch9.ScriptableObjects;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    [CreateAssetMenu(fileName = nameof(AIDevKitSettings), menuName = AIDevKitConfig.kAIDevKitSettings, order = AIDevKitConfig.kAIDevKitSettingsOrder)]
    public class AIDevKitSettings : ScriptableResource<AIDevKitSettings>
    {
        // Default Models
        [SerializeField] private string defaultLLM = AIDevKitConfig.kDefault_OpenAI_LLM;
        [SerializeField] private string defaultIMG = AIDevKitConfig.kDefault_OpenAI_IMG;
        [SerializeField] private string defaultTTS = AIDevKitConfig.kDefault_OpenAI_TTS;
        [SerializeField] private string defaultSTT = AIDevKitConfig.kDefault_OpenAI_STT;
        [SerializeField] private string defaultEMB = AIDevKitConfig.kDefault_OpenAI_EMB;
        [SerializeField] private string defaultMOD = AIDevKitConfig.kDefault_OpenAI_MOD;

        // Path Settings
        [SerializeField] private string runtimeOutputPath;
        [SerializeField] private string editorOutputPath;
        [SerializeField] private bool checkForModelUpdatesOnStartup = true;
        [SerializeField] private bool promptHistoryOnRuntime = true;

        // Script Generator (CodeGen) Settings
        [SerializeField] private bool componentGenerator = AIDevKitConfig.kComponentGenerator;
        [SerializeField] private bool scriptDebugger = AIDevKitConfig.kScriptDebugger;

        // Log Levels
        [SerializeField] private RESTLogLevel logLevel = RESTLogLevel.RequestEndpoint | RESTLogLevel.ResponseBody;
        [SerializeField] private int requestTimeout = AIDevKitConfig.kDefaultTimeoutInSeconds;

        // Project Context
        [SerializeField] private ProjectContext projectContext = new();

        // Static GetSetters ------------------------------------------------------------------------------------------------

        /// <summary>
        /// Default path for runtime downloads, used in dynamically updating or adding resources at runtime.
        /// </summary>
        public static string RuntimeOutputPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Instance.runtimeOutputPath)
                    || !Instance.runtimeOutputPath.Contains(Application.persistentDataPath))
                {
                    Instance.runtimeOutputPath = Path.Combine(Application.persistentDataPath, "Generated");
                }
                return Instance.runtimeOutputPath;
            }
        }

        /// <summary>
        /// Default path where editor-specific assets are downloaded, commonly used in Unity projects.
        /// </summary>
        public static string EditorOutputPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Instance.editorOutputPath)
                    || !Instance.editorOutputPath.Contains(Application.dataPath))
                {
                    // Use Application.dataPath to ensure the path is within the Unity project directory
                    // and not outside of it, which is important for Unity's asset management.
                    Instance.editorOutputPath = Path.Combine(Application.dataPath, "Generated");
                }
                return Instance.editorOutputPath;
            }
        }

        /// <summary>
        /// Enables or disables the component generator, which facilitates the creation of new Unity components from specified prompts.
        /// </summary>
        internal static bool EnableComponentGenerator => Instance.componentGenerator;
        internal static bool EnableScriptDebugger => Instance.scriptDebugger;
        public static RESTLogLevel LogLevel => Instance.logLevel;
        public static int RequestTimeout => Instance.requestTimeout;
        public static ProjectContext ProjectContext => Instance.projectContext;
        public static bool CheckForModelUpdatesOnStartup => Instance.checkForModelUpdatesOnStartup;
        public static string DefaultLLM => Instance.defaultLLM;
        public static string DefaultIMG => Instance.defaultIMG;
        public static string DefaultTTS => Instance.defaultTTS;
        public static string DefaultSTT => Instance.defaultSTT;
        public static string DefaultEMB => Instance.defaultEMB;
        public static string DefaultMOD => Instance.defaultMOD;
        public static bool PromptHistoryOnRuntime => Instance.promptHistoryOnRuntime;
    }
}