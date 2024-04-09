using System;

namespace Skimur.Web.ViewModels.Posts
{
    public class EditPostModel
    {
        public Guid PostId { get; set; }

        public string Content { get; set; }
    }
}
