using Skimur.Data.Models;
using Skimur.Data.ReadModel;
using System;
using System.Collections.Generic;

namespace Skimur.Web.ViewModels.Messages
{
    public class InboxViewModel
    {
        public InboxType InboxType { get; set; }

        public PagedList<MessageWrapped> Messages { get; set; }

        public List<Guid> ModeratorMailForSubs { get; set; }

        public Sub Sub { get; set; }

        public bool IsModerator { get; set; } 
    }
}
