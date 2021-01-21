﻿using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class EditComment : ICommandReturns<EditCommentResponse>
    {

        public Guid EditedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Guid CommentId { get; set; }

        public string Body;
        
    }

    public class EditCommentResponse
    {
        public string Error { get; set; }

        public string Body { get; set; }

        public string FormattedBody { get; set; }
    }
}
