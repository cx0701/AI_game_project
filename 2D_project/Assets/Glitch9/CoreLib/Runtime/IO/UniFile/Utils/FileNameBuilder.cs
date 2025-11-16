using System;

namespace Glitch9.IO.Files
{
    public enum FileNamingRule
    {
        DateTime,
        GUID,
    }

    public class FileNameBuilder
    {
        public static string GetUniqueFileName(MIMEType fileContentType, FileNamingRule namingRule)
             => GetUniqueFileName(null, fileContentType, namingRule);

        public static string GetUniqueFileName(string tag, MIMEType fileContentType, FileNamingRule namingRule)
        {
            tag ??= fileContentType.ToString();

            string baseName = namingRule switch
            {
                FileNamingRule.DateTime => BuildTimestampedName(tag),
                FileNamingRule.GUID => BuildGuidName(tag),
                _ => throw new ArgumentOutOfRangeException(nameof(namingRule), namingRule, null),
            };

            string extension = MIMETypeUtil.GetFileExtension(fileContentType);
            return $"{baseName}{extension}";
        }

        private static string BuildTimestampedName(string tag) => $"{tag}_{DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}";
        private static string BuildGuidName(string tag) => $"{tag}_{Guid.NewGuid()}";
    }
}