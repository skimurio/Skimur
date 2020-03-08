using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Posts")]
    public class Post
    {
        /// <summary>
        /// The unique id of the post
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The id of the sub this post belongs to
        /// </summary>
        public Guid SubId { get; set; }

        /// <summary>
        /// The id of the user this post belongs to
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The title of the post
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The content of the post
        /// </summary>
        public string Content { get; set; }

        //public int Type { get; set; }

        public PostType PostType { get; set; }

        public PostState PostState { get; set; }

        /// <summary>
        /// The date and time this post was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        public int VoteUpCount { get; set; }

        public int VoteDownCount { get; set; }

        public bool Deleted { get; set; }

        public bool Nsfw { get; set; }

        public bool Mirrored { get; set; }

        public bool InAll { get; set; }

        public bool Sticky { get; set; }

        [CustomField("json")]
        public string Media { get; set; }
    }

    public enum PostType
    {
        Link = 0,
        Text = 1,
        Image = 2
    }

    public enum PostState
    {
        None = 0,
        ModApproved = 1,
        ModRemoved = 2
    }
}
