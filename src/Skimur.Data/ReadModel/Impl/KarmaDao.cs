using System;
using Cassandra;
using Skimur.Data.Services.Impl;

namespace Skimur.Data.ReadModel.Impl
{
    public class KarmaDao :
        // temp until we get a proper caching layer.
        KarmaService, IKarmaDao
    {
        public KarmaDao(ISession session) : base(session)
        {

        }
    }
}
