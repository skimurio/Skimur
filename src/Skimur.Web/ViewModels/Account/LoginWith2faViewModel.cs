using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Skimur.Web.ViewModels.Account
{
	public class LoginWith2faViewModel
	{
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "AAuthenticator code")]
		public string TwoFactorCode { get; set; }

        [Display(Name = "Remember this device")]
        public bool RememberMachine { get; set; }

        public bool RememberMe { get; set; }
	}
}

