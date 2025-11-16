using UnityEngine;

namespace Glitch9.EditorKit
{
    public class HideIfFalseAttribute : PropertyAttribute
    {
        public string ConditionalSourceField = "";

        public HideIfFalseAttribute(string booleanFieldName)
        {
            this.ConditionalSourceField = booleanFieldName;
        }
    }
}