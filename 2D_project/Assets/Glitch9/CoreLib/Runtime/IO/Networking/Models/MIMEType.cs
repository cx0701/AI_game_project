using Glitch9.IO.RESTApi;
using System.IO;

namespace Glitch9.IO.Files
{
    public enum MIMEType
    {
        [ApiEnum("")]
        Unknown,

        /// <summary>
        /// Represents JSON formatted text (.json).
        /// </summary>
        [ApiEnum("application/json")]
        Json,

        /// <summary>
        /// Represents JSON Lines formatted text (.jsonl).
        /// </summary>
        [ApiEnum("application/jsonl")]
        Jsonl,

        /// <summary>
        /// Represents XML formatted text (.xml).
        /// </summary>
        [ApiEnum("application/xml")]
        Xml,

        /// <summary>
        /// Represents form data encoded as URL parameters.
        /// </summary>
        [ApiEnum("application/x-www-form-urlencoded")]
        WWWForm,

        /// <summary>
        /// Represents multipart form data, allowing multiple values and file uploads.
        /// </summary>
        [ApiEnum("multipart/form-data")]
        MultipartForm,

        /// <summary>
        /// Represents plain text data (.txt).
        /// </summary>
        [ApiEnum("text/plain")]
        PlainText,

        /// <summary>
        /// Represents HTML formatted text (.html).
        /// </summary>
        [ApiEnum("text/html")]
        HTML,

        /// <summary>
        /// Represents comma-separated values (.csv).
        /// </summary>
        [ApiEnum("text/csv")]
        CSV,

        /// <summary>
        /// Represents raw binary data (no specific file extension).
        /// </summary>
        [ApiEnum("application/octet-stream")]
        OctetStream,

        /// <summary>
        /// Represents Adobe Portable Document Format (.pdf).
        /// </summary>
        [ApiEnum("application/pdf")]
        PDF,

        /// <summary>
        /// Represents MPEG video format (.mpeg).
        /// </summary>
        [ApiEnum("video/mpeg")]
        MPEG,

        /// <summary>
        /// Represents Waveform Audio File Format (.wav).
        /// </summary>
        [ApiEnum("audio/wav")]
        WAV,

        [ApiEnum("audio/flac")]
        FLAC,

        [ApiEnum("audio/ogg")]
        OGG,

        [ApiEnum("audio/aac")]
        AAC,

        [ApiEnum("audio/opus")]
        Opus,

        [ApiEnum("audio/pcm")]
        PCM,

        [ApiEnum("audio/ulaw")]
        uLaw,

        [ApiEnum("audio/alaw")]
        aLaw,

        [ApiEnum("audio/basic")]
        MuLaw,

        /// <summary>
        /// Represents JPEG image format (.jpeg).
        /// </summary>
        [ApiEnum("image/jpeg")]
        JPEG,

        /// <summary>
        /// Represents Portable Network Graphics format (.png).
        /// </summary>
        [ApiEnum("image/png")]
        PNG,

        /// <summary>
        /// Represents Graphics Interchange Format (.gif).
        /// </summary>
        [ApiEnum("image/gif")]
        GIF,

        /// <summary>
        /// Represents MPEG-4 video format (.mp4).
        /// </summary>
        [ApiEnum("video/mp4")]
        MP4,

        /// <summary>
        /// Represents Audio Video Interleave format (.avi).
        /// </summary>
        [ApiEnum("video/x-msvideo")]
        AVI,

        /// <summary>
        /// Represents C source code (.c).
        /// </summary>
        [ApiEnum("text/x-c")]
        C,

        /// <summary>
        /// Represents C# source code (.cs).
        /// </summary>
        [ApiEnum("text/x-csharp")]
        CSharp,

        /// <summary>
        /// Represents C++ source code (.cpp).
        /// </summary>
        [ApiEnum("text/x-c++src")]
        CPP,

        /// <summary>
        /// Represents a Microsoft Word binary document (.doc).
        /// </summary>
        [ApiEnum("application/msword")]
        MsDoc,

        /// <summary>
        /// Represents a Microsoft Word document in XML format (.docx).
        /// </summary>
        [ApiEnum("application/vnd.openxmlformats-officedocument.wordprocessingml.document")]
        MsDocXML,

        /// <summary>
        /// Represents a Microsoft PowerPoint Presentation (.pptx).
        /// </summary>
        [ApiEnum("application/vnd.openxmlformats-officedocument.presentationml.presentation")]
        MsPowerPointPresentation,

        /// <summary>
        /// Represents a Microsoft Excel Spreadsheet in XML format (.xlsx).
        /// </summary>
        [ApiEnum("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")]
        MsExcelXML,

        /// <summary>
        /// Represents Java source code (.java).
        /// </summary>
        [ApiEnum("text/x-java-source")]
        Java,

        /// <summary>
        /// Represents Markdown formatted text (.md).
        /// </summary>
        [ApiEnum("text/markdown")]
        Markdown,

