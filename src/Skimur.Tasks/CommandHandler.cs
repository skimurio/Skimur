using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using PowerArgs;
using Skimur.Common;
using Skimur.Data.Services;
using Skimur.Data.Models;
using Skimur.Data.Models.Wiki;
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

        [ArgActionMethod, ArgDescription("Update kudos for all users")]
        public void UpdateKudos()
        {
            var membershipService = SkimurContext.ServiceProvider.GetRequiredService<IMembershipService>();
            var karmaService = SkimurContext.ServiceProvider.GetRequiredService<IKarmaService>();
            var postService = SkimurContext.ServiceProvider.GetRequiredService<IPostService>();
            var commentService = SkimurContext.ServiceProvider.GetRequiredService<ICommentService>();

            int currentIndex = 0;
            int pageSize = 10;

            var users = membershipService.GetAllUsers(currentIndex, pageSize);

            // loop through users while count is greater than 0
            while (users.Count > 0)
            {
                foreach (var user in users)
                {
                    // post karma mapping for user
                    var postKudos = new Dictionary<Guid, int>();

                    // get all posts for user
                    var userPosts = postService.GetPosts(userId: user.Id, hideRemovedPosts: false, showDeleted: true)
                        .Select(postId => postService.GetPostById(postId))
                        .Where(post => post != null);

                    // loop through user posts
                    foreach (var post in userPosts)
                    {
                        // add the post sub if it is not in our mapping
                        if (!postKudos.ContainsKey(post.SubId))
                        {
                            postKudos.Add(post.SubId, 0);
                        }

                        // calculate kudos based on post up and down vote count
                        postKudos[post.SubId] = postKudos[post.SubId] + (post.VoteUpCount - post.VoteDownCount);
                    }

                    // comment karma mapping for user
                    var commentKudos = new Dictionary<Guid, int>();

                    // get all comments for user
                    var userComments = commentService.GetCommentsForUser(user.Id)
                        .Select(commentId => commentService.GetCommentById(commentId))
                        .Where(comment => comment != null);

                    // loop through user comments
                    foreach (var comment in userComments)
                    {
                        // add the comment sub if it is not in our mapping
                        if (!commentKudos.ContainsKey(comment.SubId))
                        {
                            commentKudos.Add(comment.SubId, 0);
                        }

                        // calculate kudos based on comment up and down vote count
                        commentKudos[comment.SubId] = commentKudos[comment.SubId] + (comment.VoteUpCount - comment.VoteDownCount);
                    }

                    // delete all karma for user
                    karmaService.DeleteAllKarmaForUser(user.Id);

                    if (postKudos.Count > 0)
                    {
                        foreach (var subId in postKudos.Keys)
                        {
                            karmaService.AdjustKarma(user.Id, subId, KarmaType.Post, postKudos[subId]);
                        }
                    }

                    if (commentKudos.Count > 0)
                    {
                        foreach (var subId in commentKudos.Keys)
                        {
                            karmaService.AdjustKarma(user.Id, subId, KarmaType.Comment, commentKudos[subId]);
                        }
                    }
                }

                currentIndex += users.Count;
                users = membershipService.GetAllUsers(currentIndex, pageSize);
            }
        }

        [ArgActionMethod, ArgDescription("Creates the default Skimur system account")]
        public void CreateDefaultSystemUser()
        {
            var membershipService = SkimurContext.ServiceProvider.GetRequiredService<IMembershipService>();
            var passwordManager = SkimurContext.ServiceProvider.GetRequiredService<IPasswordManager>();
            var password = CreateRandomPassword();

            var user = new User
            {
                UserName = "skimur",
                PasswordHash = passwordManager.HashPassword(password),
                IsAdmin = true,
                IsSystem = true
            };

            // create the user
            if (membershipService.InsertUser(user))
            {
                Console.WriteLine($"Created system user {user.UserName} with password: {password}");
            }
            else
            {
                Console.WriteLine("Failed to create system user. User skimur already exists.");
            }
        }

        [ArgActionMethod, ArgDescription("Creates a user account with specified password")]
        public void CreateUser(string username, string password)
        {
            var membershipService = SkimurContext.ServiceProvider.GetRequiredService<IMembershipService>();
            var passwordManager = SkimurContext.ServiceProvider.GetRequiredService<IPasswordManager>();

            var user = new User
            {
                UserName = username,
                PasswordHash = passwordManager.HashPassword(password)
            };

            // create the user
            if (membershipService.InsertUser(user))
            {
                Console.WriteLine($"Created user: {user.UserName}");
            }
            else
            {
                Console.WriteLine("Failed to create user. User already exists.");
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

        public void DeleteSub(string subName)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            var sub = connectionProvider.Perform(conn => conn.Single<Sub>(x => x.Name.ToLower() == subName.ToLower()));
            if (sub == null)
            {
                Console.WriteLine("No sub with that name.");
                return;
            }

            var subPosts = connectionProvider.Perform(conn => conn.Select<Post>(x => x.SubId == sub.Id));
            if (subPosts.Count > 0)
            {
                foreach (var post in subPosts)
                {
                    DeletePost(post.Id);
                }
            }

            var subComments = connectionProvider.Perform(conn => conn.Select<Comment>(x => x.SubId == sub.Id));
            if (subPosts.Count > 0)
            {
                foreach (var comment in subComments)
                {
                    DeleteComment(comment.Id);
                }
            }

            var subWikiPages = connectionProvider.Perform(conn => conn.Select<Page>(x => x.SubId == sub.Id));
            if (subWikiPages.Count > 0)
            {
                foreach (var wikiPage in subWikiPages)
                {
                    DeleteWikiPage(wikiPage.Id);
                }
            }

            connectionProvider.Perform(conn => conn.Delete<Moderator>(x => x.SubId == sub.Id));
            connectionProvider.Perform(conn => conn.Delete<ModeratorInvite>(x => x.SubId == sub.Id));
            connectionProvider.Perform(conn => conn.Delete<SubCss>(x => x.SubId == sub.Id));
            connectionProvider.Perform(conn => conn.Delete<SubUserBan>(x => x.SubId == sub.Id));
            connectionProvider.Perform(conn => conn.Delete<Sub>(x => x.Id == sub.Id));

            Console.WriteLine("Successfully deleted sub.");
        }

        [ArgActionMethod, ArgDescription("Delete comment")]
        public void DeleteComment(Guid commentId)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            var comment = connectionProvider.Perform(conn => conn.Single<Comment>(x => x.Id == commentId));
            if (comment == null)
            {
                Console.WriteLine("No comment with that id.");
                return;
            }

            connectionProvider.Perform(conn => conn.Delete<Vote>(x => x.CommentId == comment.Id));
            connectionProvider.Perform(conn => conn.Delete<Report.CommentReport>(x => x.CommentId == comment.Id));

            // delete any messages associated with this comment
            var commentMessages = connectionProvider.Perform(conn => conn.Select<Message>(x => x.CommentId == comment.Id));
            if (commentMessages.Count > 0)
            {
                foreach (var message in commentMessages)
                {
                    DeleteMessageThread(message.Id);
                }
            }

            // delete child comments
            var childComments = connectionProvider.Perform(conn => conn.Select<Comment>(x => x.ParentId == comment.Id));
            if (childComments.Count > 0)
            {
                foreach (var childComment in childComments)
                {
                    DeleteComment(childComment.Id);   
                }
            }

            connectionProvider.Perform(conn => conn.Delete<Comment>(x => x.Id == comment.Id));
        }

        [ArgActionMethod, ArgDescription("Delete post")]
        public void DeletePost(Guid postId)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            var post = connectionProvider.Perform(conn => conn.Single<Post>(x => x.Id == postId));
            if (post == null)
            {
                Console.WriteLine("No post with that id.");
                return;
            }

            connectionProvider.Perform(conn => conn.Delete<Vote>(x => x.PostId == post.Id));
            connectionProvider.Perform(conn => conn.Delete<Report.PostReport>(x => x.PostId == post.Id));

            // delete any messages associated with this comment
            var postMessages = connectionProvider.Perform(conn => conn.Select<Message>(x => x.PostId == post.Id));
            if (postMessages.Count > 0)
            {
                foreach (var message in postMessages)
                {
                    DeleteMessageThread(message.Id);
                }
            }

            // delete post comments
            var comments = connectionProvider.Perform(conn => conn.Select<Comment>(x => x.PostId == post.Id));
            if (comments.Count > 0)
            {
                foreach (var comment in comments)
                {
                    DeleteComment(comment.Id);
                }
            }

            connectionProvider.Perform(conn => conn.Delete<Post>(x => x.Id == post.Id));
        }

        [ArgActionMethod, ArgDescription("Delete message")]
        public void DeleteMessageThread(Guid messageId)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            var message = connectionProvider.Perform(conn => conn.Single<Message>(x => x.Id == messageId));
            if (message == null)
            {
                Console.WriteLine("No message with that id.");
                return;
            }

            if (message.ParentId.HasValue)
            {
                connectionProvider.Perform(conn => conn.Delete<Message>(x => x.ParentId == message.ParentId || x.Id == message.ParentId));
            }

            connectionProvider.Perform(conn => conn.Delete<Message>(x => x.Id == message.Id));
        }

        [ArgActionMethod, ArgDescription("Delete Wiki Page")]
        public void DeleteWikiPage(Guid wikiPageId)
        {
            var connectionProvider = SkimurContext.ServiceProvider.GetRequiredService<IDbConnectionProvider>();

            var wikiPage = connectionProvider.Perform(conn => conn.Single<Page>(x => x.Id == wikiPageId));
            if (wikiPage == null)
            {
                Console.WriteLine("No wiki page with that id.");
                return;
            }

            // delete any revision history the page may have
            connectionProvider.Perform(conn => conn.Delete<PageRevision>(x => x.PageId == wikiPage.Id));
            connectionProvider.Perform(conn => conn.Delete<Page>(x => x.Id == wikiPage.Id));
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
