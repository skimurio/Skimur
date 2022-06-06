using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Skimur.Web.Controllers
{
	public class PoliciesController : BaseController
	{
		public ActionResult PrivacyPolicy()
        {
			ViewBag.ManageNavigationKey = "privacypolicy";
			return View();
        }
	}
}

