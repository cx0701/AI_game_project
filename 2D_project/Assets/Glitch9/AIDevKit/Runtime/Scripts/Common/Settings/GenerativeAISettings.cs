using UnityEngine;

namespace Glitch9.AIDevKit.Google
{
    public enum PaymentMode
    {
        [InspectorName("Free of charge")] Free,
        [InspectorName("Pay-as-you-go")] Paid
    }

    [CreateAssetMenu(fileName = nameof(GenerativeAISettings), menuName = AIDevKitConfig.kGoogleSettings, order = AIDevKitConfig.kGoogleSettingsOrder)]
    public class GenerativeAISettings : AIClientSettings<GenerativeAISettings>
    {
        [SerializeField] private PaymentMode paymentMode = PaymentMode.Free;
        [SerializeField] private string projectId;

        // default models
        [SerializeField] private string defaultLLM = AIDevKitConfig.kDefault_Google_LLM;
        [SerializeField] private string defaultEMB = AIDevKitConfig.kDefault_Google_EMB;
        [SerializeField] private string defaultIMG = AIDevKitConfig.kDefault_Google_IMG;
        [SerializeField] private string defaultVID = AIDevKitConfig.kDefault_Google_VID;

        public static PaymentMode PaymentMode => Instance.paymentMode;
        public static string ProjectId => Instance.projectId;

        public static string DefaultLLM => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultLLM, AIDevKitConfig.kDefault_OpenAI_LLM);
        public static string DefaultEMB => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultEMB, AIDevKitConfig.kDefault_OpenAI_EMB);
        public static string DefaultIMG => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultIMG, AIDevKitConfig.kDefault_OpenAI_IMG);
        public static string DefaultVID => AIDevKitUtils.ReturnDefaultIfEmpty(Instance.defaultVID, AIDevKitConfig.kDefault_Google_VID);

        public static bool IsDefaultModel(string id, ModelCapability cap)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            if (cap.HasFlag(ModelCapability.TextGeneration) && id == DefaultLLM) return true;
            if (cap.HasFlag(ModelCapability.ImageGeneration) && id == DefaultIMG) return true;
            if (cap.HasFlag(ModelCapability.TextEmbedding) && id == DefaultEMB) return true;
            if (cap.HasFlag(ModelCapability.VideoGeneration) && id == DefaultVID) return true;

            return false;
        }

    }
}