using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using System.Runtime.InteropServices;
using Skimur.Web.Infrastructure;
using Skimur.Common.Utils;
using Skimur.Common;
using Skimur.Data;
using Skimur.Data.Models;
using Skimur.Web.Infrastructure.Identity;
using Skimur.Web.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Skimur.Web
{
    public class Startup : IRegistrar
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSkimurBase(
                this,
                new DataRegistrar());
                        
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
            });

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
