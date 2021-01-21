using System;
using Skimur.Messaging;


namespace Skimur.Data.Commands
{
    public class DeleteComment : ICommandReturns<DeleteCommentResponse>
    {
        public Guid CommentId { get; set; }

        public string Username { get; set; }

        public DateTime DeletedAt { get; set; }
    }

    public class DeleteCommentResponse
    {
        public string Error { get; set; }
    }
}
