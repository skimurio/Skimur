using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("SubUserBans")]
    public class SubUserBan
    {
        /// <summary>
        /// The id of the ban
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The id of the sub
        /// </summary>
        public Guid SubId { get; set; }


        /// <summary>
        /// The id of the user this ban applies to
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The reason for the ban
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// The moderator that banned the user
        /// </summary>
        public Guid BannedBy { get; set; }

        /// <summary>
        /// Indicates if this ban is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The time this ban expires if it is a temporary ban
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// The date and time that this ban was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
