namespace Glitch9.EditorKit
{
    /// <summary>
    /// Provides a popup for selecting a string value
    /// </summary>
    public class TextSelectDialog : SelectDialog<TextSelectDialog, string>
    {
        protected override string DrawContent(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            int index = ValueList.IndexOf(value);
            index = ExGUILayout.StringListToolbar(index, ValueList, null, 1);
            if (index < 0) return value;
            if (index >= ValueList.Count) return value;
            return ValueList[index];
        }
    }
}