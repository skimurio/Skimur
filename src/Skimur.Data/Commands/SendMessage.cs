using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Commands
{
    public class SendMessage : ICommandReturns<SendMessageResponse>
    {
        public SendMessage()
        {
            Type = MessageType.Private;
        }

        public Guid AuthorId { get; set; }

        public string AuthorIpAddress { get; set; }

        public Guid? SendAsSub { get; set; }

        public string To { get; set; }

        public Guid? ToUserId { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public MessageType Type { get; set; }

        public Guid? CommentId { get; set; }

        public Guid? PostId { get; set; }
    }

    public class SendMessageResponse
    {
        public string Error { get; set; }

        public Guid MessageId { get; set; }
    }
}
