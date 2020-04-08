using System;
using System.Collections.Generic;

namespace Skimur.Markdown
{
    public interface IMarkdownCompiler : IDisposable
    {
        string Compile(string markdown);

        string Compile(string markdown, out List<string> mentions);
    }
}
