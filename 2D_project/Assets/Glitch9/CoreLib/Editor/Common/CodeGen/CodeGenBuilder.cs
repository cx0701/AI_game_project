using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace Glitch9.EditorKit.CodeGen
{
    #region Enums

    public enum CommentType
    {
        Summary,
        Param,
        Returns,
        Exception,
        Remarks,
    }

    public enum AccessModifier
    {
        Public,
        Private,
        Protected,
        Internal,
        ProtectedInternal,
    }

    public enum ClassType
    {
        Class,
        Struct,
        Interface,
        Enum,
        Delegate,
    }

    public class DirectiveComment
    {
        public const string ReSharperDisableAll = "ReSharper disable All";
    }

    #endregion

    #region Interfaces

    public interface ICodeGenObject
    {
        string Build(int indentLevel, int? spaces = null);
    }

    public interface ICodeGenCode : ICodeGenObject
    {
    }

    #endregion

    #region Base Class

    public abstract class CodeGenObject : ICodeGenObject
    {
        public string Name => _name;

        protected readonly AccessModifier _accessModifier;
        protected readonly bool _isStatic;
        protected readonly string _name;
        protected readonly List<CodeGenAttribute> _attributes;
        protected readonly List<CodeGenComment> _comments;

        public CodeGenObject(
            string name,
            AccessModifier accessModifier = AccessModifier.Public,
            bool isStatic = false,
             List<CodeGenComment> comments = null,
             List<CodeGenAttribute> attributes = null)
        {
            _accessModifier = accessModifier;
            _isStatic = isStatic;
            _name = name;
            _comments = comments;
            _attributes = attributes;
        }

        public abstract string Build(int indentLevel, int? spaces = null);
    }

    #endregion

    #region Basic Building Blocks

    public class CodeGenComment : ICodeGenObject
    {
        private readonly List<string> _comments;
        private readonly string _paramName;
        private readonly string _cref;
        private readonly CommentType _type;

        public CodeGenComment(List<string> comments, CommentType type = CommentType.Summary, string paramName = null, string cref = null)
        {
            _comments = comments;
            _type = type;
            _paramName = paramName;
            _cref = cref;
        }

        public static CodeGenComment Summary(params string[] comments) => new(comments.ToList(), CommentType.Summary);
        public static CodeGenComment Param(string paramName, params string[] comments) => new(comments.ToList(), CommentType.Param, paramName);
        public static CodeGenComment Returns(params string[] comments) => new(comments.ToList(), CommentType.Returns);
        public static CodeGenComment Exception(string cref, params string[] comments) => new(comments.ToList(), CommentType.Exception, cref: cref);
        public static CodeGenComment Remarks(params string[] comments) => new(comments.ToList(), CommentType.Remarks);

        private void AppendComments(StringBuilder sb, int indentLevel, int? spaces)
        {
            foreach (var comment in _comments)
            {
                if (string.IsNullOrEmpty(comment)) continue;

                string[] smartSplit = SmartStringSplitter.SplitSmart(comment);

                foreach (var line in smartSplit)
                {
                    sb.AppendLineWithIndent($"/// {line}", indentLevel, spaces);
                }
            }
        }

        public string Build(int indentLevel, int? spaces)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                switch (_type)
                {
                    case CommentType.Summary:
                        sb.AppendLineWithIndent("/// <summary>", indentLevel, spaces);
                        AppendComments(sb, indentLevel, spaces);
                        sb.AppendWithIndent("/// </summary>", indentLevel, spaces);
                        break;
                    case CommentType.Param:
                        sb.AppendLineWithIndent($"/// <param name=\"{_paramName}\">", indentLevel, spaces);
                        AppendComments(sb, indentLevel, spaces);
                        sb.AppendWithIndent("/// </param>", indentLevel, spaces);
                        break;
                    case CommentType.Returns:
                        sb.AppendLineWithIndent("/// <returns>", indentLevel, spaces);
                        AppendComments(sb, indentLevel, spaces);
                        sb.AppendWithIndent("/// </returns>", indentLevel, spaces);
                        break;
                    case CommentType.Exception:
                        sb.AppendLineWithIndent($"/// <exception cref=\"{_cref}\">", indentLevel, spaces);
                        AppendComments(sb, indentLevel, spaces);
                        sb.AppendWithIndent("/// </exception>", indentLevel, spaces);
                        break;
                    case CommentType.Remarks:
                        sb.AppendLineWithIndent("/// <remarks>", indentLevel, spaces);
                        AppendComments(sb, indentLevel, spaces);
                        sb.AppendWithIndent("/// </remarks>", indentLevel, spaces);
                        break;
                }
                return sb.ToString();
            }
        }
    }

    public class CodeGenAttribute : ICodeGenObject
    {
        private readonly string _name;
        private readonly List<string> _values;

        public CodeGenAttribute(string name, List<string> values = null)
        {
            _name = name;
            _values = values;
        }

        public static CodeGenAttribute Create(string name, params string[] values)
        {
            return new CodeGenAttribute(name, values.ToList());
        }

        public static CodeGenAttribute Obsolete(string message = null)
        {
            return new CodeGenAttribute("Obsolete", new List<string> { message });
        }

        public string Build(int indentLevel, int? spaces = null)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                sb.AppendWithIndent($"[{_name}", indentLevel, spaces);

                if (_values != null && _values.Count > 0)
                {
                    sb.Append("(");
                    for (int i = 0; i < _values.Count; i++)
                    {
                        sb.Append(_values[i]);
                        if (i < _values.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                    sb.Append(")");
                }

                sb.Append("]");

                return sb.ToString();
            }
        }
    }

    #endregion

    #region Main CodeGen Units

    public class CodeGenClass : CodeGenObject
    {
        private readonly ClassType _type;
        private readonly bool _isPartial;
        private readonly List<string> _superClasses;
        private readonly List<string> _interfaces;
        private readonly List<ICodeGenCode> _codes = new();

        public CodeGenClass(
            string name,
            ClassType type = ClassType.Class,
            List<ICodeGenCode> codes = null,
            bool isStatic = false,
            bool isPartial = false,
            AccessModifier accessModifier = AccessModifier.Public,
            List<CodeGenComment> comments = null,
            List<string> superClasses = null,
            List<string> interfaces = null
            ) : base(name, accessModifier, isStatic, comments)
        {
            _type = type;
            _isPartial = isPartial;
            _superClasses = superClasses;
            _interfaces = interfaces;

            if (codes != null) _codes.AddRange(codes);
        }

        public void AddCode(ICodeGenCode code)
        {
            _codes.Add(code);
        }

        public override string Build(int indentLevel, int? spaces = null)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                sb.AppendComments(_comments, indentLevel, spaces);
                sb.AppendAttributes(_attributes, indentLevel, spaces);

                sb.AppendWithIndent($"{_accessModifier.ToString().ToLower()}", indentLevel, spaces);
                if (_isStatic) sb.Append(" static");
                if (_isPartial) sb.Append(" partial");
                sb.Append($" {_type.ToString().ToLower()} {_name}");

                if (_superClasses != null && _superClasses.Count > 0)
                {
                    sb.Append(" : ");
                    for (int i = 0; i < _superClasses.Count; i++)
                    {
                        sb.Append(_superClasses[i]);
                        if (i < _superClasses.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                }

                if (_interfaces != null && _interfaces.Count > 0)
                {
                    if (_superClasses == null || _superClasses.Count == 0)
                        sb.Append(" : ");
                    else
                        sb.Append(", ");

                    for (int i = 0; i < _interfaces.Count; i++)
                    {
                        sb.Append(_interfaces[i]);
                        if (i < _interfaces.Count - 1)
                        {
                            sb.Append(", ");
                        }
                    }
                }

                sb.AppendLine();
                sb.AppendWithIndent("{", indentLevel, spaces);
                sb.AppendLine();

                foreach (var code in _codes)
                {
                    sb.AppendLine(code.Build(indentLevel + 1, spaces));
                    sb.AppendLine();
                }

                sb.AppendWithIndent("}", indentLevel, spaces);

                return sb.ToString();
            }
        }
    }

    public class CodeGenEnumValue : ICodeGenCode
    {
        private readonly string _name;
        private readonly List<CodeGenComment> _comments;
        private readonly List<CodeGenAttribute> _attributes;

        public CodeGenEnumValue(string name, List<CodeGenComment> comments = null, List<CodeGenAttribute> attributes = null)
        {
            _name = name;
            _comments = comments;
            _attributes = attributes;
        }

        public string Build(int indentLevel, int? spaces = null)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                sb.AppendComments(_comments, indentLevel, spaces);
                sb.AppendAttributes(_attributes, indentLevel, spaces);
                sb.AppendWithIndent($"{_name},", indentLevel, spaces);
                return sb.ToString();
            }
        }
    }

    public class CodeGenVariable : CodeGenObject, ICodeGenCode
    {
        private readonly string _type;
        private readonly string _value;
        private readonly bool _isReadOnly;
        private readonly bool _isConst;

        public CodeGenVariable(
            string type,
            string name,
            string value = null,
            AccessModifier accessModifier = AccessModifier.Public,
            bool isStatic = false,
            bool isReadOnly = false,
            bool isConst = false,
            List<CodeGenComment> comments = null,
            List<CodeGenAttribute> attributes = null
            ) : base(name, accessModifier, isStatic, comments, attributes)
        {
            _type = type;
            _value = value;
            _isReadOnly = isReadOnly;
            _isConst = isConst;
        }

        public override string Build(int indentLevel, int? spaces = null)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                sb.AppendComments(_comments, indentLevel, spaces);
                sb.AppendAttributes(_attributes, indentLevel, spaces);

                sb.AppendWithIndent($"{_accessModifier.ToString().ToLower()}", indentLevel, spaces);

                if (_isConst)
                {
                    sb.Append(" const");
                }
                else
                {
                    if (_isStatic) sb.Append(" static");
                    if (_isReadOnly) sb.Append(" readonly");
                }

                sb.Append($" {_type} {_name}");

                if (_value != null) sb.Append($" = {_value};");
                else sb.Append(";");

                return sb.ToString();
            }
        }
    }

    public class CodeGenProperty : CodeGenObject, ICodeGenCode
    {
        private readonly string _type;
        private readonly string _value;
        private readonly bool _isReadOnly;
        private readonly bool _isConst;

        public CodeGenProperty(
            string type,
            string name,
            string value = null,
            AccessModifier accessModifier = AccessModifier.Public,
            bool isStatic = false,
            bool isReadOnly = false,
            bool isConst = false,
            List<CodeGenComment> comments = null
            ) : base(name, accessModifier, isStatic, comments)
        {
            _type = type;
            _value = value;
            _isReadOnly = isReadOnly;
            _isConst = isConst;
        }

        public override string Build(int indentLevel, int? spaces = null)
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                sb.AppendComments(_comments, indentLevel, spaces);
                sb.AppendAttributes(_attributes, indentLevel, spaces);

                sb.AppendWithIndent($"{_accessModifier.ToString().ToLower()}", indentLevel, spaces);

                if (_isConst)
                {
                    sb.Append(" const");
                }
                else
                {
                    if (_isStatic) sb.Append(" static");
                    if (_isReadOnly) sb.Append(" readonly");
                }

                sb.Append($" {_type} {_name} {{ get; set; }}");

                if (_value != null) sb.Append($" = {_value};");

                return sb.ToString();
            }
        }
    }

    #endregion

    #region Builder

    public class CodeGenBuilder
    {
        private readonly List<string> _usings = new();
        private string _namespace;
        private readonly List<CodeGenClass> _classes = new();
        private readonly List<string> _directiveComments = new();

        //private bool _addReSharperDisableAll = false;

        public CodeGenBuilder AddUsing(string usingName)
        {
            _usings.Add(usingName);
            return this;
        }

        public CodeGenBuilder SetNamespace(string namespaceName)
        {
            _namespace = namespaceName;
            return this;
        }

        public CodeGenBuilder AddClass(
            string className,
            ClassType type = ClassType.Class,
            List<ICodeGenCode> codes = null,
            bool isStatic = false,
            bool isPartial = false,
            AccessModifier accessModifier = AccessModifier.Public,
            List<CodeGenComment> comments = null,
            List<string> superClasses = null,
            List<string> interfaces = null)
        {
            _classes.Add(new CodeGenClass(className, type, codes, isStatic, isPartial, accessModifier, comments, superClasses, interfaces));
            return this;
        }

        public CodeGenBuilder AddEnumClass(
            string className,
            AccessModifier accessModifier = AccessModifier.Public,
            List<CodeGenEnumValue> enums = null,
            List<CodeGenComment> comments = null,
            List<string> superClasses = null)
        {
            List<ICodeGenCode> codes = new();
            if (enums != null) codes.AddRange(enums);
            _classes.Add(new CodeGenClass(className, ClassType.Enum, codes, false, false, accessModifier, comments, superClasses));
            return this;
        }

        public CodeGenBuilder AddCode(
            string className,
            ICodeGenCode code)
        {
            var @class = _classes.Find(c => c.Name == className);
            @class?.AddCode(code);
            return this;
        }

        public CodeGenBuilder AddContString(
            string className,
            string propertyName,
            string value,
            AccessModifier accessModifier = AccessModifier.Public,
            List<CodeGenComment> comments = null,
            List<CodeGenAttribute> attributes = null)
        {
            var @class = _classes.Find(c => c.Name == className);
            @class?.AddCode(new CodeGenVariable("string", propertyName, value: $"\"{value}\"", accessModifier, false, false, true, comments, attributes));
            return this;
        }

        public CodeGenBuilder AddEnumValue(
            string className,
            string valueName,
            List<CodeGenComment> comments = null,
            List<CodeGenAttribute> attributes = null)
        {
            var @class = _classes.Find(c => c.Name == className);
            @class?.AddCode(new CodeGenEnumValue(valueName, comments, attributes));
            return this;
        }

        public CodeGenBuilder AddDirectiveComment(string directiveComment)
        {
            _directiveComments.Add(directiveComment);
            return this;
        }

        public string Build()
        {
            using (StringBuilderPool.Get(out StringBuilder sb))
            {
                foreach (var usingName in _usings)
                {
                    sb.AppendLine($"using {usingName};");
                }

                sb.AppendLine();

                if (_directiveComments.Count > 0)
                {
                    foreach (var directiveComment in _directiveComments)
                    {
                        sb.AppendLine($"// {directiveComment}");
                    }
                }

                int classIndentLevel = 0;

                if (_namespace != null)
                {
                    sb.AppendLine($"namespace {_namespace}");
                    sb.AppendLine("{");
                    classIndentLevel++;
                }

                foreach (var @class in _classes)
                {
                    sb.AppendLine(@class.Build(classIndentLevel));
                    sb.AppendLine();
                }

                if (_namespace != null)
                {
                    sb.AppendLine("}");
                }

                return sb.ToString();
            }
        }

        public void Generate(string path, bool overwrite = true, bool createBackup = true)
        {
            if (string.IsNullOrEmpty(System.IO.Path.GetDirectoryName(path)))
                throw new System.Exception($"Invalid path: {path}");

            if (System.IO.File.Exists(path))
            {
                if (overwrite)
                {
                    if (createBackup)
                    {
                        string backupPath = path + ".bak";
                        if (System.IO.File.Exists(backupPath)) System.IO.File.Delete(backupPath);
                        System.IO.File.Move(path, backupPath);
                    }
                    System.IO.File.Delete(path);
                }
                else
                {
                    throw new System.Exception($"File already exists at {path}");
                }
            }

            var content = Build();

            if (string.IsNullOrEmpty(content))
                throw new System.Exception("The generated content is empty");

            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(path)))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            }

            System.IO.File.WriteAllText(path, content);
            AssetDatabase.SaveAssets();
        }
    }

    #endregion
}
