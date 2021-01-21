using System;
using Skimur.Backend.Sql;
using Skimur.Data.Services.Impl;

namespace Skimur.Data.ReadModel.Impl
{
    public class PostDao
        // this class temporarily implements the service, until we can implement the proper read-only layer
        : PostService, IPostDao
    {
        public PostDao(IDbConnectionProvider conn) : base(conn)
        {
        }
    }
}
