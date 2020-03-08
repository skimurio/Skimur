using System;
using System.Data;

namespace Skimur.Backend.Sql
{
    public interface IDbConnectionProvider
    {
        IDbConnection OpenConnection();

        void Perform(Action<IDbConnection> action);

        T Perform<T>(Func<IDbConnection, T> func);
    }
}
