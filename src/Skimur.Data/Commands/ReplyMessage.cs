using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class ReplyMessage : ICommandReturns<ReplyMessageResponse>
    {
        public Guid ReplyToMessageId { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorIpAddress { get; set; }

        public string Body { get; set; }
    }

    public class ReplyMessageResponse
    {
        public string Error { get; set; }

        public Guid MessageId { get; set; }
    }
}
