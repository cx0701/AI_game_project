using Glitch9.EditorKit;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal static class GUIContents
    {
        internal static readonly GUIContent ApiKeySectionTitle = new(
            "API Access & Configuration",
            "An API key is a bridge between your applications and OpenAI's powerful suite of AI technologies, allowing you to seamlessly integrate advanced AI functionalities into your projects.");

        internal static readonly GUIContent UsefulLinksSectionTitle = new(
            "Useful Links",
            "Links to the official documentation and support channels for the AIDevKit.");

        internal static readonly GUIContent DefaultModelsSectionTitle = new(
            "Default Models",
            "The default models to use for the API. This is used when no model is specified in the request.");

        internal static readonly GUIContent DefaultModelsAndVoicesSectionTitle = new(
            "Default Models & Voices",
            "Default models and voices to use for the API. This is used when no model or voice is specified in the request.");

        internal static readonly GUIContent ApiEncryptionLabel = new(
            "API Key Encryption (AES)",
            "Encrypt the API key to prevent unauthorized access to the key. The encrypted key will be stored in the settings file.");

        internal static readonly GUIContent ApiOrganizationLabel = new(
            "Organization (Optional)",
            "This is used to specify the unique identifier of the organization under which the API call is being made. " +
            "This is particularly relevant for users who are part of an organization that has its own OpenAI account, " +
            "separate from personal accounts. By including the organization ID in the API request header, " +
            "it ensures that the usage and billing for that call are correctly attributed to the organization's account, " +
            "rather than an individual's account.");

        internal static readonly GUIContent ApiProjectIdLabel = new(
            "Project ID (Optional)",
            "This is used to specify the unique identifier of the project under which the API call is being made. " +
            "This is particularly relevant for users who are part of a project that has its own OpenAI account, " +
            "separate from personal accounts. By including the project ID in the API request header, " +
            "it ensures that the usage and billing for that call are correctly attributed to the project's account, " +
            "rather than an individual's account.");

        internal static readonly GUIContent MaxTokens = new(
            "Max Prompt Tokens",
            "The maximum number of tokens allowed for the prompt in a request. This setting helps prevent the prompt from exceeding the model's token limit.");

        internal static readonly GUIContent UpdateModelsOnStartup = new(
            "Auto Update Models On Startup",
            "If enabled, the plugin will automatically update the models on startup. Only available in the Pro version.");

        internal static readonly GUIContent SystemInstruction = new("System Instruction", "The system instruction to guide the model's behavior.");
        internal static readonly GUIContent StartingMessage = new("Starting Message", "The initial message from the assistant.");
        internal static readonly GUIContent Temperature = new("Temperature", "The temperature controls the randomness of the output. Higher values (e.g., 1) make the output more random, while lower values (e.g., 0.1) make it more focused and deterministic.");
        internal static readonly GUIContent TopP = new("Top P", "An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass.");
        internal static readonly GUIContent FrequencyPenalty = new("Frequency Penalty", "Penalty for how often new tokens should be different from existing ones.");
        internal static readonly GUIContent MaxInputTokens = new("Max Input Tokens", "The maximum number of tokens in the input. -1 means no limit.");
        internal static readonly GUIContent MaxOutputTokens = new("Max Output Tokens", "The maximum number of tokens to generate. -1 means no limit.");
        internal static readonly GUIContent Stream = new("Stream", "Whether to stream the response.");
        internal static readonly GUIContent VoiceSpeed = new("Voice Speed", "The speed of the generated audio.");
        internal static readonly GUIContent VoiceSeed = new("Voice Seed", "The seed for the voice generation.");
        internal static readonly GUIContent RecordingsFolder = new("Recordings Folder", "The folder where the recording files are stored.");
        internal static readonly GUIContent VoicesFolder = new("Voices Folder", "The folder where the voice files are stored.");
        internal static readonly GUIContent RecordingFrequency = new("Recording Frequency", "The frequency of the recording.");
        internal static readonly GUIContent MaxRecordingDuration = new("Max Recording Duration", "The maximum duration of the recording.");
        internal static readonly GUIContent ImageSize = new("Image Size", "The size of the image to be generated.");
        internal static readonly GUIContent ImageQuality = new("Image Quality", "The quality of the image to be generated.");
        internal static readonly GUIContent ImageStyle = new("Image Style", "The style of the image to be generated.");


        // Added 2025.04.17        
        internal static readonly GUIContent ModelSelector = new("Model", "Select the model to use to generate the content.");
        internal static readonly GUIContent VoiceSelector = new("Voice Actor", "Select the voice to use to generate the content.");
        internal static readonly GUIContent IsTileable = new("Tileable", "If checked, AI will try to generate a tileable texture, but it may not be perfect.");
        internal static readonly GUIContent UseProjectContext = new("Use Project Context", "Apply the project context set in AIDevKit settings to the prompt.");
        internal static readonly GUIContent RemoveBackground = new("Remove Background", "If checked, AI will try to remove the background from the generated image.");


        // Added 2025.05.01
        internal static readonly GUIContent DefaultLLM = new("Text Generation Model", "The default model to use for text generation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultIMG = new("Image Generation Model", "The default model to use for image generation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultTTS = new("Text-to-Speech Model", "The default model to use for text to speech. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultSTT = new("Speech-to-Text Model", "The default model to use for speech to text. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultEMB = new("Text-Embedding Model", "The default model to use for embeddings generation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultMOD = new("Moderation Model", "The default model to use for moderation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultVCM = new("Voice Changer Model", "The default model to use for voice synthesis. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultASS = new("Assistants API Model", "The default GPT model to use for the Assistants API.");
        internal static readonly GUIContent DefaultRTM = new("Realtime API Model", "The default model to use for real-time generation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultVID = new("Video Generation Model", "The default model to use for video generation. This is used when no model is specified in the request.");
        internal static readonly GUIContent DefaultVoice = new("Voice Actor", "The default voice to use for text to speech or voice changer. This is used when no voice is specified in the request.");

        internal static readonly GUIContent AddToLibrary = new(EditorIcons.Import, "Add to Library");
        internal static readonly GUIContent RemoveFromLibrary = new(EditorIcons.Delete, "Remove from Library");
        internal static readonly GUIContent OnlineDocument = new("Online Document", "Open the AIDevKit online documentation in your default browser.");
        internal static readonly GUIContent JoinDiscord = new("Join Discord", "Join the AIDevKit Discord server for support and community discussions.");
        internal static readonly GUIContent Preferences = new("Preferences", "Open the AIDevKit preferences window.");
        internal static readonly GUIContent ApiDefaultModel = new("Default Model", "The default model to use for the API. This is used when no model is specified in the request.");


        internal static readonly GUIContent PersonGeneration = new("Person Generation", "Allow the model to generate images (or videos) of people.");
        internal static readonly GUIContent AspectRatio = new("Aspect Ratio", "The aspect ratio of the generated image (or video). Supported values are \"1:1\", \"3:4\", \"4:3\", \"9:16\", and \"16:9\". The default is \"1:1\".");
        internal static readonly GUIContent SaveHistory = new("Save Request History", "If checked, all chat requests will be saved in 'PromptHistory.asset' file.");

        internal static readonly GUIContent RecordingClipTempPath = new("Recording Folder", "The path to the temporary folder where the recording clip will be saved. This is used to store the recording clip before it is processed.");
        internal static readonly GUIContent FunctionManager = new("Function Manager (Optional)", "The function manager for handling function calls.");
        internal static readonly GUIContent SaveOutputs = new("Save Outputs", "If checked, the output files will be saved to the specified output path. This is used to store the output files after they are processed.");
        internal static readonly GUIContent OutputPath = new("Output Path", "The path to the folder where the output files will be saved. This is used to store the output files after they are processed.");

        internal static readonly GUIContent ReasoningEffort = new("Reasoning Effort", "The effort level for reasoning. Higher values indicate more effort.");
    }
}