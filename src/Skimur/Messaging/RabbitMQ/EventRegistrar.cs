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
    public class EventRegistrar : IEventRegistrar
    {
        private readonly RabbitMqServer _server;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EventRegistrar> _logger;

        public EventRegistrar(RabbitMqServer server, IServiceProvider serviceProvider, ILogger<EventRegistrar> logger)
        {
            _server = server;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public void RegisterEvent<T, TEventHandler>()
            where T : class, IEvent
            where TEventHandler : class, IEventHandler<T>
        {
            _logger.Info("Registering event handler " + typeof(T).Name);

            var queueName = "mq:" + typeof(TEventHandler).Name + ":" + typeof(T).Name + ".inq";

            using (var messageProducer = (RabbitMqProducer)_server.CreateMessageProducer())
            {
                using (var channel = messageProducer.Channel)
                {
                    channel.ExchangeDeclare(string.Concat(QueueNames.Exchange, ".", "events"),
                        ExchangeType.Direct,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueDeclare(queueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    channel.QueueBind(queueName,
                        string.Concat(QueueNames.Exchange, ".", "events"),
                        QueueNames<T>.In);
                }
            }

            new RabbitMqWorker((RabbitMqMessageFactory)_server.MessageFactory,
                new MessageHandler<T>(_server, message =>
                {
                    _serviceProvider.GetService<TEventHandler>().Handle(message.GetBody());
                    return null;
                })
                {
                    ProcessQueueNames = new[] { queueName }
                },
                queueName,
                (worker, exception) =>
                {
                    _logger.Error("Error from processing queue " + queueName, exception);
                }).Start();
        }
    }
}
