using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Skimur.Web.ViewModels.Manage
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        // todo: find other logins
        //public IList<AuthenticationDescription> OtherLogins { get; set; }

        public bool IsPasswordSet { get; set; }
    }
}
