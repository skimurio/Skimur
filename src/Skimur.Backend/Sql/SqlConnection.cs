using System;
using System.Data;

namespace Skimur.Backend.Sql
{
    public class SqlConnection : IDbConnection
    {
        private readonly IDbConnection _con;

        public SqlConnection(IDbConnection con)
        {
            _con = con;
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _con.BeginTransaction(il);
        }

        public IDbTransaction BeginTransaction()
        {
            return _con.BeginTransaction();
        }

        public void ChangeDatabase(string databaseName)
        {
            _con.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            _con.Close();
        }

        public string ConnectionString
        {
            get { return _con.ConnectionString; }
            set { _con.ConnectionString = value; }
        }

        public int ConnectionTimeout
        {
            get { return _con.ConnectionTimeout; }
        }

        public IDbCommand CreateCommand()
        {
            return new SqlCommand(_con.CreateCommand());
        }

        public string Database
        {
            get { return _con.Database; }
        }

        public void Open()
        {
            _con.Open();
        }

        public ConnectionState State
        {
            get { return _con.State; }
        }

        public void Dispose()
        {
            _con.Dispose();
        }
    }
}
