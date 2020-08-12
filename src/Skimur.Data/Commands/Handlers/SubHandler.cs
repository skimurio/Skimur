using System;
using Skimur.Data.Services;
using Skimur.Messaging.Handling;
using System.Text.RegularExpressions;
using Skimur.Common.Utils;
using Skimur.Settings;
using Skimur.Data.Settings;
using Skimur.Data.Models;
using Skimur.Markdown.Compiler;

namespace Skimur.Data.Commands.Handlers
{
    public class SubHandler :
        ICommandHandlerResponse<CreateSub, CreateSubResponse>
    {
        private readonly ISubService _subService;
        private readonly IMembershipService _membershipService;
        private readonly IModerationService _moderationService;
        private readonly IMarkdownCompiler _markdownCompiler;
        private readonly ISettingsProvider<SubSettings> _subSettings;

        public SubHandler(ISubService subService,
            IMembershipService membershipService,
            IModerationService moderationService,
            IMarkdownCompiler markdownCompiler,
            ISettingsProvider<SubSettings> subSettings)
        {
            _subService = subService;
            _membershipService = membershipService;
            _moderationService = moderationService;
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
    }
}
