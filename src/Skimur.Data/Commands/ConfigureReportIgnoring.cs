using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class ConfigureReportIgnoring : ICommand
    {
        public Guid UserId { get; set; }

        public Guid? PostId { get; set; }

        public Guid? CommentId { get; set; }

        public bool IgnoreReports { get; set; }
    }
}
