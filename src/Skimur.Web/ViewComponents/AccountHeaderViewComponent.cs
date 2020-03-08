using System;
using Microsoft.AspNetCore.Mvc;
using Skimur.Web.Services;
using Skimur.Web.ViewModels;
using Skimur.Data.ReadModel;

namespace Skimur.Web.ViewComponents
{
    public class AccountHeaderViewComponent : ViewComponent
    {
        private IUserContext _userContext;

        public AccountHeaderViewComponent(IUserContext userContext)
        {
            _userContext = userContext;
        }

        public IViewComponentResult Invoke()
        {
            var model = new AccountHeaderViewModel();
            model.CurrentUser = _userContext.CurrentUser;

            if (model.CurrentUser != null)
            {
                model.UnreadMessages = 0; // temp, todo: get actual unread messages
            }

            return View(model);
        }
    }
}
