using System;
using System.Linq;
using Markdig;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;
using Skimur.Markdown.Extensions.Mentions;

namespace Skimur.Markdown.Extensions
{
    public class MentionExtension : IMarkdownExtension
    {
        private readonly MentionOptions _options;

        public MentionExtension(MentionOptions options)
        {
            this._options = options;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            OrderedList<InlineParser> parsers;

            parsers = pipeline.InlineParsers;
            if (!parsers.Contains<MentionParser>())
            {
                parsers.Add(new MentionParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer;
            ObjectRendererCollection renderers;

            htmlRenderer = renderer as HtmlRenderer;
            renderers = htmlRenderer?.ObjectRenderers;

            if (renderers != null && !renderers.Contains<MentionRenderer>())
            {
                renderers.Add(new MentionRenderer(_options));
            }
        }
    }
}
