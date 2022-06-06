using System;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Skimur.Backend.Sql
{
    public class ConnectionStringProvider : IConnectionStringProvider
    {
        private readonly string _connectionString = null;

        public ConnectionStringProvider(IConfiguration configuration)
        {
            // detect if we are running on heroku
            if (IsHeroku || InContainer)
            {
                var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                var databaseUri = new Uri(databaseUrl);
                var userInfo = databaseUri.UserInfo.Split(':');

                // Heroku requires us to use use ssl for postgres connection
                var builder = new NpgsqlConnectionStringBuilder
                {
                    Host = databaseUri.Host,
                    Port = databaseUri.Port,
                    Username = userInfo[0],
                    Password = userInfo[1],
                    Database = databaseUri.LocalPath.TrimStart('/')
                };

                if (UsePooling)
                {
                    builder.Pooling = true;
                }

                if (UseSSL)
                {
                    builder.SslMode = SslMode.Prefer;
                    builder.TrustServerCertificate = true;
                }

                _connectionString = builder.ToString();
                return;
            }

            var connection = configuration.GetValue<string>("Skimur:Data:Postgres", null);

            if (string.IsNullOrEmpty(connection))
            {
                return;
            }

            _connectionString = connection;
        }

        public bool HasConnectionString
        {
            get { return !string.IsNullOrEmpty(_connectionString); }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        private bool IsHeroku
        {
            get
            {
                return Environment.GetEnvironmentVariable("HEROKU") == "true";
            }
        }

        private bool InContainer
        {
            get
            {
                return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            }
        }

        private bool UseSSL
        {
            get
            {
                return Environment.GetEnvironmentVariable("USE_SSL") == "true";
            }
        }

        private bool UsePooling
        {
            get
            {
                return Environment.GetEnvironmentVariable("USE_POOLING") == "true";
            }
        }
    }
}
