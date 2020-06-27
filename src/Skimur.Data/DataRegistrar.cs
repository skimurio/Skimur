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
            services.AddSingleton<IPasswordManager, PasswordManager>();
            services.AddSingleton<IModerationService, ModerationService>();
            services.AddSingleton<ISubService, SubService>();
            services.AddSingleton<ISubDao, SubDao>();
            services.AddSingleton<IPermissionService, PermissionService>();

        }

        public int Order
        {
            get { return 0; }
        }
    }
}
