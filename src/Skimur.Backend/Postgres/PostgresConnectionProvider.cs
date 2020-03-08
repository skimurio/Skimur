using System;
using System.Data;
using System.Data.Common;
using Npgsql;
using ServiceStack;
using ServiceStack.OrmLite;
using Skimur.Backend.Sql;

namespace Skimur.Backend.Postgres
{ 
    public class PostgresConnectionProvider : IDbConnectionProvider
    {
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly DbProviderFactory _factory;

        public PostgresConnectionProvider(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;

            OrmLiteConfig.DialectProvider = PostgreSqlDialect.Provider;
            _factory = NpgsqlFactory.Instance;
        }

        public IDbConnection OpenConnection()
        {
            if (!_connectionStringProvider.HasConnectionString)
            {
                throw new Exception("There is no connection string configured!");
            }

            var connection = _factory.CreateConnection();
            if (connection == null)
            {
                throw new Exception("Couldn't create connection from the factory.");
            }

            connection.ConnectionString = _connectionStringProvider.ConnectionString;
            connection.Open();

            // wrap the connetion
            return new SqlConnection(connection);
        }

        public void Perform(Action<IDbConnection> action)
        {
            using (var conn = OpenConnection())
            {
                action(conn);
            }
        }

        public T Perform<T>(Func<IDbConnection, T> func)
        {
            using (var conn = OpenConnection())
            {
                return func(conn);
            }
        }

    }
}
