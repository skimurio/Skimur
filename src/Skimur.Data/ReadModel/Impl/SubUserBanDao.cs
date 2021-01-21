using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class SubUserBanDao
        // this class temporarily implements the service, until we implement the proper read-only layer
        : SubUserBanService, ISubUserBanDao
    {
        public SubUserBanDao(IDbConnectionProvider conn)
            : base(conn)
        {
        }
    }
}
