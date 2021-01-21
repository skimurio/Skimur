using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Commands
{
    public class ChangeModePermissionsForSub : ICommandReturns<ChangeModPermissionsForSubResponse>
    {
        public string SubName { get; set; }

        public Guid? SubId { get; set; }

        public Guid RequestingUser { get; set; }

        public string UsernameToChange { get; set; }

        public Guid? UserIdToChange { get; set; }

        public ModeratorPermissions Permissions { get; set;  }
    }

    public class ChangeModPermissionsForSubResponse
    {
        public string Error { get; set; }
    }
}
