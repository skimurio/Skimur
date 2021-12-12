using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.Services;
using Skimur.Data.ReadModel;
using Skimur.Data.Commands;
using Skimur.Messaging;
using Skimur.Web.Infrastructure;
using Skimur.Web.Services;
using Skimur.Web.ViewModels;
using Skimur.Web.ViewModels.Subs;

namespace Skimur.Web.Controllers
{
    public class PostsController : BaseController
    {
        private readonly ISubDao _subDao;
        private readonly ISubWrapper _subWrapper;
        private readonly IPostDao _postDao;
        private readonly IPostWrapper _postWrapper;
        private readonly ICommandBus _commandBus;
        private readonly IUserContext _userContext;
        private readonly IContextService _contextService;
        private static Guid? _annoncementSubId;

        public PostsController(ISubDao subDao,
            ISubWrapper subWrapper,
            IPostDao postDao,
            IPostWrapper postWrapper,
            ICommandBus commandBus,
            IUserContext userContext,
            IContextService contextService)
        {
            _subDao = subDao;
            _subWrapper = subWrapper;
            _postDao = postDao;
            _postWrapper = postWrapper;
            _commandBus = commandBus;
            _userContext = userContext;
            _contextService = contextService;
        }

        public ActionResult Frontpage(PostsSortBy? sort, PostsTimeFilter? time, int? pageNumber, int? pageSize)
        {
            var subs = _contextService.GetSubscribedSubIds();

            // if the user is not subscribed to any subs, show the default content.
            if (subs.Count == 0)
            {
                subs = _subDao.GetDefaultSubs();
            }

            if (sort == null)
            {
                sort = PostsSortBy.Hot;
            }

            if (time == null)
            {
                time = PostsTimeFilter.All;
            }

            if (pageNumber == null || pageNumber < 1)
            {
                pageNumber = 1;
            }

            if (pageSize == null)
            {
                pageSize = 25;
            }
            if (pageSize > 100)
            {
                pageSize = 100;
            }
            if (pageSize < 1)
            {
                pageSize = 1;
            }
            
            // anonymous users don't see NSFW content.
            // logged in users only see NSFW if preferences say so.
            // If they want to see NSFW, they will see all content (SFW/NSFW).
            var postIds = _postDao.GetPosts(subs,
                sortby: sort.Value,
                timeFilter: time.Value,
                nsfw: _userContext.CurrentUser == null ? false : _userContext.CurrentUser.ShowNsfw ? (bool?)null : false,
                skip: (pageNumber - 1) * pageSize,
                take: pageSize);

            var model = new SubPostsModel();
            model.SortBy = sort.Value;
            model.TimeFilter = time;

            // maybe the user hasn't subscribed to any subs?
            if (subs.Any())
            {
                model.Posts = new PagedList<PostWrapped>(_postWrapper.Wrap(postIds, _userContext.CurrentUser), pageNumber.Value, pageSize.Value, postIds.HasMore);
            }

            return View("Posts", model);
        }
    }
}
