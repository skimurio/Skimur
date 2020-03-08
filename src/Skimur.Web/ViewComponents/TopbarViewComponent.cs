using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Skimur.Web.ViewModels;

namespace Skimur.Web.ViewComponents
{
    public class TopbarViewComponent : ViewComponent
    {
        public TopbarViewComponent()
        {
        }

        public IViewComponentResult Invoke()
        {
            var model = new TopBarViewModel();

            return View(model);
        }
    }
}
