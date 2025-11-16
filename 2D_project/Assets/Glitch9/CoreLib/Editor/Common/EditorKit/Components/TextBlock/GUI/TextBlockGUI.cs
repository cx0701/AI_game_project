using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    internal static class TextBlockGUI
    {
        private static readonly GUIStyleCache _cache = new();

        internal static GUIStyle PlainText => _cache.Get(nameof(PlainText), new GUIStyle(GUI.skin.label)
        {
            wordWrap = true,
            richText = true,
            stretchWidth = true,
            stretchHeight = true,
            alignment = TextAnchor.UpperLeft,
        });

        internal static GUIStyle UList => _cache.Get(nameof(UList), new GUIStyle(PlainText)
        {
            margin = new RectOffset(0, 0, 0, 20),
        });

        internal static GUIStyle HeaderLabel => _cache.Get(nameof(HeaderLabel), new GUIStyle(GUI.skin.label)
        {
            fontSize = 10,
            fontStyle = FontStyle.Bold,
            padding = new RectOffset(3, 3, 1, 1),
            margin = new RectOffset(0, 0, 0, 0),
        });

        internal static GUIStyle CodeBlockHeaderButton => _cache.Get(nameof(CodeBlockHeaderButton), new GUIStyle(EditorStyles.iconButton)
        {
            fontSize = 10,
            fixedWidth = 20,
            fixedHeight = 18,
            alignment = TextAnchor.MiddleCenter,
        });

        internal static GUIStyle CodeBlockHeader => _cache.Get(nameof(CodeBlockHeader), new GUIStyle(EditorStyles.helpBox)
        {
            richText = true,
            normal = { background = CodeBlockHeaderTexture, textColor = new Color(0.8f, 0.8f, 0.8f) },
            fontSize = 10,
            fixedHeight = 22,
            margin = new RectOffset(0, 10, 0, 0),
            padding = new RectOffset(6, 6, 3, 3),
            stretchWidth = true,
        });

        internal static GUIStyle CodeBlockContent => _cache.Get(nameof(CodeBlockContent), new GUIStyle(EditorStyles.helpBox)
        {
            richText = true,
            margin = new RectOffset(0, 10, 0, 6),
            padding = new RectOffset(8, 8, 6, 6),
            normal = { background = CodeBlockBodyTexture, textColor = Color.white },
            fontSize = 12,
        });

        private const string kTextureDir = "{0}/CoreLib/Editor/Common/Gizmos/Textures/";
        private static readonly EditorTextureCache<Texture2D> _textureCache = new(string.Format(kTextureDir, EditorPathUtil.FindGlitch9Path()));

        internal static Texture2D CodeBlockBodyTexture => _textureCache.Get("codeblock-body.psd");
        internal static Texture2D CodeBlockHeaderTexture => _textureCache.Get("codeblock-header.psd");
    }
}