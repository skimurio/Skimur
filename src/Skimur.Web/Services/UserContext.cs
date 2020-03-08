using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Skimur.Data.Models;
using Skimur.Data.Services;

namespace Skimur.Web.Services
{
    public class UserContext : IUserContext
    {
        private readonly IMembershipService _membershipService;
        private User _currentUser;
        private IHttpContextAccessor _httpContextAccessor;

        public UserContext(IMembershipService membershipService, IHttpContextAccessor httpContextAccessor)
        {
            _membershipService = membershipService;
            _httpContextAccessor = httpContextAccessor;
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser != null) return _currentUser;

                if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    return null;
                }

                _currentUser = _membershipService.GetUserByUserName(_httpContextAccessor.HttpContext.User.Identity.Name);

                if (_currentUser == null)
                {
                    throw new Exception("Auth cookie exists for an invalid user. UserName=" + _httpContextAccessor.HttpContext.User.Identity.Name);
                }

                return _currentUser;
            }
        }

        public bool? CurrentNsfw
        {
            get
            {
                // anonymous users don't see NSFW content
                // logged in users only see NSFW if preferences say so.
                // If they want to see NSFW, they will see all content (SFW/NSFW).
                return CurrentUser == null 
                    ? false
                    : CurrentUser.ShowNsfw ? (bool?)null : false;
            }
        }

    }
}
