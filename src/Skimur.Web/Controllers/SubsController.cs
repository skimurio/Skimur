using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.ReadModel;
using Skimur.Web.Services;
using Skimur.Web.ViewModels.Subs;

namespace Skimur.Web.Controllers
{
    public class SubsController : BaseController
    {

        private readonly IContextService _contextService;
        private readonly ISubDao _subDao;
        private readonly IUserContext _userContext;

        public SubsController(IContextService contextService,
            ISubDao subDao,
            IUserContext userContext)
        {
            _contextService = contextService;
            _subDao = subDao;
            _userContext = userContext;  
        }

        [Authorize]
        public ActionResult Create()
        {
            var model = new CreateEditSubModel();

            // admins can create default subs
            if (_userContext.CurrentUser.IsAdmin)
            {
                model.IsDefault = false;
            }

            model.InAll = true;

            return View(model);
        }
    }
}
