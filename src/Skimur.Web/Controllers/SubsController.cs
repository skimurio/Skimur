using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.ReadModel;
using Skimur.Web.Services;
using Skimur.Web.ViewModels.Subs;
using Skimur.Messaging;
using Skimur.Data.Commands;
using Skimur.Web.Infrastructure;
using Skimur.Web.ViewModels;
using Skimur.Common.Utils;
using Skimur.Data.Services;
using Skimur.Settings;
using Skimur.Data.Settings;

namespace Skimur.Web.Controllers
{
    public class SubsController : BaseController
    {

        private readonly IContextService _contextService;
        private readonly ISubDao _subDao;
        private readonly IMapper _mapper;
        private readonly ICommandBus _commandBus;
        private readonly IUserContext _userContext;
        private readonly IPostDao _postDao;
        private readonly IVoteDao _voteDao;
        private readonly ICommentDao _commentDao;
        private readonly IPermissionDao _permissionDao;
        private readonly ICommentNodeHierarchyBuilder _commentNodeHierarchyBuilder;
        private readonly ICommentTreeContextBuilder _commentTreeContextBuilder;
        private readonly IPostWrapper _postwrapper;
        private readonly ISubWrapper _subwrapper;
        private readonly ICommentWrapper _commentWrapper;
        private readonly IMembershipService _membershiipService;
        private readonly ISettingsProvider<SubSettings> _subSettings;
        private readonly ISubActivityDao _subActivityDao;
        private readonly IModerationDao _moderationDao;

        public SubsController(IContextService contextService,
            ISubDao subDao,
            IMapper mapper,
            ICommandBus commandBus,
            IUserContext userContext,
            IPostDao postDao,
            IVoteDao voteDao,
            ICommentDao commentDao,
            IPermissionDao permissionDao,
            ICommentNodeHierarchyBuilder commentNodeHierarchyBuilder,
            ICommentTreeContextBuilder commentTreeContextBuilder,
            IPostWrapper postWrapper,
            ISubWrapper subWrapper,
            ICommentWrapper commentWrapper,
            IMembershipService membershipService,
            ISettingsProvider<SubSettings> subSettings,
            ISubActivityDao subActivityDao,
            IModerationDao moderationDao)
        {
            _contextService = contextService;
            _subDao = subDao;
            _mapper = mapper;
            _commandBus = commandBus;
            _userContext = userContext;
            _postDao = postDao;
            _voteDao = voteDao;
            _commentDao = commentDao;
            _permissionDao = permissionDao;
            _commentNodeHierarchyBuilder = commentNodeHierarchyBuilder;
            _commentTreeContextBuilder = commentTreeContextBuilder;
            _postwrapper = postWrapper;
            _subwrapper = subWrapper;
            _commentWrapper = commentWrapper;
            _membershiipService = membershipService;
            _subSettings = subSettings;
            _subActivityDao = subActivityDao;
            _moderationDao = moderationDao;
        }


        public ActionResult Popular(string query, int? pageNumber, int? pageSize)
        {
            ViewBag.NavigationKey = "popular";

            ViewBag.Query = query;

            if (pageNumber == null || pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize == null)
            {
                pageSize = 24;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            if (pageSize < 1)
            {
                pageSize = 1;
            }

            var subs = _subDao.GetAllSubs(query,
                sortBy: SubsSortBy.Subscribers,
                nsfw: _userContext.CurrentNsfw,
                skip: ((pageNumber - 1) * pageSize),
                take: pageSize);

            return View("List", new PagedList<SubWrapped>(_subwrapper.Wrap(subs, _userContext.CurrentUser), pageNumber.Value, pageSize.Value, subs.HasMore));
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
