using System;
using Skimur.Backend.Sql;
using Skimur.Data.Services;
using Skimur.Data.Services.Impl;

namespace Skimur.Data.ReadModel.Impl
{
    public class MembershipDao
        : MembershipService, IMembershipDao
    {
        public MembershipDao(IDbConnectionProvider conn, IPasswordManager passwordManager)
            : base(conn, passwordManager)
        {
        }
    }
}
