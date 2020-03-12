using Microsoft.Extensions.DependencyInjection;
using Skimur.Data.Commands;
using Skimur.Data.Commands.Handlers;
using Skimur.Messaging.Handling;
using Skimur.Common.Utils;

namespace Skimur.Data
{
    public class MessagingRegistrar : IRegistrar
    {
        public void Register(IServiceCollection services)
        {
            services.AddTransient<ICommandHandler<SendEmail>, EmailHandler>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
