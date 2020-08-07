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
            var connection = configuration.GetValue<string>("Skimur:Data:Postgres", null);

            if (string.IsNullOrEmpty(connection))
            {
                // check if DATABASE_URL environment variable exists, if so use that
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL")))
                {
                    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
                    var databaseUri = new Uri(databaseUrl);
                    var userInfo = databaseUri.UserInfo.Split(':');

                    var builder = new NpgsqlConnectionStringBuilder
                    {
                        Host = databaseUri.Host,
                        Port = databaseUri.Port,
                        Username = userInfo[0],
                        Password = userInfo[1],
                        Database = databaseUri.LocalPath.TrimStart('/')
                    };

                    _connectionString = builder.ToString();
                    return;
                }

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
    }
}
