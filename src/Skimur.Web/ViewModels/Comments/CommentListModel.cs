using Skimur.Data.ReadModel;
using System.Collections.Generic;

namespace Skimur.Web.ViewModels.Comments
{
    public class CommentListModel
    {
        public CommentSortBy SortBy { get; set; }

        public List<ICommentNode> CommentNodes { get; set; }
    }
}
