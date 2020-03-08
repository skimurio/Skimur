using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Skimur.Common.Email;
using Skimur.Common.Utils;
using Skimur.Backend.Sql;
using Skimur.Backend.Postgres;
using Skimur.Logging;

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

            collection.AddSingleton<IServiceCollection>(provider => collection);

            collection.AddSkimurBase(registrars);


            return collection.BuildServiceProvider();
        }

        public static IServiceCollection AddSkimurBase(this IServiceCollection services, params IRegistrar[] registrars)
        {
            // all the default services
            services.AddSingleton<IMapper, Mapper>();
            services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
            services.AddSingleton<IDbConnectionProvider, PostgresConnectionProvider>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services.AddSingleton<Backend.Postgres.Migrations.IMigrationEngine, Backend.Postgres.Migrations.MigrationEngine>();
            services.AddSingleton<Backend.Postgres.Migrations.IMigrationResourceFinder, Backend.Postgres.Migrations.MigrationResourceFinder>();

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
