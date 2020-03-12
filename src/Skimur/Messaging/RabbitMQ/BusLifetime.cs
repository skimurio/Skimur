using System;
using RabbitMQ.Client;
using ServiceStack;
using ServiceStack.Messaging;
using ServiceStack.RabbitMq;
using Skimur.Logging;
using Skimur.Messaging.Handling;
using Microsoft.Extensions.DependencyInjection;

namespace Skimur.Messaging.RabbitMQ
{
    public class BusLifetime : IBusLifetime
    {
        private readonly RabbitMqServer _server;
        private readonly IEventRegistrar _eventRegistrar;
        private readonly ICommandRegistrar _commandRegistrar;
        private readonly ILogger<BusLifetime> _logger;

        public BusLifetime(
            RabbitMqServer server,
            ICommandDiscovery commandDiscovery,
            IEventDiscovery eventDiscovery,
            IEventRegistrar eventRegistrar,
            ICommandRegistrar commandRegistrar,
            ILogger<BusLifetime> logger)
        {
            _server = server;
            _eventRegistrar = eventRegistrar;
            _commandRegistrar = commandRegistrar;
            _logger = logger;

            commandDiscovery.Register(_commandRegistrar);
            eventDiscovery.Register(_eventRegistrar);

            _server.DisablePriorityQueues = true;
            _server.DisablePublishingResponses = true;
            _logger.Info("Starting RabbitMQ server");
            _server.Start();
        }

        public void Dispose()
        {
            _logger.Info("Stopping RabbitMQ server");
            _server.Stop();
            _server.Dispose();
        }
    }
}
