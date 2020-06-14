using Microsoft.Extensions.DependencyInjection;
using Skimur.Data.Commands;
using Skimur.Data.Commands.Handlers;
using Skimur.Messaging.Handling;
using Skimur.Common.Utils;
using Skimur.Data.Events.Handlers;

namespace Skimur.Data
{
    public class MessagingRegistrar : IRegistrar
    {
        public void Register(IServiceCollection services)
        {
            // commands
            services.AddTransient<ICommandHandler<SendEmail>, EmailHandler>();

            // sub commands
            services.AddTransient<ICommandHandlerResponse<CreateSub, CreateSubResponse>, SubHandler>();

            // events
            services.AddTransient<SubEventHandler>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
