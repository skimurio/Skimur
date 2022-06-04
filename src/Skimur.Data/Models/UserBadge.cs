using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("UserBadges")]
    public class UserBadge
    {
        /// <summary>
        /// UserId for the user that has the badge
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// BadgeId for the badge
        /// </summary>
        public Guid BadgeId { get; set; }
    }
}
