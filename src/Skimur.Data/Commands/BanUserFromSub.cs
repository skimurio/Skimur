using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class BanUserFromSub : ICommandReturns<BanUserFromSubResponse>
    {

        public Guid? UserId { get; set; }

        public string Username { get; set; }

        public Guid BannedBy { get; set; }

        public Guid? SubId { get; set; }

        public string SubName { get; set; }

        public DateTime DateBanned { get; set; }

        public DateTime? Expires { get; set; }

        // todo: private and public reasons?
        public string Reason { get; set; }
    }

    public class BanUserFromSubResponse
    {
        public string Error { get; set; }
    }
}
