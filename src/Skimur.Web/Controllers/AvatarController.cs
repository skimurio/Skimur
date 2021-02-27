using Microsoft.AspNetCore.Mvc;
using Skimur.Web.Infrastructure;
using Skimur.Web.Services;

namespace Skimur.Web.Controllers
{
    public class AvatarController : BaseController
    {
        private readonly IAvatarService _avatarService;

        public AvatarController(IAvatarService avatarService)
        {
            _avatarService = avatarService;
        }

        public ActionResult Key(string key)
        {
            var avatarStream = _avatarService.GetAvatarStream(key);

            if (avatarStream != null)
            {
                return File(avatarStream, "image/jpeg");
            }

            throw new NotFoundException();
        }
    }
}
