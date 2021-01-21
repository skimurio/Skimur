using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class SubscribeToSub : ICommandReturns<SubscribeToSubResponse>
    {
        public string Username { get; set; }

        public string SubName { get; set; }

        public Guid? SubId { get; set; }
    }

    public class SubscribeToSubResponse
    {
        public string Error { get; set; }

        public bool Success { get; set; }
    }
}
