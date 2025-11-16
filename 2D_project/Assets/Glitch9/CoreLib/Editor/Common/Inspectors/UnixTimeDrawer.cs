using Glitch9.EditorKit;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorTools
{
    [CustomPropertyDrawer(typeof(UnixTime))]
    public class UnixTimeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty unixTimeAsLong = property.FindPropertyRelative("_value");
            UnixTime unixTime = new(unixTimeAsLong.longValue);


            float singleLineHeight = EditorGUIUtility.singleLineHeight;

            Rect currentPosition = EditorGUI.PrefixLabel(position, label);
            position.height = singleLineHeight;

            // Indent label
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            UnixTime newUnixTime = ExGUI.UnixTimeField(currentPosition, null, unixTime, true, true, true, true, true, true);

            if (newUnixTime != unixTime)
            {
                unixTimeAsLong.longValue = newUnixTime.Value;
            }

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
}