using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Skimur.Data.Services;
using Skimur.Data.ReadModel;
using Skimur.Data.Commands;
using Skimur.Web.Infrastructure;
using Skimur.Web.Services;
using Skimur.Web.ViewModels;

namespace Skimur.Web.Controllers
{
    public class PostsController : BaseController
    {
        private readonly ISubDao _subDao;
        private readonly IUserContext _userContext;
        private readonly IContextService _contextService;
        private static Guid? _annoncementSubId;

        public PostsController(ISubDao subDao,
            IUserContext userContext,
            IContextService contextService)
        {
            _subDao = subDao;
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

            
            return View();
        }
    }
}
