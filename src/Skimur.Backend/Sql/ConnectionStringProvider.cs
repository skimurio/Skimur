using System;
using Microsoft.Extensions.Configuration;

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
