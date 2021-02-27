using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ServiceStack.RabbitMq;
using ServiceStack.Redis;
using Cassandra;
using Skimur.Common.Email;
using Skimur.Common.Utils;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres;
using Skimur.Logging;
using Skimur.IO;
using Skimur.Messaging;
using Skimur.Messaging.Handling;
using Skimur.Messaging.RabbitMQ;
using Skimur.Settings;
using Skimur.Caching;
using Skimur.Backend.Cassandra;

namespace Skimur.Common
{
    public static class SkimurContext
    {
        private static IServiceProvider _sserviceProvider;
        private static object _lock = new object();

        public static void Initialize(params IRegistrar[] registrars)
        {
            lock (_lock)
            {
                _sserviceProvider = BuildServiceProvider(registrars);
            }
        }

        public static IServiceProvider BuildServiceProvider(params IRegistrar[] registrars)
        {
            var collection = new ServiceCollection();

            //collection.AddSingleton<IServiceCollection>(provider => collection);

            collection.AddSkimurBase(registrars);

            return collection.BuildServiceProvider();
        }

        public static IServiceCollection AddSkimurBase(this IServiceCollection services, params IRegistrar[] registrars)
        {
            // register our IServiceCollection
            services.AddSingleton(provider => services);

            // all the default services
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<IDbConnectionProvider, PostgresConnectionProvider>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IPathResolver, PathResolver>();

            services.AddSingleton<ICache, RedisCache>();
            services.AddSingleton<IRedisClientsManager>(provider =>
            {
                if (EnvironmentUtils.IsHeroku)
                {
                    // override host and use CLOUDAMQP_URL variable
                    var redisHost = Environment.GetEnvironmentVariable("REDIS");
                    return new PooledRedisClientManager(redisHost);
                }
                else
                {
                    var configuration = provider.GetService<IConfiguration>();
                    var readWrite = configuration.GetValue<string>("Skimur:Data:RedisReadWrite");
                    var read = configuration.GetValue<string>("Skimur:Data:RedisRead");
                    return new PooledRedisClientManager(readWrite.Split(";"), read.Split(";"));
                }
            });

            services.AddSingleton<ICassandraConnectionStringProvider, CassandraConnectionStringProvider>();
            services.AddSingleton(provider =>
            {
                var connectionProvider = provider.GetService<ICassandraConnectionStringProvider>();

                if (!connectionProvider.HasConnectionString)
                {
                    throw new Exception("No connection string configured for cassandra.");
                }

                return Cluster.Builder()
                    .AddContactPoint(connectionProvider.ConnectionString)
                    .WithDefaultKeyspace("skimur")
                    .Build();
            });
            services.AddSingleton(provider =>
            {
                var cluster = provider.GetService<Cluster>();
                return cluster.ConnectAndCreateDefaultKeyspaceIfNotExists();
            });
            services.AddSingleton<Backend.Cassandra.Migrations.IMigrationEngine, Backend.Cassandra.Migrations.MigrationEngine>();
            services.AddSingleton<Backend.Cassandra.Migrations.IMigrationResourceFinder, Backend.Cassandra.Migrations.MigrationResourceFinder>();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddSingleton<IEventDiscovery, EventDiscovery>();
            services.AddSingleton<ICommandDiscovery, CommandDiscovery>();

            services.AddSingleton<Backend.Postgres.Migrations.IMigrationEngine, Backend.Postgres.Migrations.MigrationEngine>();
            services.AddSingleton<Backend.Postgres.Migrations.IMigrationResourceFinder, Backend.Postgres.Migrations.MigrationResourceFinder>();

            services.AddSingleton(typeof(ISettingsProvider<>), typeof(JsonFileSettingsProvider<>));

            services.AddSingleton(provider =>
            {
                var configuration = provider.GetService<IConfiguration>();
                var rabbitMqHost = configuration.GetValue<string>("Skimur:Data:RabbitMQHost");

                if (EnvironmentUtils.IsHeroku)
                {
                    // override host and use CLOUDAMQP_URL variable
                    rabbitMqHost = Environment.GetEnvironmentVariable("CLOUDAMQP_URL");
                }

                if (string.IsNullOrEmpty(rabbitMqHost))
                {
                    throw new Exception("You must provide a 'Skimur:Data:RabbitMQHost' app setting.");
                }

                return new RabbitMqServer(rabbitMqHost)
                {
                    ErrorHandler = exception =>
                    {
                        Logging.Logger.For<RabbitMqServer>().Error("There was an error processing a message.", exception);
                    }
                };
            });

            services.AddSingleton<ICommandRegistrar, CommandRegistrar>();
            services.AddSingleton<IEventRegistrar, EventRegistrar>();
            services.AddSingleton<ICommandBus, CommandBus>();
            services.AddSingleton<IEventBus, EventBus>();
            services.AddSingleton<IBusLifetime, BusLifetime>();

            foreach (var registrar in registrars.OrderBy(x => x.Order))
            {
                registrar.Register(services);
            }

            return services;
        }

        public static IServiceProvider ServiceProvider
        {
            get
            {
                EnsureInitialized();
                return _sserviceProvider;
            }
        }

        private static void EnsureInitialized()
        {
            if (_sserviceProvider == null)
            {
                throw new Exception("SkimurContext has not been initialized.");
            }
        }
    }
}
