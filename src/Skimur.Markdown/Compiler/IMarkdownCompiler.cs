using System;
using System.Collections.Generic;

namespace Skimur.Markdown.Compiler
{
    public interface IMarkdownCompiler
    {
        string Compile(string markdown);
    }
}
