using System;

namespace Skimur.Markdown.Extensions.Mentions
{
    public class MentionOptions
    {
        public bool OpenInNewWindow { get; set; }

        public string Url { get; set; }

        public MentionOptions()
        {
            this.OpenInNewWindow = false;
        }

        public MentionOptions(string url) : this()
        {
            this.Url = url;
        }

        public MentionOptions(Uri uri) : this()
        {
            this.Url = uri.OriginalString;
        }
    }
}
