using System;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.ReadModel
{
    public interface ISubUserBanDao
    {
        SeekedList<SubUserBan> GetBannedUsersInSub(Guid subId, Guid? userId = null, int? skip = null, int? take = null);

        SubUserBan GetBannedUserInSub(Guid subId, Guid userId);

        bool IsUserBannedFromSub(Guid subId, Guid userId);
    }
}
