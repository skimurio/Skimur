using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class ModerationDao
        // todo: only implement read-only methods
        : ModerationService, IModerationDao
    {
        public ModerationDao(IDbConnectionProvider conn)
            : base(conn)
        {
        }
    }
}
