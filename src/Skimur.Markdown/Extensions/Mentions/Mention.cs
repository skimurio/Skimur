using System;
using System.Diagnostics;
using Markdig.Helpers;
using Markdig.Syntax.Inlines;

namespace Skimur.Markdown.Extensions.Mentions
{
    [DebuggerDisplay("@{" + nameof(Username) + "}")]
    public class Mention : LeafInline
    {
        public StringSlice Username;
    }
}
