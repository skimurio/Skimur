using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Moderators")]
    public class Moderator
    {
        /// <summary>
        /// The id of this record
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// the sub id
        /// </summary>
        public Guid SubId { get; set; }

        /// <summary>
        /// the user that added this moderator
        /// </summary>
        public Guid? AddedBy { get; set; }

        /// <summary>
        /// the date this moderator was added on
        /// </summary>
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// the permissions this moderator has
        /// </summary>
        public ModeratorPermissions Permissions { get; set; }
    }
}
