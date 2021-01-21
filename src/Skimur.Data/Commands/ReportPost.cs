using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class ReportPost : ICommand
    {
        public Guid ReportedBy { get; set; }

        public Guid PostId { get; set; }

        public string Reason { get; set; }
    }
}
