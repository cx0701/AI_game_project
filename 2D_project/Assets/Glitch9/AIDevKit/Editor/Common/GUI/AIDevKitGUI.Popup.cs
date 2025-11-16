using System;
using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.AIDevKit.Editor
{
    internal partial class AIDevKitGUI
    {
        private static readonly ModelPopupGUI _modelPopupGUI = new();
        private static readonly VoicePopupGUI _voicePopupGUI = new();

        // -- Model Return  
        internal static Model LLMPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.LLM(api), label, style);

        internal static Model IMGPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.IMG(api), label, style);

        internal static Model TTSPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.TTS(api), label, style);

        internal static Model STTPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.STT(api), label, style);

        internal static Model EMBPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.EMB(api), label, style);

        internal static Model MODPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.MOD(api), label, style);

        internal static Model RTMPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.RTM(api), label, style);

        internal static Model VIDPopup(Model selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.VID(api), label, style);

        internal static Voice VoicePopup(Voice selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawVoicePopup(selected, VoiceFilter.API(api), label, style);


        // -- SerializedProperty 

        internal static void LLMPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.LLM(api), label, style);

        internal static void IMGPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.IMG(api), label, style);

        internal static void TTSPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.TTS(api), label, style);

        internal static void STTPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.STT(api), label, style);

        internal static void EMBPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.EMB(api), label, style);

        internal static void MODPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.MOD(api), label, style);

        internal static void RTMPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.RTM(api), label, style);

        internal static void VCMPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.VCM(api), label, style);

        internal static void VIDPopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(property, ModelFilter.VID(api), label, style);

        internal static void VoicePopup(SerializedProperty property, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawVoicePopup(property, VoiceFilter.API(api), label, style);

        internal static void VoicePopup(SerializedProperty voiceProperty, SerializedProperty modelProperty, GUIContent label = null, GUIStyle style = null)
        {
            Model model = ModelLibrary.Get(modelProperty.stringValue);
            AIProvider api = model == null ? AIProvider.OpenAI : model.Api;
            DrawVoicePopup(voiceProperty, VoiceFilter.API(api), label, style);
        }

        // -- string ID 
        internal static string LLMPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.LLM(api), label, style);

        internal static string IMGPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.IMG(api), label, style);

        internal static string TTSPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.TTS(api), label, style);

        internal static string STTPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.STT(api), label, style);

        internal static string EMBPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.EMB(api), label, style);

        internal static string MODPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.MOD(api), label, style);

        internal static string RTMPopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawModelPopup(selected, ModelFilter.RTM(api), label, style);

        internal static string VoicePopup(string selected, AIProvider api = AIProvider.All, GUIContent label = null, GUIStyle style = null)
            => DrawVoicePopup(selected, VoiceFilter.API(api), label, style);


        internal static Model DrawModelPopup(Model selected, ModelFilter filter, GUIContent label = null, GUIStyle style = null, float offset = 2f)
        {
            return _modelPopupGUI.Draw(selected, filter, label, style, offset);
        }

        private static Voice DrawVoicePopup(Voice selected, VoiceFilter filter, GUIContent label = null, GUIStyle style = null)
        {
            return _voicePopupGUI.Draw(selected, filter, label, style);
        }

        private static void DrawModelPopup(SerializedProperty property, ModelFilter filter, GUIContent label = null, GUIStyle style = null)
        {
            if (property == null || property.propertyType != SerializedPropertyType.String)
            {
                ExGUILayout.ErrorLabel("Only string properties are supported.");
                return;
            }

            try
            {
                Model selected = property.stringValue;
                Model newModel = _modelPopupGUI.Draw(selected, filter, label, style);

                if (newModel != null && newModel != selected)
                {
                    property.stringValue = newModel.Id;
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            catch (Exception e)
            {
                // fix Type is not supported error
                ExGUILayout.ErrorLabel(e.Message);
            }
        }

        private static void DrawVoicePopup(SerializedProperty property, VoiceFilter filter, GUIContent label = null, GUIStyle style = null)
        {
            if (property == null || property.propertyType != SerializedPropertyType.String)
            {
                ExGUILayout.ErrorLabel("Only string properties are supported.");
                return;
            }

            try
            {
                Voice selected = property.stringValue;
                Voice newVoice = _voicePopupGUI.Draw(selected, filter, label, style);

                if (newVoice != null && newVoice != selected)
                {
                    property.stringValue = newVoice.Id;
                    property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
                }
            }
            catch (Exception e)
            {
                ExGUILayout.ErrorLabel(e.Message);
            }
        }

        private static string DrawModelPopup(string selectedId, ModelFilter filter, GUIContent label = null, GUIStyle style = null)
        {
            Model selected = selectedId;
            selected = _modelPopupGUI.Draw(selected, filter, label, style);
            return selected.Id;
        }

        private static string DrawVoicePopup(string selectedId, VoiceFilter filter, GUIContent label = null, GUIStyle style = null)
        {
            Voice selected = selectedId;
            selected = _voicePopupGUI.Draw(selected, filter, label, style);
            return selected.Id;
        }
    }
}