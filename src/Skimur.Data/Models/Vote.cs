using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Votes")]
    public class Vote
    {
        /// <summary>
        /// The id of this vote
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// The date this vote was created
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The user that made the vote
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The post this vote belongs to
        /// </summary>
        public Guid? PostId { get; set; }

        /// <summary>
        /// The comment this vote belongs to
        /// </summary>
        public Guid? CommentId { get; set; }

        /// <summary>
        /// The type of vote this is
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// the date the vote was casted
        /// </summary>
        public DateTime DateCasted { get; set; }

        /// <summary>
        /// The ip address from which the vote was made
        /// </summary>
        public string IpAddress { get; set; }

        [Ignore]
        public VoteType VoteType
        {
            get { return (VoteType) Type; }
            set { Type = (int) value; }
        }
    }

    public enum VoteType
    {
        Up = 1,
        Down = 0
    }
}
