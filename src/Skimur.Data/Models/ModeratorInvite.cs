using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("ModeratorInvites")]
    public class ModeratorInvite
    {
        /// <summary>
        /// The id of this invite
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// The id of this user
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The id of the sub for the invite
        /// </summary>
        public Guid SubId { get; set; }

        /// <summary>
        /// The mod that invited this user
        /// </summary>
        public Guid? InvitedBy { get; set; }

        /// <summary>
        /// The date the user was invited
        /// </summary>
        public DateTime InvitedOn { get; set; }

        /// <summary>
        /// Permission that were granted to the user
        /// </summary>
        public ModeratorPermissions Permissions { get; set; }
    }
}
