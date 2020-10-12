using System;
using ServiceStack.OrmLite;
using Skimur.Backend.Sql;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.Services.Impl
{
    public class SubUserBanService : ISubUserBanService
    {
        private readonly IDbConnectionProvider _conn;

        public SubUserBanService(IDbConnectionProvider conn)
        {
            _conn = conn;
        }

        public SeekedList<SubUserBan> GetBannedUsersInSub(Guid subId, Guid? userId = null, int? skip = null, int? take = null)
        {
            return _conn.Perform(conn =>
            {
                var query = conn.From<SubUserBan>().Where(x => x.SubId == subId);

                if (userId != null) {
                    query.Where(x => x.UserId == userId);
                }

                var totalCount = conn.Count(query);
                query.Skip(skip).Take(take);

                return new SeekedList<SubUserBan>(conn.Select(query), skip ?? 0, take, totalCount);
            });
        }

        public SubUserBan GetBannedUserInSub(Guid subId, Guid userId)
        {
            return _conn.Perform(conn =>
            {
                return conn.Single<SubUserBan>(x => x.SubId == subId && x.UserId == userId && x.IsActive == true);
            });
        }

        public bool IsUserBannedFromSub(Guid subId, Guid userId)
        {
            return _conn.Perform(conn => conn.Count<SubUserBan>(x => x.SubId == subId && x.UserId == userId && x.IsActive == true)) > 0;
        }

        public void BanUserFromSub(Guid subId, Guid userId, DateTime dateBanned, Guid bannedBy, string reason, DateTime? expires = null)
        {
            _conn.Perform(conn =>
            {
                var existing = conn.Single<SubUserBan>(x => x.SubId == userId && x.SubId == subId && x.IsActive == true);

                if (existing != null)
                {
                    existing.BannedBy = bannedBy;
                    existing.Reason = reason;
                    existing.CreatedAt = dateBanned;
                    existing.Expires = expires;
                    existing.IsActive = expires.Value == TimeHelper.CurrentTime() ? false : true;
                    conn.Update(existing);
                }
                else
                {
                    existing = new SubUserBan();
                    existing.Id = Guid.NewGuid();
                    existing.BannedBy = bannedBy;
                    existing.Reason = reason;
                    existing.CreatedAt = dateBanned;
                    existing.Expires = expires;
                    existing.SubId = subId;
                    existing.UserId = userId;
                    existing.IsActive = true;

                    conn.Insert(existing);
                }
            });
        }

        public void UnbanUserFromSub(Guid subId, Guid userId)
        {
            _conn.Perform(conn => conn.Update<SubUserBan>(new { IsActive = false }, x => x.SubId == subId
            && x.UserId == userId && x.IsActive == true));
        }

        public void UpdateSubBanForUser(Guid subId, Guid userId, string reason)
        {
            _conn.Perform(conn => conn.Update<SubUserBan>(new { Reason = false }, x => x.SubId == subId
            && x.UserId == userId && x.IsActive == true));
        }

    }
}
