using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class VoteDao
        // this class temporarily implements the service, until we implement the proper read-only layer
        : VoteService, IVoteDao
    {
        public VoteDao(IDbConnectionProvider conn)
            : base(conn)
        {
        }
    }
}
