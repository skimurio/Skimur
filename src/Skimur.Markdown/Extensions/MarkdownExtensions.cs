using System;
using Markdig;
using Markdig.Helpers;
using Skimur.Markdown.Extensions.Mentions;

namespace Skimur.Markdown.Extensions
{
    public static class MarkdownExtensions
    {
        public static MarkdownPipelineBuilder UseMentions(this MarkdownPipelineBuilder pipeline, MentionOptions options)
        {
            OrderedList<IMarkdownExtension> extensions;

            extensions = pipeline.Extensions;

            if (!extensions.Contains<MentionExtension>())
            {
                extensions.Add(new MentionExtension(options));
            }

            return pipeline;
        }
    }
}
