using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class UnsubscribeToSub : ICommandReturns<UnsubscribeToSubResponse>
    {
        public Guid UserId { get; set; }

        public string SubName { get; set; }

        public Guid SubId { get; set; }
    }

    public class UnsubscribeToSubResponse
    {
        public string Error { get; set; }

        public bool Success { get; set; }
    }
}
