using System;
using Skimur.Messaging;
using Skimur.Common.Utils;

namespace Skimur.Data.Commands
{
    public class UnbanUserFromSub : ICommandReturns<UnbanUserFromSubResponse>
    {
        public UnbanUserFromSub()
        {
            Expires = TimeHelper.CurrentTime();
        }

        public Guid? UserId { get; set; }

        public string Username { get; set; }

        public Guid? SubId { get; set; }

        public string SubName { get; set; }

        public Guid UnbannedBy { get; set; }

        public DateTime Expires { get; set; }
    }

    public class UnbanUserFromSubResponse
    {
        public string Error { get; set; }
    }
}
