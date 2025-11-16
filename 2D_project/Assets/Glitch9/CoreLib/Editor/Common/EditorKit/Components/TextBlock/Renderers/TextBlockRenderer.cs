using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public interface ITextBlockRenderer
    {
        void Draw(TextBlock block, float maxWidth);
    }

    public class TextBlockRenderer
    {
        private static readonly Dictionary<TextBlockType, ITextBlockRenderer> _renderers = new()
        {
            { TextBlockType.Text, new PlainTextRenderer() },
            { TextBlockType.Header, new HeaderRenderer() },
            { TextBlockType.UList, new UnorderedListRenderer() },
            { TextBlockType.Quote, new QuoteRenderer() },
            { TextBlockType.CodeBlock, new CodeBlockRenderer() },
        };

        public static void Draw(TextBlock block, float maxWidth)
        {
            if (block == null) return;

            if (_renderers.TryGetValue(block.type, out ITextBlockRenderer renderer))
            {
                renderer.Draw(block, maxWidth);
            }
        }
    }

    public class PlainTextRenderer : ITextBlockRenderer
    {
        public void Draw(TextBlock block, float maxWidth)
        {
            string text = block.content.Trim();
            if (string.IsNullOrEmpty(text)) return;
            ExGUILayout.SelectableLabel(text, maxWidth, TextBlockGUI.PlainText);
        }
    }

    public class HeaderRenderer : ITextBlockRenderer
    {
        public void Draw(TextBlock block, float maxWidth)
        {
            int level = block.headerLevel;
            int topMargin = Mathf.Clamp(level * 6, 2, 12);
            int botMargin = level * 2;

            GUIStyle headerStyle = new(TextBlockGUI.PlainText)
            {
                fontSize = TextBlockGUI.PlainText.fontSize + level,
                fontStyle = FontStyle.Bold,
                margin = new RectOffset(0, 0, topMargin, botMargin),
            };

            ExGUILayout.SelectableLabel(block.content, maxWidth, headerStyle);
        }
    }

    public class UnorderedListRenderer : ITextBlockRenderer
    {
        public void Draw(TextBlock block, float maxWidth)
        {
            string text = block.content.Trim();
            if (string.IsNullOrEmpty(text)) return;

            GUILayout.BeginHorizontal();
            GUILayout.Space(5f);
            GUILayout.Label("\u2022", TextBlockGUI.HeaderLabel, GUILayout.Width(10f));
            ExGUILayout.SelectableLabel(text, maxWidth, TextBlockGUI.UList);
            GUILayout.EndHorizontal();
        }
    }

    public class QuoteRenderer : ITextBlockRenderer
    {
        public void Draw(TextBlock block, float maxWidth)
        {
            GUIStyle style = new(ExEditorStyles.helpBox) { fontSize = 12, fontStyle = FontStyle.Italic };
            ExGUILayout.SelectableLabel(block.content, maxWidth, style);
        }
    }

    public class CodeBlockRenderer : ITextBlockRenderer
    {
        public void Draw(TextBlock block, float maxWidth)
        {
            string text = block.content;
            GUILayout.BeginVertical(GUILayout.MaxWidth(maxWidth));
            try
            {
                GUILayout.Space(5f);

                DrawHeader(text, block, maxWidth);
                DrawBody(text, block, maxWidth);
            }
            finally
            {
                GUILayout.EndVertical();
            }
        }

        private void DrawHeader(string text, TextBlock block, float maxWidth)
        {
            GUILayout.BeginHorizontal(TextBlockGUI.CodeBlockHeader, GUILayout.MaxWidth(maxWidth));
            try
            {
                GUILayout.Label(block.language, TextBlockGUI.HeaderLabel);
                GUILayout.FlexibleSpace();

                // save button
                GUIContent saveBtnLabel = new(EditorIcons.Save, "Save Code");
                if (GUILayout.Button(saveBtnLabel, TextBlockGUI.CodeBlockHeaderButton))
                {
                    string directory = Application.dataPath;
                    string extension = TextBlockUtil.GetExtension(block.language);
                    string path = EditorUtility.SaveFilePanel("Save Code", directory, "", extension);
                    if (!string.IsNullOrEmpty(path)) System.IO.File.WriteAllText(path, text);
                }

                GUILayout.Space(5f);

                GUIContent copyBtnLabel = new(EditorIcons.Clipboard, "Copy to Clipboard");//block.IsCopied ? new("\u2713 Copied!") : new("Copy Code");
                if (GUILayout.Button(copyBtnLabel, TextBlockGUI.CodeBlockHeaderButton))
                {
                    EditorGUIUtility.systemCopyBuffer = text;
                    block.IsCopied = true;
                }

                GUILayout.Space(5f);
            }
            finally
            {
                GUILayout.EndHorizontal();
            }
        }

        private void DrawBody(string text, TextBlock block, float maxWidth)
        {
            string code;

            try
            {
                code = SyntaxHighlighter.Highlight(block.language, text);
            }
            catch
            {
                code = text;
            }

            //ExGUILayout.SelectableLabel(code, maxWidth, TextBlockGUI.CodeBlockContent);
            GUILayout.Label(code, TextBlockGUI.CodeBlockContent, GUILayout.MaxWidth(maxWidth));
        }
    }
}
