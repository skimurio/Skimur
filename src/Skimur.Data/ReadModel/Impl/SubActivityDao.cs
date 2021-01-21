using System;
using Cassandra;
using Skimur.Data.Services.Impl;
using Skimur.Settings;
using Skimur.Caching;
using Skimur.Data.Settings;
using Skimur.Common.Utils;

namespace Skimur.Data.ReadModel.Impl
{
    public class SubActivityDao : SubActivityService, ISubActivityDao
    {
        private readonly ICache _cache;

        public SubActivityDao(ISession session,
            ISettingsProvider<SubSettings> subSettings,
            ICache cache) : base(session, subSettings)
        {
            _cache = cache;
        }

        public override int GetActiveNumberOfUsersForSub(Guid subId)
        {
            return _cache.GetAcquire("sub." + subId + ".activeusers", CacheTime.Short,
                () => base.GetActiveNumberOfUsersForSub(subId));
        }

        public int GetActiveNumberOfUsersForSubFuzzed(Guid subId, out bool wasActuallyFuzzed)
        {
            wasActuallyFuzzed = false;
            var numberOfUsers = GetActiveNumberOfUsersForSub(subId);

            if (numberOfUsers < 100)
            {
                wasActuallyFuzzed = true;
                return _cache.GetAcquire("sub." + subId + ".activeusers.fuzzed",
                    CacheTime.Short,
                    () => StringHelper.Fuzz(numberOfUsers));
            }

            return numberOfUsers;
        }
    }
}
