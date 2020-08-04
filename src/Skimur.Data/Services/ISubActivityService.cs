using System;
namespace Skimur.Data.Services
{
    public interface ISubActivityService
    {
        void MarkSubActive(Guid userId, Guid subId);

        int GetActiveNumberOfUsersForSub(Guid subId);
    }
}
