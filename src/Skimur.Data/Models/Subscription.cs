using System;
using ServiceStack.DataAnnotations;

namespace Skimur.Data.Models
{
    [Alias("Subscriptions")]
    public class Subscription
    {
        /// <summary>
        /// The identifier for this subscription
        /// </summary>
        public virtual Guid Id { get; set; }

        /// <summary>
        /// The user for the subscription
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// The guild the user is subscribed to
        /// </summary>
        public Guid SubId { get; set; }

        /// <summary>
        /// Determines if this subscription is active, true by default
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
