using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Skimur.Web.Controllers
{
    public class BaseController : Controller
    {
        protected virtual void AddSuccessMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var successMessages = TempData["SuccessMessages"] as List<string>;

            if (successMessages == null)
            {
                successMessages = new List<string>();
                TempData["SuccessMessages"] = successMessages;
            }

            if (!successMessages.Any(x => x.Equals(message))) {
                successMessages.Add(message);
            }
        }


        protected virtual void AddErrorMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            var errorMessages = TempData["ErrorMessages"] as List<string>;

            if (errorMessages == null)
            {
                errorMessages = new List<string>();
                TempData["ErrorMessages"] = errorMessages;
            }

            if (!errorMessages.Any(x => x.Equals(message)))
            {
                errorMessages.Add(message);
            }
        }

        public ActionResult CommonJsonResult(bool success, string error = null)
        {
            return Json(new { success, error });
        }

        public ActionResult CommonJsonResult(string error)
        {
            return !string.IsNullOrEmpty(error) ? CommonJsonResult(false, error) : CommonJsonResult(true);
        }

        public ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
        
    }
}
