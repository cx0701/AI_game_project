using System;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public class GUIButtonEntry
    {
        public GUIContent Label { get; set; }
        public Action OnClick { get; set; }
        public Func<bool> IsEnabled { get; set; }

        public GUIButtonEntry() { }

        public GUIButtonEntry(GUIContent label, Action onClick, Func<bool> isEnabled = null)
        {
            Label = label ?? throw new ArgumentNullException(nameof(label));
            OnClick = onClick ?? throw new ArgumentNullException(nameof(onClick));
            IsEnabled = isEnabled ?? (() => true);
        }

        public GUIButtonEntry(string label, Action onClick, Func<bool> isEnabled = null)
        {
            Label = new GUIContent(label ?? throw new ArgumentNullException(nameof(label)));
            OnClick = onClick ?? throw new ArgumentNullException(nameof(onClick));
            IsEnabled = isEnabled ?? (() => true);
        }
    }
}