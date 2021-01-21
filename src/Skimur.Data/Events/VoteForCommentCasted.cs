using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Events
{
    public class VoteForCommentCasted : IEvent
    {
        public Guid CommentId { get; set; }

        public string Username { get; set; }

        public VoteType? PreviousVote { get; set; }

        public VoteType? VoteType { get; set; }
    }
}
