using System.IO;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public static class ExEditorGUI
    {
        #region Resettable Fields
        public static void ResettableIntField(string label, SerializedProperty property, int defaultValue)
            => ResettableIntField(new GUIContent(label), property, defaultValue);
        public static void ResettableFloatField(string label, SerializedProperty property, float defaultValue)
            => ResettableFloatField(new GUIContent(label), property, defaultValue);
        public static void ResettableTextField(string label, SerializedProperty property, string defaultValue)
            => ResettableTextField(new GUIContent(label), property, defaultValue);

        public static void ResettableIntField(GUIContent label, SerializedProperty property, int defaultValue)
        {
            if (!ExGUIUtility.SerializedPropertyIsValid(property, SerializedPropertyType.Integer)) return;

            GUILayout.BeginHorizontal();
            try
            {
                int newValue = EditorGUILayout.IntField(label, property.intValue);
                if (ExGUILayout.ResetButton()) newValue = defaultValue;
                if (newValue != property.intValue) property.intValue = newValue;
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        public static void ResettableFloatField(GUIContent label, SerializedProperty property, float defaultValue)
        {
            if (!ExGUIUtility.SerializedPropertyIsValid(property, SerializedPropertyType.Float)) return;

            GUILayout.BeginHorizontal();
            try
            {
                float newValue = EditorGUILayout.FloatField(label, property.floatValue);
                if (ExGUILayout.ResetButton()) newValue = defaultValue;
                if (newValue != property.floatValue) property.floatValue = newValue;
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        public static void ResettableTextField(GUIContent label, SerializedProperty property, string defaultValue)
        {
            if (!ExGUIUtility.SerializedPropertyIsValid(property, SerializedPropertyType.String)) return;

            GUILayout.BeginHorizontal();
            try
            {
                string newValue = EditorGUILayout.TextField(label, property.stringValue);
                if (ExGUILayout.ResetButton()) newValue = defaultValue;
                if (newValue != property.stringValue) property.stringValue = newValue;
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        #endregion Resettable Fields 

        #region Path Fields
        public static void PathField(string label, SerializedProperty property, string rootPath, string defaultPath)
            => PathField(new GUIContent(label), property, rootPath, defaultPath);
        public static void PathField(GUIContent label, SerializedProperty property, string rootPath, string defaultPath)
            => DrawPathFieldINTERNAL(label, property, rootPath, defaultPath);
        private static void DrawPathFieldINTERNAL(GUIContent label, SerializedProperty property, string rootPath, string defaultPath)
        {
            GUILayout.BeginHorizontal();

            string value = property.stringValue;

            if (string.IsNullOrEmpty(value))
            {
                value = defaultPath.FixSlashes();
            }

            string displayValue = value.Replace(rootPath, "");
            EditorGUILayout.LabelField(label, new GUIContent(displayValue), EditorStyles.textField, GUILayout.MinWidth(20));

            if (ExGUILayout.ResetButton(EditorStyles.miniButtonMid))
            {
                value = defaultPath.FixSlashes();
            }

            if (ExGUILayout.FolderPanelButton(EditorStyles.miniButtonMid))
            {
                value = ExGUIUtility.OpenFolderPanel(value, rootPath);
            }

            if (ExGUILayout.OpenFolderButton(EditorStyles.miniButtonRight))
            {
                ExGUIUtility.OpenFolder(value, defaultPath);
            }

            GUILayout.EndHorizontal();

            if (value != property.stringValue)
            {
                property.stringValue = value.FixSlashes();
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        #endregion Path Fields 


        #region Toggle Selector (with multiple bool values) 

        public static void ToggleSelector(string label, params SerializedProperty[] properties)
            => ToggleSelector(new GUIContent(label), properties);

        public static void ToggleSelector(GUIContent label, params SerializedProperty[] properties)
        {
            if (properties == null || properties.Length == 0) return;

            int count = properties.Length;
            float width = ExGUIUtility.DivideViewWidthWithoutLabel(count);

            GUILayout.BeginHorizontal();
            try
            {
                EditorGUILayout.PrefixLabel(label);

                int oldSelected = -1;
                int newSelected = -1;

                for (int i = 0; i < count; i++)
                {
                    bool value = properties[i].boolValue;
                    if (value) oldSelected = i;

                    GUIStyle style = ExGUIUtility.GetToggleStyle(i, count);

                    bool newValue = GUILayout.Toggle(value, properties[i].displayName, style, GUILayout.Width(width));
                    if (newValue) newSelected = i;

                    if (oldSelected != newSelected)
                    {
                        for (int j = 0; j < count; j++)
                        {
                            properties[j].boolValue = j == newSelected;
                            properties[j].serializedObject.ApplyModifiedProperties();
                        }
                        break;
                    }
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }


        #endregion Toggle Selector (with multiple bool values)


        #region ExpandableTextField

        public static void ExpandableTextField(string label, SerializedProperty property, GUIStyle style = null, params GUILayoutOption[] options)
            => ExpandableTextField(new GUIContent(label), property, style, options);
        public static void ExpandableTextField(GUIContent label, SerializedProperty property, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            try
            {
                string value = property.stringValue;
                value = EditorGUILayout.TextField(label, value, style ?? GUI.skin.textField, options);
                if (GUILayout.Button(EditorIcons.Pick, ExEditorStyles.miniButton))
                {
                    EditTextWindow.Show(label.text, value, (edited) =>
                    {
                        property.stringValue = edited;
                        property.serializedObject.ApplyModifiedProperties();
                    }, new Vector2(1200, 1200));
                }
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        #endregion ExpandableTextField
    }
}