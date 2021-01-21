using Skimur.Data.Services;
using Skimur.Data.Services.Impl;

namespace Skimur.Data.ReadModel.Impl
{
    public class PermissionDao
        // this class temporarily implements the service, until we implement the proper read-only layer
        : PermissionService, IPermissionDao
    {
        public PermissionDao(IModerationService moderationService) : base(moderationService)
        {
        }
    }
}
