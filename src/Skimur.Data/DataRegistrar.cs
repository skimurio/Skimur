using Microsoft.Extensions.DependencyInjection;
using Skimur.Common.Utils;
using Skimur.Data.Services;
using Skimur.Data.Services.Impl;
using Skimur.Data.ReadModel;
using Skimur.Data.ReadModel.Impl;

namespace Skimur.Data
{
    public class DataRegistrar : IRegistrar
    {
        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IMembershipService, MembershipService>();
            services.AddSingleton<IMembershipDao, MembershipDao>();
            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<IModeratorInviteWrapper, ModeratorInviteWrapper>();
            services.AddSingleton<IModerationInviteService, ModerationInviteService>();
            services.AddSingleton<IKarmaService, KarmaService>();
            services.AddSingleton<IKarmaDao, KarmaDao>();
            services.AddSingleton<IModerationService, ModerationService>();
            services.AddSingleton<IModerationDao, ModerationDao>();
            services.AddSingleton<IReportService, ReportService>();
            services.AddSingleton<IReportDao, ReportDao>();
            services.AddSingleton<ISubActivityService, SubActivityService>();
            services.AddSingleton<ISubActivityDao, SubActivityDao>();
            services.AddSingleton<ISubUserBanService, SubUserBanService>();
            services.AddSingleton<ISubUserBanDao, SubUserBanDao>();
            services.AddSingleton<ISubService, SubService>();
            services.AddSingleton<ISubDao, SubDao>();
            services.AddSingleton<IPostService, PostService>();
            services.AddSingleton<IPostDao, PostDao>();
            services.AddSingleton<IVoteService, VoteService>();
            services.AddSingleton<IVoteDao, VoteDao>();
            services.AddSingleton<ICommentService, CommentService>();
            services.AddSingleton<ICommentDao, CommentDao>();
            services.AddSingleton<IPermissionService, PermissionService>();
            services.AddSingleton<IPermissionDao, PermissionDao>();
            services.AddSingleton<IMessageService, MessageService>();
            services.AddSingleton<IMessageDao, MessageDao>();
            services.AddSingleton<ICommentTreeBuilder, CommentTreeBuilder>();
            services.AddSingleton<ICommentTreeContextBuilder, CommentTreeContextBuilder>();
            services.AddSingleton<ICommentNodeHierarchyBuilder, CommentNodeHierarchyBuilder>();
            services.AddSingleton<ICommentWrapper, CommentWrapper>();
            services.AddSingleton<IPostWrapper, PostWrapper>();
            services.AddSingleton<ISubWrapper, SubWrapper>();
            services.AddSingleton<ISubUserBanWrapper, SubUserBanWrapper>();
            services.AddSingleton<IMessageWrapper, MessageWrapper>();
            services.AddSingleton<IModeratorWrapper, ModeratorWrapper>();
            services.AddSingleton<ISubCssService, SubCssService>();
            services.AddSingleton<ISubCssDao, SubCssDao>();
            services.AddSingleton<IPostThumbnailService, PostThumbnailService>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
