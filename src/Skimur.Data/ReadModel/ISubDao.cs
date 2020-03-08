using System;
using System.Collections.Generic;
using Skimur.Common.Utils;
using Skimur.Data.Models;

namespace Skimur.Data.ReadModel
{
    public interface ISubDao
    {
        SeekedList<Guid> GetAllSubs(string searchText = null,
            SubsSortBy sortBy = SubsSortBy.Relevance,
            bool? nsfw = null,
            int? skip = null,
            int? take = null);

        List<Guid> GetDefaultSubs();

        List<Guid> GetSubscribedSubsForUser(Guid userId);

        bool IsUserSubscribedToSub(Guid userId, Guid subId);

        Guid? GetRandomSub(bool? nsfw = null);

        Sub GetSubByName(string name);

        List<Sub> GetSubsByIds(List<Guid> ids);

        Sub GetSubById(Guid id);
    }

    public enum SubsSortBy
    {
        Relevance,
        Subscribers,
        New
    }
}
