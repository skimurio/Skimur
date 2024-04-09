using Skimur.Data.Models;
using Skimur.Data.ReadModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Skimur.Web.ViewModels.Posts
{
    public class CreatePostModel
    {
        public string Title { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }

        public PostType PostType { get; set; }

        [Display(Name = "Sub name")]
        public string PostToSub {  get; set; }

        [Display(Name = "Notify replies")]

        public bool NotifyReplies { get; set; }

        public SubWrapped Sub;
    }
}
