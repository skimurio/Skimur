using System;
using Skimur.Messaging;

namespace Skimur.Data.Commands
{
    public class EditPost : ICommandReturns<EditPostResponse>
    {
        public Guid EditedBy { get; set; }

        public Guid PostId { get; set; }

        public string Content { get; set; }

    }

    public class EditPostResponse
    {
        public string Error { get; set; }

        public string Content { get; set; }

        public string ContentFormatted { get; set; }
    }
}