        /// <summary>
        /// Represents PHP source code (PHP: Hypertext Preprocessor) (.php).
        /// </summary>
        [ApiEnum("application/x-httpd-php")]
        HypertextPreprocessor,

        /// <summary>
        /// Represents Python source code (.py).
        /// </summary>
        [ApiEnum("text/x-python")]
        Python,

        /// <summary>
        /// Represents Python script files (.py).
        /// </summary>
        [ApiEnum("text/x-python-script")]
        PythonScript,

        /// <summary>
        /// Represents Ruby source code (.rb).
        /// </summary>
        [ApiEnum("text/x-ruby")]
        Ruby,

        /// <summary>
        /// Represents TeX typesetting source (.tex).
        /// </summary>
        [ApiEnum("application/x-tex")]
        TeX,

        /// <summary>
        /// Represents Cascading Style Sheets (.css).
        /// </summary>
        [ApiEnum("text/css")]
        CascadingStyleSheets,

        /// <summary>
        /// Represents JavaScript code (.js).
        /// </summary>
        [ApiEnum("application/javascript")]
        JavaScript,

        /// <summary>
        /// Represents Shell script (.sh).
        /// </summary>
        [ApiEnum("application/x-sh")]
        ShellScript,

        /// <summary>
        /// Represents TypeScript source code (.ts).
        /// </summary>
        [ApiEnum("application/typescript")]
        TypeScript,

        /// <summary>
        /// Represents a Tape Archive file, used for file archiving (.tar).
        /// </summary>
        [ApiEnum("application/x-tar")]
        TapeArchive,

        /// <summary>
        /// Represents a ZIP archive file, used for compression and archiving (.zip).
        /// </summary>
        [ApiEnum("application/zip")]
        ZIP,
    }

    public static class MIMETypeUtil
    {
        public static bool IsImage(this MIMEType mimeType)
        {
            return mimeType switch
            {
                MIMEType.JPEG => true,
                MIMEType.PNG => true,
                MIMEType.GIF => true,
                _ => false
            };
        }

        public static bool IsAudio(this MIMEType mimeType)
        {
            return mimeType switch
            {
                MIMEType.MPEG => true,
                MIMEType.WAV => true,
                MIMEType.AAC => true,
                MIMEType.OGG => true,
                MIMEType.FLAC => true,
                MIMEType.Opus => true,
                MIMEType.PCM => true,
                MIMEType.uLaw => true,
                MIMEType.aLaw => true,
                _ => false
            };
        }

        public static RESTHeader GetHeader(this MIMEType mimeType)
        {
            return new RESTHeader("Content-Type", mimeType.ToApiValue());
        }

        public static MIMEType Parse(string contentTypeAsString)
        {
            return contentTypeAsString switch
            {
                "application/json" => MIMEType.Json,
                "application/jsonl" => MIMEType.Jsonl,
                "application/xml" => MIMEType.Xml,
                "application/x-www-form-urlencoded" => MIMEType.WWWForm,
                "multipart/form-data" => MIMEType.MultipartForm,
                "text/plain" => MIMEType.PlainText,
                "text/html" => MIMEType.HTML,
                "text/csv" => MIMEType.CSV,
                "text/x-c" => MIMEType.C,
                "text/x-csharp" => MIMEType.CSharp,
                "text/x-c++" => MIMEType.CPP,
                "application/msword" => MIMEType.MsDoc,
                "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => MIMEType.MsDocXML,
                "application/vnd.openxmlformats-officedocument.presentationml.presentation" => MIMEType.MsPowerPointPresentation,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => MIMEType.MsExcelXML,
                "text/x-java" => MIMEType.Java,
                "text/markdown" => MIMEType.Markdown,
                "text/x-php" => MIMEType.HypertextPreprocessor,
                "text/x-python" => MIMEType.Python,
                "text/x-script.python" => MIMEType.PythonScript,
                "text/x-ruby" => MIMEType.Ruby,
                "text/x-tex" => MIMEType.TeX,
                "text/css" => MIMEType.CascadingStyleSheets,
                "text/javascript" => MIMEType.JavaScript,
                "application/x-sh" => MIMEType.ShellScript,
                "application/typescript" => MIMEType.TypeScript,
                "application/octet-stream" => MIMEType.OctetStream,
                "application/pdf" => MIMEType.PDF,
                "audio/mpeg" => MIMEType.MPEG,
                "audio/wav" => MIMEType.WAV,
                "audio/flac" => MIMEType.FLAC,
                "audio/ogg" => MIMEType.OGG,
                "audio/aac" => MIMEType.AAC,
                "audio/opus" => MIMEType.Opus,
                "audio/ulaw" => MIMEType.uLaw,
                "audio/alaw" => MIMEType.aLaw,
                "audio/mulaw" => MIMEType.MuLaw,
                "audio/pcm" => MIMEType.PCM,
                "image/jpeg" => MIMEType.JPEG,
                "image/png" => MIMEType.PNG,
                "image/gif" => MIMEType.GIF,
                "video/mp4" => MIMEType.MP4,
                "video/x-msvideo" => MIMEType.AVI,
                "application/x-tar" => MIMEType.TapeArchive,
                "application/zip" => MIMEType.ZIP,
                _ => MIMEType.Json
            };
        }

