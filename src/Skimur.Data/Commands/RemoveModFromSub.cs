using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class RemoveModFromSub : ICommandReturns<RemoveModFromSubResponse>
    {
        public string SubName { get; set; }

        public Guid? SubId { get; set; }

        public Guid RequestingUser { get; set; }

        public string UsernameToRemove { get; set; }

        public Guid? UserIdToRemove { get; set; }
    }

    public class RemoveModFromSubResponse
    {
        public string Error { get; set; }
    }
}
