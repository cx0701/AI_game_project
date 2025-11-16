using System.Text.RegularExpressions;

namespace Glitch9.AIDevKit
{
    /// <summary>
    /// AIDevKit 내부에서 사용하는 다목적 유틸리티 클래스
    /// </summary>
    internal class AIDevKitUtils
    {
        internal static string RemoveSlashPrefixFromID(string id)
        {
            int lastSlashIndex = id.LastIndexOf('/');
            if (lastSlashIndex >= 0 && lastSlashIndex < id.Length - 1)
            {
                return id.Substring(lastSlashIndex + 1);
            }

            return id;
        }

        internal static string ExtractDateString(string input)
        {
            var match = Regex.Match(input, @"\b\d{4}-\d{2}-\d{2}\b");
            return match.Success ? match.Value : null;
        }

        internal static string ReturnDefaultIfEmpty(string value, string defaultValue)
        {
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}