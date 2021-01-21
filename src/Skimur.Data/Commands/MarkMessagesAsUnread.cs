using System;
using System.Collections.Generic;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class MarkMessagesAsUnread : ICommand
    {
        public Guid UserId { get; set; }

        public List<Guid> Messages { get; set; }
    }
}
