using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;

namespace Skimur.Data.ReadModel.Impl
{
    public class SubCssDao
        : SubCssService, ISubCssDao
    {
        public SubCssDao(IDbConnectionProvider conn)
            : base(conn)
        {
        }
    }
}
