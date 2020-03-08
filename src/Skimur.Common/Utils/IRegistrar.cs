using Microsoft.Extensions.DependencyInjection;

namespace Skimur.Common.Utils
{
    public interface IRegistrar
    {
        void Register(IServiceCollection services);

        int Order { get; }
    }
}
