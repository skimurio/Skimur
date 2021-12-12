using System;
using Skimur.Data.ReadModel;

namespace Skimur.Web.ViewModels.Subs
{
    public class SubPostsModel
    {
        public SubWrapped Sub { get; set; }

        public PagedList<PostWrapped> Posts { get; set; }

        public PostsSortBy SortBy { get; set; }

        public PostsTimeFilter? TimeFilter { get; set; }

        public bool IsFrontpage { get; set; }

        public bool IsAll { get; set; }
    }
}

