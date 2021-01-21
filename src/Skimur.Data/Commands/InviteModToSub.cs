using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Commands
{
    public class InviteModToSub : ICommandReturns<InviteModToSubResponse>
    {
        public Guid RequestingUserId { get; set; }

        public string Username { get; set; }

        public Guid? UserId { get; set; }

        public string SubName { get; set; }

        public Guid? SubId { get; set; }

        public ModeratorPermissions Permissions { get; set; }
    }

    public class InviteModToSubResponse
    {
        public string Error { get; set; }
    }
}
