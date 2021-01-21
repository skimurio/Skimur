using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class ReportComment : ICommand
    {
        public Guid ReportedBy { get; set; }

        public Guid CommentId { get; set; }

        public string Reason { get; set; }
    }
}