        public static MIMEType ParseFromPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return MIMEType.Unknown;

            string extension = Path.GetExtension(filePath).ToLower();

            return extension switch
            {
                ".json" => MIMEType.Json,
                ".jsonl" => MIMEType.Jsonl,
                ".xml" => MIMEType.Xml,
                ".txt" => MIMEType.PlainText,
                ".html" => MIMEType.HTML,
                ".csv" => MIMEType.CSV,
                ".c" => MIMEType.C,
                ".cs" => MIMEType.CSharp,
                ".cpp" => MIMEType.CPP,
                ".doc" => MIMEType.MsDoc,
                ".docx" => MIMEType.MsDocXML,
                ".ppt" => MIMEType.MsPowerPointPresentation,
                ".pptx" => MIMEType.MsPowerPointPresentation,
                ".xls" => MIMEType.MsExcelXML,
                ".xlsx" => MIMEType.MsExcelXML,
                ".java" => MIMEType.Java,
                ".md" => MIMEType.Markdown,
                ".php" => MIMEType.HypertextPreprocessor,
                ".py" => MIMEType.Python,
                ".rb" => MIMEType.Ruby,
                ".tex" => MIMEType.TeX,
                ".css" => MIMEType.CascadingStyleSheets,
                ".js" => MIMEType.JavaScript,
                ".sh" => MIMEType.ShellScript,
                ".ts" => MIMEType.TypeScript,
                ".bin" => MIMEType.OctetStream,
                ".pdf" => MIMEType.PDF,
                ".mp3" => MIMEType.MPEG,
                ".wav" => MIMEType.WAV,
                ".flac" => MIMEType.FLAC,
                ".ogg" => MIMEType.OGG,
                ".aac" => MIMEType.AAC,
                ".opus" => MIMEType.Opus,
                ".pcm" => MIMEType.PCM,
                ".au" => MIMEType.MuLaw,
                ".ulaw" => MIMEType.uLaw,
                ".alaw" => MIMEType.aLaw,
                ".jpeg" => MIMEType.JPEG,
                ".jpg" => MIMEType.JPEG,
                ".png" => MIMEType.PNG,
                ".gif" => MIMEType.GIF,
                ".mp4" => MIMEType.MP4,
                ".avi" => MIMEType.AVI,
                ".tar" => MIMEType.TapeArchive,
                ".zip" => MIMEType.ZIP,
                _ => MIMEType.Unknown
            };
        }

        public static string GetFileExtension(this MIMEType contentType)
        {
            return contentType switch
            {
                MIMEType.Json => ".json",
                MIMEType.Jsonl => ".jsonl",
                MIMEType.Xml => ".xml",
                MIMEType.WWWForm => ".form",
                MIMEType.MultipartForm => ".form",
                MIMEType.PlainText => ".txt",
                MIMEType.HTML => ".html",
                MIMEType.CSV => ".csv",
                MIMEType.C => ".c",
                MIMEType.CSharp => ".cs",
                MIMEType.CPP => ".cpp",
                MIMEType.MsDoc => ".doc",
                MIMEType.MsDocXML => ".docx",
                MIMEType.MsPowerPointPresentation => ".ppt",
                MIMEType.MsExcelXML => ".xls",
                MIMEType.Java => ".java",
                MIMEType.Markdown => ".md",
                MIMEType.HypertextPreprocessor => ".php",
                MIMEType.Python => ".py",
                MIMEType.Ruby => ".rb",
                MIMEType.TeX => ".tex",
                MIMEType.CascadingStyleSheets => ".css",
                MIMEType.JavaScript => ".js",
                MIMEType.ShellScript => ".sh",
                MIMEType.TypeScript => ".ts",
                MIMEType.OctetStream => ".bin",
                MIMEType.PDF => ".pdf",
                MIMEType.MPEG => ".mp3",
                MIMEType.WAV => ".wav",
                MIMEType.FLAC => ".flac",
                MIMEType.OGG => ".ogg",
                MIMEType.AAC => ".aac",
                MIMEType.Opus => ".opus",
                MIMEType.PCM => ".pcm",
                MIMEType.uLaw => ".ulaw",
                MIMEType.aLaw => ".alaw",
                MIMEType.MuLaw => ".au",
                MIMEType.JPEG => ".jpeg",
                MIMEType.PNG => ".png",
                MIMEType.GIF => ".gif",
                MIMEType.MP4 => ".mp4",
                MIMEType.AVI => ".avi",
                MIMEType.TapeArchive => ".tar",
                MIMEType.ZIP => ".zip",
                _ => null,
            };
        }
    }
}