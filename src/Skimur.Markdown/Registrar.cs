using Microsoft.Extensions.DependencyInjection;
using Skimur.Common.Utils;
using Skimur.Markdown.Compiler;

namespace Skimur.Markdown
{
    public class Registrar : IRegistrar
    {

        public void Register(IServiceCollection services)
        {
            // register markdig
            services.AddSingleton<IMarkdownCompiler, MarkdownCompiler>();
        }


        public int Order { get { return 0; } }
    }
}
