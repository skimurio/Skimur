using System;
using Skimur.Data.Services;
using Skimur.Messaging.Handling;
using System.Text.RegularExpressions;
using Skimur.Common.Utils;
using Skimur.Settings;
using Skimur.Data.Settings;
using Skimur.Data.Models;
using Skimur.Markdown.Compiler;
using Skimur.Messaging;
using Skimur.Data.Events;

namespace Skimur.Data.Commands.Handlers
{
    public class SubHandler :
        ICommandHandlerResponse<CreateSub, CreateSubResponse>,
        ICommandHandlerResponse<EditSub, EditSubResponse>,
        ICommandHandlerResponse<SubscribeToSub, SubscribeToSubResponse>,
        ICommandHandlerResponse<UnsubscribeToSub, UnsubscribeToSubResponse>
    {
        private readonly ISubService _subService;
        private readonly IMembershipService _membershipService;
        private readonly IPostService _postService;
        private readonly IEventBus _eventBus;
        private readonly ICommandBus _commandBus;
        private readonly ISubUserBanService _subUserBanService;
        private readonly IModerationService _moderationService;
        private readonly IPermissionService _permissionService;
        private readonly IMarkdownCompiler _markdownCompiler;
        private readonly ISettingsProvider<SubSettings> _subSettings;

        public SubHandler(ISubService subService,
            IMembershipService membershipService,
            IPostService postService,
            IEventBus eventBus,
            ICommandBus commandBus,
            ISubUserBanService subUserBanService,
            IModerationService moderationService,
            IPermissionService permissionService,
            IMarkdownCompiler markdownCompiler,
            ISettingsProvider<SubSettings> subSettings)
        {
            _subService = subService;
            _membershipService = membershipService;
            _postService = postService;
            _eventBus = eventBus;
            _commandBus = commandBus;
            _subUserBanService = subUserBanService;
            _moderationService = moderationService;
            _permissionService = permissionService;
            _markdownCompiler = markdownCompiler;
            _subSettings = subSettings;
        }

        public CreateSubResponse Handle(CreateSub command)
        {
            var response = new CreateSubResponse();

            try
            {
                if (string.IsNullOrEmpty(command.Name))
                {
                    response.Error = "Sub name is required.";
                    return response;
                }

                if (!Regex.IsMatch(command.Name, "^[a-zA-Z0-9]*$"))
                {
                    response.Error = "No spaces or special characters.";
                    return response;
                }

                if (command.Name.Length > 20)
                {
                    response.Error = "The name length is limited to 20 characters.";
                    return response;
                }

                if (string.IsNullOrEmpty(command.Description))
                {
                    response.Error = "Please describe your sub.";
                    return response;
                }

                if (!string.IsNullOrEmpty(command.SubmissionText) && command.SubmissionText.Length > 1000)
                {
                    response.Error = "The sidebar text cannot be greater than 1000 characters";
                    return response;
                }

                if (!string.IsNullOrEmpty(command.SidebarText) && command.SidebarText.Length > 3000)
                {
                    response.Error = "The submission text cannot be greater than 1000 characters";
                    return response;
                }

                // attempt to get the user that is creating the sub
                var user = _membershipService.GetUserById(command.CreatedByUserId);
                if (user == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                // verify account age, allow admins to create subs regardless of age
                var userAccountAge = TimeHelper.CurrentTime() - user.CreatedAt;
                if (!user.IsAdmin && userAccountAge.TotalDays < _subSettings.Settings.MinUserAgeCreateSub)
                {
                    response.Error = "Your account is too new to create a sub.";
                    return response;
                }
                
                // make sure the requested sub doesn't already exist in the database
                if (_subService.GetSubByName(command.Name) != null)
                {
                    response.Error = "The sub already exist.";
                    return response;
                }

                if (command.Name.Equals("random", StringComparison.InvariantCultureIgnoreCase)
                    || command.Name.Equals("all", StringComparison.InvariantCultureIgnoreCase)
                    || StringHelper.IsReservedKeyword(command.Name))
                {
                    response.Error = "The name is invalid.";
                    return response;
                }

                // let's make sure the user creating this sub doesn't already have the maximum number of subs they are modding.
                // admin users bypass this limit
                var moddedSubsForUser = _moderationService.GetSubsModeratoredByUser(user.Id);
                if (!user.IsAdmin && moddedSubsForUser.Count >= _subSettings.Settings.MaximumNumberOfModdedSubs)
                {
                    response.Error = "You can only moderate a maximum of " + _subSettings.Settings.MaximumNumberOfModdedSubs + " subs.";
                    return response;
                }

                // create the sub
                var sub = new Sub
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = TimeHelper.CurrentTime(),
                    Name = command.Name,
                    Description = command.Description,
                    SidebarText = command.SidebarText,
                    SidebarTextFormatted = _markdownCompiler.Compile(command.SidebarText),
                    SubmissionText = command.SubmissionText,
                    SubmissionTextFormatted = _markdownCompiler.Compile(command.SubmissionText),
                    SubType = command.Type,
                    CreatedBy = user.Id,
                    InAll = command.InAll,
                    IsNsfw = command.Nsfw
                };

                // only admins can configure default subs
                if (user.IsAdmin && command.IsDefault.HasValue)
                {
                    sub.IsDefault = command.IsDefault.Value;
                }

                // finally, we insert the sub
                _subService.InsertSub(sub);

                // prepare our response
                response.SubId = sub.Id;
                response.SubName = sub.Name;

                // subscribe creating user to sub and add as moderator with all permissions
                _subService.SubscribeToSub(user.Id, sub.Id);
                _moderationService.AddModToSub(user.Id, sub.Id, ModeratorPermissions.All);
            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

        public EditSubResponse Handle(EditSub command)
        {
            var response = new EditSubResponse();

            try
            {
                var sub = _subService.GetSubByName(command.Name);

                if (sub == null)
                {
                    response.Error = "No sub found with the given name.";
                    return response;
                }

                var user = _membershipService.GetUserById(command.EditedByUserId);

                if (user == null)
                {
                    response.Error = "Invalid user";
                    return response;
                }

                if (!_permissionService.CanUserManageSubConfig(user, sub.Id))
                {
                    response.Error = "You are not allowed to modify this sub.";
                    return response;
                }

                if (string.IsNullOrEmpty(command.Description))
                {
                    response.Error = "Please describe your sub.";
                    return response;
                }

                if (!string.IsNullOrEmpty(command.SidebarText) && command.SidebarText.Length > 1000)
                {
                    response.Error = "The sidebar text cannot be greater than 1000 characters.";
                    return response;
                }

                if (!string.IsNullOrEmpty(command.SubmissionText) && command.SubmissionText.Length > 1000)
                {
                    response.Error = "The submission text cannot be greater than 1000 characters.";
                    return response;
                }

                // only admins can determine if a sub is a defult sub
                if (user.IsAdmin && command.IsDefault.HasValue)
                {
                    sub.IsDefault = command.IsDefault.Value;
                }

                sub.Description = command.Description;
                sub.SidebarText = command.SidebarText;
                sub.SidebarTextFormatted = _markdownCompiler.Compile(command.SidebarText);
                sub.SubmissionText = command.SubmissionText;
                sub.SubmissionTextFormatted = _markdownCompiler.Compile(command.SubmissionText);
                sub.SubType = command.Type;
                sub.InAll = command.InAll;
                sub.IsNsfw = command.Nsfw;

                _subService.UpdateSub(sub);
            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

        public SubscribeToSubResponse Handle(SubscribeToSub command)
        {
            var response = new SubscribeToSubResponse();

            try
            {
                var user = _membershipService.GetUserByUserName(command.Username);
                if (user == null)
                {
                    response.Error = "Invalid user.";
                    return response;
                }

                var sub = command.SubId.HasValue ? _subService.GetSubById(command.SubId.Value) : _subService.GetSubByName(command.SubName);
                if (sub == null)
                {
                    response.Error = "Invalid sub name.";
                    return response;
                }

                // todo: check for private subs

                var subscribedSubs = _subService.GetSubscribedSubsForUser(user.Id);

                // check for maximum number of subscribed subs allowed
                // admins can subscribed to unlimtited subs
                if (!user.IsAdmin && subscribedSubs.Count >= _subSettings.Settings.MaximumNumberOfSubscribedSubs)
                {
                    response.Error = "You can not have more than " + _subSettings.Settings.MaximumNumberOfSubscribedSubs + " subscribed subs. To subscribe to unlimited subs, " +
                        "upgrade to Skimur Pro.";
                    return response;
                }

                if (subscribedSubs.Contains(sub.Id))
                {
                    // already subscribed
                    response.Error = "You are already subscribed to that sub";
                    return response;
                }

                _subService.SubscribeToSub(user.Id, sub.Id);

                _eventBus.Publish(new SubScriptionChanged
                {
                    Subcribed = true,
                    UserId = user.Id,
                    SubId = sub.Id
                });

                response.Success = true;
            }
            catch (Exception ex)
            {
                // todo: log
                response.Error = ex.Message;
            }

            return response;
        }

        public UnsubscribeToSubResponse Handle(UnsubscribeToSub command)
        {
            var response = new UnsubscribeToSubResponse();

            try
            {
                var sub = _subService.GetSubById(command.SubId);
                if (sub == null)
                {
                    response.Error = "Invalid sub.";
                    return response;
                }

                _subService.UnSubscribeToSub(command.UserId, sub.Id);

                _eventBus.Publish(new SubScriptionChanged
                {
                    Unsubscribed = true,
                    UserId = command.UserId,
                    SubId = sub.Id
                });

                response.Success = true;
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
