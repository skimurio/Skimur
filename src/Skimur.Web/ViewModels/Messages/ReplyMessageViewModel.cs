using System;

namespace Skimur.Web.ViewModels.Messages
{
    public class ReplyMessageViewModel
    {
        public Guid ReplyToMessage {  get; set; }

        public string Body { get; set; }
    }
}
