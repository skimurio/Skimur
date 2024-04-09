using System;

namespace Skimur.Web.ViewModels.Comments
{
    public class EditCommentModel
    {
        public Guid CommentId {  get; set; }

        public string Body { get; set; }
    }
}
