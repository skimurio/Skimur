using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Skimur.Web.Infrastructure;
using Skimur.Common.Utils;
using Skimur.Common;
using Skimur.Data;
using Skimur.Data.Models;
using Skimur.Web.Infrastructure.Identity;
using Skimur.Web.Services;
using Skimur.Web.Services.Impl;

namespace Skimur.Web
{
    public class Startup : IRegistrar
    {

        public Startup(IWebHostEnvironment env)
        {
            // setup configuration sources
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

           services.AddSkimurBase(
                this,
                new DataRegistrar(),
                new MessagingRegistrar());

            //services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                Routes.Register(endpoints);
                /*endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");*/
            });
        }

        public int Order => 0;

        public void Register(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(provider => Configuration);
            services.AddSingleton<IEmailSender, AuthMessageSender>();

            var useProxy = Configuration.GetValue<bool>("Skimur:proxy", false);
            if (useProxy)
            {
                // we are on a unix or linux based system, setup for proxy support
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

                });
            }

            // auth services
            services.AddScoped<ApplicationUserStore>();
            services.AddScoped<IUserStore<User>>(provider => provider.GetService<ApplicationUserStore>());
            services.AddScoped<IRoleStore<Role>>(provider => provider.GetService<ApplicationUserStore>());
            services.AddScoped<IUserRoleStore<User>>(provider => provider.GetService<ApplicationUserStore>());
            services.AddScoped<IPasswordHasher<User>>(provider => provider.GetService<ApplicationUserStore>());
            services.AddScoped<IUserValidator<User>>(provider => provider.GetService<ApplicationUserStore>());
            services.AddScoped<ILookupNormalizer, ApplicationLookupNormalizer>();
            services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            }).AddDefaultTokenProviders();

            var facebookAppId = Configuration["Skimur:Authentication:Facebook:AppId"];
            var facebookAppSecret = Configuration["Skimur:Authentication:Facebook:AppSecret"];
            if (!string.IsNullOrEmpty(facebookAppId) && !string.IsNullOrEmpty(facebookAppSecret))
            {
                services.AddAuthentication().AddFacebook(options =>
                {
                    options.AppId = facebookAppId;
                    options.AppSecret = facebookAppSecret;
                });
            }

            var googleClientId = Configuration["Skimur:Authentication:Google:ClientId"];
            var googleClientSecret = Configuration["Skimur:Authentication:Google:ClientSecret"];
            if (!string.IsNullOrEmpty(googleClientId) && !string.IsNullOrEmpty(googleClientSecret))
            {
                services.AddAuthentication().AddGoogle(options =>
                {
                    options.ClientId = googleClientId;
                    options.ClientSecret = googleClientSecret;
                    options.Scope.Add("https://www.googleapis.com/auth/plus.profile.emails.read");
                });
            }

            services.AddControllersWithViews();

            services.AddScoped<IUserContext, UserContext>();

            services.AddSession();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });
        }

    }
}
