﻿using System;
using ServiceStack.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Skimur.Data.Models
{
    /// <summary>
    /// Represents a user
    /// </summary>
    [Alias("Users")]
    public class User
    {
        //// <summary>
        /// User ID (Primary Key)
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// The date the user was created
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public virtual string Email { get; set; }

        /// <summary>
        /// True if the email is confirmed, default is false
        /// </summary>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// The salted/hashed form of the user password
        /// </summary>
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// A random value that should change whenever a users credentials have changed (password changed, login removed)
        /// </summary>
        public virtual string SecurityStamp { get; set; }

        /// <summary>
        /// PhoneNumber for the user
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// True if the phone number is confirmed, default is false
        /// </summary>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Is two factor enabled for the user
        /// </summary>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// DateTime in UTC when lockout ends, any time in the past is considered not locked out.
        /// </summary>
        public virtual DateTime? LockoutEndDate { get; set; }

        /// <summary>
        /// Is lockout enabled for this user
        /// </summary>
        public virtual bool LockoutEnabled { get; set; }

        /// <summary>
        /// Used to record failures for the purposes of lockout
        /// </summary>
        public virtual int AccessFailedCount { get; set; }

        /// <summary>
        /// Security question for resetting password
        /// </summary>
        public virtual string SecurityQuestion { get; set; }

        /// <summary>
        /// Security answer for resetting password
        /// </summary>
        public virtual string SecurityAnswer { get; set; }

        /// <summary>
        /// The full name of the user
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// A short bio for the user
        /// </summary>
        public string Bio { get; set; }

        /// <summary>
        /// A url for the user. Maybe a blog?
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Where the user is located.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or set the avatar to use for the user
        /// </summary>
        public string AvatarIdentifier { get; set; }

        /// <summary>
        /// Is this user an administrator?
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// Is this user a system user?
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Does the user want to see NSFW content?
        /// </summary>
        public bool ShowNsfw { get; set; }

        /// <summary>
        /// Does the user want to be presented with any custom styles?
        /// </summary>
        [Alias("Styles")]
        public bool EnableStyles { get; set; }

        /// <summary>
        /// The registration ip that the user signed up with
        /// </summary>
        public string RegistrationIp { get; set; }

        /// <summary>
        /// The current ip that the user is logged in with
        /// </summary>
        public string CurrentIp { get; set; }

        /// <summary>
        /// The date the user last logged in
        /// </summary>
        public DateTime LastLogin { get; set; }

    }
}
