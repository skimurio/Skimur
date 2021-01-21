using Microsoft.Extensions.DependencyInjection;
using Skimur.Data.Commands;
using Skimur.Data.Commands.Handlers;
using Skimur.Messaging.Handling;
using Skimur.Common.Utils;
using Skimur.Data.Events.Handlers;

namespace Skimur.Data
{
    public class MessagingRegistrar : IRegistrar
    {
        public void Register(IServiceCollection services)
        {
            // email commands
            services.AddTransient<ICommandHandler<SendEmail>, EmailHandler>();

            // sub commands
            services.AddTransient<ICommandHandlerResponse<CreateSub, CreateSubResponse>, SubHandler>();
            services.AddTransient<ICommandHandlerResponse<EditSub, EditSubResponse>, SubHandler>();
            services.AddTransient<ICommandHandlerResponse<SubscribeToSub, SubscribeToSubResponse>, SubHandler>();
            services.AddTransient<ICommandHandlerResponse<UnsubscribeToSub, UnsubscribeToSubResponse>, SubHandler>();

            // ban command handlers
            services.AddTransient<ICommandHandlerResponse<BanUserFromSub, BanUserFromSubResponse>, SubBanning>();
            services.AddTransient<ICommandHandlerResponse<UnbanUserFromSub, UnbanUserFromSubResponse>, SubBanning>();
            services.AddTransient<ICommandHandlerResponse<UpdateUserSubBan, UpdateUserSubBanResponse>, SubBanning>();

            // vote command handlers
            services.AddTransient<ICommandHandler<CastVoteForPost>, VoteHandler>();
            services.AddTransient<ICommandHandler<CastVoteForComment>, VoteHandler>();

            // comment command handlers
            services.AddTransient<ICommandHandlerResponse<CreateComment, CreateCommentResponse>, CommentHandler>();
            services.AddTransient<ICommandHandlerResponse<EditComment, EditCommentResponse>, CommentHandler>();
            services.AddTransient<ICommandHandlerResponse<DeleteComment, DeleteCommentResponse>, CommentHandler>();

            // generator handlers

            // events
            services.AddTransient<ScoringAndSortingEventHandler>();
            services.AddTransient<ReplyNotificationEventHandler>();
            services.AddTransient<SubEventHandler>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
