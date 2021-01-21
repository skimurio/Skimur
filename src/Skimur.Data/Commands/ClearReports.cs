using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class ClearReports : ICommand
    {
        public Guid UserId { get; set; }

        public Guid? CommentId { get; set; }

        public Guid? PostId { get; set; }
    }


}
