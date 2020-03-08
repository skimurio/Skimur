using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    public class Report
    {
        /// <summary>
        /// The id of the report
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The time the report was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The user that reported the post or comment
        /// </summary>
        public Guid ReportedBy { get; set; }

        /// <summary>
        /// The reason for the report
        /// </summary>
        public string Reason { get; set; }

        [Alias("ReportedPosts")]
        public class PostReport : Report
        {
            [Alias("Post")]
            public Guid PostId { get; set; }
        }

        [Alias("ReportedComments")]
        public class CommentReport : Report
        {
            [Alias("Comment")]
            public Guid CommentId { get; set; }
        }
    }
}
