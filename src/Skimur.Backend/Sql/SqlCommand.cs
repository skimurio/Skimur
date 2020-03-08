using System;
using System.Data;

namespace Skimur.Backend.Sql
{
    public class SqlCommand : IDbCommand
    {
        private readonly IDbCommand _com;

        public SqlCommand(IDbCommand com)
        {
            _com = com;
        }

        public void Cancel()
        {
            _com.Cancel();
        }

        public string CommandText
        {
            get { return _com.CommandText; }
            set { _com.CommandText = value; }
        }

        public int CommandTimeout
        {
            get { return _com.CommandTimeout; }
            set { _com.CommandTimeout = value; }
        }

        public CommandType CommandType
        {
            get { return _com.CommandType; }
            set { _com.CommandType = value; }
        }

        public IDbConnection Connection
        {
            get { return _com.Connection; }
            set { _com.Connection = value; }
        }

        public IDbDataParameter CreateParameter()
        {
            return _com.CreateParameter();
        }

        public int ExecuteNonQuery()
        {
            return _com.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return _com.ExecuteReader(behavior);
        }

        public IDataReader ExecuteReader()
        {
            return _com.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            return _com.ExecuteScalar();
        }

        public IDataParameterCollection Parameters
        {
            get { return _com.Parameters; }
        }

        public void Prepare()
        {
            _com.Prepare();
        }

        public IDbTransaction Transaction
        {
            get { return _com.Transaction; }
            set { _com.Transaction = value; }
        }

        public UpdateRowSource UpdatedRowSource
        {
            get { return _com.UpdatedRowSource; }
            set { _com.UpdatedRowSource = value; }
        }

        public void Dispose()
        {
            _com.Dispose();
        }
    }
}
