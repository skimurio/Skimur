using Microsoft.Extensions.DependencyInjection;
using Jering.Javascript.NodeJS;
using Skimur.Common.Utils;

namespace Skimur.Markdown
{
    public class Registrar : IRegistrar
    {

        public void Register(IServiceCollection services)
        {
            services.AddNodeJS();
        }

        public int Order { get { return 0; } }
    }
}
