using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Glitch9.EditorKit
{
    public static class EditorColors
    {
        private static readonly Dictionary<string, Color> _cache = new();
        public const string kEditorBlueHex = "#4592ff";
        public static string LinkLabelHex => EditorStyles.linkLabel.normal.textColor.ToHex();
        public const string kEditorOrangeHex = "#ff6e40";

#pragma warning disable IDE1006
        public static Color textColor => GetWithComplementary(nameof(textColor), Color.black);
        public static Color gray => GetWithComplementary(nameof(gray), new Color(0.3f, 0.3f, 0.3f));
        public static Color blue => Get(nameof(blue), kEditorBlueHex.ToColor());
        public static Color linkLabel => Get(nameof(linkLabel) + "v1", EditorStyles.linkLabel.normal.textColor);
        public static Color orange => Get(nameof(orange), kEditorOrangeHex.ToColor());

#pragma warning disable IDE1006

        #region utility
        private static Color GetComplementaryColor(Color color)
            => new(1 - color.r, 1 - color.g, 1 - color.b, color.a);

        private static Color GetWithComplementary(string key, Color lightColor)
        {
            if (_cache.TryGetValue(key, out Color color)) return color;
            color = EditorGUIUtility.isProSkin ? GetComplementaryColor(lightColor) : lightColor;
            _cache.Add(key, color);
            return color;
        }

        private static Color Get(string key, Color unknownColor)
        {
            if (_cache.TryGetValue(key, out Color color)) return color;
            color = unknownColor;
            _cache.Add(key, color);
            return color;
        }

        private static Color Get(string key, Color lightColor, Color darkColor)
        {
            if (_cache.TryGetValue(key, out Color color)) return color;
            color = EditorGUIUtility.isProSkin ? darkColor : lightColor;
            _cache.Add(key, color);
            return color;
        }

        #endregion utility 
    }
}