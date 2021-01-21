using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class CreateComment : ICommandReturns<CreateCommentResponse>
    {
        public Guid PostId { get; set; }

        public Guid? ParentId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorUsername { get; set; }

        public string AuthorIpAddress { get; set; }

        public string Body { get; set; }

        public bool SendReplies { get; set; }
        
    }

    public class CreateCommentResponse
    {
        public string Error { get; set; }

        public Guid? CommentId { get; set; }
    }
}
