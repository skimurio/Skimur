using System;

namespace Skimur.Web.ViewModels.Comments
{
    public class CreateCommentModel
    {

        public CreateCommentModel()
        {
            SendReplies = true;
        }

        public Guid PostId {  get; set; }

        public Guid? ParentId { get; set; }

        public string Body { get; set; }

        public bool SendReplies { get; set; }
    }
}
