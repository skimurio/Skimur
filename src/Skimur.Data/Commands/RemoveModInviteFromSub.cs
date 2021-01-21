using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class RemoveModInviteFromSub : ICommandReturns<RemoveModInviteFromSubResponse>
    {
        public Guid RequestingUserId { get; set; }

        public string Username { get; set; }

        public Guid? UserId { get; set; }

        public string SubName { get; set; }

        public Guid? SubId { get; set; }
    }

    public class RemoveModInviteFromSubResponse
    {
        public string Error { get; set; }
    }
}
