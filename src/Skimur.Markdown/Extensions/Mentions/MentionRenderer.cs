using System;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Skimur.Markdown.Extensions.Mentions
{
    public class MentionRenderer : HtmlObjectRenderer<Mention>
    {
        private MentionOptions _options;

        public MentionRenderer(MentionOptions options)
        {
            this._options = options;
        }

        protected override void Write(HtmlRenderer renderer, Mention obj)
        {
            StringSlice username = obj.Username;

            if (renderer.EnableHtmlForInline)
            {
                //TODO: find username from database so we can verify that a user exists,
                // if a user exists, render the link, if not just output the @username
                renderer.Write("<a href=\"")
                    .Write(_options.Url)
                    .Write("/u/").Write(username).Write('"');

                if (_options.OpenInNewWindow)
                {
                    renderer.Write(" target=\"blank\" rel=\"noopener noreferrer\"");
                }

                renderer.Write('>').Write('@').Write(username).Write("</a>");
            }
            else
            {
                renderer.Write('@').Write(obj.Username);
            }
        }
    }
}
