using System;
using Skimur.Messaging.Handling;
using Skimur.Data.Models;
using Skimur.Data.Services;

namespace Skimur.Data.Commands.Handlers
{
    public class SubBanning :
        ICommandHandlerResponse<BanUserFromSub, BanUserFromSubResponse>,
        ICommandHandlerResponse<UnbanUserFromSub, UnbanUserFromSubResponse>,
        ICommandHandlerResponse<UpdateUserSubBan, UpdateUserSubBanResponse>
    {

        private readonly IPermissionService _permissionService;
        private readonly IMembershipService _membershipService;
        private readonly ISubService _subService;
        private readonly ISubUserBanService _subUserBanService;

        public SubBanning(IPermissionService permissionService,
            IMembershipService membershipService,
            ISubService subService,
            ISubUserBanService subUserBanService)
        {
            _permissionService = permissionService;
            _membershipService = membershipService;
            _subService = subService;
            _subUserBanService = subUserBanService;
        }

        public BanUserFromSubResponse Handle(BanUserFromSub command)
        {
            var response = new BanUserFromSubResponse();

            try
            {
                var user = command.UserId.HasValue ? _membershipService.GetUserById(command.UserId.Value) : _membershipService.GetUserByUserName(command.Username);

                if (user == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var bannedBy = _membershipService.GetUserById(command.BannedBy);

                if (bannedBy == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var sub = command.SubId.HasValue
                    ? _subService.GetSubById(command.SubId.Value)
                    : _subService.GetSubByName(command.SubName);

                if (sub == null)
                {
                    response.Error = "Invalid sub.";
                    return response;
                }

                if (!_permissionService.CanUserManageSubAccess(bannedBy, sub.Id))
                {
                    response.Error = "You are not authorized to ban.";
                    return response;
                }

                _subUserBanService.BanUserFromSub(sub.Id, user.Id, command.DateBanned, command.BannedBy, command.Reason, command.Expires);
            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

        public UnbanUserFromSubResponse Handle(UnbanUserFromSub command)
        {
            var response = new UnbanUserFromSubResponse();

            try
            {
                var user = command.UserId.HasValue
                    ? _membershipService.GetUserById(command.UserId.Value)
                    : _membershipService.GetUserByUserName(command.Username);

                if (user == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var unbannedBy = _membershipService.GetUserById(command.UnbannedBy);

                if (unbannedBy == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var sub = command.SubId.HasValue
                    ? _subService.GetSubById(command.SubId.Value)
                    : _subService.GetSubByName(command.SubName);

                if (sub == null)
                {
                    response.Error = "Invalid sub.";
                    return response;
                }

                if (!_permissionService.CanUserManageSubAccess(unbannedBy, sub.Id))
                {
                    response.Error = "You are not authorized to unban.";
                    return response;
                }

                _subUserBanService.UnbanUserFromSub(sub.Id, user.Id);
            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

        public UpdateUserSubBanResponse Handle(UpdateUserSubBan command)
        {
            var response = new UpdateUserSubBanResponse();

            try
            {

                var user = command.UserId.HasValue
                    ? _membershipService.GetUserById(command.UserId.Value)
                    : _membershipService.GetUserByUserName(command.Username);

                if (user == null)
                {
                    response.Error = "Invalid User.";
                    return response;
                }

                var updatedBy = _membershipService.GetUserById(command.UpdatedBy);

                if (updatedBy == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var sub = command.SubId.HasValue
                    ? _subService.GetSubById(command.SubId.Value)
                    : _subService.GetSubByName(command.SubName);

                if (sub == null)
                {
                    response.Error = "Invalid sub.";
                    return response;
                }

                if (!_permissionService.CanUserManageSubAccess(updatedBy, sub.Id))
                {
                    response.Error = "You are not authorized to manage bans.";
                    return response;
                }

                _subUserBanService.UpdateSubBanForUser(sub.Id, user.Id, command.ReasonPrivate);
                response.UserId = user.Id;

            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

    }
}
