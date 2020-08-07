using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.ReadModel;
using Skimur.Web.Services;
using Skimur.Web.ViewModels.Subs;
using Skimur.Messaging;
using Skimur.Data.Commands;
using Skimur.Web.Infrastructure;

namespace Skimur.Web.Controllers
{
    public class SubsController : BaseController
    {

        private readonly IContextService _contextService;
        private readonly ICommandBus _commandBus;
        private readonly ISubDao _subDao;
        private readonly IUserContext _userContext;

        public SubsController(IContextService contextService,
            ISubDao subDao,
            ICommandBus commandBus,
            IUserContext userContext)
        {
            _contextService = contextService;
            _subDao = subDao;
            _commandBus = commandBus;
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEditSubModel model)
        {
            var response = _commandBus.Send<CreateSub, CreateSubResponse>(new CreateSub
            {
                CreatedByUserId = _userContext.CurrentUser.Id,
                Name = model.Name,
                Description = model.Description,
                SidebarText = model.SidebarText,
                SubmissionText = model.SubmissionText,
                Type = model.SubType,
                IsDefault = model.IsDefault,
                InAll = model.InAll,
                Nsfw = model.Nsfw
            });

            if (!string.IsNullOrEmpty(response.Error))
            {
                ModelState.AddModelError(string.Empty, response.Error);
                return View(model);
            }

            AddSuccessMessage("You sub has been succesfully created.");

            return Redirect("/");
        }
    }
}
