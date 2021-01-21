using System;
using Skimur.Messaging;

namespace Skimur.Data.Events
{
    public class CommentCreated : IEvent
    {
        public Guid CommentId { get; set; }

        public Guid PostId { get; set; }
    }
}
