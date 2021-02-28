using System;
using Microsoft.Extensions.DependencyInjection;
using PowerArgs;
using Skimur.Common;
using Skimur.Data.Services;
using Skimur.Data.Models;
using Skimur.Backend.Sql;
using ServiceStack.OrmLite;

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

        [ArgActionMethod, ArgDescription("Toggle admin status for a user")]
        public void ToggleUserAdmin(string username, bool isAdmin)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            // attempt to find the user with the provided username
            var user = connectionProvider.Perform(conn => conn.Single<User>(x => x.UserName.ToLower() == username.ToLower()));
            if (user == null)
            {
                // no user found
                Console.WriteLine("No user with that username found.");
                return;
            }

            // update user admin status
            connectionProvider.Perform(conn => conn.Update<User>(new { IsAdmin = isAdmin }, x => x.Id == user.Id));

            Console.WriteLine($"Successfully updated admin status for the user {user.UserName}");
        }

        [ArgActionMethod, ArgDescription("Resets the password for a specific user")]
        public void ResetPassword(string username)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();
            var passwordManager = SkimurContext.ServiceProvider.GetRequiredService<IPasswordManager>();

            var password = CreateRandomPassword(10);

            // attempt to find the user with the provided username
            var user = connectionProvider.Perform(conn => conn.Single<User>(x => x.UserName.ToLower() == username.ToLower()));
            if (user == null)
            {
                // no user found
                Console.WriteLine("No user with that username found.");
                return;
            }

            // update password
            connectionProvider.Perform(conn => conn.Update<User>(new
            {
                PasswordHash = passwordManager.HashPassword(password)
            }, x => x.Id == user.Id));

            Console.WriteLine($"Successfully updated password for {user.UserName} to {password}");
        }

        private string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }
    }
}
