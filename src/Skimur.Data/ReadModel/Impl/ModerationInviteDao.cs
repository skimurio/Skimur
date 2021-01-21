using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class ModerationInviteDao
        : ModerationInviteService, IModerationInviteDao
    {
        public ModerationInviteDao(IDbConnectionProvider conn) : base(conn)
        {
        }
    }
}
