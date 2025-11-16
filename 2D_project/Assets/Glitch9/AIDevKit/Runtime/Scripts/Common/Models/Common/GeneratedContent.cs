using System;
using Glitch9.IO.RESTApi;
using UnityEngine;

namespace Glitch9.AIDevKit
{
    public class GeneratedContent : IGeneratedContent
    {
        public static implicit operator Content(GeneratedContent generatedContent) => generatedContent?.content;
        public static implicit operator string(GeneratedContent generatedContent) => generatedContent?.content?.ToString();

        public int Count => content.Length;
        public Content Content => content;
        public Usage Usage => usage;

        private readonly Content content;
        private readonly ToolCall[] toolCalls;
        private readonly Usage usage;

        public GeneratedContent(Content content, ToolCall[] toolCalls, Usage usage = null)
        {
            this.content = content;
            this.usage = usage;
        }
    }

    public interface IGeneratedContent
    {
        int Count { get; }
        Usage Usage { get; }
    }

    public abstract class GeneratedContent<T> : RESTResponse, IGeneratedContent
    {
        public int Count => contents.Length;
        public T[] Contents => contents;
        public Usage Usage { get => usage; set => usage = value; }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= contents.Length)
                    throw new IndexOutOfRangeException($"Index {index} is out of range. Count: {contents.Length}");
                return contents[index];
            }
        }

        protected readonly T[] contents;
        protected Usage usage;

        protected GeneratedContent(T content, Usage usage)
        {
            if (content == null) Debug.LogWarning("The generation request returned no content. Please check the request and try again.");
            contents = new[] { content };
            this.usage = usage;
        }

        protected GeneratedContent(T[] contents, Usage usage)
        {
            if (contents.IsNullOrEmpty()) Debug.LogWarning("The generation request returned no content. Please check the request and try again.");
            this.contents = contents;
            this.usage = usage;
        }
    }
}
