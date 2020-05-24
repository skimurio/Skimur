using System;
using System.Linq;
using Skimur.Markdown.Extensions;
using Skimur.Markdown.Extensions.Mentions;

namespace Skimur.Markdown.Compiler
{
    using Markdig;

    public class MarkdownCompiler : IMarkdownCompiler
    {
        private MarkdownPipeline pipeline;

        public MarkdownCompiler()
        {
            pipeline = new MarkdownPipelineBuilder()
                .UseMentions(new MentionOptions("https://skimur.io/u/"))
                .Build();

        }

        public string Compile(string markdown)
        {
            var results = Markdown.ToHtml(markdown, pipeline);

            return results;
        }
    }
}
