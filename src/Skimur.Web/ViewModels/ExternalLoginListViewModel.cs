using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace Skimur.Web.ViewModels
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }

        public string Action { get; set; }        
    }
}
