using System;
using System.Collections.Generic;
using Skimur.Data.Models;
using Skimur.Common.Utils;

namespace Skimur.Data.Services
{
    public interface IMembershipService
    {
        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        bool UpdateUser(User user);

        /// <summary>
        /// Inserts a new user
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns></returns>
        bool InsertUser(User user);

        /// <summary>
        /// Deletes the user
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns></returns>
        bool DeleteUser(Guid userId);

        /// <summary>
        /// Gets the user by the identifier
        /// </summary>
        /// <param name="userId">The user identifyer</param>
        /// <returns></returns>
        User GetUserById(Guid userId);

        /// <summary>
        /// Gets the user by username
        /// </summary>
        /// <param name="userName">The username</param>
        /// <returns></returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Gets the user by email
        /// </summary>
        /// <param name="email">The email address</param>
        /// <returns></returns>
        User GetUserByEmail(string email);

        /// <summary>
        /// Get a list of users by ids
        /// </summary>
        /// <param name="ids">The list of ids to search for</param>
        /// <returns></returns>
        List<User> GetUsersByIds(List<Guid> ids);

        /// <summary>
        /// All the users in the system
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        SeekedList<User> GetAllUsers(int? skip = null, int? take = null);

        /// <summary>
        /// Determines if the given username is a valid one.
        /// Does not check if it is already in use
        /// </summary>
        /// <param name="userName">The username</param>
        /// <returns></returns>
        bool IsUserNameValid(string userName);

        /// <summary>
        /// Determines if the email is a valid one.
        /// Does not check if it is already in use
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <returns></returns>
        bool IsEmailValid(string email);

        /// <summary>
        /// Determines if the given password is a valid one.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        bool IsPasswordValid(string password);

        /// <summary>
        /// Can the given user id change their email to the provided email?
        /// null userId implies new user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        bool CanChangeEmail(Guid userid, string email);

        /// <summary>
        /// Retrieves a Role from the system.
        /// </summary>
        /// <param name="id">Id of the role</param>
        /// <returns></returns>
        Role GetRoleById(Guid id);

        /// <summary>
        /// Get a role by name
        /// </summary>
        /// <param name="roleName">Name of the role</param>
        /// <returns></returns>
        Role GetRoleByName(string roleName);

        /// <summary>
        /// Updates a pre-existing role,
        /// replacing the Name and Description
        /// </summary>
        /// <param name="role">The role to be updated</param>
        /// <returns></returns>
        bool UpdateRole(Role role);

        /// <summary>
        /// Creates a membership role in the system.
        /// </summary>
        /// <param name="role">The role to be created</param>
        /// <returns></returns>
        bool InsertRole(Role role);

        /// <summary>
        /// Removes a Role from the system.
        /// </summary>
        /// <param name="roleId">The role identifier</param>
        /// <returns></returns>
        bool DeleteRole(Guid roleId);

        /// <summary>
        /// Get all roles from the system
        /// </summary>
        /// <returns></returns>
        IList<Role> GetRoles();

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="roleId">The role identifier</param>
        /// <returns></returns>
        bool AddUserToRole(Guid userId, Guid roleId);

        /// <summary>
        /// Adds a user to a role
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="roleId">The role identifier</param>
        /// <returns></returns>
        bool RemoveUserFromRole(Guid userId, Guid roleId);

        /// <summary>
        /// Verifies whether a user is part of a given role
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="roleId">The role identifier</param>
        /// <returns></returns>
        bool IsInRole(Guid userId, Guid roleId);

        /// <summary>
        /// Retrieves all the Roles a User possesses.
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <returns></returns>
        IList<Role> GetUserRoles(Guid userId);

        /// <summary>
        /// Validate the user (update or insert)
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserValidationResult ValidateUser(User user);

        /// <summary>
        /// Reset the access failed count for the user
        /// </summary>
        /// <param name="userId"></param>
        void ResetAccessFailedCount(Guid userId);

        /// <summary>
        /// Add a remote login for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="loginKey"></param>
        void AddRemoteLogin(Guid userId, string loginProvider, string loginKey);

        /// <summary>
        /// Remove a remote login from a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="loginProvider"></param>
        /// <param name="loginKey"></param>
        void RemoveRemoteLogin(Guid userId, string loginProvider, string loginKey);

        /// <summary>
        /// Get logins for user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<UserLogin> GetRemoteLoginsForUser(Guid userId);

        /// <summary>
        /// Fiond a user by an external login
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="loginKey"></param>
        /// <returns></returns>
        User FindUserByExternalLogin(string loginProvider, string loginKey);

        /// <summary>
        /// Updates just profile data about a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fullName"></param>
        /// <param name="bio"></param>
        /// <param name="url"></param>
        /// <param name="location"></param>
        void UpdateUserProfile(Guid userId, string fullName, string bio, string url, string location);

        /// <summary>
        /// Updates the user's avatar identifier
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="avatarIdentifier">The avatar identifier.</param>
        void UpdateUserAvatar(Guid userId, string avatarIdentifier);
    }

    public enum UserValidationResult
    {
        Success = 0,
        UnknownError = 1,
        InvalidUserName = 1 << 1,
        UserNameInUse = 1 << 2,
        CantChangeUsername = 1 << 3,
        InvalidEmail = 1 << 4,
        EmailInUse = 1 << 5
    }
}
