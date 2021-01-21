using System;

namespace Skimur.Data.ReadModel
{
    public interface ISubActivityDao
    {
        void MarkSubActive(Guid userId, Guid subId);

        int GetActiveNumberOfUsersForSub(Guid subId);

        int GetActiveNumberOfUsersForSubFuzzed(Guid subId, out bool wasActuallyFuzzed);
    }
}
