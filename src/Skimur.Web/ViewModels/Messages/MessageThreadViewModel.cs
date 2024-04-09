using Skimur.Data.ReadModel;
using System.Collections.Generic;

namespace Skimur.Web.ViewModels.Messages
{
    public class MessageThreadViewModel
    {
        public MessageThreadViewModel() 
        {
            Messages = new List<MessageWrapped>();
        }

        public bool IsModerator { get; set; }

        public MessageWrapped FirstMessage {  get; set; }

        public MessageWrapped ContextMessage { get; set; }

        public List<MessageWrapped> Messages { get; set; }
    }
}
