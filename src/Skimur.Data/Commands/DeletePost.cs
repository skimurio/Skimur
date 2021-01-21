using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class DeletePost : ICommandReturns<DeletePostResponse>
    {
        public Guid PostId { get; set; }

        public Guid DeletedBy { get; set; }

        public string Reason { get; set; }
    }

    public class DeletePostResponse
    {
        public string Error { get; set; }
    }
}
