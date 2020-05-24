using System;
using Skimur.Data.Services.Impl;
using Skimur.Backend.Sql;
using Skimur.Common.Utils;

namespace Skimur.Data.ReadModel.Impl
{
    // this class temporarily implements the service,
    // until we implement the proper read-only layer
    public class SubDao : SubService, ISubDao
    {
        public SubDao(IDbConnectionProvider conn, IMapper mapper) : base(conn, mapper)
        {
            
        }
    }
}
