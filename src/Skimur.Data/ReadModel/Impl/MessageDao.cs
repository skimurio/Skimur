using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class MessageDao 
        // this class temporarily implements the service, unitl we can implement the proper read-only layer
        : MessageService, IMessageDao
    {
        public MessageDao(IDbConnectionProvider conn) : base(conn)
        {
        }
    }
}
