using System;
using Skimur.Messaging;
using Skimur.Data.Models;

namespace Skimur.Data.Commands
{
    public class CastVoteForPost : ICommand
    {
        public DateTime DateCasted { get; set; }

        public Guid UserId { get; set; }

        public Guid PostId { get; set; }

        public VoteType? VoteType { get; set; }

        public string IpAddress { get; set; }
    }
}
