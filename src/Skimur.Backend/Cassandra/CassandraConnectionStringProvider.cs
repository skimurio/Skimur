using Microsoft.Extensions.Configuration;

namespace Skimur.Backend.Cassandra
{
    public class CassandraConnectionStringProvider : ICassandraConnectionStringProvider
    {
        private readonly string _connectionString = null;

        public CassandraConnectionStringProvider(IConfiguration configuration)
        {
            var connection = configuration.GetValue<string>("Data:Cassandra", null);

            if (connection == null) return;

            if (string.IsNullOrEmpty(connection))
                return;

            _connectionString = connection;
        }

        /// <summary>
        /// Is there a valid connection string configured?
        /// </summary>
        public bool HasConnectionString
        {
            get { return !string.IsNullOrEmpty(_connectionString); }
        }

        /// <summary>
        /// The connection string
        /// </summary>
        public string ConnectionString
        {
            get { return _connectionString; }
        }

    }
}
