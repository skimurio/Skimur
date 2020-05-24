using System;
using Skimur.Data.Services;
using Skimur.Web.Services;

namespace Skimur.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IMembershipService _membershipService;
        private readonly IUserContext _userContext;

        public UsersController(IMembershipService membershipService,
            IUserContext userContext)
        {
            _membershipService = membershipService;
            _userContext = userContext;
        }
    }
}
