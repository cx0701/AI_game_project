using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public class EditTextWindow : EditorEditWindow<EditTextWindow, string>
    {
        protected override string DrawGUI(string value)
        {
            return EditorGUILayout.TextArea(value, ExEditorStyles.paddedTextField, GUILayout.MinHeight(18f), GUILayout.ExpandHeight(true));
        }
    }
}