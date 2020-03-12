using System;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.RabbitMq;
using Skimur.Messaging.Handling;
using Skimur.Logging;
using ServiceStack;
using ServiceStack.Messaging;
using RabbitMQ.Client;

namespace Skimur.Messaging.RabbitMQ
{
    public class CommandRegistrar : ICommandRegistrar
    {
        private readonly RabbitMqServer _server;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CommandRegistrar> _logger;

        public CommandRegistrar(RabbitMqServer server, IServiceProvider serviceProvider, ILogger<CommandRegistrar> logger)
        {
            _server = server;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void RegisterCommand<T>() where T : class, ICommand
        {
            _logger.Info("Registering command handler " + typeof(T).Name);

            _server.RegisterHandler<T>(message =>
            {
                try
                {
                    _serviceProvider.GetService<ICommandHandler<T>>().Handle(message.GetBody());
                    return null;
                }
                catch (Exception ex)
                {
                    _logger.Error("Error processing command.", ex);
                    throw;
                }

            });
        }

        public void RegisterCommandResponse<TRequest, TResponse>()
            where TRequest : class, ICommand, ICommandReturns<TResponse>
            where TResponse : class
        {
            _logger.Info("Registering command handler " + typeof(TRequest).Name + " with response "
                + typeof(TResponse).Name);

            _server.RegisterHandler<TRequest>(message =>
            {
                try
                {
                    return _serviceProvider.GetService<ICommandHandlerResponse<TRequest, TResponse>>().Handle(message.GetBody());
                }
                catch (Exception ex)
                {
                    _logger.Error("Error processing command.", ex);
                    throw;
                }
            });
        }
    }
}
