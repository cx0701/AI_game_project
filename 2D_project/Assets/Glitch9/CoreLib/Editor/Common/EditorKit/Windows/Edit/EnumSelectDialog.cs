using System;
using UnityEditor;

namespace Glitch9.EditorKit
{
    /// <summary>
    /// Provides a popup for selecting an enum value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumSelectDialog<T> : SelectDialog<EnumSelectDialog<T>, T> where T : Enum
    {
        protected override T DrawContent(T value)
        {
            T newValue = (T)EditorGUILayout.EnumPopup(value);
            return newValue;
        }
    }
}