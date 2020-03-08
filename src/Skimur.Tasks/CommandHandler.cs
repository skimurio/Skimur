using System;
using Microsoft.Extensions.DependencyInjection;
using PowerArgs;
using Skimur.Common;
using Skimur.Data.Services;
using Skimur.Data.Models;
using Skimur.Backend.Sql;

namespace Skimur.Tasks
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class CommandHandler
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Migration the database (postgres/cassandra) to their latest versions.")]
        public void MigrateDatabase()
        {
            Backend.Postgres.Migrations.Migrations.Run(SkimurContext.ServiceProvider);
        }

        [ArgActionMethod, ArgDescription("Creates the default Skimur admin account with specified password")]
        public void CreateDefaultAdmin(string password)
        {
            var membershipService = SkimurContext.ServiceProvider.GetRequiredService<IMembershipService>();
            var passwordManager = SkimurContext.ServiceProvider.GetRequiredService<IPasswordManager>();

            var user = new User
            {
                UserName = "skimur",
                PasswordHash = passwordManager.HashPassword(password),
                IsAdmin = true
            };

            // create the user
            if (membershipService.InsertUser(user))
            {
                Console.WriteLine($"Created user: {user.UserName}");
            } else
            {
                Console.WriteLine("Failed to create user. User skimur already exists.");
            }
        }

        [ArgActionMethod, ArgDescription("Creates a role with the provided name")]
        public void CreateRole(string name)
        {
            var membershipService = SkimurContext.ServiceProvider.GetRequiredService<IMembershipService>();

            var role = new Role
            {
                Name = name
            };

            // create the role
            if (membershipService.InsertRole(role))
            {
                Console.WriteLine($"Created the role {role.Name} successfully.");
            } else
            {
                Console.WriteLine($"Failed to create a role with the name {role.Name}.");
            }
        }
    }
}
