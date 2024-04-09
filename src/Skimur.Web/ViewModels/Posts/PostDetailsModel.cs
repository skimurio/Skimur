using Skimur.Data.ReadModel;
using Skimur.Web.ViewModels.Comments;

namespace Skimur.Web.ViewModels.Posts
{
    public class PostDetailsModel
    {
        public PostWrapped Post { get; set; }

        public SubWrapped Sub { get; set; }

        public CommentListModel Comments { get; set; }

        public bool ViewingSpecificComment { get; set; }
    }
}
