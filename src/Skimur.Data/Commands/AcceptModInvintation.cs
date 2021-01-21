using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class AcceptModInvintation : ICommandReturns<AcceptModInvitationResponse>
    {
        public string Username { get; set; }

        public Guid? UserId { get; set; }

        public string SubName { get; set; }

        public Guid? SubId { get; set; }
    }

    public class AcceptModInvitationResponse
    {
        public string Error { get; set; }
    }
}
