using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class GenerateThumbnailForPost : ICommand
    {
        public Guid PostId { get; set; }

        public bool Force { get; set; }
    }
}
