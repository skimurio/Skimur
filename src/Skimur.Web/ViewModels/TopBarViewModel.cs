using System;
using System.Collections.Generic;
using System.Linq;

namespace Skimur.Web.ViewModels
{
    public class TopBarViewModel
    {
        public List<string> SubscribedSubs { get; set; }
        public List<string> DefaultSubs { get; set; }

        public TopBarViewModel()
        {
            SubscribedSubs = new List<string>();
            DefaultSubs = new List<string>();
        }
    }
}
