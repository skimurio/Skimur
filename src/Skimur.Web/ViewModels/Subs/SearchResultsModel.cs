using Skimur.Data.ReadModel;

namespace Skimur.Web.ViewModels.Subs
{
    public class SearchResultsModel
    {
        public string Query { get; set; }

        public SubWrapped LimitingToSub { get; set; }

        public PagedList<SubWrapped> Subs { get; set; }

        public PagedList<PostWrapped> Posts {  get; set; }

        public PostsSearchSortBy SortBy { get; set; }

        public PostsTimeFilter? TimeFilter { get; set; }

        public SearchResultType? ResultType { get; set; }
    }
}
