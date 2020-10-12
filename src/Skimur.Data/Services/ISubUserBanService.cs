using System;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.Services
{
    public interface ISubUserBanService
    {
        SeekedList<SubUserBan> GetBannedUsersInSub(Guid subId, Guid? userId = null, int? skip = null, int? take = null);

        SubUserBan GetBannedUserInSub(Guid subId, Guid userId);

        bool IsUserBannedFromSub(Guid subId, Guid userId);

        void BanUserFromSub(Guid subId, Guid userId, DateTime dateBanned, Guid bannedBy, string reason, DateTime? expires = null);

        void UnbanUserFromSub(Guid subId, Guid userId);

        void UpdateSubBanForUser(Guid subId, Guid userId, string reason);
    }
}
